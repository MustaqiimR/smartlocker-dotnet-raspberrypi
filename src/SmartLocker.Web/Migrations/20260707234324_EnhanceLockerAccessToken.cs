using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLocker.Web.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceLockerAccessToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "LockerAccessTokens",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailedAttemptCount",
                table: "LockerAccessTokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "LockerAccessTokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "LockerAccessTokens",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "LockerAccessTokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LockerAccessTokens_CreatedByUserId",
                table: "LockerAccessTokens",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LockerAccessTokens_ItemId",
                table: "LockerAccessTokens",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LockerAccessTokens_UserId",
                table: "LockerAccessTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LockerAccessTokens_Items_ItemId",
                table: "LockerAccessTokens",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LockerAccessTokens_Users_CreatedByUserId",
                table: "LockerAccessTokens",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LockerAccessTokens_Users_UserId",
                table: "LockerAccessTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockerAccessTokens_Items_ItemId",
                table: "LockerAccessTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_LockerAccessTokens_Users_CreatedByUserId",
                table: "LockerAccessTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_LockerAccessTokens_Users_UserId",
                table: "LockerAccessTokens");

            migrationBuilder.DropIndex(
                name: "IX_LockerAccessTokens_CreatedByUserId",
                table: "LockerAccessTokens");

            migrationBuilder.DropIndex(
                name: "IX_LockerAccessTokens_ItemId",
                table: "LockerAccessTokens");

            migrationBuilder.DropIndex(
                name: "IX_LockerAccessTokens_UserId",
                table: "LockerAccessTokens");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "LockerAccessTokens");

            migrationBuilder.DropColumn(
                name: "FailedAttemptCount",
                table: "LockerAccessTokens");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "LockerAccessTokens");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "LockerAccessTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LockerAccessTokens");
        }
    }
}
