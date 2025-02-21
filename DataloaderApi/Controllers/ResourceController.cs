using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataloaderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        [Route("verify")]

        public ActionResult verify()
        {
             return Ok("you Are authorized");
        }


    }
}
