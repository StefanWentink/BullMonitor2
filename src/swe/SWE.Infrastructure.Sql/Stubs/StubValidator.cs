using FluentValidation;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Stubs
{
    public class StubValidator<T>
        : AbstractValidator<T>
    { }

    public class StubCreateValidator<T>
        : StubValidator<T>
        , ICreateValidator<T>
    { }

    public class StubUpdateValidator<T>
        : StubValidator<T>
        , IUpdateValidator<T>
    { }

    public class StubDeleteValidator<T>
        : StubValidator<T>
        , IDeleteValidator<T>
    { }
}