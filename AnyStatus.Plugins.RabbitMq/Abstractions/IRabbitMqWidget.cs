using AnyStatus.API;

namespace AnyStatus.Plugins.RabbitMq.Abstractions
{
    public interface IRabbitMqWidget : IWebPage
    {
        /// <summary>
        /// Username for basic authentication.
        /// </summary>
        string Username { get; set; }
        
        /// <summary>
        /// Password for basic authentication.
        /// </summary>
        string Password { get; set; }
    }
}
