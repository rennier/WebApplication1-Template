using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedVerifiedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnverifiedBy",
                table: "Zproducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UnverifiedDate",
                table: "Zproducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifiedBy",
                table: "Zproducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedDate",
                table: "Zproducts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnverifiedBy",
                table: "Zproducts");

            migrationBuilder.DropColumn(
                name: "UnverifiedDate",
                table: "Zproducts");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "Zproducts");

            migrationBuilder.DropColumn(
                name: "VerifiedDate",
                table: "Zproducts");
        }
    }
}
