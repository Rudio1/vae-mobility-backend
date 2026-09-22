using VaeMobility.Application;
using VaeMobility.Application.Generic.Services.Interfaces;
using VaeMobility.Infra.Data;
using VaeMobility.Infra.ExternalApis;
using VaeMobility.Infra.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICurrentUserContext, NullCurrentUserContext>();

builder.Services
    .AddApplication()
    .AddInfraData(builder.Configuration)
    .AddExternalApis(builder.Configuration)
    .AddInfraJobs();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "Jobs host healthy" }));

app.Run();
