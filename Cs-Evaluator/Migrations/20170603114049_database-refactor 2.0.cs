using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CsEvaluator.Migrations
{
    public partial class databaserefactor20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Subjects_SubjectID",
                table: "Homeworks");

            migrationBuilder.DropTable(
                name: "StudentHomeworks");

            migrationBuilder.RenameColumn(
                name: "SubjectID",
                table: "Homeworks",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Homeworks_SubjectID",
                table: "Homeworks",
                newName: "IX_Homeworks_StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Students_StudentID",
                table: "Homeworks",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Students_StudentID",
                table: "Homeworks");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Homeworks",
                newName: "SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Homeworks_StudentID",
                table: "Homeworks",
                newName: "IX_Homeworks_SubjectID");

            migrationBuilder.CreateTable(
                name: "StudentHomeworks",
                columns: table => new
                {
                    StudentID = table.Column<int>(nullable: false),
                    HomeworkID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentHomeworks", x => new { x.StudentID, x.HomeworkID });
                    table.ForeignKey(
                        name: "FK_StudentHomeworks_Homeworks_HomeworkID",
                        column: x => x.HomeworkID,
                        principalTable: "Homeworks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentHomeworks_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentHomeworks_HomeworkID",
                table: "StudentHomeworks",
                column: "HomeworkID");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Subjects_SubjectID",
                table: "Homeworks",
                column: "SubjectID",
                principalTable: "Subjects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
