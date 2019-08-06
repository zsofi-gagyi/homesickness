using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MemoryStorage;
using HomesicknessVisualiser.Controllers;
using HomesicknessVisualiser.Repositories;
using HomesicknessVisualiser.Services;

namespace HonvagyVisualiser
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddHttpClient("temperatureGetter", c =>
            {
                var finalUri = WeatherApiUriBuilder.Build();
                c.BaseAddress = new Uri(finalUri);
            });

            services.AddHangfire(configuration => configuration
                    .UseMemoryStorage());
            services.AddHangfireServer();

            services.AddDbContext<ApplicationContext>(builder => builder
               .UseMySQL($"server={Environment.GetEnvironmentVariable("HvHOST")};" +
                         $"database={Environment.GetEnvironmentVariable("HvDATABASE")};" +
                         $"user={Environment.GetEnvironmentVariable("HvUSERNAME")};" +
                         $"password={Environment.GetEnvironmentVariable("HvPASSWORD")}"
                         )
               );

            services.AddScoped<RecordService>();

            services.AddScoped<TemperatureAsker>();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            app.UseMvc();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseHangfireDashboard();
            var temperatureAsker = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<TemperatureAsker>();
            RecurringJob.AddOrUpdate(() => temperatureAsker.Ask(), Cron.Hourly);
        }
    }
}
