using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerHouse.Data.Migrations
{
    public partial class AddCustomerContactUsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerContactUs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PoneNumber = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    Message = table.Column<string>(maxLength: 255, nullable: false),
                    Picture = table.Column<byte[]>(nullable: true),
                    IsHappyCustomer = table.Column<bool>(nullable: false),
                    Display = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerContactUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerContactUs_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContactUs_ApplicationUserId",
                table: "CustomerContactUs",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerContactUs");
        }
    }
}
