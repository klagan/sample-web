using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dynamic.Endpoint
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options => { options.DefaultScheme = "cookie1"; })
                .AddCookie("cookie1", "cookie1", options =>
                {
                    options.Cookie.Name = "cookie1";
                    options.LoginPath = "/loginc1";
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

             app.Use(next =>
             {
                 return async ctx =>
                 {
                     switch(ctx.Request.Path)
                     {
                         case "/loginc1":
                             var identity1 = new ClaimsIdentity("cookie1");
                             identity1.AddClaim(new Claim("name", "Alice-c1"));
                             await ctx.SignInAsync("cookie1", new ClaimsPrincipal(identity1));
                             break;
                         case "/loginc2":
                             var identity2 = new ClaimsIdentity("cookie2");
                             identity2.AddClaim(new Claim("name", "Alice-c2"));
                             await ctx.SignInAsync("cookie2", new ClaimsPrincipal(identity2));
                             break;
                         case "/logoutc1":
                             await ctx.SignOutAsync("cookie1");
                             break;
                         case "/logoutc2":
                             await ctx.SignOutAsync("cookie2");
                             break;
                         case "/kam":
                             await ctx.Response.WriteAsync("Hello!  This is dynamic!");
                             break;
                         default:
                             await next(ctx);
                             break;
                     }
                 };
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
