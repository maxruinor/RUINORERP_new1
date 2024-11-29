using AutoMapper;
using Krypton.Navigator;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserCenter.DataParts
{

    /// <summary>
    /// 控制工作单元显示和排序
    /// </summary>
    public partial class UCCellSetting : UserControl
    {
        public UCCellSetting()
        {
            InitializeComponent();

            this.kryptonTreeViewCells.AfterCheck += KryptonTreeViewCells_AfterCheck;
        }

        private void KryptonTreeViewCells_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // e.Node
            //导航到指向的单据界面
            if (e.Node != null && e.Node.Tag is KryptonPage kp)
            {
                kp.Visible = e.Node.Checked; //切换是否显示.
            }
        }

        /// <summary>
        /// 要控制的单元 不包含自己
        /// </summary>
        public KryptonPageCollection Kpages { get; set; } = new KryptonPageCollection();

        
        private void RefreshData_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                if (menuItem.Owner is ContextMenuStrip contextMenu)
                {
                    if (contextMenu.SourceControl.Parent == kryptonTreeViewCells)
                    {
                        if (kryptonTreeViewCells.Nodes[0].Tag is tb_WorkCenterConfig config)
                        {
                            BuilderCellListTreeView(config);
                        }
                    }
                }
            }
            //tb_WorkCenterConfig

        }
       

        /// <summary>
        /// 绑定工作单元个数
        /// </summary>
        private void BuilderCellListTreeView(tb_WorkCenterConfig centerConfig)
        {
            kryptonTreeViewCells.Nodes.Clear();
            TreeNode nd = new TreeNode();
            nd.Text = "工作单元设置";
            nd.ImageIndex = 0;
            nd.Tag = centerConfig;
            List<string> DataOverviewItems = centerConfig.DataOverview.Split(',').ToList();
            foreach (var item in DataOverviewItems)
            {
                if (item.IsNullOrEmpty())
                {
                    continue;
                }
                数据概览 DataOverview = (数据概览)Enum.Parse(typeof(数据概览), item);
                var kp = Kpages.FirstOrDefault(c => c.Name == DataOverview.ToString()
                && !c.Name.Equals(GlobalConstants.UCCellSettingName));
                if (kp != null)
                {
                    TreeNode node = new TreeNode(kp.Text);
                    node.Tag = kp;
                    node.Checked = kp.Visible;
                    nd.Nodes.Add(node);
                }
            }

            //添加重新加载的菜单
            nd.ContextMenuStrip = contextMenuStrip1;
            kryptonTreeViewCells.Nodes.Add(nd);
            //  kryptonTreeViewJobList.Nodes[0].Expand();
            kryptonTreeViewCells.ExpandAll();
        }

        private void UCTodoList_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            //月销售，依客戶，依业务
            tb_RoleInfo CurrentRole = MainForm.Instance.AppContext.CurrentRole;
            tb_UserInfo CurrentUser = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
            //先取人，无人再取角色。
            tb_WorkCenterConfig centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID && c.User_ID == CurrentUser.User_ID);
            if (centerConfig == null)
            {
                centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID);
            }
            BuilderCellListTreeView(centerConfig);

        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 绑定工作单元个数
        /// </summary>
        public void BuilderCellListTreeView()
        {
            kryptonTreeViewCells.Nodes.Clear();
            TreeNode nd = new TreeNode();
            nd.Text = "工作单元设置";
            nd.ImageIndex = 0;

            foreach (var kp in Kpages)
            {
                if (!kp.Name.Equals(GlobalConstants.UCCellSettingName))
                {
                    TreeNode node = new TreeNode(kp.Text);
                    node.Tag = kp;
                    node.Checked = kp.Visible;
                    nd.Nodes.Add(node);
                }
            }

            //添加重新加载的菜单
            nd.ContextMenuStrip = contextMenuStrip1;
            kryptonTreeViewCells.Nodes.Add(nd);
            //  kryptonTreeViewJobList.Nodes[0].Expand();
            kryptonTreeViewCells.ExpandAll();
        }


    }
}
