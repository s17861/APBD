using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StudentAPI
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if(context.Request != null)
            {
                var method = context.Request.Method;
                var path = context.Request.Path.ToString();
                var queryString = context.Request?.QueryString.ToString();
                var body = "";
                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
                using (StreamWriter writer = new StreamWriter(@"..\requests.txt", true))
                {
                    writer.WriteLine("Method: " + method + "; Path: " + path + "; Query String: " + queryString + "; Body: " + body);
                }
            }

            await _next(context);
        }
    }
}