using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLocker.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorrowLogs",
                columns: table => new
                {
                    BorrowLogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BorrowId = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ActionDetails = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    PreviousStatus = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NewStatus = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowLogs", x => x.BorrowLogId);
                    table.ForeignKey(
                        name: "FK_BorrowLogs_Borrows_BorrowId",
                        column: x => x.BorrowId,
                        principalTable: "Borrows",
                        principalColumn: "BorrowId");
                    table.ForeignKey(
                        name: "FK_BorrowLogs_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "BorrowStatuses",
                keyColumn: "BorrowStatusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6242));

            migrationBuilder.UpdateData(
                table: "BorrowStatuses",
                keyColumn: "BorrowStatusId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6244));

            migrationBuilder.UpdateData(
                table: "BorrowStatuses",
                keyColumn: "BorrowStatusId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6246));

            migrationBuilder.UpdateData(
                table: "BorrowStatuses",
                keyColumn: "BorrowStatusId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6247));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(5970));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(5980));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(5981));

            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6027));

            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6034));

            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6035));

            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6037));

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Local).AddTicks(6166));

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Local).AddTicks(6171));

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Local).AddTicks(6174));

            migrationBuilder.UpdateData(
                table: "LockerStatuses",
                keyColumn: "LockerStatusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6072));

            migrationBuilder.UpdateData(
                table: "LockerStatuses",
                keyColumn: "LockerStatusId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6084));

            migrationBuilder.UpdateData(
                table: "LockerStatuses",
                keyColumn: "LockerStatusId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6086));

            migrationBuilder.UpdateData(
                table: "LockerStatuses",
                keyColumn: "LockerStatusId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6087));

            migrationBuilder.UpdateData(
                table: "Lockers",
                keyColumn: "LockerId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Local).AddTicks(6126));

            migrationBuilder.UpdateData(
                table: "Lockers",
                keyColumn: "LockerId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Local).AddTicks(6130));

            migrationBuilder.UpdateData(
                table: "Lockers",
                keyColumn: "LockerId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Local).AddTicks(6133));

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6207));

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6213));

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(6214));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(5475));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Utc).AddTicks(5483));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Local).AddTicks(5802));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 21, 55, 30, 926, DateTimeKind.Local).AddTicks(5837));

            migrationBuilder.CreateIndex(
                name: "IX_BorrowLogs_BorrowId",
                table: "BorrowLogs",
                column: "BorrowId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowLogs_ItemId",
                table: "BorrowLogs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowLogs_UserId",
                table: "BorrowLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowLogs");

            migrationBuilder.UpdateData(
                table: "BorrowStatuses",
                keyColumn: "BorrowStatusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5835));

            migrationBuilder.UpdateData(
                table: "BorrowStatuses",
                keyColumn: "BorrowStatusId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5839));

            migrationBuilder.UpdateData(
                table: "BorrowStatuses",
                keyColumn: "BorrowStatusId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5840));

            migrationBuilder.UpdateData(
                table: "BorrowStatuses",
                keyColumn: "BorrowStatusId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5841));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5561));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5570));

            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5604));

            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5608));

            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5610));

            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5611));

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Local).AddTicks(5709));

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Local).AddTicks(5712));

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Local).AddTicks(5715));

            migrationBuilder.UpdateData(
                table: "LockerStatuses",
                keyColumn: "LockerStatusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5641));

            migrationBuilder.UpdateData(
                table: "LockerStatuses",
                keyColumn: "LockerStatusId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5645));

            migrationBuilder.UpdateData(
                table: "LockerStatuses",
                keyColumn: "LockerStatusId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5646));

            migrationBuilder.UpdateData(
                table: "LockerStatuses",
                keyColumn: "LockerStatusId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5647));

            migrationBuilder.UpdateData(
                table: "Lockers",
                keyColumn: "LockerId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Local).AddTicks(5676));

            migrationBuilder.UpdateData(
                table: "Lockers",
                keyColumn: "LockerId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Local).AddTicks(5680));

            migrationBuilder.UpdateData(
                table: "Lockers",
                keyColumn: "LockerId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Local).AddTicks(5683));

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5799));

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5805));

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5806));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5267));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Utc).AddTicks(5273));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Local).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 14, 54, 3, 840, DateTimeKind.Local).AddTicks(5525));
        }
    }
}
