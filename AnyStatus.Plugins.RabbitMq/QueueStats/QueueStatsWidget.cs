using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Abstractions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.RabbitMq.QueueStats
{
    [DisplayName("Queue statistics")]
    [DisplayColumn("RabbitMq")]
    [Description("Ensures messages is not stuck and processing.")]
    public class QueueStatsWidget : Build, IRabbitMqWidget, ISchedulable, IHealthCheck
    {
        private const string CATEGORY = "RabbitMq.QueueStatistics";

        [Url]
        [Required]
        [PropertyOrder(10)]
        [Category(CATEGORY)]
        [Description("RabbitMq management api url")]
        public string URL { get; set; } = "http://localhost:15672";

        [Required]
        [PropertyOrder(20)]
        [Category(CATEGORY)]
        [XmlIgnore]
        public string Username { get; set; } = "guest";

        [Required]
        [PropertyOrder(30)]
        [Category(CATEGORY)]
        [XmlIgnore]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string Password { get; set; } = "guest";

        [Required]
        [PropertyOrder(40)]
        [Category(CATEGORY)]
        public string VirtualHost { get; set; } = "/";

        [Required]
        [PropertyOrder(50)]
        [Category(CATEGORY)]
        [Description("Url segment to retrive queue stats.")]
        public string QueuesUrlPath { get; set; } = "/api/queues";

        [Required]
        [PropertyOrder(60)]
        [Category(CATEGORY)]
        public int MaxMessagesInQueue { get; set; } = 500;

        [Required]
        [PropertyOrder(70)]
        [Category(CATEGORY)]
        public int MaxMessagesInAllQueues { get; set; } = 2000;

        [PropertyOrder(80)]
        [Category(CATEGORY)]
        [Description("A regular expression to filter queues by name in calculation stats.")]
        public string QueueFilterRegex { get; set; } = "^[^#]+$";

        public QueueStatsWidget()
        {
            Name = "Queue statistics";
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }

        [Obsolete]
        public override Notification CreateNotification()
        {
            return State == State.Failed
                ? new Notification(Message, NotificationIcon.Error)
                : Notification.Empty;
        }
    }
}
