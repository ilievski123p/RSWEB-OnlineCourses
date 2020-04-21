using Microsoft.EntityFrameworkCore.Migrations;

namespace rswebfaks.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher_Teacherid",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher_Teacherid1",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_Teacherid",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_Teacherid1",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Teacherid",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Teacherid1",
                table: "Course");

            migrationBuilder.CreateIndex(
                name: "IX_Course_FirstTeacherId",
                table: "Course",
                column: "FirstTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SecondTeacherId",
                table: "Course",
                column: "SecondTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher_FirstTeacherId",
                table: "Course",
                column: "FirstTeacherId",
                principalTable: "Teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher_SecondTeacherId",
                table: "Course",
                column: "SecondTeacherId",
                principalTable: "Teacher",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher_FirstTeacherId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher_SecondTeacherId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_FirstTeacherId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_SecondTeacherId",
                table: "Course");

            migrationBuilder.AddColumn<int>(
                name: "Teacherid",
                table: "Course",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Teacherid1",
                table: "Course",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_Teacherid",
                table: "Course",
                column: "Teacherid");

            migrationBuilder.CreateIndex(
                name: "IX_Course_Teacherid1",
                table: "Course",
                column: "Teacherid1");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher_Teacherid",
                table: "Course",
                column: "Teacherid",
                principalTable: "Teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher_Teacherid1",
                table: "Course",
                column: "Teacherid1",
                principalTable: "Teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
