namespace MyCalculatorApp
{
    class Program
    {
        static void ShowUsage()
        {
            Console.WriteLine("Справка по использованию:");
            Console.WriteLine("> для ввода числа");
            Console.WriteLine("+, -, *, / для операций");
            Console.WriteLine("#N для возврата к шагу N");
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
                    Console.WriteLine("Выход из программы.");
                    break;
                }

                if (calculator.IsFirstOperand() && double.TryParse(input, out double inputOperand))
                {
                    calculator.AddFirstOperand(inputOperand);
                }
                else if (input == "+" || input == "-" || input == "*" || input == "/")
                {
                    string operation = input;
                    Console.Write("> ");
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
