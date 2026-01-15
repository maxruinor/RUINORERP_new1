using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{
    #region 关键业务财务数据状态
    // 定义对账类型枚举
    public enum StatementType
    {
        余额对账=1,
        收款对账=2,
        付款对账=3
    }

    /// <summary>
    /// 对账状态
    /// </summary>
    public enum StatementStatus
    {
        草稿 = 1,       // 初始状态
        新建 = 2,     // 已发送给客户
        确认 = 3,     // 客户确认对账
        全部结清 = 4,     // 完全结清
        部分结算 = 5,   // 部分金额结算
        已作废 = 6,     // 流程终止
    }
    /// <summary>
    /// 对账单明细中的应收付核销状态
    /// </summary>
    public enum ARAPWriteOffStatus
    {
        [Description("待核销")]
        待核销 = 1,  // 已纳入对账单，尚未核销任何金额

        [Description("部分核销")]
        部分核销 = 2,  // 本次对账中部分金额已核销

        [Description("全额核销")]
        全额核销 = 3  // 本次对账中全部未核销金额已核销
    }

    /// <summary>
    /// 财务科目代码属性
    /// 用于关联枚举值与科目表
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SubjectCodeAttribute : Attribute
    {
        public string Code { get; }
        
        /// <summary>
        /// 初始化科目代码属性
        /// </summary>
        /// <param name="code">科目代码</param>
        public SubjectCodeAttribute(string code)
        {
            Code = code;
        }
    }
    
    /// <summary>
    /// 损益类型（财务报表专用）
    /// </summary>
    public enum ProfitLossType
    {
        #region 支出类（1-49）
        
        /// <summary>
        /// 样品赠送支出
        /// </summary>
        [Description("样品赠送支出")]
        [SubjectCode("660101")]
        样品赠送支出 = 1,

        /// <summary>
        /// 存货盘亏损失
        /// </summary>
        [Description("存货盘亏损失")]
        [SubjectCode("671101")]
        存货盘亏损失 = 2,
        
        /// <summary>
        /// 固定资产折旧
        /// </summary>
        [Description("固定资产折旧")]
        [SubjectCode("160201")]
        固定资产折旧 = 3,
        
        /// <summary>
        /// 资产报废损失
        /// </summary>
        [Description("资产报废损失")]
        [SubjectCode("671102")]
        资产报废损失 = 4,
        
        /// <summary>
        /// 生产工艺损耗
        /// </summary>
        [Description("生产工艺损耗")]
        [SubjectCode("500101")]
        生产工艺损耗 = 5,
        
        /// <summary>
        /// 研发支出
        /// </summary>
        [Description("研发支出")]
        [SubjectCode("530101")]
        研发支出 = 6,
        
        /// <summary>
        /// 设备维修损失
        /// </summary>
        [Description("设备维修损失")]
        [SubjectCode("660201")]
        设备维修损失 = 7,
        
        /// <summary>
        /// 运输途中损耗
        /// </summary>
        [Description("运输途中损耗")]
        [SubjectCode("660102")]
        运输途中损耗 = 8,
        
        /// <summary>
        /// 汇兑损失
        /// </summary>
        [Description("汇兑损失")]
        [SubjectCode("660301")]
        汇兑损失 = 9,
        
        /// <summary>
        /// 坏账准备
        /// </summary>
        [Description("坏账准备")]
        [SubjectCode("123101")]
        坏账准备 = 10,
        
        /// <summary>
        /// 第三方平台服务费
        /// </summary>
        [Description("第三方平台服务费")]
        [SubjectCode("660103")]
        第三方平台服务费 = 11,
        
        /// <summary>
        /// 售后维修支出
        /// </summary>
        [Description("售后维修支出")]
        [SubjectCode("660104")]
        售后维修支出 = 12,
        
        /// <summary>
        /// 其他营业外支出
        /// </summary>
        [Description("其他营业外支出")]
        [SubjectCode("671199")]
        其他营业外支出 = 13,
        
        /// <summary>
        /// 资产减值损失
        /// </summary>
        [Description("资产减值损失")]
        [SubjectCode("670101")]
        资产减值损失 = 14,
        
        /// <summary>
        /// 债务重组损失
        /// </summary>
        [Description("债务重组损失")]
        [SubjectCode("671103")]
        债务重组损失 = 15,
        
        /// <summary>
        /// 税收滞纳金
        /// </summary>
        [Description("税收滞纳金")]
        [SubjectCode("671104")]
        税收滞纳金 = 16,
        
        /// <summary>
        /// 捐赠支出
        /// </summary>
        [Description("捐赠支出")]
        [SubjectCode("671105")]
        捐赠支出 = 17,
        
        /// <summary>
        /// 非常损失
        /// </summary>
        [Description("非常损失")]
        [SubjectCode("671106")]
        非常损失 = 18,
        
        #endregion
        
        #region 收入类（20-99）
        
        /// <summary>
        /// 存货盘盈收益
        /// </summary>
        [Description("存货盘盈收益")]
        [SubjectCode("630101")]
        存货盘盈收益 = 20,
        
        /// <summary>
        /// 接受捐赠收入
        /// </summary>
        [Description("接受捐赠收入")]
        [SubjectCode("630102")]
        接受捐赠收入 = 21,
        
        /// <summary>
        /// 销售退回溢余
        /// </summary>
        [Description("销售退回溢余")]
        [SubjectCode("600101")]
        销售退回溢余 = 22,
        
        /// <summary>
        /// 生产过程溢余
        /// </summary>
        [Description("生产过程溢余")]
        [SubjectCode("500102")]
        生产过程溢余 = 23,
        
        /// <summary>
        /// 采购过程溢余
        /// </summary>
        [Description("采购过程溢余")]
        [SubjectCode("140301")]
        采购过程溢余 = 24,
        
        /// <summary>
        /// 汇兑收益
        /// </summary>
        [Description("汇兑收益")]
        [SubjectCode("606101")]
        汇兑收益 = 25,
        
        /// <summary>
        /// 其他营业外收入
        /// </summary>
        [Description("其他营业外收入")]
        [SubjectCode("630199")]
        其他营业外收入 = 26,
        
        /// <summary>
        /// 公允价值变动收益
        /// </summary>
        [Description("公允价值变动收益")]
        [SubjectCode("610101")]
        公允价值变动收益 = 27,
        
        /// <summary>
        /// 政府补助收入
        /// </summary>
        [Description("政府补助收入")]
        [SubjectCode("611701")]
        政府补助收入 = 28,
        
        /// <summary>
        /// 债务重组收益
        /// </summary>
        [Description("债务重组收益")]
        [SubjectCode("630103")]
        债务重组收益 = 29,
        
        #endregion
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



    //要改为：5，7合并为 “处理中”=5 已部分处理，余额>0，加一个“混合结清”=7 部分核销+部分退款，余额=0
    /// <summary>预付款状态（仅1-8值，全额核销/全额退款为终态）</summary>
    public enum PrePaymentStatus
    {
        [Description("草稿")]
        草稿 = 1,

        [Description("待审核")]
        待审核 = 2,

        [Description("已生效")]
        已生效 = 3,  // 审核通过后状态

        [Description("待核销")]
        待核销 = 4,  // 支付完成后状态，待核销， 全新待处理，从未操作过

        [Description("部分核销")]
        部分核销 = 5,  // 过程态：可继续核销至全额，或发起退款

        [Description("全额核销")]
        全额核销 = 6,  // 唯一终态1：核销流程结束

        [Description("部分退款")]   
        部分退款 = 7,  // 过程态：可继续退款至全额

        [Description("全额退款")]
        全额退款 = 8   // 唯一终态2：退款流程结束
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
    /// 如果在需要编辑。值不能0开始。这里是使用了。不好修改了。
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
        草稿=1,


        /// <summary>
        /// 发票已提交税局	红冲、核销
        /// </summary>
        已开票=2,


        /// <summary>
        /// 发票已作废	不可修改
        /// </summary>
        已红冲=3,

        /// <summary>
        /// 发票金额已关联应收/应付	不可修改
        /// </summary>
        已核销=4,
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
        银行账户对公普票 = 8,
        银行账户对公专票 = 1,
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
