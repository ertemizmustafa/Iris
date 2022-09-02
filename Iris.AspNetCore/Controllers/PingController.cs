using Microsoft.AspNetCore.Mvc;

namespace Iris.AspNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {

        private readonly ILogger<PingController> _logger;

        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            object? response = null;

            await Task.Run(() =>
             {
                 response = new
                 {
                     Message = "pong",
                     SystemDateTime = DateTime.UtcNow,
                     UpTime = "", //AppSettings.Current.GetUptime().ToFriendlyString(),
                     AppSettings = "", //AppSettings.Current,
                     Versions = new Dictionary<string, string>()
                 };
             });


            //var userAssembliesOrderedByName = AssemblyManager.UserAssemblies.Select(assembly => new
            //{
            //    Name = assembly.GetName().Name,
            //    Version = assembly.GetName().Version.ToString()
            //}
            //).OrderBy(a => a.Name);

            //foreach (var assembly in userAssembliesOrderedByName)
            //{
            //    response.Versions.Add(assembly.Name, assembly.Version);
            //}

            return Accepted(response);
        }
    }
}