using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    public class ModuleMenuDefine
    {
        /// <summary>
        /// 里面的定义要与下面的枚举一一对应，不然会出错。这里是固定的菜单
        /// </summary>
        public enum 模块定义
        {
            生产管理,
            进销存管理,
            售后管理,
            客户关系,
            财务管理,
            行政管理,
            报表管理,
            基础资料,
            系统设置,
        }


        public enum 生产管理
        {
            MRP基本资料,
            制造规划,
            制程生产,
            生产品控,
            BOM分析报表,
            成本管理,
        }

        public enum 进销存管理
        {
            采购管理,
            销售管理,
            库存管理,
            借出归还,
            盘点管理,
            调拨管理,
            产品分割与组合,
            其他出入库管理,
        }
        public enum 售后管理
        {
            售后流程,
            维修中心,
            资产处置,
            提前交付,

            
        }
        public enum 客户关系
        {
            市场营销,
            客户管理,
            跟进管理,
            报价管理,
            合同管理,
            回款管理,
            开票管理,
            商机总览,
            //销售看板
            绩效分析,
        }

        public enum 财务管理
        {
            基础设置,
            收款管理,
            付款管理,
            费用管理,       //报销业务-》报销单=》报销单明细=》报销单统计
            固定资产,
            合同管理,
            发票管理,
        }

        public enum 行政管理
        {
            业务流管理,
            资料管理,
            基础资料,
            人事管理
        }

        public enum 报表管理
        {
            生产分析,
            进销存分析,
            综合分析,
            报表设置,
            基础资料,
        }
        public enum 基础资料
        {
            产品资料,
            仓库资料,
            供销资料,
            行政资料,
            财务资料,
            包装资料
        }

        public enum 系统设置
        {
            系统参数,
            权限管理,
            个性化设置,
            流程设计,
            系统工具,
            动态参数
        }

    }
}
