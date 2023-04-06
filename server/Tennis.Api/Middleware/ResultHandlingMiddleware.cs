using Newtonsoft.Json;
using System.Net;
using Tennis.Model.Results;
namespace Tennis.Api.Middleware;
public class ResultHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ResultHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Items.ContainsKey("Result"))
        {
            var result = context.Items["Result"] as Result<dynamic, dynamic>;

            if (result != null)
            {
                if (result.IsSuccess)
                {
                    if (result.Success != null)
                    {
                        await WriteResponse(context, HttpStatusCode.OK, new { message = "Success", success = result.Success });
                    }
                }
                else
                {
                    if (result.Failure != null)
                    {
                        await WriteResponse(context, HttpStatusCode.BadRequest, new { message = "Failure", failure = result.Failure });
                    }
                }
            }
        }
    }
    private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object content)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(content, settings));
    }
}