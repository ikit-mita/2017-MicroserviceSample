using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MisroserviceSample.Models;
using MisroserviceSample.Options;
using MisroserviceSample.Services;
using Newtonsoft.Json;

namespace MisroserviceSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataService, DataService>();
            services.AddOptions();
            services.Configure<DataOptions>(Configuration.GetSection("Data"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(Timer);
            app.UseStaticFiles();
            app.Use(ValidateMethod);

            app.MapMethod(HttpMethods.Get, ProcessGet);
            app.MapMethod(HttpMethods.Post, ProcessPost);
        }

        private void ProcessPost(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                var service = context.RequestServices.GetService<IDataService>();
                var model = await context.Request.ReadJsonAsync<Model>();
                await service.SaveDataAsync(model.Name);
            });
        }

        private void ProcessGet(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                var service = context.RequestServices.GetService<IDataService>();
                string data = await service.GetDataAsync();
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                    Name = data
                }));
            });
        }

        private Task ValidateMethod(HttpContext context, Func<Task> next)
        {
            if (HttpMethods.IsGet(context.Request.Method) ||
                HttpMethods.IsPost(context.Request.Method))
            {
                return next();
            }
            context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
            return Task.CompletedTask;
        }

        private async Task Timer(HttpContext context, Func<Task> next)
        {
            var sw = new Stopwatch();
            sw.Start();
            context.Response.OnStarting(resp =>
            {
                ((HttpResponse)resp).Headers.Add("X-Elapsed", $"{sw.ElapsedMilliseconds}");
                return Task.CompletedTask;
            }, context.Response);

            await next();
        }

    }
}
