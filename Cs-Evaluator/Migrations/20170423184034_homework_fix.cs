using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CsEvaluator.Migrations
{
    public partial class homework_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EvaluationResult",
                table: "Homeworks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "WasEvaluated",
                table: "Homeworks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvaluationResult",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "WasEvaluated",
                table: "Homeworks");
        }
    }
}
