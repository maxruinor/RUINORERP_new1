using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{
    public static class CommandRegistry
    {
        private static readonly Dictionary<CommandId, Type> s_map = new();

        public static void Register<TPayload>(CommandId id)
            => s_map[id] = typeof(TPayload);

        public static Type GetPayloadType(CommandId id)
            => s_map.TryGetValue(id, out var t) ? t : null;
    }

     
}
