using System.Threading.Tasks;
using MicroserviceSample.Models;
using MicroserviceSample.Services;
using Microsoft.AspNetCore.Http;

namespace MicroserviceSample.Middlewares
{
    public class PostDataMiddleware
    {
        public PostDataMiddleware(RequestDelegate next) { }
        public async Task Invoke(HttpContext context, IDataService dataService)
        {
            var model = await context.Request.ReadJsonAsync<Model>();
            await dataService.SaveDataAsync($"{context.User.Identity.Name}: {model.Name}");
        }

    }
}
