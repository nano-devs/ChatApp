using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Api.Migrations
{
    public partial class UpdateMessageRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Groups_SendToGroupId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_Users_UserId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_PrivateMessages_MessageId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_PrivateMessages_UserId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SendToGroupId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SendToUser",
                table: "PrivateMessages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PrivateMessages");

            migrationBuilder.DropColumn(
                name: "SendToGroupId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PrivateMessages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "GroupMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MessageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMessage_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessage_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_MessageId",
                table: "PrivateMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_SendToUserId",
                table: "PrivateMessages",
                column: "SendToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_GroupId",
                table: "GroupMessage",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_MessageId",
                table: "GroupMessage",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_Users_SendToUserId",
                table: "PrivateMessages",
                column: "SendToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_Users_SendToUserId",
                table: "PrivateMessages");

            migrationBuilder.DropTable(
                name: "GroupMessage");

            migrationBuilder.DropIndex(
                name: "IX_PrivateMessages_MessageId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_PrivateMessages_SendToUserId",
                table: "PrivateMessages");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "PrivateMessages",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "SendToUser",
                table: "PrivateMessages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PrivateMessages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SendToGroupId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_MessageId",
                table: "PrivateMessages",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_UserId",
                table: "PrivateMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendToGroupId",
                table: "Messages",
                column: "SendToGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Groups_SendToGroupId",
                table: "Messages",
                column: "SendToGroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_Users_UserId",
                table: "PrivateMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
