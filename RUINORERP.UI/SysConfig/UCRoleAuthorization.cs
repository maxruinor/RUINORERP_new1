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
using RUINORERP.Model;
using RUINORERP.Business;
using System.Linq.Expressions;
using SqlSugar;
using RUINORERP.Common.Helper;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using System.Reflection;
using RUINORERP.UI.Common;
using System.Collections.Concurrent;
using RUINORERP.Model.Dto;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINOR.WinFormsUI.TreeViewThreeState;
using RUINORERP.UI.UControls;
using FastReport.DevComponents.DotNetBar.Controls;
using StackExchange.Redis;
using System.Threading;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Extensions.Middlewares;

using RUINORERP.Business.CommService;
using System.Windows.Documents;
using FastReport.Forms;
using System.Diagnostics;
using OpenTK.Input;
using Castle.Components.DictionaryAdapter.Xml;
using log4net.Repository.Hierarchy;
using RUINORERP.UI.SS;
using RUINORERP.UI.Monitoring.Auditing;
using Mysqlx.Crud;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Global;
using Netron.GraphLib;
using Microsoft.Extensions.DependencyInjection;
using System.Management;
using RUINORERP.PacketSpec.Models.Core;



namespace RUINORERP.UI.SysConfig
{

    /// <summary>
    /// 比用户授权角色简单，那个是行记录存在性控制， 这里是默认每个角色都有。通过关系表中的字段来控制的
    /// </summary>
    [MenuAttrAssemblyInfo("角色授权", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCRoleAuthorization : UserControl, IContextMenuInfoAuth
    {
        private readonly IRowAuthService _rowAuthService;
        // 缓存默认的行级权限策略，避免重复从数据库加载
        private Dictionary<BizType, List<tb_RowAuthPolicy>> _policyCache = new Dictionary<BizType, List<tb_RowAuthPolicy>>();
        public GridViewDisplayTextResolver DisplayTextResolver;
        public UCRoleAuthorization()
        {
            InitializeComponent();
            // 在窗体构造函数中
            typeof(TreeView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, TreeView1, new object[] { true });
            BuildContextMenuController();


            _rowAuthService = Startup.GetFromFac<IRowAuthService>();

            newSumDataGridViewRowAuthPolicy.CellFormatting += new DataGridViewCellFormattingEventHandler(DataGridView3_CellFormatting);
        }


        #region 检测是否重复
        public List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_检测是否重复);
            ContextClickList.Add(NewSumDataGridView_删除行级权限);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【检测是否重复】", true, false, "NewSumDataGridView_检测是否重复"));
            list.Add(new ContextMenuController("【删除行级权限】", true, false, "NewSumDataGridView_删除行级权限"));
            return list;
        }
        public void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_检测是否重复);
            ContextClickList.Add(NewSumDataGridView_删除行级权限);

            List<ContextMenuController> list = new List<ContextMenuController>();
            list = AddContextMenu();

            if (dataGridView1 != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = this.dataGridView1.GetContextMenu(this.dataGridView1.ContextMenuStrip
                , ContextClickList, list, true
                    );
                dataGridView1.ContextMenuStrip = newContextMenuStrip;
            }

            if (dataGridView2 != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = this.dataGridView2.GetContextMenu(this.dataGridView2.ContextMenuStrip
                , ContextClickList, list, true
                    );
                dataGridView2.ContextMenuStrip = newContextMenuStrip;
            }

            if (newSumDataGridViewRowAuthPolicy != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = this.newSumDataGridViewRowAuthPolicy.GetContextMenu(this.newSumDataGridViewRowAuthPolicy.ContextMenuStrip
                , ContextClickList, list, true
                    );
                newSumDataGridViewRowAuthPolicy.ContextMenuStrip = newContextMenuStrip;
            }
        }

        private void NewSumDataGridView_检测是否重复(object sender, EventArgs e)
        {
            tb_MenuInfo selectMenu = TreeView1.SelectedNode.Tag as tb_MenuInfo;
            if (kryptonNavigator1.SelectedPage.Name == kryptonPageBtn.Name)
            {
                dataGridView1.UseSelectedColumn = true;
                if (!dataGridView1.UseSelectedColumn)
                {
                    //请先开启多模式，重复结果将会勾选
                    MessageBox.Show("请先开启多模式，重复结果将会勾选");
                    return;
                }

                // 定义参与比较的属性列表
                var ButtonProperties = new List<string>()
                    .Include<tb_P4Button>(c => c.ButtonInfo_ID)
                    .Include<tb_P4Button>(c => c.RoleID);

                //数据源开始绑定时用的BindingSortCollection
                var oklist = UITools.CheckDuplicateData<tb_P4Button>(ListDataSoure1.Cast<tb_P4Button>().ToList(), ButtonProperties.ToList());
                List<long> buttonids = oklist.Select(c => c.P4Btn_ID).ToList();
                if (dataGridView1.UseSelectedColumn)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        var dr = dataGridView1.Rows[i];
                        if (dr.DataBoundItem is tb_P4Button p4Button)
                        {
                            if (buttonids.Contains(p4Button.P4Btn_ID))
                            {
                                dr.Cells["Selected"].Value = true;
                            }
                            else
                            {
                                dr.Cells["Selected"].Value = false;
                            }
                        }
                    }

                }
            }

            if (kryptonNavigator1.SelectedPage.Name == kryptonPageFieldInfo.Name)
            {
                dataGridView2.UseSelectedColumn = true;
                if (!dataGridView2.UseSelectedColumn)
                {
                    //请先开启多模式，重复结果将会勾选
                    MessageBox.Show("请先开启多模式，重复结果将会勾选");
                    return;
                }
                // 定义参与比较的属性列表
                var includeProperties = new List<string>()
                    .Include<tb_P4Field>(c => c.FieldInfo_ID)
                    .Include<tb_P4Field>(c => c.RoleID);

                var okList = UITools.CheckDuplicateData<tb_P4Field>(ListDataSoure2.Cast<tb_P4Field>().ToList(), includeProperties.ToList());
                if (dataGridView1.UseSelectedColumn)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        var dr = dataGridView1.Rows[i];
                        if (dr.DataBoundItem is tb_P4Field p4Field)
                        {
                            if (okList.Contains(p4Field))
                            {
                                p4Field.Selected = true;
                            }
                            else
                            {
                                p4Field.Selected = false;
                            }
                        }
                    }

                }

            }


        }



        #endregion

        /// <summary>
        /// 删除行级权限 - 右键菜单功能
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void NewSumDataGridView_删除行级权限(object sender, EventArgs e)
        {


            // 获取当前选中的行
            if (newSumDataGridViewRowAuthPolicy.CurrentRow == null)
            {
                MessageBox.Show("请先选择要删除的行级权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取绑定的数据对象
            var selectedPolicy = newSumDataGridViewRowAuthPolicy.CurrentRow.DataBoundItem as tb_P4RowAuthPolicyByRole;
            if (selectedPolicy == null)
            {
                MessageBox.Show("无法获取选中的行级权限数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 确认删除提示
            var result = MessageBox.Show("确定要删除选中的行级权限吗？此操作不可恢复。", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // 从数据源中移除
                bindingSourceRowAuthPolicy.Remove(selectedPolicy);

                // 从数据库中删除
                var db = MainForm.Instance.AppContext.Db.CopyNew();
                await db.Deleteable<tb_P4RowAuthPolicyByRole>()
                      .Where(p => p.Policy_Role_RID == selectedPolicy.Policy_Role_RID)
                      .ExecuteCommandAsync();

                // 更新缓存（节点的tag）
                if (TreeView1.SelectedNode != null && TreeView1.SelectedNode.Tag is tb_MenuInfo selectMenu)
                {
                    // 清除相关缓存，下次访问时会重新加载
                    // 清除当前业务类型的策略缓存，确保下次加载时获取最新数据
                    if (selectMenu.BizType.HasValue)
                    {
                        BizType bizType = (BizType)selectMenu.BizType.Value;
                        if (_policyCache.ContainsKey(bizType))
                        {
                            _policyCache.Remove(bizType);
                        }
                    }

                    // 更新节点tag中的缓存信息
                    if (CurrentRole.tb_P4RowAuthPolicyByRoles.Contains(selectedPolicy))
                    {
                        CurrentRole.tb_P4RowAuthPolicyByRoles.Remove(selectedPolicy);
                    };

                }

                MessageBox.Show("行级权限删除成功", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除行级权限时发生错误：{ex.Message}", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KryptonNavigator1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode == null)
                return;
            // TODO: 实现导航器索引改变时的逻辑
            NewSumDataGridView dg = kryptonNavigator1.SelectedPage.Controls[0] as NewSumDataGridView;
            if (dg == null && kryptonNavigator1.SelectedPage.Controls.Count > 0 && TreeView1.SelectedNode.Tag != null)
            {
                dg = newSumDataGridViewRowAuthPolicy;
                //加载行级授权的配置
                if ((TreeView1.SelectedNode.Tag is tb_MenuInfo menuInfo))
                {
                    MainForm.Instance.AppContext.Db.Queryable<tb_P4RowAuthPolicyByRole>()
                        .Where(c => c.MenuID == menuInfo.MenuID)
                        .Where(c => c.RoleID == CurrentRole.RoleID)
                        .ToList();
                    return;
                }



            }

            #region 设置全选菜单

            //如果是bool型的才显示右键菜单全选全不选
            foreach (DataGridViewColumn dc in dg.Columns)
            {
                if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
                {
                    dc.HeaderCell.ContextMenuStrip = contextMenuStrip1;
                }
            }

            foreach (DataGridViewColumn dc in dg.Columns)
            {
                if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
                {
                    dc.HeaderCell.ContextMenuStrip = contextMenuStrip1;
                }
            }


            #endregion
        }
        private async void UCRoleAuthorization_Load(object sender, EventArgs e)
        {
            UIBizService.RequestCache<tb_RowAuthPolicy>();
            DisplayTextResolver = new GridViewDisplayTextResolver(typeof(tb_P4RowAuthPolicyByRole));
            kryptonNavigator1.SelectedPageChanged += KryptonNavigator1_SelectedIndexChanged;

            TreeView1.HideSelection = false;
            TreeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            //角色 取到关系 。再找对应的实际为了显示
            List<tb_RoleInfo> roleInfos = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_RoleInfo>()
            .Includes(m => m.tb_P4Buttons, c => c.tb_buttoninfo)
           .Includes(m => m.tb_P4Fields, c => c.tb_fieldinfo)
           .Includes(m => m.tb_P4Menus, c => c.tb_menuinfo)
           .Includes(m => m.tb_P4RowAuthPolicyByRoles, c => c.tb_rowauthpolicy)
           .ToListAsync();

            DataBindingHelper.InitCmb<tb_RoleInfo>(k => k.RoleID, v => v.RoleName, cmRoleInfo.ComboBox, true, roleInfos);

            //加载空菜单，等待cmb选择时再勾选上对应角色的菜单
            await LoadTreeView();
            InitListData();
            dataGridView1.NeedSaveColumnsXml = true;
            dataGridView2.NeedSaveColumnsXml = true;
            newSumDataGridViewRowAuthPolicy.NeedSaveColumnsXml = true;
            dataGridView1.Use是否使用内置右键功能 = false;
            dataGridView2.Use是否使用内置右键功能 = false;
            newSumDataGridViewRowAuthPolicy.Use是否使用内置右键功能 = false;
            // dataGridView1.ContextMenuStrip = contextMenuStrip1;
            // dataGridView2.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDown);
            dataGridView2.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView2_CellMouseDown);
            DisplayTextResolver.Initialize(newSumDataGridViewRowAuthPolicy);
        }


        tb_ModuleDefinitionController<tb_ModuleDefinition> ctrMod = Startup.GetFromFac<tb_ModuleDefinitionController<tb_ModuleDefinition>>();
        tb_MenuInfoController<tb_MenuInfo> ctrMenu = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
        tb_ButtonInfoController<tb_ButtonInfo> ctrBut = Startup.GetFromFac<tb_ButtonInfoController<tb_ButtonInfo>>();
        tb_FieldInfoController<tb_FieldInfo> ctrField = Startup.GetFromFac<tb_FieldInfoController<tb_FieldInfo>>();

        tb_P4MenuController<tb_P4Menu> ctrPMenu = Startup.GetFromFac<tb_P4MenuController<tb_P4Menu>>();
        tb_P4ButtonController<tb_P4Button> ctrPBut = Startup.GetFromFac<tb_P4ButtonController<tb_P4Button>>();
        tb_P4FieldController<tb_P4Field> ctrPField = Startup.GetFromFac<tb_P4FieldController<tb_P4Field>>();


        List<tb_ModuleDefinition> DefaultModules = new List<tb_ModuleDefinition>();

        /// <summary>
        /// 将模块 菜单 显示为树
        /// </summary>
        private async Task<bool> LoadTreeView(bool Seleted = false)
        {
            DefaultModules = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_ModuleDefinition>()
           .Includes(m => m.tb_MenuInfos, a => a.tb_ButtonInfos)
           .Includes(m => m.tb_MenuInfos, a => a.tb_FieldInfos)
           .ToListAsync();

            //检测CRM如果没有购买则不会显示
            if (!MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM))
            {
                DefaultModules = DefaultModules.Where(m => m.ModuleName != ModuleMenuDefine.模块定义.客户关系.ToString()).ToList();
            }

            // 使用扩展方法实现异步UI调用
            await MainForm.Instance.InvokeAsync(() =>
            {
                TreeView1.CheckBoxes = true;
                TreeView1.Nodes.Clear();
                ThreeStateTreeNode nd = new ThreeStateTreeNode();
                nd.Text = "系统根节点";
                nd.Checked = Seleted;
                //加载模块 模块和顶级菜单相同
                //  AddTopTreeNode(Modules, nd, MenuInfoList, 0);
                AddTreeNodeByMod(DefaultModules, nd);
                TreeView1.Nodes.Add(nd);
                TreeView1.Nodes[0].Expand();
            });


            return true;
        }


        private void tVtypeList_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // 检查是否需要跨线程调用
            if (TreeView1.InvokeRequired)
            {
                TreeView1.Invoke(new Action<object, DrawTreeNodeEventArgs>(tVtypeList_DrawNode), sender, e);
                return;
            }

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
        /// 暂时不使用，保存时关联上模块即可显示时。与顶级菜单一样
        /// </summary>
        /// <param name="Modules"></param>
        /// <param name="nd"></param>
        /// <param name="list"></param>
        /// <param name="parent_id"></param>
        private void AddTopTreeNode(List<tb_ModuleDefinition> Modules, ThreeStateTreeNode nd, List<tb_MenuInfo> list, long parent_id)
        {

            var selectList = list.Where(m => m.Parent_id == parent_id);
            foreach (var item in selectList)
            {
                tb_ModuleDefinition ModuleDefin = Modules.FirstOrDefault(c => c.ModuleName == item.MenuName);
                string NodeText = "";
                NodeText = item.CaptionCN;
                //建立一个新节点
                ThreeStateTreeNode node = new ThreeStateTreeNode(NodeText);
                node.Tag = item;
                node.Checked = ModuleDefin.Visible;
                node.Name = ModuleDefin.ModuleID.ToString();
                //模块与菜单关系是用枚举的硬编码 在窗体的特性中定义标记的
                //初始化时已经将模块ID保存为外键到菜单中关联起来了
                AddTreeNode(node, list, item.MenuID);
                nd.Nodes.Add(node);
            }
        }



        /// <summary>
        /// 2023-12-29
        /// </summary>
        /// <param name="Modules"></param>
        /// <param name="nd"></param>
        /// <param name="list"></param>
        /// <param name="parent_id"></param>
        private void AddTreeNodeByMod(List<tb_ModuleDefinition> Modules, ThreeStateTreeNode nd, bool Seleted = false)
        {
            //默认加载时。顶级菜单也就是模块。名称相同
            foreach (var item in Modules)
            {
                tb_MenuInfo _MenuInfo = item.tb_MenuInfos.FirstOrDefault(c => c.MenuName == item.ModuleName && c.IsEnabled);
                if (_MenuInfo == null)
                {
                    continue;
                }
                string NodeText = "";
                NodeText = item.ModuleName;
                //建立一个新节点
                ThreeStateTreeNode node = new ThreeStateTreeNode(NodeText);
                node.Tag = _MenuInfo;
                node.Checked = item.Visible;
                if (Seleted)
                {
                    node.Checked = true;
                }
                node.Name = item.ModuleID.ToString();
                //模块与菜单关系是用枚举的硬编码 在窗体的特性中定义标记的
                //初始化时已经将模块ID保存为外键到菜单中关联起来了

                AddTreeNode(node, item.tb_MenuInfos, _MenuInfo.MenuID);
                nd.Nodes.Add(node);
            }
        }


        private void AddTreeNode(ThreeStateTreeNode nd, List<tb_MenuInfo> list, long parent_id, bool Seleted = false)
        {
            var selectList = list.Where(m => m.Parent_id == parent_id).OrderBy(c => c.Sort);
            foreach (var item in selectList)
            {
                if (!item.IsVisble)
                {
                    continue;
                }
                if (!item.IsEnabled)
                {
                    continue;
                }
                string NodeText = "";
                NodeText = item.CaptionCN;
                //建立一个新节点
                ThreeStateTreeNode node = new ThreeStateTreeNode(NodeText);
                node.Tag = item;
                if (Seleted)
                {
                    node.Checked = true;
                }
                if (item.MenuType == "行为菜单")
                {
                    node.ContextMenuStrip = contextMenuStrip4InitData;
                }
                //模块与菜单关系是用枚举的硬编码 在窗体的特性中定义标记的
                //初始化时已经将模块ID保存为外键到菜单中关联起来了
                AddTreeNode(node, list, item.MenuID, Seleted);
                nd.Nodes.Add(node);
            }
        }


        #region 基础声明

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

            var otherkey = keyData & Keys.KeyCode;
            var othermodeifierKey = keyData & Keys.Modifiers;
            if (othermodeifierKey == Keys.Control && otherkey == Keys.F)
            {
                MessageBox.Show("Control+F is pressed");
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }

        bool Edited = false;
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

        public System.Windows.Forms.BindingSource _ListDataSoure1 = null;

        [Description("列表中的要显示的数据来源[BindingSource]"), Category("自定属性"), Browsable(true)]
        /// <summary>
        /// 列表的数据源(实际要显示的)
        /// </summary>
        public System.Windows.Forms.BindingSource ListDataSoure1
        {
            get { return _ListDataSoure1; }
            set { _ListDataSoure1 = value; }
        }


        public System.Windows.Forms.BindingSource _ListDataSoure2 = null;

        [Description("列表中的要显示的数据来源[BindingSource]"), Category("自定属性"), Browsable(true)]
        /// <summary>
        /// 列表的数据源(实际要显示的)
        /// </summary>
        public System.Windows.Forms.BindingSource ListDataSoure2
        {
            get { return _ListDataSoure2; }
            set { _ListDataSoure2 = value; }
        }

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(tb_P4Button));
            dataGridView1.XmlFileName = "UCRoleAuthorization1";
            dataGridView1.FieldNameList = FieldNameList1;
            dataGridView1.DataSource = null;
            ListDataSoure1 = bindingSource1;
            //绑定导航
            dataGridView1.DataSource = ListDataSoure1.DataSource;

            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList2 = UIHelper.GetFieldNameColList(typeof(tb_P4Field));
            dataGridView2.XmlFileName = "UCRoleAuthorization2";
            dataGridView2.FieldNameList = FieldNameList2;
            dataGridView2.DataSource = null;
            ListDataSoure2 = bindingSource2;
            //绑定导航
            dataGridView2.DataSource = ListDataSoure2.DataSource;


            newSumDataGridViewRowAuthPolicy.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList3 = UIHelper.GetFieldNameColList(typeof(tb_P4RowAuthPolicyByRole));
            newSumDataGridViewRowAuthPolicy.XmlFileName = "UCtb_P4RowAuthPolicyByRole";
            newSumDataGridViewRowAuthPolicy.FieldNameList = FieldNameList3;
            newSumDataGridViewRowAuthPolicy.DataSource = null;
            //绑定导航
            newSumDataGridViewRowAuthPolicy.DataSource = bindingSourceRowAuthPolicy.DataSource;

        }

        #endregion
        tb_RoleInfo CurrentRole;

        private async void btnSave_Click(object sender, EventArgs e)
        {
            #region


            if (CurrentRole == null)
            {
                MessageBox.Show("请选择要操作的角色！");
                return;
            }

            //保存勾选菜单

            //准备要更新的集合
            if (CurrentRole.tb_P4Menus == null)
            {
                CurrentRole.tb_P4Menus = new List<tb_P4Menu>();
            }
            // 记录加载后的状态为原始值
            CurrentRole.tb_P4Menus.ForEach(c => c.BeginOperation());

            //保存顶级菜单勾选情况  是用更新 第一个是根节点
            UpdateP4Module(TreeView1.Nodes[0].Nodes, CurrentRole.tb_P4Menus);

            CurrentRole.tb_P4Menus.ForEach<tb_P4Menu>(c => c.AcceptChanges());


            toolStripButtonSave.Enabled = false;
            if (TreeView1.SelectedNode == null || !(TreeView1.SelectedNode.Tag is tb_MenuInfo))
            {
                return;
            }
            tb_MenuInfo selectMenu = TreeView1.SelectedNode.Tag as tb_MenuInfo;
            if (selectMenu.MenuType != "行为菜单")
            {
                return;
            }

            #endregion

            dataGridView1.EndEdit();
            dataGridView2.EndEdit();

            //@@@@@@！分层级处理！@@@@@@@@
            //==================看到的  选项 处理一次保存一次
            //更新按钮关系
            List<tb_P4Button> pblist = new List<tb_P4Button>();
            foreach (var item in bindingSource1.List)
            {
                tb_P4Button pb = item as tb_P4Button;
                BusinessHelper.Instance.EditEntity(pb);
                pblist.Add(pb);
            }

            bool rs1 = await MainForm.Instance.AppContext.Db.CopyNew().Updateable(pblist).ExecuteCommandHasChangeAsync();

            //更新字段关系
            List<tb_P4Field> pflist = new List<tb_P4Field>();
            foreach (var item in bindingSource2.List)
            {
                tb_P4Field pf = item as tb_P4Field;
                BusinessHelper.Instance.EditEntity(pf);
                pflist.Add(pf);
            }
            bool rs2 = await MainForm.Instance.AppContext.Db.CopyNew().Updateable(pflist).ExecuteCommandHasChangeAsync();

            // 保存行级权限规则
            try
            {
                if (bindingSourceRowAuthPolicy.List != null && bindingSourceRowAuthPolicy.List.Count > 0 && CurrentRole != null)
                {
                    var roleId = CurrentRole.RoleID;
                    var menuId = selectMenu.MenuID;

                    // 获取所有与当前角色和菜单相关的行级权限规则
                    List<tb_P4RowAuthPolicyByRole> policies = new List<tb_P4RowAuthPolicyByRole>();
                    foreach (var item in bindingSourceRowAuthPolicy.List)
                    {
                        tb_P4RowAuthPolicyByRole policy = item as tb_P4RowAuthPolicyByRole;
                        //只处理新增加的数据
                        if (policy != null && policy.Policy_Role_RID == 0)
                        {
                            policies.Add(policy);
                        }
                    }

                    // 删除旧的关联记录
                    //await MainForm.Instance.AppContext.Db.Deleteable<tb_P4RowAuthPolicyByRole>()
                    //    .Where(p => p.RoleID == roleId && p.MenuID == menuId)
                    //    .ExecuteCommandAsync();

                    // 保存新的关联记录
                    if (policies.Any())
                    {
                        await MainForm.Instance.AppContext.Db.Insertable(policies.Where(c => c.Policy_Role_RID == 0).ToList()).ExecuteReturnSnowflakeIdAsync();
                    }

                    // 更新内存中的CurrentRole对象，同步最新的权限规则
                    // 1. 先移除当前菜单下的所有旧规则
                    CurrentRole.tb_P4RowAuthPolicyByRoles = CurrentRole.tb_P4RowAuthPolicyByRoles
                        .Where(r => !(r.RoleID == roleId && r.MenuID == menuId))
                        .ToList();

                    // 2. 添加新保存的规则
                    if (policies.Any())
                    {
                        CurrentRole.tb_P4RowAuthPolicyByRoles.AddRange(policies);
                    }

                    // 清除当前业务类型的策略缓存，确保下次加载时获取最新数据
                    if (selectMenu.BizType.HasValue)
                    {
                        BizType bizType = (BizType)selectMenu.BizType.Value;
                        if (_policyCache.ContainsKey(bizType))
                        {
                            _policyCache.Remove(bizType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("保存行级权限规则失败", ex);
                MessageBox.Show($"保存行级权限规则失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            toolStripButtonSave.Enabled = false;

        }



        private void SaveRoleRelation()
        {

        }



        private void UpdateP4Menu(TreeNodeCollection Nodes, List<tb_P4Menu> tb_P4Menus)
        {
            //保存菜单勾选情况  是用更新
            foreach (ThreeStateTreeNode tn in Nodes)
            {
                if (tn.Tag is tb_MenuInfo menuInfo)
                {
                    var pm = tb_P4Menus.Where(e => e.MenuID == menuInfo.MenuID).FirstOrDefault();
                    if (pm != null)
                    {
                        //已经存在菜单角色关系的只更新选择状态
                        if (pm.IsVisble != tn.Checked)
                        {
                            pm.IsVisble = tn.Checked;
                            BusinessHelper.Instance.EditEntity(pm);
                        }
                    }
                    else
                    {
                        //新的菜单则需要新增 这种情况少 单个操作也可以
                        tb_P4Menu pmNew = new tb_P4Menu();
                        pmNew.RoleID = CurrentRole.RoleID;
                        pmNew.MenuID = menuInfo.MenuID;
                        pmNew.ModuleID = menuInfo.ModuleID;
                        pmNew.IsVisble = tn.Checked;
                        BusinessHelper.Instance.InitEntity(pmNew);
                        tb_P4Menus.Add(pmNew);
                        //var id = ctrPMenu.AddAsync(pmNew);
                    }
                }
                UpdateP4Menu(tn.Nodes, tb_P4Menus);
            }
        }

        /// <summary>
        /// 更新模块  模块等于顶级菜单这里要同时保存一下菜单关系
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="tb_P4Modules"></param>
        private async Task UpdateP4Module(TreeNodeCollection Nodes, List<tb_P4Menu> tb_P4Menus)
        {

            // 先检查数据库中的重复数据
            var duplicates = tb_P4Menus.Where(c => c.RoleID == CurrentRole.RoleID)
                .GroupBy(p => p.MenuID)
                .Where(g => g.Count() > 1)
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .ToList();

            if (duplicates.Any())
            {
                // 记录日志或抛出异常
                MainForm.Instance.logger.Error($"tb_P4Menus 角色ID:{CurrentRole.RoleID}下发现重复的 MenuID: " + string.Join(", ", duplicates.Select(d => d.Key)));

                // 删除重复的数据
                var deleteIds = duplicates.Select(d => d.Key).ToList();
                await MainForm.Instance.AppContext.Db.Deleteable<tb_P4Menu>().Where(p => p.RoleID == CurrentRole.RoleID && deleteIds.Contains(p.MenuID)).ExecuteCommandAsync();

                // 重新加载数据
                CurrentRole.tb_P4Menus = tb_P4Menus.Where(p => !deleteIds.Contains(p.MenuID)).ToList();
            }

            //保存菜单勾选情况  是用更新 顶级 只有一级 对应模块
            //模块等于顶级菜单这里要同时保存一下菜单关系
            //准备要更新的集合
            //List<tb_P4Menu> tb_P4Menus = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Menu>()
            //    .Where(p => p.RoleID == CurrentRole.RoleID)
            //    .Includes(m => m.tb_menuinfo)
            //    .ToListAsync();

            foreach (ThreeStateTreeNode tn in Nodes)
            {
                if (tn.Tag is tb_MenuInfo menuInfo)
                {
                    //保存菜单勾选情况  是用更新
                    UpdateP4Menu(tn.Nodes, tb_P4Menus);
                    tb_P4Menu p4Menu = tb_P4Menus.FirstOrDefault(c => c.MenuID == menuInfo.MenuID);
                    if (p4Menu != null)
                    {
                        if (p4Menu.IsVisble != tn.Checked)
                        {
                            p4Menu.IsVisble = tn.Checked;
                            BusinessHelper.Instance.EditEntity(p4Menu);

                        }
                    }
                    else
                    {
                        #region 添加顶级菜单
                        //新的菜单则需要新增 这种情况少 单个操作也可以
                        tb_P4Menu pmNew = new tb_P4Menu();
                        pmNew.RoleID = CurrentRole.RoleID;
                        pmNew.MenuID = menuInfo.MenuID;
                        pmNew.ModuleID = menuInfo.ModuleID;
                        pmNew.IsVisble = tn.Checked;
                        BusinessHelper.Instance.InitEntity(pmNew);
                        tb_P4Menus.Add(pmNew);
                        #endregion
                    }

                }
            }

            if (tb_P4Menus.Where(c => c.P4Menu_ID == 0).ToList().Count > 0)
            {
                var rs = await MainForm.Instance.AppContext.Db.CopyNew().Insertable(tb_P4Menus.Where(c => c.P4Menu_ID == 0).ToList()).ExecuteReturnSnowflakeIdListAsync();
            }

            List<tb_P4Menu> UpdateP4Menus = tb_P4Menus.Where(c => c.P4Menu_ID > 0
            && c.RoleID == CurrentRole.RoleID && c.HasChanged
            ).ToList();

            if (UpdateP4Menus.Count > 0)
            {
                bool rs = await MainForm.Instance.AppContext.Db.CopyNew().Updateable(UpdateP4Menus).ExecuteCommandHasChangeAsync();
                UpdateP4Menus.ForEach(c => c.AcceptChanges());
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseTheForm(this);
        }

        //模块角色关系可能用不上。保存在菜单关系表中了？

        private async void cmRoleInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmRoleInfo.SelectedItem == null)
            {
                return;
            }

            CurrentRole = cmRoleInfo.SelectedItem as tb_RoleInfo;

            if (CurrentRole.RoleID == -1)
            {
                return;
            }

            // 使用等待对话框

            try
            {
                //如果选中了一个角色 ，则其它角色 自动生成下拉按钮
                List<tb_RoleInfo> roleList = cmRoleInfo.Items.CastToList<tb_RoleInfo>();
                roleList.Remove(CurrentRole);
                toolStripCopyRoleConfig.DropDownItems.Clear();
                toolStripCopyRoleConfig.DropDownItems.Add(new ToolStripSeparator());
                roleList.ForEach(x =>
                {
                    if (x.RoleName != "请选择")
                    {
                        var item = new ToolStripMenuItem(x.RoleName);
                        item.Tag = x;
                        item.Click += (s, e) =>
                        {
                            #region 复制选中角色的权限
                            CopyRole(item);
                            #endregion
                        };
                        toolStripCopyRoleConfig.DropDownItems.Add(item);
                    }
                });

                toolStripCopyRoleConfig.Enabled = roleList.Any();

                // 使用SqlSugar异步查询并传递CancellationToken
                var db = MainForm.Instance.AppContext.Db.CopyNew();

                // 设置合理的超时时间（单位：秒）

                db.Ado.CommandTimeOut = 30;
                var pmlis = await db.Queryable<tb_P4Menu>().Where(r => r.RoleID == CurrentRole.RoleID)
                                 .Includes(t => t.tb_menuinfo, b => b.tb_P4Buttons)
                                    .Includes(t => t.tb_menuinfo, b => b.tb_P4Fields)
                                    .Includes(t => t.tb_menuinfo, b => b.tb_P4Menus)
                                    .Includes(t => t.tb_roleinfo, b => b.tb_P4Buttons)
                                    .Includes(t => t.tb_roleinfo, b => b.tb_P4Fields)
                                    .Includes(t => t.tb_roleinfo, b => b.tb_P4Menus)
                                     .ToListAsync() // 传递CancellationToken
                                    .ConfigureAwait(false);
                if (pmlis.Count == 0)
                {
                    // 改为异步初始化
                    await InitMenuByRoleAsync(CurrentRole, true)
                        .ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }



            if (TreeView1.Nodes.Count <= 1)
            {
                await LoadTreeView();
            }
            //循环去钩选
            UpdateP4MenuUI(TreeView1.Nodes[0].Nodes, CurrentRole.tb_P4Menus);



        }




        private void UpdateP4MenuUI(TreeNodeCollection Nodes, List<tb_P4Menu> tb_P4Menus)
        {
            // 检查是否需要跨线程调用
            if (TreeView1.InvokeRequired)
            {
                TreeView1.Invoke(new Action<TreeNodeCollection, List<tb_P4Menu>>(UpdateP4MenuUI), Nodes, tb_P4Menus);
                return;
            }

            //保存菜单勾选情况  是用更新
            foreach (ThreeStateTreeNode tn in Nodes)
            {
                if (tn.Tag is tb_MenuInfo)
                {
                    tb_MenuInfo menuInfo = tn.Tag as tb_MenuInfo;
                    var pm = tb_P4Menus.Where(e => e.MenuID == menuInfo.MenuID).FirstOrDefault();
                    if (pm != null)
                    {
                        tn.Checked = pm.IsVisble;
                    }
                    else
                    {
                        //如果没有关系关联 默认不钩
                    }

                }
                UpdateP4MenuUI(tn.Nodes, tb_P4Menus);
            }
        }

        #region 角色级初始化菜单
        private readonly object _menuLock = new object();
        private readonly object _newListLock = new object();
        /// <summary>
        /// 按角色初始化所有菜单,默认为否
        /// </summary>
        /// <param name="role"></param>
        public async Task InitMenuByRole(tb_RoleInfo role, bool Selected = false)
        {
            var db = MainForm.Instance.AppContext.Db;
            var list = await ctrMenu.QueryByNavAsync();
            int totalItems = list.Count;
            int lastReported = -1;
            List<tb_P4Menu> Newplist = new List<tb_P4Menu>();

            // 使用状态栏进度条

            ProgressManager.Instance.RunAsync(worker =>
            {
                try
                {
                    //foreach (var item in list)
                    foreach (var (item, index) in list.Select((item, idx) => (item, idx)))
                    {
                        // 检查取消请求
                        if (worker.CancellationPending)
                        {
                            worker.ReportProgress(0, "Operation cancelled");
                            return Task.CompletedTask;
                        }


                        // 报告进度 
                        int currentPercent = (index + 1) * 100 / totalItems;
                        //避免过于频繁的UI更新
                        if (currentPercent != lastReported)
                        {
                            worker.ReportProgress(currentPercent, $"Processing item {index + 1}/{totalItems}");
                            lastReported = currentPercent;
                        }

                        try
                        {
                            // 同步执行异步方法
                            Task.Run(async () =>
                            {
                                #region 耗时业务代码

                                tb_P4Menu pm = new tb_P4Menu();
                                // 加锁访问 role.tb_P4Menus
                                lock (_menuLock)
                                {
                                    pm = role.tb_P4Menus.FirstOrDefault(c => c.MenuID == item.MenuID && c.RoleID == role.RoleID);
                                }
                                if (pm == null)
                                {
                                    pm = new tb_P4Menu();
                                }
                                else
                                {
                                    if (item.MenuType == "行为菜单")
                                    {
                                        #region 每个菜单 都添加按钮和字段
                                        if (item.MenuID == 1920107280263680000)
                                        {

                                        }

                                        List<tb_P4Button> p4Buttons = await InitBtnByRole(role, item, true);


                                        List<tb_P4Field> p4Fields = await InitFiledByRole(role, item, true);


                                        #endregion
                                    }
                                }
                                pm.RoleID = role.RoleID;
                                pm.MenuID = item.MenuID;
                                pm.ModuleID = item.ModuleID;
                                if (Selected)
                                {
                                    pm.IsVisble = Selected;
                                }
                                else
                                {
                                    pm.IsVisble = false;
                                }
                                pm.IsVisble = item.IsVisble;
                                BusinessHelper.Instance.InitEntity(pm);
                                if (pm.P4Menu_ID == 0)
                                {
                                    Newplist.Add(pm);
                                }
                                #endregion

                            }).Wait(); // 同步等待任务完成
                        }
                        catch (Exception exx)
                        {
                            MainForm.Instance.logger.Error("初始化时出错了。", exx);
                            worker.ReportProgress(100, $"Error: {exx.Message}");
                            throw; // 触发外层 catch

                        }
                        //支持取消
                        if (worker.CancellationPending)
                        {
                            worker.ReportProgress(100, "Cancelled");
                            return Task.CompletedTask;
                        }
                    }
                    // 最终强制报告100%
                    worker.ReportProgress(100, "Completed");
                    worker.DoWork += Worker_DoWork;
                    worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                }
                catch (Exception ex)
                {
                    // 标记错误并终止任务
                    worker.ReportProgress(100, $"Error: {ex.Message}");
                }

                return Task.CompletedTask;
            }, (cancelled, error) =>
                {
                    if (error != null) MessageBox.Show(error.Message);
                });

            // var ids = await db.Insertable(plist).ExecuteReturnSnowflakeIdListAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = e.Argument; // 将结果传递给 RunWorkerCompleted
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBox.Show($"Error: {e.Error.Message}");
                    return;
                }
                ////var ids = ctrPMenu.AddAsync(Newplist.Where(c => c.P4Menu_ID == 0).ToList());
                ////CurrentRole.tb_P4Menus.AddRange(Newplist);
                ///
                // 更新UI
                UpdateP4MenuUI(TreeView1.Nodes, CurrentRole.tb_P4Menus);

                // 确保在UI线程上执行UI更新
                // 确保在UI线程上执行
                UpdateUI(() =>
                {
                    CompleteMenuInitialization();
                });
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("Error in Worker_RunWorkerCompleted", ex);
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void UpdateUI(Action action)
        {
            if (TreeView1.InvokeRequired)
            {
                TreeView1.Invoke(action);
            }
            else
            {
                action();
            }
        }
        private void CompleteMenuInitialization()
        {
            try
            {
                // 统一将所有新生成的权限菜单添加到数据库
                //var newMenusToAdd = Newplist.Where(c => c.P4Menu_ID == 0).ToList();
                //if (newMenusToAdd.Count > 0)
                //{
                //    var ids = await ctrPMenu.AddAsync(newMenusToAdd);
                //    CurrentRole.tb_P4Menus.AddRange(newMenusToAdd);
                //}

                // 更新UI
                UpdateP4MenuUI(TreeView1.Nodes, CurrentRole.tb_P4Menus);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("Error in CompleteMenuInitialization", ex);
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        List<MenuAttrAssemblyInfo> MenuAssemblylist = UIHelper.RegisterForm();

        /// <summary>
        /// 初始化菜单下的按钮权限
        /// </summary>
        /// <param name="role">当前角色</param>
        /// <param name="menuInfo">当前菜单</param>
        /// <param name="ButtonInfolist"></param>
        /// <param name="P4Buttonlist">如果已经存在部分正常的权限和按钮关系，则要排除</param>
        /// <returns></returns>
        public async Task<List<tb_P4Button>> InitBtnByRole_old(tb_RoleInfo role, tb_MenuInfo menuInfo, bool Seleted = false)
        {
            MenuAttrAssemblyInfo mai = MenuAssemblylist.FirstOrDefault(e => e.ClassPath == menuInfo.ClassPath);
            if (mai != null)
            {
                await Task.Run(() =>
                {
                    MainForm.Instance.Invoke(new Action(async () =>
                    {
                        InitModuleMenu imm = Startup.GetFromFac<InitModuleMenu>();
                        await imm.InitToolStripItemAsync(mai, menuInfo);

                    }));
                });


            }
            if (menuInfo.tb_P4Buttons == null)
            {
                menuInfo.tb_P4Buttons = new List<tb_P4Button>();
            }

            List<tb_P4Button> Newpblist = new List<tb_P4Button>();
            List<tb_P4Button> Updatepblist = new List<tb_P4Button>();

            for (int i = 0; i < menuInfo.tb_ButtonInfos.Count; i++)
            {
                var item = menuInfo.tb_ButtonInfos[i];

                //    foreach (var item in menuInfo.tb_ButtonInfos)
                //{
                //这里的意思是 按钮的数量要与角色对应按钮一致。可以不启用 但是重复的则可以不用加入
                //存在过就不添加了 没有的才添加
                tb_P4Button pb = new tb_P4Button();
                pb = menuInfo.tb_P4Buttons.Where(p => p.MenuID == menuInfo.MenuID && p.RoleID == CurrentRole.RoleID && p.ButtonInfo_ID == item.ButtonInfo_ID).FirstOrDefault();
                if (pb == null)
                {
                    pb = new tb_P4Button();
                    BusinessHelper.Instance.InitEntity(pb);
                    pb.Notes = item.BtnText;
                    pb.ButtonType = item.ButtonType;
                }
                else
                {
                    pb.HasChanged = false;
                }

                pb.RoleID = CurrentRole.RoleID;
                pb.ButtonInfo_ID = item.ButtonInfo_ID;
                pb.MenuID = menuInfo.MenuID;
                if (Seleted)
                {
                    pb.IsVisble = true;
                    pb.IsEnabled = true;
                }
                //else
                //{
                //    pb.IsVisble = false;
                //    pb.IsEnabled = false;
                //}
                if (pb.P4Btn_ID == 0)
                {
                    Newpblist.Add(pb);
                }
                else
                {
                    if (pb.HasChanged == true)
                    {
                        Updatepblist.Add(pb);
                    }

                }

            }
            if (Newpblist.Count > 0)
            {
                //这里要防止添加重复的按钮对Newpblist 按菜单ID和权限ID分组。ID>0才添加
                List<tb_P4Button> Newpblist2 = Newpblist.GroupBy(c => new { c.MenuID, c.RoleID }).Select(g => g.First()).ToList();
                Newpblist = Newpblist2;
                var ids = await ctrPBut.AddAsync(Newpblist.Where(c => c.P4Btn_ID == 0).ToList());
                if (ids.Count > 0)
                {
                    menuInfo.tb_P4Buttons.AddRange(Newpblist);
                }
            }
            if (Updatepblist.Count > 0)
            {
                int updatecounter = await MainForm.Instance.AppContext.Db.Updateable(Updatepblist).ExecuteCommandAsync();
                if (updatecounter > 0)
                {

                }
            }

            return menuInfo.tb_P4Buttons.Where(c => c.RoleID == role.RoleID).ToList();
            // var ids = await MainForm.Instance.AppContext.Db.Insertable(pblist).ExecuteReturnSnowflakeIdListAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrentRole"></param>
        /// <param name="SelectedMenuInfo">菜单下有默认的按钮数据</param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public async Task<List<tb_P4Button>> InitBtnByRole(tb_RoleInfo CurrentRole, tb_MenuInfo SelectedMenuInfo, bool selected = false)
        {
            // 检查菜单信息是否为空
            if (SelectedMenuInfo == null)
            {
                return new List<tb_P4Button>();
            }

            // 查找菜单属性信息并初始化菜单项
            MenuAttrAssemblyInfo mai = MenuAssemblylist.FirstOrDefault(e => e.ClassPath == SelectedMenuInfo.ClassPath);
            if (mai != null)
            {
                // 使用扩展方法实现异步UI调用
                await MainForm.Instance.InvokeAsync(async () =>
                {
                    InitModuleMenu imm = Startup.GetFromFac<InitModuleMenu>();
                    await imm.InitToolStripItemAsync(mai, SelectedMenuInfo);
                });
            }

            // 先检查数据库中的重复数据，当前角色下的当前菜单下的 按钮是否有重复
            var duplicates = CurrentRole.tb_P4Buttons.Where(c => c.RoleID == CurrentRole.RoleID && c.MenuID == SelectedMenuInfo.MenuID)
                .GroupBy(p => p.ButtonInfo_ID)
                .Where(g => g.Count() > 1)
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .ToList();

            if (duplicates.Any())
            {
                // 记录日志或抛出异常
                MainForm.Instance.logger.Error($"tb_P4Buttons 菜单ID:{SelectedMenuInfo.MenuID},角色ID:{this.CurrentRole.RoleID}下发现重复的 ButtonInfo_ID: " + string.Join(", ", duplicates.Select(d => d.Key)));

                // 删除重复的数据
                var deleteIds = duplicates.Select(d => d.Key).ToList();
                await MainForm.Instance.AppContext.Db.Deleteable<tb_P4Button>().Where(p => p.RoleID == this.CurrentRole.RoleID && deleteIds.Contains(p.ButtonInfo_ID)).ExecuteCommandAsync();

                // 重新加载数据
                CurrentRole.tb_P4Buttons = CurrentRole.tb_P4Buttons.Where(p => !deleteIds.Contains(p.ButtonInfo_ID)).ToList();
            }

            var currentRoleId = this.CurrentRole.RoleID;
            var existingButtons = CurrentRole.tb_P4Buttons
                .Where(p => p.MenuID == SelectedMenuInfo.MenuID && p.RoleID == currentRoleId)
                .ToDictionary(p => p.ButtonInfo_ID);

            var newButtons = new List<tb_P4Button>();
            var updatedButtons = new List<tb_P4Button>();

            foreach (var buttonInfo in SelectedMenuInfo.tb_ButtonInfos)
            {
                if (!existingButtons.TryGetValue(buttonInfo.ButtonInfo_ID, out tb_P4Button button))
                {
                    // 创建新按钮
                    button = new tb_P4Button();
                    BusinessHelper.Instance.InitEntity(button);
                    button.Notes = buttonInfo.BtnText;
                    button.RoleID = currentRoleId;
                    button.ButtonInfo_ID = buttonInfo.ButtonInfo_ID;
                    button.MenuID = SelectedMenuInfo.MenuID;
                    // 设置选中状态
                    SetButtonSelection(button, selected);
                    BusinessHelper.Instance.InitEntity(button);
                    if (!newButtons.Contains(button))
                    {
                        newButtons.Add(button);
                    }

                }
                else
                {
                    //存在的关系，不用更新。保持数据显示
                    // 更新现有按钮状态,如果本身选中，就不改，如果没选中则选上
                    if (button.IsVisble == false)
                    {
                        if (SetButtonSelection(button, true))
                        {
                            updatedButtons.Add(button);
                        }
                    }

                }
            }

            // 批量添加新按钮
            if (newButtons.Any())
            {
                var ids = await ctrPBut.AddAsync(newButtons);
                if (ids?.Count > 0)
                {
                    CurrentRole.tb_P4Buttons.AddRange(newButtons);
                }
            }

            // 批量更新按钮
            if (updatedButtons.Any())
            {
                var updateCount = await MainForm.Instance.AppContext.Db.Updateable(updatedButtons).ExecuteCommandAsync();
                // 可以记录更新结果
            }

            // 返回指定角色的按钮列表
            return CurrentRole.tb_P4Buttons.Where(c => c.RoleID == CurrentRole.RoleID && c.MenuID == SelectedMenuInfo.MenuID).ToList();
        }

        // 辅助方法：设置按钮选中状态并返回是否有变化
        /// <summary>
        /// 这个方法只设置，将没选，改为勾选。 选中方式不在这里处理。
        /// </summary>
        /// <param name="button"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        private bool SetButtonSelection(tb_P4Button button, bool selected)
        {
            bool hasChanged = false;

            if (button.IsVisble != selected)
            {
                button.IsVisble = selected;
                hasChanged = true;
            }

            if (button.IsEnabled != selected)
            {
                button.IsEnabled = selected;
                hasChanged = true;
            }

            return hasChanged;
        }



        /// <summary>
        /// 根据权限初始化字段 以角色权限为基础了。不是以菜单为基础了。菜单只是默认
        /// </summary>
        /// <param name="role"></param>
        /// <param name="menuInfo"></param>
        /// <param name="Seleted">是否选中</param>
        /// <returns></returns>
        public async Task<List<tb_P4Field>> InitFiledByRole_old(tb_RoleInfo role, tb_MenuInfo menuInfo, bool Seleted = false)
        {
            //List<tb_FieldInfo> objlist = new List<tb_FieldInfo>();
            //没有就添加 有就查询出来 
            List<tb_P4Field> Newpblist = new List<tb_P4Field>();
            List<tb_P4Field> Updatepblist = new List<tb_P4Field>();
            //objlist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_FieldInfo>()
            //        .Where(t => t.MenuID == menuInfo.MenuID)
            //        .Includes(t => t.tb_menuinfo)
            //        .ToListAsync();
            if (menuInfo.tb_FieldInfos == null)
            {
                menuInfo.tb_FieldInfos = new List<tb_FieldInfo>();
            }

            if (menuInfo.tb_P4Fields == null)
            {
                menuInfo.tb_P4Fields = new List<tb_P4Field>();
            }

            //增量式增加字段
            Assembly dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            Type[] ModelTypes = dalAssemble.GetExportedTypes();
            List<string> typeNames = ModelTypes.Select(m => m.Name).ToList();
            Type mai = ModelTypes.FirstOrDefault(e => e.Name == menuInfo.EntityName);
            if (mai != null)
            {
                InitModuleMenu imm = Startup.GetFromFac<InitModuleMenu>();
                //imm.InitFieldInoMainAndSub(mai, menuInfo, false, "");
                await imm.InitFieldInoMainAndSubAsync(mai, menuInfo, false, "");
                //尝试找子表类型
                string childType = typeNames.FirstOrDefault(s => s.Contains(mai.Name + "Detail"));
                if (!string.IsNullOrEmpty(childType))
                {
                    Type cType = ModelTypes.FirstOrDefault(t => t.FullName == mai.FullName + "Detail");
                    if (cType != null)
                    {
                        await imm.InitFieldInoMainAndSubAsync(cType, menuInfo, true, childType);
                    }
                }
            }
            for (int i = 0; i < menuInfo.tb_FieldInfos.Count; i++)
            {
                var item = menuInfo.tb_FieldInfos[i];
                //}

                //foreach (var item in menuInfo.tb_FieldInfos)
                //{
                tb_P4Field pb = new tb_P4Field();
                pb = menuInfo.tb_P4Fields.FirstOrDefault(e => e.FieldInfo_ID == item.FieldInfo_ID
                && e.MenuID == menuInfo.MenuID && e.RoleID == role.RoleID);
                if (pb == null)
                {
                    pb = new tb_P4Field();
                    BusinessHelper.Instance.InitEntity(pb);
                    pb.Notes = item.FieldText;
                }
                else
                {
                    pb.HasChanged = false;
                }
                //冗余 ,让 配置权限时更方便查看
                pb.IsChild = item.IsChild;

                pb.RoleID = CurrentRole.RoleID;
                pb.FieldInfo_ID = item.FieldInfo_ID;
                pb.MenuID = menuInfo.MenuID;
                //pb.IsVisble = false;//加上就不对了 保存了勾选都被显示为没勾选了
                if (Seleted)
                {
                    pb.IsVisble = true;
                    if (pb.P4Field_ID > 0 && pb.HasChanged)
                    {
                        Updatepblist.Add(pb);
                    }
                }

                if (pb.P4Field_ID == 0)
                {
                    Newpblist.Add(pb);
                }
            }
            if (Newpblist.Count > 0)
            {
                var ids = await ctrPField.AddAsync(Newpblist);
                if (ids.Count > 0)
                {
                    menuInfo.tb_P4Fields.AddRange(Newpblist);
                }
            }

            if (Updatepblist.Count > 0)
            {
                await MainForm.Instance.AppContext.Db.Updateable(Updatepblist).ExecuteCommandAsync();
            }

            // var ids = await MainForm.Instance.AppContext.Db.Insertable(pblist).ExecuteReturnSnowflakeIdListAsync();
            return menuInfo.tb_P4Fields.Where(c => c.RoleID == role.RoleID).ToList();
        }


        /// <summary>
        /// 这里检查到主表和子表。但是有时子表明细中。还有公共部分。为了也可以控制1
        /// </summary>
        /// <param name="CurrentRole"></param>
        /// <param name="SelectedMenuInfo"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public async Task<List<tb_P4Field>> InitFiledByRole(tb_RoleInfo CurrentRole, tb_MenuInfo SelectedMenuInfo, bool selected = false)
        {
            // 空值检查
            if (SelectedMenuInfo == null || CurrentRole == null)
            {
                return new List<tb_P4Field>();
            }

            // 初始化集合
            SelectedMenuInfo.tb_FieldInfos ??= new List<tb_FieldInfo>();

            // 加载模型类型并初始化字段信息
            try
            {
                var dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
                var modelTypes = dalAssemble.GetExportedTypes();
                var typeNames = modelTypes.Select(m => m.Name).ToList();
                var mainType = modelTypes.FirstOrDefault(e => e.Name == SelectedMenuInfo.EntityName);

                if (mainType != null)
                {
                    var imm = Startup.GetFromFac<InitModuleMenu>();
                    await imm.InitFieldInoMainAndSubAsync(mainType, SelectedMenuInfo, false, "");

                    // 尝试找子表类型
                    var childTypeName = typeNames.FirstOrDefault(s => s.Contains(mainType.Name + "Detail"));
                    if (!string.IsNullOrEmpty(childTypeName))
                    {
                        var childType = modelTypes.FirstOrDefault(t => t.FullName == mainType.FullName + "Detail");
                        if (childType != null)
                        {
                            await imm.InitFieldInoMainAndSubAsync(childType, SelectedMenuInfo, true, childTypeName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常，但继续执行后续逻辑
                MainForm.Instance.logger.Error("加载模型类型时出错", ex);
            }

            // 先检查数据库中的重复数据
            var duplicates = CurrentRole.tb_P4Fields.Where(c => c.RoleID == CurrentRole.RoleID && c.MenuID == SelectedMenuInfo.MenuID)
                .GroupBy(p => p.FieldInfo_ID)
                .Where(g => g.Count() > 1)
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .ToList();

            if (duplicates.Any())
            {
                // 记录日志或抛出异常
                MainForm.Instance.logger.Error($"在tb_P4Fields中 菜单ID:{SelectedMenuInfo.MenuID},角色ID:{this.CurrentRole.RoleID}下发现重复的 FieldInfo_ID: " + string.Join(", ", duplicates.Select(d => d.Key)));

                // 删除重复的数据
                var deleteIds = duplicates.Select(d => d.Key).ToList();
                await MainForm.Instance.AppContext.Db.Deleteable<tb_P4Field>().Where(p => p.RoleID == this.CurrentRole.RoleID && deleteIds.Contains(p.FieldInfo_ID)).ExecuteCommandAsync();

                // 重新加载数据
                CurrentRole.tb_P4Fields = CurrentRole.tb_P4Fields.Where(p => !deleteIds.Contains(p.FieldInfo_ID)).ToList();
            }


            // 使用字典优化查找性能
            var currentRoleId = this.CurrentRole.RoleID;
            var existingFields = CurrentRole.tb_P4Fields
                .Where(p => p.MenuID == SelectedMenuInfo.MenuID && p.RoleID == currentRoleId)
                .ToDictionary<tb_P4Field, string>(p => p.FieldInfo_ID.ToString(),
                    StringComparer.OrdinalIgnoreCase
                );

            var newFields = new List<tb_P4Field>();
            var updatedFields = new List<tb_P4Field>();

            // 处理字段信息
            foreach (var fieldInfo in SelectedMenuInfo.tb_FieldInfos)
            {
                if (!existingFields.TryGetValue(fieldInfo.FieldInfo_ID.ToString(), out tb_P4Field field))
                {
                    // 创建新字段权限
                    field = new tb_P4Field();
                    BusinessHelper.Instance.InitEntity(field);
                    field.Notes = fieldInfo.FieldText;
                    field.IsChild = fieldInfo.IsChild;
                    field.RoleID = currentRoleId;
                    field.FieldInfo_ID = fieldInfo.FieldInfo_ID;
                    field.MenuID = SelectedMenuInfo.MenuID;
                    // 设置选中状态
                    field.IsVisble = selected;
                    if (!newFields.Contains(field))
                    {
                        newFields.Add(field);
                    }
                }
                else
                {
                    // 更新现有字段权限
                    bool hasChanged = false;
                    if (!field.IsVisble)
                    {
                        field.IsVisble = selected;
                        hasChanged = true;
                        if (hasChanged)
                        {
                            updatedFields.Add(field);
                        }
                    }

                }
            }

            // 批量添加新字段权限
            if (newFields.Any())
            {
                var ids = await ctrPField.AddAsync(newFields);
                if (ids?.Count > 0)
                {
                    CurrentRole.tb_P4Fields.AddRange(newFields);
                }
            }

            // 批量更新字段权限
            if (updatedFields.Any())
            {
                await MainForm.Instance.AppContext.Db.Updateable(updatedFields).ExecuteCommandAsync();
            }

            // 返回指定角色的字段权限列表
            return CurrentRole.tb_P4Fields.Where(c => c.RoleID == CurrentRole.RoleID && c.MenuID == SelectedMenuInfo.MenuID).ToList();
        }

        #endregion


        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;
        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList2;
        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList3;

        tb_RoleInfoController<tb_RoleInfo> ctrRole = Startup.GetFromFac<tb_RoleInfoController<tb_RoleInfo>>();


        private async void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (TreeView1.SelectedNode == null)
            {
                return;
            }

            if (CurrentRole == null || CurrentRole.RoleID == -1)
            {
                return;
            }
            if (!(TreeView1.SelectedNode.Tag is tb_MenuInfo))
            {
                return;
            }
            tb_MenuInfo selectMenu = TreeView1.SelectedNode.Tag as tb_MenuInfo;
            if (selectMenu.MenuType != "行为菜单")
            {
                return;
            }
            kryptonNavigator1.SelectedPage = kryptonPageBtn;

            CurrentMenuInfo = selectMenu;
            await InitLoadP4Button(selectMenu, false);

            await InitLoadP4Field(selectMenu, false);

            //加载行级权限规则，优先默认的在前面
            InitLoadRowLevelAuthPolicy(selectMenu);

            #region 按钮和字段列表的中的值 有变化则保存可用
            CurrentRole.tb_P4Buttons.Where(c => c.RoleID == CurrentRole.RoleID).ForEach(x => UpdateSaveEnabled<tb_P4Button>(x));
            CurrentRole.tb_P4Fields.Where(c => c.RoleID == CurrentRole.RoleID).ForEach(x => UpdateSaveEnabled<tb_P4Field>(x));
            #endregion

            #region 设置全选菜单

            //如果是bool型的才显示右键菜单全选全不选
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
                {
                    dc.HeaderCell.ContextMenuStrip = contextMenuStrip1;
                }
            }

            #endregion
        }

        /// <summary>
        /// 加载按钮
        /// </summary>
        /// <param name="selectMenu"></param>
        /// <param name="InitLoadData"></param>
        private async Task InitLoadP4Button(tb_MenuInfo selectMenu, bool InitLoadData = false)
        {
            if (CurrentRole == null)
            {
                return;
            }

            List<tb_P4Button> pblist = new List<tb_P4Button>();
            //优先显示当前选中角色的对应的数据。如果没有才会显示默认为空的。没勾选的数据
            if (CurrentRole.tb_P4Buttons != null)
            {
                pblist = CurrentRole.tb_P4Buttons.Where(c => c.MenuID == selectMenu.MenuID).ToList();

            }
            if (pblist.Count == 0 || InitLoadData)
            {
                pblist = await InitBtnByRole(CurrentRole, selectMenu);
            }
            bindingSource1.DataSource = pblist.ToBindingSortCollection();
            dataGridView1.DataSource = ListDataSoure1;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.ValueType.Name == "Boolean")
                {
                    col.ReadOnly = false;
                }
            }

            pblist.ForEach(x => UpdateSaveEnabled<tb_P4Button>(x));

        }

        private async Task InitLoadP4Field(tb_MenuInfo selectMenu, bool InitLoadData = false)
        {
            if (CurrentRole == null)
            {
                return;
            }
            var pflist = new List<tb_P4Field>();
            if (CurrentRole.tb_P4Fields != null && CurrentRole.tb_P4Fields.Count > 0)
            {
                pflist = CurrentRole.tb_P4Fields.Where(c => c.MenuID == selectMenu.MenuID).ToList();
            }
            #region 加载默认的
            if (pflist.Count == 0 || InitLoadData)
            {
                pflist = await InitFiledByRole(CurrentRole, selectMenu);
            }
            #endregion

            bindingSource2.DataSource = pflist.ToBindingSortCollection();
            dataGridView2.DataSource = ListDataSoure2;
            foreach (DataGridViewColumn col in dataGridView2.Columns)
            {
                if (col.ValueType.Name == "Boolean")
                {
                    col.ReadOnly = false;
                }
                //ischild只是标记是否为子表。他不可以编辑
                if (col.DataPropertyName == "IsChild")
                {
                    col.ReadOnly = true;
                }
            }

            pflist.ForEach(x => UpdateSaveEnabled<tb_P4Field>(x));



        }

        private void InitLoadRowLevelAuthPolicy(tb_MenuInfo selectMenu)
        {
            if (CurrentRole == null)
            {
                return;
            }

            #region 加载默认的
            if (selectMenu.BizType.HasValue)
            {
                BizType bizType = (BizType)selectMenu.BizType.Value;
                // 尝试从缓存获取默认策略
                List<tb_RowAuthPolicy> Policies;
                if (!_policyCache.TryGetValue(bizType, out Policies))
                {
                    // 缓存未命中时才从服务层获取
                    Policies = _rowAuthService.GetAllPolicies(bizType);
                    _policyCache[bizType] = Policies;
                }

                BindingSource bs = new BindingSource();
                bs.DataSource = Policies;

                // 先移除事件订阅，防止设置数据源时触发事件
                cmbRowAuthPolicy.SelectedIndexChanged -= cmbRowAuthPolicy_SelectedIndexChanged;

                // 设置数据源
                DataBindingHelper.InitDataToCmb<tb_RowAuthPolicy>(bs, k => k.PolicyId, v => v.PolicyName, cmbRowAuthPolicy);

                // 数据源设置完成后再重新订阅事件
                cmbRowAuthPolicy.SelectedIndexChanged += cmbRowAuthPolicy_SelectedIndexChanged;
            }

            #endregion

            var RlAList = CurrentRole.tb_P4RowAuthPolicyByRoles.Where(k => k.MenuID == selectMenu.MenuID).ToList();
            bindingSourceRowAuthPolicy.DataSource = RlAList.ToBindingSortCollection();
            newSumDataGridViewRowAuthPolicy.DataSource = bindingSourceRowAuthPolicy;
            foreach (DataGridViewColumn col in dataGridView2.Columns)
            {
                if (col.ValueType.Name == "Boolean")
                {
                    col.ReadOnly = false;
                }
                //ischild只是标记是否为子表。他不可以编辑
                if (col.DataPropertyName == "IsChild")
                {
                    col.ReadOnly = true;
                }
            }

        }


        private void UpdateSaveEnabled<T>(BaseEntity entity)
        {
            entity.PropertyChanged -= (sender, s2) => { };
            entity.PropertyChanged += (sender, s2) =>
             {
                 if (typeof(T).Name == typeof(tb_P4Button).Name)
                 {
                     //如果有变化
                     if (s2.PropertyName == entity.GetPropertyName<tb_P4Button>(c => c.IsEnabled) ||
                     s2.PropertyName == entity.GetPropertyName<tb_P4Button>(c => c.IsVisble))
                     {
                         toolStripButtonSave.Enabled = true;
                     }

                 }
                 if (typeof(T).Name == typeof(tb_P4Field).Name)
                 {
                     //如果有变化
                     if (s2.PropertyName == entity.GetPropertyName<tb_P4Field>(c => c.IsVisble))
                     {
                         toolStripButtonSave.Enabled = true;
                     }
                 }
             };
        }

        /*
        //说明：CellContextMenuStripNeeded事件处理方法的参数中，e.RowIndex=-1表示列头，e.ColumnIndex=-1表示行头。RowContextMenuStripNeeded则不存在e.ColumnIndex=-1的情况。
        //这个方法可以控制不同位置 显示不同的右键菜单。灵活 控制
        private void dataGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.RowIndex < 0)
            {
                //设置列头右键
                //e.ContextMenuStrip = cmsHeaderCell;
            }
            else if (e.ColumnIndex < 0)
            {
                //设置行头右键菜单
                //e.ContextMenuStrip = cmsRow;
            }
            else if (dgv[e.ColumnIndex, e.RowIndex].Value.ToString().Equals("男"))
            {
                //e.ContextMenuStrip = cmsCell;
            }
            else
            {
                //e.ContextMenuStrip = cmsDgv;
            }
        }
        */

        #region 显示值



        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }



        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!dataGridView1.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //图片特殊处理
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }

            //固定字典值显示
            string colDbName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                    }

                }
            }

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is tb_P4Button)
            {
                tb_P4Button pb = dataGridView1.Rows[e.RowIndex].DataBoundItem as tb_P4Button;
                if (pb.tb_buttoninfo != null && colDbName == pb.GetPropertyName<tb_P4Button>(c => c.ButtonInfo_ID))
                {
                    e.Value = pb.tb_buttoninfo.BtnText;
                    return;
                }
                //因为是下拉选的角色来配置这里直接用下拉的
                if (colDbName == pb.GetPropertyName<tb_P4Button>(c => c.RoleID))
                {
                    e.Value = CurrentRole.RoleName;
                    return;
                }


            }

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_ButtonInfo>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }



        }


        private void DataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!dataGridView2.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //图片特殊处理
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    return;
                }
            }
            //固定字典值显示
            string colDbName = dataGridView2.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                    }

                }
            }

            if (dataGridView2.Rows[e.RowIndex].DataBoundItem is tb_P4Field)
            {
                tb_P4Field pf = dataGridView2.Rows[e.RowIndex].DataBoundItem as tb_P4Field;

                if (pf.tb_fieldinfo != null && colDbName == pf.GetPropertyName<tb_P4Field>(c => c.FieldInfo_ID))
                {
                    e.Value = pf.tb_fieldinfo.FieldText;
                }
                //因为是下拉选的角色来配置这里直接用下拉的
                if (colDbName == pf.GetPropertyName<tb_P4Field>(c => c.RoleID))
                {
                    e.Value = CurrentRole.RoleName;
                    return;
                }
            }

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_FieldInfo>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }




        }


        private void DataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridViewRowAuthPolicy.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //图片特殊处理
            if (newSumDataGridViewRowAuthPolicy.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    return;
                }
            }
            //固定字典值显示
            string colDbName = dataGridView2.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                    }

                }
            }

            if (newSumDataGridViewRowAuthPolicy.Rows[e.RowIndex].DataBoundItem is tb_P4RowAuthPolicyByRole authPolicy)
            {
                if (authPolicy.tb_rowauthpolicy != null && colDbName == authPolicy.GetPropertyName<tb_RowAuthPolicy>(c => c.PolicyId))
                {
                    e.Value = authPolicy.tb_rowauthpolicy.PolicyName;
                }

                if (colDbName == authPolicy.GetPropertyName<tb_MenuInfo>(c => c.MenuID))
                {
                    e.Value = CurrentMenuInfo.MenuName;
                    return;
                }

                //因为是下拉选的角色来配置这里直接用下拉的
                if (colDbName == authPolicy.GetPropertyName<tb_RoleInfo>(c => c.RoleID))
                {
                    e.Value = CurrentRole.RoleName;
                    return;
                }
            }

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_RowAuthPolicy>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

        }


        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // 选择了行和列
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
            }
        }

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // 选择了行和列
                    dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
            }
        }

        /*
                /// <summary>
                /// 右键列头
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ShowMenu(DataGridView1, headMenu, e);
                    }
                }

                /// <summary>
                /// 右键内容区域
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ShowMenu(DataGridView1, MenuRemove, e);
                    }
                }
        */
        #endregion

        private void selectAll_Click(object sender, EventArgs e)
        {
            //dataGridView1.Columns[4].CellTemplate
            if (dataGridView1.CurrentCell != null)
            {

            }
        }

        private void selectNoAll_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStrip1.Tag = null;
            NewSumDataGridView dg = new NewSumDataGridView();
            if (contextMenuStrip1.SourceControl == null)
            {
                if (dataGridView1.Focused)
                {
                    dg = dataGridView1;
                }
            }
            else
            {
                //找到点击的列保存到tag中。再单项点时控制
                dg = contextMenuStrip1.SourceControl as NewSumDataGridView;
            }
            Point pt = dg.PointToClient(Cursor.Position);
            for (int i = 0; i < dg.ColumnCount; i++)
            {
                Rectangle r = dg.GetColumnDisplayRectangle(i, true);
                if (r.Contains(pt))
                {
                    contextMenuStrip1.Tag = i;
                    return;
                }
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (contextMenuStrip1.Tag != null)
            {
                NewSumDataGridView dg = new NewSumDataGridView();
                if (dataGridView1.Focused)
                {
                    dg = dataGridView1;
                }
                else
                {
                    if (contextMenuStrip1.SourceControl != null)
                    {
                        dg = contextMenuStrip1.SourceControl as NewSumDataGridView;
                    }
                }
                int ClickedColindex = int.Parse(contextMenuStrip1.Tag.ToString());
                foreach (DataGridViewRow dr in dg.Rows)
                {
                    if (e.ClickedItem.Text == "全不选")
                    {
                        dr.Cells[ClickedColindex].Value = false;
                    }
                    else
                    {
                        dr.Cells[ClickedColindex].Value = true;
                    }

                }
            }

        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemInitBtn_Click(object sender, EventArgs e)
        {
            if (CurrentRole == null)
            {
                return;
            }
            if (TreeView1.SelectedNode == null)
            {
                return;
            }
            if (!(TreeView1.SelectedNode.Tag is tb_MenuInfo))
            {
                return;
            }

            tb_MenuInfo mInfo = TreeView1.SelectedNode.Tag as tb_MenuInfo;
            InitLoadP4Button(mInfo, true);
        }

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemInitField_Click(object sender, EventArgs e)
        {
            if (CurrentRole == null)
            {
                return;
            }
            if (TreeView1.SelectedNode == null)
            {
                return;
            }
            if (!(TreeView1.SelectedNode.Tag is tb_MenuInfo))
            {
                return;
            }
            tb_MenuInfo mInfo = TreeView1.SelectedNode.Tag as tb_MenuInfo;


            InitLoadP4Field(mInfo, true);

        }

        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            toolStripButtonSave.Enabled = true;
        }


        private void toolsbtnFullAuthorization_ClickAsync(object sender, EventArgs e)
        {
            if (CurrentRole == null)
            {
                MessageBox.Show("请选择要设置的角色。");
                return;
            }
            if (CurrentRole.RoleName == "请选择")
            {
                MessageBox.Show("请选择要设置的角色。");
                return;
            }
            //当前操作会将当前角色的权限完全授权，请确认是否继续？
            if (MessageBox.Show($"【{CurrentRole.RoleName}】的权限将完全授权为【超级管理员】角色，请确认是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ////全量授权，菜单全选。每个菜单的按钮全启用可见，每个菜单的字段全显示
                //#region

                ////默认插入一遍,并且重新加载一下
                //InitMenuByRole(CurrentRole, true);
                ////bool rs = await LoadTreeView();
                //////循环去钩选
                ////UpdateP4MenuUI(TreeView1.Nodes, CurrentRole.tb_P4Menus);

                //#endregion

                using (var dialog = new ProgressDialog("数据处理中...", true))
                {
                    dialog.RunAsync(async (progress, ct) =>
                    {
                        try
                        {
                            // 启动后台操作
                            progress.Report((0, "开始初始化..."));
                            // 你的初始化逻辑
                            var newMenus = await InitMenuByRoleAsync(CurrentRole, true, progress, ct);
                            await BulkInsertMenusAsync(newMenus, progress, ct);
                            progress.Report((100, "完成"));
                        }
                        catch (Exception ex)
                        {
                            // 记录日志
                            MainForm.Instance.logger.Error(ex, "操作失败");
                            throw; // 自动触发Abort状态
                        }
                    });

                    var result = dialog.ShowDialog(this);

                    // 使用switch处理所有可能结果
                    switch (result)
                    {
                        case DialogResult.OK:
                            UpdateP4MenuUI(TreeView1.Nodes, CurrentRole.tb_P4Menus);

                            MessageBox.Show("操作成功完成");
                            break;
                        case DialogResult.Cancel:
                            //_logger.Warn("用户取消了操作");
                            break;
                        case DialogResult.Abort:
                            MessageBox.Show("操作异常终止", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }


                }

            }
        }


        #region 全量复制 

        private async Task<List<tb_P4Menu>> InitMenuByRoleAsync(tb_RoleInfo role,
            bool selected, IProgress<(int Percentage, string Message)> progress = null,
            CancellationToken ct = new CancellationToken())
        {
            var newMenus = new List<tb_P4Menu>();
            var list = await ctrMenu.QueryByNavAsync();

            for (int i = 0; i < list.Count; i++)
            {
                ct.ThrowIfCancellationRequested();
                var item = list[i];
                if (progress != null)
                {
                    // 计算进度百分比
                    var percentage = (int)((i + 1) * 100 / list.Count);
                    progress.Report((percentage, $"处理菜单：{item.MenuName} ({i + 1}/{list.Count})"));
                }
                var pm = await ProcessMenuItemAsync(role, item, selected);
                if (pm != null && pm.P4Menu_ID == 0)
                {
                    newMenus.Add(pm);
                }
            }

            return newMenus;
        }

        private async Task<tb_P4Menu> ProcessMenuItemAsync(tb_RoleInfo role, tb_MenuInfo menu, bool selected)
        {
            return await Task.Run(async () =>
            {
                var pm = role.tb_P4Menus.FirstOrDefault(c => c.MenuID == menu.MenuID) ?? new tb_P4Menu();

                pm.RoleID = role.RoleID;
                pm.MenuID = menu.MenuID;
                pm.ModuleID = menu.ModuleID;
                pm.IsVisble = selected || menu.IsVisble;

                if (menu.MenuType == "行为菜单")
                {
                    List<tb_P4Button> p4Buttons = await InitBtnByRole(role, menu, selected);
                    List<tb_P4Field> p4Fields = await InitFiledByRole(role, menu, selected);

                }

                if (pm.P4Menu_ID == 0)
                {
                    BusinessHelper.Instance.InitEntity(pm);
                }

                return pm;
            });
        }

        private async Task BulkInsertMenusAsync(List<tb_P4Menu> menus, IProgress<(int Percentage, string Message)> progress,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            progress.Report((99, "正在保存到数据库..."));

            // 使用批量插入
            var inserted = await ctrPMenu.AddAsync(menus);

            // 更新当前角色的菜单集合
            CurrentRole.tb_P4Menus.AddRange(menus);

        }



        #endregion

        private async void tsbtnDelete_Click(object sender, EventArgs e)
        {

            if (kryptonNavigator1.SelectedPage.Name == kryptonPageBtn.Name)
            {
                if (dataGridView1.UseSelectedColumn)
                {
                    await BatchDelete<tb_P4Button>(ListDataSoure1, true);
                }
                else
                {
                    await Delete<tb_P4Button>(ListDataSoure1);
                }
            }

            if (kryptonNavigator1.SelectedPage.Name == kryptonPageFieldInfo.Name)
            {
                tb_MenuInfo selectMenu = TreeView1.SelectedNode.Tag as tb_MenuInfo;
                if (dataGridView1.UseSelectedColumn)
                {
                    List<long> ids = await BatchDelete<tb_P4Field>(ListDataSoure2, true);
                    selectMenu.tb_P4Fields = selectMenu.tb_P4Fields.Where(c => !ids.Contains(c.P4Field_ID)).ToList();
                }
                else
                {
                    long id = await Delete<tb_P4Field>(ListDataSoure2);
                    selectMenu.tb_P4Fields = selectMenu.tb_P4Fields.Where(c => c.P4Field_ID != id).ToList();
                }
            }
        }

        /// <summary>
        /// 注意这里是物理删除
        /// </summary>
        protected async Task<long> Delete<T>(BindingSource bindingSource) where T : class
        {
            long id = 0;
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            bool rs = false;
            if (MessageBox.Show("系统不建议删除基本资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                T loc = (T)bindingSource.Current;
                string PKColName = UIHelper.GetPrimaryKeyColName(typeof(T));
                object PKValue = bindingSource.Current.GetPropertyValue(PKColName);
                bindingSource.Remove(loc);
                rs = await ctr.BaseDeleteAsync(loc);
                if (rs)
                {
                    id = PKValue.ToLong();
                    if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                    {
                        MainForm.Instance.logger.LogInformation($"删除:{typeof(T).Name}，主键值：{PKValue.ToString()} ");
                    }
                }
            }
            return id;
        }


        protected async virtual Task<List<long>> BatchDelete<T>(BindingSource bindingSourceList, bool UseSelectedColumn) where T : class
        {
            List<long> ids = new List<long>();
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            List<T> SelectedList = new List<T>();
            //多选模式时
            if (UseSelectedColumn)
            {
                foreach (var item in bindingSourceList)
                {
                    if (item is T sourceEntity)
                    {
                        var selected = (sourceEntity as BaseEntity).Selected;
                        if (selected.HasValue && selected.Value)
                        {
                            SelectedList.Add(sourceEntity);
                        }
                    }
                }
            }
            bool rs = false;
            int counter = 0;
            if (MessageBox.Show($"系统不建议删除基本资料\r\n确定删除选择的【{SelectedList.Count}】条记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //再次尝试单个导航删除
                for (int i = 0; i < SelectedList.Count; i++)
                {
                    T loc = SelectedList[i];
                    string PKColName = UIHelper.GetPrimaryKeyColName(typeof(T));
                    object PKValue = SelectedList[i].GetPropertyValue(PKColName);
                    bindingSourceList.Remove(loc);
                    rs = await ctr.BaseDeleteAsync(loc);
                    if (rs)
                    {
                        ids.Add(PKValue.ToLong());
                        counter++;
                        if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                        {
                            MainForm.Instance.logger.LogInformation($"删除:{typeof(T).Name}，主键值：{PKValue.ToString()} ");
                        }
                        MainForm.Instance.AuditLogHelper.CreateAuditLog("删除", "角色授权时");
                    }
                }
            }

            #region 更新缓存
            if (SelectedList.Count == counter)
            {
                foreach (var item in SelectedList)
                {
                    string PKColName = UIHelper.GetPrimaryKeyColName(typeof(T));
                    object PKValue = item.GetPropertyValue(PKColName);
                    if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                    {
                        MainForm.Instance.logger.LogInformation($"删除:{typeof(T).Name}，主键值：{PKValue.ToString()} ");
                    }

                    //提示服务器开启推送工作流
                    //OriginalData beatDataDel = ClientDataBuilder.BaseInfoChangeBuilder(typeof(T).Name);
                    //MainForm.Instance.ecs.AddSendData(beatDataDel);
                    RUINORERP.UI.Common.CacheManager.DeleteEntity(typeof(T).Name, PKValue.ToLong());

                }

            }

            #endregion
            return ids;
        }


        private void Test()
        {
            using (var dialog = new ProgressDialog("角色授权", allowCancel: true))
            {
                dialog.RunAsync(async (progress, ct) =>
                {
                    try
                    {
                        progress.Report((0, "开始初始化权限..."));

                        // 模拟长时间操作
                        for (int i = 0; i <= 100; i++)
                        {
                            ct.ThrowIfCancellationRequested();
                            await Task.Delay(5, ct);
                            progress.Report((i, $"处理进度 {i}%"));
                        }
                        progress.Report((100, $"处理进度完成。%"));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        throw;
                    }
                });

                var result = dialog.ShowDialog(this);
                switch (result)
                {
                    case DialogResult.OK:
                        MessageBox.Show("操作成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case DialogResult.Cancel:
                        MessageBox.Show("用户取消", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case DialogResult.Abort:
                        MessageBox.Show("操作异常", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }


        private void CopyRole(ToolStripMenuItem menuItem)
        {
            //复制选中角色的权限，包括显示的菜单，每个菜单下的按钮和字段,
            //思路是按选中角色查出 tb_P4Menu，tb_P4Button，tb_P4Field的集合。再去对比 当前节点选中的角色是否拥有。没有则复制过来。有则不管？
            #region
            var sourceRole = menuItem?.Tag as tb_RoleInfo;

            if (sourceRole == null || CurrentRole == null)
            {
                MessageBox.Show("请选择有效的源角色和目标角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sourceRole.RoleID == CurrentRole.RoleID)
            {
                MessageBox.Show("不能将角色权限复制到自身", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 确认对话框
            if (MessageBox.Show($"确定要将角色【{sourceRole.RoleName}】的权限复制到角色【{CurrentRole.RoleName}】吗？\n\n这将覆盖当前角色的所有权限设置。",
                               "确认复制权限", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            using (var dialog = new ProgressDialog("正在复制权限...", true))
            {
                dialog.RunAsync(async (progress, ct) =>
                {
                    try
                    {
                        progress.Report((0, "开始复制权限..."));

                        // 复制菜单权限
                        await CopyMenuPermissions(sourceRole, progress, ct);

                        // 复制按钮权限
                        await CopyButtonPermissions(sourceRole, progress, ct);

                        // 复制字段权限
                        await CopyFieldPermissions(sourceRole, progress, ct);

                        progress.Report((100, "权限复制完成"));

                        // 更新UI
                        UpdateUI(() =>
                        {
                            UpdateP4MenuUI(TreeView1.Nodes, CurrentRole.tb_P4Menus);
                            MessageBox.Show("权限复制成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        });
                    }
                    catch (OperationCanceledException)
                    {
                        progress.Report((100, "操作已取消"));
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.Error("复制权限失败", ex);
                        progress.Report((100, $"错误: {ex.Message}"));
                        throw;
                    }
                });

                dialog.ShowDialog(this);
            }

            #endregion
        }

        // 角色权限复制功能实现

        private List<tb_RoleInfo> _sourceRoles = new List<tb_RoleInfo>();

        // 在初始化时加载可参考的源角色列表
        private async Task InitializeSourceRoles()
        {
            try
            {
                // 获取所有角色列表，排除当前选中的角色
                var allRoles = await ctrRole.QueryAsync();
                _sourceRoles = allRoles.Where(r => r.RoleID != CurrentRole?.RoleID).ToList();

                // 清空并加载下拉菜单
                toolStripCopyRoleConfig.DropDownItems.Clear();
                foreach (var role in _sourceRoles)
                {
                    var item = new ToolStripMenuItem(role.RoleName);
                    item.Tag = role;
                    //item.Click += SourceRole_Click;
                    toolStripCopyRoleConfig.DropDownItems.Add(item);
                }

                // 如果没有可参考的角色，禁用菜单项
                toolStripCopyRoleConfig.Enabled = _sourceRoles.Any();
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("加载源角色列表失败", ex);
                MessageBox.Show($"加载源角色列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #region 复制权限

        // 复制菜单权限
        private async Task CopyMenuPermissions(tb_RoleInfo sourceRole, IProgress<(int Percentage, string Message)> progress, CancellationToken ct)
        {
            try
            {
                // 获取源角色的菜单权限
                var sourceP4Menus = await MainForm.Instance.AppContext.Db.Queryable<tb_P4Menu>()
                    .Where(pm => pm.RoleID == sourceRole.RoleID)
                    .ToListAsync();

                // 获取所有菜单信息
                var allMenus = await ctrMenu.QueryAsync();

                // 清除当前角色的菜单权限
                var currentP4Menus = CurrentRole.tb_P4Menus.Where(pm => pm.RoleID == CurrentRole.RoleID).ToList();
                if (currentP4Menus.Any())
                {
                    await MainForm.Instance.AppContext.Db.Deleteable(currentP4Menus).ExecuteCommandAsync();
                    CurrentRole.tb_P4Menus.RemoveAll(pm => pm.RoleID == CurrentRole.RoleID);
                }

                // 复制菜单权限
                var newP4Menus = new List<tb_P4Menu>();
                for (int i = 0; i < sourceP4Menus.Count; i++)
                {
                    ct.ThrowIfCancellationRequested();

                    var sourceP4Menu = sourceP4Menus[i];
                    var menu = allMenus.FirstOrDefault(m => m.MenuID == sourceP4Menu.MenuID);

                    if (menu != null)
                    {
                        var newP4Menu = new tb_P4Menu
                        {
                            RoleID = CurrentRole.RoleID,
                            MenuID = sourceP4Menu.MenuID,
                            ModuleID = sourceP4Menu.ModuleID,
                            IsVisble = sourceP4Menu.IsVisble,
                        };

                        BusinessHelper.Instance.InitEntity(newP4Menu);
                        newP4Menus.Add(newP4Menu);
                    }

                    if (progress != null && i % 10 == 0)
                    {
                        var percentage = (int)((i + 1) * 100 / sourceP4Menus.Count);
                        progress.Report((percentage, $"复制菜单权限: {i + 1}/{sourceP4Menus.Count}"));
                    }
                }

                // 批量插入新的菜单权限
                if (newP4Menus.Any())
                {
                    await ctrPMenu.AddAsync(newP4Menus);
                    CurrentRole.tb_P4Menus.AddRange(newP4Menus);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("复制菜单权限失败", ex);
                throw;
            }
        }

        // 复制按钮权限
        private async Task CopyButtonPermissions(tb_RoleInfo sourceRole, IProgress<(int Percentage, string Message)> progress, CancellationToken ct)
        {
            try
            {
                // 获取源角色的按钮权限
                var sourceP4Buttons = await MainForm.Instance.AppContext.Db.Queryable<tb_P4Button>()
                    .Where(pb => pb.RoleID == sourceRole.RoleID)
                    .ToListAsync();

                // 获取所有菜单信息
                var allMenus = await ctrMenu.QueryAsync();

                // 清除当前角色的按钮权限
                var currentP4Buttons = CurrentRole.tb_P4Buttons.Where(pb => pb.RoleID == CurrentRole.RoleID).ToList();
                if (currentP4Buttons.Any())
                {
                    await MainForm.Instance.AppContext.Db.Deleteable(currentP4Buttons).ExecuteCommandAsync();
                    foreach (var menu in allMenus)
                    {
                        if (menu.tb_P4Buttons != null)
                        {
                            menu.tb_P4Buttons.RemoveAll(pb => pb.RoleID == CurrentRole.RoleID);
                        }
                    }
                    CurrentRole.tb_P4Buttons.Where(pb => pb.RoleID == CurrentRole.RoleID).ToList().Clear();
                }

                // 复制按钮权限
                var newP4Buttons = new List<tb_P4Button>();
                for (int i = 0; i < sourceP4Buttons.Count; i++)
                {
                    ct.ThrowIfCancellationRequested();

                    var sourceP4Button = sourceP4Buttons[i];
                    var menu = allMenus.FirstOrDefault(m => m.MenuID == sourceP4Button.MenuID);

                    if (menu != null)
                    {
                        var newP4Button = new tb_P4Button
                        {
                            RoleID = CurrentRole.RoleID,
                            MenuID = sourceP4Button.MenuID,
                            ButtonInfo_ID = sourceP4Button.ButtonInfo_ID,
                            IsVisble = sourceP4Button.IsVisble,
                            IsEnabled = sourceP4Button.IsEnabled,
                        };

                        BusinessHelper.Instance.InitEntity(newP4Button);
                        newP4Buttons.Add(newP4Button);

                        // 更新菜单的按钮权限集合
                        if (menu.tb_P4Buttons == null)
                        {
                            menu.tb_P4Buttons = new List<tb_P4Button>();
                        }
                        menu.tb_P4Buttons.Add(newP4Button);
                    }

                    if (progress != null && i % 20 == 0)
                    {
                        var percentage = (int)((i + 1) * 100 / sourceP4Buttons.Count);
                        progress.Report((percentage, $"复制按钮权限: {i + 1}/{sourceP4Buttons.Count}"));
                    }
                }

                // 批量插入新的按钮权限
                if (newP4Buttons.Any())
                {
                    await ctrPBut.AddAsync(newP4Buttons);
                    CurrentRole.tb_P4Buttons.AddRange(newP4Buttons);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("复制按钮权限失败", ex);
                throw;
            }
        }

        // 复制字段权限
        private async Task CopyFieldPermissions(tb_RoleInfo sourceRole, IProgress<(int Percentage, string Message)> progress, CancellationToken ct)
        {
            try
            {
                // 获取源角色的字段权限
                var sourceP4Fields = await MainForm.Instance.AppContext.Db.Queryable<tb_P4Field>()
                    .Where(pf => pf.RoleID == sourceRole.RoleID)
                    .ToListAsync();

                // 获取所有菜单信息
                var allMenus = await ctrMenu.QueryAsync();

                // 清除当前角色的字段权限
                var currentP4Fields = CurrentRole.tb_P4Fields.Where(pf => pf.RoleID == CurrentRole.RoleID).ToList();
                if (currentP4Fields.Any())
                {
                    await MainForm.Instance.AppContext.Db.Deleteable(currentP4Fields).ExecuteCommandAsync();
                    foreach (var menu in allMenus)
                    {
                        if (menu.tb_P4Fields != null)
                        {
                            menu.tb_P4Fields.RemoveAll(pf => pf.RoleID == CurrentRole.RoleID);
                        }
                    }
                }

                // 复制字段权限
                var newP4Fields = new List<tb_P4Field>();
                for (int i = 0; i < sourceP4Fields.Count; i++)
                {
                    ct.ThrowIfCancellationRequested();

                    var sourceP4Field = sourceP4Fields[i];
                    var menu = allMenus.FirstOrDefault(m => m.MenuID == sourceP4Field.MenuID);

                    if (menu != null)
                    {
                        var newP4Field = new tb_P4Field
                        {
                            RoleID = CurrentRole.RoleID,
                            MenuID = sourceP4Field.MenuID,
                            FieldInfo_ID = sourceP4Field.FieldInfo_ID,
                            IsVisble = sourceP4Field.IsVisble,
                            IsChild = sourceP4Field.IsChild
                        };

                        BusinessHelper.Instance.InitEntity(newP4Field);
                        newP4Fields.Add(newP4Field);

                        // 更新菜单的字段权限集合
                        if (menu.tb_P4Fields == null)
                        {
                            menu.tb_P4Fields = new List<tb_P4Field>();
                        }
                        menu.tb_P4Fields.Add(newP4Field);
                    }

                    if (progress != null && i % 20 == 0)
                    {
                        var percentage = (int)((i + 1) * 100 / sourceP4Fields.Count);
                        progress.Report((percentage, $"复制字段权限: {i + 1}/{sourceP4Fields.Count}"));
                    }
                }

                // 批量插入新的字段权限
                if (newP4Fields.Any())
                {
                    await ctrPField.AddAsync(newP4Fields);
                    CurrentRole.tb_P4Fields.AddRange(newP4Fields);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("复制字段权限失败", ex);
                throw;
            }

        }
        #endregion
        private void kryptonNavigator1_SelectedPageChanged(object sender, EventArgs e)
        {
            #region 设置全选菜单

            if (kryptonNavigator1.SelectedPage.Name == kryptonPageBtn.Name)
            {
                //如果是bool型的才显示右键菜单全选全不选
                foreach (DataGridViewColumn dc in dataGridView1.Columns)
                {
                    if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
                    {
                        dc.HeaderCell.ContextMenuStrip = contextMenuStrip1;
                    }
                }
            }
            else
            {
                foreach (DataGridViewColumn dc in dataGridView2.Columns)
                {
                    if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
                    {
                        dc.HeaderCell.ContextMenuStrip = contextMenuStrip1;
                    }
                }
            }

            #endregion
        }

        tb_MenuInfo CurrentMenuInfo;
        private void cmbRowAuthPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 获取该业务类型支持的默认规则选项
            if (TreeView1.SelectedNode != null && cmbRowAuthPolicy.SelectedItem is tb_RowAuthPolicy policy)
            {
                // 为每个默认选项创建策略
                tb_MenuInfo selectMenu = TreeView1.SelectedNode.Tag as tb_MenuInfo;
                if (selectMenu.MenuType != "行为菜单")
                {
                    return;
                }

                if (selectMenu.BizType.HasValue && CurrentRole != null)
                {
                    BizType bizType = (BizType)selectMenu.BizType.Value;
                    // 创建角色与行级规则的关联对象
                    tb_P4RowAuthPolicyByRole newPolicyRelation = new tb_P4RowAuthPolicyByRole
                    {
                        RoleID = CurrentRole.RoleID,
                        MenuID = selectMenu.MenuID,
                        PolicyId = policy.PolicyId,
                        IsEnabled = true,
                    };

                    BusinessHelper.Instance.InitEntity(newPolicyRelation);

                    // 检查是否已存在相同的规则关联（一个规则在一个角色对应的一个菜单下面只能添加一次）
                    bool isDuplicate = false;
                    foreach (tb_P4RowAuthPolicyByRole existingRelation in bindingSourceRowAuthPolicy)
                    {
                        if (existingRelation.RoleID == newPolicyRelation.RoleID &&
                            existingRelation.MenuID == newPolicyRelation.MenuID &&
                            existingRelation.PolicyId == newPolicyRelation.PolicyId)
                        {
                            isDuplicate = true;
                            break;
                        }
                    }

                    if (!isDuplicate)
                    {
                        // 添加到数据源
                        bindingSourceRowAuthPolicy.Add(newPolicyRelation);
                        toolStripButtonSave.Enabled = true;
                    }
                    else
                    {
                        // 已存在相同的规则关联，不重复添加
                        MessageBox.Show("该规则已添加到当前角色的此菜单中，不能重复添加。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }



}

