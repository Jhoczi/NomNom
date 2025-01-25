using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace NomNomApi.Functions;

public class HealthCheckFunction
{
    [Function("health")]
    public IActionResult HealthCheck([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
    {
        return new OkObjectResult("Health");
    }
}