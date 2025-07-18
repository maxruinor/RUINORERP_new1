using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using RUINORERP.Business;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.IServices;
using RUINORERP.Services;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.Common;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.Global;
using FastReport.Table;
using System.Threading.Tasks;



namespace RUINORERP.UI.SysConfig
{

    [MenuAttrAssemblyInfo("�˵���ʼ��", ModuleMenuDefine.ģ�鶨��.ϵͳ����, ModuleMenuDefine.ϵͳ����.Ȩ�޹���)]
    public partial class UCMenuEdit : UserControl
    {
        bool w_EidtFlag;
        bool Loaded = false;

        public bool ShowInvalidMessage(ValidationResult results)
        {
            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;
            //validator.ValidateAndThrow(info);
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in failures)
            {
                msg.Append(counter.ToString() + ") ");
                msg.Append(item.ErrorMessage).Append("\r\n");
                counter++;
            }
            if (!results.IsValid)
            {
                MessageBox.Show(msg.ToString(), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return results.IsValid;
        }
        public UCMenuEdit()
        {
            InitializeComponent();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadEnevt();
        }

        //������ʱ��ѡ�е��ǵ����ϼ�
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (comboBoxTreeView1.TreeView.SelectedNode == null || string.IsNullOrEmpty(comboBoxTreeView1.Text))
            {
                MessageBox.Show("��ѡ��Ҫ��ӵ���һ���˵���", "��ʾ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }
            else
            {
                tb_MenuInfo info = new tb_MenuInfo();
                info.MenuName = "�½��˵�";
                if (comboBoxTreeView1.TreeView.SelectedNode.Tag != null)
                {
                    info.Parent_id = (comboBoxTreeView1.TreeView.SelectedNode.Tag as tb_MenuInfo).Parent_id;
                    info.Sort = (comboBoxTreeView1.TreeView.SelectedNode.Tag as tb_MenuInfo).Sort + 1;
                }
                else
                {
                    info.Parent_id = 0;
                }

                info.IsEnabled = true;
                info.IsVisble = true;
                info.MenuType = "�˵�";
                BindData(info);
                tree_MainMenu.SelectedNode.Tag = info;
                CmdEnable(false);
                w_EidtFlag = false;

                //tree_MainMenu.SelectedNode = tree_MainMenu.Nodes[0].Nodes.Add(info);

            }
        }

        private void frmMenuEdit_Load(object sender, EventArgs e)
        {
            // MenuRefresh(this.comboBoxTreeView1.TreeView);
            MenuRefreshByCom(this.comboBoxTreeView1.TreeView);


            dataGridView1.XmlFileName = typeof(MenuAttrAssemblyInfo).Name;
            dataGridView1.UseCustomColumnDisplay = false;
            //���ص�ǰ���򼯵�ҵ����
            List<MenuAttrAssemblyInfo> list = UIHelper.RegisterForm();
            dataGridView1.DataSource = list.ToBindingSortCollection<MenuAttrAssemblyInfo>();
            txtMenuName.Tag = dataGridView1.DataSource;//�����������������ʱ�ָ�����ʹ�á�
            LoadEnevt();
            tree_MainMenu.HideSelection = false;
            tree_MainMenu.Nodes[0].Expand();
            //tree_MainMenu.DrawMode = TreeViewDrawMode.OwnerDrawText;

            txtMenuName.TextChanged += txtMenuName_TextChanged;
        }

        private void txtMenuName_TextChanged(object sender, EventArgs e)
        {
            BindingSortCollection<MenuAttrAssemblyInfo> oldList = new BindingSortCollection<MenuAttrAssemblyInfo>();

            if (txtMenuName.Tag != null && txtMenuName.Tag is List<MenuAttrAssemblyInfo> list)
            {
                oldList = list.ToBindingSortCollection();
            }
            if (txtMenuName.Tag != null && txtMenuName.Tag is BindingSortCollection<MenuAttrAssemblyInfo> sortList)
            {
                oldList = sortList;
            }

            //�����ǻָ����ݣ�ԭʼ���ݡ����˵Ļ�����ÿ����Ҫ


            if (dataGridView1.Rows.Count > 0 && txtMenuName.Text.Trim().Length > 0)
            {
                if (dataGridView1.DataSource is List<MenuAttrAssemblyInfo> menuAttrAssemblyInfos)
                {
                    dataGridView1.DataSource = oldList.Where(c => c.Caption.Contains(txtMenuName.Text.Trim())).ToList();
                }
                if (dataGridView1.DataSource is BindingSortCollection<MenuAttrAssemblyInfo> SortMenuAttrAssemblyInfos)
                {
                    dataGridView1.DataSource = oldList.Where(c => c.Caption.Contains(txtMenuName.Text.Trim())).ToList();
                }
            }
            else
            {

                dataGridView1.DataSource = oldList;
            }
        }


        /// <summary>
        /// ���ز˵���
        /// </summary>
        private void LoadEnevt()
        {
            MenuRefresh(this.tree_MainMenu);


            DateTime now = DateTime.Now;

            TimeSpan spand = DateTime.Now - now;
            this.Text = "�˵�������" + "��ʱ" + spand.TotalSeconds.ToString();
            if (txtAssembly.Text.Trim().Length == 0)
            {
                return;
            }

            //���˵��Ѿ���ӵ�
            BindingSortCollection<MenuAttrAssemblyInfo> menuInfoList = LoadTypes(txtAssembly.Text);
            List<MenuAttrAssemblyInfo> treeNodeList = new List<MenuAttrAssemblyInfo>();
            GetTreeNodesToList(tree_MainMenu.Nodes, ref treeNodeList);
            var addList = menuInfoList.Where(x => !treeNodeList.Exists(y => y.Caption == x.Caption)).ToList();

            this.dataGridView1.DataSource = addList.ToBindingSortCollection();

            //this.dataGridView1.Columns[0].HeaderText = "���� " + ds.Tables[0].Rows.Count.ToString() + "��";
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            if (comboBoxTreeView1.TreeView.SelectedNode != null)
            {
                RecordSave(w_EidtFlag);
            }
            else
            {
                MessageBox.Show("���ڲ˵�����ѡ��Ҫ����Ľڵ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


        }

        /// <summary>
        /// ��ȡ��֯�ṹ��
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <param name="NodeList"></param>
        /// <returns></returns>
        static void GetTreeNodesToList(TreeNodeCollection TreeNodes, ref List<MenuAttrAssemblyInfo> NodeList)
        {
            if (TreeNodes == null)
                return;


            foreach (TreeNode item in TreeNodes)
            {
                //ֻ��ͨ���˵����������Ѿ���ӹ���
                NodeList.Add(new MenuAttrAssemblyInfo(item.Name, item.Text, "", ""));
                GetTreeNodesToList(item.Nodes, ref NodeList);
            }
        }


        private async void RecordSave(bool w_EidtFlag)
        {
            if (tree_MainMenu.SelectedNode.Tag is tb_MenuInfo)
            {
                tb_MenuInfo info = tree_MainMenu.SelectedNode.Tag as tb_MenuInfo;
                info.MenuName = this.txt_MenuName.Text.Trim();

                if (txtBizType.Tag != null)
                {
                    info.BizType = (int)Enum.Parse(typeof(BizType), txtBizType.Tag.ToString());
                }

                if (info.MenuType.Trim().Length == 0)
                {
                    MessageBox.Show("�˵����Ͳ���Ϊ�գ�");
                    return;
                }
                info.EntityName = txtEntityName.Text;
                if (txtSort.Text.Trim().Length > 0)
                {
                    info.Sort = int.TryParse(txtSort.Text, out int sort) ? sort : 0;
                }



                //��ʱ���ӽڵ�ĸ���
                if (comboBoxTreeView1.TreeView.SelectedNode != null && info.Sort == 0)
                {
                    info.Sort = comboBoxTreeView1.TreeView.SelectedNode.Nodes.Count;
                }
                if (comboBoxTreeView1.TreeView.SelectedNode.Tag is tb_MenuInfo)
                {
                    tb_MenuInfo mi = (comboBoxTreeView1.TreeView.SelectedNode.Tag as tb_MenuInfo);
                    info.Parent_id = mi.MenuID;
                    info.ModuleID = mi.ModuleID;
                }
                else
                {
                    info.Parent_id = 0;
                }

                bool vd = ShowInvalidMessage(mc.Validator(info));
                if (!vd)
                {
                    return;
                }
                if (!w_EidtFlag)//�¼�
                {
                    info.Created_at = System.DateTime.Now;
                    await mc.AddMenuInfoAsync(info);
                }
                else //�޸�
                {
                    info.Modified_at = System.DateTime.Now;
                    if (tree_MainMenu.SelectedNode.Tag is tb_MenuInfo)
                    {
                        info.MenuID = (tree_MainMenu.SelectedNode.Tag as tb_MenuInfo).MenuID;
                    }
                    //drset.Created_by = UserSettings.Instance.EmpNo;
                    w_EidtFlag = !await mc.UpdateMenuInfo(info);
                }
                //LoadEnevt();
                CmdEnable(true);
                w_EidtFlag = false;
            }
        }

        //�ݹ鷽��
        private void Bind(TreeNode parNode, List<tb_MenuInfo> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.MenuID.ToString();
                node.Text = nodeObj.MenuName;
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                Bind(node, list, nodeObj.MenuID);
            }
        }
        private void btn_modify_Click(object sender, EventArgs e)
        {
            w_EidtFlag = true;
            CmdEnable(false);

        }
        public void BindData(tb_MenuInfo entity)
        {
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuName, txt_MenuName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Discription, txtDiscription, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.CaptionCN, txt_CaptonC, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.CaptionEN, txt_CaptionE, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.ClassPath, txtClassPath, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.UIPropertyIdentifier, txtUIPropertyIdentifier, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BizInterface, txtBizInterface, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.FormName, txtFormName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.EntityName, txtEntityName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BIBaseForm, txtBIBaseForm, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BIBizBaseForm, txtBIBizBaseform, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_MenuInfo>(entity, t => t.IsVisble, chkisview, false);
            DataBindingHelper.BindData4CheckBox<tb_MenuInfo>(entity, t => t.IsEnabled, chkEnable, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Text, false);
            cmbMenuType.SelectedIndex = cmbMenuType.FindString(entity.MenuType.ToString());
            entity.AcceptChanges();
        }
        private void tree_MainMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tree_MainMenu.SelectedNode.Tag is tb_MenuInfo)
            {
                tb_MenuInfo info = tree_MainMenu.SelectedNode.Tag as tb_MenuInfo;
                //ѡ��ʱ dg��Ҳѡ��
                dataGridView1.ClearSelection();
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    if (dr.DataBoundItem is MenuAttrAssemblyInfo)
                    {
                        MenuAttrAssemblyInfo ma = dr.DataBoundItem as MenuAttrAssemblyInfo;
                        if (ma.ClassPath == info.ClassPath)
                        {
                            dr.Selected = true;
                            if (dr.Visible)
                            {
                                dataGridView1.FirstDisplayedScrollingRowIndex = dr.Index;
                                //dataGridView1.CurrentCell = dr.Cells[2]���Ҳ����
                            }

                            break;
                        }

                    }
                }



                //this.txt_MenuName.Text = info.MenuName;
                //chkisview.Checked = info.IsVisble;
                //chkEnable.Checked = info.IsEnabled;
                //txtDiscription.Text = info.Discription;
                //txt_CaptonC.Text = info.CaptionCN;
                //txt_CaptionE.Text = info.CaptionEN;
                //txtClassPath.Text = info.ClassPath;
                //txtUIPropertyIdentifier.Text = info.UIPropertyIdentifier;
                //txtBizInterface.Text = info.BizInterface;
                //txtDiscription.Text = info.Discription;
                //txtFormName.Text = info.FormName;
                //txtEntityName.Text = info.EntityName;
                //txtSort.Text = info.Sort.ToString();
                //txtBIBaseForm.Text = info.BIBaseForm;
                //txtBIBizBaseform.Text = info.BIBizBaseForm;
                //if (info.BizType.HasValue)
                //{
                //    txtBizType.Text = info.BizType.ToString();
                //    txtBizType.Tag = info.BizType;
                //}
                //else
                //{
                //    txtBizType.Text = "";
                //    txtBizType.Tag = null;
                //}
                //cmbMenuType.SelectedIndex = cmbMenuType.FindString(info.MenuType);

                BindData(info);
                //�����ϼ��˵��ڵ�
                SearchNodes(info.Parent_id.ToString(), comboBoxTreeView1.TreeView.Nodes, comboBoxTreeView1.TreeView);

                //treeView1.SelectedImageIndex = treeView1.SelectedNode.ImageIndex;
            }
        }

        #region TreeView���Ҳ�ѡ�нڵ�

        private void SearchNodesByLeft(string SearchText, TreeNodeCollection tc, Krypton.Toolkit.KryptonTreeView treeView1)
        {
            if (tc == null)
            {
                return;
            }
            foreach (TreeNode StartNode in tc)
            {
                if (StartNode.Name.ToLower().Contains(SearchText.ToLower()))
                {
                    treeView1.SelectedNode = StartNode;
                    treeView1.SelectedNode.Expand();
                    treeView1.Select();
                    return;
                }
                if (StartNode.Nodes.Count != 0)
                {
                    SearchNodesByLeft(SearchText, StartNode.Nodes, treeView1);//�ݹ�����
                }
            }
        }

        private void SearchNodes(string SearchText, TreeNodeCollection tc, TreeView treeView1)
        {
            if (tc == null)
            {
                return;
            }
            foreach (TreeNode StartNode in tc)
            {
                if (StartNode.Name.ToLower().Contains(SearchText.ToLower()))
                {
                    treeView1.SelectedNode = StartNode;
                    comboBoxTreeView1.Text = StartNode.Text;
                    treeView1.SelectedNode.Expand();
                    comboBoxTreeView1.SelectedItem = StartNode;
                    treeView1.Select();
                    return;
                }
                if (StartNode.Nodes.Count != 0)
                {
                    SearchNodes(SearchText, StartNode.Nodes, treeView1);//�ݹ�����
                }
            }
        }
        #endregion



        private void CmdEnable(bool p_YesNo)
        {
            this.btn_add.Enabled = p_YesNo;
            this.btn_modify.Enabled = p_YesNo;
            btn_save.Enabled = !p_YesNo;
            btn_del.Enabled = p_YesNo;
            btn_cancel.Enabled = !p_YesNo;
            this.btn_Refresh.Enabled = p_YesNo;
        }




        /// <summary>
        /// ȡ���޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            w_EidtFlag = false;
            CmdEnable(true);
        }

        /// <summary>
        /// ɾ���ڵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_del_Click(object sender, EventArgs e)
        {
            TreeNode snode = null;
            if (tree_MainMenu.SelectedNode == null)
            {
                return;
            }
            else
            {
                snode = tree_MainMenu.SelectedNode;
            }

            //������ ��ʱ֧���������ٶ����д
            if (snode.Nodes.Count > 0)
            {
                if (MessageBox.Show("��ǰѡ��ڵ㡾" + snode.Text + "��" + "�����ӽڵ㣬��ȫ��ɾ����ȷ����", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    foreach (TreeNode item in snode.Nodes)
                    {
                        foreach (TreeNode subitem in item.Nodes)
                        {
                            await mc.DeleteMenuInfo(int.Parse(subitem.Name));
                        }
                        await mc.DeleteMenuInfo(int.Parse(item.Name));
                    }
                    await mc.DeleteMenuInfo(int.Parse(snode.Name));
                    LoadEnevt();
                    MessageBox.Show("ɾ���ɹ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (MessageBox.Show("ȷ��Ҫɾ��" + "��" + snode.Text + "��" + "��", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (await mc.BaseDeleteByNavAsync(snode.Tag as tb_MenuInfo))
                    {
                        LoadEnevt();
                        MessageBox.Show("ɾ���ɹ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }



        }


        #region



        /// <summary>
        /// ˢ�²˵���  ������ һ����һ������
        /// </summary>
        public void MenuRefresh(Krypton.Toolkit.KryptonTreeView tree_MainMenu)
        {

            LoadTree(tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }

        }

        tb_MenuInfoController<tb_MenuInfo> mc = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
        private void LoadTree(Krypton.Toolkit.KryptonTreeView tree_MainMenu)
        {
            List<tb_MenuInfo> list = new List<tb_MenuInfo>();
            list = mc.Query();
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "����ϵͳ�����";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            //tree_MainMenu.ExpandAll();
            tree_MainMenu.Nodes[0].Expand();
        }

        public void MenuRefreshByCom(TreeView tree_MainMenu)
        {

            LoadTreeByCom(tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }

        }

        List<tb_MenuInfo> list = new List<tb_MenuInfo>();

        private void LoadTreeByCom(TreeView tree_MainMenu)
        {

            list = mc.Query();
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "����ϵͳ�����";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            //tree_MainMenu.ExpandAll();
            tree_MainMenu.Nodes[0].Expand();
        }

        #endregion


        /// <summary>
        /// esc�˳�����
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //����س���
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        Exit(this);//csc�رմ���
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

        KryptonPage AdvPage = null;
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
            //����򿪹��߼���ѯ Ҫɾ��
            if (AdvPage != null)
            {
                MainForm.Instance.kryptonDockingManager1.RemovePage(AdvPage.UniqueName, true);
                AdvPage.Dispose();
            }


            /*
           if (page == null)
           {
               //����

           }
           else
           {
               //���
               if (cell.Pages.Contains(page))
               {
                   cell.Pages.Remove(page);
                   page.Dispose();
               }
           }
           */
        }
        protected virtual void Exit(object thisform)
        {
            if (!w_EidtFlag)
            {
                //�˳�
                CloseTheForm(thisform);
            }
            else
            {
                if (MessageBox.Show(this, "������û�б���\r\n��ȷ��Ҫ�˳���?   �����ǲ��ǿ��Խ�һ����ʾ ��Щ����û�б��棿", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //�˳�
                    CloseTheForm(thisform);
                }
            }
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void frmMenuEdit_Shown(object sender, EventArgs e)
        {
            Loaded = true;
        }


        private void cmbInstallType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                LoadEnevt();
            }
        }

        private void tree_MainMenu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            //���ص�ǰ���򼯵�ҵ����
            List<MenuAttrAssemblyInfo> list = UIHelper.RegisterForm();
            dataGridView1.DataSource = list.ToBindingSortCollection<MenuAttrAssemblyInfo>();
            txtMenuName.Tag = dataGridView1.DataSource;//�����������������ʱ�ָ�����ʹ�á�
            //MenuController mc = Startup.GetFromFac<MenuController>();
            //List<tb_MenuInfo> listAA = new List<tb_MenuInfo>();
            //listAA = await mc.Query();

            /*
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtAssembly.Text = openFileDialog1.FileName;
                //��˼�� ���ع������ݿ������ú��ˣ��Ͳ���ʾ�ˡ�


                this.dataGridView1.DataSource = LoadTypes(txtAssembly.Text);

                // this.dataGridView1.Columns[0].HeaderText = "�˵���" + menuList.Count + "��";
            }*/

        }


        private BindingSortCollection<MenuAttrAssemblyInfo> LoadTypes(string assemblyPath)
        {
            //  BindingSortCollection<MenuAssemblyInfo> menuList = new BindingSortCollection<MenuAssemblyInfo>();
            return UIHelper.RegisterForm(assemblyPath).ToBindingSortCollection<MenuAttrAssemblyInfo>();
        }

        private void txtAssembly_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkOnlyNew_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOnlyNew.Checked)
            {
                List<tb_MenuInfo> list = new List<tb_MenuInfo>();
                list = mc.Query();

                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    MenuAttrAssemblyInfo info = dr.DataBoundItem as MenuAttrAssemblyInfo;
                    if (list.Where<tb_MenuInfo>(m => m.ClassPath == info.ClassPath).ToList().Count > 0)
                    {
                        CurrencyManager cm = (CurrencyManager)BindingContext[dr.DataGridView.DataSource];//
                        cm.SuspendBinding(); //�������б�����
                        dr.ReadOnly = true;//���������п�ѡ��������DataGridView�Ǳ༭�ľͼ��ϰɡ�
                        dr.Visible = false;
                        cm.ResumeBinding();//���������б�����

                    }
                    else
                    {
                        dr.Visible = true;
                    }
                }

                txtMenuName.Tag = dataGridView1.DataSource;//�����������������ʱ�ָ�����ʹ�á�
            }
            else
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Visible = true;
                }
            }
        }

        private void kryptonSplitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            #region
            if (this.dataGridView1.SelectedRows != null && this.dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].DataBoundItem is MenuAttrAssemblyInfo)
                {
                    MenuAttrAssemblyInfo menuInfo = dataGridView1.SelectedRows[0].DataBoundItem as MenuAttrAssemblyInfo;
                    btn_modify.Enabled = true;
                    dataGridView1.ReadOnly = false;
                    if (menuInfo.MenuBizType.HasValue)
                    {
                        txtBizType.Text = ((int)menuInfo.MenuBizType).ToString();
                        txtBizType.Tag = menuInfo.MenuBizType;
                    }
                    else
                    {
                        txtBizType.Text = "";
                        txtBizType.Tag = null;
                    }
                    if (tree_MainMenu.SelectedNode != null && tree_MainMenu.SelectedNode.Tag is tb_MenuInfo)
                    {
                        tb_MenuInfo info = tree_MainMenu.SelectedNode.Tag as tb_MenuInfo;
                        //BindData(info);
                        if (info.ClassPath == menuInfo.ClassPath || info.CaptionCN == menuInfo.Caption || info.MenuID == 0)
                        {
                            info.EntityName = menuInfo.EntityName;
                            info.MenuName = menuInfo.Caption;
                            info.FormName = menuInfo.ClassName;
                            info.ClassPath = menuInfo.ClassPath;
                            info.CaptionCN = menuInfo.Caption;
                            //info.CaptionCN = menuInfo.Caption;
                            info.UIPropertyIdentifier = menuInfo.UIPropertyIdentifier;
                            info.BizInterface = menuInfo.BizInterface;
                            info.BizInterface = menuInfo.BizInterface;
                            info.BIBaseForm = menuInfo.BIBaseForm;
                            info.BIBizBaseForm = menuInfo.BIBizBaseForm;
                            info.BizInterface = menuInfo.BizInterface;
                            if (menuInfo.MenuBizType.HasValue)
                            {
                                info.BizType = (int)menuInfo.MenuBizType;
                            }
                            info.MenuType = "��Ϊ�˵�";
                        }
                    }

                    if (menuInfo.BIBaseForm.Trim().Length > 0)
                    {
                        tb_MenuInfo tb_Menu = list.Where(c => c.CaptionCN == menuInfo.Caption && c.MenuType == "��Ϊ�˵�").FirstOrDefault();
                        if (tb_Menu != null)
                        {
                            SearchNodesByLeft(tb_Menu.MenuID.ToString(), tree_MainMenu.Nodes, tree_MainMenu);
                        }

                    }

                }
            }
            #endregion
        }

        private void tree_MainMenu_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // ��ʼ�϶�����
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tree_MainMenu_DragOver(object sender, DragEventArgs e)
        {
            // �������
            e.Effect = DragDropEffects.Move;
        }

        private async void tree_MainMenu_DragDropAsync(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // ��ȡ�϶��Ľڵ�
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // ��ȡ�϶��Ľڵ�������Ƴ���
            TreeNode sourceNode = new TreeNode();
            sourceNode.Name = draggedNode.Name;
            sourceNode.Text = draggedNode.Text;
            sourceNode.Tag = draggedNode.Tag;

            // ��ȡ���õĽڵ�
            TreeNode NewTargetNode = tree_MainMenu.GetNodeAt(tree_MainMenu.PointToClient(new Point(e.X, e.Y)));
            if (NewTargetNode != null)
            {
                // �жϷ���λ�ã�����Ϊ�ӽڵ㻹��ͬ���ڵ�
                if (NewTargetNode.Tag is tb_MenuInfo NewTargetMenuInfo && sourceNode.Tag is tb_MenuInfo SourceMenuInfo)
                {
                    draggedNode.Remove();

                    if (NewTargetMenuInfo.Parent_id == SourceMenuInfo.Parent_id)
                    {
                        //ͬ���ڵ� ֻ��λ��˳��
                        // ��ԭλ���Ƴ��ڵ�
                        // ��Ϊͬ���ڵ����
                        TreeNode parentNode = NewTargetNode.Parent;
                        if (parentNode == null)
                        {
                            return;
                        }
                        int index = parentNode.Nodes.IndexOf(NewTargetNode);
                        parentNode.Nodes.Insert(index + 1, sourceNode);

                        // ���� sort ֵ
                        int tempSort = SourceMenuInfo.Sort;
                        SourceMenuInfo.Sort = NewTargetMenuInfo.Sort;
                        NewTargetMenuInfo.Sort = tempSort;

                        //ʵ���������н���sort��ֵû��ɶ��
                        //��������     Level: ������ã�
                        SourceMenuInfo.Sort = sourceNode.Index;

                        //�������� Ҫ���µ�ǰ�ڵ�����������ӽڵ������ֵ+1  or -1
                        //List<tb_MenuInfo> levelMenuItems = mc.Query().Where(c => c.Parent_id == NewTargetMenuInfo.Parent_id && ).ToList();
                        List<tb_MenuInfo> levelMenuItems = new List<tb_MenuInfo>();
                        foreach (var item in NewTargetNode.Parent.Nodes)
                        {
                            if (item is TreeNode treeNode)
                            {
                                if (treeNode.Index >= NewTargetNode.Index)
                                {
                                    if (treeNode.Tag is tb_MenuInfo sortNodeMenuInfo)
                                    {
                                        sortNodeMenuInfo.Sort = treeNode.Index;
                                        await mc.UpdateMenuInfo(sortNodeMenuInfo);
                                    }
                                }
                            }
                        }



                    }
                    else
                    {
                        // ��Ϊ�ӽڵ� ��Ҫ��pid
                        // ��Ϊ�ӽڵ����
                        NewTargetNode.Nodes.Add(sourceNode);
                        // ���� sort ֵ
                        int tempSort = SourceMenuInfo.Sort;
                        SourceMenuInfo.Sort = NewTargetMenuInfo.Sort;
                        NewTargetMenuInfo.Sort = tempSort;
                        SourceMenuInfo.Parent_id = NewTargetMenuInfo.MenuID;

                        //ʵ���������н���sort��ֵû��ɶ��
                        //��������
                        //SourceMenuInfo.Sort = sourceNode.Index;
                    }

                    //await mc.UpdateMenuInfo(SourceMenuInfo);
                    //await mc.UpdateMenuInfo(NewTargetMenuInfo);
                }





            }
        }

        private void txtMenuName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}