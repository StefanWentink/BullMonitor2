using Microsoft.Extensions.Hosting;
using SWE.Rabbit.Abstractions.Interfaces;

namespace SWE.Rabbit.Receiver
{
    /// <summary>
    /// Setup like this
    /// 
    /// var builder = WebApplication.CreateBuilder(args);
    /// builder.Services.AddHostedService<RabbitService<T>>();
    /// 
    /// var app = builder.Build();
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RabbitService<T>
        : IHostedService
    {
        protected IMessageReceiver<T> Receiver { get; private set; }

        public RabbitService(IMessageReceiver<T> receiver)
        {
            Receiver = receiver;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
#if DEBUG
            Console.WriteLine($"{nameof(IHostedService)} for receiving {typeof(T).Name} started.");
#endif
            await Receiver
                .StartReceiving(cancellationToken)
                .ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Receiver?.Dispose();
            return Task.CompletedTask;
        }
    }
}