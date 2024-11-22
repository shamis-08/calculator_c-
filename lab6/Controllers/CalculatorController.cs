using Microsoft.AspNetCore.Mvc;
using MyCalculatorApi.Models;
using MyCalculatorApi.Services;

namespace MyCalculatorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly Calculator _calculator;

        public CalculatorController(Calculator calculator)
        {
            _calculator = calculator;
        }
        /// Добавить первый операнд для начала расчета.
        [HttpPost("operand")]
        public ActionResult<CalculatorStep> AddFirstOperand([FromBody] OperandDto operandDto)
        {
            if (_calculator.IsFirstOperand())
            {
                var step = _calculator.AddFirstOperand(operandDto.Value);
                return Ok(step);
            }
            else
            {
                return BadRequest("Первый операнд уже установлен. Выполните операцию или вернитесь к предыдущему шагу.");
            }
        }

        /// Выполнить операцию (+, -, *, /) с заданным операндом.
        [HttpPost("operation")]
        public ActionResult<CalculatorStep> PerformOperation([FromBody] OperationDto operationDto)
        {
            try
            {
                var step = _calculator.PerformOperation(operationDto.Operation, operationDto.Operand);
                return Ok(step);
            }
            catch (DivideByZeroException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Вернуться к определенному шагу.
        [HttpPost("revert")]
        public ActionResult<CalculatorStep> RevertToStep([FromBody] RevertDto revertDto)
        {
            try
            {
                var step = _calculator.RevertToStep(revertDto.StepNumber);
                return Ok(step);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Получить все шаги расчета.
        [HttpGet("steps")]
        public ActionResult<IEnumerable<CalculatorStep>> GetSteps()
        {
            var steps = _calculator.GetSteps();
            return Ok(steps);
        }

        /// Получить текущий результат.
        [HttpGet("result")]
        public ActionResult<double> GetCurrentResult()
        {
            var result = _calculator.GetCurrentResult();
            return Ok(result);
        }

        /// Очистить текущую сессию расчета.
        [HttpDelete("clear")]
        public IActionResult ClearSession()
        {
            _calculator.ClearSession();
            return NoContent();
        }
    }

    // DTOs для тела запроса
    public class OperandDto
    {
        public double Value { get; set; }
    }

    public class OperationDto
    {
        public string Operation { get; set; } // "+", "-", "*", "/"
        public double Operand { get; set; }
    }

    public class RevertDto
    {
        public int StepNumber { get; set; }
    }
}
