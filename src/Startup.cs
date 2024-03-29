using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using AutoMapper.Extensions.ExpressionMapping;

using Jaxofy.Data;
using Jaxofy.Data.Repositories;
using Jaxofy.Models.Dto;
using Jaxofy.Models.Settings;
using Jaxofy.Services.AuthTokenService;
using Jaxofy.Services.Environment;
using Jaxofy.Services.HttpEncoder;
using Jaxofy.Services.Login;
using Jaxofy.Services.PasswordHashing;
using Jaxofy.Services.TrackService;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Serilog;

namespace Jaxofy
{
    /// <summary>
    /// Service injection hosted service registrations happens in this class (created inside <see cref="Program"/>).
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IEnvironmentDiscovery _environmentDiscovery;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _environmentDiscovery = new EnvironmentDiscovery();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMemoryCache();
            services.AddCors(options =>
            {
                options.AddPolicy("cors",
                    builder => builder
                        .WithOrigins(
                            "http://localhost:4200",
                            "http://127.0.0.1:4200",
                            "http://localhost:8100",
                            "http://127.0.0.1:8100",
                            "http://jaxofy.fam-burger.de"
                            ) // TODO: once deployed, add the production server's domain name to this list. 
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                );
            });

            services.AddSwaggerGen();

            services.Configure<IpRateLimitOptions>(_configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(_configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddScoped<ITrackService, TrackService>();
            services.AddScoped<IHttpEncoder, HttpEncoder>();

            services.Configure<JwtSettings>(_configuration.GetSection(JwtSettings.SectionName));
            services.Configure<Argon2HashingParameters>(_configuration.GetSection(Argon2HashingParameters.SectionName));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    using RSA rsa = RSA.Create();
                    JwtSettings jwtSettings = _configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
                    rsa.ImportFromPem(jwtSettings.RSAPublicKeyPEM);

                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(rsa.ExportParameters(false)),
                        ValidateIssuer = jwtSettings.ValidateIssuer,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = jwtSettings.ValidateAudience,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        NameClaimType = ClaimTypes.NameIdentifier,
                        ClockSkew = TimeSpan.FromSeconds(4)
                    };
                });

            services.AddAuthorization();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_configuration["ApplicationConnectionString"]));

            services.AddResponseCompression(options =>
                options.Providers.Add<BrotliCompressionProvider>());

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                )
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory =
                        context => new BadRequestObjectResult(new ResponseBodyDto(context));
                });

            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<ITrackRepository, TrackRepository>();
            services.AddTransient<IMigrationRepository, MigrationRepository<ApplicationDbContext>>();

            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IAuthTokenService, AuthTokenService>();
            services.AddTransient<IPasswordHashing, PasswordHashingArgon2>();
            services.AddTransient<IEnvironmentDiscovery, EnvironmentDiscovery>();
            services.AddTransient<IDataSeeder, DataSeeder>();

            services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); }, AppDomain.CurrentDomain.GetAssemblies());

            // In production, the Angular files will be served from this directory.
            services.AddSpaStaticFiles(config =>
                config.RootPath = "ClientApp/dist");

            // services.AddHostedService<TestService>();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext db, IDataSeeder seeder)
        {
            app.UseIpRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            loggerFactory.AddSerilog();

            app.UseResponseCompression();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    if (ctx.Context?.Request.Path.Value?.StartsWith("/docs/") is true && !env.IsDevelopment())
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
            });

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseCors("cors");
            app.UseAuthentication();
            app.UseAuthorization();
            
            if (_environmentDiscovery.IsDocker)
            {
                db.Database.Migrate();
            }

            if (_environmentDiscovery.IsTestEnvironment || _environmentDiscovery.IsDocker)
            {
                try
                {
                    Task.Run(seeder.SeedData);
                }
                catch (Exception e)
                {
                    throw;
                    // Ignored. 
                }
            }

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}