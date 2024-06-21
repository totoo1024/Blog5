using App.Core.Config;
using App.Core.Extensions;
using App.Framwork.DataValidation.Extensions;
using App.Framwork.DependencyInjection;
using App.Framwork.DependencyInjection.Extensions;
using App.Framwork.Mapper.Extensions;
using App.Hosting.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyCaching.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

string cacheProviderName = "default";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    //自动注入
    containerBuilder.RegisterModule<AutofacModule>();

    //自动注入service（业务层）
    containerBuilder.AutoRegisterService("service");

    containerBuilder.AddAspectCoreInterceptor(x => x.CacheProviderName = cacheProviderName);
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = _ => false;//关闭GDPR规范
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;

});
builder.Services.AddHttpContextAccessor();
builder.Services.AddConfig(builder.Configuration);
builder.Services.AddMvc().AddNewtonsoftJson(opt =>
{
    //忽略循环引用
    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

    //不改变字段大小
    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
});
builder.Services.AddControllersWithViews().AddValidation();//加入模型验证
builder.Services.AddMapper();//自动模型映射

SysConfig? sysConfig = builder.Services.BuildServiceProvider().GetService<IOptionsMonitor<SysConfig>>()?.CurrentValue;
builder.Services.AddEasyCaching(options =>
{
    ////使用文档 https://easycaching.readthedocs.io/en/latest
    if (sysConfig is { UseRedis: true })
    {
        options.UseCSRedis(builder.Configuration);
        cacheProviderName = EasyCachingConstValue.DefaultCSRedisName;
    }
    else
    {
        cacheProviderName = EasyCachingConstValue.DefaultInMemoryName;
        options.UseInMemory(builder.Configuration);
    }
    options.WithJson(cacheProviderName);

});
builder.Services.AddSqlSugarConnection(builder.Configuration);

//设置认证cookie名称、过期时间、是否允许客户端读取
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "appsoft";//cookie名称
    options.Cookie.HttpOnly = true;//不允许客户端获取
    options.SlidingExpiration = true;// 是否在过期时间过半的时候，自动延期
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseSession();
app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//使用静态Autofac容器
app.UseStaticContainer();

//异常处理中间件
app.UseExceptionHandle();

#region 解决Ubuntu Nginx 代理不能获取IP问题
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.Run();