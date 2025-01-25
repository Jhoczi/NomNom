using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NomNom.Core.Mapping;
using NomNom.Infrastructure.Data;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Logging.AddApplicationInsights();
builder.ConfigureFunctionsWebApplication();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<RecipesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("No connection string recognized: 'DefaultConnection'.")));

var app = builder.Build();

await app.RunAsync();
