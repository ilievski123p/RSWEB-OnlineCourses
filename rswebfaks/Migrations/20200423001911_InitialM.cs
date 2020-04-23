using Microsoft.EntityFrameworkCore.Migrations;

namespace rswebfaks.Migrations
{
    public partial class InitialM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_Courseid",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Student_Studentid",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                name: "CurrentSemester",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Teacher",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Studentid",
                table: "Enrollment",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "Courseid",
                table: "Enrollment",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_Studentid",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_Courseid",
                table: "Enrollment",
                newName: "IX_Enrollment_CourseId");

            migrationBuilder.AddColumn<int>(
                name: "CurrentSemestar",
                table: "Student",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectPoints",
                table: "Enrollment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_CourseId",
                table: "Enrollment",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student_StudentId",
                table: "Enrollment",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_CourseId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Student_StudentId",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                name: "CurrentSemestar",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ProjectPoints",
                table: "Enrollment");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Teacher",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Enrollment",
                newName: "Studentid");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Enrollment",
                newName: "Courseid");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentId",
                table: "Enrollment",
                newName: "IX_Enrollment_Studentid");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                newName: "IX_Enrollment_Courseid");

            migrationBuilder.AddColumn<int>(
                name: "CurrentSemester",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_Courseid",
                table: "Enrollment",
                column: "Courseid",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student_Studentid",
                table: "Enrollment",
                column: "Studentid",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
