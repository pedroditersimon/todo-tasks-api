using Microsoft.Extensions.Options;
using TodoAPI.Models;
using TodoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<PostgreDBSettings>(builder.Configuration.GetSection("PostgreDBSettings"));
builder.Services.AddScoped<ITodoDBHandler, PostgreDBService>(provider =>
{
    PostgreDBSettings dbSettings = provider.GetRequiredService<IOptions<PostgreDBSettings>>().Value;
    return new PostgreDBService(dbSettings.Host, dbSettings.Username, dbSettings.Password, dbSettings.DatabaseName);
});

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
