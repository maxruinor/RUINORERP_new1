using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{


    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountType
    {
        银行账户对公 = 0,
        银行账户对私 = 1,
        现金账户 = 2,
        微信账户 = 3,
        支付宝账户 = 4,
        收款码 = 5,
        其他 = 6,
    }


    /// <summary>
    /// 科目类型
    /// 资产类：用于记录企业的资产，如现金、应收账款、固定资产等。
    /// 负债类：用于记录企业的负债，如应付账款、短期借款、长期借款等。
    /// 权益类：用于记录企业的所有者权益，如股本、资本公积、留存收益等。
    /// 成本类：用于记录企业的生产成本，如直接材料、直接人工、制造费用等。
    /// 损益类：用于记录企业的收入和费用，如销售收入、销售成本、管理费用、财务费用等。
    /// </summary>
    public enum SubjectType
    {
        /// <summary>
        /// ASSET
        /// </summary>
        资产类 = 1,
        /// <summary>
        /// LIABILITY类型
        /// </summary>
        负债类 = 2,
        /// <summary>
        /// EQUITY类型
        /// </summary>
        所有者权益类 = 3,

        /// <summary>
        ///  COST
        /// </summary>
        成本 = 4,

        /// <summary>
        /// ProfitAndLoss
        /// </summary>
        损益 = 5,
        /// <summary>
        /// Other
        /// </summary>
        其他 = 6



    }
}
