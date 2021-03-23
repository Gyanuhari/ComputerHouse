using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerHouse.Data.Migrations
{
    public partial class SomeChangesInDeviceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_BrandCategories_BrandCategoryId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Brands_BrandId",
                table: "Devices");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Devices",
                nullable: true,
                oldClrType: typeof(byte));

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_BrandCategories_BrandCategoryId",
                table: "Devices",
                column: "BrandCategoryId",
                principalTable: "BrandCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Brands_BrandId",
                table: "Devices",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_BrandCategories_BrandCategoryId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Brands_BrandId",
                table: "Devices");

            migrationBuilder.AlterColumn<byte>(
                name: "Image",
                table: "Devices",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_BrandCategories_BrandCategoryId",
                table: "Devices",
                column: "BrandCategoryId",
                principalTable: "BrandCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Brands_BrandId",
                table: "Devices",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
