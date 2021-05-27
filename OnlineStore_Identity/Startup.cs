using BlazorDownloadFile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using OnlineStore_Identity.Data;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OnlineStore_Identity
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>{ 
            options.SignIn.RequireConfirmedAccount = false;
            //// Your Cookie settings
            //options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(1);
            //options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
            //options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut";
            }).AddEntityFrameworkStores<ApplicationDbContext>()/*.AddDefaultTokenProviders()*/;



            services.AddControllersWithViews();
            services.AddRazorPages();
            // needed to make the razor component's event firing works
            services.AddServerSideBlazor();
            services.AddBlazorDownloadFile();

            //services.AddKendo();
            //services.AddControllers()
            //.AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //});

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //});

            services.ConfigureApplicationCookie(c =>
            { c.LoginPath = "/Identity/Account/Login";
                c.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //c.Events.OnRedirectToLogin = context =>
                //{
                //    context.Response.StatusCode = 401;
                //    return Task.CompletedTask;
                //};
            });
            //services.AddMvc().AddJsonOptions(options => {
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //});

            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true; options.Cookie.IsEssential = true;
            });
            services.AddMvc().AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("/{0}.cshtml");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                // needed to make the razor component's event firing works
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                ///////////////////////////////
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

            RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);
        }
    }
}