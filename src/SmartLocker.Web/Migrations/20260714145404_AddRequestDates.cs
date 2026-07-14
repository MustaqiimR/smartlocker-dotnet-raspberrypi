using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLocker.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedEndDate",
                table: "Requests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedStartDate",
                table: "Requests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestedEndDate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestedStartDate",
                table: "Requests");
        }
    }
}
