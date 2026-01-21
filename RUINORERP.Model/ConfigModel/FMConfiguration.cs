using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ConfigModel
{

    /// <summary>
    /// 财务模块配置
    /// </summary>
    public class FMConfiguration
    {
        /// <summary>
        /// 应收款自动核销预收款-------销售订单与销售出库单对应的情况下才执行
        /// </summary>
        public bool EnableARAutoOffsetPreReceive { get; set; } = true;

        /// <summary>
        /// 应付款自动核销预付款--------采购订单与采购入库对应时才执行
        /// </summary>
        public bool EnableAPAutoOffsetPrepay { get; set; } = true;


        /// <summary>
        /// 收款单自动核销应收款
        
        /// 预收的收款单审核时。会自动去核销 销售出库的应收款 前提是 应收有审核了。没有审核则
        /// </summary>
        public bool EnablePaymentAutoOffsetAR { get; set; } = true;

        /// <summary>
        /// 付款单自动核销应付款
        /// </summary>
        public bool EnablePaymentAutoOffsetAP { get; set; } = true;


        /// <summary>
        /// 自动审核预收款单
        /// </summary>
        public bool AutoAuditPreReceive { get; set; }



        /// <summary>
        /// 自动审核预付款单
        /// </summary>
        public bool AutoAuditPrePayment { get; set; } = false;


        /// <summary>
        /// 平台订单时，自动审核收款单
        /// 平台订单才可能直接进账
        /// </summary>
        public bool AutoAuditReceivePaymentRecordByPlatform { get; set; }


        /// <summary>
        /// 销售出库了。就应该应收了。这时一个开关，方便能反审
        /// 自动审核应收款
        /// </summary>
        public bool AutoAuditReceiveable { get; set; }


        /// <summary>
        /// 采购入库了。就可能应付了。这时一个开关，方便能反审
        /// 自动审核应付款
        /// </summary>
        public bool AutoAuditPaymentable { get; set; }




        /// <summary>
        /// 平台订单取消作废时启用财务数据自动退款功能
        /// </summary>
       public bool EnableAutoRefundOnOrderCancel { get; set; } = false;

        /// <summary>
        /// 费用报销的付款单自动审核
        /// </summary>
        public bool AutoAuditExpensePaymentRecord { get; set; } = false;

        /// <summary>
        /// 全额预收款订单，销售出库时可以自动审核。销售出库审核时会自动核销预收款单。
        /// 账期订单或没有收到尾款的销售订单对应的销售出库则不会自动审核，由财务手工审核。
        /// </summary>
        public bool EnableAutoAuditSalesOutboundForFullPrepaymentOrders { get; set; } = false;

        /// <summary>
        /// 启用销售订单付款状态验证，禁止未满足付款条件的订单进行销售出库审核
        /// 当销售订单处于以下付款状态时，禁止出库（审核）操作：
        /// - 未付款状态
        /// - 部分预付状态
        /// - 部分付款状态
        /// </summary>
        public bool EnableSalesOrderPaymentStatusValidation { get; set; } = false;

        /// <summary>
        /// 金额计算容差阈值
        /// 用于处理浮点数计算精度问题，当|计算金额-原始金额|≤阈值时按0处理
        /// 适用于折扣、运费分摊等各种金额计算场景
        /// 默认值：0.0001，最大精度：4位小数
        /// </summary>
        public decimal AmountCalculationTolerance { get; set; } = 0.0001m;

    }
}