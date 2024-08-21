using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static RUINORERP.Model.ModuleMenuDefine;

namespace RUINORERP.Common
{
    //判断菜单引用方面的使用 
    /// <summary>
    /// 这个Attribute就是使用时候的验证，把它添加到要缓存数据的方法中，即可完成缓存的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class MenuAttributeXXX : Attribute
    {
        /// <summary>
        /// 不能去掉，注入会用到？
        /// </summary>
        public MenuAttributeXXX()
        {

        }

        /// <summary>
        /// 描述内容
        /// </summary>
        public string Describe { get; private set; }

        /// <summary>
        /// 菜单路径|分割 如果这个为空则是没有直接菜单进入，是通过其它窗体弹出
        /// </summary>
        public string MenuPath { get; private set; }

        ///// <summary>
        ///// 窗体类型
        ///// </summary>
        //public Type FormType { get; private set; }

        /// <summary>
        /// 标识是什么样的类型与业务有关系  暂时不使用？
        /// </summary>
        public UIType UiType { get; set; }

        /// <summary>
        /// 业务类型关联到全局的业务类型枚举定义以及单据转换工厂
        /// </summary>
        public BizType? MenubizType { get; set; }

        public bool Enabled { get; set; }



        /// <summary>
        /// 窗口式的。无路径
        /// </summary>
        /// <param name="type"></param>
        /// <param name="describe"></param>
        /// <param name="isForm"></param>
        /// <param name="_MenuPath">菜单路径|分割</param>
        public MenuAttributeXXX(string describe, bool enabled, UIType uiType)
        {
            this.Describe = describe;
            Enabled = enabled;
            UiType = uiType;
        }


        public MenuAttributeXXX(string describe, 模块定义 所属模块, 生产管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenubizType = bizType[0];
            }
        }

        public MenuAttributeXXX(string describe, 模块定义 所属模块, 进销存管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenubizType = bizType[0];
            }
        }

        public MenuAttributeXXX(string describe, 模块定义 所属模块, 财务管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenubizType = bizType[0];
            }
        }

        public MenuAttributeXXX(string describe, 模块定义 所属模块, 基础资料 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenubizType = bizType[0];
            }
        }

        public MenuAttributeXXX(string describe, 模块定义 所属模块, 行政管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenubizType = bizType[0];
            }
        }

        public MenuAttributeXXX(string describe, 模块定义 所属模块, 报表管理 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenubizType = bizType[0];
            }
        }

        public MenuAttributeXXX(string describe, 模块定义 所属模块, 客户关系 NextNavMenu, params BizType[] bizType)
        {
            string _MenuPath = string.Empty;
            _MenuPath = 所属模块.ToString() + "|" + NextNavMenu.ToString() + "|";
            this.Describe = describe;
            this.MenuPath = _MenuPath;
            Enabled = true;
            UiType = UIType.单表数据;
            if (bizType != null && bizType.Length > 0)
            {
                MenubizType = bizType[0];
            }
        }



    }
}
