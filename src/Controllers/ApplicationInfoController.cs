using Jaxofy.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Jaxofy.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ApplicationInfoController : ApiBaseController
    {
        [HttpGet]
        public ActionResult GetInfo()
        {
            return Ok(new {success=true});
        }
    }
}