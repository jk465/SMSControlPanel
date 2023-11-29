using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMSControlPanel.BusinessLogic;
using SMSControlPanel.DataLayer;
using SMSControlPanel.Repository;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Repository.DataLayer;
using SMSControlPanel.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserLogger;

namespace SMSControlPanel
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
            services.AddSingleton(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserLogonDL, UserLogonDL>();
            services.AddScoped<IUserLogonBL, UserLogonBL>();
            services.AddScoped<IDeptUserBL, DeptUserBL>();
            services.AddScoped<IDeptUserDL, DeptUserDL>();
            services.AddScoped<IGetDistListBL,GetDistListBL>();
            services.AddScoped<IGetDistListDL, GetDistListDL>(); 
            services.AddScoped<IVirginmediasmsDL, VirginmediasmsDL>(); 
            services.AddScoped<IVirginmediasmsBL, VirginmediasmsBL>(); 
            services.AddScoped<IGenerateReportBL, GenerateReportBL>(); 
            services.AddScoped<IGenerateReportDL, GenerateReportDL>(); 
            services.AddScoped<IMsgDetailsBL, MsgDetailsBL>(); 
            services.AddScoped<IMsgDetailsDL, MsgDetailsDL>(); 
            services.AddScoped<ISendSMS, SendSMS>(); 
            services.AddScoped<IAdminBL, AdminBL>(); 
            services.AddScoped<IAdminDL, AdminDL>();
            services.AddScoped<IUserLogger, UserInfoLogger>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(8);
            });

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo"; // Optional: Set the schema name
                options.TableName = "SMSControlPanelSessionData"; // Optional: Set the table name
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Base}/{action=Initial}/{id?}");
            });
        }
    }
}
