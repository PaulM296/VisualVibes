using Microsoft.AspNetCore.Server.HttpSys;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace VisualVibes.Api
{
    public class TimingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public TimingMiddleware(ILogger<TimingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            var start = DateTime.UtcNow;
            await _next.Invoke(ctx);
            _logger.LogInformation($"Timing: {ctx.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds}ms");
        }
    }

    public static class TimingExtensions
    {
        public static IApplicationBuilder UseTiming(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TimingMiddleware>();
        }

        //public static void AddTiming(this IServiceCollection svcs)
        //{
        //    svcs.AddTransient<ITiming, SomeTiming>();
        //}
    }
}
