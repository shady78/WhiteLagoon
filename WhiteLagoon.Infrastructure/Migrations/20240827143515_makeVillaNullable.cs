using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class makeVillaNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 8, 27, 17, 35, 14, 670, DateTimeKind.Local).AddTicks(6535));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 8, 27, 17, 35, 14, 670, DateTimeKind.Local).AddTicks(6583));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 8, 27, 17, 35, 14, 670, DateTimeKind.Local).AddTicks(6586));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 8, 27, 16, 18, 37, 129, DateTimeKind.Local).AddTicks(2017));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 8, 27, 16, 18, 37, 129, DateTimeKind.Local).AddTicks(2068));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 8, 27, 16, 18, 37, 129, DateTimeKind.Local).AddTicks(2071));
        }
    }
}
