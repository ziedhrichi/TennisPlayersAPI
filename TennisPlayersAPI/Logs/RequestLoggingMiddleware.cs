namespace TennisPlayersAPI.Logs
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            _logger.LogInformation("➡️ Requête {Method} {Path}", request.Method, request.Path);

            // Exécute la requête suivante dans le pipeline
            await _next(context);

            _logger.LogInformation("⬅️ Réponse {StatusCode}", context.Response.StatusCode);
        }
    }
}
