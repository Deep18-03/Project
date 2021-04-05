 using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Data;
using Microsoft.AspNetCore.Identity;
using Data.Repository.Interface;
using Business.Interface;
using Data.Repository;
using Business;
using System;
using Microsoft.AspNetCore.Http;
using DotnetCore.MiddleWare;

namespace DotnetCore
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
            services.AddResponseCaching();
            // Register Dbcontext 
            services.AddDbContext<AppDBContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Register Identity
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDBContext>();
            services.AddRazorPages();
            services.AddMvc();
            // Solve dependency for EmployeeManager and EmployeeRepository
            services.AddScoped(typeof(IEmployeeRepository), typeof(EmployeeRepository));
            services.AddTransient(typeof(IEmployeeManager), typeof(EmployeeManager));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseResponseCaching();
            // Custom Middleware for log response time
            app.UseResponseTimeMiddleware();
            // For the exception handling
            app.UseExceptionHandleMiddleware();
            // For caching
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Employee}/{action=List}");
            });
        }
    }
}
