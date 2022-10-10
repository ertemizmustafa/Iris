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
        private readonly IStringLocalizer<HomeController> _stringLocalizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> stringLocalizer)
        {
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public string Get()
        {
            var aa = _stringLocalizer["ERROR_OCCURED"];
            var aa2 = _stringLocalizer["ERROR_OCCURED"];
            return _stringLocalizer["ERROR_OCCURED"];
        }
    }
}