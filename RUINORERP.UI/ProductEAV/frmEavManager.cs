using FluentValidation.Results;
using RUINORERP.Business;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;

namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("属性值管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.货品资料)]
    public partial class frmEavManager : UserControl
    {
        enum BtnAction
        {
            add,
            cancel,
            save,
            edit
        }

        public frmEavManager()
        {
            InitializeComponent();
        }

        private void ControlBtnEnable4Prop(BtnAction action)
        {
            switch (action)
            {
                case BtnAction.add:
                    btnAddOption.Enabled = false;
                    btnSaveProp.Enabled = true;
                    btnCancelProp.Enabled = true;
                    break;
                case BtnAction.cancel:
                    btnAddOption.Enabled = true;
                    btnSaveProp.Enabled = false;
                    btnCancelProp.Enabled = false;
                    break;
                case BtnAction.save:
                    btnAddOption.Enabled = true;
                    btnSaveProp.Enabled = false;
                    btnCancelProp.Enabled = false;
                    break;
                case BtnAction.edit:
                    btnAddOption.Enabled = false;
                    btnSaveProp.Enabled = true;
                    btnCancelProp.Enabled = true;
                    break;
                default:
                    break;
            }


        }

        private void ControlBtnEnable4PropValue(BtnAction action)
        {
            switch (action)
            {
                case BtnAction.add:
                    btnAddOptionValue.Enabled = false;
                    btnSavePropValue.Enabled = true;
                    btnCancelValue.Enabled = true;
                    break;
                case BtnAction.cancel:
                    btnAddOptionValue.Enabled = true;
                    btnSavePropValue.Enabled = false;
                    btnCancelValue.Enabled = false;
                    break;
                case BtnAction.save:
                    btnAddOptionValue.Enabled = true;
                    btnSavePropValue.Enabled = false;
                    btnCancelValue.Enabled = false;
                    break;
                case BtnAction.edit:
                    btnAddOptionValue.Enabled = false;
                    btnSavePropValue.Enabled = true;
                    btnCancelValue.Enabled = true;
                    break;
                default:
                    break;
            }


        }


        private async void btn选项保存_Click(object sender, EventArgs e)
        {
            tb_ProdProperty entity = bindingSourceProperty.Current as tb_ProdProperty;
            ValidationResult results = controllerProp.Validator(entity);
            if (ShowInvalidMessage(results))
            {
                bindingSourceProperty.EndEdit();
                if (entity.actionStatus == ActionStatus.修改)
                {
                    await controllerProp.UpdateAsync(entity);
                }
                if (entity.actionStatus == ActionStatus.新增)
                {
                    await controllerProp.AddAsync(entity);
                }
                entity.actionStatus = ActionStatus.无操作;
                LoadProperty();
                InitCmbProperty();
                ControlBtnEnable4Prop(BtnAction.save);
            }

        }

        private async void btn选项值保存_Click(object sender, EventArgs e)
        {
            tb_ProdPropertyValue entity = bindingSourcePropertyValue.Current as tb_ProdPropertyValue;
            if (entity == null)
            {
                MessageBox.Show("属性值不能为空。");
                return;
            }
            ValidationResult results = controllerPropValue.Validator(entity);
            if (ShowInvalidMessage(results))
            {
                bindingSourcePropertyValue.EndEdit();
                if (entity.actionStatus == ActionStatus.修改)
                {
                    await controllerPropValue.UpdateAsync(entity);
                }
                if (entity.actionStatus == ActionStatus.新增)
                {
                    await controllerPropValue.AddAsync(entity);
                }
                entity.actionStatus = ActionStatus.无操作;

                LoadPropertyValue();
                ControlBtnEnable4PropValue(BtnAction.save);
                QueryPropertyValue(entity.Property_ID);
            }

        }

        private void frmEavManager_Load(object sender, EventArgs e)
        {
            dataGridView属性.AllowUserToAddRows = false;
            dataGridView属性.AllowUserToDeleteRows = true;
            LoadProperty();
            InitCmbProperty();
            dataGridView属性.XmlFileName = "tb_Products_Property";
            btnSaveProp.Enabled = false;
            dataGridView属性.UserDeletingRow += DataGridView属性_UserDeletingRow;
            dataGridView属性.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_ProdProperty));//.GetFieldNameList<tb_ProdProperty>();
            dataGridView属性.Tag = dataGridView属性.FieldNameList;
            //=================
            LoadPropertyValue();
            //dataGridView属性值.XmlFileName = "tb_Products_PropertyValue";
            dataGridView属性值.UserDeletingRow += DataGridView属性值_UserDeletingRow;
            btnSaveProp.Enabled = false;
            btnCancelValue.Enabled = false;
            dataGridView属性值.CellFormatting += DataGridView属性值_CellFormatting;
            //dataGridView属性值.FieldNameList = UIHelper.GetFieldNameList<tb_Products_PropertyValue>();
        }

        private void DataGridView属性值_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView属性值.Columns[e.ColumnIndex].Name == "Property_ID")
            {
                if (e.Value != null)
                {
                    //这里用缓存
                    tb_ProdProperty entity = listForProp.Find(delegate (tb_ProdProperty p) { return p.Property_ID == int.Parse(e.Value.ToString()); });
                    if (entity != null)
                    {
                        e.Value = entity.PropertyName;
                    }
                }
            }
        }

        private void DataGridView属性值_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show(this, "本操作是不能恢复的\r 你确定删除选择的数据吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in dataGridView属性值.SelectedRows)
                {
                    tb_ProdPropertyValue entity = item.DataBoundItem as tb_ProdPropertyValue;
                    //controllerPropValue.DeleteAsync(entity.Property_ID);
                    controllerPropValue._unitOfWorkManage.GetDbClient().Deleteable<tb_ProdPropertyValue>().In(entity.PropertyValueID).ExecuteCommand();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        List<tb_ProdProperty> listForProp = new List<tb_ProdProperty>();
        private async void LoadProperty()
        {
            //Task<DataTable?> printRes = await controllerProp.QueryAsync();
            //listForProp = await controllerProp.QueryAsync();
            Task<List<tb_ProdProperty>> TaskList = controllerProp.QueryAsync();
            await TaskList;
            dataGridView属性.DataSource = null;
            bindingSourceProperty.DataSource = TaskList.Result.ToBindingSortCollection();
            dataGridView属性.DataSource = bindingSourceProperty;
            TaskList.GetAwaiter().OnCompleted(() =>
            {
                ///显示列表对应的中文
                dataGridView属性.ColumnDisplayControl(UIHelper.GetFieldNameList<tb_ProdProperty>());
                dataGridView属性.ReadOnly = true;
            });


        }

        private async void LoadPropertyValue()
        {
            List<tb_ProdPropertyValue> list = await controllerPropValue.QueryByPropertyIDAsync(0);
            dataGridView属性值.DataSource = null;
            bindingSourcePropertyValue.DataSource = list.ToBindingSortCollection();
            dataGridView属性值.DataSource = bindingSourcePropertyValue;
            ///显示列表对应的中文
            dataGridView属性值.ColumnDisplayControl(UIHelper.GetFieldNameList<tb_ProdPropertyValue>());
            dataGridView属性值.ReadOnly = true;
            dataGridView属性值.AllowUserToAddRows = false;
            dataGridView属性值.AllowUserToDeleteRows = true;
        }
        private void DataGridView属性_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show(this, "本操作是不能恢复的\r 你确定删除选择的数据吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in dataGridView属性.SelectedRows)
                {
                    tb_ProdProperty entity = item.DataBoundItem as tb_ProdProperty;
                    //controllerProp.DeleteAsync(entity.Property_ID);
                    controllerProp._unitOfWorkManage.GetDbClient().Deleteable<tb_ProdProperty>().In(entity.Property_ID).ExecuteCommand();
                }
                InitCmbProperty();
            }
            else
            {
                e.Cancel = true;
            }
        }





        tb_ProdPropertyController<tb_ProdProperty> controllerProp = Startup.GetFromFac<tb_ProdPropertyController<tb_ProdProperty>>();
        tb_ProdPropertyValueController<tb_ProdPropertyValue> controllerPropValue = Startup.GetFromFac<tb_ProdPropertyValueController<tb_ProdPropertyValue>>();


        #region 查询选项值
        private async void QueryPropertyValue()
        {
            List<tb_ProdPropertyValue> list = await controllerPropValue.QueryAsync();
            dataGridView属性值.DataSource = null;
            bindingSourcePropertyValue.DataSource = list.ToBindingSortCollection();
            dataGridView属性值.DataSource = bindingSourcePropertyValue;
            dataGridView属性值.ColumnDisplayControl(UIHelper.GetFieldNameList<tb_ProdPropertyValue>());
        }


        private async void QueryPropertyValue(long OptionID)
        {
            List<tb_ProdPropertyValue> list = await controllerPropValue.QueryByPropertyIDAsync(OptionID);
            dataGridView属性值.DataSource = null;
            bindingSourcePropertyValue.DataSource = list.ToBindingSortCollection();
            dataGridView属性值.DataSource = bindingSourcePropertyValue;
            dataGridView属性值.ColumnDisplayControl(UIHelper.GetFieldNameList<tb_ProdPropertyValue>());
        }

        #endregion


        private async void InitCmbProperty()
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = await controllerProp.QueryAsync();
            ComboBoxHelper.InitDropList(bs, cmbOption, "Property_ID", "PropertyName", ComboBoxStyle.DropDownList, false, true);
        }


        private void btnAddOption_Click(object sender, EventArgs e)
        {
            tb_ProdProperty entity = bindingSourceProperty.AddNew() as tb_ProdProperty;
            entity.actionStatus = ActionStatus.新增;
            BindData属性(entity);
            ControlBtnEnable4Prop(BtnAction.add);
        }

        public void BindData属性(tb_ProdProperty entity)
        {
            txtPropertyName.DataBindings.Clear();
            txtPropertyDesc.DataBindings.Clear();
            txtSortOrder.DataBindings.Clear();
            txtInputType.DataBindings.Clear();

            txtInputType.Text = "";
            txtPropertyName.Text = "";
            txtPropertyDesc.Text = "";
            txtSortOrder.Value = 0;

            txtPropertyName.DataBindings.Add("Text", entity, "PropertyName", false, DataSourceUpdateMode.OnValidation);
            txtPropertyDesc.DataBindings.Add("Text", entity, "PropertyDesc", false, DataSourceUpdateMode.OnValidation);

            //排序
            var sort = new Binding("Value", entity, "SortOrder", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            sort.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            sort.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            txtSortOrder.DataBindings.Add(sort);

            txtInputType.DataBindings.Add("Text", entity, "InputType", false, DataSourceUpdateMode.OnValidation);
        }


        private void btnAddOptionValue_Click(object sender, EventArgs e)
        {
            tb_ProdPropertyValue entity = bindingSourcePropertyValue.AddNew() as tb_ProdPropertyValue;
            entity.actionStatus = ActionStatus.新增;
            BindData属性值(entity);
            ControlBtnEnable4PropValue(BtnAction.add);
        }


        public async void BindData属性值(tb_ProdPropertyValue entity)
        {
            txtPropertyValueName.DataBindings.Clear();
            txtPropertyValueDesc.DataBindings.Clear();
            txtValueSort.DataBindings.Clear();
            txtPropertyValueName.Text = "";
            txtPropertyValueDesc.Text = "";
            txtValueSort.Text = "0";

            //InitCmbProperty();
            cmbOption.DataBindings.Clear();
            //这个初始的方法使用了异步不能再放到另一个异步方法中。否则会绑定cmb互动失效
            BindingSource bs = new BindingSource();
            bs.DataSource = await controllerProp.QueryAsync();
            ComboBoxHelper.InitDropList(bs, cmbOption, "Property_ID", "PropertyName", ComboBoxStyle.DropDownList, false, true);

            var depa = new Binding("SelectedValue", entity, "Property_ID", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbOption.DataBindings.Add(depa);
            txtPropertyValueName.DataBindings.Add("Text", entity, "PropertyValueName", false, DataSourceUpdateMode.OnValidation);
            txtPropertyValueDesc.DataBindings.Add("Text", entity, "PropertyValueDesc", false, DataSourceUpdateMode.OnValidation);

            //排序
            var sort = new Binding("Value", entity, "SortOrder", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            sort.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            sort.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            txtValueSort.DataBindings.Add(sort);



        }

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
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return results.IsValid;
        }

        private void dataGridView属性值_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView属性值.CurrentRow != null)
            {
                bindingSourcePropertyValue.CancelEdit();
                tb_ProdPropertyValue entity = new tb_ProdPropertyValue();
                if (dataGridView属性值.CurrentRow == null)
                {
                    return;
                }
                entity = dataGridView属性值.CurrentRow.DataBoundItem as tb_ProdPropertyValue;
                entity.actionStatus = ActionStatus.修改;
                BindData属性值(entity);
                ControlBtnEnable4PropValue(BtnAction.edit);
                cmbOption.SelectedValue = entity.Property_ID;
            }
        }

        private void dataGridView属性_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView属性.CurrentRow != null)
            {
                //if (btnAddOption.Enabled==false)
                //{
                //    MessageBox.Show("正在添加时，不能编辑。");
                //    return;
                //}

                bindingSourceProperty.CancelEdit();
                if (dataGridView属性.CurrentRow == null)
                {
                    return;
                }
                tb_ProdProperty entity = new tb_ProdProperty();
                entity = dataGridView属性.CurrentRow.DataBoundItem as tb_ProdProperty;
                entity.actionStatus = ActionStatus.修改;
                BindData属性(entity);
                ControlBtnEnable4Prop(BtnAction.edit);
                cmbOption.SelectedValue = entity.Property_ID;
                QueryPropertyValue(entity.Property_ID);
            }
        }

        private void btnCancelProp_Click(object sender, EventArgs e)
        {
            bindingSourceProperty.CancelEdit();
            ControlBtnEnable4Prop(BtnAction.cancel);
            if (bindingSourceProperty.Current == null)
            {
                return;
            }
            tb_ProdProperty entity = bindingSourceProperty.Current as tb_ProdProperty;
            entity.actionStatus = ActionStatus.无操作;

        }

        private void btnCancelValue_Click(object sender, EventArgs e)
        {
            bindingSourcePropertyValue.CancelEdit();
            ControlBtnEnable4PropValue(BtnAction.cancel);
            if (bindingSourcePropertyValue.Current == null)
            {
                return;
            }
            tb_ProdPropertyValue entity = bindingSourcePropertyValue.Current as tb_ProdPropertyValue;
            entity.actionStatus = ActionStatus.无操作;


        }

        private void dataGridView属性值_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!dataGridView属性值.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = dataGridView属性值.Columns[e.ColumnIndex].Name;
            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProdPropertyValue).Name, colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }
            //图片特殊处理
            if (dataGridView属性值.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }
        }
    }
}
