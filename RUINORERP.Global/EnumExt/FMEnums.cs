using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{
    public enum CustomerStatus
    {
        潜在客户 = 0,
        成交客户 = 1,
        战略合作 = 2,
        无效客户 = 3
    }
    public enum 关系型客户类型
    {
        忠实客户,
        新客户,
        潜在客户
    }
    public enum 交易型客户类型
    {
        批发客户 = 1,
        零售客户 = 2,
        自用客户 = 3,
        分销商 = 4,
        企业客户 = 5,
        合作伙伴 = 6
    }

    public enum LeadsStatus
    {
        新建 = 0,
        跟进中 = 1,
        已转化 = 2,
        已丢失 = 3
    }

    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountType
    {
        银行账户 = 0,
        现金账户 = 1,
        微信账户 = 2,
        支付宝账户 = 3,
        收款码 = 4,
    }


    /// <summary>
    /// 科目类型
    /// 资产类科目：用于记录企业的资产，如现金、应收账款、固定资产等。
    /// 负债类科目：用于记录企业的负债，如应付账款、短期借款、长期借款等。
    /// 权益类科目：用于记录企业的所有者权益，如股本、资本公积、留存收益等。
    /// 成本类科目：用于记录企业的生产成本，如直接材料、直接人工、制造费用等。
    /// 损益类科目：用于记录企业的收入和费用，如销售收入、销售成本、管理费用、财务费用等。
    /// </summary>
    public enum SubjectType
    {
        /// <summary>
        /// ASSET
        /// </summary>
        资产 = 1,
        /// <summary>
        /// LIABILITY类型
        /// </summary>
        负债 = 2,
        /// <summary>
        /// EQUITY类型
        /// </summary>
        权益 = 3,

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
