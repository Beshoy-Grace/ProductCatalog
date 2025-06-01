namespace ProductCatalog.Client
{
    public class UnauthorizedRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Store original response body
            var originalBodyStream = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            // Reset stream position to read response
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(memoryStream).ReadToEnd();

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;

            if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.Redirect("/Account/Unauthorized");
            }
        }
    }


}
