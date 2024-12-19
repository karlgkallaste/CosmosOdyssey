﻿// <auto-generated />
using System;
using CosmosOdyssey.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CosmosOdyssey.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241218181300_Initial3")]
    partial class Initial3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CosmosOdyssey.Domain.Features.PriceLists.PriceList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Legs")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<DateTime>("ValidUntil")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("PriceLists");
                });
#pragma warning restore 612, 618
        }
    }
}
