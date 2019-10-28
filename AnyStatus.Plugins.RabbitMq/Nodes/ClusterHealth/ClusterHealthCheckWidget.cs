using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Abstractions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.RabbitMq.Nodes.ClusterHealth
{
    [DisplayName("Cluster health check")]
    [DisplayColumn("RabbitMq")]
    [Description("Cluster Health.")]
    public class ClusterHealthCheckWidget : Build, IRabbitMqWidget, ISchedulable, IHealthCheck
    {
        private const string CATEGORY = "RabbitMq.ClusterHealthCheck";

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
        [Description("Url segment to retrive nodes info.")]
        public string NodesUrlPath { get; set; } = "/api/nodes";

        [Required]
        [PropertyOrder(50)]
        [Category(CATEGORY)]
        [Description("Max node memory usage in percent.")]
        public int MaxMemoryUsagePercent { get; set; } = 85;

        [Required]
        [PropertyOrder(60)]
        [Category(CATEGORY)]
        [Description("Min node free disk space.")]
        public int MinFreeDiskSpacePercent { get; set; } = 25;

        public ClusterHealthCheckWidget()
        {
            Name = "Cluster health";
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
