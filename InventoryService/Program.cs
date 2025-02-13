using InventoryService.Services;
using Microsoft.Data.SqlClient;
using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Init;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddGrpc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppEventPublisher>();

string OutboxConnectionString = "Data Source=localhost,1433;Initial Catalog=OutBox;Integrated Security = true;TrustServerCertificate=True";

builder.Services.AddKeyedTransient<IDbConnection, SqlConnection>("OutBoxConnection", (ServiceProvider, cnt) => new SqlConnection(OutboxConnectionString));
InitializeApp.InitMassTransit(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGrpcService<InventoryHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
