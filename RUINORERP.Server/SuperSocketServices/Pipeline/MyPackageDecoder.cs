﻿using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.Server.ServerSession
{
    /// <summary>
    /// ⚠️ [已过时] 自定义数据包解码器 - 旧架构实现
    /// 已由新架构 RUINORERP.PacketSpec.Serialization.BinaryPackageDecoder 替代
    /// 
    /// 迁移说明:
    /// - 数据包解码已迁移到: RUINORERP.PacketSpec.Serialization.BinaryPackageDecoder
    /// - 统一序列化已迁移到: RUINORERP.PacketSpec.Serialization.UnifiedPacketSerializer
    /// - 二进制序列化已迁移到: RUINORERP.PacketSpec.Serialization.BinarySerializer
    /// 
    /// 建议使用新架构进行开发，此解码器将在未来版本中移除
    /// </summary>
    [Obsolete("此解码器已过时，请使用 RUINORERP.PacketSpec.Serialization.BinaryPackageDecoder 替代", false)]
    public class MyPackageDecoder : IPackageDecoder<BizPackageInfo>
    {
        public BizPackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context)
        {

            var package = new BizPackageInfo();
            try
            {
                var reader = new SequenceReader<byte>(buffer);
                reader.TryRead(out byte packageKey);
                package.Key = "packageKey";
                //reader.Advance(2);// skip the length   
                package.Body = reader.Sequence.ToArray();
            }
            catch (Exception ex)
            {
                frmMain.Instance._logger.LogError("Decode" + ex);
            }

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



