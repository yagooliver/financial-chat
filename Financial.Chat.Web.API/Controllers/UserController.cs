﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Financial.Chat.Application.Interfaces;
using Financial.Chat.Application.SignalR;
using Financial.Chat.Domain.Core.Commands;
using Financial.Chat.Domain.Core.Commands.Message;
using Financial.Chat.Domain.Core.Interfaces.Services;
using Financial.Chat.Domain.Shared.Bot;
using Financial.Chat.Domain.Shared.Entity;
using Financial.Chat.Domain.Shared.Handler;
using Financial.Chat.Domain.Shared.Notifications;
using MassTransit;
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
        private IQueueMessageService _queueMessageService;
        public UserController(IUserService userService, IHubContext<ChatHub> chatHub, IQueueMessageService queueMessageService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _userService = userService;
            _chatHub = chatHub;
            _queueMessageService = queueMessageService;
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
            if (!string.IsNullOrEmpty(messageAddCommand.Consumer))
            {
                await _queueMessageService.SendMessageAsync(new MessageDto
                {
                    Consumer = messageAddCommand.Consumer,
                    Date = DateTime.Now,
                    Message = messageAddCommand.Message,
                    Sender = messageAddCommand.Sender
                });
            }
            else
                await _chatHub.Clients.Groups(messageAddCommand.Sender).SendAsync("ReceiveMessage", messageAddCommand.Sender, "Was not delivered. please, select an user");

            return Response();
        }

        [HttpPost("receive")]
        [Authorize]
        public async Task<IActionResult> ReceiveMessage([FromBody] MessageDto message)
        {
            var bot = new BotCall();
            if (bot.IsStockCall(message.Message))
            {
                var msg = bot.CallServiceStock(message.Message.Substring(7, message.Message.Length - 7));
                await _chatHub.Clients.Groups(message.Sender).SendAsync("ReceiveMessage", "Bot", msg);
                if (!string.IsNullOrEmpty(message.Consumer) && bot.VerifyResponse())
                    await _chatHub.Clients.Groups(message.Consumer).SendAsync("ReceiveMessage", "Bot", msg);
            }
            else
            {
                await _mediator.SendCommandResult(new MessageAddCommand { Consumer = message.Consumer, Message = message.Message, Sender = message.Sender });

                if (!string.IsNullOrEmpty(message.Consumer))
                {
                    await _chatHub.Clients.Groups(message.Consumer).SendAsync("ReceiveMessage", message.Sender, message.Message);
                }
                else
                    await _chatHub.Clients.Groups(message.Sender).SendAsync("ReceiveMessage", message.Sender, "Was not delivered. please, select an user");
            }
            return Response(true);
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