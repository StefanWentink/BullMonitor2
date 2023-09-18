using FluentValidation;

namespace SWE.Infrastructure.Sql.Interfaces.Validators
{
    public interface IUpdateValidator<in T>
        :IValidator<T>
    { }
}