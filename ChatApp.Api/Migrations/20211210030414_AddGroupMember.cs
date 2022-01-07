namespace ChatApp.Api.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddGroupMember : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "MemberIds",
			table: "Groups");

		migrationBuilder.CreateTable(
			name: "GroupMembers",
			columns: table => new
			{
				UserId = table.Column<Guid>(type: "TEXT", nullable: false),
				GroupId = table.Column<Guid>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_GroupMembers", x => new { x.UserId, x.GroupId });
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "GroupMembers");

		migrationBuilder.AddColumn<string>(
			name: "MemberIds",
			table: "Groups",
			type: "TEXT",
			nullable: true);
	}
}
