using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using WorkflowCore.Interface;
using RUINORERP.UI.WorkFlowTester;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices;
using RUINORERP.Services;
using RUINORERP.Repository.Base;
using RUINORERP.IRepository.Base;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using System.Linq.Expressions;
using RUINORERP.Extensions.Middlewares;
using SqlSugar;
using RUINORERP.UI.Report;
using System.Reflection;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using System.Collections.Concurrent;
using Org.BouncyCastle.Math.Field;
using RUINORERP.UI.PSI.PUR;
using System.Runtime.InteropServices;
using RUINOR.WinFormsUI.TreeViewThreeState;
using StackExchange.Redis;
using FastReport.DevComponents.DotNetBar.Controls;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.UI.UserCenter;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 只创建一个UI没有继续了。
    /// </summary>
    [MenuAttrAssemblyInfo("流程配置", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.流程设计)]
    public partial class UCProcessConfig : UserControl
    {
        public UCProcessConfig()
        {
            InitializeComponent();
            this.BaseToolStrip.ItemClicked += ToolStrip1_ItemClicked;
        }
        protected virtual void ToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = e.ClickedItem.Text;
            if (e.ClickedItem.Text.Length > 0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(e.ClickedItem.Text));
            }
        }
        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {
                case MenuItemEnums.删除:
                    if (bindingSourceList.Current == null)
                    {
                        return;
                    }
                    break;
                case MenuItemEnums.修改:
                    toolStripButtonSave.Enabled = true;
                    break;
                case MenuItemEnums.保存:
                    //操作前将数据收集
                    this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
                    Save();
                    break;
                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                default:
                    break;
            }


        }

        private bool editflag;

        /// <summary>
        /// 是否为编辑 如果为是则true
        /// </summary>
        public bool Edited
        {
            get { return editflag; }
            set { editflag = value; }
        }
        protected virtual void Exit(object thisform)
        {
            if (!Edited)
            {
                //退出
                CloseTheForm(thisform);
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗?   这里是不是可以进一步提示 哪些内容没有保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //退出
                    CloseTheForm(thisform);
                }
            }
        }
        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        Exit(this);//csc关闭窗体
                        break;
                }

            }
            //return false;
            var key = keyData & Keys.KeyCode;
            var modeifierKey = keyData & Keys.Modifiers;
            if (modeifierKey == Keys.Control && key == Keys.F)
            {
                // MessageBox.Show("Control+F is pressed");
                return true;

            }
            return base.ProcessCmdKey(ref msg, keyData);

        }


        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            if ((thisform as Control).Parent is KryptonPage)
            {
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
            else
            {
                Form frm = (thisform as Control).Parent.Parent as Form;
                frm.Close();
            }


            /*
           if (page == null)
           {
               //浮动

           }
           else
           {
               //活动内
               if (cell.Pages.Contains(page))
               {
                   cell.Pages.Remove(page);
                   page.Dispose();
               }
           }
           */
        }

        Type[] ModelTypes;
        /// <summary>
        /// 为了查找明细表名类型，保存所有类型名称方便查找
        /// </summary>
        List<string> typeNames = new List<string>();

        List<SugarTable> stlist = new List<SugarTable>();

        private void UCWorkCenterConfig_Load(object sender, EventArgs e)
        {
            TreeView1.HideSelection = false;
            TreeView1.CheckBoxes = false;
            //实现的对应的重绘方法tVtypeList_DrawNode
            TreeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            LoadTreeView();
            LoadToDolist();
            LoadDataOverview();
        }


        private void LoadToDolist()
        {
            kryptonCheckedListBox待办事项.Items.Clear();
            foreach (待办事项 item in Enum.GetValues(typeof(待办事项)))
            {
                kryptonCheckedListBox待办事项.Items.Add(item.ToString());
            }
        }

        private void LoadDataOverview()
        {
            kryptonCheckedListBox数据概览.Items.Clear();
            foreach (数据概览 item in Enum.GetValues(typeof(数据概览)))
            {
                kryptonCheckedListBox数据概览.Items.Add(item.ToString());
            }
        }

        /// <summary>
        /// 将模块 菜单 显示为树
        /// </summary>
        private async void LoadTreeView()
        {
            TreeView1.Nodes.Clear();

            //加载角色 及个人,可以针对角色或个人配置。个人优先。
            tb_User_RoleController<tb_User_Role> ctrUserRole = Startup.GetFromFac<tb_User_RoleController<tb_User_Role>>();
            List<tb_User_Role> tb_User_Roles = await ctrUserRole.QueryRoleByNavWithMoreInfo();

            ThreeStateTreeNode nd = new ThreeStateTreeNode();
            nd.Text = "工作台配置对象";
            //加载模块 模块和顶级菜单相同
            //  AddTopTreeNode(Modules, nd, MenuInfoList, 0);
            AddUserTreeNodeByRole(tb_User_Roles, nd);
            TreeView1.Nodes.Add(nd);
            TreeView1.Nodes[0].Expand();
            //}
        }

        private void TreeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Node.Bounds);
            if (e.State == TreeNodeStates.Selected)//做判断
            {
                e.Graphics.FillRectangle(Brushes.CornflowerBlue, new Rectangle(e.Node.Bounds.Left, e.Node.Bounds.Top, e.Node.Bounds.Width, e.Node.Bounds.Height));//背景色为蓝色
                RectangleF drawRect = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height);
                e.Graphics.DrawString(e.Node.Text, TreeView1.Font, Brushes.White, drawRect);//字体为白色
            }
            else
            {
                e.DrawDefault = true;
            }
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="UserRolesList"></param>
        /// <param name="nd"></param>
        private void AddUserTreeNodeByRole(List<tb_User_Role> UserRolesList, ThreeStateTreeNode nd)
        {
            //先加载角色，后加载用户，角色目前只支持一级
            var Roles = UserRolesList.GroupBy(c => c.RoleID);
            foreach (var role in Roles)
            {
                tb_User_Role user_Role = UserRolesList.FirstOrDefault(c => c.RoleID == role.Key && c.Authorized);
                if (user_Role != null)
                {                //建立一个新节点
                    ThreeStateTreeNode node = new ThreeStateTreeNode(user_Role.tb_roleinfo.RoleName);
                    node.Checked = true;
                    node.Name = role.Key.ToString();
                    node.Tag = user_Role;
                    node.Text = user_Role.tb_roleinfo.RoleName;
                    //模块与菜单关系是用枚举的硬编码 在窗体的特性中定义标记的
                    //初始化时已经将模块ID保存为外键到菜单中关联起来了
                    AddTreeNode(node, UserRolesList, user_Role.tb_roleinfo.RoleID);
                    nd.Nodes.Add(node);

                }

            }
        }

        //添加用户
        private void AddTreeNode(ThreeStateTreeNode nd, List<tb_User_Role> UserRolesList, long RoleID)
        {
            List<tb_User_Role> selectList = UserRolesList.Where(m => m.RoleID == RoleID).ToList();
            var users = selectList.GroupBy(c => c.User_ID);
            foreach (var user in users)
            {
                tb_User_Role user_Role = UserRolesList.FirstOrDefault(c => c.RoleID == RoleID && c.User_ID == user.Key && c.Authorized);
                if (user_Role != null)
                {
                    string NodeText = "";
                    NodeText = user_Role.tb_userinfo.UserName;
                    //建立一个新节点
                    ThreeStateTreeNode node = new ThreeStateTreeNode(NodeText);
                    node.Tag = user_Role;
                    //node.ContextMenuStrip = contextMenuStrip4InitData;
                    //用户目前只支持一有，不用再循环
                    // AddTreeNode(node, list, item.MenuID);
                    nd.Nodes.Add(node);
                }

            }
        }

        /// <summary>
        /// 是否指向用户设置，否的话，只指向角色
        /// </summary>
        private bool IsUserSetting = false;


        tb_User_Role CurrentRole;
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (TreeView1.SelectedNode == null)
            {
                return;
            }

            if (!(TreeView1.SelectedNode.Tag is tb_User_Role))
            {
                return;
            }

            //加载前，清掉上次选择的
            for (int i = 0; i < kryptonCheckedListBox待办事项.Items.Count; i++)
            {
                kryptonCheckedListBox待办事项.SetItemCheckState(i, CheckState.Unchecked);
            }
            for (int i = 0; i < kryptonCheckedListBox数据概览.Items.Count; i++)
            {
                kryptonCheckedListBox数据概览.SetItemCheckState(i, CheckState.Unchecked);
            }

            tb_User_Role selectNode = TreeView1.SelectedNode.Tag as tb_User_Role;
            CurrentRole = selectNode;
            toolStripButtonSave.Enabled = true;
            tb_WorkCenterConfig workCenterConfig = null;
            List<string> ToDoList = new List<string>();
            List<string> DataOverviewList = new List<string>();
            //加载值
            if (TreeView1.SelectedNode.Text == selectNode.tb_roleinfo.RoleName)
            {
                IsUserSetting = false;
                //角色
                workCenterConfig = MainForm.Instance.AppContext.Db.Queryable<tb_WorkCenterConfig>()
                   .Where(c => c.RoleID == CurrentRole.RoleID && SqlFunc.EqualsNull(c.User_ID, null)).Single();
                //                    .Where(c => c.RoleID == CurrentRole.RoleID && SqlFunc.IsNull(c.User_ID, 0) == 0).Single();
            }
            else
            {
                IsUserSetting = true;
                //用户+角色
                workCenterConfig = MainForm.Instance.AppContext.Db.Queryable<tb_WorkCenterConfig>()
                   .Where(c => c.RoleID == CurrentRole.RoleID && c.User_ID == CurrentRole.User_ID).Single();
            }

            if (workCenterConfig != null)
            {
                ToDoList = workCenterConfig.ToDoList.Split(',').ToList();
                DataOverviewList = workCenterConfig.DataOverview.Split(',').ToList();
                for (int i = 0; i < kryptonCheckedListBox待办事项.Items.Count; i++)
                {
                    if (ToDoList.Contains(kryptonCheckedListBox待办事项.Items[i].ToString()))
                    {
                        kryptonCheckedListBox待办事项.SetItemCheckState(i, CheckState.Checked);
                    }
                }
                for (int i = 0; i < kryptonCheckedListBox数据概览.Items.Count; i++)
                {
                    if (DataOverviewList.Contains(kryptonCheckedListBox数据概览.Items[i].ToString()))
                    {
                        kryptonCheckedListBox数据概览.SetItemCheckState(i, CheckState.Checked);
                    }
                }
            }
        }
        protected virtual void Save()
        {
            if (CurrentRole == null)
            {
                MainForm.Instance.PrintInfoLog("请先选择角色或人员。");
                return;
            }

            //角色
            tb_WorkCenterConfig workCenterConfig = MainForm.Instance.AppContext.Db.Queryable<tb_WorkCenterConfig>()
                .Where(c => c.RoleID == CurrentRole.RoleID)
                .WhereIF(IsUserSetting && CurrentRole.User_ID > 0, c => c.User_ID == CurrentRole.User_ID)
                .Single();

            if (workCenterConfig == null)
            {
                workCenterConfig = new tb_WorkCenterConfig();
                long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                workCenterConfig.ConfigID = sid;
                workCenterConfig.RoleID = CurrentRole.RoleID;
                if (IsUserSetting)
                {
                    workCenterConfig.User_ID = CurrentRole.User_ID;
                }
            }
            string todolist = string.Empty;
            foreach (var item in kryptonCheckedListBox待办事项.CheckedItems)
            {
                todolist += item + ",";
            }

            string dataoverviewList = string.Empty;
            foreach (var item in kryptonCheckedListBox数据概览.CheckedItems)
            {
                dataoverviewList += item + ",";
            }

            workCenterConfig.ToDoList = todolist.TrimEnd(',');
            workCenterConfig.DataOverview = dataoverviewList.TrimEnd(',');

            workCenterConfig = MainForm.Instance.AppContext.Db.Storageable<tb_WorkCenterConfig>(workCenterConfig).ExecuteReturnEntity();
            toolStripButtonSave.Enabled = false;
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                if (menuItem.Owner is ContextMenuStrip contextMenu)
                {
                    if (contextMenu.SourceControl == kryptonCheckedListBox待办事项)
                    {
                        for (int i = 0; i < kryptonCheckedListBox待办事项.Items.Count; i++)
                        {
                            kryptonCheckedListBox待办事项.SetItemCheckState(i, CheckState.Checked);
                        }
                    }
                    if (contextMenu.SourceControl == kryptonCheckedListBox数据概览)
                    {
                        for (int i = 0; i < kryptonCheckedListBox数据概览.Items.Count; i++)
                        {
                            kryptonCheckedListBox数据概览.SetItemCheckState(i, CheckState.Checked);
                        }
                    }
                }
            }
        }

        private void selectNoAll_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                if (menuItem.Owner is ContextMenuStrip contextMenu)
                {
                    if (contextMenu.SourceControl == kryptonCheckedListBox待办事项)
                    {
                        for (int i = 0; i < kryptonCheckedListBox待办事项.Items.Count; i++)
                        {
                            kryptonCheckedListBox待办事项.SetItemCheckState(i, CheckState.Unchecked);
                        }
                    }
                    if (contextMenu.SourceControl == kryptonCheckedListBox数据概览)
                    {
                        for (int i = 0; i < kryptonCheckedListBox数据概览.Items.Count; i++)
                        {
                            kryptonCheckedListBox数据概览.SetItemCheckState(i, CheckState.Unchecked);
                        }
                    }
                }
            }
        }
    }
}
