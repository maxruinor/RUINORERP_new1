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
        /// 
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

    }
}
