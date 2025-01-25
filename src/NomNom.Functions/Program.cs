using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Logging.AddApplicationInsights();
builder.ConfigureFunctionsWebApplication();

var app = builder.Build();

await app.RunAsync();
