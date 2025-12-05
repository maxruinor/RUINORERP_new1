using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global
{

    public enum MenuItemEnums
    {
        新增,
        复制性新增,
        数据特殊修正,
        删除,
        修改,
        提交,
        查询,
        选中,
        保存,
        关闭,
        刷新,
        导入,
        导出,
        打印,

        //打印预览
        预览,

        //报表设计
        设计,

        功能,
        联查,
        转单,
        结案,
        反结案,
        属性,
        审核,
        反审,
        帮助,
        已锁定,
    }


    public enum BaseListRunWay
    {
        /// <summary>
        /// 主菜单
        /// </summary>
        菜单,
        /// <summary>
        /// 暂时定义为菜单下面的
        /// </summary>
        快捷栏,

        /// <summary>
        /// 右边
        /// </summary>
        工具栏,
        /// <summary>
        /// 基本编辑业务数据时带出
        /// </summary>
        窗体,

        /// <summary>
        /// 基本编辑业务数据时带出
        /// </summary>
        选中模式,
    }

    public enum ButtonType
    {
        /// <summary>
        /// 上下文右键菜单
        /// </summary>
        ContextMenu = 1,


        /// <summary>
        /// 按钮
        /// </summary>
        Button,
 
        /// <summary>
        /// 链接
        /// </summary>
        Link,

        //工具栏按钮
        Toolbar
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum QueryFieldType
    {

        String,

        Money,

        Qty,

        /// <summary>
        /// 下拉枚举
        /// </summary>
        CmbEnum,

        /// <summary>
        /// 下拉数据源
        /// </summary>
        CmbDbList,

        DateTime,

        DateTimeRange,

        CheckBox,

        RdbYesNo,

    }

}
