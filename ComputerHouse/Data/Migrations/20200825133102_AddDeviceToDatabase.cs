using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerHouse.Data.Migrations
{
    public partial class AddDeviceToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    HDCapacity = table.Column<int>(nullable: false),
                    RAMCapacity = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Bluetooth = table.Column<bool>(nullable: false),
                    ScreenSize = table.Column<double>(nullable: false),
                    Image = table.Column<byte>(nullable: false),
                    HDType = table.Column<int>(nullable: false),
                    HDMI = table.Column<bool>(nullable: false),
                    IsTouchScreen = table.Column<bool>(nullable: false),
                    IsNew = table.Column<bool>(nullable: false),
                    OSId = table.Column<int>(nullable: false),
                    BrandCategoryId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_BrandCategories_BrandCategoryId",
                        column: x => x.BrandCategoryId,
                        principalTable: "BrandCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Devices_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Devices_OperatingSystems_OSId",
                        column: x => x.OSId,
                        principalTable: "OperatingSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_BrandCategoryId",
                table: "Devices",
                column: "BrandCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_BrandId",
                table: "Devices",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_OSId",
                table: "Devices",
                column: "OSId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
