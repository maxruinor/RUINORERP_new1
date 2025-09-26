using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Exceptions
{
    public sealed class RpcException : Exception
    {
        public int Code { get; set; }
        public RpcException(int code, string message) : base(message) => Code = code;
    }
}
