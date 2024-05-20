using System.Net;
using System.Text.Json;
using VisualVibes.Api.Contracts;
using VisualVibes.App.Exceptions;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
    
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            } 
            catch (Exception ex) when (ex is EntityNotFoundException || ex is CommentsNotFoundException 
            || ex is ConversationNotFoundException || ex is MessageNotFoundException || ex is PostNotFoundException
            || ex is ReactionNotFoundException || ex is UserNotFoundException || ex is UserProfileNotFoundException)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (InvalidCredentialsException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (Exception ex) when (ex is EntityAlreadyExistsException || ex is EmailAlreadyExistsException)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode httpStatusCode)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            var error = new Error
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedError = JsonSerializer.Serialize(error, options);

            await context.Response.WriteAsync(serializedError);
        }
    }
}
