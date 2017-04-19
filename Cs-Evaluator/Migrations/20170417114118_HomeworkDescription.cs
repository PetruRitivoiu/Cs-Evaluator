using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CsEvaluator.Migrations
{
    public partial class HomeworkDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HomeworkDescriptionID",
                table: "Homeworks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HomeworkDescriptions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    fullDescription = table.Column<string>(nullable: true),
                    shortDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkDescriptions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_HomeworkDescriptionID",
                table: "Homeworks",
                column: "HomeworkDescriptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_HomeworkDescriptions_HomeworkDescriptionID",
                table: "Homeworks",
                column: "HomeworkDescriptionID",
                principalTable: "HomeworkDescriptions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_HomeworkDescriptions_HomeworkDescriptionID",
                table: "Homeworks");

            migrationBuilder.DropTable(
                name: "HomeworkDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_HomeworkDescriptionID",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "HomeworkDescriptionID",
                table: "Homeworks");
        }
    }
}
