using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoAPI.API.Interceptors;
using TodoAPI.API.Repositories;
using TodoAPI.API.Services;
using TodoAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// set env varaibles to appsetting.json
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.

// load the PostgreDBSettings from appsettings.json
builder.Services.Configure<PostgresDBSettings>(builder.Configuration.GetSection("PostgresDBSettings"));

// configure DBContext of PostgreDBService, using loaded settings
builder.Services.AddDbContext<TodoDBContext>((IServiceProvider provider, DbContextOptionsBuilder optionsBuilder) =>
{
	PostgresDBSettings dbSettings = provider.GetRequiredService<IOptions<PostgresDBSettings>>().Value;
	var connectionString = $"Host={dbSettings.Host};Username={dbSettings.User};Password={dbSettings.Pass};Database={dbSettings.DbName}";
	Console.WriteLine(connectionString);
	optionsBuilder.UseNpgsql(connectionString);
	//optionsBuilder.UseLazyLoadingProxies();
	optionsBuilder.AddInterceptors(
		new ReadExampleInterceptor()/*,
        new SecondLevelCacheInterceptor(provider.GetRequiredService<IMemoryCache>())
        */
	);
});

//builder.Services.AddMemoryCache();

builder.Services.AddScoped<DbContext, TodoDBContext>();

// configure Repositories to use the PostgreDBService
builder.Services.AddScoped<ITodoTaskGoalRepository, TodoTaskGoalRepository>();
builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
builder.Services.AddScoped<ITodoGoalRepository, TodoGoalRepository>();

builder.Services.AddScoped<ITodoTaskGoalService, TodoTaskGoalService>();
builder.Services.AddScoped<ITodoGoalService, TodoGoalService>();
builder.Services.AddScoped<ITodoTaskService, TodoTaskService>();
builder.Services.AddScoped<IGoalCompletedStatusService, GoalCompletedStatusService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(TodoAPI.Data.Mappers.MappingProfile));


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAnyOrigin",
		policy =>
		{
			policy.WithOrigins("*")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	DbContext context = scope.ServiceProvider.GetRequiredService<TodoDBContext>();

	// Create database using the DbContext
	//context.Database.EnsureCreated();

	// Create and sync database using 'dotnet ef migrations'
	context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseRouting();

app.UseCors("AllowAnyOrigin"); // Aplica la política CORS

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
