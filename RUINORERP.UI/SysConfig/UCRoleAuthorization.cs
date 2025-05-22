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
using TransInstruction;
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.Business.CommService;
using System.Windows.Documents;
using FastReport.Forms;
using System.Diagnostics;
using OpenTK.Input;
using Castle.Components.DictionaryAdapter.Xml;


namespace RUINORERP.UI.SysConfig
{

    /// <summary>
    /// 比用户授权角色简单，那个是行记录存在性控制， 这里是默认每个角色都有。通过关系表中的字段来控制的
    /// </summary>
    [MenuAttrAssemblyInfo("角色授权", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCRoleAuthorization : UserControl, IContextMenuInfoAuth
    {
        public UCRoleAuthorization()
        {
            InitializeComponent();
            // 在窗体构造函数中
            typeof(TreeView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, TreeView1, new object[] { true });
            BuildContextMenuController();
        }


        #region 检测是否重复
        public List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_检测是否重复);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【检测是否重复】", true, false, "NewSumDataGridView_检测是否重复"));
            return list;
        }
        public void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_检测是否重复);

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
        }

        private void NewSumDataGridView_检测是否重复(object sender, EventArgs e)
        {


            tb_MenuInfo selectMenu = TreeView1.SelectedNode.Tag as tb_MenuInfo;
            if (kryptonNavigator1.SelectedPage.Name == "kryptonPage1")
            {
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

            if (kryptonNavigator1.SelectedPage.Name == "kryptonPage3")
            {
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

        private void KryptonNavigator1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: 实现导航器索引改变时的逻辑
            NewSumDataGridView dg = kryptonNavigator1.SelectedPage.Controls[0] as NewSumDataGridView;
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

            kryptonNavigator1.SelectedPageChanged += KryptonNavigator1_SelectedIndexChanged;

            TreeView1.HideSelection = false;
            TreeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;

            List<tb_RoleInfo> roleInfos = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_RoleInfo>()
           .Includes(m => m.tb_P4Modules)
           .AsNavQueryable()
           .Includes(m => m.tb_P4Menus, t => t.tb_menuinfo, b => b.tb_P4Buttons)
           .Includes(m => m.tb_P4Menus, t => t.tb_menuinfo, b => b.tb_P4Fields)
           .Includes(m => m.tb_P4Menus, t => t.tb_menuinfo, b => b.tb_P4Menus)
           //.Includes(m => m.tb_P4Menus, t => t.tb_roleinfo, b => b.tb_P4Buttons)
           //.Includes(m => m.tb_P4Menus, t => t.tb_roleinfo, b => b.tb_P4Fields)
           //.Includes(m => m.tb_P4Menus, t => t.tb_roleinfo, b => b.tb_P4Menus)
           //.Includes(m => m.tb_P4Buttons)
           //.Includes(m => m.tb_P4Fields)
           //.Includes(m => m.tb_P4Modules)
           .ToListAsync();

            DataBindingHelper.InitCmb<tb_RoleInfo>(k => k.RoleID, v => v.RoleName, cmRoleInfo.ComboBox, true, roleInfos);
            await LoadTreeView();
            InitListData();
            dataGridView1.NeedSaveColumnsXml = true;
            dataGridView2.NeedSaveColumnsXml = true;
            dataGridView1.Use是否使用内置右键功能 = false;
            dataGridView2.Use是否使用内置右键功能 = false;
            // dataGridView1.ContextMenuStrip = contextMenuStrip1;
            // dataGridView2.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDown);
            dataGridView2.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView2_CellMouseDown);

        }





        tb_ModuleDefinitionController<tb_ModuleDefinition> ctrMod = Startup.GetFromFac<tb_ModuleDefinitionController<tb_ModuleDefinition>>();
        tb_MenuInfoController<tb_MenuInfo> ctrMenu = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
        tb_ButtonInfoController<tb_ButtonInfo> ctrBut = Startup.GetFromFac<tb_ButtonInfoController<tb_ButtonInfo>>();
        tb_FieldInfoController<tb_FieldInfo> ctrField = Startup.GetFromFac<tb_FieldInfoController<tb_FieldInfo>>();

        tb_P4ModuleController<tb_P4Module> ctrPModule = Startup.GetFromFac<tb_P4ModuleController<tb_P4Module>>();
        tb_P4MenuController<tb_P4Menu> ctrPMenu = Startup.GetFromFac<tb_P4MenuController<tb_P4Menu>>();
        tb_P4ButtonController<tb_P4Button> ctrPBut = Startup.GetFromFac<tb_P4ButtonController<tb_P4Button>>();
        tb_P4FieldController<tb_P4Field> ctrPField = Startup.GetFromFac<tb_P4FieldController<tb_P4Field>>();




        /// <summary>
        /// 将模块 菜单 显示为树
        /// </summary>
        private async Task<bool> LoadTreeView(bool Seleted = false)
        {
            //if (CurrentRole.RoleID != -1)
            //{
            TreeView1.CheckBoxes = true;
            TreeView1.Nodes.Clear();
            //MenuInfoList = await ctrMenu.QueryByNavAsync();

            List<tb_ModuleDefinition> Modules = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_ModuleDefinition>()
            .Includes(m => m.tb_MenuInfos, a => a.tb_ButtonInfos)
            .Includes(m => m.tb_MenuInfos, a => a.tb_FieldInfos)
            .Includes(m => m.tb_MenuInfos, a => a.tb_P4Buttons, b => b.tb_buttoninfo)
            .Includes(m => m.tb_MenuInfos, a => a.tb_P4Fields, b => b.tb_fieldinfo)
            .Includes(m => m.tb_MenuInfos, a => a.tb_P4Menus)
            .ToListAsync();

            //检测CRM如果没有购买则不会显示
            if (!MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM))
            {
                Modules = Modules.Where(m => m.ModuleName != ModuleMenuDefine.模块定义.客户关系.ToString()).ToList();
            }

            //MenuInfoList = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_MenuInfo>()
            //.ToListAsync();

            ThreeStateTreeNode nd = new ThreeStateTreeNode();
            nd.Text = "系统根节点";
            nd.Checked = Seleted;
            //加载模块 模块和顶级菜单相同
            //  AddTopTreeNode(Modules, nd, MenuInfoList, 0);
            AddTreeNodeByMod(Modules, nd);
            TreeView1.Nodes.Add(nd);
            TreeView1.Nodes[0].Expand();
            return true;
        }


        private void tVtypeList_DrawNode(object sender, DrawTreeNodeEventArgs e)
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
            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList2 = UIHelper.GetFieldNameColList(typeof(tb_P4Field));
            dataGridView2.XmlFileName = "UCRoleAuthorization2";
            dataGridView2.FieldNameList = FieldNameList2;
            dataGridView1.DataSource = null;
            ListDataSoure1 = bindingSource1;
            //绑定导航
            dataGridView1.DataSource = ListDataSoure1.DataSource;
            dataGridView2.DataSource = null;
            ListDataSoure2 = bindingSource2;
            //绑定导航
            dataGridView2.DataSource = ListDataSoure2.DataSource;

        }

        #endregion
        tb_RoleInfo CurrentRole;

        private async void btnSave_Click(object sender, EventArgs e)
        {
            #region
            if (TreeView1.SelectedNode == null)
            {
                return;
            }

            if (CurrentRole == null)
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

            #endregion
            if (CurrentRole != null)
            {
                dataGridView1.EndEdit();
                dataGridView2.EndEdit();

                //@@@@@@！分层级处理！@@@@@@@@

                //准备要更新的集合
                List<tb_P4Module> tb_P4Modules = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Module>()
                    .Where(p => p.RoleID == CurrentRole.RoleID)
                    .Includes(m => m.tb_moduledefinition)
                    .ToListAsync();
                //保存顶级菜单勾选情况  是用更新 第一个是根节点
                UpdateP4Module(TreeView1.Nodes[0].Nodes, tb_P4Modules);


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

                //更新到对应的菜单下的集合中不然还是用前面的缓存


                /*
                //准备要更新的集合
                List<tb_P4Menu> tb_P4Menus = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Menu>().Where(p => p.RoleID == CurrentRole.RoleID).ToListAsync();

                //保存菜单勾选情况  是用更新
                UpdateP4Menu(TreeView1.Nodes, tb_P4Menus);

                //更新到关系表中tb_p4menu，因为上面有些只是更新了字段。要保存到db
                bool rs = await MainForm.Instance.AppContext.Db.CopyNew().Updateable(tb_P4Menus).ExecuteCommandHasChangeAsync();
                */
                toolStripButtonSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("请选择要操作的角色！");
            }
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
                    //tb_MenuInfo menuInfo = tn.Tag as tb_MenuInfo;
                    var pm = tb_P4Menus.Where(e => e.MenuID == menuInfo.MenuID).FirstOrDefault();
                    if (pm != null)
                    {
                        //已经存在菜单角色关系的只更新选择状态
                        pm.IsVisble = tn.Checked;
                        BusinessHelper.Instance.EditEntity(pm);
                    }
                    else
                    {
                        //新的菜单则需要新增 这种情况少 单个操作也可以
                        tb_P4Menu pmNew = new tb_P4Menu();
                        pmNew.RoleID = CurrentRole.RoleID;
                        pmNew.MenuID = menuInfo.MenuID;
                        pmNew.ModuleID = menuInfo.ModuleID;
                        pmNew.IsVisble = tn.Checked;
                        //pmNew.IsEnabled = false;
                        BusinessHelper.Instance.InitEntity(pmNew);
                        var id = ctrPMenu.AddAsync(pmNew);
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
        private async void UpdateP4Module(TreeNodeCollection Nodes, List<tb_P4Module> tb_P4Modules)
        {
            //保存菜单勾选情况  是用更新 顶级 只有一级 对应模块
            //模块等于顶级菜单这里要同时保存一下菜单关系

            //准备要更新的集合
            List<tb_P4Menu> tb_P4Menus = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Menu>()
                .Where(p => p.RoleID == CurrentRole.RoleID)
                .Includes(m => m.tb_menuinfo)
                .ToListAsync();
            foreach (ThreeStateTreeNode tn in Nodes)
            {
                if (tn.Tag is tb_MenuInfo menuInfo)
                {

                    var modMenu = tb_P4Menus.FirstOrDefault(w => w.tb_menuinfo.MenuName == menuInfo.MenuName && w.tb_menuinfo.Parent_id == 0);
                    //理论上不会为空
                    if (modMenu == null)
                    {
                        MainForm.Instance.uclog.AddLog("模块菜单不能为空，请联系管理员。");
                        continue;
                    }
                    else
                    {
                        modMenu.IsVisble = tn.Checked;
                    }
                    //tb_MenuInfo menuInfo = tn.Tag as tb_MenuInfo;
                    var pm = tb_P4Modules.Where(e => e.tb_moduledefinition.ModuleName == menuInfo.MenuName).FirstOrDefault();
                    if (pm != null)
                    {

                        //已经存在菜单角色关系的只更新选择状态
                        if (pm.IsVisble != tn.Checked)
                        {
                            pm.IsVisble = tn.Checked;
                            BusinessHelper.Instance.EditEntity(pm);
                            //pm.IsEnabled=tn.enab
                            await ctrPModule.UpdateAsync(pm);
                        }

                    }
                    else
                    {
                        //新的则需要新增 这种情况少 单个操作也可以
                        tb_P4Module pmNew = new tb_P4Module();
                        pmNew.RoleID = CurrentRole.RoleID;
                        pmNew.ModuleID = long.Parse(tn.Name);
                        pmNew.IsVisble = tn.Checked;
                        pmNew.IsEnabled = tn.Checked;
                        BusinessHelper.Instance.InitEntity(pmNew);
                        var id = ctrPModule.AddAsync(pmNew);
                    }


                    //处理下级
                    //如果选中才添加下级关系数据
                    if (tn.Checked)
                    {
                        //保存菜单勾选情况  是用更新
                        UpdateP4Menu(tn.Nodes, tb_P4Menus);
                        //更新到关系表中tb_p4menu，因为上面有些只是更新了字段。要保存到db
                        bool rs = await MainForm.Instance.AppContext.Db.CopyNew().Updateable(tb_P4Menus).ExecuteCommandHasChangeAsync();

                    }
                }



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
            using (var dialog = new ProgressDialog("加载权限清单", allowCancel: true))
            {
                dialog.RunAsync(async (progress, ct) =>
                {
                    try
                    {
                        progress.Report((0, "开始加载权限清单..."));

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
                                item.Click += (s, e) =>
                                {
                                    #region 复制选中角色的权限
                                    if (CurrentRole.tb_P4Menus == null)
                                    {
                                        CurrentRole.tb_P4Menus = new List<tb_P4Menu>();
                                    }
                                    //TODO 要完善 如果每次执行不如在查询是直接传入条件
                                    if (CurrentRole.tb_P4Menus.Count == 0)
                                    {
                                        //默认插入一遍,并且重新加载一下
                                        InitMenuByRole(x);
                                    }
                                  
                                    #endregion
                                };
                                toolStripCopyRoleConfig.DropDownItems.Add(item);
                            }
                        });

                        progress.Report((30, "加载权限清单..."));
                        //TODO 要完善 如果每次执行不如在查询是直接传入条件
                        var pmlis = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Menu>().Where(r => r.RoleID == CurrentRole.RoleID)
                                         .Includes(t => t.tb_menuinfo, b => b.tb_P4Buttons)
                                            .Includes(t => t.tb_menuinfo, b => b.tb_P4Fields)
                                            .Includes(t => t.tb_menuinfo, b => b.tb_P4Menus)
                                            .Includes(t => t.tb_roleinfo, b => b.tb_P4Buttons)
                                            .Includes(t => t.tb_roleinfo, b => b.tb_P4Fields)
                                            .Includes(t => t.tb_roleinfo, b => b.tb_P4Menus)
                                .ToListAsync();
                        if (pmlis.Count == 0)
                        {
                            //默认插入一遍,并且重新加载一下
                            InitMenuByRole(CurrentRole);
                        }
                        // 调用其他耗时操作...

                        progress.Report((100, "加载权限清单结束"));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                });

                var result = dialog.ShowDialog(this);
                switch (result)
                {
                    case DialogResult.OK:
                        if (TreeView1.Nodes.Count <= 1)
                        {
                            await LoadTreeView();
                        }
                        //循环去钩选
                        UpdateP4MenuUI(TreeView1.Nodes[0].Nodes, CurrentRole.tb_P4Menus);
                        break;
                    case DialogResult.Cancel:
                        MessageBox.Show("用户取消", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case DialogResult.Abort:
                        MessageBox.Show("操作异常", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
         


        /*
        // 使用等待对话框
        await BusyDialog.ShowAsync(async () =>
        {
            // await Task.Delay(2000); // 你的异步操作

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
                    item.Click += async (s, e) =>
                    {
                        #region 复制选中角色的权限
                        if (CurrentRole.tb_P4Menus == null)
                        {
                            CurrentRole.tb_P4Menus = new List<tb_P4Menu>();
                        }
                        //TODO 要完善 如果每次执行不如在查询是直接传入条件
                        if (CurrentRole.tb_P4Menus.Count == 0)
                        {
                            //默认插入一遍,并且重新加载一下
                            InitMenuByRole(x);
                            await LoadTreeView();
                        }
                        else
                        {
                            if (TreeView1.Nodes.Count == 0)
                            {
                                await LoadTreeView();
                            }
                            //循环去钩选
                            UpdateP4MenuUI(TreeView1.Nodes[0].Nodes, CurrentRole.tb_P4Menus);
                        }
                        #endregion
                    };
                    toolStripCopyRoleConfig.DropDownItems.Add(item);
                }
            });


            //TODO 要完善 如果每次执行不如在查询是直接传入条件
            var pmlis = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Menu>().Where(r => r.RoleID == CurrentRole.RoleID)
                             .Includes(t => t.tb_menuinfo, b => b.tb_P4Buttons)
                                .Includes(t => t.tb_menuinfo, b => b.tb_P4Fields)
                                .Includes(t => t.tb_menuinfo, b => b.tb_P4Menus)
                                .Includes(t => t.tb_roleinfo, b => b.tb_P4Buttons)
                                .Includes(t => t.tb_roleinfo, b => b.tb_P4Fields)
                                .Includes(t => t.tb_roleinfo, b => b.tb_P4Menus)
                    .ToListAsync();
            if (pmlis.Count == 0)
            {
                //默认插入一遍,并且重新加载一下
                InitMenuByRole(CurrentRole);
                await LoadTreeView();
            }
            else
            {
                if (TreeView1.Nodes.Count <= 1)
                {
                    await LoadTreeView();
                }

                //循环去钩选
                UpdateP4MenuUI(TreeView1.Nodes[0].Nodes, pmlis);
            }

            // 调用其他耗时操作...


        }, "加载数据...");
        */

    }





    private void UpdateP4MenuUI(TreeNodeCollection Nodes, List<tb_P4Menu> tb_P4Menus)
    {
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
    public async void InitMenuByRole(tb_RoleInfo role, bool Selected = false)
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
    private async void CompleteMenuInitialization()
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
    public async Task<List<tb_P4Button>> InitBtnByRole(tb_RoleInfo role, tb_MenuInfo menuInfo, bool Seleted = false)
    {
        MenuAttrAssemblyInfo mai = MenuAssemblylist.FirstOrDefault(e => e.ClassPath == menuInfo.ClassPath);
        if (mai != null)
        {
            await Task.Run(() =>
            {
                MainForm.Instance.Invoke(new Action(() =>
                {
                    InitModuleMenu imm = Startup.GetFromFac<InitModuleMenu>();
                    imm.InitToolStripItem(mai, menuInfo);

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
    /// 根据权限初始化字段
    /// </summary>
    /// <param name="role"></param>
    /// <param name="menuInfo"></param>
    /// <param name="Seleted">是否选中</param>
    /// <returns></returns>
    public async Task<List<tb_P4Field>> InitFiledByRole(tb_RoleInfo role, tb_MenuInfo menuInfo, bool Seleted = false)
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
            imm.InitFieldInoMainAndSub(mai, menuInfo, false, "");
            //尝试找子表类型
            string childType = typeNames.FirstOrDefault(s => s.Contains(mai.Name + "Detail"));
            if (!string.IsNullOrEmpty(childType))
            {
                Type cType = ModelTypes.FirstOrDefault(t => t.FullName == mai.FullName + "Detail");
                if (cType != null)
                {
                    imm.InitFieldInoMainAndSub(cType, menuInfo, true, childType);
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

    #endregion


    ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;
    ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList2;


    tb_RoleInfoController<tb_RoleInfo> ctrRole = Startup.GetFromFac<tb_RoleInfoController<tb_RoleInfo>>();


    private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
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
        kryptonNavigator1.SelectedPage = kryptonPage1;

        InitLoadP4Button(selectMenu);

        InitLoadP4Field(selectMenu);

        #region 按钮和字段列表的中的值 有变化则保存可用
        selectMenu.tb_P4Buttons.Where(c => c.RoleID == CurrentRole.RoleID).ForEach(x => UpdateSaveEnabled(x));
        selectMenu.tb_P4Fields.Where(c => c.RoleID == CurrentRole.RoleID).ForEach(x => UpdateSaveEnabled(x));
        #endregion


        #region  自定义组件开始

        // DataGridViewColumnSelector cs = new DataGridViewColumnSelector(dataGridView1);
        // cs.MaxHeight = 200;
        // cs.Width = 110;

        // dataGridView1.SetColToCheckBox<tb_P4Button>(c => c.IsVisble);
        // dataGridView2.SetColToCheckBoxNew<tb_P4Field>(c => c.IsVisble);


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

        foreach (DataGridViewColumn dc in dataGridView2.Columns)
        {
            if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
            {
                dc.HeaderCell.ContextMenuStrip = contextMenuStrip1;
            }
        }

        /*
         //设置DataGridView的右键菜单
    this.dgv_Users.ContextMenuStrip = cmsDgv;
    //设置列的右键菜单
    this.dgv_Users.Columns[1].ContextMenuStrip = cmsColumn;
    //设置列头的右键菜单
    this.dgv_Users.Columns[1].HeaderCell.ContextMenuStrip = cmsHeaderCell;
    //设置行的右键菜单
    this.dgv_Users.Rows[2].ContextMenuStrip = cmsRow;
    //设置单元格的右键菜单
    this.dgv_Users[1, 2].ContextMenuStrip = cmsCell;
         */

        #endregion
    }
    private async void InitLoadP4Button(tb_MenuInfo selectMenu, bool InitLoadData = true)
    {
        if (selectMenu.tb_P4Buttons == null)
        {
            selectMenu.tb_P4Buttons = new List<tb_P4Button>();
        }
        List<tb_P4Button> pblist = selectMenu.tb_P4Buttons.Where(c => c.RoleID == CurrentRole.RoleID).ToList();
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
    }

    private async void InitLoadP4Field(tb_MenuInfo selectMenu, bool InitLoadData = true)
    {
        if (selectMenu.tb_P4Fields == null)
        {
            selectMenu.tb_P4Fields = new List<tb_P4Field>();
        }
        List<tb_P4Field> pflist = selectMenu.tb_P4Fields.Where(c => c.RoleID == CurrentRole.RoleID).ToList();
        if (pflist.Count == 0 || InitLoadData)
        {
            pflist = await InitFiledByRole(CurrentRole, selectMenu);
        }
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
    }


    private void UpdateSaveEnabled(BaseEntity entity)
    {
        entity.PropertyChanged += (sender, s2) =>
        {
            //如果客户有变化，带出对应有业务员
            //if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.CustomerVendor_ID))
            //{

            //}

            toolStripButtonSave.Enabled = true;

        };
    }


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
        /*
        List<tb_FieldInfo> objlist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_FieldInfo>()
                .Where(t => t.MenuID == mInfo.MenuID)
                .Includes(t => t.tb_menuinfo)
                .ToListAsync();

        if (objlist.Count == 0 ||
            (objlist.Count > 0 && MessageBox.Show("您确定添加字段吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes))
        {
            Assembly dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            Type[] ModelTypes = dalAssemble.GetExportedTypes();
            List<string> typeNames = ModelTypes.Select(m => m.Name).ToList();
            Type mai = ModelTypes.FirstOrDefault(ee => ee.Name == mInfo.EntityName);
            if (mai != null)
            {
                InitModuleMenu imm = Startup.GetFromFac<InitModuleMenu>();
                //imm.InitFieldIno(mInfo, mai);
                imm.InitFieldInoMainAndSub(mai, mInfo, false, "");
                //尝试找子表类型
                string childType = typeNames.FirstOrDefault(s => s.Contains(mai.Name + "Detail"));
                if (!string.IsNullOrEmpty(childType))
                {
                    Type cType = ModelTypes.FirstOrDefault(t => t.FullName == mai.FullName + "Detail");
                    if (cType != null)
                    {
                        imm.InitFieldInoMainAndSub(cType, mInfo, true, childType);
                    }
                }
            }
        }

        if (mInfo.tb_P4Fields == null)
        {
            mInfo.tb_P4Fields = new List<tb_P4Field>();
        }
        List<tb_P4Field> pflist = mInfo.tb_P4Fields.Where(c => c.RoleID == CurrentRole.RoleID).ToList();
        pflist = await InitFiledByRole(CurrentRole, mInfo);
        */

        InitLoadP4Field(mInfo, true);

    }

    private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
    {
        toolStripButtonSave.Enabled = true;
    }


    private async void toolsbtnFullAuthorization_ClickAsync(object sender, EventArgs e)
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
        bool selected, IProgress<(int Percentage, string Message)> progress,
        CancellationToken ct)
    {
        var newMenus = new List<tb_P4Menu>();
        var list = await ctrMenu.QueryByNavAsync();

        for (int i = 0; i < list.Count; i++)
        {
            ct.ThrowIfCancellationRequested();
            var item = list[i];
            // 计算进度百分比
            var percentage = (int)((i + 1) * 100 / list.Count);
            progress.Report((percentage, $"处理菜单：{item.MenuName} ({i + 1}/{list.Count})"));

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

        if (kryptonNavigator1.SelectedPage.Name == "kryptonPage1")
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

        if (kryptonNavigator1.SelectedPage.Name == "kryptonPage3")
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
                    AuditLogHelper.Instance.CreateAuditLog("删除", "角色授权时");
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
                OriginalData beatDataDel = ClientDataBuilder.BaseInfoChangeBuilder(typeof(T).Name);
                MainForm.Instance.ecs.AddSendData(beatDataDel);

                //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                //只处理需要缓存的表
                if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(T).Name, out pair))
                {
                    //如果有更新变动就上传到服务器再分发到所有客户端
                    OriginalData odforCache = ActionForClient.删除缓存<T>(PKColName, PKValue.ToLong());
                    byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                    MainForm.Instance.ecs.client.Send(buffer);
                }
            }

        }

        #endregion
        return ids;
    }

    private void toolStripCopyRoleConfig_Click(object sender, EventArgs e)
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
}
}
