using System.Net;
using System.Text.Json;
using TennisPlayers.Domain.Exceptions;

namespace TennisPlayers.Infrastructure.Exceptions
{
    /// <summary>
    /// Une classe de middleware pour gérer les exceptions dans l'api
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Middleware d’interception des requêtes HTTP.
        /// Exécute le traitement suivant dans le pipeline puis capture les exceptions éventuelles.
        /// - Si une <see cref="PlayerException"/> est levée, elle est gérée via <c>HandlePlayerExceptionAsync</c>.
        /// - Si une exception générique est levée, elle est gérée via <c>HandleGenericExceptionAsync</c>.
        /// </summary>
        /// <param name="context">Le contexte HTTP de la requête en cours.</param>
        /// <returns>Une tâche asynchrone représentant l’exécution du middleware.</returns>
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

        /// <summary>
        /// Gère les exceptions spécifiques au domaine <see cref="PlayerException"/> 
        /// et renvoie une réponse HTTP appropriée.
        /// 
        /// - Associe un <see cref="PlayerErrorType"/> à un code HTTP standard :
        ///   • NotFound → 404  
        ///   • AlreadyExists → 409  
        ///   • CreationFailed, UpdateFailed, DeletionFailed → 400  
        ///   • Autres → 500  
        /// 
        /// - Sérialise une réponse JSON contenant :
        ///   • <c>errorType</c> : le type d’erreur (chaîne)  
        ///   • <c>message</c> : le message d’erreur détaillé  
        ///   • <c>playerId</c> : identifiant du joueur concerné (peut être null)  
        /// 
        /// La réponse est envoyée avec le code HTTP adéquat et le type MIME <c>application/json</c>.
        /// </summary>
        /// <param name="context">Le contexte HTTP de la requête.</param>
        /// <param name="ex">L’exception <see cref="PlayerException"/> capturée.</param>
        /// <returns>Une tâche représentant l’écriture asynchrone de la réponse HTTP.</returns>
        private static Task HandlePlayerExceptionAsync(HttpContext context, PlayerException ex)
        {
            var statusCode = ex.ErrorType switch
            {
                PlayerErrorType.NotFound => HttpStatusCode.NotFound,         // 404
                PlayerErrorType.AlreadyExists => HttpStatusCode.Conflict,         // 409
                PlayerErrorType.CreationFailed => HttpStatusCode.BadRequest,       // 400
                PlayerErrorType.UpdateFailed => HttpStatusCode.BadRequest,       // 400
                PlayerErrorType.DeletionFailed => HttpStatusCode.BadRequest,       // 400
                PlayerErrorType.Forbidden => HttpStatusCode.Forbidden,       // 403
                _ => HttpStatusCode.InternalServerError //500
            };


            //var payload = JsonSerializer.Serialize(response);

            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)statusCode;

            //return context.Response.WriteAsync(payload);

            var response = new
            {
                errorType = ex.ErrorType.ToString(),
                message = ex.Message,
                playerId = ex.PlayerId
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";

            return context.Response.WriteAsJsonAsync(response);
        }

        /// <summary>
        /// Gère les exceptions génériques non prises en charge spécifiquement
        /// et renvoie une réponse HTTP 500 (Internal Server Error).
        /// 
        /// - Construit une réponse JSON contenant :
        ///   • <c>errorType</c> : fixé à "InternalServerError"  
        ///   • <c>message</c> : un message générique indiquant qu’une erreur inattendue est survenue  
        ///   • <c>details</c> : le message détaillé de l’exception capturée  
        /// 
        /// La réponse est envoyée avec le type MIME <c>application/json</c>
        /// et le code HTTP <see cref="HttpStatusCode.InternalServerError"/> (500).
        /// </summary>
        /// <param name="context">Le contexte HTTP de la requête.</param>
        /// <param name="ex">L’exception générique capturée.</param>
        /// <returns>Une tâche représentant l’écriture asynchrone de la réponse HTTP.</returns>
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
