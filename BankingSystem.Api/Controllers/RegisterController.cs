using BankingSystem.Core.Interfaces;
using BankingSystem.Core.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IOperatorServices _operatorServices;

        public RegisterController(IOperatorServices operatorServices)
        {
            _operatorServices = operatorServices;
        }

        [HttpPost]
        public IActionResult Register([FromBody] CreateOperatorRequest request)
        {

           var result = _operatorServices.RegisterOperator(request);

            return Ok(result);
        }

    }
}
