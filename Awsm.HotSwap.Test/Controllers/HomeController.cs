using Microsoft.AspNetCore.Mvc;

namespace Awsm.HotSwap.Test.Controllers
{
    [ApiController]
    [Route(("/"))]
    public class HomeController : ControllerBase
    {
        private readonly IStringService srv;
        public HomeController(IStringService srv)
        {
            this.srv = srv;
        }
        
        [HttpGet]
        public string Index()
        {
            return srv.FormatString("Hello, World!");
        }
    }
}