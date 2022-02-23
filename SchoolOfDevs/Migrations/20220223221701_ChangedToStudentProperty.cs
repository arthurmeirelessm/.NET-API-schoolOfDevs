using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolOfDevs.Migrations
{
    public partial class ChangedToStudentProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_UserId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notes",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                newName: "IX_Notes_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_StudentId",
                table: "Notes",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_StudentId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Notes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_StudentId",
                table: "Notes",
                newName: "IX_Notes_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
