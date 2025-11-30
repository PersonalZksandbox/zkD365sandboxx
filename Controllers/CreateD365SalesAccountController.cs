using Microsoft.AspNetCore.Mvc;
using zkD365tryout.infrastructure;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using zkD365tryout.infrastructure.Interface;
namespace zkD365tryout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateD365SalesAccountController : ControllerBase
    {
        private readonly ILogger<CreateD365SalesAccountController> _logger;
        private readonly ID365Sales _dynamicsCrmService;
        public CreateD365SalesAccountController(ID365Sales dynamicsCrmService, ILogger<CreateD365SalesAccountController> logger)
        {
            _logger = logger;
            _dynamicsCrmService = dynamicsCrmService;
        }

        [HttpPost(Name = "CreateSalesAccount")]
        public async Task<IActionResult> CreateSalesAccount(string account)
        {
            var token = await _dynamicsCrmService.GetAccessTokenAsync();
            var location = await _dynamicsCrmService.CreateD365SalesAccountName(token,account);
            return Ok(token);

        }
    }
}
