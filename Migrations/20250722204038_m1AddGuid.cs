using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_2.Migrations
{
    /// <inheritdoc />
    public partial class m1AddGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Borrows",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Books");
        }
    }
}
