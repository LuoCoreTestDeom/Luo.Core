using Luo.Core.DatabaseFactory;
using Luo.Core.Common;
using Luo.Core.Utility.ServiceExtensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton(new Appsettings(builder.Configuration));
builder.Services.AddDistributedCacheSteup();
builder.Services.AddSqlSugarSetup();
builder.Services.AddBatchService("Luo.Core.Services");
builder.Services.AddBatchService("Luo.Core.Repository");
builder.Services.AddAuthCookieSetup();
//builder.Services.InitEntityData();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();//路由中间件
app.UseAuthentication();//认证中间件
app.UseAuthorization();//授权中间件

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Main}/{id?}");

app.Run();