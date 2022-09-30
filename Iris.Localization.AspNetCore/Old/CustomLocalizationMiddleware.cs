using Microsoft.AspNetCore.Http;

namespace Iris.Localization.AspNetCore.Old
{
    /// <summary>
    /// Arakatman localizasyon uygulanıyor
    /// </summary>
    public class CustomLocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        // IMPORTANT NOTE: https://stackoverflow.com/questions/48590579/cannot-resolve-scoped-service-from-root-provider-net-core-2

        public CustomLocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private static readonly string[] StaticFileExtensions = { ".css", ".js", ".ico", ".woff", ".woff2", ".map", ".png", "gif", ".jpg", ".jpeg", ".svg" };

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string path = httpContext.Request.Path;

            if (StaticFileExtensions.Any(i => path.EndsWith(i, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(httpContext);
                return;
            }





            await _next(httpContext);

            //Donen ortak obje
            var response = httpContext.Response;
        }
    }


}