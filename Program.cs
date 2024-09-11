using ChatApp.Hubs;
using ChatApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("angularApp", pb =>
    {
        pb.WithOrigins("http://localhost:4200");
        pb.AllowAnyHeader();
        pb.AllowAnyMethod();
        pb.AllowCredentials();
    });
}
);
builder.Services.AddSignalR();
builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opt => new Dictionary<string, UserConnection>());

var app = builder.Build();

app.UseCors("angularApp");

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chat");

app.MapControllers();

app.Run();
