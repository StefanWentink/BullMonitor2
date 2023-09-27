using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace SWE.Infrastructure.Web.Policies
{
    public static class HttpPolicyBuilders
    {
        public static PolicyBuilder<HttpResponseMessage> GetBaseBuilder()
        {
            return HttpPolicyExtensions.HandleTransientHttpError();
        }
    }
}