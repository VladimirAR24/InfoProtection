using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoProtection.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncryptedMessages_Users_UserId",
                table: "EncryptedMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_NfaCodes_Users_UserId",
                table: "NfaCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "EncryptedMessageText",
                table: "EncryptedMessages",
                newName: "OriginalText");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "EncryptedMessages",
                newName: "EncryptionDate");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedText",
                table: "EncryptedMessages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "public",
                table: "users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EncryptedMessages_users_UserId",
                table: "EncryptedMessages",
                column: "UserId",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NfaCodes_users_UserId",
                table: "NfaCodes",
                column: "UserId",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncryptedMessages_users_UserId",
                table: "EncryptedMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_NfaCodes_users_UserId",
                table: "NfaCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "public",
                table: "users");

            migrationBuilder.DropColumn(
                name: "EncryptedText",
                table: "EncryptedMessages");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "public",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "OriginalText",
                table: "EncryptedMessages",
                newName: "EncryptedMessageText");

            migrationBuilder.RenameColumn(
                name: "EncryptionDate",
                table: "EncryptedMessages",
                newName: "CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EncryptedMessages_Users_UserId",
                table: "EncryptedMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NfaCodes_Users_UserId",
                table: "NfaCodes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
