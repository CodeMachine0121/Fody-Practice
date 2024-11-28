using Microsoft.AspNetCore.Mvc;

namespace DemoApplication
{
    [Route("[controller]")]
    public class DemoController: ControllerBase
    {
        [HttpGet]
        public string Demo()
        {
            var demoService = new DemoService();
            demoService.Echo();
            return  "";
        }
        
    }
}