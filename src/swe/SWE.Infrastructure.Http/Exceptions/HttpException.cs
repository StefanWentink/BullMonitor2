using System.Net;
using System.Runtime.Serialization;

namespace SWE.Infrastructure.Web.Exceptions
{
    [Serializable]
    public class HttpException
       : System.Exception
    {
        /// <summary>
        /// Contents of <see cref="Url"/>
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Contents of <see cref="HttpVerbs"/>
        /// </summary>
        public HttpMethod Verb { get; set; }

        /// <summary>
        /// Contents of <see cref="HttpVerbs"/>
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Contents of <see cref="Reason"/>
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Exception thrown by <see cref="Handlers.HttpClientHandler"/>.
        /// </summary>
        [Obsolete("Non preferred constructor.", true)]
        public HttpException()
            : base(string.Empty, null)
        { }

        /// <summary>
        /// Exception thrown by <see cref="Handlers.HttpClientHandler"/>.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actualCount"></param>
        /// <param name="innerException"></param>
        public HttpException(
            string url,
            HttpMethod verb,
            HttpStatusCode statusCode)
            : this(
                url,
                verb,
                statusCode,
                null)
        { }

        /// <summary>
        /// Exception thrown by <see cref="Handlers.HttpClientHandler"/>.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actualCount"></param>
        /// <param name="innerException"></param>
        public HttpException(
            string url,
            HttpMethod verb,
            HttpStatusCode statusCode,
            string? reason)
            : base(
                $"{nameof(HttpException)}: {GetMessage(url, verb, statusCode, reason)}",
               null)
        {
            Url = url;
            Verb = verb;
            StatusCode = statusCode;
            Reason = reason ?? string.Empty;
        }

        /// <summary>
        /// Exception thrown by <see cref="Handlers.HttpClientHandler"/>.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actualCount"></param>
        /// <param name="innerException"></param>
        public HttpException(
            string url,
            HttpMethod verb,
            HttpStatusCode statusCode,
            string reason,
            System.Exception innerException)
            : base(
                $"{nameof(HttpException)}: {GetMessage(url, verb, statusCode, reason)}",
                innerException)
        {
            Url = url;
            Verb = verb;
            StatusCode = statusCode;
            Reason = reason;
        }

        /// <summary>
        /// Exception raised by <see cref="System.Linq.Single{T}"/>
        /// </summary>
        [Obsolete("Non preferred constructor.", true)]
        protected HttpException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Url = (string)info.GetValue(nameof(Url), typeof(string));
            Verb = (HttpMethod)info.GetValue(nameof(Verb), typeof(HttpMethod));
            StatusCode = (HttpStatusCode)info.GetValue(nameof(StatusCode), typeof(HttpStatusCode));
            Reason = (string)info.GetValue(nameof(Reason), typeof(string));
        }

        /// <inheritdoc/>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == default)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Url), Url);
            info.AddValue(nameof(Verb), Verb);
            info.AddValue(nameof(StatusCode), StatusCode);
            info.AddValue(nameof(Reason), Reason);

            base.GetObjectData(info, context);
        }

        private static string GetMessage(
            string url,
            HttpMethod verb,
            HttpStatusCode statusCode,
            string? reason)
        {
            return $"{verb}=>{url}. resulted in a {statusCode} with {nameof(reason)} {reason}].";
        }
    }
}