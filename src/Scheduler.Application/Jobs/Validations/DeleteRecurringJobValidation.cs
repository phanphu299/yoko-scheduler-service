using AHI.Infrastructure.Exception;
using FluentValidation;
using Scheduler.Application.Command;

namespace Scheduler.Application.Validation
{
    public class DeleteRecurringJobValidation : AbstractValidator<DeleteRecurringJob>
    {
        public DeleteRecurringJobValidation()
        {
            RuleFor(x => x.Ids).NotEmpty().WithMessage(ExceptionErrorCode.DetailCode.ERROR_VALIDATION_REQUIRED);
        }
    }
}