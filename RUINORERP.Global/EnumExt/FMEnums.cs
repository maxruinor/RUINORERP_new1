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
    /// 对账状态
    /// </summary>
    public enum StatementStatus
    {
        草稿 = 1,       // 初始状态
        已发送 = 2,     // 已发送给客户
        已确认 = 3,     // 客户确认对账
        已结清 = 4,     // 完全结清
        部分结算 = 5,   // 部分金额结算
        已作废 = 6,     // 流程终止
    }
    /// <summary>
    /// 对账单明细中的应收付核销状态
    /// </summary>
    public enum ARAPWriteOffStatus
    {
        [Description("待核销")]
        PendingWriteOff = 1,  // 已纳入对账单，尚未核销任何金额

        [Description("部分核销")]
        PartiallyWrittenOff = 2,  // 本次对账中部分金额已核销

        [Description("全额核销")]
        FullyWrittenOff = 3  // 本次对账中全部未核销金额已核销
    }

    /// <summary>
    /// 损益类型
    /// </summary>
    public enum ProfitLossType
    {
        //（视同销售）
        [Description("样品赠出")] 样品赠出 = 1,

        //（进项税转出）
        [Description("库存盘亏")] 库存盘亏 = 2,
        [Description("折旧损失")] 折旧损失 = 3,
        [Description("报废损耗")] 报废损耗 = 4,
        [Description("工艺报废")] 工艺报废 = 5,
        [Description("研发试制损失")] 研发试制损失 = 6,
        [Description("设备故障损失")] 设备故障损失 = 7,
        [Description("物流损毁")] 物流损毁 = 8,
        [Description("汇率损失")] 汇率损失 = 9,
        [Description("坏账损失")] 坏账损失 = 10,
        [Description("平台费用")] 平台费用 = 11,
        [Description("其他经营损失")] 其他经营损失 = 12,
        //------------------------------------------------------------可以以值为分隔下拉选择时
        [Description("库存盘盈")] 库存盘盈 = 20,
        [Description("供应商赠品")] 供应商赠品 = 21,
        [Description("客户退货溢余")] 客户退货溢余 = 22,
        [Description("生产溢余")] 生产溢余 = 23,
        [Description("采购溢余")] 采购溢余 = 24,
        [Description("汇率收益")] 汇率收益 = 25,
        [Description("其他经营溢余")] 其他经营溢余 = 26
    }


    /// <summary>
    /// 亏盈方向
    /// </summary>
    public enum ProfitLossDirection
    {
        /// <summary>
        /// 损失（支出）
        /// </summary>
        [Description("损失（支出）")]
        损失 = 1,

        /// <summary>
        /// 溢余（收入）
        /// </summary>
        [Description("溢余（收入）")]
        溢余 = 2
    }


 
    public enum IncomeExpenseDirection
    {
        /// <summary>
        /// 损失（支出）
        /// </summary>
        [Description("损失（支出）")]
        支出 = 1,

        /// <summary>
        /// 溢余（收入）
        /// </summary>
        [Description("溢余（收入）")]
        收入 = 2
    }


    /// <summary>预付款状态</summary>
    public enum PrePaymentStatus
    {
        [Description("草稿")]
        草稿 = 1,

        [Description("待审核")]
        待审核 = 2,

        [Description("已生效")]
        已生效 = 3,  // 审核通过后状态

        [Description("待核销")]
        待核销 = 4,// 支付完成后状态，待核销，是不是可以和部分核销合并？

        [Description("部分核销")]
        部分核销 = 5,

        [Description("全额核销")]
        全额核销 = 6,

        [Description("已结案")] //包含部分退款和全额退款？
        已结案 = 7,

    }


    /// <summary>应收应付状态</summary>
    public enum ARAPStatus
    {
        [Description("草稿")]
        草稿 = 1,

        [Description("待审核")]
        待审核 = 2,

        //出库生成应付，应付审核时如果有预收付核销后应该是部分支付了。
        [Description("待支付")]
        待支付 = 3,

        [Description("部分支付")]
        部分支付 = 4,

        [Description("全部支付")]
        全部支付 = 5,

        [Description("坏账")]
        坏账 = 6,

        [Description("已冲销")]
        已冲销 = 7

    }





    /// <summary>付款状态</summary>
    public enum PaymentStatus
    {

        [Description("草稿")]
        草稿 = 1,

        [Description("待审核")]
        待审核 = 2,

        [Description("已支付")]
        已支付 = 3
    }




    /// <summary>
    /// 核销类型
    /// 需核销的场景
    /// 预收款抵扣应收款
    /// 收款单直接核销应收款
    /// 预付冲应付
    /// </summary>
    public enum SettlementType
    {

        [Description("未核销")]
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

        [Description("退款红字核销")]
        红字核销 = 6,

        /// <summary>
        /// 一个单位即收又付。双方需进行核销
        /// </summary>
        [Description("应收冲应付")]
        应收冲应付 = 7,

        [Description("应付冲应收")]
        应付冲应收 = 8,

        [Description("对账收款核销")]
        对账收款核销 = 9,

        [Description("对账付款核销")]
        对账付款核销 = 10
    }

    /// <summary>
    /// 收付款方式
    /// </summary>
    public enum ReceivePaymentType
    {
        收款 = 1,//SharedFlag.Flag1;
        付款 = 2,//SharedFlag.Flag2;
    }


    #endregion





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
