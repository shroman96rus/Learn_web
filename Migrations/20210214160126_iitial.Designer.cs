﻿// <auto-generated />
using System;
using Learn_web.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Learn_web.Migrations
{
    [DbContext(typeof(OrdersContext))]
    [Migration("20210214160126_iitial")]
    partial class iitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Learn_web.Models.Order", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Translator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("clientData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("costOfTranslationServices")
                        .HasColumnType("double(18)");

                    b.Property<double>("costOfWork")
                        .HasColumnType("double(18)");

                    b.Property<DateTime>("dateOrder")
                        .HasColumnType("datetime2");

                    b.Property<string>("originalLanguage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("translateLanguage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Learn_web.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("nameUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
