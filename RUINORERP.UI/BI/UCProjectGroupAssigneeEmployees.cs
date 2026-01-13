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
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using SHControls.DataGrid;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 多对多的关系
    /// </summary>
    [MenuAttrAssemblyInfo("项目分配组员", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCProjectGroupAssigneeEmployees : UserControl
    {
        public UCProjectGroupAssigneeEmployees()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                DisplayTextResolver = new GridViewDisplayTextResolver(typeof(tb_ProjectGroupEmployees));
                DisplayTextResolver.Initialize(dataGridView1);
                this.dataGridView1.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            }
        }


        public GridViewDisplayTextResolver DisplayTextResolver;
        tb_ProjectGroupController<tb_ProjectGroup> ctrGroup = Startup.GetFromFac<tb_ProjectGroupController<tb_ProjectGroup>>();
        tb_EmployeeController<tb_Employee> ctrEmp = Startup.GetFromFac<tb_EmployeeController<tb_Employee>>();
        tb_ProjectGroupEmployeesController<tb_ProjectGroupEmployees> ctr = Startup.GetFromFac<tb_ProjectGroupEmployeesController<tb_ProjectGroupEmployees>>();
        private async Task LoadGroups()
        {
            TreeView1.Nodes.Clear();
            var lambda = Expressionable.Create<tb_ProjectGroup>()
                             .And(t => t.Is_enabled == true)
                            .ToExpression();//注意 这一句 不能少

            var list = await ctrGroup.QueryAsync(lambda);
            foreach (var item in list)
            {
                if (item != null)
                {
                    //如果是禁用或者删除状态，则不显示
                    if (item.Is_enabled == false)
                    {
                        continue;
                    }
                }
                string NodeText = "";
                NodeText = item.ProjectGroupName;
                //if (item.tb != null)
                //{
                //    NodeText += "【" + item.tb_ProjectGroup.Employee_Name + "】";
                //}

                //建立一个新节点
                TreeNode node = new TreeNode(NodeText);
                node.Tag = item;
                //付完值，添加到传入节点的子节点集合中
                TreeView1.Nodes.Add(node);
            }
        }

        #region 

        /// <summary>
        /// ！！！whereexp 要必需 users.Where(m => m.ParentId == parentId);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chilNode"></param>
        /// <param name="expTextField"></param>
        /// <param name="expValueField"></param>
        /// <param name="wxp"></param>
        /// <param name="list"></param>
        public void TreeViewBind<T>(TreeNode chilNode, Expression<Func<T, string>> expTextField, Expression<Func<T, object>> expValueField, Expression<Func<T, bool>> wxp, List<T> list)
        {
            var queryable = list.AsQueryable();
            MemberInfo infoName = expTextField.GetMemberInfo();
            string textField = infoName.Name;
            MemberInfo infoID = expValueField.GetMemberInfo();
            string valueField = infoID.Name;

            //数据源，GetTreeData（）是我本身打的一个获取数据源方法
            //var data = users.Where(m => m.ParentId == parentId);
            //  list.GroupBy<T>(wxp);
            var data = queryable.Where(wxp);
            //  var data = list.Where(wxp);
            foreach (var item in queryable)
            {
                //建立一个新节点
                TreeNode node = new TreeNode(item.GetPropertyValue(textField).ToString());

                node.Tag = item.GetPropertyValue(valueField);
                //付完值，添加到传入节点的子节点集合中
                chilNode.Nodes.Add(node);
                //从新调用递归绑定
                //TreeViewBind(node, int.Parse(node.Tag.ToString()), expTextField, expValueField, wxp, list);
                TreeViewBind(node, expTextField, expValueField, wxp, list);
            }
        }



        #endregion

        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public HashSet<string> InvisibleCols { get; set; } = new HashSet<string>();
        private void UCUserAuthorization_Load(object sender, EventArgs e)
        {
            this.dataGridView1.SetUseCustomColumnDisplay(true);
            dataGridView1.NeedSaveColumnsXml = true;
            TreeView1.HideSelection = false;
            // FieldNameList = UIHelper.GetFieldNameColList(typeof(SelectDto), typeof(tb_ProjectGroup));
            dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_ProjectGroupEmployees));
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.XmlFileName = nameof(tb_ProjectGroupEmployees);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
            }
            dataGridView1.BizInvisibleCols = InvisibleCols;
            dataGridView1.UseCustomColumnDisplay = true;
            InitListData();
            bindingSourceList.DataSource = new List<tb_ProjectGroupEmployees>();
            this.dataGridView1.DataSource = null;
            dataGridView1.DataSource = bindingSourceList;
            dataGridView1.BindColumnStyle();
            try
            {
                LoadGroups();
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse && sender is TreeView tv)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(tv.Nodes, e.Node);
                }
            }

            //TreeViewSingleSelectedAndChecked(TreeView1, e);

            e.Node.Checked = true;
            node_AfterCheck(sender, e);

            var lambda = Expressionable.Create<tb_Employee>()
                 .And(t => t.Is_enabled == true)
                .ToExpression();//注意 这一句 不能少

            List<tb_Employee> empList = await ctrEmp.QueryAsync(lambda);

            List<tb_ProjectGroupEmployees> listUR = await ctr.QueryByNavAsync();
            tb_ProjectGroup group = TreeView1.SelectedNode.Tag as tb_ProjectGroup;
            group.tb_ProjectGroupEmployeeses = new List<tb_ProjectGroupEmployees>();
            foreach (tb_Employee item in empList)
            {
                var urnew = listUR.FirstOrDefault(c => c.ProjectGroup_ID == group.ProjectGroup_ID && c.Employee_ID == item.Employee_ID);
                if (urnew == null)
                {
                    //没有添加到的显示一下，给保存修改准备数据
                    urnew = new tb_ProjectGroupEmployees();
                    urnew.Employee_ID = item.Employee_ID;
                    urnew.ProjectGroup_ID = group.ProjectGroup_ID;
                    urnew.Assigned = false;
                    urnew.DefaultGroup = false;
                    group.tb_ProjectGroupEmployeeses.Add(urnew);
                }
                else
                {
                    group.tb_ProjectGroupEmployeeses.Add(urnew);
                }
            }
            bindingSourceList.DataSource = group.tb_ProjectGroupEmployeeses;
            dataGridView1.DataSource = bindingSourceList;
            dataGridView1.ReadOnly = false;
         
        }



        void node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // only do it if the node became checked:
            if (e.Node.Checked)
            {
                // for all the nodes in the tree...
                foreach (TreeNode cur_node in e.Node.TreeView.Nodes)
                {
                    // ... which are not the freshly checked one...
                    if (cur_node != e.Node)
                    {
                        // ... uncheck them
                        cur_node.Checked = false;
                    }
                }
            }
        }


        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="e"></param>
        public static void TreeViewSingleSelectedAndChecked(Krypton.Toolkit.KryptonTreeView tv, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(tv.Nodes, e.Node);
                }
            }
        }

        private static void CancelCheckedExceptOne(TreeNodeCollection tnc, TreeNode tn)
        {
            foreach (TreeNode item in tnc)
            {
                if (item != tn)
                    item.Checked = false;
                if (item.Nodes.Count > 0)
                    CancelCheckedExceptOne(item.Nodes, tn);

            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            tb_ProjectGroupEmployeesController<tb_ProjectGroupEmployees> ctr = Startup.GetFromFac<tb_ProjectGroupEmployeesController<tb_ProjectGroupEmployees>>();
            //  List<tb_ProjectGroupEmployees> urs = new List<tb_ProjectGroupEmployees>();
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                object obj = dr.DataBoundItem;
                if (obj is tb_ProjectGroupEmployees ur)
                {
                    //urs.Add(ur);
                    ReturnResults<tb_ProjectGroupEmployees> rr = await ctr.SaveOrUpdate(ur);
                }
                /*
               
                if (TreeView1.SelectedNode.Checked)
                {
                    #region
                    tb_ProjectGroup user = TreeView1.SelectedNode.Tag as tb_ProjectGroup;
                    if (user.tb_ProjectGroupEmployees == null)
                    {
                        user.tb_ProjectGroupEmployees = new List<tb_ProjectGroupEmployees>();
                    }
                    //如果这个人名下有这个角色就跳过
                    // bool hasRole = user.tb_ProjectGroupEmployees != null && user.tb_ProjectGroupEmployees.Where(r => r.RoleID == obj.GetPropertyValue<tb_ProjectGroupEmployees>(s => s.RoleID).ToLong()).Any();
                    var ur = user.tb_ProjectGroupEmployees.FirstOrDefault(c => c.RoleID == obj.GetPropertyValue<tb_ProjectGroupEmployees>(s => s.RoleID).ToLong() && c.User_ID == user.User_ID);
                    if (ur == null)
                    {
                        ur = new tb_ProjectGroupEmployees();
                        ur.Authorized = obj.GetPropertyValue<SelectDto>(s => s.UserSelect).ToBool();
                        ur.User_ID = user.User_ID;
                        ur.RoleID = obj.GetPropertyValue<tb_ProjectGroupEmployees>(s => s.RoleID).ToLong();
                        await ctr.AddReEntityAsync(ur);
                        user.tb_ProjectGroupEmployees.Add(ur);
                    }
                    else
                    {
                        ur.Authorized = obj.GetPropertyValue<SelectDto>(s => s.UserSelect).ToBool();
                        //ur.User_ID = user.User_ID;
                        //ur.RoleID = obj.GetPropertyValue<tb_ProjectGroupEmployees>(s => s.RoleID).ToLong();
                        await ctr.UpdateAsync(ur);
                        #region
                        //
                        /*
                         bool hasRole = user.tb_ProjectGroupEmployees != null && user.tb_ProjectGroupEmployees.Where(r => r.Authorized).Any();
                        if (hasRole)
                        {
                            continue;
                        }
                        else
                        {
                            //新勾选的
                            if (obj.GetPropertyValue<SelectDto>(s => s.UserSelect).ToBool())
                            {
                                tb_ProjectGroupEmployees ur = new tb_ProjectGroupEmployees();
                                ur.User_ID = user.User_ID;
                                ur.RoleID = obj.GetPropertyValue<tb_ProjectGroupEmployees>(s => s.RoleID).ToLong();
                                ur.Authorized = true;
                                await ctr.AddReEntityAsync(ur);
                            }
                        }
                         */

            }



        }





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
                        return;
                    }

                }
            }

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_ProjectGroupEmployees>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

        }
        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                object obj = dr.DataBoundItem;
                obj.SetPropertyValue<tb_ProjectGroupEmployees>(s => s.Assigned, chkALL.Checked);
            }
            bindingSourceList.EndEdit();
            dataGridView1.Refresh();
        }



        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        public static ConcurrentDictionary<string, string> FKValueColNameTBList = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            string tableName = typeof(tb_ProjectGroupEmployees).Name;
            foreach (var field in typeof(tb_ProjectGroupEmployees).GetProperties())
            {
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is Global.CustomAttribute.FKRelationAttribute)
                    {
                        Global.CustomAttribute.FKRelationAttribute fkrattr = attr as Global.CustomAttribute.FKRelationAttribute;
                        FKValueColNameTBList.TryAdd(fkrattr.FK_IDColName, fkrattr.FKTableName);
                    }
                }
            }

            this.dataGridView1.DataSource = null;
            //绑定导航
            this.dataGridView1.DataSource = bindingSourceList.DataSource;
        }

        #region base fucntion

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


        #endregion

        private void TreeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // e.Node.ForeColor = Color.Blue;
            // e.Node.NodeFont = new Font("宋体", 10, FontStyle.Underline | FontStyle.Bold);
            //if (theLastNode != null)
            //{
            //    theLastNode.ForeColor = TreeView1.ForeColor;
            //    theLastNode.NodeFont = new Font("宋体", 16, FontStyle.Regular);
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }
            //指定要控制的列名
            Expression<Func<tb_ProjectGroupEmployees, object>> expSelected = c => c.DefaultGroup;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string ModifyColName = dataGridView1.CurrentCell.OwningColumn.Name;
                if (ModifyColName == expSelected.GetMemberInfo().Name)
                {
                    DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    bool isChecked = (bool)checkBoxCell.EditingCellFormattedValue;
                    if (isChecked)
                    {
                        // 执行选中时的操作
                        //其他就不选 
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (i != e.RowIndex)
                            {
                                dataGridView1.Rows[i].Cells[dataGridView1.CurrentCell.OwningColumn.Index].Value = false;
                            }
                        }

                    }
                }
            }
        }

        private void chkNotALL_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                object obj = dr.DataBoundItem;
                obj.SetPropertyValue<tb_ProjectGroupEmployees>(s => s.Assigned, !chkNotALL.Checked);
            }
            bindingSourceList.EndEdit();
            dataGridView1.Refresh();
        }

        private void chkReverseSelect_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                object obj = dr.DataBoundItem;
                tb_ProjectGroupEmployees groupEmployees = obj as tb_ProjectGroupEmployees;
                //groupEmployees.Assigned = !groupEmployees.Assigned;
                obj.SetPropertyValue<tb_ProjectGroupEmployees>(s => s.Assigned, !groupEmployees.Assigned);
            }
            bindingSourceList.EndEdit();
            dataGridView1.Refresh();
        }
    }
}
