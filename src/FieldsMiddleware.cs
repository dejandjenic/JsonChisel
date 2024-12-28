using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace JsonChisel;

public class FieldsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _headerName;

    public FieldsMiddleware(RequestDelegate next,string headerName = "fields")
    {
        _next = next;
        _headerName = headerName;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var fields = context.Request.Headers[_headerName];
        if (!string.IsNullOrEmpty(fields))
        {
            Stream originalBody = context.Response.Body;
            try
            {
                using var memStream = new MemoryStream();
                context.Response.Body = memStream;

                await _next(context);

                memStream.Position = 0;
                string responseBody = await new StreamReader(memStream).ReadToEndAsync();
            
                memStream.Position = 0;

                await JsonSerializer.SerializeAsync(originalBody, FieldsExtractor.ExtractFields(responseBody, fields),new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
        else
        {
            await _next(context);
        }
    }
}