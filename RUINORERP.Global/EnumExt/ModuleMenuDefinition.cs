using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    public class ModuleMenuDefine
    {
        public enum 模块定义
        {
            生产管理,
            进销存管理,
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
            外发加工,
            BOM分析报表,
            成本管理,
        }

        public enum 供应链管理
        {
            采购管理,
            库存管理,
            销售管理,
            品质检验,
        }

        public enum 客户关系
        {
            客户管理,
            任务管理,
            客户关系,
            订单分析,
            基础资料,
        }

        public enum 财务管理
        {
            账务管理,
            账薄打印,
            收付账款,
            固定资产,

            //报销业务-》报销单=》报销单明细=》报销单统计
            出纳管理,

        }

        public enum 行政管理
        {
            业务流管理,
            合同管理,
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
            异常日志,
            审计日志,
            流程设计,
            系统工具,
            动态参数
        }

    }
}
