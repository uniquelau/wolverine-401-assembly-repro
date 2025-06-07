using JasperFx;
using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opts =>
{
	// opts.ApplicationAssembly = typeof(Program).Assembly;
});

builder.Services.AddControllers();
builder.Services.AddWolverineHttp();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapWolverineEndpoints();

app.UseAuthorization();

app.MapControllers();

return await app.RunJasperFxCommands(args);
