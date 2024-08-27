using System;

namespace Scheduler.Application.Pipeline.ValidatorPipeline.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAutomaticValidationAttribute : System.Attribute
    {
    }
}