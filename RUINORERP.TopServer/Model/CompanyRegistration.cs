using System;
using System.Collections.Generic;

namespace RUINORERP.TopServer.Model
{
    /// <summary>
    /// 公司注册信息模型
    /// 用于管理ERP系统的公司（客户）注册信息
    /// </summary>
    public class CompanyRegistration
    {
        /// <summary>
        /// 公司ID（主键）
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; } = string.Empty;

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string ContactEmail { get; set; } = string.Empty;

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 机器码（基于硬件生成的唯一标识）
        /// </summary>
        public string MachineCode { get; set; } = string.Empty;

        /// <summary>
        /// 注册码（基于机器码生成的授权码）
        /// </summary>
        public string RegistrationCode { get; set; } = string.Empty;

        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// 到期日期（续费时间）
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 注册状态
        /// </summary>
        public RegistrationStatus Status { get; set; }

        /// <summary>
        /// 许可用户数量
        /// </summary>
        public int LicensedUsers { get; set; }

        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 是否已过期
        /// </summary>
        public bool IsExpired => DateTime.Now > ExpiryDate;

        /// <summary>
        /// 剩余天数
        /// </summary>
        public int RemainingDays => (ExpiryDate - DateTime.Now).Days;
    }

    /// <summary>
    /// 注册状态枚举
    /// </summary>
    public enum RegistrationStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 已过期
        /// </summary>
        Expired = 1,

        /// <summary>
        /// 已停用
        /// </summary>
        Disabled = 2,

        /// <summary>
        /// 待审核
        /// </summary>
        Pending = 3
    }
}
