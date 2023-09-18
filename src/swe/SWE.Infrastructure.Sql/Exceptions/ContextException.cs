using System.Runtime.Serialization;

namespace SWE.Infrastructure.Sql.Exceptions
{
    [Serializable]
    public class ContextException
        : Exception
    {
        [Obsolete("Only for serialization.", true)]
        public ContextException()
        { }

        [Obsolete("Only for serialization.", true)]
        protected ContextException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextException"></see> class
        /// Inherits the <see cref="InternalProcessException"></see>
        /// </summary>
        /// <param name="message">The message that is provided with this exception</param>
        public ContextException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextException"></see> class
        /// Inherits the <see cref="InternalProcessException"></see>
        /// </summary>
        /// <param name="message">The message that is provided with this exception</param>
        /// <param name="cause">The inner exception that caused this exception</param>
        public ContextException(string message, Exception cause)
            : base(message, cause)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextException"></see> class
        /// Inherits the <see cref="InternalProcessException"></see>
        /// </summary>
        /// <param name="cause">The inner exception that caused this exception</param>
        public ContextException(Exception cause)
            : base(string.Empty, cause)
        { }
    }
}