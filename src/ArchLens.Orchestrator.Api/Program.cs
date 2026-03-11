using ArchLens.Orchestrator.Api.Configurations;
using ArchLens.Orchestrator.Api.ExceptionHandlers;
using ArchLens.Orchestrator.Api.Middlewares;
using ArchLens.Orchestrator.Application;
using ArchLens.Orchestrator.Infrastructure;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddSerilogLogging();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHealthChecks();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    app.UseExceptionHandler();
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

namespace ArchLens.Orchestrator.Api
{
    public partial class Program;
}
