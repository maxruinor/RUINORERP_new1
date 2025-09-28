using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{
    public static class CommandRegistry
    {
        private static ImmutableDictionary<CommandId, Type> s_mapIdToType = ImmutableDictionary<CommandId, Type>.Empty;
        private static ImmutableDictionary<Type, CommandId> s_mapTypeToId = ImmutableDictionary<Type, CommandId>.Empty;

        public static void Register<TPayload>(CommandId id)
        {
            if (s_mapIdToType.ContainsKey(id))
                throw new ArgumentException($"Command ID {id} is already registered");
                
            if (s_mapTypeToId.ContainsKey(typeof(TPayload)))
                throw new ArgumentException($"Type {typeof(TPayload).FullName} is already registered");
                
            s_mapIdToType = s_mapIdToType.Add(id, typeof(TPayload));
            s_mapTypeToId = s_mapTypeToId.Add(typeof(TPayload), id);
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