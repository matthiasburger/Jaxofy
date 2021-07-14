﻿// <auto-generated />
using System;
using DasTeamRevolution.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DasTeamRevolution.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210324143025_ApplicationUserTable")]
    partial class ApplicationUserTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DasTeamRevolution.Models.ApplicationUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ChangePassword")
                        .HasColumnType("bit")
                        .HasColumnName("ChangePassword");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2(0)")
                        .HasColumnName("CreatedOn");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("FirstName");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("IsActive");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit")
                        .HasColumnName("IsAdmin");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("LastName");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Password");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");
                });
#pragma warning restore 612, 618
        }
    }
}
