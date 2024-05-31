using FamilyHubs.Idam.Core.Commands.Add;
using FamilyHubs.Idam.Core.Commands.Delete;
using FamilyHubs.Idam.Core.Commands.Update;
using FamilyHubs.Idam.Core.Models;
using FamilyHubs.Idam.Core.Queries.GetUserSession;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FamilyHubs.Idam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSessionController : Controller
    {
        private readonly IMediator _mediator;

        public UserSessionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<string> Create([FromBody] AddUserSessionCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            _ = _mediator.Send(new DeleteExpiredUserSessionsCommand(), cancellationToken);//do not await
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            HttpContext.Response.Headers.Add("Location", $"Api/UserSession/{result}");
            return result;
        }

        [HttpGet("{sid}")]
        public async Task<UserSessionDto?> GetById(string sid, CancellationToken cancellationToken)
        {
            var command = new GetUserSessionCommand { Sid = sid };
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
            return result;
        }

        [HttpGet]
        public async Task<UserSessionDto?> GetByEmail(string? email, CancellationToken cancellationToken)
        {
            var command = new GetUserSessionCommand { Email = email };
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
            return result;
        }

        [HttpDelete("{sid}")]
        public async Task<bool> Delete(string sid, CancellationToken cancellationToken)
        {
            var command = new DeleteUserSessionCommand { Sid = sid };
            var result = await _mediator.Send(command, cancellationToken);
            return result;
        }

        [HttpDelete()]
        [Route("DeleteAllUserSessions/{email}")]
        public async Task DeleteAllUserSessions(string email, CancellationToken cancellationToken)
        {
            var command = new DeleteAllUserSessionsCommand { Email = email };
            await _mediator.Send(command, cancellationToken);
        }

        [HttpPut("{sid}")]
        public async Task<string> Refresh(string sid, CancellationToken cancellationToken)
        {
            var command = new UpdateUserSessionCommand { Sid = sid };
            var result = await _mediator.Send(command, cancellationToken);
            return result;
        }
    }
}
