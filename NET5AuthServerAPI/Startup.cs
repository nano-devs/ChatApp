using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NET5AuthServerAPI.Models;
using NET5AuthServerAPI.Services.PasswordHashers;
using NET5AuthServerAPI.Services.TokenGenerators;
using NET5AuthServerAPI.Services.UserRepositories;

namespace NET5AuthServerAPI
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            AuthenticationConfiguration authConfig = new AuthenticationConfiguration();
            configuration.Bind("Authentication", authConfig);
            services.AddSingleton(authConfig);

            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IPasswordHasher, BCryptHasher>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
