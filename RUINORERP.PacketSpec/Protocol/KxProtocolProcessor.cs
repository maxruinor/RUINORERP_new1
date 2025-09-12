using System;
using System.Buffers;
using System.Text;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Security;

namespace RUINORERP.PacketSpec.Protocol
{
    /// <summary>
    /// KX协议处理器 - 负责KX协议的加密、解密和包解析
    /// </summary>
    public static class KxProtocolProcessor
    {
        private const int HeaderLength = 18;
        private static readonly KxCryptographyService _cryptoService = KxCryptographyService.Instance;
        
        /// <summary>
        /// 分析客户端包头部，获取包体长度
        /// </summary>
        public static int AnalyzeClientPackHeader(byte[] header)
        {
            if (header == null || header.Length < HeaderLength)
                throw new ArgumentException("Header length must be at least 18 bytes");
            
            return _cryptoService.AnalyzeClientPackHeader(header);
        }
        
        /// <summary>
        /// 解码KX协议数据包
        /// </summary>
        public static BizPackageInfo DecodeKxPackage(ReadOnlySpan<byte> buffer)
        {
            var kxData = _cryptoService.DecryptClientPackage(buffer);
            
            var packageInfo = new BizPackageInfo
            {
                Body = buffer.ToArray(),
                PackageType = PackageType.KxProtocol,
                Command = kxData.Command,
                Key = "KX"
            };
            
            // 特殊包处理逻辑
            if (buffer.Length == 18)
            {
                packageInfo.SpecialOrder = SpecialOrder.HeaderOnly;
            }
            else if (buffer.Length < 18)
            {
                packageInfo.SpecialOrder = SpecialOrder.InvalidLength;
            }
            
            return packageInfo;
        }
        
        /// <summary>
        /// 编码KX协议数据包
        /// </summary>
        public static byte[] EncodeKxPackage(BizPackageInfo packageInfo)
        {
            var kxData = new KxData
            {
                Command = packageInfo.Command,
                PartOne = packageInfo.Body,
                PartTwo = null
            };
            
            return _cryptoService.EncryptPackage(kxData);
        }
    }
}