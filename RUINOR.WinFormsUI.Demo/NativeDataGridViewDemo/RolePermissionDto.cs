using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINOR.WinFormsUI.Demo.NativeDataGridViewDemo
{
    /// <summary>
    /// 角色权限数据传输对象（模拟 UCUserAuthorization 中的权限数据场景）
    /// </summary>
    public class RolePermissionDto
    {
        /// <summary>
        /// 角色 ID
        /// </summary>
        [DisplayName("角色 ID")]
        public long RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [DisplayName("角色名称")]
        public string RoleName { get; set; }

        /// <summary>
        /// 已授权（布尔属性 - 用于测试 CheckBox 显示）
        /// </summary>
        [DisplayName("已授权")]
        public bool Authorized { get; set; }

        /// <summary>
        /// 默认角色（布尔属性 - 用于测试 CheckBox 显示）
        /// </summary>
        [DisplayName("默认角色")]
        public bool DefaultRole { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Description { get; set; }
    }
}
