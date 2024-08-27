using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Scheduler.Application.Command;
using MediatR;

namespace Scheduler.Api.Controller
{
    [Route("sch/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "oidc")]
    public class CronExpressionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CronExpressionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchCronExpressionsAsync([FromBody] SearchCronExpression command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}