
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Financial.Chat.Domain.Core.CommandHandlers;
using MediatR;
using Financial.Chat.Domain.Shared.Notifications;
using Financial.Chat.Domain.Shared.Handler;
using Microsoft.AspNetCore.Authorization;

namespace Financial.Chat.Web.API.Controllers
{
    [ApiController]
    [Route("api/login")]
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        public LoginController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(AuthenticateUserCommand command)
        {
            var token = await _mediator.SendCommandResult(command);
            bool success = token != null;

            if (success)
            {
                return Response(token);
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route("Hello")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok("Hello world");
        }
    }
}