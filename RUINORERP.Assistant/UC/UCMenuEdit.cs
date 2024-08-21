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

namespace RUINORERP.Assistant
{
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


        private void btn_add_Click(object sender, EventArgs e)
        {
            if (comboBoxTreeView1.TreeView.SelectedNode == null || string.IsNullOrEmpty(comboBoxTreeView1.Text))
            {
                MessageBox.Show("��ѡ��Ҫ��ӵ���һ���˵���", "��ʾ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }
            else
            {
                this.txt_MenuName.Text = "";
                this.txt_CaptonC.Text = "";
                this.txt_CaptionE.Text = "";
                this.chkisview.Checked = true;
                this.chkEnable.Checked = true;
                cmbMenuType.Text = "";
                txtFormName.Text = "";
                this.txtDiscription.Text = "";
                CmdEnable(false);
                w_EidtFlag = false;
            }


        }

        private void frmMenuEdit_Load(object sender, EventArgs e)
        {
            LoadEnevt();
        }



        private void LoadEnevt()
        {
            MenuRefresh(this.tree_MainMenu);

            MenuRefresh(this.comboBoxTreeView1.TreeView);


            DateTime now = DateTime.Now;

            TimeSpan spand = DateTime.Now - now;
            this.Text = "�˵�������" + "��ʱ" + spand.TotalSeconds.ToString();
            if (txtAssembly.Text.Trim().Length == 0)
            {
                return;
            }

            //���˵��Ѿ���ӵ�
            BindingSortCollection<MenuAssemblyInfo> menuInfoList = LoadTypes(txtAssembly.Text);
            List<MenuAssemblyInfo> treeNodeList = new List<MenuAssemblyInfo>();
            GetTreeNodesToList(tree_MainMenu.Nodes, ref treeNodeList);
            var addList = menuInfoList.Where(x => !treeNodeList.Exists(y => y.Caption == x.Caption)).ToList();

            this.dataGridView1.DataSource = addList.ToBindingSortCollection();

            //this.dataGridView1.Columns[0].HeaderText = "���� " + ds.Tables[0].Rows.Count.ToString() + "��";
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            RecordSave(w_EidtFlag);
        }

        /// <summary>
        /// ��ȡ��֯�ṹ��
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <param name="NodeList"></param>
        /// <returns></returns>
        static void GetTreeNodesToList(TreeNodeCollection TreeNodes, ref List<MenuAssemblyInfo> NodeList)
        {
            if (TreeNodes == null)
                return;


            foreach (TreeNode item in TreeNodes)
            {
                //ֻ��ͨ���˵����������Ѿ���ӹ���
                NodeList.Add(new MenuAssemblyInfo(item.Name, item.Text, "", ""));
                GetTreeNodesToList(item.Nodes, ref NodeList);
            }
        }


        private async void RecordSave(bool w_EidtFlag)
        {
            Model.tb_MenuInfo info = new Model.tb_MenuInfo();

            info.MenuName = this.txt_MenuName.Text.Trim();
            info.IsVisble = chkisview.Checked;
            info.IsEnabled = chkEnable.Checked;
            info.CaptionCN = txt_CaptonC.Text.Trim();
            info.CaptionEN = txt_CaptionE.Text.Trim();
            info.ClassPath = txtClassPath.Text.Trim();
            info.FormName = txtFormName.Text;
            info.MenuType = cmbMenuType.Text;
            info.Discription = txtDiscription.Text.Trim();
            //��ʱ���ӽڵ�ĸ���
            if (comboBoxTreeView1.TreeView.SelectedNode != null)
            {
                info.Sort = comboBoxTreeView1.TreeView.SelectedNode.Nodes.Count;
            }
            if (comboBoxTreeView1.TreeView.SelectedNode.Tag is tb_MenuInfo)
            {
                info.parent_id = (comboBoxTreeView1.TreeView.SelectedNode.Tag as tb_MenuInfo).MenuID;
            }
            else
            {
                info.parent_id = 0;
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
            LoadEnevt();
            CmdEnable(true);
            w_EidtFlag = false;
        }



        //�ݹ鷽��
        private void Bind(TreeNode parNode, List<tb_MenuInfo> list, int nodeId)
        {
            var childList = list.FindAll(t => t.parent_id == nodeId).OrderBy(t => t.Sort);
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

        private void tree_MainMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tree_MainMenu.SelectedNode.Tag is tb_MenuInfo)
            {
                tb_MenuInfo info = tree_MainMenu.SelectedNode.Tag as tb_MenuInfo;
                this.txt_MenuName.Text = info.MenuName;
                chkisview.Checked = info.IsVisble.Value;
                chkEnable.Checked = info.IsEnabled.Value;
                txtDiscription.Text = info.Discription;
                txt_CaptonC.Text = info.CaptionCN;
                txt_CaptionE.Text = info.CaptionEN;
                txtClassPath.Text = info.ClassPath;
                txtDiscription.Text = info.Discription;
                txtFormName.Text = info.FormName;
                cmbMenuType.SelectedIndex = cmbMenuType.FindString(info.MenuType);
                SearchNodes(info.parent_id.ToString(), comboBoxTreeView1.TreeView.Nodes, comboBoxTreeView1.TreeView);
            }
        }

        #region TreeView���Ҳ�ѡ�нڵ�
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
                    treeView1.SelectedNode.Expand();
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
            this.btn_quit.Enabled = p_YesNo;

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
            if (tree_MainMenu.SelectedNode == null)
            {
                return;
            }
            if (MessageBox.Show("ȷ��Ҫɾ����" + tree_MainMenu.SelectedNode.Text, "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (await mc.DeleteMenuInfo(long.Parse(tree_MainMenu.SelectedNode.Name)))
                {
                    LoadEnevt();
                    MessageBox.Show("ɾ���ɹ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }


        #region



        /// <summary>
        /// ˢ�²˵���
        /// </summary>
        public void MenuRefresh(System.Windows.Forms.TreeView tree_MainMenu)
        {

            LoadTree(tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }

        }

        tb_MenuInfoController  mc = Program.GetFromFac<tb_MenuInfoController>();
        private async void LoadTree(System.Windows.Forms.TreeView tree_MainMenu)
        {
            List<tb_MenuInfo> list = new List<tb_MenuInfo>();
            list = await mc.QueryAsync();
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "����ϵͳ�����";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            tree_MainMenu.ExpandAll();
        }




        #endregion



        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows != null && this.dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].DataBoundItem is MenuAssemblyInfo)
                {
                    MenuAssemblyInfo menuInfo = dataGridView1.SelectedRows[0].DataBoundItem as MenuAssemblyInfo;
                    txt_MenuName.Text = menuInfo.Caption;
                    txt_CaptonC.Text = menuInfo.Caption;
                    txtClassPath.Text = menuInfo.ClassPath;
                    txtFormName.Text = menuInfo.MenuPath;
                }
            }
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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtAssembly.Text = openFileDialog1.FileName;
                //��˼�� ���ع������ݿ������ú��ˣ��Ͳ���ʾ�ˡ�


                this.dataGridView1.DataSource = LoadTypes(txtAssembly.Text);

                // this.dataGridView1.Columns[0].HeaderText = "�˵���" + menuList.Count + "��";
            }
        }


        private BindingSortCollection<MenuAssemblyInfo> LoadTypes(string assemblyPath)
        {
            BindingSortCollection<MenuAssemblyInfo> menuList = new BindingSortCollection<MenuAssemblyInfo>();
            //List<MenuAssemblyInfo> menuList = new List<MenuAssemblyInfo>();
            List<Type> loadTypes = new List<Type>();
            Type[]? types = Assembly.LoadFrom(assemblyPath).GetExportedTypes();
            if (types != null)
            {
                var descType = typeof(MenuAttribute);
                foreach (Type type in types)
                {
                    // �����Ƿ�Ϊ���壬����������������һ��ѭ��
                    //if (type.GetTypeInfo() != form)
                    //    continue;

                    //// �Ƿ�Ϊ�Զ������ԣ�����������������һ��ѭ��
                    //if (!type.IsDefined(descType, false))
                    //    continue;

                    // ǿ��Ϊ�Զ�������
                    MenuAttribute? attribute = type.GetCustomAttribute(descType, false) as MenuAttribute;
                    // ���ǿ��ʧ�ܻ��߲���Ҫע��Ĵ���������������һ��ѭ��
                    if (attribute == null || !attribute.IsForm)
                        continue;
                    // Console.WriteLine($"ע�룺{attribute.FormType.Namespace}.{attribute.FormType.Name},{attribute.Describe}");
                    menuList.Add(new MenuAssemblyInfo(attribute.FormType.Name, attribute.Describe, attribute.FormType.FullName, attribute.FormType.Name));
                    loadTypes.Add(attribute.FormType);
                }
            }
            return menuList;
        }

        private void txtAssembly_TextChanged(object sender, EventArgs e)
        {

        }
    }
}