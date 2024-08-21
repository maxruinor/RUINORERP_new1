using RUINORERP.Common;
using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RUINORERP.Model.ModuleMenuDefine;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 通过特性找到标记的窗体设置的属性或权限等个性化设置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class MenuPropertyAttribute : Attribute
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


        public string BIBaseForm { get; set; }

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

        public MenuPropertyAttribute()
        {

        }

        public MenuPropertyAttribute(string _name, string _caption, string _ClassPath, string _menuName)
        {
            ClassName = _name;
            Caption = _caption;
            ClassPath = _ClassPath;
            MenuPath = _menuName;

        }

    }

    public enum PropertyType
    {


    }


}
