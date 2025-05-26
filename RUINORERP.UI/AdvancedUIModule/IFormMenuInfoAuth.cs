using RUINORERP.Model;
using RUINORERP.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.AdvancedUIModule
{

    //通过两个接口来 额外定义菜单，一个是工具栏的，一个是右键上下文的，分别对应编辑和查询



    /// <summary>
    /// 给窗体添加一些特殊的权限控制等
    /// 添加工具栏按钮
    /// </summary>
    public interface IToolStripMenuInfoAuth
    {
        /// <summary>
        /// 根据菜单判断来扩展可能要额外添加的按钮
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo);
    }

    /// <summary>
    /// 右键上下文菜单
    /// </summary>
    public interface IContextMenuInfoAuth
    {
        /// <summary>
        /// 根据菜单判断来扩展可能要额外添加的按钮
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        List<UI.UControls.ContextMenuController> AddContextMenu();
    }

    /// <summary>
    ///  窗体中的显示会要控制到 除主子表之外的公共性的实体。如单据的产品公共部分。
    /// </summary>
    //public interface IPublicEntityObject
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="menuInfo"></param>
    //    /// <returns></returns>
    //    List<BaseEntity> PublicEntityObjects { get; set; }
    //}
}
