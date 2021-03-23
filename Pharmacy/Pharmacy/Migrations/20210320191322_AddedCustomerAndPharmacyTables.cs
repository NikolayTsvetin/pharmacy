using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Migrations
{
    public partial class AddedCustomerAndPharmacyTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PharmacyId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PharmacyId",
                table: "Products",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_PharmacyId",
                table: "Products",
                column: "PharmacyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_PharmacyId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PharmacyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
