using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderRules
{

    // 库存积压提醒规则模型
    public class StockOverstockRule : tb_ReminderRule
    {
        public StockOverstockConfig BusinessConfig { get; set; }
    }

    public class StockOverstockConfig
    {
        public List<int> ProductIds { get; set; }
        public int OverstockThreshold { get; set; } // 超储阈值(天数)
        public List<string> ResponsiblePersons { get; set; } // 责任人
        public bool EnableWeeklyReport { get; set; } // 是否每周报告
    }

    // 库存盘点提醒规则模型
    public class StockAuditRule : tb_ReminderRule
    {
        public StockAuditConfig BusinessConfig { get; set; }
    }

    public class StockAuditConfig
    {
        public List<int> WarehouseIds { get; set; }
        public string AuditFrequency { get; set; } // 盘点频率(每日、每周、每月)
        public string AuditTime { get; set; } // 盘点时间
        public List<string> AuditTeam { get; set; } // 盘点小组
    }

    // 付款到期提醒规则模型
    public class PaymentDueRule : tb_ReminderRule
    {
        public PaymentDueConfig BusinessConfig { get; set; }
    }

    public class PaymentDueConfig
    {
        public List<int> SupplierIds { get; set; }
        public int LeadTime { get; set; } // 提前预警天数
        public List<string> FinancialOfficers { get; set; } // 财务负责人
        public bool EnableEscalation { get; set; } // 是否升级提醒
    }

    // 收款逾期提醒规则模型
    public class ReceivableOverdueRule : tb_ReminderRule
    {
        public ReceivableOverdueConfig BusinessConfig { get; set; }
    }

    public class ReceivableOverdueConfig
    {
        public List<int> CustomerIds { get; set; }
        public int OverdueThreshold { get; set; } // 逾期天数阈值
        public List<string> CollectionTeam { get; set; } // 收款团队
        public bool EnableLegalAction { get; set; } // 是否启动法律程序
    }

    // 生产任务到期提醒规则模型
    public class ProductionTaskDueRule : tb_ReminderRule
    {
        public ProductionTaskDueConfig BusinessConfig { get; set; }
    }

    public class ProductionTaskDueConfig
    {
        public List<int> TaskIds { get; set; }
        public int WarningDays { get; set; } // 提前预警天数
        public List<string> ProductionManagers { get; set; } // 生产经理
        public bool IsCriticalTask { get; set; } // 是否关键任务
    }

    // 原材料短缺提醒规则模型
    public class RawMaterialShortageRule : tb_ReminderRule
    {
        public RawMaterialShortageConfig BusinessConfig { get; set; }
    }

    public class RawMaterialShortageConfig
    {
        public List<int> MaterialIds { get; set; }
        public int MinQuantity { get; set; } // 最低库存数量
        public List<string> PurchasingAgents { get; set; } // 采购人员
        public bool AutoCreatePO { get; set; } // 自动创建采购订单
    }

    // 潜在客户跟进提醒规则模型
    public class PotentialCustomerFollowUpRule : tb_ReminderRule
    {
        public PotentialCustomerFollowUpConfig BusinessConfig { get; set; }
    }

    public class PotentialCustomerFollowUpConfig
    {
        public List<int> CustomerIds { get; set; }
        public int FollowUpFrequency { get; set; } // 跟进频率(天数)
        public List<string> SalesReps { get; set; } // 销售代表
        public bool EnableFollowUpLog { get; set; } // 启用跟进日志
    }

    // 客户订单跟进提醒规则模型
    public class CustomerOrderFollowUpRule : tb_ReminderRule
    {
        public CustomerOrderFollowUpConfig BusinessConfig { get; set; }
    }

    public class CustomerOrderFollowUpConfig
    {
        public List<int> OrderIds { get; set; }
        public int ExpectedDeliveryDays { get; set; } // 预计交货天数
        public List<string> AccountManagers { get; set; } // 客户经理
        public bool EnableCustomerNotification { get; set; } // 启用客户通知
    }

    // 专利预警提醒规则模型
    public class PatentExpiryRule : tb_ReminderRule
    {
        public PatentExpiryConfig BusinessConfig { get; set; }
    }

    public class PatentExpiryConfig
    {
        public List<int> PatentIds { get; set; }
        public int RenewalLeadTime { get; set; } // 续期提前天数
        public List<string> IntellectualPropertyManagers { get; set; } // 知识产权经理
        public bool AutoFileRenewal { get; set; } // 自动提交续期
    }

    // 单据审批提醒规则模型
    public class DocumentApprovalRule : tb_ReminderRule
    {
        public DocApprovalConfig BusinessConfig { get; set; }
    }


   
}
