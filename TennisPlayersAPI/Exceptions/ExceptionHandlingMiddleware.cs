using System.Net;
using System.Text.Json;

namespace TennisPlayersAPI.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (PlayerException ex)
            {
                await HandlePlayerExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericExceptionAsync(context, ex);
            }
        }

        private static Task HandlePlayerExceptionAsync(HttpContext context, PlayerException ex)
        {
            var statusCode = ex.ErrorType switch
            {
                PlayerErrorType.NotFound => HttpStatusCode.NotFound,         // 404
                PlayerErrorType.AlreadyExists => HttpStatusCode.Conflict,         // 409
                PlayerErrorType.CreationFailed => HttpStatusCode.BadRequest,       // 400
                PlayerErrorType.UpdateFailed => HttpStatusCode.BadRequest,       // 400
                PlayerErrorType.DeletionFailed => HttpStatusCode.BadRequest,       // 400
                _ => HttpStatusCode.InternalServerError
            };

            var response = new
            {
                errorType = ex.ErrorType.ToString(),
                message = ex.Message,
                playerId = ex.PlayerId // optionnel (sera null si pas fourni)
            };

            var payload = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        }

        private static Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new
            {
                errorType = "InternalServerError",
                message = "Une erreur inattendue est survenue.",
                details = ex.Message
            };

            var payload = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(payload);
        }

    }
}
