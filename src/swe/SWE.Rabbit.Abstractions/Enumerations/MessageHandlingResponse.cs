using System.ComponentModel;

namespace SWE.Rabbit.Abstractions.Enumerations
{
    public enum MessageHandlingResponse
    {
        /// <summary>
        /// Message is successfully handled and can be removed from the queue by the broker
        /// </summary>
        [Description("Successful")]
        Successful = 10,

        /// <summary>
        /// When a message must be offered again some time later by the broker
        /// </summary>
        [Description("Unsuccessful")]
        Unsuccessful = 20,

        /// <summary>
        /// Worker is unable to handle the message it must be dead lettered
        /// </summary>
        [Description("Rejected")]
        Rejected = 30
    }
}
