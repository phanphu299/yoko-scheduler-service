using AHI.Infrastructure.Exception;
using FluentValidation;
using Scheduler.Application.Command;

namespace Scheduler.Application.Validation
{
    public class UpsertRecurringJobValidation : AbstractValidator<UpsertRecurringJob>
    {
        public UpsertRecurringJobValidation()
        {
            RuleFor(x => x.Cron).NotEmpty().WithMessage(ExceptionErrorCode.DetailCode.ERROR_VALIDATION_REQUIRED);
            RuleFor(x => x.Endpoint).NotEmpty().WithMessage(ExceptionErrorCode.DetailCode.ERROR_VALIDATION_REQUIRED);
            RuleFor(x => x.TimeZoneName).NotEmpty().WithMessage(ExceptionErrorCode.DetailCode.ERROR_VALIDATION_REQUIRED);
        }
    }
}