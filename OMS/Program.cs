using OMS.Application.Contracts.Services;
using OMS.Application.Services.Orders;
using OMS.Application.Commands.Initialization;
using OMS.Application.Queries.Initialization;
using OMS.Domain.Orders.Repositories;
using OMS.Infrastructure.Persistance.EF.Repositories;
using OMS.Infrastructure.Persistance.EF.Context;
using OMS.Infrastructure.Persistance.EF.Initializations;
using OMS.Application.Services.Init;
using OMS.Application.Services.StateMachine;
using OMS.Infrastructure.Messaging.Masstransit.Init;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.InitializeCommands();

builder.Services.InitializeQueries();

string OutBoxDbConstr = builder.Configuration.GetSection("OutBoxDb").Value;

string AppConnectionStr = builder.Configuration.GetSection("AppConstr").Value;

builder.Services.InjectSqlServerEfCoreDependencies(AppConnectionStr);

builder.Services.InitMasstransit<OrderStateMachine,OrderStateInstance>();

builder.Services.InitializeJobs();

builder.Services.InitializeApplicationService(OutBoxDbConstr);

builder.Services.InjectOutboxDb(OutBoxDbConstr);

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
