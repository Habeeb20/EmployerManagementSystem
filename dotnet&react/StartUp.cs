using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using dotnet_react.Data;



namespace WebAPI
{
    public class StartUp(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Connect database
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString( "EmployeeAppCon")));

            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<DataContext>();
            //Enable CORS

            services.AddCors(options => {
                var frontendURL = Configuration.GetValue<string>("frontend");

                options.AddDefaultPolicy(services => {
                    services.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
                });

            });
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                 .AllowAnyHeader());
            });

            //JSON Serializer
            services.AddControllersWithViews();
                // // .AddNewtonsoftJson(options =>
                // // options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                // // .Json.ReferenceLoopHandling.Ignore)
                // .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
                // = new DefaultContractResolver());

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseStaticFiles(new StaticFileOptions
            {
                // FileProvider = new PhysicalFileProvider(
                //     Path.Combine(Directory.GetCurrentDirectory(),"Photos")),
                // RequestPath="/Photos"
            });
        }
    }
}
