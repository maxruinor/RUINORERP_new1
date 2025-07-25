﻿using RUINORERP.Common;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RUINORERP.Model.ModuleMenuDefine;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 通过特性找到标记的窗体 转换为菜单
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class MenuAttrAssemblyInfo : Attribute
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get; set; }
        public string ClassName { get; set; }
        public string ClassPath { get; set; }
        /// <summary>
        /// 窗体类类型
        /// </summary>
        public Type ClassType { get; set; }

        private string _menuPath = "|";
        public string MenuPath
        {
            get => _menuPath; set => _menuPath = value;
        }

        /// <summary>
        /// 借财务模块合并建表但是菜单分开时，用接口来标记在菜单子类中
        /// </summary>
        public string BizInterface { get; set; }


        public string UIPropertyIdentifier { get; set; }
        /// <summary>
        /// 框架性基类
        /// </summary>
        public string BIBaseForm { get; set; }

        /// <summary>
        /// 业务性基类
        /// </summary>
        public string BIBizBaseForm { get; set; }

        /// <summary>
        /// 标识是什么样的类型与业务有关系
        /// </summary>
        public UIType UiType { get; set; }

        /// <summary>
        /// 业务类型 业务类型关联到全局的业务类型枚举定义以及单据转换工厂
        /// </summary>
        public BizType? MenuBizType { get; set; }


        /// <summary>
        /// 对应的实体名
        /// </summary>
        public string EntityName { get; set; }

        public MenuAttrAssemblyInfo()
        {

        }

        #region old

        /// <summary>
        /// 描述内容
        /// </summary>
        public string Describe { get; private set; }

        /// <summary>
        /// 菜单路径|分割 如果这个为空则是没有直接菜单进入，是通过其他窗体弹出
        /// </summary>
        //public string MenuPath { get; private set; }

        ///// <summary>
        ///// 窗体类型
        ///// </summary>
        //public Type FormType { get; private set; }

        ///// <summary>
        ///// 标识是什么样的类型与业务有关系  暂时不使用？
        ///// </summary>
        //public UIType UiType { get; set; }


        public bool Enabled { get; set; }



        /// <summary>
        /// 窗口式的。无路径
        /// </summary>
        /// <param name="type"></param>
        /// <param name="describe"></param>
        /// <param name="isForm"></param>
        /// <param name="_MenuPath">菜单路径|分割</param>
        public MenuAttrAssemblyInfo(string describe, bool enabled, UIType uiType)
        {
            this.Describe = describe;
            Enabled = enabled;
            UiType = uiType;
        }


        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 生产管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }

        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 进销存管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }

        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 财务管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }

        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 基础资料 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }

        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 行政管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }

        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 系统设置 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }

        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 报表管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }

        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 客户关系 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }

        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 售后管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }


        public MenuAttrAssemblyInfo(string describe, 模块定义 所属模块, 电商运营 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenuBizType = bizType[0];
            }
        }


        #endregion

        public MenuAttrAssemblyInfo(string _name, string _caption, string _ClassPath, string _menuName)
        {
            ClassName = _name;
            Caption = _caption;
            ClassPath = _ClassPath;
            MenuPath = _menuName;

        }

    }

}
