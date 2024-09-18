namespace ApiRestRs.Authentication
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration? _configuration;
        public string? ApiKey { get; set; }

        public ApiKeyAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeader, out var extractedApiKey))
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync("ApiKey Erroneo.");
            //    return;
            //}

            //var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
            //if (!apiKey.Equals(extractedApiKey))
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync("ApiKey Invalida.");
            //    return;

            //}

           
            ApiKey = "ApiKey Valida";
            

            await _next(context);
        }
    }
}
