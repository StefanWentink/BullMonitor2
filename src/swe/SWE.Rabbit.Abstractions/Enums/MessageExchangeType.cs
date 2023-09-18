using System.ComponentModel;

namespace SWE.Rabbit.Abstractions.Enums
{
    public enum MessageExchangeType
    {
        /// <summary>
        /// Direct messaging:
        /// One copy of a message is delivered to one consumer.
        /// May be combined with different routing keys (exact match) on the Queue's.
        /// </summary>
        [Description("direct")]
        Direct = 10,

        /// <summary>
        /// Fanout messaging:
        /// All queues get a copy of a message
        /// Can not be combined with routing keys!
        /// </summary>
        [Description("fanout")]
        Fanout = 20,

        [Description("headers")]
        Headers = 30,

        /// <summary>
        /// Topic messaging:
        /// Delivers a copy of each message to the matching queues
        /// Combined with different routing keys (pattern like *.consumption.*) on the Queue's
        /// </summary>
        [Description("topic")]
        Topic = 40,
    }
}