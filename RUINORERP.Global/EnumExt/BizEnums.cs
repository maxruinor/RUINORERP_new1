using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global
{

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
        需要返回 = 1
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
        正常,
        重要,
        重要紧急,
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
        ProjectGroupCode
    }

    //上面只是测试，b

    /// <summary>
    /// 目前用在生成单号和审核时得到单据公共信息
    /// 因为是不是一次性写完，并且会保存到菜单配置表中。指定具体值更好，更灵活
    /// TODO:后面优化重构要将菜单数据库表中的biztype int 改为名称
    /// </summary>
    public enum BizType
    {
        销售订单,
        销售出库单,
        销售退回单,
        采购订单,
        采购入库单,
        采购入库退回单,
        其他入库单,
        其他出库单,
        返厂入库,
        返厂出库,
        售后入库,
        售后出库,
        报损单,
        报溢单,
        盘点单,
        制令单,
        BOM物料清单,
        生产领料单,
        生产退料单,
        生产补料单,
        发料计划单,
        成品缴库,//作废
        托外加工单,
        托外领料单,
        退料单,
        托外补料单,
        托外加工缴回单,
        人事考勤,
        其他费用支出,
        其他费用收入,
        费用报销单,
        库存查询,
        采购订单统计,
        采购入库统计 = 33,
        销售订单统计,
        销售出库统计,
        //添加暂时要添加到下面。菜单配置中 有一个业务类型用了int位置会乱，或是引用单据时，类型是这个枚举值
        生产需求分析,
        材料需求分析,
        库存跟踪,
        销售订单跟踪,
        采购订单跟踪,
        制令单跟踪,
        销售退回统计 = 43,
        销售数据汇总 = 44,
        采购数据汇总 = 45,
        库存数据汇总 = 46,
        生产计划单 = 47,
        缴库单 = 48,
        请购单 = 49,
        产品分割单 = 50,
        产品组合单 = 51,
        借出单 = 52,
        归还单 = 53,
        套装组合 = 54,
        包装信息 = 97,
        产品档案 = 98,
        默认数据 = 99,
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
    /// 单据记帐类型 是不是要分进销项/ 借 贷 
    /// 立账：此字段主要控制是否产生应付账款以及何时产生账款。若选择“记应付账”，则单据在存盘（终审）后，立即形成应付账款，若选择“不立账”，则不产生任何应付账款，比如对一些赠品就可以采用这样的处理，若选择“收票记账”，则不产生应付账款，在收到供应商的发票时，再形成应付账款。 
    /// </summary>
    public enum KeepAccountsType
    {
        不立帐,

        /// <summary>
        /// 自动生成应付单
        /// </summary>
        记应付帐,

        收票记帐,

        /// <summary>
        /// 自动生成应收单
        /// </summary>
        记应收帐,

        开票记帐,
    }


    /// <summary>
    /// 盘点调整模式
    /// </summary>
    public enum Adjust_Type
    {
        增加,
        减少,
        全部
    }

    /// <summary>
    /// 数据状态 单据才用结案
    /// </summary>
    public enum DataStatus
    {
        [Description("添加")]
        /// <summary>
        /// 草稿Draft
        /// </summary>
        草稿 = 1,


        [Description("提交")]
        /// <summary>
        /// 提交后才会推送给审核人员
        /// </summary>
        新建 = 2,


        [Description("审核")]
        /// <summary>
        /// Confimmed,
        /// </summary>
        确认 = 4,


        [Description("结案")]
        /// <summary>
        /// close
        /// </summary>
        //结案
        完结 = 8

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
        全部付款 = 2,
        部分付款 = 3,
    }


    /// <summary>
    /// 盘点方式
    /// </summary>
    public enum CheckMode
    {
        一般盘点,
        日常盘点,
        期初盘点,
    }

    /// <summary>
    /// 审批状态 如果需要多级 多人审核 设计会签功能  如业务审核，财务审核
    /// </summary>
    public enum ApprovalStatus
    {
        未审核 = 0,
        已审核 = 1,
        // 待审核 = 2,
        审核中 = 3,
        结案 = 4,
    }

    /// <summary>
    /// 审批结果 是否需要有会签功能？https://www.likecs.com/show-747870.html
    /// DB设计了一个审核配置主子表  配置了审核的人，比方 业务审核要求在明细一行，财务审核也在明细一行 两个人审核通过才能通过，但是这个是缓存在工作流引擎中的。如果引擎重启则
    /// 可能要重新审核，如果当时审核的配置只有一行。则审核立刻生效保存到数据库中。如果是多人会签，则等结果流转完成再更新到数据库。并通知对应的人员。
    /// </summary> 改为了0  1  bool
    //public enum ApprovalResults
    //{
    //    同意, 
    //    驳回
    //}

    /// <summary>
    /// 存货成本计算方式 的摘要说明。如先进先出法（FIFO）、加权平均法（WA）
    /// </summary>
    public enum 库存成本计算方式
    {
        先进先出法 = 0,
        后进先出法 = 1,
        加权平均法 = 2,
        移动平均法 = 3
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
