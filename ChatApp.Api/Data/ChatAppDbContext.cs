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
			entity.HasKey("UserId", "GroupChatId");
		});

		builder.Entity<User>(entity =>
		{
			entity.Property(u => u.UniqueGuid).ValueGeneratedOnAdd();
			entity.HasAlternateKey(u => u.UniqueGuid);
		});

		// create foreign key
		builder.Entity<Message>(entity =>
		{
			entity.HasAlternateKey(o => o.PostedByUserId);
		});

		builder.Entity<PrivateMessage>(entity =>
		{
			entity.HasAlternateKey(o => o.SendToUserId);
		});
	}

	#region Properties

	public DbSet<GroupChat>? GroupChats { set; get; }
	public DbSet<PendingGroupChat>? PendingGroupChats { set; get; }
	public DbSet<PrivateChat>? PrivateChats { set; get; }

	public DbSet<Friends>? Friends { set; get; }

	public DbSet<Group>? Groups { set; get; }

	public DbSet<GroupMember>? GroupMembers { set; get; }

	public DbSet<User>? Users { get; set; }

	public DbSet<RefreshToken>? RefreshTokens { get; set; }

	public DbSet<Message>? Messages { get; set; }

	public DbSet<PrivateMessage>? PrivateMessages { get; set; }

	public DbSet<GroupMessage>? GroupMessages { get; set; }

	#endregion Properties
}
