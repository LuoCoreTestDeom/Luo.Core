using Luo.Core.Common;
using Luo.Core.EnumModels;
using Luo.Core.FiltersExtend.PolicysHandlers;
using Luo.Core.Utility.ApplicationExtend;
using Luo.Core.Utility.ServiceExtensions;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//clientId/clientIp解析器使用它。
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(new Appsettings(builder.Configuration));

builder.Services.AddCorsSetup();
builder.Services.AddHttpPollySetup();


builder.Services.AddSwaggerService<ApiVersionEnum>();
builder.Services.AddCorsSetup();
builder.Services.AddDistributedCacheSteup();
builder.Services.AddSqlSugarSetup();
builder.Services.AddBatchService("Luo.Core.Services");
builder.Services.AddBatchService("Luo.Core.Repository");

builder.Services.AddAutoMapperSetup();
builder.Services.AddJWTService();

builder.Services.AddSession(options =>
{
    var sessTime = Appsettings.GetObject<double>("SessionIdleTimeoutMinutes");
    if (sessTime > 0)
    {
        options.IdleTimeout = TimeSpan.FromMinutes(sessTime);
    }

});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

var app = builder.Build();
app.UseSession();
app.UseCors();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwaggerMiddle<ApiVersionEnum>();

app.UseHttpsRedirection();

app.UseAuthentication();        // 注意这里
app.UseAuthorization();

app.MapControllers();

app.Run();
