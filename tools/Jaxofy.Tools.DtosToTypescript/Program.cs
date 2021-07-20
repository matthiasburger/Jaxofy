using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Jaxofy.Attributes;
using Jaxofy.Tools.DtosToTypescript.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jaxofy.Tools.DtosToTypescript
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;

        private static async Task MainAsync(string[] args)
        {
            ServiceCollection serviceCollection = new();
            _configureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            Program program = serviceProvider?.GetService<Program>();
            if (program is not null)
                await program.Run();
        }

        private async Task Run()
        {
            Assembly assembly = typeof(DtoAttribute).Assembly;
            Settings config = _configuration.Get<Settings>();

            IEnumerable<Type> _ = (from type in assembly.GetExportedTypes()//.AsParallel()
                where type.GetCustomAttribute<DtoAttribute>() is not null
                let analyzedType = TypeAnalyzer.Create(type)
                    .Analyze()
                select analyzedType
                    .WriteToPath(config.Export.Directory)).ToList();
        }

        private static int Main(string[] args)
        {
            try
            {
                MainAsync(args).Wait();
                return 0;
            }
            catch(Exception e)
            {
                throw;
                return 1;
            }
        }

        private static void _configureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging();

            // Build configuration
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton(_configuration);
            serviceCollection.AddTransient<Program>();
        }
    }
}