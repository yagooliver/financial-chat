using System;
using System.Linq;
using System.Threading.Tasks;
using Financial.Chat.Application.SignalR;
using Financial.Chat.Domain.Core.Commands;
using Financial.Chat.Domain.Core.Commands.Message;
using Financial.Chat.Domain.Core.Interfaces.Services;
using Financial.Chat.Domain.Shared.Bot;
using Financial.Chat.Domain.Shared.Entity;
using Financial.Chat.Domain.Shared.Handler;
using Financial.Chat.Domain.Shared.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;

namespace Financial.Chat.Web.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ApiController
    {
        private IUserService _userService;
        private IHubContext<ChatHub> _chatHub;
        public UserController(IUserService userService, IHubContext<ChatHub> chatHub, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _userService = userService;
            _chatHub = chatHub;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var users = await Task.Run(() => _userService.GetUsers());

            return Response(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await Task.Run(() => _userService.GetUser(id));

            return Response(user);
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await Task.Run(() => _userService.GetMessages());

            return Response(messages);
        }
        [HttpGet("messages/{email}")]
        public async Task<IActionResult> GetMessages(string email)
        {
            var messages = await Task.Run(() => _userService.GetMessages(email));
            if (messages.Any())
                return Response(messages.Select(x => new MessageDto
                {
                    Consumer = x.Consumer,
                    Message = x.Message,
                    Sender = x.Sender,
                    Date = x.Date
                }));
            return Response(messages);
        }

        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody]MessageAddCommand messageAddCommand)
        {
            var bot = new BotCall();
            if (bot.IsStockCall(messageAddCommand.Message))
            {
                var msg = bot.CallServiceStock(messageAddCommand.Message.Substring(7, messageAddCommand.Message.Length - 7));
                await _chatHub.Clients.Groups(messageAddCommand.Sender).SendAsync("ReceiveMessage", "Bot", msg);
                if(!string.IsNullOrEmpty(messageAddCommand.Consumer) && bot.VerifyResponse())
                    await _chatHub.Clients.Groups(messageAddCommand.Consumer).SendAsync("ReceiveMessage", "Bot", msg);
            }
            else
            {
                await _mediator.SendCommandResult(messageAddCommand);

                if (!string.IsNullOrEmpty(messageAddCommand.Consumer))
                {
                    await _chatHub.Clients.Groups(messageAddCommand.Consumer).SendAsync("ReceiveMessage", messageAddCommand.Sender, messageAddCommand.Message);
                }
                else
                    await _chatHub.Clients.Groups(messageAddCommand.Sender).SendAsync("ReceiveMessage", messageAddCommand.Sender, "Was not delivered. please, select an user");
            }

            return Response();
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]UserAddCommand command)
        {
            var result = await _mediator.SendCommandResult(command);

            if (result)
                await _chatHub.Clients.All.SendAsync("NewUserRegistered", new UserDto { Email = command.Email, Name = command.Name });

            return Response(result);
        }
    }
}