using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MicroserviceSample.Middlewares
{
    public class IsAuthenticatedMiddleware
    {
        public IsAuthenticatedMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        private RequestDelegate Next { get; }

        public async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            await Next(context);
        }
    }
}
