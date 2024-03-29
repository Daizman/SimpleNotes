using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace SimpleNotes.Endpoints;

public static class HealthChecksEndpoints
{
    public static void MapSimpleNotesHealthChecks(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = async (context, report) =>
            {
                var result = JsonSerializer.Serialize(
                    new
                    {
                        status = report.Status.ToString(),
                        check = report.Entries.Select(entry => new 
                        { 
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            exception = entry.Value.Exception is not null ? entry.Value.Exception.Message : "none",
                            duration = entry.Value.Duration.ToString()
                        })
                    });

                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(result);
            }
        });
        app.MapHealthChecks("/health/alive", new HealthCheckOptions
        {
            Predicate = _ => false
        });
    }
}