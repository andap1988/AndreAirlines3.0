using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AndreAirlinesAPI3._0Frontend.Data;
using AndreAirlinesAPI3._0Frontend.Utils;
using Microsoft.Extensions.Options;
using AndreAirlinesAPI3._0Frontend.Service;

namespace AndreAirlinesAPI3._0Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<AndreAirlinesAPI3_0FrontendContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AndreAirlinesAPI3_0FrontendContext")));

            services.Configure<AndreAirlinesDatabaseAirportSettings>(
                Configuration.GetSection(nameof(AndreAirlinesDatabaseAirportSettings)));

            services.AddSingleton<IAndreAirlinesDatabaseAirportSettings>(sp =>
                    sp.GetRequiredService<IOptions<AndreAirlinesDatabaseAirportSettings>>().Value);

            services.AddSingleton<AirportService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
