namespace AnyStatus.Plugins.RabbitMq.QueueStats.Contracts
{
    public class QueueStatistics
    {
        public int Consumers { get; set; }

        public int Messages { get; set; }

        public string Name { get; set; }
    }
}