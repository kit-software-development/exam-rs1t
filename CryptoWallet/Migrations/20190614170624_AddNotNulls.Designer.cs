﻿// <auto-generated />
using System;
using CryptoWallet.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CryptoWallet.Migrations
{
    [DbContext(typeof(CryptoWalletDbContext))]
    [Migration("20190614170624_AddNotNulls")]
    partial class AddNotNulls
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CryptoWallet.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CryptoWallet.Database.Entities.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("UserId");

                    b.Property<string>("Wif")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("Wif")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("CryptoWallet.Database.Entities.Wallet", b =>
                {
                    b.HasOne("CryptoWallet.Database.Entities.User", "User")
                        .WithMany("Wallets")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}