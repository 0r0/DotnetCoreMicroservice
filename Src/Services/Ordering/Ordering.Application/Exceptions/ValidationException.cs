using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

public class ValidationException: ApplicationException
{
    public ValidationException() : base("one or more validation failures have occured")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures):this()
    {
        Errors = failures.GroupBy(failure => failure.PropertyName, failure => failure.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        
    }

    public Dictionary<string,string[]> Errors { get;  }
}