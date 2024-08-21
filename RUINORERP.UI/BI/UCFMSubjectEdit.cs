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
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("科目编辑", true, UIType.单表数据)]
    public partial class UCFMSubjectEdit : BaseEditGeneric<tb_FM_Subject>
    {
        public UCFMSubjectEdit()
        {
            InitializeComponent();
            DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_FM_Subject>(typeof(SubjectType), e => e.Subject_Type, cmbSubject_Type, false);
        }

        private tb_FM_Subject _EditEntity;
        public tb_FM_Subject EditEntity { get => _EditEntity; set => _EditEntity = value; }

        List<tb_FM_Subject> list = new List<tb_FM_Subject>(0);
        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_FM_Subject;
            if (_EditEntity.subject_id == 0)
            {
                string 上级代码 = "1";
                if (_EditEntity.tb_fm_subject != null)
                {
                    上级代码 = _EditEntity.tb_fm_subject.subject_code;
                }
                _EditEntity.subject_code = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.FMSubject, 上级代码);
            }

            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_FM_Subject>(entity, t => t.Balance_direction, rdb贷, rdb借);
            DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_code, txtsubject_code, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_name, txtsubject_name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_en_name, txtsubject_en_name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_FM_Subject>(entity, k => k.Subject_Type, typeof(SubjectType), cmbSubject_Type, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_Subject>(entity, t => t.Is_enabled, chkIs_enabled, false);
            //有默认值
            DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            //父类
            var parent_subjet = new Binding("Text", entity, "parent_subject_id", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            parent_subjet.Format += new ConvertEventHandler(DataSourceToControl);
            //将控件的数据类型转换为数据源要求的数据类型。
            parent_subjet.Parse += new ConvertEventHandler(ControlToDataSource);
            cmbTreeParent_id.DataBindings.Add(parent_subjet);

            txtSort.Value = 0;
            //排序
            var sort = new Binding("Value", entity, "Sort", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            sort.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            sort.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            txtSort.DataBindings.Add(sort);

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
                tb_FM_Subject entity = list.Find(t => t.subject_id.ToString() == cevent.Value.ToString());
                if (entity != null)
                {
                    cevent.Value = entity.subject_name;
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
                tb_FM_Subject entity = list.Find(t => t.subject_name == cevent.Value.ToString());
                if (entity != null)
                {
                    cevent.Value = entity.subject_id;
                }
                else
                {
                    cevent.Value = 0;
                }
            }



        }

        //递归方法
        private void Bind(TreeNode parNode, List<tb_FM_Subject> list, long nodeId)
        {
            var childList = list.FindAll(t => t.parent_subject_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.subject_id.ToString();
                node.Text = nodeObj.subject_name;
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                Bind(node, list, nodeObj.subject_id);
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
            if (EditEntity.parent_subject_id == null)
            {
                EditEntity.parent_subject_id = 0;
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
            tb_FM_SubjectController<tb_FM_Subject> ctr = Startup.GetFromFac<tb_FM_SubjectController<tb_FM_Subject>>();
            list = await ctr.QueryAsync();
            Common.UIFMSubjectHelper.BindToTreeView(list, cmbTreeParent_id.TreeView);
        }
    }
}
