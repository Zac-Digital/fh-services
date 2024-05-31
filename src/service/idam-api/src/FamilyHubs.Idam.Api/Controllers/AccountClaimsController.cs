using FamilyHubs.Idam.Core.Commands.Add;
using FamilyHubs.Idam.Core.Commands.Delete;
using FamilyHubs.Idam.Core.Commands.Update;
using FamilyHubs.Idam.Core.Queries.GetAccountClaims;
using FamilyHubs.Idam.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FamilyHubs.Idam.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountClaimsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountClaimsController> _logger;

        public AccountClaimsController(IMediator mediator, ILogger<AccountClaimsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAccountClaimsByEmail")]
        public async Task<List<AccountClaim>> GetAccountClaimsByEmail(string email, CancellationToken cancellationToken)
        {
            var command = new GetAccountClaimsByEmailCommand { Email = email };
            
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Any())
            {
                _logger.LogWarning("Result from mediator has no records");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }

            return result;
        }

        [HttpPost]
        [Route("AddClaim")]
        public async Task<long> AddClaims([FromBody] AddClaimCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            
            return result;
        }

        [HttpPut]
        [Route("UpdateClaim")]
        public async Task<long> UpdateClaims([FromBody] UpdateClaimCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            
            return result;
        }

        [HttpDelete]
        [Route("DeleteClaim")]
        public async Task<bool> DeleteClaims([FromBody] DeleteClaimCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            
            return result;
        }

        [HttpDelete]
        [Route("DeleteAllClaims")]
        public async Task<bool> DeleteAllClaims([FromBody] DeleteAllClaimsCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            
            return result;
        }
    }
}