using FastReport.DevComponents.DotNetBar.Controls;
using Krypton.Navigator;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using Krypton.Workspace;
using RUINORERP.Business;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Services;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig
{


    /// <summary>
    /// 多对多的关系
    /// </summary>
    [MenuAttrAssemblyInfo("用户授权", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCUserAuthorization : UserControl
    {
        public UCUserAuthorization()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                DisplayTextResolver = new GridViewDisplayTextResolver(typeof(tb_RoleInfo));
                DisplayTextResolver.Initialize(dataGridView1);
                this.dataGridView1.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            }
        }
        public GridViewDisplayTextResolver DisplayTextResolver;
        tb_RoleInfoController<tb_RoleInfo> ctrRole = Startup.GetFromFac<tb_RoleInfoController<tb_RoleInfo>>();
        private async Task LoadUser()
        {
            TreeView1.Nodes.Clear();
            tb_UserInfoController<tb_UserInfo> ctr = Startup.GetFromFac<tb_UserInfoController<tb_UserInfo>>();

            var list = await ctr.QueryByNavAsync();
            foreach (var item in list)
            {
                if (item.tb_employee != null)
                {
                    //如果是禁用或者删除状态，则不显示
                    if (item.tb_employee.Is_enabled == false)
                    {
                        continue;
                    }
                }
                string NodeText = "";
                NodeText = item.UserName;
                if (item.tb_employee != null)
                {
                    NodeText += "【" + item.tb_employee.Employee_Name + "】";
                }

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

        public List<tb_RoleInfo> listRole { get; set; }
        private void UCUserAuthorization_Load(object sender, EventArgs e)
        {
            listRole = ctrRole.Query();
            TreeView1.HideSelection = false;
            // FieldNameList = UIHelper.GetFieldNameColList(typeof(SelectDto), typeof(tb_RoleInfo));
            dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_User_Role));

            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //属性–Columns–DefaultCellStyle–Alignment–MiddleRight
            //这里是不是与那个缓存 初始化时的那个字段重复？
            ///显示列表对应的中文
            // FieldNameList = UIHelper.GetFieldNameList<tb_RoleInfo>();
            //重构？
            dataGridView1.NeedSaveColumnsXml = true;
            dataGridView1.XmlFileName = "UCUserAuthorization";
            dataGridView1.HideColumn<tb_User_Role>(s => s.User_ID);
            InitListData();
            /*
            combinedType = Common.EmitHelper.MergeTypesNew(typeof(SelectDto), typeof(tb_RoleInfo));
            object InstObj = Activator.CreateInstance(combinedType);
            List<object> objlist = new List<object>();
            objlist.Add(InstObj);*/
            bindingSourceList.DataSource = new List<tb_User_Role>();
            //ListDataSoure.DataSource = objlist;
            dataGridView1.DataSource = bindingSourceList;

            try
            {
                LoadUser();
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
            tb_User_RoleController<tb_User_Role> ctr = Startup.GetFromFac<tb_User_RoleController<tb_User_Role>>();
            e.Node.Checked = true;
            node_AfterCheck(sender, e);


            if (e.Node.Tag is tb_UserInfo userInfo)
            {
                tb_UserInfo user = TreeView1.SelectedNode.Tag as tb_UserInfo;
                List<tb_User_Role> listUR = await ctr.QueryByNavAsync(c => c.User_ID == user.User_ID);
                user.tb_User_Roles = new List<tb_User_Role>();
                foreach (tb_RoleInfo item in listRole)
                {
                    var urnew = listUR.FirstOrDefault(c => c.RoleID == item.RoleID && c.User_ID == user.User_ID);
                    if (urnew == null)
                    {
                        //没有添加到的显示一下，给保存修改准备数据
                        urnew = new tb_User_Role();
                        urnew.User_ID = user.User_ID;
                        urnew.RoleID = item.RoleID;
                        urnew.Authorized = false;
                        urnew.DefaultRole = false;
                        user.tb_User_Roles.Add(urnew);
                    }
                    else
                    {
                        user.tb_User_Roles.Add(urnew);
                    }
                }
                bindingSourceList.DataSource = user.tb_User_Roles;
                dataGridView1.DataSource = bindingSourceList;
                dataGridView1.ReadOnly = false;
            }
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
            // 确保所有编辑更改提交到数据源
            bindingSourceList.EndEdit();

            tb_User_RoleController<tb_User_Role> ctr = Startup.GetFromFac<tb_User_RoleController<tb_User_Role>>();
            //  List<tb_User_Role> urs = new List<tb_User_Role>();
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                object obj = dr.DataBoundItem;
                if (obj is tb_User_Role ur)
                {
                    if (ur.HasChanged)
                    {
                        ReturnResults<tb_User_Role> rr = await ctr.SaveOrUpdate(ur);
                    }
                    ur.AcceptChanges();
                }
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
            string colName = UIHelper.ShowGridColumnsNameValue<tb_User_Role>(colDbName, e.Value);
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
                obj.SetPropertyValue<tb_User_Role>(s => s.Authorized, chkALL.Checked);
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
            string tableName = typeof(tb_User_Role).Name;
            foreach (var field in typeof(tb_User_Role).GetProperties())
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
            Expression<Func<tb_User_Role, object>> expSelected = c => c.DefaultRole;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string ModifyColName = dataGridView1.CurrentCell.OwningColumn.Name;
                if (ModifyColName == expSelected.GetMemberInfo().Name)
                {
                    // 确保编辑已提交
                    bindingSourceList.EndEdit();

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
    }
}
