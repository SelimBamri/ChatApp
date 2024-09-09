using ChatApp.Hubs;
using ChatApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opt => new Dictionary<string, UserConnection>());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapHub<ChatHub>("/chat");

app.MapControllers();

app.Run();
