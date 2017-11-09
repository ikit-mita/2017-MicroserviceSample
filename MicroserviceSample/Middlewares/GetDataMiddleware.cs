using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroserviceSample.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MicroserviceSample.Middlewares
{
    public class GetDataMiddleware
    {
        public GetDataMiddleware(RequestDelegate next) { }

        public async Task Invoke(HttpContext context, IDataService dataService)
        {
            string data = await dataService.GetDataAsync();
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Name = data,
                User = context.User.Identity.IsAuthenticated
                    ? context.User.Identity.Name
                    : "anon"
            }));
        }
    }
}
