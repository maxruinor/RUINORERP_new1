using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{

    /// <summary>
    /// 付款状态
    /// </summary>
    public enum FMPaymentStatus
    {
        /// <summary>
        /// 数据未提交审核（预收付、应收付、收付款、核销单均适用）
        /// </summary>
        草稿 = 0,

        /// <summary>
        /// 数据生效可执行后续操作
        /// </summary>
        已审核 = 1,

        /// <summary>
        /// 预收预付部分核销、应收应付部分结清
        /// </summary>
        部分生效 = 2,

        /// <summary>
        /// 预收预付完全核销、应收应付全额结清
        /// </summary>
        全部生效 = 3,

        /// <summary>
        /// 数据被反向操作撤销（退款、错误修正）
        /// </summary>
        已冲销 = 4,

        ///// <summary>
        ///// 已冲销，预收付款记录已冲销，通常用于取消或调整
        ///// </summary>
        //WrittenOff = 5
    }


    /// <summary>
    /// 收付款类型
    /// </summary>
    public enum ReceivePaymentType
    {
        收款 = 1,        付款 = 2,
    }


    // 枚举定义
    public enum ContractStatus { Draft, Active, Completed, Terminated }
    public enum InvoiceType { Sales, Purchase }

    public enum InvoiceStatus {


        /// <summary>
        /// 发票未正式开具	编辑、删除
        /// </summary>
        草稿,


        /// <summary>
        /// 发票已提交税局	红冲、核销
        /// </summary>
        已开票,


        /// <summary>
        /// 发票已作废	不可修改
        /// </summary>
        已红冲,

        /// <summary>
        /// 发票金额已关联应收/应付	不可修改
        /// </summary>
        已核销,
    }
    public enum PaymentType { Cash, BankTransfer, CreditCard, Other }
    public enum ARPType { Receivable, Payable }
    public enum PaymentStatus { Unpaid, PartiallyPaid, Paid }


    public enum BusinessType { Unsettled, PartiallySettled, Settled }

    //1:应收/2:应付/3:预收/4:预付
    public enum ARPTypes { Receivable = 1, Payable = 2, AdvanceReceivable = 3, AdvancePayable = 4 }


    /*
     | 状态值 | 状态描述 | 说明 | 
| --- | --- | --- | 
| 0 | 草稿 | 记录尚未审核，处于草稿状态 | 
| 1 | 已审核 | 记录已审核，处于生效状态 | 
| 2 | 部分收付 | 部分金额已收付，但未完全结清 | 
| 3 | 已结清 | 全部金额已收付，记录已完全结清 | 
| 4 | 已冲销 | 记录已冲销，通常用于坏账处理或核销 | 
| 5 | 已关闭 | 记录已关闭，通常用于不再处理的情况 | 

     */
    public enum ReceivablePayableStatus
    {
        草稿,
        已审核,
        部分收付,
        已结清,
        已冲销,
        已关闭,
    }

    //创建一个财务模块中的应收应付中的一个状态枚举 0=未结清,1=部分结清,2=已结清

    /// <summary>
    /// 应收应付状态 未收款、已收款、逾期
    /// </summary>
    public enum Receivable
    {

    }

    /// <summary>
    /// 付款 状态 未付款、部分付款，已付款
    /// </summary>
    public enum Payable
    {

    }

    /// <summary>
    /// 结算状态
    /// </summary>
    public enum SettlementStatus
    {
        未结清,
        部分结清,
        已结清
    }

    /// <summary>
    /// 审核状态：0=草稿,1=已审核
    /// </summary>
    public enum AuditStatus
    {
        草稿,
        已审核,
    }


    /// <summary>
    /// 默认付款方式
    /// 在付款方式管理时 第一次系统自动检测后添加
    /// </summary>
    public enum DefaultPaymentMethod
    {
        //1=现金,2=银行转账,3 账期， 4=支票
        现金 = 1,
        银行转账 = 2,
        账期 = 3,
        支票 = 4,
        //微信 = 5,
        //支付宝 =6
    }

    /// <summary>
    /// 账户类型
    /// 注意0值不用能。验证机制不通过
    /// </summary>
    public enum AccountType
    {
        银行账户对公 = 1,
        银行账户对私 = 2,
        现金账户 = 3,
        微信账户 = 4,
        支付宝账户 = 5,
        收款码 = 6,
        其他 = 7,
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
