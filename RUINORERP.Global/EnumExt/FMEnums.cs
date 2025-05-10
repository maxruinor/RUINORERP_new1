using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{
    #region 关键业务财务数据状态

    /// <summary>
    /// 对账单状态
    /// 0=草稿,1=已发送,2=已确认,3=已关闭，4=已结算，5=部分结算
    /// </summary>
    public enum StatementStatus : long
    {
        草稿 = 0,
        已发送 = 1,
        已确认 = 2,
        已关闭 = 3,
        已结清 = 4,
        部分结算 = 5
    }

    // 基础状态 (所有财务单据共用)
    [Flags]
    public enum BaseFMPaymentStatus : long
    {
        [Description("未提交")]
        草稿 = 0,

        [Description("待审核")]
        待审核 = 1 << 0,

        [Description("流程终止")]
        已取消 = 1 << 1,

        [Description("审核通过")]
        已生效 = 1 << 2,

        [Description("反向冲抵")]
        已冲销 = 1 << 3
    }

    // 预收/预付单据状态
    // 预收付状态扩展（10~19位）
    [Flags]
    public enum PrePaymentStatus : long
    {
        // 继承基础状态
        草稿 = BaseFMPaymentStatus.草稿,
        待审核 = BaseFMPaymentStatus.待审核,
        已取消 = BaseFMPaymentStatus.已取消,

        //审核就变成已生效，并且生成收付单，审核后变为待核销
        [Description("已生效")]
        已生效 = BaseFMPaymentStatus.已生效,

        已冲销 = BaseFMPaymentStatus.已冲销,

        // 专属状态
        [Description("部分核销")]
        部分核销 = 1 << 10,

        [Description("全额核销")]
        全额核销 = 1 << 11,

        //表示已经支付成功
        [Description("待核销")]
        待核销 = 1 << 12
    }

    // 应收/应付单据状态
    // 应收付状态扩展（20~29位）
    [Flags]
    public enum ARAPStatus : long
    {
        // 继承基础状态
        草稿 = BaseFMPaymentStatus.草稿,
        待审核 = BaseFMPaymentStatus.待审核,
        已取消 = BaseFMPaymentStatus.已取消,
        已生效 = BaseFMPaymentStatus.已生效,
        已冲销 = BaseFMPaymentStatus.已冲销,

        // 专属状态
        [Description("已结清")]
        已结清 = 1 << 20,

        [Description("坏账")]
        坏账 = 1 << 21,

        [Description("部分支付")]
        部分支付 = 1 << 22
    }

    // 收/付款单据状态
    // 收付款状态扩展（30~39位）
    [Flags]
    public enum PaymentStatus : long
    {
        // 继承基础状态
        草稿 = BaseFMPaymentStatus.草稿,
        待审核 = BaseFMPaymentStatus.待审核,
        已取消 = BaseFMPaymentStatus.已取消,

        ///// <summary>
        ///// 表示收款单或付款单已通过审核，可以进行后续操作（如支付或收款）。
        ///// </summary>
        //已生效 = BaseFMPaymentStatus.已生效,
        // 专属状态 审核就是已支付,已核销
        [Description("已支付")]
        已支付 = 1 << 30,

        //是通过预收付等 抵扣时用户自定义的
        [Description("已核销")]
        已冲销 = 1 << 31,

    }
    #endregion

    /*

    /// <summary>
    /// 支付状态
    /// </summary>
    public enum FMPaymentStatus
    {
        /// <summary>未提交（无任何有效状态）</summary>
        草稿 = 0,       // 0b0000（默认值，标志位全0）

        /// <summary>已提交审核</summary>
        提交 = 1,       // 0b0001（2^0）

        /// <summary>财务审核通过支付生效</summary>
        已审核 = 2,     // 0b0010（2^1）

        /// <summary>部分生效（如部分核销/结清）</summary>
        部分核销 = 4,   // 0b0100（2^2）

        /// <summary>全部生效（全额核销/结清）</summary>
        全额核销 = 8,   // 0b1000（2^3）

        /// <summary>数据被冲销（反向操作）</summary>
        已冲销 = 16,    // 0b10000（2^4，新增无冲突的2的幂）

        /// <summary>单据关闭）</summary>
        已取消 = 32,    // 0b10000（2^4，新增无冲突的2的幂）
    }
    */


    /// <summary>
    /// 核销类型
    /// 需核销的场景
    /// 预收款抵扣应收款
    /// 收款单直接核销应收款
    /// 预付冲应付
    /// </summary>
    public enum SettlementType
    {

        [Description("手工核销")]
        未核销 = 0,

        [Description("收款核销应收")]
        收款核销 = 1,

        [Description("付款核销应付")]
        付款核销 = 2,

        [Description("预收冲抵应收")]
        预收冲应收 = 3,

        [Description("预付冲抵应付")]
        预付冲应付 = 4,

        [Description("坏账核销")]
        坏账核销 = 5,

        [Description("退款红冲")]
        红冲核销 = 6,

        [Description("多币种调汇")]
        汇差调整 = 7

    }

    /// <summary>
    /// 收付款方式
    /// </summary>
    public enum ReceivePaymentType
    {
        收款 = 1,
        付款 = 2,
    }


    // 枚举定义
    public enum ContractStatus { Draft, Active, Completed, Terminated }
    public enum InvoiceType { Sales, Purchase }

    public enum InvoiceStatus
    {


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




    ///// <summary>
    ///// 结算状态
    ///// </summary>
    //public enum SettlementStatus
    //{
    //    未结清,
    //    部分结清,
    //    已结清
    //}

    /// <summary>
    /// 审核状态：0=草稿,1=已审核
    /// </summary>
    public enum AuditStatus
    {
        草稿,
        已审核,
    }

    /// <summary>
    /// 默认币种
    /// 在币种管理时 第一次系统自动检测后添加
    /// </summary>
    public enum DefaultCurrency
    {
        RMB = 1,
        USD = 2,
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
