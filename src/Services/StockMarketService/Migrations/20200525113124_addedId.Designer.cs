﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockMarketService;

namespace StockMarketService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200525113124_addedId")]
    partial class addedId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Stock", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockMarketService.Models.Seller", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("SellerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SellingAmount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StockId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("Seller");
                });

            modelBuilder.Entity("StockMarketService.Models.StockPrice", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("stockId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("stockId");

                    b.ToTable("StockPrice");
                });

            modelBuilder.Entity("StockMarketService.Models.Seller", b =>
                {
                    b.HasOne("Stock", "Stock")
                        .WithMany("Seller")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StockMarketService.Models.StockPrice", b =>
                {
                    b.HasOne("Stock", "stock")
                        .WithMany("HistoricPrice")
                        .HasForeignKey("stockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}