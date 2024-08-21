using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    public class CurrentUserInfo : ICurrentUserInfo
    {
        /// <summary>
        /// 当前登录用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public long Id { get; set; }
 


        #region 数据库级别

        /// <summary>
        /// 设置了默认值
        /// </summary>
        public List<tb_ModuleDefinition> UserModList { get; set; } = new List<tb_ModuleDefinition>();

        //public List<tb_ButtonInfo> UserButtonList { get; set; } = new List<tb_ButtonInfo>();
        //public List<tb_FieldInfo> UserFieldList { get; set; } = new List<tb_FieldInfo>();
        //public List<tb_MenuInfo> UserMenuList { get; set; } = new List<tb_MenuInfo>();

        public tb_UserInfo UserInfo { get; set; } = new tb_UserInfo();
      
        #endregion

    }
}
