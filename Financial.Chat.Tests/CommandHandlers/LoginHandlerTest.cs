﻿using AutoMapper;
using Financial.Chat.Application.AutoMapper;
using Financial.Chat.Application.Services;
using Financial.Chat.Domain.Core.CommandHandlers;
using Financial.Chat.Domain.Core.Entity;
using Financial.Chat.Domain.Core.Interfaces;
using Financial.Chat.Domain.Core.Interfaces.Services;
using Financial.Chat.Domain.Shared.Handler;
using Financial.Chat.Domain.Shared.Helper;
using Financial.Chat.Domain.Shared.Notifications;
using Financial.Chat.Infra.Data;
using Financial.Chat.Infra.Data.Repositories;
using Financial.Chat.Tests.ContextDb;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace Financial.Chat.Tests.CommandHandlers
{
    [TestClass]
    public class LoginHandlerTest : FinancialChatDbContextFixure
    {
        private IUnitOfWork _unitOfWork;
        private Mock<IMediatorHandler> _mockMediator;
        private IUserRepository _userRepository;
        private ILoginService _loginService;
        private DomainNotificationHandler _domainNotificationHandler;
        private IMapper _mapper;
        private LoginHandler handler;

        [TestInitialize]
        public void InitTests()
        {
            db = GetDbInstance();
            _unitOfWork = new UnitOfWork(db);
            _userRepository = new UserRepository(db);
            _mockMediator = new Mock<IMediatorHandler>();
            _domainNotificationHandler = new DomainNotificationHandler();
            _mockMediator.Setup(x => x.RaiseEvent(It.IsAny<DomainNotification>())).Callback<DomainNotification>((x) =>
            {
                _domainNotificationHandler.Handle(x, CancellationToken.None);
            });

            _mapper = AutoMapperConfig.RegisterMappings().CreateMapper();

            _userRepository.Add(new User
            {
                Email = "test@test.com",
                Name = "Test",
                Password = Cryptography.PasswordEncrypt("123456")
            });
            _unitOfWork.Commit();

            var mockConfig = new Mock<IConfiguration>();

            mockConfig.Setup(x => x[It.Is<string>(s => s.Equals("Jwt:Issuer"))])
                .Returns("Test");

            mockConfig.Setup(x => x[It.Is<string>(s => s.Equals("Jwt:Duration"))])
                .Returns("120");

            mockConfig.Setup(x => x[It.Is<string>(s => s.Equals("Jwt:Key"))])
                .Returns("IZpipYfLNJro403p");

            _loginService = new LoginService(_userRepository, mockConfig.Object);
            handler = new LoginHandler(_unitOfWork, _userRepository, _mockMediator.Object, _mapper, _loginService);
        }
        [TestMethod]
        public async Task Should_not_get_authenticated()
        {
            var result = await handler.Handle(new AuthenticateUserCommand { Email = "test2@test.com", Password = "123356" }, CancellationToken.None);

            Assert.IsNull(result);
            Assert.IsTrue(_domainNotificationHandler.HasNotifications());
        }
        [TestMethod]
        public async Task Should_not_get_authenticated_invalid_email()
        {
            var result = await handler.Handle(new AuthenticateUserCommand { Email = "test.com", Password = "123456" }, CancellationToken.None);

            Assert.IsNull(result);
            Assert.IsTrue(_domainNotificationHandler.HasNotifications());
        }

        [TestMethod]
        public async Task Should_get_authenticated()
        {
            var result = await handler.Handle(new AuthenticateUserCommand { Email = "test@test.com", Password = "123456" }, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsFalse(_domainNotificationHandler.HasNotifications());
        }
    }
}
