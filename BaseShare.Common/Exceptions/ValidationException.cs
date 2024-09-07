﻿using BaseShare.Common.Results;
using FluentValidation.Results;

namespace BaseShare.Common.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException()
        : base("one or more validation errors has occured")
        {

        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures.GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        public ValidationException(IEnumerable<Error> failures) : this()
        {
            Errors = failures.GroupBy(x => x.Code, x => x.Description)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        public IDictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
    }
}
