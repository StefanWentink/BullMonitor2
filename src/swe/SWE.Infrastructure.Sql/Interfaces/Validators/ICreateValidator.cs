using FluentValidation;

namespace SWE.Infrastructure.Sql.Interfaces.Validators
{
    public interface ICreateValidator<in T>
        :IValidator<T>
    { }
}