using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using NET5ChatAppServerAPI.Configurations;
using NET5ChatAppServerAPI.Data;

namespace NET5ChatAppServerAPI
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public IWebHostEnvironment Environment { get; }

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			this.Configuration = configuration;
			this.Environment = env;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "NET5ChatAppServerAPI", Version = "v1" });
			});
			services.AddRouting(options =>
			{
				options.LowercaseUrls = true;
				options.LowercaseQueryStrings = true;
			});
			services.Configure<JsonOptions>(options =>
			{
				if (this.Environment.IsDevelopment())
				{
					options.JsonSerializerOptions.WriteIndented = true;
				}
				else
				{
					options.JsonSerializerOptions.WriteIndented = false;
				}
				options.JsonSerializerOptions.AllowTrailingCommas = true;
			});
			services.AddDatabase(this.Configuration, this.Environment.IsDevelopment());
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NET5ChatAppServerAPI v1"));
			}

			//app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
