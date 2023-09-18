namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface IFactory<out T>
    {
        T Create();
    }

    public interface IFactory<in TIn, out TOut>
    {
        TOut Create(TIn value);
    }
}