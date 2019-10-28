using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Clients;
using AnyStatus.Plugins.RabbitMq.Nodes.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.RabbitMq.Nodes.MemoryUsage
{
    public class SingleNodeMemoryCheck : IRequestHandler<MetricQueryRequest<SingleNodeMemoryWidget>>
    {
        public async Task Handle(MetricQueryRequest<SingleNodeMemoryWidget> request, CancellationToken cancellationToken)
        {
            var ctx = request.DataContext;
            var client = new RabbitMqClient(ctx.URL, ctx.Username, ctx.Password);

            try
            {
                var nodeInfo = await client.GetNodeInfoAsync(ctx.NodesUrlPath, ctx.NodeName)
                                           .ConfigureAwait(false);

                if (!nodeInfo.IsMemoryHealthy(ctx.MaxMemoryUsagePercent, out _, out var memoryUsagePercent))
                {
                    ctx.Value = memoryUsagePercent;
                    ctx.State = State.Failed;
                }
                else
                {
                    ctx.Value = memoryUsagePercent;
                    ctx.State = State.Ok;
                }
            }
            catch (Exception e)
            {
                ctx.State = State.Error;
                ctx.Message = e.Message;
            }
        }
    }
}