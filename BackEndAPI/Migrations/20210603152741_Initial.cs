using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateEditDate",
                value: new DateTime(2021, 6, 2, 22, 27, 40, 904, DateTimeKind.Local).AddTicks(3810));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateEditDate",
                value: new DateTime(2021, 6, 1, 22, 27, 40, 905, DateTimeKind.Local).AddTicks(6249));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateEditDate",
                value: new DateTime(2021, 5, 31, 22, 27, 40, 905, DateTimeKind.Local).AddTicks(6277));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateEditDate",
                value: new DateTime(2021, 5, 30, 22, 27, 40, 905, DateTimeKind.Local).AddTicks(6280));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "11e11d0a-cd84-4f45-a7cc-6aa6d4160e1c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "4d56cf48-b2f2-4591-bdfb-28e5d380d156");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a333ad94-2f2d-43c3-92a7-af03208413f3");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "177897aa-4deb-4b1d-8d30-c026fa73ab47");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "078e2a30-c685-492e-add1-a89da4be0393");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "dc721c23-d80f-4d4b-99b6-1277f49ff3db");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateEditDate",
                value: new DateTime(2021, 6, 2, 17, 40, 39, 883, DateTimeKind.Local).AddTicks(7485));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateEditDate",
                value: new DateTime(2021, 6, 1, 17, 40, 39, 884, DateTimeKind.Local).AddTicks(5421));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateEditDate",
                value: new DateTime(2021, 5, 31, 17, 40, 39, 884, DateTimeKind.Local).AddTicks(5436));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateEditDate",
                value: new DateTime(2021, 5, 30, 17, 40, 39, 884, DateTimeKind.Local).AddTicks(5439));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f99d8020-dc6f-4ff6-ac58-9964a1f4005f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "37656048-00a9-4abc-af86-d2605266a4de");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "ad6557db-e475-41df-b9df-2b80fdf1c0d3");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "ed3f5369-116f-4745-b484-8c7d5c0bea84");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "c385c7d4-402a-44aa-ae60-5df850e6ced6");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "750cea8a-9a64-4610-8a69-c25407fbc495");
        }
    }
}
