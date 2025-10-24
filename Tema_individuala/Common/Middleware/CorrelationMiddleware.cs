using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Tema_individuala.Common.Middleware
{
    // Middleware-ul asigura ca fiecare request are un Correlation ID unic
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationMiddleware> _logger;

        // Constructorul primeste urmatorul middleware si loggerul
        public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verificam daca requestul are deja un Correlation ID (ex. dintr-un header)
            string correlationId = context.Request.Headers.ContainsKey("X-Correlation-ID")
                ? context.Request.Headers["X-Correlation-ID"].ToString()
                : Guid.NewGuid().ToString();

            // Salvam ID-ul in HttpContext.Items (ca sa-l putem accesa in handleri)
            context.Items["CorrelationId"] = correlationId;

            // Adaugam headerul in response (pentru debugging sau tracking)
            context.Response.Headers["X-Correlation-ID"] = correlationId;

            // Cream un "scope" de logging, astfel incat toate logurile ulterioare sa contina ID-ul
            using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
            {
                var stopwatch = Stopwatch.StartNew();
                _logger.LogInformation("Started processing request {Method} {Path} with CorrelationId: {CorrelationId}",
                    context.Request.Method, context.Request.Path, correlationId);

                try
                {
                    await _next(context); // Continua catre urmatorul middleware / endpoint
                }
                finally
                {
                    stopwatch.Stop();
                    _logger.LogInformation("Finished request {Method} {Path} with CorrelationId: {CorrelationId} in {ElapsedMilliseconds}ms",
                        context.Request.Method, context.Request.Path, correlationId, stopwatch.ElapsedMilliseconds);
                }
            }
        }
    }
}
