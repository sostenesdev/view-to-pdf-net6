using DinkToPdf.Contracts;
using DinkToPdf;
using Rotativa.AspNetCore;
using TesteRotativa.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//DinkToPDF
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();


var app = builder.Build();
IWebHostEnvironment env = app.Environment;




// Configurações do DinkToPDF
// Verifica qual a arquiterura para utilizar os arquivos necessários
var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
// Encontra o caminho onde estão os arquivos
//var wkHtmlToPdfPath = Path.Combine(env.ContentRootPath, $"v0.12.4\\{architectureFolder}\\libwkhtmltox");
var wkHtmlToPdfPath = Path.Combine(env.ContentRootPath, $"wwwroot\\wkhtmltopdf\\bin\\wkhtmltox");
// Carrega os arquivos necessários, passadas as configurações
CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(wkHtmlToPdfPath);



//Rotativa
RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
