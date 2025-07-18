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
using RUINORERP.UI.Common;
using RUINORERP.Global;
using System.Numerics;

namespace RUINORERP.UI.EOP
{


    [MenuAttrAssemblyInfo("蓄水登记", true, UIType.单表数据)]
    public partial class UCEOPWaterStorageEdit : BaseEditGeneric<tb_EOP_WaterStorage>
    {
        public UCEOPWaterStorageEdit()
        {
            InitializeComponent();
        }

        private tb_EOP_WaterStorage _EditEntity;
        public tb_EOP_WaterStorage EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_EOP_WaterStorage;

            if (_EditEntity.WSR_ID == 0)
            {
                _EditEntity.ActionStatus = ActionStatus.新增;
                if (string.IsNullOrEmpty(_EditEntity.WSRNo))
                {
                    //_EditEntity.WSRNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售订单);
                    _EditEntity.WSRNo = "WSR" + DateTime.Now.ToString("yyMMddHHmmssfff");
                }
                _EditEntity.OrderDate = DateTime.Now;
                _EditEntity.PlatformType = (int)PlatformType.阿里1688;
                //第一次建的时候 应该是业务建的。分配给本人
                _EditEntity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
            }

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);

            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.WSRNo, txtWSRNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);

            //DataBindingHelper.BindData4ControlByEnum<tb_EOP_WaterStorage>(entity, t => t.PlatformType, cmbPlatformType, BindDataType4Enum.EnumName, typeof(Global.PlatformType));
            DataBindingHelper.BindData4CmbByEnum<tb_EOP_WaterStorage, PlatformType>(entity, k => k.PlatformType, cmbPlatformType, false);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.PlatformFeeAmount.ToString(), txtPlatformFeeAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_EOP_WaterStorage>(entity, t => t.OrderDate, dtpOrderDate, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_EOP_WaterStorageValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            base.BindData(entity);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();

                if (EditEntity.PlatformOrderNo != null)
                {
                    EditEntity.PlatformOrderNo = EditEntity.PlatformOrderNo.Trim();//去空格
                }
                if (!string.IsNullOrEmpty(EditEntity.PlatformOrderNo))
                {
                    //检测平台单号。重复性提示
                    var ctr = Startup.GetFromFac<tb_EOP_WaterStorageController<tb_EOP_WaterStorage>>();
                    tb_EOP_WaterStorage WaterStorage = ctr.ExistFieldValueWithReturn(c => c.PlatformOrderNo == txtPlatformOrderNo.Text.Trim());
                    if (WaterStorage != null)
                    {
                        string empName = UIHelper.ShowGridColumnsNameValue(typeof(tb_SaleOrder), "Employee_ID", WaterStorage.Employee_ID);
                        if (MessageBox.Show($"系统检测到相同平台单号的蓄水订单，订单号是：{WaterStorage.PlatformOrderNo},业务员是{empName}\r\n确定继续保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
