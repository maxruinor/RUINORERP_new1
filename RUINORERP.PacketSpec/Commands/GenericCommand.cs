using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Models.Requests;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands
{
    [PacketCommand("GenericCmd", CommandCategory.System)]
    [MessagePackObject]
    public class GenericCommand<TPayload> : BaseCommand
    {
        [Key(0)]
        public TPayload Payload { get; set; }
        
        public GenericCommand() : base()
        {
        }
        
        public GenericCommand(CommandId id, TPayload payload) : base()
        {
            CommandIdentifier = id;
            Payload = payload;
            Direction = PacketDirection.ClientToServer;
        }
        
       
        public override object GetSerializableData() => Payload;
        
       
    }
    
  
}
