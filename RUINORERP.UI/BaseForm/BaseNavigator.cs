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

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// outlook
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public partial class BaseNavigator : UserControl 
    {
        public BaseNavigator()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 当前窗体的菜单信息
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; } = new tb_MenuInfo();

        public NewSumDataGridView BaseMainDataGridView { get; set; }
 
    }
}
