using Microsoft.EntityFrameworkCore.Migrations;

namespace rswebfaks.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Teacherid",
                table: "Course",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_Teacherid",
                table: "Course",
                column: "Teacherid");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher_Teacherid",
                table: "Course",
                column: "Teacherid",
                principalTable: "Teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher_Teacherid",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_Teacherid",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Teacherid",
                table: "Course");
        }
    }
}
