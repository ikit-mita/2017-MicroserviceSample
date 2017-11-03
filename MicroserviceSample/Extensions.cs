using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MicroserviceSample
{
    public static class Extensions
    {
        public static IApplicationBuilder MapMethod(this IApplicationBuilder app,
            string httpMethod,
            Action<IApplicationBuilder> configuration)
        {
            return app.MapWhen(ctx => ctx.Request.Method == httpMethod, configuration);
        }

        public static async Task<T> ReadJsonAsync<T>(this HttpRequest request)
        {
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string body = Encoding.UTF8.GetString(buffer);
            T model = JsonConvert.DeserializeObject<T>(body);
            return model;
        }
    }
}
