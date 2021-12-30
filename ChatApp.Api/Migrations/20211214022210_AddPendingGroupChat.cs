using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.API.Migrations;

public partial class AddPendingGroupChat : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "AlreadySendTo",
			table: "GroupChats");

		migrationBuilder.CreateTable(
			name: "PendingGroupChats",
			columns: table => new
			{
				GroupChatId = table.Column<Guid>(type: "TEXT", nullable: false),
				UserId = table.Column<Guid>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_PendingGroupChats", x => new { x.GroupChatId, x.UserId });
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "PendingGroupChats");

		migrationBuilder.AddColumn<string>(
			name: "AlreadySendTo",
			table: "GroupChats",
			type: "TEXT",
			nullable: true);
	}
}
