using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using RUINORERP.Global;
using Krypton.Toolkit;
using RUINORERP.UI.BI;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.AdvancedUIModule;

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// 数据权限规则列表界面
    /// 支持标准编辑和智能编辑两种模式
    /// </summary>
    [MenuAttrAssemblyInfo("数据权限规则", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCRowAuthPolicyList : BaseForm.BaseListGeneric<tb_RowAuthPolicy>, IToolStripMenuInfoAuth
    {
        private bool _useSmartEditor = false;

        public UCRowAuthPolicyList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCRowAuthPolicyEdit);
        }

        /// <summary>
        /// 初始化智能特性
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            // 创建智能添加按钮
            ToolStripButton btnSmartAdd = new ToolStripButton();
            btnSmartAdd.Text = "智能添加";
            btnSmartAdd.Size = new System.Drawing.Size(70, 25);
            btnSmartAdd.Tag = "SmartAdd";

            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, btnSmartAdd);
            btnSmartAdd.ToolTipText = "分配给指定业务员。";
            btnSmartAdd.Click += new System.EventHandler(this.BtnSmartAdd_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { btnSmartAdd };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;


        }

       
 

  

 

        /// <summary>
        /// 智能添加按钮点击事件
        /// </summary>
        private void BtnSmartAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _useSmartEditor = true;
                base.EditForm = typeof(UCRowAuthPolicyEditEnhanced);
                // 直接调用基类的Add方法
                base.Add();
                // 恢复默认编辑器
                _useSmartEditor = false;
                base.EditForm = typeof(UCRowAuthPolicyEdit);
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("智能添加失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 重写添加方法
        /// </summary>
        protected override Task Add()
        {
            // 确保使用正确的编辑器
            if (_useSmartEditor)
            {
                base.EditForm = typeof(UCRowAuthPolicyEditEnhanced);
            }
            else
            {
                base.EditForm = typeof(UCRowAuthPolicyEdit);
            }

            return base.Add();
        }

        /// <summary>
        /// 重写修改方法
        /// </summary>
        protected override void Modify()
        {
            // 编辑时使用智能编辑器
            base.EditForm = typeof(UCRowAuthPolicyEditEnhanced);
            base.Modify();
            // 恢复默认编辑器
            base.EditForm = typeof(UCRowAuthPolicyEdit);
        }
    }
}