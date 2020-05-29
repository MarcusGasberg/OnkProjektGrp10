using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockMarketService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seller_Stocks_StockId",
                table: "Seller");

            migrationBuilder.DropForeignKey(
                name: "FK_StockPrice_Stocks_stockId",
                table: "StockPrice");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stocks",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "stockId",
                table: "StockPrice",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "StockPrice",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "StockPrice",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "StockPrice",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "StockId",
                table: "Seller",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "SellingAmount",
                table: "Seller",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "Seller",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Seller",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Seller_Stocks_StockId",
                table: "Seller",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockPrice_Stocks_stockId",
                table: "StockPrice",
                column: "stockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seller_Stocks_StockId",
                table: "Seller");

            migrationBuilder.DropForeignKey(
                name: "FK_StockPrice_Stocks_stockId",
                table: "StockPrice");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stocks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Stocks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "stockId",
                table: "StockPrice",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdateTime",
                table: "StockPrice",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "StockPrice",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "StockPrice",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "StockId",
                table: "Seller",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SellingAmount",
                table: "Seller",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "Seller",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Seller",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Seller_Stocks_StockId",
                table: "Seller",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockPrice_Stocks_stockId",
                table: "StockPrice",
                column: "stockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
