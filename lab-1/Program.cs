using System.Text.Json;

namespace MyCalculator
{
    // Класс, представляющий отдельный шаг вычислений
    public class CalculatorStep
    {
        public int StepNumber { get; set; }
        public string Operation { get; set; } // '+', '-', '*', '/'
        public double Operand { get; set; }
        public double Result { get; set; }
    }

    // Класс калькулятора
    public class Calculator
    {
        private List<CalculatorStep> steps;
        private double currentResult;

        public Calculator()
        {
            steps = new List<CalculatorStep>();
            currentResult = 0;
        }

        // Добавление первого операнда
        public void AddFirstOperand(double operand)
        {
            currentResult = operand;
            steps.Add(new CalculatorStep
            {
                StepNumber = steps.Count + 1,
                Operation = "Initial",
                Operand = operand,
                Result = currentResult
            });
            Console.WriteLine($"[#1] = {currentResult}");
        }

        // Выполнение операции
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
                        Console.WriteLine("Ошибка: Деление на ноль");
                        return;
                    }
                    currentResult /= operand;
                    break;
                default:
                    Console.WriteLine("Неизвестная операция.");
                    return;
            }

            steps.Add(new CalculatorStep
            {
                StepNumber = steps.Count + 1,
                Operation = operation,
                Operand = operand,
                Result = currentResult
            });

            Console.WriteLine($"[#${steps.Count}] = {currentResult}");
        }

        // Возврат к предыдущему шагу
        public void RevertToStep(int stepNumber)
        {
            if (stepNumber < 1 || stepNumber > steps.Count)
            {
                Console.WriteLine("Некорректный номер шага.");
                return;
            }

            currentResult = steps[stepNumber - 1].Result;
            steps.Add(new CalculatorStep
            {
                StepNumber = steps.Count + 1,
                Operation = $"Revert to #{stepNumber}",
                Operand = 0,
                Result = currentResult
            });

            Console.WriteLine($"[#${steps.Count}] = {currentResult}");
        }

        // Сохранение данных в JSON файл
        public void SaveSession(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(steps, options);
                File.WriteAllText(filePath, jsonString);
                Console.WriteLine($"Сессия сохранена в файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении сессии: {ex.Message}");
            }
        }

        // Загрузка данных из JSON файла (опционально)
        public void LoadSession(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    steps = JsonSerializer.Deserialize<List<CalculatorStep>>(jsonString);
                    if (steps.Count > 0)
                    {
                        currentResult = steps[steps.Count - 1].Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке сессии: {ex.Message}");
            }
        }

        // Проверка, является ли это первый операнд
        public bool IsFirstOperand()
        {
            return steps.Count == 0;
        }
    }

    abstract class Program
    {
        static void ShowUsage()
        {
            Console.WriteLine("Справка по использованию:");
            Console.WriteLine("если знак в строке ‘>’ – введите число");
            Console.WriteLine("если знак в строке ‘@’ – введите операцию");
            Console.WriteLine("операция может быть одной из ‘+’, ‘-’, ‘/’, ‘*’");
            Console.WriteLine("‘#’, за которым следует номер шага для возврата к предыдущему результату");
            Console.WriteLine("‘q’ для выхода");

        }

        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();
            ShowUsage();

            while (true)
            {
                if (calculator.IsFirstOperand())
                {
                    Console.Write("> ");  // Ожидание ввода числа
                }
                else
                {
                    Console.Write("@ ");  // Ожидание ввода операции
                }

                string input = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Пустой ввод. Пожалуйста, введите команду.");
                    continue;
                }

                if (input.ToLower() == "q")
                {
                    calculator.SaveSession("session.json");
                    Console.WriteLine("Выход из программы.");
                    break;
                }

                if (calculator.IsFirstOperand() && double.TryParse(input, out double inputOperand))
                {
                    // Ввод первого операнда
                    calculator.AddFirstOperand(inputOperand);
                }
                else if (input == "+" || input == "-" || input == "*" || input == "/")
                {
                    // Ввод операции без '@'
                    string operation = input;

                    Console.Write("> ");  // Запрос на ввод числа после операции
                    string operandInput = Console.ReadLine().Trim();

                    if (double.TryParse(operandInput, out double operationOperand))
                    {
                        calculator.PerformOperation(operation, operationOperand);
                    }
                    else
                    {
                        Console.WriteLine("Некорректный операнд. Пожалуйста, введите число.");
                    }
                }
                else if (input.StartsWith("#"))
                {
                    // Возврат к предыдущему шагу
                    string stepNumberStr = input.Substring(1).Trim();
                    if (int.TryParse(stepNumberStr, out int stepNumber))
                    {
                        calculator.RevertToStep(stepNumber);
                    }
                    else
                    {
                        Console.WriteLine("Некорректный номер шага.");
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод.");
                }
            }
        }
    }
}
