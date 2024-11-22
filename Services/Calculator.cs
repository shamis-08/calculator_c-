using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyCalculatorApi.Data;
using MyCalculatorApi.Models;

namespace MyCalculatorApi.Services
{
    public class Calculator
    {
        private readonly CalculatorContext _context;
        private List<CalculatorStep> steps;
        private double currentResult;

        public Calculator(CalculatorContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();  // Создаем базу данных, если её нет

            steps = _context.Steps.OrderBy(s => s.StepNumber).ToList();
            currentResult = steps.LastOrDefault()?.Result ?? 0;
        }

        public CalculatorStep AddFirstOperand(double operand)
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
            return step;
        }

        public CalculatorStep PerformOperation(string operation, double operand)
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
                        throw new DivideByZeroException("Деление на ноль невозможно.");
                    currentResult /= operand;
                    break;
                default:
                    throw new InvalidOperationException("Неизвестная операция.");
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
            return step;
        }

        public CalculatorStep RevertToStep(int stepNumber)
        {
            if (stepNumber < 1 || stepNumber > steps.Count)
                throw new ArgumentOutOfRangeException(nameof(stepNumber), "Некорректный номер шага.");

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
            return step;
        }

        public void ClearSession()
        {
            _context.Steps.RemoveRange(_context.Steps);
            _context.SaveChanges();
            steps.Clear();
            currentResult = 0;
        }

        public bool IsFirstOperand() => steps.Count == 0;

        public IEnumerable<CalculatorStep> GetSteps() => steps.AsReadOnly();

        public double GetCurrentResult() => currentResult;
    }
}
