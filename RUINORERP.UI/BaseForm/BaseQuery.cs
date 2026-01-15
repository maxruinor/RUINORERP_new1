using RUINORERP.Business.Processor;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.HelpSystem.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem.Core;

namespace RUINORERP.UI.BaseForm
{
    public partial class BaseQuery : UserControl
    {
        public BaseQuery()
        {
            InitializeComponent();

            // 初始化帮助系统
            InitializeHelpSystem();
        }

        #region 帮助系统集成

        /// <summary>
        /// 是否启用智能帮助系统
        /// </summary>
        [Browsable(true)]
        [Category("帮助系统")]
        [Description("是否启用智能帮助系统")]
        public bool EnableSmartHelp { get; set; } = true;

        /// <summary>
        /// 窗体帮助键(可选,覆盖默认值)
        /// </summary>
        [Category("帮助系统")]
        [Description("窗体帮助键,留空则使用窗体类型名称")]
        public string FormHelpKey { get; set; }

        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        protected virtual void InitializeHelpSystem()
        {
            if (!EnableSmartHelp) return;

            try
            {
                // 启用F1帮助
                this.EnableF1Help();

                // 启用智能提示(避免设计模式时报错)
                if (!this.DesignMode && System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
                {
                    // 为所有控件启用智能提示
                    HelpManager.Instance.EnableSmartTooltipForAll(this, FormHelpKey);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化帮助系统失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示控件帮助
        /// </summary>
        /// <param name="control">目标控件</param>
        protected void ShowControlHelp(Control control)
        {
            if (EnableSmartHelp)
            {
                HelpManager.Instance.ShowControlHelp(control);
            }
        }

        #endregion

        /// <summary>
        /// 当前窗体的菜单信息
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; } = new tb_MenuInfo();

        public NewSumDataGridView BaseMainDataGridView { get; set; }
        public NewSumDataGridView BaseSubDataGridView { get; set; }

        /// <summary>
        /// 从工作台点击过来的时候，这个保存初始化时的查询参数
        ///  这个可用可不用。
        /// </summary>
        public object QueryDtoProxy { get; set; } 


        /// <summary>
        /// 传查询参数对象，对象已经给了查询参数具体值，具体在窗体那边判断
        /// </summary>
        /// <param name="QueryParameters"></param>
        internal virtual void LoadQueryParametersToUI(object loadItems)
        {

        }

        /// <summary>
        /// 传查询参数对象，对象已经给了查询参数具体值，具体在窗体那边判断
        /// </summary>
        /// <param name="QueryParameters"></param>
        internal virtual void LoadQueryParametersToUI(object QueryParameters, QueryParameter nodeParameter = null)
        {
            //
        }


        /// <returns></returns>
        internal virtual object LoadQueryConditionToUI(decimal QueryConditionShowColQty)
        {

            return null;
        }



        /// <summary>
        /// 设置GridView显示配置
        /// 主子表查询与单表查询是不是命名要统一起来？
        /// </summary>
        public virtual void SetGridViewDisplayConfig()
        {

        }



        #region 如果窗体，有些按钮不用出现在这个业务窗体时。这里手动排除。集合有值才行
        
        List<MenuItemEnums> _excludeMenuList = new List<MenuItemEnums>();
        public List<MenuItemEnums> ExcludeMenuList { get => _excludeMenuList; set => _excludeMenuList = value; }

        List<string> _excludeMenuTextList = new List<string>();
        public List<string> ExcludeMenuTextList { get => _excludeMenuTextList; set => _excludeMenuTextList = value; }

 
        public virtual void AddExcludeMenuList(string menuItemText)
        {
            ExcludeMenuTextList.Add(menuItemText);
        }

        /// <summary>
        /// 如果查询窗体，有些按钮不用出现在这个业务窗体时。这里手动排除
        /// </summary>
        /// <returns></returns>
        public virtual void AddExcludeMenuList()
        {

        }

        public virtual void AddExcludeMenuList(MenuItemEnums menuItem)
        {
            ExcludeMenuList.Add(menuItem);
        }
        #endregion


        #region 相反，如果就一两个生效，我要手动指定设置菜单，那么不在这里指定的，则失效.这个优先处理,如果集合大于0，有值时

        List<MenuItemEnums> _includedMenuList = new List<MenuItemEnums>();
        public List<MenuItemEnums> IncludedMenuList { get => _includedMenuList; set => _includedMenuList = value; }
        /// <summary>
        /// 如果查询窗体，有些按钮不用出现在这个业务窗体时。这里手动排除
        /// </summary>
        /// <returns></returns>
        public virtual void AddIncludedMenuList()
        {

        }

        public virtual void AddIncludedMenuList(MenuItemEnums menuItem)
        {
            IncludedMenuList.Add(menuItem);
        }
        #endregion

    }
}
