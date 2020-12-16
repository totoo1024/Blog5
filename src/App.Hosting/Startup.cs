using System;
using System.Reflection;
using App.Core.Config;
using App.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using App.Framwork.DataValidation.Extensions;
using App.Framwork.DependencyInjection;
using App.Framwork.DependencyInjection.Extensions;
using App.Framwork.Generate.Geetest;
using App.Framwork.Mapper.Extensions;
using App.Hosting.Extensions;
using Autofac;
using EasyCaching.Core;
using EasyCaching.Core.Configurations;
using EasyCaching.CSRedis;
using EasyCaching.InMemory;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Hosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        string cacheProviderName = "default";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //������ͼʵʱ��Ч
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;//�ر�GDPR�淶
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //���Session ����
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;

            });
            services.AddHttpContextAccessor();

            //�Զ�ע������
            services.AddConfig(Configuration);

            services.AddMvc().AddNewtonsoftJson(opt =>
            {
                //����ѭ������
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                //���ı��ֶδ�С
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddControllersWithViews().AddValidation();//����ģ����֤
            services.AddMapper();//�Զ�ģ��ӳ��
            //services.AddAutoDependencyInjection();//�Զ�ע�루���·��� builder.RegisterModule<AutofacModule>(); ��ѡһ���ɣ�

            #region ��������

            var sysConfig = services.BuildServiceProvider().GetService<SysConfig>();
            services.AddEasyCaching(options =>
            {
                ////ʹ���ĵ� https://easycaching.readthedocs.io/en/latest
                if (sysConfig.UseRedis)
                {
                    options.UseCSRedis(Configuration);
                    cacheProviderName = EasyCachingConstValue.DefaultCSRedisName;
                }
                else
                {
                    cacheProviderName = EasyCachingConstValue.DefaultInMemoryName;
                    options.UseInMemory(Configuration);
                }
                options.WithJson(cacheProviderName);

            });

            #endregion

            #region �������ݿ�����
            //֧�ֶ���������
            services.AddSqlSugarConnection(Configuration);
            #endregion

            #region cookieȫ������

            //������֤cookie���ơ�����ʱ�䡢�Ƿ�����ͻ��˶�ȡ
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "appsoft";//cookie����
                options.Cookie.HttpOnly = true;//������ͻ��˻�ȡ
                options.SlidingExpiration = true;// �Ƿ��ڹ���ʱ������ʱ���Զ�����
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseSession();
            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //ʹ�þ�̬Autofac����
            app.UseStaticContainer();

            //�쳣�����м��
            app.UseExceptionHandle();

            #region ���Ubuntu Nginx �����ܻ�ȡIP����
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            #endregion

            app.UseAuthentication();
            app.UseAuthorization();

            #region ·������

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            #endregion
        }

        /// <summary>
        /// Autofac��������
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //�Զ�ע��
            builder.RegisterModule<AutofacModule>();

            //�Զ�ע��service��ҵ��㣩
            builder.AutoRegisterService("service");

            //ʹ��AspectCore���붯̬������������ע���м�Autofac.Extensions.DependencyInjection��nuget��������6.0�汾���������쳣
            builder.AddAspectCoreInterceptor(x => x.CacheProviderName = cacheProviderName);
        }
    }
}
