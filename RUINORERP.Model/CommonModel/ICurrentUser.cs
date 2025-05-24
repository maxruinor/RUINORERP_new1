using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RUINORERP.Model

{
    /// <summary>
    /// 当前用户 认为任何系统都会有一个当前用户
    /// </summary>
    public interface ICurrentUserInfo
    {
        /// <summary>
        /// 当前登录用户名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 当前登录用户ID empID
        /// </summary>
        long Id { get; set; }


        #region 数据库级别

        List<tb_ModuleDefinition> UserModList { get; set; }
        //List<tb_MenuInfo> UserMenuList { get; set; }
        //List<tb_ButtonInfo> UserButtonList { get; set; }
        //List<tb_FieldInfo> UserFieldList { get; set; }
        /// <summary>
        /// 保存了所有的相关的菜单 按钮 字段最全的 角色  个性化数据
        /// </summary>
        tb_UserInfo UserInfo { get; set; }

        #endregion
    }
}