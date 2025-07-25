using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ConfigModel
{
    public class FMConfiguration
    {
        /// <summary>
        /// 应收款自动核销预收款
        /// </summary>
        public bool EnableARAutoOffsetPreReceive { get; set; }=true;

        /// <summary>
        /// 应付款自动核销预付款
        /// </summary>
        public bool EnableAPAutoOffsetPrepay { get; set; } = true;


        /// <summary>
        /// 平台订单时，自动审核预收款单
        /// </summary>
        public bool AutoAuditPreReceivePayment { get; set; }


        /// <summary>
        /// 平台订单时，自动审核收款单
        /// tb_FM_PaymentRecord
        /// </summary>
        public bool AutoAuditReceivePayment { get; set; }


        /// <summary>
        /// 平台订单时，自动审核应收款单
        /// </summary>
        public bool AutoAuditReceivePaymentable { get; set; }


    }
}
