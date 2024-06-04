using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mc2.CrudTest.Presentation.Server.Migrations
{
    /// <inheritdoc />
    public partial class removealltheeventfieldsexcepttheDatafield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccount",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Events",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Events",
                type: "text",
                nullable: true);
        }
    }
}
