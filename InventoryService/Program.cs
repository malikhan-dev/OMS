using InventoryService.Services;
using Microsoft.Data.SqlClient;
using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Init;
using OMS.Infrastructure.Persistance.EF.Initializations;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddGrpc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppEventPublisher>();

string OutBoxDbConstr = "Data Source=localhost,1433;Initial Catalog=OutBox;User Id=sa;password=Your_password123;TrustServerCertificate=True;";

string AppConnectionStr = "Data Source=localhost,1433;Initial Catalog=OMS;User Id=sa;password=Your_password123;TrustServerCertificate=True;";


builder.Services.InjectSqlServerEfCoreDependencies(AppConnectionStr);



InitializeApp.InitializeApplicationService(builder.Services, OutBoxDbConstr);


builder.Services.InjectOutboxDb(OutBoxDbConstr);

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
