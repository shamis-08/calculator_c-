namespace MyCalculatorApi.Models
{
    public class CalculatorStep
    {
        public int Id { get; set; }           // Первичный ключ для EF Core
        public int StepNumber { get; set; }   // Номер шага
        public string Operation { get; set; } // Операция (‘+’, ‘-’, ‘*’, ‘/’)
        public double Operand { get; set; }   // Операнд
        public double Result { get; set; }    // Результат после операции
    }
}