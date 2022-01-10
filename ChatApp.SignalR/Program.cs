using ChatApp.Api.Data;
using ChatApp.Api.Services.Repositories;
using ChatApp.SignalR.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors();

builder.Services.AddDbContext<ChatAppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DataSQLiteConnection"));
});
builder.Services.AddScoped<GroupsRepository>();
builder.Services.AddScoped<GroupMembersRepository>();

var app = builder.Build();

app.UseCors(builder => builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

app.MapGet("/", () => "Hello World!");
app.MapHub<ChatHub>("/chatHub");

app.Run();
