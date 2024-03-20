using HospitalManagement.Repository.DataClass;
using HospitalManagement.Repository.DataClass.Doctors;
using HospitalManagement.Repository.DataClass.Master;
using HospitalManagement.Repository.DataClass.Patient;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Repository.Interface.Master;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static HospitalManagement.Repository.DataClass.Master.clsSystemUser;

namespace HospitalManagement
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
            services.Configure<IISOptions>(options => { options.AutomaticAuthentication = false; });
            services.AddCors();
            services.AddScoped<IMaster, clsMaster>();            
            services.AddScoped<ISystemUser, clsSystemUsers>();            
            services.AddScoped<IDoctors, clsDoctor>();            
            services.AddScoped<IPatient, clsPatient>();
            services.AddControllers();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            app.UseExceptionHandler("/Home/Index");
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "DocImage")),
                RequestPath = "/DocImage"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "PatientImage")),
                RequestPath = "/PatientImage"
            });
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
