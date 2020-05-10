using AutoMapper;
using Financial.Chat.Domain.Core.Commands;
using Financial.Chat.Domain.Core.Commands.Message;
using Financial.Chat.Domain.Core.Entity;
using Financial.Chat.Domain.Core.Interfaces;
using Financial.Chat.Domain.Core.Interfaces.Services;
using Financial.Chat.Domain.Shared.Handler;
using Financial.Chat.Domain.Shared.Notifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Financial.Chat.Domain.Core.CommandHandlers
{
    public class UserHandler : IRequestHandler<UserAddCommand, bool>, IRequestHandler<MessageAddCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;

        public UserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMediatorHandler mediatorHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
        }

        public Task<bool> Handle(UserAddCommand request, CancellationToken cancellationToken)
        {
            if (request.IsValid())
            {
                try
                {
                    var user = _mapper.Map<User>(request);
                    _userRepository.Add(user);
                    var success = _unitOfWork.Commit();

                    return Task.FromResult(success);
                }
                catch(Exception e)
                {
                    _mediatorHandler.RaiseEvent(new DomainNotification("exception", e.Message));
                }
            }
            else
            {
                foreach (var error in request.GetErrors())
                    _mediatorHandler.RaiseEvent(new DomainNotification(error.ErrorCode, error.ErrorMessage));
            }

            return Task.FromResult(false);
        }

        public Task<bool> Handle(MessageAddCommand request, CancellationToken cancellationToken)
        {
            if (request.IsValid())
            {
                try
                {
                    var message = new Messages(request.Message, request.Sender, request.Consumer);
                    _userRepository.Add(message);
                    var success = _unitOfWork.Commit();

                    return Task.FromResult(success);
                }
                catch (Exception e)
                {
                    _mediatorHandler.RaiseEvent(new DomainNotification("exception", e.Message));
                }
            }
            else
            {
                foreach (var error in request.GetErrors())
                    _mediatorHandler.RaiseEvent(new DomainNotification(error.ErrorCode, error.ErrorMessage));
            }

            return Task.FromResult(false);
        }
    }
}
