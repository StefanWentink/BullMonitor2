using FluentValidation;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class CollectionToSingleValidator<T>
        : AbstractValidator<IEnumerable<T>>
        , ICreateValidator<IEnumerable<T>>
        , IUpdateValidator<IEnumerable<T>>
        , IDeleteValidator<IEnumerable<T>>
    {
        public CollectionToSingleValidator(IValidator<T> validator)
        {
            RuleForEach(x => x).SetValidator(validator);
        }
    }
}