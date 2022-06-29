using BOMA.WRT.Application;
using BOMA.WRT.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure(app.Environment, app.Configuration);
app.MapControllers();

app.Run();