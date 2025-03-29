using NetArch.Template.HttpApi;
using NetArch.Template.Infrastructure;

using Serilog;

Log.Logger = SerilogExtensions.CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilogConfiguration(builder.Configuration);

    builder.Services.AddControllers()
        .AddApplicationPart(AssemblyReference.PresentationAssembly);

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLoggingConfiguration();

    app.UseAuthorization();

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
