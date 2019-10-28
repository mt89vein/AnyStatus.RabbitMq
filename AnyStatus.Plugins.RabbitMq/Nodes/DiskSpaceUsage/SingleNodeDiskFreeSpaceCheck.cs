using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Clients;
using AnyStatus.Plugins.RabbitMq.Nodes.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.RabbitMq.Nodes.DiskSpaceUsage
{
    public class SingleNodeDiskFreeSpaceCheck : IRequestHandler<MetricQueryRequest<NodeDiskSpaceUsageWidget>>
    {
        public async Task Handle(MetricQueryRequest<NodeDiskSpaceUsageWidget> request, CancellationToken cancellationToken)
        {
            var ctx = request.DataContext;
            var client = new RabbitMqClient(ctx.URL, ctx.Username, ctx.Password);

            try
            {
                var nodeInfo = await client.GetNodeInfoAsync(ctx.NodesUrlPath, ctx.NodeName)
                                           .ConfigureAwait(false);

                if (!nodeInfo.IsHasEnoughDiskSpace(ctx.MinFreeDiskSpacePercent, out _, out var diskSpaceUsedPercent))
                {
                    ctx.Value = diskSpaceUsedPercent;
                    ctx.State = State.Failed;
                }
                else
                {
                    ctx.Value = diskSpaceUsedPercent;
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