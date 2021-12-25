using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NET5AuthServerAPI.Models;
using NET5AuthServerAPI.Services.Authenticators;
using NET5AuthServerAPI.Services.RefreshTokenRepositories;
using NET5AuthServerAPI.Services.TokenGenerators;
using NET5AuthServerAPI.Services.TokenValidators;
using System.Text;

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

            services.AddIdentityCore<User>(option =>
            {
                option.User.RequireUniqueEmail = true;

                // For testing only, remove this for production
                option.Password.RequireDigit = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequiredLength = 0;
            }).AddEntityFrameworkStores<AuthenticationDbContext>();
            services.AddSingleton<IdentityErrorDescriber>();

            AuthenticationConfiguration authConfig = new AuthenticationConfiguration();
            configuration.Bind("Authentication", authConfig);
            services.AddSingleton(authConfig);

            string connectionString = configuration.GetConnectionString("sqlite");
            services.AddDbContext<AuthenticationDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
            services.AddScoped<IRefreshTokenRepository, DatabaseRefreshTokenRepository>();
            services.AddScoped<Authenticator>();

            services.AddSingleton<AccessTokenGenerator>();
            services.AddSingleton<RefreshTokenGenerator>();
            services.AddSingleton<ITokenValidator, RefreshTokenValidator>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessTokenSecret)),
                    ValidIssuer = authConfig.Issuer,
                    ValidAudience = authConfig.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = System.TimeSpan.Zero,
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
