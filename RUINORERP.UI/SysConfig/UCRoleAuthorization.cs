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

namespace RUINORERP.UI.SysConfig
{

    /// <summary>
    /// 比用户授权角色简单，那个是行记录存在性控制， 这里是默认每个角色都有。通过关系表中的字段来控制的
    /// </summary>
    [MenuAttrAssemblyInfo("角色授权", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCRoleAuthorization : UserControl
    {
        public UCRoleAuthorization()
        {
            InitializeComponent();
        }

        private void UCRoleAuthorization_Load(object sender, EventArgs e)
        {
            TreeView1.HideSelection = false;
            TreeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            DataBindingHelper.InitCmb<tb_RoleInfo>(k => k.RoleID, v => v.RoleName, cmRoleInfo.ComboBox, true);
            LoadTreeView();



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
        private async void LoadTreeView()
        {
            //if (CurrentRole.RoleID != -1)
            //{
            TreeView1.CheckBoxes = true;
            TreeView1.Nodes.Clear();
            //MenuInfoList = await ctrMenu.QueryByNavAsync();

            List<tb_ModuleDefinition> Modules = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_ModuleDefinition>()
            .Includes(m => m.tb_MenuInfos, a => a.tb_ButtonInfos)
            .Includes(m => m.tb_MenuInfos, a => a.tb_FieldInfos)
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
            //加载模块 模块和顶级菜单相同
            //  AddTopTreeNode(Modules, nd, MenuInfoList, 0);
            AddTreeNodeByMod(Modules, nd);
            TreeView1.Nodes.Add(nd);
            TreeView1.Nodes[0].Expand();
            //}
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
        private void AddTreeNodeByMod(List<tb_ModuleDefinition> Modules, ThreeStateTreeNode nd)
        {
            foreach (var item in Modules)
            {
                tb_MenuInfo _MenuInfo = item.tb_MenuInfos.FirstOrDefault(c => c.MenuName == item.ModuleName);
                if (_MenuInfo == null)
                {
                    return;
                }
                string NodeText = "";
                NodeText = item.ModuleName;
                //建立一个新节点
                ThreeStateTreeNode node = new ThreeStateTreeNode(NodeText);
                node.Tag = _MenuInfo;
                node.Checked = item.Visible;
                node.Name = item.ModuleID.ToString();
                //模块与菜单关系是用枚举的硬编码 在窗体的特性中定义标记的
                //初始化时已经将模块ID保存为外键到菜单中关联起来了

                AddTreeNode(node, item.tb_MenuInfos, _MenuInfo.MenuID);
                nd.Nodes.Add(node);
            }
        }


        private void AddTreeNode(ThreeStateTreeNode nd, List<tb_MenuInfo> list, long parent_id)
        {
            var selectList = list.Where(m => m.Parent_id == parent_id).OrderBy(c => c.Sort);
            foreach (var item in selectList)
            {
                if (!item.IsVisble)
                {
                    return;
                }
                if (!item.IsEnabled)
                {
                    return;
                }
                string NodeText = "";
                NodeText = item.CaptionCN;
                //建立一个新节点
                ThreeStateTreeNode node = new ThreeStateTreeNode(NodeText);
                node.Tag = item;
             
                if (item.MenuType == "行为菜单")
                {
                    node.ContextMenuStrip = contextMenuStrip4InitData;
                }
                //模块与菜单关系是用枚举的硬编码 在窗体的特性中定义标记的
                //初始化时已经将模块ID保存为外键到菜单中关联起来了
                AddTreeNode(node, list, item.MenuID);
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

            //TODO 要完善 如果每次执行不如在查询是直接传入条件
            var pmlis = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Menu>().Where(r => r.RoleID == CurrentRole.RoleID)
                    .Includes(t => t.tb_menuinfo)
                    .Includes(t => t.tb_roleinfo)
                    .ToListAsync();
            if (pmlis.Count == 0)
            {
                //默认插入一遍,并且重新加载一下
                InitMenuByRole(CurrentRole);
                LoadTreeView();
            }
            else
            {
                if (TreeView1.Nodes.Count == 0)
                {
                    LoadTreeView();
                }

                //循环去钩选
                UpdateP4MenuUI(TreeView1.Nodes[0].Nodes, pmlis);
            }

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

        /// <summary>
        /// 按角色初始化所有菜单,默认为否
        /// </summary>
        /// <param name="role"></param>
        public async void InitMenuByRole(tb_RoleInfo role)
        {
            var db = MainForm.Instance.AppContext.Db;
            var list = await ctrMenu.QueryByNavAsync();
            List<tb_P4Menu> plist = new List<tb_P4Menu>();
            foreach (var item in list)
            {
                tb_P4Menu pm = new tb_P4Menu();
                pm.RoleID = role.RoleID;
                pm.MenuID = item.MenuID;
                pm.ModuleID = item.ModuleID;
                pm.IsVisble = false;
                //ctrMenu.InitEntity(pm);
                BusinessHelper.Instance.InitEntity(pm);
                plist.Add(pm);
            }
            var ids = ctrPMenu.AddAsync(plist);
            // var ids = await db.Insertable(plist).ExecuteReturnSnowflakeIdListAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="menuInfo"></param>
        /// <param name="pblist">如果已经存在部分正常的权限和按钮关系，则要排除</param>
        /// <returns></returns>
        public async Task<bool> InitBtnByRole(tb_RoleInfo role, tb_MenuInfo menuInfo, List<tb_P4Button> pblist, List<tb_ButtonInfo> objlist)
        {
            //List<tb_ButtonInfo> objlist = new List<tb_ButtonInfo>();
            ////没有就添加 有就查询出来 
            ////List<tb_P4Button> pblist = new List<tb_P4Button>();
            //// objlist = await ctrBut.QueryByNavAsync();
            //objlist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_ButtonInfo>()
            //.Where(t => t.MenuID == menuInfo.MenuID)
            //.Includes(t => t.tb_menuinfo)
            //.ToListAsync();
            //如果默认的按钮为0的话，执行一次初始化？
            //if (objlist.Count == 0)
            //{
            List<MenuAttrAssemblyInfo> MenuAssemblylist = UIHelper.RegisterForm();
            MenuAttrAssemblyInfo mai = MenuAssemblylist.FirstOrDefault(e => e.ClassPath == menuInfo.ClassPath);
            if (mai != null)
            {
                InitModuleMenu imm = Startup.GetFromFac<InitModuleMenu>();
                imm.InitToolStripItem(mai, menuInfo);
            }
            //}
            List<tb_P4Button> Newpblist = new List<tb_P4Button>();
            foreach (var item in objlist)
            {
                //这里的意思是 按钮的数量要与角色对应按钮一致。可以不启用 但是重复的则可以不用加入
                //存在过就不添加了 没有的才添加
                if (pblist.Where(p => p.MenuID == menuInfo.MenuID && p.RoleID == CurrentRole.RoleID && p.ButtonInfo_ID == item.ButtonInfo_ID).FirstOrDefault() == null)
                {
                    tb_P4Button pb = new tb_P4Button();
                    pb.RoleID = CurrentRole.RoleID;
                    pb.ButtonInfo_ID = item.ButtonInfo_ID;
                    pb.MenuID = menuInfo.MenuID;
                    pb.IsVisble = false;
                    pb.IsEnabled = false;
                    BusinessHelper.Instance.InitEntity(pb);
                    Newpblist.Add(pb);
                }

            }
            if (Newpblist.Count == 0)
            {
                return false;
            }
            else
            {
                var ids = await ctrPBut.AddAsync(Newpblist);
            }
            return true;
            // var ids = await MainForm.Instance.AppContext.Db.Insertable(pblist).ExecuteReturnSnowflakeIdListAsync();
        }


        public async Task<bool> InitFiledByRole(tb_RoleInfo role, tb_MenuInfo menuInfo)
        {
            List<tb_FieldInfo> objlist = new List<tb_FieldInfo>();
            //没有就添加 有就查询出来 
            List<tb_P4Field> pblist = new List<tb_P4Field>();
            // objlist = await ctrField.QueryByNavAsync();
            objlist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_FieldInfo>()
                    .Where(t => t.MenuID == menuInfo.MenuID)
                    .Includes(t => t.tb_menuinfo)
                    .ToListAsync();

            if (objlist.Count == 0)
            {
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
            }

            foreach (var item in objlist)
            {
                tb_P4Field pb = new tb_P4Field();
                pb.RoleID = CurrentRole.RoleID;
                pb.FieldInfo_ID = item.FieldInfo_ID;
                pb.MenuID = menuInfo.MenuID;
                pb.IsVisble = false;
                BusinessHelper.Instance.InitEntity(pb);
                bool isexist = ctrPField.IsExist(e => e.RoleID == pb.RoleID
                && e.FieldInfo_ID == pb.FieldInfo_ID
                && e.MenuID == menuInfo.MenuID
               );
                if (!isexist)
                {
                    pblist.Add(pb);
                }

            }
            if (pblist.Count == 0)
            {
                return false;
            }
            var ids = await ctrPField.AddAsync(pblist);
            // var ids = await MainForm.Instance.AppContext.Db.Insertable(pblist).ExecuteReturnSnowflakeIdListAsync();
            return true;
        }

        #endregion


        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;
        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList2;


        tb_RoleInfoController<tb_RoleInfo> ctrRole = Startup.GetFromFac<tb_RoleInfoController<tb_RoleInfo>>();


        private async void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
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

            //没有就添加 有就查询出来 
            List<tb_P4Button> pblist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Button>()
                .Where(t => t.RoleID == CurrentRole.RoleID && t.MenuID == selectMenu.MenuID)
                .Includes(t => t.tb_buttoninfo)
                .ToListAsync();

            List<tb_ButtonInfo> btnInfolist = new List<tb_ButtonInfo>();
            //没有就添加 有就查询出来 
            btnInfolist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_ButtonInfo>()
            .Where(t => t.MenuID == selectMenu.MenuID)
            .Includes(t => t.tb_menuinfo)
            .ToListAsync();

            //当前角色下如果可操作按钮为0时，或当前角色下添加的按钮数量小于当前菜单下的所有按钮时
            if (pblist.Count == 0 || pblist.Count < btnInfolist.Count)
            {
                await InitBtnByRole(CurrentRole, selectMenu, pblist, btnInfolist);
            }
            //&& t.tb_ButtonInfo != null && t.tb_ButtonInfo.MenuID == selectMenu.MenuID
            bindingSource1.DataSource = pblist.ToBindingSortCollection();
            dataGridView1.DataSource = ListDataSoure1;


            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.ValueType.Name == "Boolean")
                {
                    col.ReadOnly = false;
                }
            }

            //没有就添加 有就查询出来 
            List<tb_P4Field> pflist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Field>().Where(t => t.RoleID == CurrentRole.RoleID && t.MenuID == selectMenu.MenuID)
            .Includes(t => t.tb_fieldinfo)
            .ToListAsync();
            if (pflist.Count == 0)
            {
                await InitFiledByRole(CurrentRole, selectMenu);
            }
            bindingSource2.DataSource = pflist.ToBindingSortCollection(); ;
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

            #region 按钮和字段列表的中的值 有变化则保存可用
            pblist.ForEach(x => UpdateSaveEnabled(x));
            pflist.ForEach(x => UpdateSaveEnabled(x));
            #endregion


            #region  自定义组件开始

            // DataGridViewColumnSelector cs = new DataGridViewColumnSelector(dataGridView1);
            // cs.MaxHeight = 200;
            // cs.Width = 110;

            // dataGridView1.SetColToCheckBox<tb_P4Button>(c => c.IsVisble);
            // dataGridView2.SetColToCheckBoxNew<tb_P4Field>(c => c.IsVisble);


            #endregion


            #region 设置左右菜单

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
            //找到点击的列保存到tag中。再单项点时控制
            NewSumDataGridView dg = contextMenuStrip1.SourceControl as NewSumDataGridView;
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
                NewSumDataGridView dg = contextMenuStrip1.SourceControl as NewSumDataGridView;
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

        private async void toolStripMenuItemInitBtn_Click(object sender, EventArgs e)
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
            List<tb_ButtonInfo> objlist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_ButtonInfo>()
            .Where(t => t.MenuID == mInfo.MenuID)
            .Includes(t => t.tb_menuinfo)
            .ToListAsync();

            //没有就添加 有就查询出来 
            List<tb_P4Button> pblist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_P4Button>()
                .Where(t => t.RoleID == CurrentRole.RoleID && t.MenuID == mInfo.MenuID)
                .Includes(t => t.tb_buttoninfo)
                .ToListAsync();


            await InitBtnByRole(CurrentRole, mInfo, pblist, objlist);

            //if (objlist.Count == 0)
            //{
            //    List<MenuAttrAssemblyInfo> MenuAssemblylist = UIHelper.RegisterForm();
            //    MenuAttrAssemblyInfo mai = MenuAssemblylist.FirstOrDefault(e => e.ClassPath == mInfo.ClassPath);
            //    if (mai != null)
            //    {
            //        InitModuleMenu imm = Startup.GetFromFac<InitModuleMenu>();
            //        imm.InitToolStripItem(mai, mInfo);
            //    }
            //}


        }

        private async void toolStripMenuItemInitField_Click(object sender, EventArgs e)
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


            await InitFiledByRole(CurrentRole, mInfo);
        }

        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            toolStripButtonSave.Enabled = true;
        }
    }
}
