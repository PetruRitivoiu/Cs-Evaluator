using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CsEvaluator.Migrations
{
    public partial class HomeworkDescription2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectID",
                table: "HomeworkDescriptions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkDescriptions_SubjectID",
                table: "HomeworkDescriptions",
                column: "SubjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkDescriptions_Subjects_SubjectID",
                table: "HomeworkDescriptions",
                column: "SubjectID",
                principalTable: "Subjects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkDescriptions_Subjects_SubjectID",
                table: "HomeworkDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_HomeworkDescriptions_SubjectID",
                table: "HomeworkDescriptions");

            migrationBuilder.DropColumn(
                name: "SubjectID",
                table: "HomeworkDescriptions");
        }
    }
}
