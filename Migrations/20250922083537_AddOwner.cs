using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_2.Migrations
{
    /// <inheritdoc />
    public partial class AddOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_OwnerId",
                table: "Books",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Admins_OwnerId",
                table: "Books",
                column: "OwnerId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Admins_OwnerId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_OwnerId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Books");
        }
    }
}
