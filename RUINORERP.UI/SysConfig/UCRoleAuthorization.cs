using Castle.Components.DictionaryAdapter.Xml;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Forms;
using Krypton.Navigator;
using Krypton.Workspace;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mysqlx.Crud;
using Netron.GraphLib;
using OpenTK.Input;
using RUINOR.WinFormsUI.TreeViewThreeState;
using RUINORERP.Business;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Common;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.UI.SS;
using RUINORERP.UI.UControls;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;



namespace RUINORERP.UI.SysConfig
{

    /// <summary>
    /// 比用户授权角色简单，那个是行记录存在性控制， 这里是默认每个角色都有。通过关系表中的字段来控制的1
    /// </summary>
    [MenuAttrAssemblyInfo("角色授权", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCRoleAuthorization : UserControl, IContextMenuInfoAuth
    {
        private readonly IRowAuthService _rowAuthService;

        // 缓存默认的行级权限策略，避免重复从数据库加载
        private Dictionary<BizType, List<tb_RowAuthPolicy>> _policyCache = new Dictionary<BizType, List<tb_RowAuthPolicy>>();

        // 菜单树缓存，避免重复加载
        private List<tb_ModuleDefinition> _menuTreeCache = null;

        // 加载状态标志，防止重复加载
        private bool _isLoading = false;

        private CancellationTokenSource _loadingCancellationTokenSource;

        // DataGridView配置状态缓存
        private bool _dataGridView1Configured = false;
        private bool _dataGridView2Configured = false;

        /// <summary>
        /// 异步初始化任务的缓存，避免重复初始化同一菜单
        /// Key: MenuID, Value: 正在执行的Task
        /// </summary>
        private readonly ConcurrentDictionary<long, Task> _initializationTasks = new ConcurrentDictionary<long, Task>();

        public GridViewDisplayTextResolver DisplayTextResolver;
        public GridViewDisplayTextResolver DisplayTextResolverForField;
        public GridViewDisplayTextResolver DisplayTextResolverForButton;

        public UCRoleAuthorization()
        {
            InitializeComponent();
            // 在窗体构造函数中
            typeof(TreeView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, TreeView1, new object[] { true });
            BuildContextMenuController();
            _rowAuthService = Startup.GetFromFac<IRowAuthService>();
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

            if (dataGridViewButton != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = this.dataGridViewButton.GetContextMenu(this.dataGridViewButton.ContextMenuStrip
                , ContextClickList, list, true
                    );
                dataGridViewButton.ContextMenuStrip = newContextMenuStrip;
            }

            if (dataGridViewField != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = this.dataGridViewField.GetContextMenu(this.dataGridViewField.ContextMenuStrip
                , ContextClickList, list, true
                    );
                dataGridViewField.ContextMenuStrip = newContextMenuStrip;
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
                dataGridViewButton.UseSelectedColumn = true;
                if (!dataGridViewButton.UseSelectedColumn)
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
                if (dataGridViewButton.UseSelectedColumn)
                {
                    for (int i = 0; i < dataGridViewButton.Rows.Count; i++)
                    {
                        var dr = dataGridViewButton.Rows[i];
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
                dataGridViewField.UseSelectedColumn = true;
                if (!dataGridViewField.UseSelectedColumn)
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
                if (dataGridViewButton.UseSelectedColumn)
                {
                    for (int i = 0; i < dataGridViewButton.Rows.Count; i++)
                    {
                        var dr = dataGridViewButton.Rows[i];
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
                    }
                    ;

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
            await UIBizService.RequestCache<tb_RowAuthPolicy>();
            DisplayTextResolver = new GridViewDisplayTextResolver(typeof(tb_P4RowAuthPolicyByRole));

            DisplayTextResolverForField = new GridViewDisplayTextResolver(typeof(tb_P4Field));
            DisplayTextResolverForField.AddReferenceKeyMapping<tb_FieldInfo, tb_P4Field>(c => c.FieldName, c => c.FieldInfo_ID, c => c.FieldText);

            DisplayTextResolverForButton = new GridViewDisplayTextResolver(typeof(tb_P4Button));
            DisplayTextResolverForButton.AddReferenceKeyMapping<tb_ButtonInfo, tb_P4Button>(c => c.BtnName, c => c.ButtonInfo_ID, c => c.BtnText);
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
            dataGridViewButton.NeedSaveColumnsXml = true;
            dataGridViewField.NeedSaveColumnsXml = true;
            newSumDataGridViewRowAuthPolicy.NeedSaveColumnsXml = true;
            dataGridViewButton.Use是否使用内置右键功能 = false;
            dataGridViewField.Use是否使用内置右键功能 = false;
            newSumDataGridViewRowAuthPolicy.Use是否使用内置右键功能 = false;
            // dataGridView1.ContextMenuStrip = contextMenuStrip1;
            // dataGridView2.ContextMenuStrip = contextMenuStrip1;
            dataGridViewButton.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDown);
            dataGridViewField.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView2_CellMouseDown);
            DisplayTextResolver.Initialize(newSumDataGridViewRowAuthPolicy);
            DisplayTextResolverForButton.Initialize(dataGridViewButton);
            DisplayTextResolverForField.Initialize(dataGridViewField);
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
        /// 将模块 菜单 显示为树（优化版本，增加缓存）
        /// </summary>
        private async Task<bool> LoadTreeView(bool Seleted = false)
        {
            // 使用缓存机制，避免重复加载菜单树
            if (_menuTreeCache != null)
            {
                DefaultModules = _menuTreeCache;
            }
            else
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

                // 缓存菜单树数据
                _menuTreeCache = DefaultModules;
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
            dataGridViewButton.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(tb_P4Button));
            dataGridViewButton.XmlFileName = "UCRoleAuthorization1";
            dataGridViewButton.FieldNameList = FieldNameList1;
            dataGridViewButton.DataSource = null;
            ListDataSoure1 = bindingSource1;
            //绑定导航
            dataGridViewButton.DataSource = ListDataSoure1.DataSource;

            dataGridViewField.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList2 = UIHelper.GetFieldNameColList(typeof(tb_P4Field));
            dataGridViewField.XmlFileName = "UCRoleAuthorization2";
            dataGridViewField.FieldNameList = FieldNameList2;
            dataGridViewField.DataSource = null;
            ListDataSoure2 = bindingSource2;
            //绑定导航
            dataGridViewField.DataSource = ListDataSoure2.DataSource;


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

            dataGridViewButton.EndEdit();
            dataGridViewField.EndEdit();

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

            // 防止重复加载
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            _loadingCancellationTokenSource = new CancellationTokenSource();

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

                // 检查是否需要加载菜单权限数据
                var pmlis = await db.Queryable<tb_P4Menu>()
                    .Where(r => r.RoleID == CurrentRole.RoleID)
                    .Includes(t => t.tb_menuinfo, b => b.tb_P4Buttons)
                    .Includes(t => t.tb_menuinfo, b => b.tb_P4Fields)
                    .Includes(t => t.tb_menuinfo, b => b.tb_P4Menus)
                    .ToListAsync(_loadingCancellationTokenSource.Token)
                    .ConfigureAwait(false);

                if (pmlis.Count == 0)
                {
                    // 改为异步初始化
                    await InitMenuByRoleAsync(CurrentRole, true)
                        .ConfigureAwait(false);
                }

                // 只在菜单树为空时重新加载
                if (TreeView1.Nodes.Count <= 1)
                {
                    await LoadTreeView();
                }

                // 检查取消请求
                _loadingCancellationTokenSource.Token.ThrowIfCancellationRequested();

                //循环去钩选
                UpdateP4MenuUI(TreeView1.Nodes[0].Nodes, CurrentRole.tb_P4Menus);

                // 默认展开两级节点（根节点和第一级模块节点）
                if (TreeView1.Nodes.Count > 0)
                {
                    // 展开根节点
                    TreeView1.Nodes[0].Expand();

                    // 展开所有第一级模块节点
                    foreach (TreeNode moduleNode in TreeView1.Nodes[0].Nodes)
                    {
                        moduleNode.Expand();
                    }
                }

            }
            catch (OperationCanceledException)
            {
                // 取消操作，不处理异常
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MainForm.Instance.logger.Error("角色选择时发生错误", ex);
            }
            finally
            {
                _isLoading = false;
                _loadingCancellationTokenSource?.Dispose();
                _loadingCancellationTokenSource = null;
            }
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
        private bool _isInitializing = false;

        /// <summary>
        /// 按角色初始化所有菜单（优化版本）
        /// </summary>
        /// <param name="role">目标角色</param>
        /// <param name="Selected">是否选中</param>
        public async Task InitMenuByRole(tb_RoleInfo role, bool Selected = false)
        {
            // 防止重复初始化
            if (_isInitializing)
            {
                MainForm.Instance.logger.Warn("菜单初始化正在进行中，跳过重复请求");
                return;
            }

            _isInitializing = true;

            try
            {
                var db = MainForm.Instance.AppContext.Db;
                var list = await ctrMenu.QueryByNavAsync();
                int totalItems = list.Count;

                // 预先过滤需要处理的菜单，减少循环中的计算
                var existingMenuDict = role.tb_P4Menus?
                    .Where(p => p.RoleID == role.RoleID)
                    .ToDictionary(p => p.MenuID.Value) ?? new Dictionary<long, tb_P4Menu>();

                List<tb_P4Menu> newMenus = new List<tb_P4Menu>();
                List<tb_P4Menu> menusToUpdate = new List<tb_P4Menu>();

                // 批量处理菜单 - 减少数据库操作
                foreach (var item in list)
                {
                    if (!existingMenuDict.TryGetValue(item.MenuID, out tb_P4Menu pm))
                    {
                        // 创建新菜单权限
                        pm = new tb_P4Menu
                        {
                            RoleID = role.RoleID,
                            MenuID = item.MenuID,
                            ModuleID = item.ModuleID,
                            IsVisble = Selected ? true : item.IsVisble
                        };

                        BusinessHelper.Instance.InitEntity(pm);
                        newMenus.Add(pm);
                    }
                    else
                    {
                        // 更新现有菜单权限
                        if (item.MenuType == "行为菜单")
                        {
                            // 并行初始化按钮和字段权限
                            var buttonTask = InitBtnByRole(role, item, true);
                            var fieldTask = InitFiledByRole(role, item, true);

                            // 等待两个任务完成
                            await Task.WhenAll(buttonTask, fieldTask);
                        }

                        // 更新菜单可见性
                        if (pm.IsVisble != (Selected ? true : item.IsVisble))
                        {
                            pm.IsVisble = Selected ? true : item.IsVisble;
                            menusToUpdate.Add(pm);
                        }
                    }
                }

                // 批量数据库操作
                if (newMenus.Count > 0)
                {
                    var ids = await ctrPMenu.AddAsync(newMenus);
                    if (ids?.Count > 0)
                    {
                        lock (_menuLock)
                        {
                            role.tb_P4Menus.AddRange(newMenus);
                        }
                    }
                }

                if (menusToUpdate.Count > 0)
                {
                    await db.Updateable(menusToUpdate).ExecuteCommandAsync();
                }

                // 更新UI
                UpdateUI(() =>
                {
                    CompleteMenuInitialization();
                });
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("菜单初始化失败", ex);
                throw;
            }
            finally
            {
                _isInitializing = false;
            }
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
                // 更新UI
                UpdateP4MenuUI(TreeView1.Nodes, CurrentRole.tb_P4Menus);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("Error in CompleteMenuInitialization", ex);
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private List<MenuAttrAssemblyInfo> MenuAssemblylist
        {
            get
            {
                if (_menuAssemblylist == null)
                {
                    // 确保在主线程调用
                    if (MainForm.Instance != null && MainForm.Instance.InvokeRequired)
                    {
                        MainForm.Instance.Invoke(new Action(() =>
                        {
                            _menuAssemblylist = UIHelper.RegisterForm();
                        }));
                    }
                    else
                    {
                        _menuAssemblylist = UIHelper.RegisterForm();
                    }
                }
                return _menuAssemblylist;
            }
        }
        private List<MenuAttrAssemblyInfo> _menuAssemblylist = null;



        /// <summary>
        /// 初始化菜单下的按钮权限（优化版本）
        /// 优化内容：使用更优雅的异步模式、增强错误处理、避免重复初始化
        /// </summary>
        /// <param name="CurrentRole">当前角色</param>
        /// <param name="SelectedMenuInfo">菜单下有默认的按钮数据</param>
        /// <param name="selected">是否选中</param>
        /// <returns>按钮权限列表</returns>
        public async Task<List<tb_P4Button>> InitBtnByRole(tb_RoleInfo CurrentRole, tb_MenuInfo SelectedMenuInfo, bool selected = false)
        {
            // 参数验证
            if (SelectedMenuInfo == null)
            {
                MainForm.Instance.logger?.LogWarning("菜单信息为空，无法初始化按钮权限");
                return new List<tb_P4Button>();
            }

            if (CurrentRole == null)
            {
                MainForm.Instance.logger?.LogWarning("角色信息为空，无法初始化按钮权限");
                return new List<tb_P4Button>();
            }

            // 确保菜单按钮信息已初始化
            SelectedMenuInfo.tb_ButtonInfos ??= new List<tb_ButtonInfo>();

            var menuId = SelectedMenuInfo.MenuID;

            // 检查是否已有正在执行的初始化任务（避免重复初始化）
            if (_initializationTasks.TryGetValue(menuId, out var existingTask))
            {
                MainForm.Instance.logger?.LogDebug($"菜单 {SelectedMenuInfo.CaptionCN} (ID:{menuId}) 正在初始化中，等待完成...");
                try
                {
                    await existingTask;
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger?.LogError(ex, $"等待菜单 {menuId} 初始化时发生错误");
                    _initializationTasks.TryRemove(menuId, out _);
                }
                return CurrentRole.tb_P4Buttons
                    .Where(c => c.RoleID == CurrentRole.RoleID && c.MenuID == menuId)
                    .ToList();
            }

            // 创建新的初始化任务
            var initTask = InitializeMenuButtonsAsync(SelectedMenuInfo, selected);
            _initializationTasks.TryAdd(menuId, initTask);

            try
            {
                await initTask;
            }
            finally
            {
                // 任务完成后移除缓存
                _initializationTasks.TryRemove(menuId, out _);
            }

            // 优化去重逻辑 - 使用更高效的查询
            var currentRoleId = CurrentRole.RoleID;

            // 检查并清理重复数据
            var duplicateButtons = CurrentRole.tb_P4Buttons
                .Where(c => c.RoleID == currentRoleId && c.MenuID == menuId)
                .GroupBy(p => p.ButtonInfo_ID)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Skip(1)) // 保留第一个，删除后续重复项
                .ToList();

            if (duplicateButtons.Any())
            {
                MainForm.Instance.logger.Warn($"发现重复按钮权限，菜单ID:{menuId},角色ID:{currentRoleId}");

                // 批量删除重复数据
                await MainForm.Instance.AppContext.Db.Deleteable<tb_P4Button>()
                    .Where(p => duplicateButtons.Select(d => d.P4Btn_ID).Contains(p.P4Btn_ID))
                    .ExecuteCommandAsync();

                // 从内存中移除重复项
                duplicateButtons.ForEach(d => CurrentRole.tb_P4Buttons.Remove(d));
            }

            // 使用字典优化查找性能
            var existingButtons = CurrentRole.tb_P4Buttons
                .Where(p => p.MenuID == menuId && p.RoleID == currentRoleId)
                .ToDictionary(p => p.ButtonInfo_ID);

            var newButtons = new List<tb_P4Button>();
            var updatedButtons = new List<tb_P4Button>();

            // 处理按钮信息 - 优化循环
            try
            {
                foreach (var buttonInfo in SelectedMenuInfo.tb_ButtonInfos)
                {
                    try
                    {
                        if (!existingButtons.TryGetValue(buttonInfo.ButtonInfo_ID, out tb_P4Button button))
                        {
                            // 创建新按钮权限
                            button = new tb_P4Button
                            {
                                Notes = buttonInfo.BtnText,
                                RoleID = currentRoleId,
                                ButtonInfo_ID = buttonInfo.ButtonInfo_ID,
                                MenuID = menuId,
                                ButtonType = buttonInfo.ButtonType
                            };

                            BusinessHelper.Instance.InitEntity(button);
                            SetButtonSelection(button, selected);
                            newButtons.Add(button);
                        }
                        else
                        {
                            // 更新现有按钮权限状态
                            if (!button.IsVisble && selected)
                            {
                                if (SetButtonSelection(button, true))
                                {
                                    updatedButtons.Add(button);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger?.LogError(ex, $"处理按钮信息失败: {buttonInfo.BtnText} (ID:{buttonInfo.ButtonInfo_ID})");
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"遍历菜单按钮信息时发生错误: 菜单ID={menuId}");
                throw;
            }

            // 批量操作优化
            try
            {
                if (newButtons.Count > 0)
                {
                    var ids = await ctrPBut.AddAsync(newButtons);
                    if (ids?.Count > 0)
                    {
                        CurrentRole.tb_P4Buttons.AddRange(newButtons);
                        MainForm.Instance.logger?.LogInformation($"为角色 {CurrentRole.RoleName} 添加了 {newButtons.Count} 个新按钮权限");
                    }
                }

                if (updatedButtons.Count > 0)
                {
                    await MainForm.Instance.AppContext.Db.Updateable(updatedButtons).ExecuteCommandAsync();
                    MainForm.Instance.logger?.LogInformation($"更新了 {updatedButtons.Count} 个按钮权限");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "批量保存按钮权限到数据库失败");
                throw;
            }

            // 返回更新后的按钮权限列表
            return CurrentRole.tb_P4Buttons
                .Where(c => c.RoleID == currentRoleId && c.MenuID == menuId)
                .ToList();
        }

        // 辅助方法：设置按钮选中状态并返回是否有变化
        /// <summary>
        /// 这个方法只设置，将没选，改为勾选。 选中方式不在这里处理。
        /// </summary>
        /// <param name="button">按钮权限对象</param>
        /// <param name="selected">是否选中</param>
        /// <returns>是否有变化</returns>
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
        /// 异步初始化菜单按钮的辅助方法
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <param name="selected">默认选中状态</param>
        private async Task InitializeMenuButtonsAsync(tb_MenuInfo menuInfo, bool selected)
        {
            try
            {
                MainForm.Instance.logger?.LogInformation($"开始初始化菜单按钮: {menuInfo.CaptionCN} (ID:{menuInfo.MenuID})");

                // 查找菜单属性信息
                MenuAttrAssemblyInfo mai = MenuAssemblylist.FirstOrDefault(e => e.ClassPath == menuInfo.ClassPath);
                if (mai == null)
                {
                    MainForm.Instance.logger?.LogWarning($"未找到菜单属性信息: {menuInfo.ClassPath}");
                    return;
                }

                // 获取InitModuleMenu服务并初始化按钮
                InitModuleMenu imm = null;
                await Task.Run(() =>
                {
                    imm = Startup.GetFromFac<InitModuleMenu>();
                });

                if (imm != null)
                {
                    // 使用缓存机制初始化按钮（useCache: true）
                    var buttons = await imm.InitToolStripItemAsync(mai, menuInfo, InsertToDb: true, useCache: true);
                    MainForm.Instance.logger?.LogInformation($"菜单 {menuInfo.CaptionCN} 初始化完成，按钮数: {buttons?.Count ?? 0}");
                }
                else
                {
                    MainForm.Instance.logger?.LogError("无法获取InitModuleMenu服务实例");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"初始化菜单按钮失败: {menuInfo.CaptionCN} (路径:{menuInfo.ClassPath})");
                // 不抛出异常，避免影响主流程
            }
        }




        InitModuleMenu imm = null;

        /// <summary>
        /// 初始化字段权限（优化版本）
        /// </summary>
        /// <param name="CurrentRole">当前角色</param>
        /// <param name="SelectedMenuInfo">选中菜单</param>
        /// <param name="selected">是否选中</param>
        /// <returns></returns>
        public async Task<List<tb_P4Field>> InitFiledByRole(tb_RoleInfo CurrentRole, tb_MenuInfo SelectedMenuInfo, bool selected = false)
        {
            // 空值检查
            if (SelectedMenuInfo == null || CurrentRole == null)
            {
                return new List<tb_P4Field>();
            }

            // 确保字段信息已初始化
            SelectedMenuInfo.tb_FieldInfos ??= new List<tb_FieldInfo>();

            // 使用缓存机制避免重复加载模型类型
            if (SelectedMenuInfo.tb_FieldInfos.Count == 0)
            {
                try
                {
                    // 使用后台任务加载模型类型，避免阻塞UI
                    await Task.Run(async () =>
                    {
                        var dalAssemble = AssemblyLoader.LoadAssembly("RUINORERP.Model");
                        var modelTypes = dalAssemble.GetExportedTypes();
                        var typeNames = modelTypes.Select(m => m.Name).ToList();
                        var mainType = modelTypes.FirstOrDefault(e => e.Name == SelectedMenuInfo.EntityName);

                        if (mainType != null)
                        {
                            // 在UI线程上下文中获取服务实例（InitModuleMenu构造函数需要STA线程）
                            InitModuleMenu imm = null;

                            imm = Startup.GetFromFac<InitModuleMenu>();
                            imm.MenuAssemblyList = MenuAssemblylist;

                            if (imm != null)
                            {
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
                    });
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.Error("加载模型类型时出错", ex);
                }
            }

            // 优化去重逻辑
            var currentRoleId = CurrentRole.RoleID;
            var menuId = SelectedMenuInfo.MenuID;

            // 检查并清理重复数据
            var duplicateFields = CurrentRole.tb_P4Fields
                .Where(c => c.RoleID == currentRoleId && c.MenuID == menuId)
                .GroupBy(p => p.FieldInfo_ID)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Skip(1)) // 保留第一个，删除后续重复项
                .ToList();

            if (duplicateFields.Any())
            {
                MainForm.Instance.logger.Warn($"发现重复字段权限，菜单ID:{menuId},角色ID:{currentRoleId}");

                // 批量删除重复数据
                await MainForm.Instance.AppContext.Db.Deleteable<tb_P4Field>()
                    .Where(p => duplicateFields.Select(d => d.P4Field_ID).Contains(p.P4Field_ID))
                    .ExecuteCommandAsync();

                // 从内存中移除重复项
                duplicateFields.ForEach(d => CurrentRole.tb_P4Fields.Remove(d));
            }

            // 使用字典优化查找性能
            var existingFields = CurrentRole.tb_P4Fields
                .Where(p => p.MenuID == menuId && p.RoleID == currentRoleId)
                .ToDictionary(p => p.FieldInfo_ID);

            var newFields = new List<tb_P4Field>();
            var updatedFields = new List<tb_P4Field>();

            // 处理字段信息 - 优化循环
            foreach (var fieldInfo in SelectedMenuInfo.tb_FieldInfos)
            {
                if (!existingFields.TryGetValue(fieldInfo.FieldInfo_ID, out tb_P4Field field))
                {
                    // 创建新字段权限
                    field = new tb_P4Field
                    {
                        Notes = fieldInfo.FieldText,
                        IsChild = fieldInfo.IsChild,
                        RoleID = currentRoleId,
                        FieldInfo_ID = fieldInfo.FieldInfo_ID,
                        MenuID = menuId,
                        IsVisble = selected
                    };

                    BusinessHelper.Instance.InitEntity(field);
                    newFields.Add(field);
                }
                else
                {
                    // 更新现有字段权限状态
                    if (!field.IsVisble && selected)
                    {
                        field.IsVisble = selected;
                        updatedFields.Add(field);
                    }
                }
            }

            // 批量操作优化
            if (newFields.Count > 0)
            {
                var ids = await ctrPField.AddAsync(newFields);
                if (ids?.Count > 0)
                {
                    CurrentRole.tb_P4Fields.AddRange(newFields);
                }
            }

            if (updatedFields.Count > 0)
            {
                await MainForm.Instance.AppContext.Db.Updateable(updatedFields).ExecuteCommandAsync();
            }

            // 返回更新后的字段权限列表
            return CurrentRole.tb_P4Fields
                .Where(c => c.RoleID == currentRoleId && c.MenuID == menuId)
                .ToList();
        }

        #endregion


        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;
        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList2;
        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList3;

        tb_RoleInfoController<tb_RoleInfo> ctrRole = Startup.GetFromFac<tb_RoleInfoController<tb_RoleInfo>>();

        private bool _isSelectingNode = false;

        /// <summary>
        /// 树节点选择后事件
        /// 当选择行为菜单节点时，强制刷新按钮和字段权限列表
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">TreeView事件参数</param>
        private async void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 防止重复选择
            if (_isSelectingNode)
            {
                return;
            }

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

            _isSelectingNode = true;
            try
            {
                kryptonNavigator1.SelectedPage = kryptonPageBtn;
                CurrentMenuInfo = selectMenu;

                // 强制刷新按钮和字段权限数据（每次点击节点都重新加载）
                // 修改参数为 true，确保数据源完全刷新
                await Task.WhenAll(
                    InitLoadP4Button(selectMenu, true),
                    InitLoadP4Field(selectMenu, true)
                );

                //加载行级权限规则，优先默认的在前面
                InitLoadRowLevelAuthPolicy(selectMenu);

                #region 按钮和字段列表的中的值 有变化则保存可用
                CurrentRole.tb_P4Buttons.Where(c => c.RoleID == CurrentRole.RoleID).ForEach(x => UpdateSaveEnabled<tb_P4Button>(x));
                CurrentRole.tb_P4Fields.Where(c => c.RoleID == CurrentRole.RoleID).ForEach(x => UpdateSaveEnabled<tb_P4Field>(x));
                #endregion

                #region 设置全选菜单
                // 只在首次选择时设置右键菜单
                if (!_dataGridView1Configured && dataGridViewButton.Columns.Count > 0)
                {
                    foreach (DataGridViewColumn dc in dataGridViewButton.Columns)
                    {
                        if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
                        {
                            dc.HeaderCell.ContextMenuStrip = contextMenuStrip1;
                        }
                    }
                    _dataGridView1Configured = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("选择菜单节点时发生错误", ex);
            }
            finally
            {
                _isSelectingNode = false;
            }
        }

        /// <summary>
        /// 加载按钮权限（优化版本）
        /// </summary>
        /// <param name="selectMenu">选中菜单</param>
        /// <param name="InitLoadData">是否强制重新加载数据</param>
        private async Task InitLoadP4Button(tb_MenuInfo selectMenu, bool InitLoadData = false)
        {
            if (CurrentRole == null)
            {
                return;
            }

            // 使用缓存机制避免重复查询
            List<tb_P4Button> pblist = CurrentRole.tb_P4Buttons?
                .Where(c => c.MenuID == selectMenu.MenuID)
                .ToList() ?? new List<tb_P4Button>();

            // 只有在需要时才重新初始化数据
            if (pblist.Count == 0 || InitLoadData)
            {
                pblist = await InitBtnByRole(CurrentRole, selectMenu);

                // 更新缓存
                if (CurrentRole.tb_P4Buttons == null)
                {
                    CurrentRole.tb_P4Buttons = new List<tb_P4Button>();
                }

                // 移除旧数据并添加新数据（优化：避免多次遍历）
                var existingButtons = CurrentRole.tb_P4Buttons.Where(c => c.MenuID == selectMenu.MenuID).ToList();
                existingButtons.ForEach(b => CurrentRole.tb_P4Buttons.Remove(b));
                CurrentRole.tb_P4Buttons.AddRange(pblist);
            }

            // 优化UI更新 - 只在必要时更新数据源
            if (bindingSource1.DataSource == null || InitLoadData)
            {
                bindingSource1.DataSource = pblist.ToBindingSortCollection();
                dataGridViewButton.DataSource = ListDataSoure1;
            }
            else
            {
                // 如果只是更新数据，刷新数据源而不是重建
                bindingSource1.ResetBindings(false);
            }

            // 优化列设置 - 只设置一次
            if (!_dataGridView1Configured && dataGridViewButton.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in dataGridViewButton.Columns)
                {
                    if (col.ValueType?.Name == "Boolean")
                    {
                        col.ReadOnly = false;
                    }
                }
                // 标记已配置
                _dataGridView1Configured = true;
            }

            // 批量更新保存状态
            pblist.ForEach(x => UpdateSaveEnabled<tb_P4Button>(x));
        }


        /// <summary>
        /// 加载字段权限（优化版本）
        /// </summary>
        /// <param name="selectMenu">选中菜单</param>
        /// <param name="InitLoadData">是否强制重新加载数据</param>
        private async Task InitLoadP4Field(tb_MenuInfo selectMenu, bool InitLoadData = false)
        {
            if (CurrentRole == null)
            {
                return;
            }

            // 使用缓存机制避免重复查询
            List<tb_P4Field> pflist = CurrentRole.tb_P4Fields?
                .Where(c => c.MenuID == selectMenu.MenuID)
                .ToList() ?? new List<tb_P4Field>();

            // 只有在需要时才重新初始化数据
            if (pflist.Count == 0 || InitLoadData)
            {
                pflist = await InitFiledByRole(CurrentRole, selectMenu);

                // 更新缓存
                if (CurrentRole.tb_P4Fields == null)
                {
                    CurrentRole.tb_P4Fields = new List<tb_P4Field>();
                }

                // 移除旧数据并添加新数据（优化：避免多次遍历）
                var existingFields = CurrentRole.tb_P4Fields.Where(c => c.MenuID == selectMenu.MenuID).ToList();
                existingFields.ForEach(f => CurrentRole.tb_P4Fields.Remove(f));
                CurrentRole.tb_P4Fields.AddRange(pflist);
            }

            // 优化UI更新 - 只在必要时更新数据源
            if (bindingSource2.DataSource == null || InitLoadData)
            {
                bindingSource2.DataSource = pflist.ToBindingSortCollection();
                dataGridViewField.DataSource = ListDataSoure2;
            }
            else
            {
                // 如果只是更新数据，刷新数据源而不是重建
                bindingSource2.ResetBindings(false);
            }

            // 优化列设置 - 只设置一次
            if (!_dataGridView2Configured && dataGridViewField.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in dataGridViewField.Columns)
                {
                    if (col.ValueType?.Name == "Boolean")
                    {
                        col.ReadOnly = false;
                    }
                    // ischild只是标记是否为子表，不可编辑
                    if (col.DataPropertyName == "IsChild")
                    {
                        col.ReadOnly = true;
                    }
                }
                // 标记已配置
                _dataGridView2Configured = true;
            }

            // 批量更新保存状态
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
            foreach (DataGridViewColumn col in dataGridViewField.Columns)
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
                    dataGridViewButton.CurrentCell = dataGridViewButton.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
                    dataGridViewField.CurrentCell = dataGridViewField.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            if (dataGridViewButton.CurrentCell != null)
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
                if (dataGridViewButton.Focused)
                {
                    dg = dataGridViewButton;
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
                if (dataGridViewButton.Focused)
                {
                    dg = dataGridViewButton;
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

                // 确保编辑更改提交到数据源
                if (dg == dataGridViewButton)
                {
                    bindingSource1.EndEdit();
                }
                else if (dg == dataGridViewField)
                {
                    bindingSource2.EndEdit();
                }
                else if (dg == newSumDataGridViewRowAuthPolicy)
                {
                    bindingSourceRowAuthPolicy.EndEdit();
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
        /// 添加字段 - 包装方法，用于事件绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toolStripMenuItemInitField_Click(object sender, EventArgs e)
        {
            await toolStripMenuItemInitField_ClickAsync(sender, e);
        }

        /// <summary>
        /// 添加字段 - 实际执行逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task toolStripMenuItemInitField_ClickAsync(object sender, EventArgs e)
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


            await InitLoadP4Field(mInfo, true);

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
                if (dataGridViewButton.UseSelectedColumn)
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
                if (dataGridViewButton.UseSelectedColumn)
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

        // 在类开始处添加：
        private static IEntityCacheManager _cacheManager;
        private static IEntityCacheManager CacheManager => _cacheManager ?? (_cacheManager = Startup.GetFromFac<IEntityCacheManager>());
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
                    CacheManager.DeleteEntity(typeof(T).Name, PKValue.ToLong());

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
                foreach (DataGridViewColumn dc in dataGridViewButton.Columns)
                {
                    if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
                    {
                        dc.HeaderCell.ContextMenuStrip = contextMenuStrip1;
                    }
                }
            }
            else
            {
                foreach (DataGridViewColumn dc in dataGridViewField.Columns)
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

                        // 确保编辑更改提交到数据源
                        bindingSourceRowAuthPolicy.EndEdit();

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

        #region 资源清理

        /// <summary>
        /// 释放资源
        /// </summary>
        private void DisposeResources()
        {
            // 取消所有正在执行的初始化任务
            if (_loadingCancellationTokenSource != null)
            {
                _loadingCancellationTokenSource.Cancel();
                _loadingCancellationTokenSource.Dispose();
                _loadingCancellationTokenSource = null;
            }

            // 清理异步初始化任务缓存
            foreach (var task in _initializationTasks.Values)
            {
                try
                {
                    if (!task.IsCompleted && !task.IsCanceled)
                    {
                        // 不等待任务完成，只是记录日志
                        MainForm.Instance.logger?.LogWarning("窗体关闭时仍有未完成的按钮初始化任务");
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger?.LogWarning(ex, "清理异步初始化任务时发生错误");
                }
            }
            _initializationTasks.Clear();

            // 清理菜单树缓存
            _menuTreeCache = null;

            // 清理策略缓存
            if (_policyCache != null)
            {
                _policyCache.Clear();
            }
        }

        #endregion

    }


}

