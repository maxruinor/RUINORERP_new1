using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.Server.ServerSession;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// 旧通讯系统适配器 - 提供向后兼容性
    /// 用于逐步替换TransInstruction组件，不修改业务逻辑
    /// </summary>
    public class LegacyCommAdapter
    {
        private readonly ILogger<LegacyCommAdapter> _logger;

        public LegacyCommAdapter(ILogger<LegacyCommAdapter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 兼容原有的CryptoProtocol.EncryptionServerPackToClient方法
        /// </summary>
        public static EncryptedData EncryptionServerPackToClient(byte cmd, byte[] one, byte[] two)
        {
            return CommunicationHelper.EncryptServerPackToClient(cmd, one, two);
        }

        /// <summary>
        /// 兼容原有的CryptoProtocol.EncryptionServerPackToClient方法
        /// </summary>
        public static EncryptedData EncryptionServerPackToClient(OriginalData data)
        {
            return CommunicationHelper.EncryptServerPackToClient(data);
        }

        /// <summary>
        /// 兼容原有的数据解密方法
        /// </summary>
        public static OriginalData DecryptClientPack(byte[] encryptedData)
        {
            return CommunicationHelper.DecryptClientPack(encryptedData);
        }

        /// <summary>
        /// 性能优化的数据发送方法
        /// </summary>
        public static async Task<bool> SendDataToSession(SessionforBiz session, OriginalData data)
        {
            if (session == null || data.Cmd == null)
                return false;

            try
            {
                var encryptedData = EncryptionServerPackToClient(data);

                // 使用session的DataQueue进行异步发送（兼容原有逻辑）
                session.AddSendData(encryptedData);

                // 批量发送队列中的数据
                await FlushSessionQueue(session);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 批量发送会话队列中的数据 - 性能优化
        /// </summary>
        private static async Task FlushSessionQueue(SessionforBiz session)
        {
            const int maxBatchSize = 10; // 批量处理大小
            int batchCount = 0;

            while (session.DataQueue.TryDequeue(out var data) && batchCount < maxBatchSize)
            {
                await session.SendAsync(data);
                batchCount++;
            }
        }

  
    }



    //注意：移除了重复的ServerCmdEnum和KxData定义，请使用RUINORERP.PacketSpec.Enums.ServerCmdEnum
    //和RUINORERP.PacketSpec.Security.KxData
}