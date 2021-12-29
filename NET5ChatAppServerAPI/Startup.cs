
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using NET6ChatAppServerApi.Configurations;

namespace NET6ChatAppServerApi
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
