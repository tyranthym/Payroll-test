using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayslipProcessorDraft.Constants;
using PayslipProcessorDraft.Mappers;
using PayslipProcessorDraft.Models;
using PayslipProcessorDraft.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PayslipProcessorDraft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Handle payslips")]
    public class PayslipController : ControllerBase
    {
        private readonly IPayslipService _payslipService;

        public PayslipController(IPayslipService payslipService)
        {
            this._payslipService = payslipService;
        }

        /// <summary>
        /// Generate payslip for employee
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Process([FromBody] PayslipInput input)
        {
            try
            {
                //failed model will auto-send 400 badrequest by api
                bool isValidInput = _payslipService.ValidateInputModel(input);

                if (!isValidInput)
                {
                    ErrorResponse errorResponse = new ErrorResponse("model is invalid or violate business rules, please change the input and try again.", ErrorTypes.BusinessRuleViolation);
                    return BadRequest(errorResponse);
                }

                PayslipOutput payslipOutput = _payslipService.GeneratePayslip(input);
                return Ok(payslipOutput);
            }
            catch (ArgumentNullException ex)
            {
                ErrorResponse errorResponse = new ErrorResponse(ex.Message, ErrorTypes.BusinessRuleViolation);
                return BadRequest(errorResponse);
            }
        }
    }
}