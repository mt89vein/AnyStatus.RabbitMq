using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Clients;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.RabbitMq.QueueStats
{
    public class QueueStatsHealthCheck : ICheckHealth<QueueStatsWidget>
    {
        public async Task Handle(HealthCheckRequest<QueueStatsWidget> request, CancellationToken cancellationToken)
        {
            var ctx = request.DataContext;
            var client = new RabbitMqClient(ctx.URL, ctx.Username, ctx.Password);

            try
            {
                var queueStatistics = await client
                    .GetQueueStatisticsAsync(ctx.QueuesUrlPath, ctx.VirtualHost)
                    .ConfigureAwait(false);

                var tooMuchMessagesQueues =
                    queueStatistics
                        .Where(x => x.Messages > ctx.MaxMessagesInQueue && new Regex(ctx.QueueFilterRegex).IsMatch(x.Name))
                        .Select(x => $"{x.Name} has {x.Messages} messages. ConsumersCount: {x.Consumers}");

                var totalMessagesSum = queueStatistics.Sum(x => x.Messages);

                var errorMessage = string.Empty;

                if (totalMessagesSum >= ctx.MaxMessagesInAllQueues)
                {
                    errorMessage = "Max messages exceeded: " + totalMessagesSum + " of " +
                                   ctx.MaxMessagesInAllQueues + Environment.NewLine;
                }

                if (tooMuchMessagesQueues.Any())
                {
                    errorMessage += string.Join(";" + Environment.NewLine, tooMuchMessagesQueues) + ";";
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ctx.Message = errorMessage;
                    ctx.State = State.Failed;
                }
                else
                {
                    ctx.State = State.Ok;
                    ctx.Message = null;
                }
            }
            catch (Exception e)
            {
                ctx.Message = e.Message;
                ctx.State = State.Failed;
            }
        }
    }
}