using SagaOrchestrator.Contexts;
using SagaOrchestrator.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddHttpClient<CamundaContext>();
builder.Services.AddApplicationOptions(builder.Configuration);
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
