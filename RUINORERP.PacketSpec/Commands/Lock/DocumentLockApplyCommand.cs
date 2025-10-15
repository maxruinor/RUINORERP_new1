using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 单据锁定申请命令
    /// </summary>
    public class DocumentLockApplyCommand : BaseCommand<DocumentLockRequest, DocumentLockResponse>
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 单据信息
        /// </summary>
        public CommBillData BillData { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DocumentLockApplyCommand()
        {
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = LockCommands.RequestLock;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="billData">单据信息</param>
        /// <param name="menuId">菜单ID</param>
        public DocumentLockApplyCommand(long billId, CommBillData billData, long menuId)
        {
            BillId = billId;
            BillData = billData;
            MenuId = menuId;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = LockCommands.RequestLock;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的锁定申请数据</returns>
        public override object GetSerializableData()
        {
            return new
            {
                BillId = this.BillId,
                BillData = this.BillData,
                MenuId = this.MenuId
            };
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            // 调用基类验证方法，将使用独立的验证器类进行验证
            var result = await base.ValidateAsync(cancellationToken);
            return result;
        }

      
        
        
    }
}
