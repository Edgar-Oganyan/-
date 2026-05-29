using CatalogApp.Models;
using CatalogApp.Repositories;
using CatalogApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "products.json");
builder.Services.AddSingleton<IProductRepository>(new JsonProductRepository(dataPath));
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();


app.UseCors();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

Directory.CreateDirectory(Path.GetDirectoryName(dataPath)!);

app.Run();app.UseStaticFiles(); 
