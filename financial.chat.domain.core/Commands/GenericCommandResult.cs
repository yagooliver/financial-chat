using Financial.Chat.Domain.Shared.Commands;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Financial.Chat.Domain.Core.Commands
{
    public abstract class GenericCommandResult<T> : ICommandResult<T>
    {
        protected GenericCommandResult()
        {
            ValidationResult = new ValidationResult();
        }

        protected ValidationResult ValidationResult { get; set; }

        public abstract bool IsValid();

        public virtual IList<ValidationFailure> GetErrors()
        {
            return ValidationResult.Errors;
        }
    }
}
