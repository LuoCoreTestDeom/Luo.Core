using Luo.Core.DatabaseFactory;
using Luo.Core.Utility;
using Luo.Core.Utility.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(new Appsettings(builder.Configuration));
builder.Services.AddSqlSugarSetup();
builder.Services.AddBatchService("Luo.Core.Services");
builder.Services.AddBatchService("Luo.Core.Repository");

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Main}/{id?}");

var initDatabase = builder.Services.BuildServiceProvider().GetService<ISqlSugarInitDatabase>();
initDatabase.CreateDatabase();
initDatabase.CreateDatabaseTables("Luo.Core.DatabaseEntity");
string executDirPath = Directory.GetCurrentDirectory();
initDatabase.CreateDatabaseEntityFile(executDirPath.Substring(0, executDirPath.IndexOf("\\Luo.Core.LayuiAdmin")), "Luo.Core.DatabaseEntity");
//var ddd = builder.Services.BuildServiceProvider().GetService<Luo.Core.IRepository.IDatabaseInitRepository>();
app.Run();