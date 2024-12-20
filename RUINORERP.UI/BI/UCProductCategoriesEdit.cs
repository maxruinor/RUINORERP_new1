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
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business;
using System.Globalization;
using RUINORERP.UI.Common;
using RUINORERP.Global;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("产品类目编辑", true, UIType.单表数据)]
    public partial class UCProductCategoriesEdit : BaseEditGeneric<tb_ProdCategories>
    {
        public UCProductCategoriesEdit()
        {
            InitializeComponent();
        }

        private tb_ProdCategories _EditEntity;
        public tb_ProdCategories EditEntity { get => _EditEntity; set => _EditEntity = value; }

        List<tb_ProdCategories> list = new List<tb_ProdCategories>(0);
        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_ProdCategories;
            if (_EditEntity.Category_ID == 0)
            {
                _EditEntity.CategoryCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.ProductNo);
            }
            DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.Category_name, txtcategory_name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.CategoryCode, txtcategoryCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_ProdCategories>(entity, t => t.Is_enabled, chkIs_enabled, false);

            #region 排序
            Binding depa = null;
            if (true)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Value", entity, "Sort", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Value", entity, "Sort", true, DataSourceUpdateMode.OnValidation);
            }


            depa.Format += (s, args) =>
            {
                args.Value = args.Value == null ? 0 : args.Value;
                txtSort.Text = args.Value.ToString();
            };

            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) =>
            {
                args.Value = args.Value == null ? 0 : args.Value;
            };

            txtSort.DataBindings.Add(depa);


            //txtSort.Value = 0;
            ////排序
            //var sort = new Binding("Value", entity, "Sort", true, DataSourceUpdateMode.OnValidation);
            ////数据源的数据类型转换为控件要求的数据类型。
            //sort.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            ////将控件的数据类型转换为数据源要求的数据类型。
            //sort.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //txtSort.DataBindings.Add(sort);

            #endregion


            //有默认值
            DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            //父类
            var parent_categorie = new Binding("Text", entity, "Parent_id", true, DataSourceUpdateMode.OnValidation);

            //数据源的数据类型转换为控件要求的数据类型。
            parent_categorie.Format += new ConvertEventHandler(DataSourceToControl);
            //将控件的数据类型转换为数据源要求的数据类型。
            parent_categorie.Parse += new ConvertEventHandler(ControlToDataSource);

            cmbTreeParent_id.DataBindings.Add(parent_categorie);

            base.BindData(entity);




        }

        private void DataSourceToControl(object sender, ConvertEventArgs cevent)
        {
            // 该方法仅转换为字符串类型。使用DesiredType进行测试。
            if (cevent.DesiredType != typeof(string)) return;
            if (cevent.Value == null || cevent.Value.ToString() == "0")
            {
                //cevent.Value = ((decimal)cevent.Value).ToString("c");
                cevent.Value = "类目根结节";
            }
            else
            {
                //显示名称
                tb_ProdCategories entity = list.Find(t => t.Category_ID.ToString() == cevent.Value.ToString());
                if (entity != null)
                {
                    cevent.Value = entity.Category_name;
                }
                else
                {
                    cevent.Value = 0;
                }
            }

        }

        private void ControlToDataSource(object sender, ConvertEventArgs cevent)
        {
            // The method converts back to decimal type only. 
            //if (cevent.DesiredType != typeof(decimal)) return;
            if (string.IsNullOrEmpty(cevent.Value.ToString()) || cevent.Value.ToString() == "类目根结节")
            {
                cevent.Value = 0;
            }
            else
            {
                tb_ProdCategories entity = list.Find(t => t.Category_name == cevent.Value.ToString());
                if (entity != null)
                {
                    cevent.Value = entity.Category_ID;
                }
                else
                {
                    cevent.Value = 0;
                }
            }



        }

        ///// <summary>
        ///// 刷新菜单树
        ///// </summary>
        //public void MenuRefresh(System.Windows.Forms.TreeView tree_MainMenu)
        //{

        //    LoadTree(tree_MainMenu);
        //    if (tree_MainMenu.Nodes.Count > 0)
        //    {
        //        tree_MainMenu.Nodes[0].Expand(); //
        //    }

        //}
        //private async void LoadTree(System.Windows.Forms.TreeView tree_MainMenu)
        //{
        //    list = await mc.QueryAsync();
        //    tree_MainMenu.Nodes.Clear();
        //    TreeNode nodeRoot = new TreeNode();
        //    nodeRoot.Text = "类目根结节";
        //    nodeRoot.Name = "0";
        //    tree_MainMenu.Nodes.Add(nodeRoot);
        //    Bind(nodeRoot, list, 0);
        //    tree_MainMenu.ExpandAll();
        //}

        //递归方法
        private void Bind(TreeNode parNode, List<tb_ProdCategories> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Category_ID.ToString();
                node.Text = nodeObj.Category_name;
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                Bind(node, list, nodeObj.Category_ID);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            //默认给个值，因为如果操作的人不动，不选。这个值会为空
            if (EditEntity.Parent_id == null)
            {
                EditEntity.Parent_id = 0;
            }

            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private async void UCProductCategoriesEdit_Load(object sender, EventArgs e)
        {
            tb_ProdCategoriesController<tb_ProdCategories> ctr = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
            list = await ctr.QueryAsync();
            Common.UIProdCateHelper.BindToTreeView(list, cmbTreeParent_id.TreeView);
        }
    }
}
