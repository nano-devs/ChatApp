using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NET5ChatAppServerAPI.Data;

namespace NET5ChatAppServerAPI.Configurations
{
	public static class ConfigureDatabase
	{
		/// <summary>
		///		Add database 
		/// </summary>
		/// <param name="services"></param>
		/// <param name="config">
		///		Configuration that contain database connection string.
		/// </param>
		/// <param name="LoggerFactory"></param>
		/// <param name="isDevelopment">
		///		Use developer database exception.
		/// </param>
		/// <returns>
		///		A reference to this instance after the operation has completed.
		/// </returns>
		public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config, bool isDevelopment = false)
		{
			services
				.AddDbContext<ChatAppDbContext>(options =>
				{
					if (isDevelopment)
					{
						options.UseSqlite(
								config.GetConnectionString("DataSQLiteConnection"));
					}
					else
					{
						options.UseNpgsql(
							config.GetConnectionString("DataPostgreConnection"),
							options => { options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null); });
					}
				});

			return services;
		}
	}
}
