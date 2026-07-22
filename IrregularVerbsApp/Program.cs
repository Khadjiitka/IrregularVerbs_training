using IrregularVerbsApp.Data;
using Microsoft.EntityFrameworkCore;

//  Создаем builder
var builder = WebApplication.CreateBuilder(args);

//  Добавляем сервисы (включая нашу БД SQLite)
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=verbs.db"));

//  Собираем приложение
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//  Автоматически создаем базу данных при запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();