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

		builder.Entity<User>(entity =>
		{
			entity.Property(u => u.UniqueGuid).ValueGeneratedOnAdd();
			entity.HasAlternateKey(u => u.UniqueGuid);
		});

		builder.Entity<Group>(entity =>
		{
			entity.Property(u => u.UniqueGuid).ValueGeneratedOnAdd();
			entity.HasAlternateKey(u => u.UniqueGuid);
		});

		builder.Entity<Friend>(entity =>
		{
			entity.HasKey("UserId", "FriendId");
		});

		builder.Entity<GroupMember>(entity =>
		{
			entity.HasKey("UserId", "GroupId");
		});

	}

	#region Properties

	public DbSet<User>? Users { get; set; }

	public DbSet<Friend>? Friends { set; get; }

	public DbSet<Group>? Groups { set; get; }

	public DbSet<GroupMember>? GroupMembers { set; get; }

	public DbSet<Message>? Messages { get; set; }

	public DbSet<PrivateMessage>? PrivateMessages { get; set; }

	public DbSet<GroupMessage>? GroupMessages { get; set; }

	#endregion Properties
}
