using Microsoft.Extensions.Logging;
using SWE.RabbitMq.Interfaces;
using RabbitMQ.Client;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Factories;
using System.Text.Json;

namespace SWE.RabbitMq.Contracts
{
    public class RabbitExchange<T>
        : IDisposable
    {
        private string? _name = null;
        private IConnectionFactory? _connectionFactory = null;
        private IConnection? _connection = null;
        private IModel? _channel = null;
        
        protected virtual string Name => _name ??= typeof(T).Name;

        protected IConnectionFactory ConnectionFactory => _connectionFactory ??=
            (string.IsNullOrWhiteSpace(Configuration.UserName) && string.IsNullOrWhiteSpace(Configuration.Password))
                ? new ConnectionFactory()
                {
                    HostName = Configuration.HostName
                    // Add additional options
                }
                : new ConnectionFactory()
                    {
                        HostName = Configuration.HostName,
                        UserName = Configuration.UserName,
                        Password = Configuration.Password
                        // Add additional options
                    };

        protected IConnection Connection => _connection ??= ConnectionFactory.CreateConnection();
        protected IModel Channel => _channel ??= Connection.CreateModel();

        protected IRabbitConfiguration Configuration { get; }
        private IRabbitExchangeConfiguration? _exchangeConfiguration = null;


        protected IRabbitExchangeConfiguration ExchangeConfiguration => _exchangeConfiguration ??= Configuration.Get(Name);

        private bool? _exchangeDurable = null;
        private bool? _exchangeAutoDelete = null;
        protected bool ExchangeDurable => _exchangeDurable ??= true;//ExchangeConfiguration.Queues.Any(x => x.Durable); // SWE: not sure yet
        protected bool ExchangeAutoDelete => _exchangeAutoDelete ??= false;//!ExchangeConfiguration.Queues.Any(x => !x.AutoDelete); // SWE: not sure yet

        protected ILogger<RabbitExchange<T>> Logger { get; }

        protected virtual JsonSerializerOptions SerializerOptions => JsonSerializerOptionsFactory.Options;

        public RabbitExchange(
            IRabbitConfiguration configuration,
            ILogger<RabbitExchange<T>> logger)
        {
            Configuration = configuration;
            Logger = logger;
            Setup();
        }

        protected virtual void Setup()
        {
            Channel.ExchangeDeclare(
                ExchangeConfiguration.ExchangeName,
                ExchangeConfiguration.Type.GetDescription(),
                durable: ExchangeDurable,
                autoDelete: ExchangeAutoDelete);

            Channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            foreach (var queue in ExchangeConfiguration.Queues)
            {
                var queueName = Channel
                    .QueueDeclare(
                        queue: queue.QueueName,
                        durable: queue.Durable,
                        exclusive: false,
                        autoDelete: queue.AutoDelete,
                        arguments: null)
                    .QueueName;

                Channel
                    .QueueBind(
                        queue: queue.QueueName,
                        exchange: ExchangeConfiguration.ExchangeName,
                    routingKey: queue.RoutingKey);

            }
        }

        private bool disposedValue;
        private SemaphoreSlim _disposeLock = new SemaphoreSlim(1);

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                _disposeLock.Wait(10000);
                
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects)
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    foreach (var queue in ExchangeConfiguration.Queues)
                    {
                        if (queue.AutoDelete)
                        {
                            Channel?.QueueDelete(queue.QueueName, queue.AutoDelete, queue.AutoDelete);
                        }
                    }

                    var queueConfigurations = ExchangeConfiguration.Queues;

                    if (ExchangeAutoDelete)
                    {
                        Channel.ExchangeDelete(ExchangeConfiguration.ExchangeName, ExchangeAutoDelete);
                    }

                    Channel?.Dispose();
                    Connection?.Dispose();

                    disposedValue = true;
                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, $"Failed to dispose {GetType().Name}");
            }
            finally
            {
                _disposeLock.Release();
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RabbitExchange()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
