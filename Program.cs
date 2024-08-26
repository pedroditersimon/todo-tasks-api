using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoAPI.Models;
using TodoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// load the PostgreDBSettings from appsettings.json
builder.Services.Configure<PostgreDBSettings>(builder.Configuration.GetSection("PostgreDBSettings"));

// configure DBContext of PostgreDBService, using loaded settings
builder.Services.AddDbContext<PostgreDBService>((IServiceProvider provider, DbContextOptionsBuilder optionsBuilder) =>
{
    PostgreDBSettings dbSettings = provider.GetRequiredService<IOptions<PostgreDBSettings>>().Value;
    var connectionString = $"Host={dbSettings.Host};Username={dbSettings.Username};Password={dbSettings.Password};Database={dbSettings.DatabaseName}";
    optionsBuilder.UseNpgsql(connectionString);
});

// configure ITodoDBHandler to use as PostgreDBService
builder.Services.AddScoped<ITodoDBHandler, PostgreDBService>();

builder.Services.AddScoped<TodoService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
