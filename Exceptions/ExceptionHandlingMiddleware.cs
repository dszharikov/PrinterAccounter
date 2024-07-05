using Microsoft.Data.SqlClient;
using PrinterAccounter.DTOs.Output;
using PrinterAccounter.Exceptions;
using System.Net;
using System.Text.Json;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string message;

        switch (exception)
        {
            case DatabaseException databaseException:
                status = HttpStatusCode.InternalServerError;
                message = databaseException.Message;
                break;
            case SqlException sqlException:
                status = HttpStatusCode.InternalServerError;
                message = "An error occurred while accessing the database.";
                break;
            case NotFoundException notFoundException:
                status = HttpStatusCode.NotFound;
                message = notFoundException.Message;
                break;
            case ValidationException validationException:
                status = HttpStatusCode.BadRequest;
                message = validationException.Message;
                break;
            case FileValidationException fileValidationException:
                status = HttpStatusCode.UnprocessableEntity;
                message = fileValidationException.Message;
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
                break;
        }

        var response = new ErrorResponseDto
        {
            StatusCode = (int)status,
            Message = message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        var result = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(result);
    }
}
