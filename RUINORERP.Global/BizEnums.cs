using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global
{

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
        请选择 = 1,
        单属性 = 2,
        可配置多属性 = 3,
        捆绑 = 4,
        虚拟 = 5
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


    public enum Module
    {
        生产,
        行政,
        库存,
        销售,
        采购
    }


    /// <summary>
    /// 审批状态
    /// </summary>
    public enum ApprovalStatus
    {
        未审核,
        已审核
    }

    /// <summary>
    /// 审批结果 是否需要有会签功能？https://www.likecs.com/show-747870.html
    /// </summary>
    public enum ApprovalResults
    {
        同意,
        拒绝
    }

    /// <summary>
    /// 存货成本计算方式 的摘要说明。
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
