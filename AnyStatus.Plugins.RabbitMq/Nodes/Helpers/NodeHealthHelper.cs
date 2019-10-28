using AnyStatus.API.Common.Utils;
using AnyStatus.Plugins.RabbitMq.Nodes.Contracts;
using System;

namespace AnyStatus.Plugins.RabbitMq.Nodes.Helpers
{
    public static class NodeHealthHelper
    {
        public static bool IsMemoryHealthy(
            this NodeInfo nodeInfo,
            int maxMemoryUsagePercent,
            out string errorMessage,
            out int memoryUsagePercent
        )
        {
            memoryUsagePercent = (int) Math.Round((double) (100 * nodeInfo.UsedMemory) / nodeInfo.MemoryLimit);

            if (memoryUsagePercent >= maxMemoryUsagePercent)
            {
                errorMessage = " memory limit exceeded: " + BytesFormatter.Format(nodeInfo.UsedMemory) +
                               " of " + BytesFormatter.Format(nodeInfo.MemoryLimit) + Environment.NewLine;

                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        public static bool IsHasEnoughDiskSpace(
            this NodeInfo nodeInfo,
            int minFreeDiskSpacePercent,
            out string errorMessage,
            out int usedDiskSpacePercent
        )
        {
            usedDiskSpacePercent = (int) Math.Round((double) (100 * nodeInfo.DiskLimit) / nodeInfo.DiskFree);

            if (usedDiskSpacePercent >= 100 - minFreeDiskSpacePercent)
            {
                errorMessage = " disk free limit excedeed: " + BytesFormatter.Format(nodeInfo.DiskFree) +
                               " of " + BytesFormatter.Format(nodeInfo.DiskLimit) + Environment.NewLine;

                return false;
            }

            errorMessage = string.Empty;

            return true;
        }
    }
}