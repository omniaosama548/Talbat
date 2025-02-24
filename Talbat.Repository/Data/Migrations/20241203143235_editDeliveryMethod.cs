using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talbat.Repository.Data.Migrations
{
    public partial class editDeliveryMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortTime",
                table: "DeliveryMethods",
                newName: "ShortName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "DeliveryMethods",
                newName: "ShortTime");
        }
    }
}
