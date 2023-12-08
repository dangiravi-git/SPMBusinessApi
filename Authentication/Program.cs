using Authentication.DataAccessObject.Implementation;
using Authentication.DataAccessObject.Interface;
using Authentication.Repositories.Implementation;
using Authentication.Repositories.Interface;
using Authentication.Utils;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//var logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .Enrich.FromLogContext()
//    .CreateLogger();

// Add services to the container.
builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(logger);

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAuthenticationDao, AuthenticationDao>();
builder.Services.AddScoped<IDbUtility, DbUtility>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder
            .WithOrigins("http://localhost:4200") // Replace with your Angular app's origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // If needed for handling cookies or credentials
    });
});

var app = builder.Build();

app.UseCors("AllowOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
