using Microsoft.Extensions.Logging;
using RUINORERP.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using AutoMapper;
using RUINORERP.UI.HelpSystem.Core;
using RUINORERP.UI.HelpSystem.Extensions;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// outlook
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public partial class BaseNavigator : UserControl
    {
        #region 帮助系统集成

        /// <summary>
        /// 是否启用智能帮助
        /// </summary>
        [Category("帮助系统")]
        [Description("是否启用智能帮助功能")]
        public bool EnableSmartHelp { get; set; } = true;

        /// <summary>
        /// 窗体帮助键
        /// </summary>
        [Category("帮助系统")]
        [Description("窗体帮助键,留空则使用控件类型名称")]
        public string FormHelpKey { get; set; }

        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        protected virtual void InitializeHelpSystem()
        {
            if (!EnableSmartHelp) return;

            try
            {
                // 为控件启用智能提示
                HelpManager.Instance.EnableSmartTooltipForAll(this, FormHelpKey);

                // 启用F1帮助
                this.EnableF1Help();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化帮助系统失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示窗体帮助
        /// </summary>
        public void ShowFormHelp()
        {
            if (!EnableSmartHelp) return;
            HelpManager.Instance.ShowControlHelp(this);
        }

        #endregion

        public BaseNavigator()
        {
            InitializeComponent();

            // 初始化帮助系统
            InitializeHelpSystem();
        }


        /// <summary>
        /// 当前窗体的菜单信息
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; } = new tb_MenuInfo();

        public NewSumDataGridView BaseMainDataGridView { get; set; }
 
    }
}
