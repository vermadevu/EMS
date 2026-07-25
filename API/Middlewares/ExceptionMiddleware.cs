using System.Net;
using System.Text.Json;
using API.Exceptions;

namespace API.Middlewares;

public class ExceptionMiddleware(
    RequestDelegate next,
    IWebHostEnvironment environment)
{
    private readonly RequestDelegate _next = next;
    private readonly IWebHostEnvironment _environment = environment;

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

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiErrorResponse();

        switch (exception)
        {
            case ApiException apiException:
                context.Response.StatusCode = apiException.StatusCode;

                response.StatusCode = apiException.StatusCode;
                response.Message = apiException.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = "An unexpected error occurred.";
                break;
        }

        if (_environment.IsDevelopment())
        {
            response.Details = exception.StackTrace;
            // If you prefer full stack trace while developing:
            // response.Details = exception.StackTrace;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}