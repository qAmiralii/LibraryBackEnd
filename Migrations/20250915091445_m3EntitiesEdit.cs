using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_2.Migrations
{
    /// <inheritdoc />
    public partial class m3EntitiesEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Members",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "Members",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Members",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Members",
                newName: "Firstname");
        }
    }
}
