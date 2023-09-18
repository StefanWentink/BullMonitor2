using FluentValidation;

namespace SWE.Infrastructure.Sql.Interfaces.Validators
{
    public interface IDeleteValidator<in T>
        :IValidator<T>
    { }
}