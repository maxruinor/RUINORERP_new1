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
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.CRM
{
    [MenuAttrAssemblyInfo("业务区域编辑", true, UIType.单表数据)]
    public partial class UCCRMRegionEdit : BaseEditGeneric<tb_CRM_Region>
    {
        public UCCRMRegionEdit()
        {
            InitializeComponent();
        }

        private tb_CRM_Region _EditEntity;
        public tb_CRM_Region EditEntity { get => _EditEntity; set => _EditEntity = value; }

        List<tb_CRM_Region> list = new List<tb_CRM_Region>(0);
        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_CRM_Region;
            if (_EditEntity.Region_ID == 0)
            {
                string 上级代码 = "1";
                if (_EditEntity.tb_crm_region != null)
                {
                    上级代码 = _EditEntity.tb_crm_region.Region_code;
                }
                _EditEntity.Region_code = BizCodeService.GetBaseInfoNo(BaseInfoType.CRM_RegionCode.ToString(), 上级代码);
            }

            DataBindingHelper.BindData4TextBox<tb_CRM_Region>(entity, t => t.Region_Name, txtRegion_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Region>(entity, t => t.Region_code, txtRegion_code, BindDataType4TextBox.Text, false);
          
            DataBindingHelper.BindData4CheckBox<tb_CRM_Region>(entity, t => t.Is_enabled, chkIs_enabled, false);
            //有默认值
            DataBindingHelper.BindData4TextBox<tb_CRM_Region>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            //父类
            var Parent_region_id = new Binding("Text", entity, "Parent_region_id", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            Parent_region_id.Format += new ConvertEventHandler(DataSourceToControl);
            //将控件的数据类型转换为数据源要求的数据类型。
            Parent_region_id.Parse += new ConvertEventHandler(ControlToDataSource);
            cmbTreeParent_id.DataBindings.Add(Parent_region_id);

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
                cevent.Value = "区域根结节";
            }
            else
            {
                //显示名称
                tb_CRM_Region entity = list.Find(t => t.Region_ID.ToString() == cevent.Value.ToString());
                if (entity != null)
                {
                    cevent.Value = entity.Region_Name;
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
            if (string.IsNullOrEmpty(cevent.Value.ToString()) || cevent.Value.ToString() == "区域根结节")
            {
                cevent.Value = 0;
            }
            else
            {
                tb_CRM_Region entity = list.Find(t => t.Region_Name == cevent.Value.ToString());
                if (entity != null)
                {
                    cevent.Value = entity.Region_ID;
                }
                else
                {
                    cevent.Value = 0;
                }
            }



        }

        //递归方法
        private void Bind(TreeNode parNode, List<tb_CRM_Region> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_region_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Region_ID.ToString();
                node.Text = nodeObj.Region_Name;
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                Bind(node, list, nodeObj.Region_ID);
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
            if (EditEntity.Parent_region_id == null)
            {
                EditEntity.Parent_region_id = 0;
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
            tb_CRM_RegionController<tb_CRM_Region> ctr = Startup.GetFromFac<tb_CRM_RegionController<tb_CRM_Region>>();
            list = await ctr.QueryAsync();
            Common.UICRMRegionHelper.BindToTreeView(list, cmbTreeParent_id.TreeView);
        }
    }
}
