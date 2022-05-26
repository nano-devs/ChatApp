global using System;
using AuthEndpoints;
using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Enabling CORS
string FrontendOrigin = "VueFrontendOrigin";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: FrontendOrigin, builder =>
	{
		builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader();
	});
});
#endregion

// TODO: configure tls/ssl and http protocol
builder.WebHost.ConfigureKestrel(o =>
{
    o.AddServerHeader = false;
    //o.ConfigureEndpointDefaults(listenOptions =>
    //{
    //	listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    //});
    //o.ConfigureHttpsDefaults(listenOptions =>
    //{
    //	listenOptions.SslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12;
    //	listenOptions.OnAuthenticate = (context, sslOptions) =>
    //	{
    //		sslOptions.EnabledSslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12;
    //		if (sslOptions.ApplicationProtocols != null)
    //		{
    //			sslOptions.ApplicationProtocols.Clear();
    //		}
    //		else
    //		{
    //			sslOptions.ApplicationProtocols = new();
    //		}

    //		sslOptions.ApplicationProtocols.Add(SslApplicationProtocol.Http2);
    //		sslOptions.ApplicationProtocols.Add(SslApplicationProtocol.Http11);
    //	};
    //});
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IFriendsRepository, FriendsRepository>();
builder.Services.AddScoped<GroupsRepository>();
builder.Services.AddScoped<IGroupMembersRepository, GroupMembersRepository>();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApp.API", Version = "v1" });
});
builder.Services.AddRouting(options =>
{
	options.LowercaseUrls = true;
	options.LowercaseQueryStrings = true;
});
builder.Services.Configure<JsonOptions>(options =>
{
	if (builder.Environment.IsDevelopment())
	{
		options.JsonSerializerOptions.WriteIndented = true;
	}
	else
	{
		options.JsonSerializerOptions.WriteIndented = false;
	}
	options.JsonSerializerOptions.AllowTrailingCommas = true;
});
builder.Services.AddDbContext<ChatAppDbContext>(options =>
{
	if (builder.Environment.IsDevelopment())
	{
		options.UseSqlite(
				builder.Configuration.GetConnectionString("DataSQLiteConnection"));
	}
	else
	{
		//options.UseNpgsql(
		//	builder.Configuration.GetConnectionString("DataPostgreConnection"),
		//	options => { options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null); });
	}
});

builder.Services.AddIdentityCore<User>(options =>
{
	options.User.RequireUniqueEmail = true;
	options.Password.RequireDigit = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 0;
}).AddEntityFrameworkStores<ChatAppDbContext>()
  .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

var accessValidationParam = new TokenValidationParameters()
{
	IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890qwerty")),
	ValidIssuer = "https://localhost:8000",
	ValidAudience = "https://localhost:8000",
	ValidateIssuerSigningKey = true,
	ClockSkew = TimeSpan.Zero,
};

builder.Services.AddAuthEndpoints<int, User>(options =>
{
	options.AccessSigningOptions = new JwtSigningOptions()
	{
		SigningKey = accessValidationParam.IssuerSigningKey,
		Algorithm = SecurityAlgorithms.HmacSha256,
		ExpirationMinutes = 120,
	};
	options.RefreshSigningOptions = new JwtSigningOptions()
	{
		SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwerty0987654321")),
		Algorithm = SecurityAlgorithms.HmacSha256,
		ExpirationMinutes = 2880,
	};
	options.Audience = "localhost:8000";
	options.Issuer = "localhost:8000";
}).AddJwtBearerAuthScheme(accessValidationParam);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
else
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp.API v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(FrontendOrigin);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=swagger}/{action=Index}/{id?}");

app.Run();
