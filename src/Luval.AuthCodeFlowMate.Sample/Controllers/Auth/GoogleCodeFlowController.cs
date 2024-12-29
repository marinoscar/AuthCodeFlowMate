using Luval.AuthCodeFlowMate.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luval.AuthCodeFlowMate.Sample.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleCodeFlowController : ControllerBase
    {
        private readonly GoogleAuthCodeFlowService _service;

        public GoogleCodeFlowController(GoogleAuthCodeFlowService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }


        public IActionResult Index()
        {
            return Ok("Google Code Flow");
        }   

        [HttpGet("getcode")]
        public IActionResult GetCode()
        {
            var loginUrl = _service.CreateAuthorizationConsentUrl();
            return Redirect(loginUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string? error)
        {
            var token = await _service.CretaeAuthorizationCodeRequestAsync(code, error);
            //var integration = await _service.PersistIntegrationAsync(token);
            return Ok(token);
        }

    }
}
