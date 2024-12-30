using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TaskManager.Db;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<ToDoListContext>(options =>
    //options.UseNpgsql(builder.Configuration.GetConnectionString("TooDooListContext") ?? throw new InvalidOperationException("Connection string 'TooDooListContext' not found.")));
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("TooDooListContext"), true);

// Добавляем сервисы в контейнер зависимостей
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Настройка конвейера обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Включаем Swagger и Swagger UI
app.UseSwagger(); 
app.UseSwaggerUI(c => 
{ 
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); 
    c.RoutePrefix = string.Empty; // Это сделает Swagger UI доступным по URL корня приложения });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TodoItems}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
