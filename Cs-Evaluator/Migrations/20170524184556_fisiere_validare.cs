using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CsEvaluator.Migrations
{
    public partial class fisiere_validare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "expectedFile",
                table: "HomeworkDescriptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "initialFile",
                table: "HomeworkDescriptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expectedFile",
                table: "HomeworkDescriptions");

            migrationBuilder.DropColumn(
                name: "initialFile",
                table: "HomeworkDescriptions");
        }
    }
}
