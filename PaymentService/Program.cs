using Microsoft.Data.SqlClient;
using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Init;
using PaymentService.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddGrpc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AppEventPublisher>();


string OutBoxDbConstr = "Data Source=localhost,1433;Initial Catalog=OutBox;Integrated Security = true;TrustServerCertificate=True";

builder.Services.AddKeyedTransient<IDbConnection, SqlConnection>("OutBoxConnection", (ServiceProvider, cnt) => new SqlConnection(OutBoxDbConstr));


InitializeApp.InitMassTransit(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGrpcService<PaymentHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
