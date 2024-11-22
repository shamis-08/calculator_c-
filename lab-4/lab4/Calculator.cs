using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyCalculatorApp
{
    public class Calculator
    {
        private readonly CalculatorContext _context;
        private List<CalculatorStep> steps;
        private double currentResult;

        public Calculator()
        {
            var options = new DbContextOptionsBuilder<CalculatorContext>()
                .UseSqlite("Data Source=calculator.db")
                .Options;

            _context = new CalculatorContext(options);
            _context.Database.EnsureCreated();  // Создаем базу данных, если её нет

            steps = _context.Steps.OrderBy(s => s.StepNumber).ToList();
            currentResult = steps.LastOrDefault()?.Result ?? 0;
        }

        public void AddFirstOperand(double operand)
        {
            currentResult = operand;
            var step = new CalculatorStep
            {
                StepNumber = steps.Count + 1,
                Operation = "Initial",
                Operand = operand,
                Result = currentResult
            };
            steps.Add(step);
            _context.Steps.Add(step);
            _context.SaveChanges();
            Console.WriteLine($"[#1] = {currentResult}");
        }

        public void PerformOperation(string operation, double operand)
        {
            switch (operation)
            {
                case "+":
                    currentResult += operand;
                    break;
                case "-":
                    currentResult -= operand;
                    break;
                case "*":
                    currentResult *= operand;
                    break;
                case "/":
                    if (operand == 0)
                    {
                        Console.WriteLine("Ошибка: Деление на ноль.");
                        return;
                    }
                    currentResult /= operand;
                    break;
                default:
                    Console.WriteLine("Неизвестная операция.");
                    return;
            }

            var step = new CalculatorStep
            {
                StepNumber = steps.Count + 1,
                Operation = operation,
                Operand = operand,
                Result = currentResult
            };
            steps.Add(step);
            _context.Steps.Add(step);
            _context.SaveChanges();
            Console.WriteLine($"[#${steps.Count}] = {currentResult}");
        }

        public void RevertToStep(int stepNumber)
        {
            if (stepNumber < 1 || stepNumber > steps.Count)
            {
                Console.WriteLine("Некорректный номер шага.");
                return;
            }

            currentResult = steps[stepNumber - 1].Result;
            var step = new CalculatorStep
            {
                StepNumber = steps.Count + 1,
                Operation = $"Revert to #{stepNumber}",
                Operand = 0,
                Result = currentResult
            };
            steps.Add(step);
            _context.Steps.Add(step);
            _context.SaveChanges();
            Console.WriteLine($"[#${steps.Count}] = {currentResult}");
        }

        public void ClearSession()
        {
            _context.Steps.RemoveRange(_context.Steps);
            _context.SaveChanges();
            steps.Clear();
            currentResult = 0;
            Console.WriteLine("Сессия очищена.");
        }

        public bool IsFirstOperand() => steps.Count == 0;
    }
}
