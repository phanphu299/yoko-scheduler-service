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
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/execution")]
        public async Task<IActionResult> GetJobExecutionAsync([FromRoute] string id)
        {
            var command = new GetJobExecutionById(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("recurring")]
        public async Task<IActionResult> AddRecurringJobAsync([FromBody] UpsertRecurringJob command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}/recurring")]
        public async Task<IActionResult> UpdateRecurringJobAsync([FromRoute] string id, [FromBody] UpsertRecurringJob command)
        {
            command.Id = id;
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}/recurring")]
        public async Task<IActionResult> DeleteRecurringJobAsync(string id)
        {
            var command = new DeleteRecurringJob(new string[] { id });
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}