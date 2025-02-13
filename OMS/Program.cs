using OMS.Application.Contracts.Services;
using OMS.Application.Services.Orders;
using OMS.Application.Commands.Initialization;
using OMS.Application.Queries.Initialization;
using OMS.Domain.Orders.Repositories;
using OMS.Infrastructure.Persistance.EF.Repositories;
using OMS.Infrastructure.Persistance.EF.Context;
using OMS.Infrastructure.Persistance.EF.Initializations;
using OMS.Application.Services.Init;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.InitializeCommands();

builder.Services.InitializeQueries();

InitializeApp.InitMassTransit(builder.Services);
InitializeApp.InitializeApplicationService(builder.Services);

builder.Services.InjectSqlServerEfCoreDependencies("Data Source=localhost,1433;Initial Catalog=OMS;Integrated Security = true;TrustServerCertificate=True");

var app = builder.Build();

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
