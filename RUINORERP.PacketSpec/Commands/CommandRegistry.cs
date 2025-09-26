using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{
    public static class CommandRegistry
    {
        private static readonly Dictionary<CommandId, Type> s_mapIdToType = new();
        private static readonly Dictionary<Type, CommandId> s_mapTypeToId = new();

        public static void Register<TPayload>(CommandId id)
        {
            s_mapIdToType[id] = typeof(TPayload);
            s_mapTypeToId[typeof(TPayload)] = id;
        }

        public static Type GetPayloadType(CommandId id)
            => s_mapIdToType.TryGetValue(id, out var t) ? t : null;
            
        /// <summary>
        /// 根据请求类型获取命令ID
        /// </summary>
        /// <typeparam name="TReq">请求类型</typeparam>
        /// <returns>对应的命令ID</returns>
        public static CommandId GetCommandId<TReq>()
        {
            if (s_mapTypeToId.TryGetValue(typeof(TReq), out var commandId))
            {
                return commandId;
            }
            
            throw new ArgumentException($"No command ID registered for type {typeof(TReq).FullName}");
        }
    }
}
