using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using HomesicknessVisualiser.Repositories;
using HomesicknessVisualiser.Services;

namespace HonvagyVisualiser
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(builder =>
                builder.UseSqlServer("name=DefaultConnection")
            );
            var context = services.BuildServiceProvider().GetRequiredService<ApplicationContext>();
            context.Database.EnsureCreated();

            services.AddMvc();

            services.AddHttpClient("temperatureGetter", c =>
            {
                var finalUri = WeatherApiUriBuilder.Build();
                c.BaseAddress = new Uri(finalUri);
            });

            services.AddScoped<RecordService>();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            app.UseMvc();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
        }
    }
}
