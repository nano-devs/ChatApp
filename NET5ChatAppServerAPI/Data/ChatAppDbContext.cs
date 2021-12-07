
using Microsoft.EntityFrameworkCore;

using NET5ChatAppServerAPI.Models;

namespace NET5ChatAppServerAPI.Data
{
	public class ChatAppDbContext : DbContext
	{
		#region Constructor & Destructor

		public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options) : base(options)
		{

		}

		#endregion Constructor & Destructor

		#region Properties

		public DbSet<PrivateChat> PrivateChats { set; get; }

		public DbSet<GroupChat> GroupChats { set; get; }

		public DbSet<Contacts> Contacts { set; get; }

		public DbSet<Groups> Groups { set; get; }

		#endregion Properties
	}
}
