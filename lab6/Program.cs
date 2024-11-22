<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> deef3dd (Лабораторная работа 6)
>>>>>>> cfc4c24 (Лабораторная работа 6)
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
<<<<<<< HEAD
app.Run();
=======
<<<<<<< HEAD
app.Run();
=======
app.Run();
=======
﻿// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
>>>>>>> 0ed2ff6 (Лабораторная работа 6)
>>>>>>> deef3dd (Лабораторная работа 6)
>>>>>>> cfc4c24 (Лабораторная работа 6)
