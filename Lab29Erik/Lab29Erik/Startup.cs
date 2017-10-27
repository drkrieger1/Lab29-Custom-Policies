using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Lab29Erik.Models;
using Lab29Erik.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Lab29Erik
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie("MyCookieLogin", options =>
                options.AccessDeniedPath = new PathString("/Accounts/AccessDenied/"));

            services.AddMvc();

            services.AddAuthorization(options => {
                options.AddPolicy("Admin Only", policy => policy.RequireClaim("Administrator"));

            });

            services.AddAuthorization(options => {
                options.AddPolicy("LikesDogs", policy => policy.Requirements.Add(new MustLikeDogs()));

            });

            services.AddAuthorization(options => { 
                options.AddPolicy("Registered User", policy => policy.RequireClaim("RegisteredUser"));
                options.AddPolicy("MinAge", policy => policy.Requirements.Add(new MinimumAgeRequierment()));
            });

            services.AddSingleton<IAuthorizationHandler, LikesDogs>();
            services.AddSingleton<IAuthorizationHandler, Is21>();


            services.AddDbContext<Lab29ErikContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Lab29ErikContext")));

            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("Lab29ErikContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                context.Response.Redirect("/Accounts/AccessDenied", false);
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
