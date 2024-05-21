using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mc2.CrudTest.Presentation.Server.Migrations
{
    /// <inheritdoc />
    public partial class correct_inheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Events",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "BankAccount",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCreatedEvent_BankAccount",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCreatedEvent_Email",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCreatedEvent_FirstName",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCreatedEvent_LastName",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCreatedEvent_PhoneNumber",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Events",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Events",
                type: "uuid",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CustomerCreatedEvent_BankAccount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CustomerCreatedEvent_Email",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CustomerCreatedEvent_FirstName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CustomerCreatedEvent_LastName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CustomerCreatedEvent_PhoneNumber",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
