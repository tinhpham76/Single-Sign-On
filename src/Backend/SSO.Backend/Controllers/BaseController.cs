using Microsoft.AspNetCore.Mvc;

namespace SSO.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("Bearer")]
    public class BaseController : ControllerBase
    {
    }
}