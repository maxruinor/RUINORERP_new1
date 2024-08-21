using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Server.ServerSession
{
    /// <summary>
    /// 你也可以通过将解析包的代码从 PipelineFilter 移到 你的包解码器中来获得更大的灵活性：
    /// 通过 host builder 的 UsePackageDecoder 方法来在SuperSocket中启用它??
    /// </summary>
    public class MyPackageDecoder : IPackageDecoder<BizPackageInfo>
    {
        public BizPackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            /*
            tcp.ReceiveTimeout = 10000; //10秒
            rxi = 0;
            if (len != 0)
            {
                ret.Two = new byte[len];
                while (rxi < len)
                {
                    rxi += tcp.Receive(ret.Two, len - rxi, System.Net.Sockets.SocketFlags.None);
                }
                try
                {
                    掩码 = 加密(KEY, ret.Two, pi, 掩码);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                outs += Tools.Hex2Str(ret.Two, 0, len, true);
            }
            Tools.ShowMsg($"---[{outs.Length / 3}]:{outs}\r\n");
            */
            var package = new BizPackageInfo();
            var reader = new SequenceReader<byte>(buffer);
            reader.TryRead(out byte packageKey);
            package.Key = "packageKey";
            //reader.Advance(2);// skip the length   
            package.Body = reader.Sequence.ToArray();
            return package;
        }
    }


    public class LanderPackageDecoder : IPackageDecoder<StringPackageInfo>
    {
        public StringPackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            var text = buffer.GetString(new UTF8Encoding(false));
            var parts = text.Split(':', 2);

            return new StringPackageInfo
            {
                Key = parts[0],
                Body = text,
                Parameters = parts[1].Split(',')
            };
        }
    }


}



