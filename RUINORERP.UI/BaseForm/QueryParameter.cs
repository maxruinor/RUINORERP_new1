using RUINORERP.Business.Processor;
using RUINORERP.Global;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 工作台节点事件中传递参数实体
    /// </summary>
    public class QueryParameter
    {
        public List<IConditionalModel> conditionals { get; set; }

        public QueryFilter queryFilter { get; set; }
        public BizType bizType { get; set; }

        /// <summary>
        /// 工作台中 如果是 共享用一个表 表达了多种业务时区别菜单用。对应共享的子类业务的每个窗体的标记
        /// </summary>
        public string UIPropertyIdentifier { get; set; }
        public Type tableType { get; set; }
        
        /// <summary>
        /// 单据主键ID列表
        /// 用于存储需要筛选的单据主键集合
        /// </summary>
        public List<long> BillIds { get; set; }

        /// <summary>
        /// 是否包含指定的BillIds进行筛选
        /// 当为true时，表示只查询BillIds列表中的单据
        /// 当为false或BillIds为空时，不进行主键筛选
        /// </summary>
        public bool IncludeBillIds { get; set; }
        
        /// <summary>
        /// 主键字段名称
        /// 根据不同业务类型可能有不同的主键字段名
        /// </summary>
        public string PrimaryKeyFieldName { get; set; }
        
        /// <summary>
        /// 存储完整的数据集合
        /// 用于本地更新操作，避免重复查询数据库
        /// </summary>
        public DataTable Data { get; set; }
    }
}
