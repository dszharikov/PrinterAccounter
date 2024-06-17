using System.Reflection;
using Microsoft.OpenApi.Models;
using PrinterAccounter.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//Swagger Documentation Section
var info = new OpenApiInfo()
{
    Title = "Printer Accounter API Documentation",
    Version = "v1",
    Description = "API respresents the Printer Accounter application. " + 
        "It provides the endpoints to get employees, branches, devices, manage the installations, print tasks.",
    Contact = new OpenApiContact()
    {
        Name = "Daniil Zharikov",
        Email = "dszharikov@yandex.ru",
    }

};

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", info);

// Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(u =>
            {
                u.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
    
    app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "swagger";
            c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "V1");
        });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
