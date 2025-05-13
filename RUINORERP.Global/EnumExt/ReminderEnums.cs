using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{

    public enum RuleEngineType
    {
        //RulesEngine/Roslyn
        None = 0,
        RulesEngine = 1,
        Roslyn = 2,
        Hybrid = 3
    }
    public enum ReminderBizType
    {
        [Description("安全库存提醒")]
        安全库存提醒,

        [Description("库存积压提醒")]
        库存积压提醒,

        [Description("库存盘点提醒")]
        库存盘点提醒,

        [Description("付款到期提醒")]
        付款到期提醒,

        [Description("收款逾期提醒")]
        收款逾期提醒,

        [Description("生产任务到期提醒")]
        生产任务到期提醒,

        [Description("原材料短缺提醒")]
        原材料短缺提醒,

        [Description("潜在客户跟进提醒")]
        潜在客户跟进提醒,

        [Description("客户订单跟进提醒")]
        客户订单跟进提醒,

        [Description("专利预警提醒")]
        专利预警提醒,

        [Description("单据审批提醒")]
        单据审批提醒,

        [Description("其他提醒")]
        其他提醒
    }


    /*
     典型业务提醒场景
库存相关提醒
安全库存提醒（当库存低于安全库存时提醒）
库存积压提醒（当库存超过一定天数未变动时提醒）
库存盘点提醒（定期提醒进行库存盘点）
销售相关提醒
客户订单跟进提醒（当订单状态变更或即将超期时提醒）
潜在客户跟进提醒（当潜在客户信息更新或跟进时间到时提醒）
销售目标达成提醒（当销售目标达成一定比例时提醒）
采购相关提醒
付款到期提醒（当应付账款即将到期时提醒）
原材料短缺提醒（当原材料库存不足时提醒）
财务相关提醒
收款逾期提醒（当应收账款逾期时提醒）
生产相关提醒
生产任务到期提醒（当生产任务即将到期时提醒）
知识产权相关提醒
专利预警提醒（当专利即将到期时提醒）
审批流程相关提醒
单据审批提醒（当有待审批单据时提醒）
     */
    public enum NotificationChannel
    {
        Realtime = 1,
        Email = 2,
        SMS = 4,
        Workflow = 8
    }

    //public enum HealthStatus
    //{
    //    Healthy,
    //    Degraded,
    //    Unhealthy
    //}
}
