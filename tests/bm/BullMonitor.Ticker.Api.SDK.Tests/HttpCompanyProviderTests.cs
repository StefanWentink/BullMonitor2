using BullMonitor.Ticker.Api.SDK.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSWE.Tests.Base;
using BullMonitor.Ticker.Api.SDK.Extensions;
using Xunit;
using Moq;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Issue.Abstraction.Messages;
using SWE.Rabbit.Abstractions.Messages;

namespace BullMonitor.Ticker.Api.SDK.Tests
{
    public class HttpCompanyProviderTests
        : BaseServiceProviderTests<IHttpCompanyProvider>
    {
        //private static readonly JsonSerializerOptions _jsonOptions = new()
        //{
        //    PropertyNameCaseInsensitive = true,
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //    WriteIndented = true,
        //    NumberHandling = JsonNumberHandling.AllowReadingFromString,
        //    Converters =
        //    {
        //        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        //    }
        //};

        private static Mock<IMessageSender<ExceptionMessage>> _exceptionSenderMock = CreateExceptionSenderMock();

        private static Mock<IMessageSender<ExceptionMessage>> CreateExceptionSenderMock()
        {
            var mock = new Mock<IMessageSender<ExceptionMessage>>();

            mock
                .Setup(x => x.Send(It.IsAny<ExceptionMessage>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask)
                .Verifiable();

            mock
                .Setup(x => x.Send(It.IsAny<RoutingMessage<ExceptionMessage>>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask)
                .Verifiable();

            return mock;
        }

        protected override IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            //var mockHttpMessageHandler = GetMockHttpMessageHandler();
            //var mockHttpClientFactory = GetMockHttpClientFactory(mockHttpMessageHandler, configuration);

            return services
                .AddSingleton(_exceptionSenderMock.Object)
                .WithBullMonitorTickerApiSdkServices(configuration)
                .AddHttpClient()
                //.AddSingleton(mockHttpClientFactory.Object);
                //.AddSingleton<ISingleProvider<TipRanksRequest, string?>, StubTipRanksRawProvider>()
            ;
        }

        [Fact]
        public void Provider_Should_Resolve()
        {
            try
            {
                var provider = Create();

                provider.Should().NotBeNull();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                exception.Should().BeNull();
            }
        }

        [SkippableTheory]
        [InlineData("LIZI")]
        [InlineData("MSFT")]
        public async Task GetByCode_Should_Call_Api(
            string value)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var provider = Create();

            try
            {
                var response = await provider
                    .GetSingleOrDefault(value, cancellationToken)
                    .ConfigureAwait(false);

                response.Should().NotBeNull();
                response?.Code.Should().Be(value);
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine(exception);

                var inconclusiveMessage = "No connection could be made because the target machine actively refused it. (localhost:7232)";

                Skip.If(exception.Message.Equals(inconclusiveMessage, StringComparison.OrdinalIgnoreCase));
            }
        }

        [SkippableTheory]
        [InlineData("F2C5FC0F-D5F4-4652-BBFE-08DBB84D9919")]
        [InlineData("D1213670-AF23-461E-BCF4-08DBB84D9919")]
        public async Task GetById_Should_Call_Api(
            string guidId)
        {
            var id = Guid.Parse(guidId);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var provider = Create();

            try
            {
                var response = await provider
                    .GetSingleOrDefault(id, cancellationToken)
                    .ConfigureAwait(false);

                response.Should().NotBeNull();

                response?.Id.Should().Be(id);
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine(exception);

                var inconclusiveMessage = "No connection could be made because the target machine actively refused it. (localhost:7232)";

                Skip.If(exception.Message.Equals(inconclusiveMessage, StringComparison.OrdinalIgnoreCase));
            }
        }

        //protected Mock<IHttpClientFactory> GetMockHttpClientFactory(
        //    Mock<HttpMessageHandler> mockHttpMessageHandler,
        //    AllocationApiConfiguration configuration)
        //{
        //    var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        //    var client = new HttpClient(mockHttpMessageHandler.Object)
        //    {
        //        BaseAddress = new Uri(configuration.BaseUrl)
        //    };

        //    mockHttpClientFactory
        //        .Setup(_ => _.CreateClient(It.IsAny<string>()))
        //        .Returns(client);

        //    return mockHttpClientFactory;
        //}

        //private Mock<HttpMessageHandler> GetMockHttpMessageHandler()
        //{
        //    var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        //    var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        //    mockHttpMessageHandler
        //        .Protected()
        //        .Setup<Task<HttpResponseMessage>>(
        //                "SendAsync",
        //                ItExpr.IsAny<HttpRequestMessage>(),
        //                ItExpr.IsAny<CancellationToken>())
        //            .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
        //            {
        //                var stringContent = request
        //                    .Content
        //                    .ReadAsStringAsync(cancellationToken)
        //                    .GetAwaiter()
        //                    .GetResult();

        //                var command = JsonSerializer
        //                    .Deserialize<AllocationCreateCommand>(stringContent, _jsonOptions);

        //                var response =
        //                        new AllocationCreateResponse(
        //                            command
        //                                .Allocations
        //                                .Select(allocation => allocation.Id));

        //                var responseContent = JsonSerializer
        //                    .Serialize(response);

        //                var responseMessage = new HttpResponseMessage
        //                {
        //                    StatusCode = HttpStatusCode.OK,
        //                    Content = new StringContent(responseContent)
        //                };

        //                return responseMessage;
        //            })
        //            .Verifiable();

        //    return mockHttpMessageHandler;
        //}
    }
}