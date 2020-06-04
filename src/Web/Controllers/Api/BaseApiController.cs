using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected string GetToken()
        {
           return Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        }
    }
}
