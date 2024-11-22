using Microsoft.EntityFrameworkCore;
using MyCalculatorApi.Data;
using MyCalculatorApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Настройка EF Core с SQLite
builder.Services.AddDbContext<CalculatorContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CalculatorDatabase") ?? "Data Source=calculator.db"));

// Регистрация Calculator как сервис с областями
builder.Services.AddScoped<Calculator>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Обеспечиваем создание базы данных при запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CalculatorContext>();
    db.Database.EnsureCreated();
}

// Настройка конвейера обработки HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();