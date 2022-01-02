namespace ChatApp.Api.Data;

using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class ChatAppDbContext : DbContext
{
	#region Constructor & Destructor

	public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options) : base(options)
	{

	}

	#endregion Constructor & Destructor

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		// Customize the ASP.NET Core Identity model and override the defaults if needed.
		// For example, you can rename the ASP.NET Core Identity table names and more.
		// Add your customizations after calling base.OnModelCreating(builder);

		builder.Entity<Friends>(entity =>
		{
			entity.HasKey("UserId", "FriendId");
		});
		builder.Entity<GroupMember>(entity =>
		{
			entity.HasKey("UserId", "GroupId");
		});
		builder.Entity<PendingGroupChat>(entity =>
		{
			entity.HasKey("GroupChatId", "UserId");
		});
	}

	#region Properties

	public DbSet<PrivateChat>? PrivateChats { set; get; }

	public DbSet<GroupChat>? GroupChats { set; get; }

	public DbSet<PendingGroupChat>? PendingGroupChats { set; get; }

	public DbSet<Friends>? Friends { set; get; }

	public DbSet<Groups>? Groups { set; get; }

	public DbSet<GroupMember>? GroupMembers { set; get; }

	#endregion Properties
}
