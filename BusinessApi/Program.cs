using BusinessApi.DataAccessObject.Implementation;
using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Repositories.Implementation;
using BusinessApi.Repositories.Interface;
using BusinessApi.Utils;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDashBoardRegisterRepository, DashBoardRegisterRepository>();
builder.Services.AddScoped<IDashBoardRegisterDao, DashBoardRegisterDao>(); 
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
