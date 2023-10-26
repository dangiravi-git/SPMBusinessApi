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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
