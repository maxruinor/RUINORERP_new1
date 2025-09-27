using RUINORERP.PacketSpec.Commands.Lock;
using RUINORERP.PacketSpec.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 请求锁命令 - 客户端向服务器发送锁相关请求
    /// </summary>
    public class RequestLockCommand : IClientCommand
    {
        public Guid PacketId { get; set; }
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }

        /// <summary>
        /// 锁命令类型
        /// </summary>
        public LockCommandType LockCommandType { get; set; }

        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 锁定信息
        /// </summary>
        public LockedInfo LockInfo { get; set; }

        /// <summary>
        /// 解锁信息
        /// </summary>
        public UnLockInfo UnlockInfo { get; set; }

        /// <summary>
        /// 请求解锁信息
        /// </summary>
        public RequestUnLockInfo RequestUnlockInfo { get; set; }

        /// <summary>
        /// 拒绝解锁信息
        /// </summary>
        public RefuseUnLockInfo RefuseUnlockInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RequestLockCommand()
        {
            PacketId = Guid.NewGuid();
            OperationType = CmdOperation.Send;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lockCommandType">锁命令类型</param>
        public RequestLockCommand(LockCommandType lockCommandType)
        {
            PacketId = Guid.NewGuid();
            OperationType = CmdOperation.Send;
            LockCommandType = lockCommandType;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="parameters">参数</param>
        /// <returns>任务</returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters)
        {
            await Task.Run(() =>
            {
                BuildDataPacket();
            }, cancellationToken);
        }

        /// <summary>
        /// 构建数据包
        /// </summary>
        public void BuildDataPacket()
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuffer(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)LockCommandType);

                switch (LockCommandType)
                {
                    case LockCommandType.LockDocument:
                        if (LockInfo != null)
                        {
                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(LockInfo);
                            tx.PushString(json);
                        }
                        break;
                    case LockCommandType.UnlockDocument:
                        if (UnlockInfo != null)
                        {
                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(UnlockInfo);
                            tx.PushString(json);
                        }
                        break;
                    case LockCommandType.RequestUnlock:
                        if (RequestUnlockInfo != null)
                        {
                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(RequestUnlockInfo);
                            tx.PushString(json);
                        }
                        break;
                    case LockCommandType.RefuseUnlock:
                        if (RefuseUnlockInfo != null)
                        {
                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(RefuseUnlockInfo);
                            tx.PushString(json);
                        }
                        break;
                }

                // 设置命令标识
                gd.cmd = (byte)ClientCmdEnum.复合型锁单处理;
                gd.One = new byte[] { (byte)LockCommandType };
                gd.Two = tx.toByte();
                
                // 发送数据
                MainForm.Instance.ecs.AddSendData(gd);
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("构建锁命令数据包异常: " + ex.Message);
            }
        }

        /// <summary>
        /// 分析数据包
        /// </summary>
        /// <param name="gd">原始数据</param>
        /// <returns>是否分析成功</returns>
        public bool AnalyzeDataPacket(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string sendTime = ByteOperations.GetString(gd.Two, ref index);
                LockCommandType lockCmd = (LockCommandType)ByteOperations.GetInt(gd.Two, ref index);

                switch (lockCmd)
                {
                    case LockCommandType.LockDocument:
                        string json = ByteOperations.GetString(gd.Two, ref index);
                        var obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                        LockedInfo lockRequest = obj.ToObject<LockedInfo>();
                        bool isSuccess = ByteOperations.Getbool(gd.Two, ref index);
                        // 处理锁定结果
                        break;
                    case LockCommandType.UnlockDocument:
                        json = ByteOperations.GetString(gd.Two, ref index);
                        obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                        UnLockInfo unLockInfo = obj.ToObject<UnLockInfo>();
                        isSuccess = ByteOperations.Getbool(gd.Two, ref index);
                        // 处理解锁结果
                        break;
                    case LockCommandType.RequestUnlock:
                        json = ByteOperations.GetString(gd.Two, ref index);
                        obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                        RequestUnLockInfo requestUnLockInfo = obj.ToObject<RequestUnLockInfo>();
                        // 处理请求解锁结果
                        break;
                    case LockCommandType.RefuseUnlock:
                        json = ByteOperations.GetString(gd.Two, ref index);
                        obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                        RefuseUnLockInfo refuseUnLockInfo = obj.ToObject<RefuseUnLockInfo>();
                        // 处理拒绝解锁结果
                        break;
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("分析锁命令数据包异常: " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 分析广播数据包
        /// </summary>
        /// <param name="gd">原始数据</param>
        /// <returns>是否分析成功</returns>
        public bool AnalyzeBroadcastDataPacket(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string sendTime = ByteOperations.GetString(gd.Two, ref index);
                LockCommandType lockCmd = (LockCommandType)ByteOperations.GetInt(gd.Two, ref index);
                if (lockCmd == LockCommandType.Broadcast)
                {
                    string lockDictionaryJson = ByteOperations.GetString(gd.Two, ref index);
                    MainForm.Instance.lockManager.UpdateLockStatusByJson(lockDictionaryJson);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("分析广播锁命令数据包异常: " + ex.Message);
            }
            return true;
        }
    }

    /// <summary>
    /// 锁命令类型
    /// </summary>
    public enum LockCommandType
    {
        /// <summary>
        /// 锁定单据
        /// </summary>
        LockDocument = 1,

        /// <summary>
        /// 解锁单据
        /// </summary>
        UnlockDocument = 2,

        /// <summary>
        /// 请求解锁
        /// </summary>
        RequestUnlock = 3,

        /// <summary>
        /// 拒绝解锁
        /// </summary>
        RefuseUnlock = 4,

        /// <summary>
        /// 广播
        /// </summary>
        Broadcast = 5
    }
}