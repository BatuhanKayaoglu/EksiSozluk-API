using EksiSozluk.Infrastructure.Persistance.Extensions;
using EksiSozluk.Api.Application.Extensions;
using FluentValidation.AspNetCore;
using EksiSozluk.Api.WebApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(); // paketi y�klemeyi unutma.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();
builder.Services.ConfigureAuth(builder.Configuration);
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = "redis-12985.c267.us-east-1-4.ec2.redns.redis-cloud.com:12985";
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureExceptionHandling(app.Environment.IsDevelopment());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
