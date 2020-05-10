using Financial.Chat.Domain.Core.Commands.User;
using Financial.Chat.Domain.Shared.Entity;

namespace Financial.Chat.Domain.Core.CommandHandlers
{
    public class AuthenticateUserCommand : LoginCommand<TokenJWT>
    {
        public override bool IsValid()
        {
            ValidationResult = new AuthenticateUserValidator().Validate(this);
            return ValidationResult.IsValid;
        }

        internal class AuthenticateUserValidator : LoginValidator<AuthenticateUserCommand>
        {
            protected override void StartRules()
            {
                base.StartRules();
            }
        }
    }
}
