using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Init;
using OMS.Infrastructure.Persistance.EF.Initializations;
using PaymentService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddGrpc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AppEventPublisher>();


string OutBoxDbConstr = "Data Source=localhost,1433;Initial Catalog=OutBox;User Id=sa;password=Your_password123;TrustServerCertificate=True;";

string AppConnectionStr = "Data Source=localhost,1433;Initial Catalog=OMS;User Id=sa;password=Your_password123;TrustServerCertificate=True;";


builder.Services.InjectSqlServerEfCoreDependencies(AppConnectionStr);

//InitializeApp.InitMassTransit(builder.Services);

InitializeApp.InitializeApplicationService(builder.Services, OutBoxDbConstr);

//OMS.Infrastructure.Messaging.Masstransit.Init.Initialization.InitMasstransit(builder.Services);



builder.Services.InjectOutboxDb(OutBoxDbConstr);

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
