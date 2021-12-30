using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.API.Migrations;

public partial class Initial : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Contacts",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "TEXT", nullable: false),
				UserId = table.Column<Guid>(type: "TEXT", nullable: false),
				FriendId = table.Column<Guid>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Contacts", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "GroupChats",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "TEXT", nullable: false),
				From = table.Column<Guid>(type: "TEXT", nullable: false),
				GroupId = table.Column<Guid>(type: "TEXT", nullable: false),
				Message = table.Column<string>(type: "TEXT", nullable: true),
				Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
				AlreadySendTo = table.Column<string>(type: "TEXT", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_GroupChats", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "Groups",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "TEXT", nullable: false),
				Name = table.Column<string>(type: "TEXT", nullable: true),
				MemberIds = table.Column<string>(type: "TEXT", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Groups", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "PrivateChats",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "TEXT", nullable: false),
				From = table.Column<Guid>(type: "TEXT", nullable: false),
				To = table.Column<Guid>(type: "TEXT", nullable: false),
				Message = table.Column<string>(type: "TEXT", nullable: true),
				Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_PrivateChats", x => x.Id);
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Contacts");

		migrationBuilder.DropTable(
			name: "GroupChats");

		migrationBuilder.DropTable(
			name: "Groups");

		migrationBuilder.DropTable(
			name: "PrivateChats");
	}
}
