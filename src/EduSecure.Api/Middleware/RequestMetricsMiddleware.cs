using System.Diagnostics;
using EduSecure.Api.Metrics;
namespace EduSecure.Api.Middleware;
public class RequestMetricsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        try { await next(context); }
        finally
        {
            sw.Stop();
            EduMetrics.HttpRequests.WithLabels(context.Request.Method, context.Response.StatusCode.ToString()).Inc();
            EduMetrics.HttpDuration.Observe(sw.Elapsed.TotalSeconds);
        }
    }
}
