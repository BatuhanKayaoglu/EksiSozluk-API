using EksiSozluk.Infrastructure.Persistance.Extensions;
using EksiSozluk.Api.Application.Extensions;
using FluentValidation.AspNetCore;
using EksiSozluk.Api.WebApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(); // paketi yüklemeyi unutma.
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();
builder.Services.ConfigureAuth(builder.Configuration);


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
