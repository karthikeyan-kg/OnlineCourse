using CourseService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CourseService.API.Middleware
{
    public class ExceptionHandlingMiddleware : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
        {
            var problem = new ProblemDetails
            {
                Title = "An error occurred",
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case InternalServerErrorException:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    problem.Title = "Conflict";
                    break;

                case UnauthorizedException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    problem.Title = "Unauthorized";
                    break;

                case ForbiddenException:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    problem.Title = "Forbidden";
                    break;

                case NotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    problem.Title = "Not Found";
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
            }

            problem.Status = context.Response.StatusCode;

            await context.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}
