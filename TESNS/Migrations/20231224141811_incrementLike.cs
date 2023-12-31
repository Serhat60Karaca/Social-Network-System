﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TESNS.Migrations
{
    public partial class incrementLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<char>(
                name: "ContentType",
                table: "Likes",
                type: "character(1)",
                nullable: true,
                oldClrType: typeof(char),
                oldType: "character(1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<char>(
                name: "ContentType",
                table: "Likes",
                type: "character(1)",
                nullable: false,
                defaultValue: ' ',
                oldClrType: typeof(char),
                oldType: "character(1)",
                oldNullable: true);
        }
    }
}
