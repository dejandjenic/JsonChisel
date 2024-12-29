using Microsoft.AspNetCore.Builder;

namespace JsonChisel;

public static class RegistrationExtensions
{
    public static IApplicationBuilder UseJsonChisel(this IApplicationBuilder app)
    {
        app.UseMiddleware<JsonChiselMiddleware>();
        return app;
    }
    
    public static IApplicationBuilder UseJsonChisel(this IApplicationBuilder app,params object[] args)
    {
        app.UseMiddleware<JsonChiselMiddleware>(args);
        return app;
    }
}