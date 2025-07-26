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
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global;
using RUINORERP.UI.UCSourceGrid;
using static OpenTK.Graphics.OpenGL.GL;
using RUINORERP.Model.ReminderModel.ReminderRules;

namespace RUINORERP.UI.SmartReminderClient
{
    [MenuAttrAssemblyInfo("单据审核配置编辑", true, UIType.单表数据)]
    public partial class UCDocumentApprovalConfigEdit : BaseEditGeneric<DocApprovalConfig>
    {
        public UCDocumentApprovalConfigEdit()
        {
            InitializeComponent();
        }


        public DocApprovalConfig DocumentApprovalConfig { get; set; }

        public void BindData(DocApprovalConfig entity)
        {
            if (entity == null)
            {
                entity = new DocApprovalConfig();
            }
            entity._DocumentTypeNames = DocumentApprovalConfig.DocumentTypes.ToJson();
            DataBindingHelper.BindData4TextBox<DocApprovalConfig>(entity, t => t._DocumentTypeNames, txt_ProductIds, BindDataType4TextBox.Text, false);
            txt_ProductIds.ReadOnly = true;//要选取，不能手输入。不然格式错误

            //DataBindingHelper.BindData4TextBox<DocumentApprovalConfig>(entity, t => t.MinStock, txtMinStock, BindDataType4TextBox.Qty, false);
            //DataBindingHelper.BindData4TextBox<DocumentApprovalConfig>(entity, t => t.MaxStock, txtMaxStock, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<DocApprovalConfig>(entity, t => t.IsEnabled, chkIs_enabled, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.EndEdit();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UCBoxRulesEdit_Load(object sender, EventArgs e)
        {
            BindData(DocumentApprovalConfig);
        }

        private void btnSeleted_Click(object sender, EventArgs e)
        {
            using (QueryFormGeneric dg = new QueryFormGeneric())
            {
                
                
            }
           
        }
    }
}
