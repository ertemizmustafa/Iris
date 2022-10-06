using Iris.AspNetCore.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Business.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[CoreApiFilter]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<string> _stringLocalizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<string> stringLocalizer)
        {
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public string Get()
        {
            return _stringLocalizer["ERROR_OCCURED"];
        }
    }
}