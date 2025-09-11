using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global
{

    /// <summary>
    /// 销售出库单退货退款状态
    /// </summary>
    public enum RefundStatus
    {
        未退款等待退货 = 1,

        未退款已退货 = 2,

        已退款等待退货 = 3,

        /// <summary>
        /// 超时完成
        /// </summary>
       // [Description("仅退款")]
        已退款未退货 = 4,


        已退款已退货 = 5,

        //用于销售出库单 中的部分退款情况？
        部分退款退货 = 6,
    }

    // 岗位类型枚举（仅用于菜单推荐逻辑）
    public enum PositionType
    {
        [Description("采购")]
        Purchasing,     // 采购

        [Description("销售")]
        Sales,          // 销售

        [Description("仓库")]
        Warehouse,      // 仓库

        [Description("财务")]
        Finance,        // 财务

        [Description("生产")]
        Manufacturing,   // 生产

        [Description("售后")]
        AfterSales,     // 售后

        [Description("行政")]
        Administrative,

        [Description("管理层")]
        Management,     // 管理层

        [Description("电商运营")]
        EcommerceOperation,

        [Description("其他")]
        Common          // 通用
    }


    public enum PlatformType
    {
        阿里1688,
        淘宝,
        京东,
        拼多多
    }

    /// <summary>
    /// 运费分摊规则
    /// </summary>
    public enum FreightAllocationRules
    {
        产品数量占比 = 1,
        产品金额占比 = 2,
        产品重量占比 = 3,
    }

    //合同类型枚举

    /// <summary>
    /// 往来单位类型枚举
    /// </summary>
    public enum BusinessPartnerType
    {
        //Supplier
        [Description("供应商")]
        供应商 = 1,

        //Customer
        [Description("客户")]
        客户 = 2,

        //Other
        [Description("其他")]
        其他 = 3
    }

    public enum ContractEnum
    {
        [Description("销售合同")]
        SaleContract = 1,
        [Description("采购合同")]
        PurContract = 2,
    }


    public enum ProdQueryUseType
    {
        None = 0,
        [Description("全部")]
        盘点导入 = 1,
        单据录入 = 2,
        产品查询 = 3
    }

    /// <summary>
    /// 在做采购入库退回单时，需要选择处理方式
    /// 和重点是财务相关。退款 会生退款单。  否则不会。
    /// </summary>
    public enum PurReProcessWay
    {
        /// <summary>
        /// 不会修改采购订单和入库相关信息， 会修改库存数量（采购订单时也要计算退回的情况。）
        /// </summary>
        厂商退款 = 0,

        /// <summary>
        /// 会修改采购订单数量，会修改库存数量
        /// </summary>
        需要返回 = 1,

        /// <summary>
        /// 会修改采购订单数量，会修改库存数量？？
        /// </summary>
        //换货 = 2,

    }


    public enum PrintStatus
    {
        未打印 = 0,
        打印一次 = 1,
        打印二次 = 2,
        打印三次 = 3,
        打印四次 = 4,
        打印五次 = 5,
        打印六次 = 6,
        打印七次 = 7,
        打印八次 = 8,
        打印九次 = 9,
        打印十次 = 10,
    }
    public enum TrackingState
    {
        /// <summary>Existing entity that has not been modified.</summary>
        Unchanged,
        /// <summary>Newly created entity.</summary>
        Added,
        /// <summary>Existing entity that has been modified.</summary>
        Modified,
        /// <summary>Existing entity that has been marked as deleted.</summary>
        Deleted
    }

    /// <summary>
    /// 产品来源
    /// </summary>
    public enum GoodsSource
    {
        外采,
        自产
    }


    /*
     1、先收到先处理。
2、使处理时间最短。
3、预先确定顺序号。
4、优先处理订货量小相对简单的订单。
5、优先处理承诺交货日期最早的订单。
6、优先处理距约定交货日期最近的订单。
     */
    /// <summary>
    /// 单据优先级
    /// </summary>
    public enum Priority
    {
        正常 = 1,
        重要 = 2,
        重要紧急 = 3,
    }
    public enum UILogType
    {

        /// <summary>
        /// 绿色
        /// </summary>
        成功提示消息,

        /// <summary>
        /// 黑色
        /// </summary>
        普通消息,


        /// <summary>
        /// 蓝色
        /// </summary>
        提示,

        /// <summary>
        /// 黄色
        /// </summary>
        警告,

        /// <summary>
        /// 红色,会写到的数据库的日志中，谨慎使用！
        /// </summary>
        错误
    }


    // [NoWantIOC()]
    public enum SearchType
    {
        Document,
        PageSearch,
    }

    // [NoWantIOC()]
    public enum BaseInfoType
    {
        ProductNo,

        /// <summary>
        /// 会计科目
        /// </summary>
        FMSubject,

        /// <summary>
        /// 产品助记码
        /// 类别拼单首字母+序号？
        /// </summary>
        ShortCode,

        /// <summary>
        /// 模块定义
        /// </summary>
        ModuleDefinition,
        ProCategories,
        Employee,
        Department,
        Storehouse,
        Supplier,
        Customer,

        /// <summary>
        /// 其他
        /// </summary>
        CVOther,

        Location,
        SKU_No,
        StoreCode,
        /// <summary>
        /// 项目代码
        /// </summary>
        ProjectGroupCode,

        /// <summary>
        /// 区域代码
        /// </summary>
        CRM_RegionCode,

        /// <summary>
        /// 往来单位
        /// </summary>
        BusinessPartner,
    }

    //上面只是测试，b

    /// <summary>
    /// 目前用在生成单号和审核时得到单据公共信息
    /// 因为是不是一次性写完，并且会保存到菜单配置表中。指定具体值更好，更灵活
    /// TODO:后面优化重构要将菜单数据库表中的biztype int 改为名称
    /// 枚举值  可以用负数，但不推荐
    /// </summary>
    public enum BizType
    {
        //Unknown=-2,
        无对应数据 = -1,
        销售订单 = 0,// SO (Sales Order)
        销售出库单 = 1,// STO (Sales Transfer Out)
        销售退回单 = 2, // STR (Sales Return)
        采购订单 = 3, // PO (Purchase Order)
        采购入库单 = 4, // PIO (Purchase Inbound Order)
        采购退货单 = 5,// PRT (Purchase Return)
        其他入库单 = 6, // OIO (Other Inbound Order)
        其他出库单 = 7,// OTO (Other Transfer Out)

        采购退货统计 = 8,
        // 返厂出库 = 9,
        //售后入库 = 10,
        //售后出库 = 11,

        报损单 = 12,
        报溢单 = 13,
        盘点单 = 14,
        制令单 = 15,  // MO (Manufacturing Order)
        BOM物料清单 = 16, // BOM (Bill of Materials)
        生产领料单 = 17,// MPR (Material Pick Requisition)
        生产退料单 = 18,  // MPRR (Material Pick Return)
        生产补料单 = 19, // SFO (Subcontracting Fabrication Order)
        发料计划单 = 20,
        //成品缴库 = 21,//作废  可以换为其它业务
        托外加工单 = 22,
        托外领料单 = 23,
        退料单 = 24,
        托外补料单 = 25,
        托外加工缴回单 = 26,
        人事考勤 = 27,
        其他费用支出 = 28,
        其他费用收入 = 29,
        费用报销单 = 30,
        库存查询 = 31,
        采购订单统计 = 32,
        采购入库统计 = 33,
        借出单统计 = 34,
        归还单统计 = 35,
        盘点明细统计 = 36,
        其他入库统计 = 37,
        其他出库统计 = 38,
        销售订单统计 = 39,
        销售出库统计 = 40,
        //添加暂时要添加到下面。菜单配置中 有一个业务类型用了int位置会乱，或是引用单据时，类型是这个枚举值
        需求分析 = 41,
        材料需求分析 = 42,
        库存跟踪 = 43,
        销售订单跟踪 = 44,
        采购订单跟踪 = 45,
        制令单跟踪 = 46,
        销售退回统计 = 47,
        销售数据汇总 = 48,
        采购数据汇总 = 49,
        库存数据汇总 = 50,
        生产计划单 = 51,
        缴库单 = 52,
        请购单 = 53,
        产品分割单 = 54,
        产品组合单 = 55,
        借出单 = 56,
        归还单 = 57,
        套装组合 = 58,
        包装信息 = 59,
        产品档案 = 60,
        默认数据 = 61,

        /// <summary>
        /// 产品转换单 A变成B后再出库,AB相近。可能只是换说明书或刷机  A  数量  加或减 。B数量增加或减少。
        /// </summary>
        产品转换单 = 62,

        调拨单 = 63,
        采购退货入库 = 64,
        售后返厂退回 = 65,
        售后返厂入库 = 66,
        生产领料统计 = 67,
        生产退料统计 = 68,
        缴库明细统计 = 69,
        生产计划统计 = 70,
        归还明细统计 = 71,
        分割明细统计 = 72,
        组合明细统计 = 73,
        转换单统计 = 74,
        调拨统计 = 75,
        其他费用统计 = 76,
        CRM跟进计划 = 77,
        CRM跟进记录 = 78,
        返工退库单 = 79,
        返工退库统计 = 80,
        返工入库单 = 81,
        返工入库统计 = 82,
        付款申请单 = 83,

        预收款单 = 84,
        预付款单 = 85,

        应收款单 = 86,  // AR (Accounts Receivable)
        应付款单 = 87,// AP (Accounts Payable)

        付款单 = 88,
        收款单 = 89,


        收款核销 = 90,
        付款核销 = 91,

        销售价格调整单 = 92,
        采购价格调整单 = 93,

        付款统计 = 94,
        收款统计 = 95,
        质量检验单 = 96,// QC (Quality Check)
        销售合同 = 150,

        售后申请单 = 161,
        售后交付单 = 162,
        维修工单 = 163,
        维修领料单 = 164,
        维修入库单 = 165,
        报废单 = 166,
        售后借出单 = 167,
        售后归还单 = 168,
        售后申请统计 = 169,
        售后交付统计 = 170,


        损失确认单 = 171,
        溢余确认单 = 172,

  
        对账单 = 175,


        营销活动 = 300,
        蓄水订单 = 301,
        未知类型 = 999,
    }


    /// <summary>
    /// 单据是否含税
    /// </summary>
    public enum TaxType
    {
        不计税,
        含税价,
        不含税价,
    }


    /// <summary>
    /// 单据记账类型 是不是要分进销项/ 借 贷 
    /// 立账：此字段主要控制是否产生应付账款以及何时产生账款。若选择“记应付账”，则单据在存盘（终审）后，立即形成应付账款，若选择“不立账”，则不产生任何应付账款，比如对一些赠品就可以采用这样的处理，若选择“收票记账”，则不产生应付账款，在收到供应商的发票时，再形成应付账款。 
    /// </summary>
    public enum KeepAccountsType
    {
        不立账,

        /// <summary>
        /// 自动生成应付单
        /// </summary>
        记应付账,

        收票记账,

        /// <summary>
        /// 自动生成应收单
        /// </summary>
        记应收账,

        开票记账,
    }


    /// <summary>
    /// 盘点调整模式
    /// </summary>
    public enum Adjust_Type
    {
        [Description("【增加库存】")]
        增加,
        [Description("【减少库存】")]
        减少,
        [Description("【全部】")]
        全部
    }

    /// <summary>
    /// 数据状态 单据才用结案
    /// 单据整体状态
    /// </summary>
    public enum DataStatus
    {
        [Description("【草稿】未提交")]
        /// <summary>
        /// 草稿Draft
        /// </summary>
        草稿 = 1,


        [Description("【新建】已提交")]
        /// <summary>
        /// 提交后才会推送给审核人员
        /// </summary>
        新建 = 2,


        [Description("【确认】已审核")]
        /// <summary>
        /// Confimmed,
        /// </summary>
        确认 = 4,


        [Description("【完结】已结案")]
        /// <summary>
        /// close
        /// </summary>
        //结案
        完结 = 8,

        [Description("【取消】作废")]
        作废 = 16
    }


    //public enum ProductType
    //{
    //    成品,
    //    半成品,
    //    在制品,
    //    原料,
    //    包材,
    //    线材
    //}

    /// <summary>
    /// 产品属性类型EVA
    /// 数据库中有对应的表及固定插入值，暂时初始化系统时，同时初始化数据
    /// </summary>
    public enum ProductAttributeType
    {
        单属性 = 1,
        可配置多属性 = 2,
        捆绑 = 3,
        虚拟 = 4
    }

    /*
     在 C# 中，如果需要表示枚举值可以是多个值的组合（即“或值”或“异或值”），可以使用位字段（bit fields）的特性来实现。位字段允许你将枚举的每个成员表示为一个单独的位，这样就可以通过位运算来组合多个枚举值。这种类型的枚举被称为“标志枚举”（Flags Enum）。
    要将你的 BoxReuleBasis 枚举修改为支持这种组合，你可以遵循以下步骤：

定义一个基本值，通常是 1，然后每个后续的枚举值是前一个值的两倍，这样可以确保它们的位是唯一的。
使用 Flags 特性来标记这个枚举，表示它可以包含多个值的组合。
     */
    /// <summary>
    /// 箱规基于三个级别维护，品名，SKU，组合级
    /// </summary>
    [Flags]
    public enum BoxRuleBasis
    {

        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,          // 0b0000

        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        Product = 1,       // 0b0001


        /// <summary>
        /// 多属性SKU级
        /// </summary>
        [Description("多属性")]
        Attributes = 2,    // 0b0010

        /// <summary>
        /// 产品组合
        /// </summary>
        [Description("产品组合")]
        Combination = 4,  // 0b0100
    }



    /*
     // 反射获取描述
var description = typeof(BoxRuleBasis)
                    .GetField("Product")
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .Cast<DescriptionAttribute>()
                    .Select(attr => attr.Description)
                    .FirstOrDefault();
     */

    /// <summary>
    /// 付款状态
    /// </summary>
    public enum PayStatus
    {
        未付款 = 1,
        全额预付 = 2,
        部分预付 = 3,
        全部付款 = 4,
        部分付款 = 5,
    }


    /// <summary>
    /// 盘点方式
    /// 一般盘点 在2025-06-14前 值是0 要改一下数据库内容。
    /// </summary>
    public enum CheckMode
    {
        一般盘点 = 1,
        期初盘点 = 2,
    }

    /// <summary>
    /// 审核流程进度 如果需要多级 多人审核 设计会签功能  如业务审核，财务审核 多级 / 多人审核（如会签、分步审核）
    /// 单级审核时用不上。就看是否支持多级审核了。那么 财务模块也要添加这个字段
    /// </summary>

    [Flags]
    public enum ApprovalStatus
    {
        未审核 = 0,
        已审核 = 1,
        驳回 = 2,
        //待审核 = 2,
        //审核中 = 3,
        //结案 = 4,
    }


    //// 重新设计为位枚举，支持组合状态（示例）
    //[Flags]
    //public enum ApprovalStatus
    //{
    //    未开始 = 0,         // 0b0000
    //    业务审核中 = 1,      // 0b0001（1<<0）
    //    财务审核中 = 2,      // 0b0010（1<<1）
    //    业务已通过 = 4,      // 0b0100（1<<2）
    //    财务已通过 = 8,      // 0b1000（1<<3）
    //    驳回 = 16           // 0b10000（1<<4，最终状态）
    //}


    /// <summary>
    /// 审批结果 是否需要有会签功能？https://www.likecs.com/show-747870.html
    /// DB设计了一个审核配置主子表  配置了审核的人，比方 业务审核要求在明细一行，财务审核也在明细一行 两个人审核通过才能通过，但是这个是缓存在工作流引擎中的。如果引擎重启则
    /// 可能要重新审核，如果当时审核的配置只有一行。则审核立刻生效保存到数据库中。如果是多人会签，则等结果流转完成再更新到数据库。并通知对应的人员。
    /// </summary> 改为了0  1  bool
    //public enum ApprovalResults
    //{
    //    同意, 
    //    否决
    //}

    /// <summary>
    /// 存货成本计算方式 的摘要说明。如先进先出法（FIFO）、加权平均法（WA）
    /// </summary>
    public enum 库存成本计算方式
    {
        /// <summary>
        /// 适用范围：适用于存货的实物流转比较符合先进先出的假设，比如食品、药品等有保质期限制的商品，先购进的存货会先发出销售。
        /// </summary>
        先进先出法 = 0,

        /// <summary>
        /// 类似移动平均成本法。只是在月结时处理，即每个月的结账时，重新计算一次加权平均单位成本，然后以这个成本来计算当次月销售成本
        /// </summary>
        月加权平均 = 1,


        /// <summary>
        /// 适用范围：这种方法适用于企业存货收发频繁的情况，能较为及时、准确地反映存货成本的变化。例如在零售企业、电商企业等存货流转速度快的企业比较适用。
        /// 计算方法：
        ///每次进货时，都要重新计算存货的加权平均单位成本。计算公式为：加权平均单位成本 = (原有库存存货实际成本 + 本次进货实际成本)÷(原有库存存货数量 + 本次进货数量)。
        ///例如，企业期初库存商品 A 有 100 件，单位成本为 10 元，总成本就是 100×10 = 1000 元。本期第一次购进商品 A 50 件，单位成本为 12 元，此时加权平均单位成本 = (1000 + 50×12)÷(100 + 50)=(1000+600)÷150 = 10.67 元。
        ///当销售商品时，就以这个加权平均单位成本来计算销售成本。假设销售了 80 件，销售成本 = 80×10.67 = 853.6 元。库存商品成本 = (100 + 50 - 80)×10.67 = 746.9 元。
        ///下次再进货时，继续按照上述公式重新计算加权平均单位成本，以此类推，随着每次进货和销售不断更新存货的单位成本和总成本。
        /// </summary>
        移动加权平均法 = 2,

        /// <summary>
        /// ERP系统会根据实际发生的成本来计算产品成本，包括材料、人工和制造费用
        /// 缴库时要用到
        /// </summary>
        实际成本法 = 3,
    }

    public enum 企业基本类型
    {
        新会计制度企业 = 0,
        商业企业 = 1,
        工程施工企业 = 2,
        餐饮旅游 = 3,
        交通运输 = 4,
        工业企业 = 5
    }




}
