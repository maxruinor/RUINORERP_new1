using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI
{

    public enum AdvQueryMenuItemEnums
    {
        新增,
        删除,
        修改,
        查询,
        选中,
        保存,
        关闭,
        属性,
    }

    public enum MenuItemEnums
    {
        新增,
        取消,
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
        预览,//打印预览
        设计,//报表设计
        功能,
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

}
