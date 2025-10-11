using MessagePack;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Commands.Cache
{
    [MessagePackObject]
    public class TypedCacheCommand<T> : BaseCommand<TypedCacheRequest<T>, TypedCacheResponse<T>>
    {
        public TypedCacheCommand() : base(PacketDirection.ClientToServer)
        {
            CommandIdentifier = CacheCommands.CacheDataList;
            Priority = CommandPriority.Normal;
            TimeoutMs = 60000;
        }

        public TypedCacheCommand(string tableName, bool forceRefresh = false)
            : this()
        {
            Request = new TypedCacheRequest<T>
            {
                TableName = tableName,
                ForceRefresh = forceRefresh,
                LastRequestTime = DateTime.MinValue
            };
        }

        public TypedCacheCommand(string tableName, int pageIndex, int pageSize)
            : this()
        {
            Request = new TypedCacheRequest<T>
            {
                TableName = tableName,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
    }
}
