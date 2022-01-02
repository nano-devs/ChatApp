namespace ChatApp.Api.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class ChangeContactsToFriends : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey(
			name: "PK_Contacts",
			table: "Contacts");

		migrationBuilder.DropColumn(
			name: "Id",
			table: "Contacts");

		migrationBuilder.AddPrimaryKey(
			name: "PK_Contacts",
			table: "Contacts",
			columns: new[] { "UserId", "FriendId" });
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey(
			name: "PK_Contacts",
			table: "Contacts");

		migrationBuilder.AddColumn<Guid>(
			name: "Id",
			table: "Contacts",
			type: "TEXT",
			nullable: false,
			defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

		migrationBuilder.AddPrimaryKey(
			name: "PK_Contacts",
			table: "Contacts",
			column: "Id");
	}
}
