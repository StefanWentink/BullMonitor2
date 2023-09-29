using SWE.Infrastructure.Http.Interfaces;

namespace SWE.Infrastructure.Http.Configurations
{
    public class HttpClientConfiguration
        : IHttpClientConfiguration
    {
        [Obsolete("Only for serialization.")]
        public HttpClientConfiguration()
            : this(
                  string.Empty,
                  string.Empty,
                  string.Empty)
        { }

        public HttpClientConfiguration(
            string uri,
            string username,
            string password)
        {
            Uri = uri;
            Username = username;
            Password = password;
        }

        public string Uri { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}