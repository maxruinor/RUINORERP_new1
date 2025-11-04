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
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using AutoMapper;
using Castle.Core.Resource;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;
using RUINORERP.UI.BI;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.SysConfig;

using FastReport.Table;
using MathNet.Numerics.Optimization;
using NPOI.SS.Formula.Functions;
using RUINOR.Core;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.StatusManagerService;


namespace RUINORERP.UI.EOP
{

    /// <summary>
    /// 蓄水登记， 
    /// </summary>
    [MenuAttrAssemblyInfo("蓄水登记", ModuleMenuDefine.模块定义.电商运营, ModuleMenuDefine.电商运营.蓄水管理, BizType.蓄水订单)]
    public partial class UCEOPWaterStorage : BaseBillEditGeneric<tb_EOP_WaterStorage, tb_EOP_WaterStorage>
    {
        public UCEOPWaterStorage()
        {
            InitializeComponent();
            // usedActionStatus = true;

        }

        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.结案);
        }

        /// <summary>
        /// 收付款方式决定对应的菜单功能
        /// </summary>
        public ReceivePaymentType PaymentType { get; set; }

        //草稿 → 已审核 → 部分生效 → 全部生效 或 草稿 → 已冲销
        public override void BindData(tb_EOP_WaterStorage entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            EditEntity = entity;
            if (entity == null)
            {
                return;
            }
            EditEntity = entity as tb_EOP_WaterStorage;

            if (EditEntity.WSR_ID == 0)
            {
                EditEntity.ActionStatus = ActionStatus.新增;
                if (string.IsNullOrEmpty(EditEntity.WSRNo))
                {
                    //_EditEntity.WSRNo = BizCodeService.GetBizBillNo(BizType.销售订单);
                    EditEntity.WSRNo = "WSR" + DateTime.Now.ToString("yyMMddHHmmssfff");
                }
                EditEntity.OrderDate = DateTime.Now;
                EditEntity.PlatformType = (int)PlatformType.阿里1688;
                //第一次建的时候 应该是业务建的。分配给本人
                EditEntity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
            }
            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);

            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.WSRNo, txtWSRNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_EOP_WaterStorage>(entity, t => t.PlatformType, cmbPlatformType, BindDataType4Enum.EnumName, typeof(Global.PlatformType));
            DataBindingHelper.BindData4CmbByEnum<tb_EOP_WaterStorage, PlatformType>(entity, k => k.PlatformType, cmbPlatformType, false);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.PlatformFeeAmount.ToString(), txtPlatformFeeAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_EOP_WaterStorage>(entity, t => t.OrderDate, dtpOrderDate, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorage>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);


            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);


            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_EOP_WaterStorage>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_EOP_WaterStorage>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, entity);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_EOP_WaterStorageValidator>(), kryptonPanel1.Controls);
            }

            entity.PropertyChanged += (sender, s2) =>
            {
                //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {


                }
            };

            base.BindData(entity);

        }


        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtTotalAmount);

            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {

                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalAmount == 0)
                {
                    System.Windows.Forms.MessageBox.Show($"总金额不能为零，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }



                ReturnMainSubResults<tb_EOP_WaterStorage> SaveResult = new ReturnMainSubResults<tb_EOP_WaterStorage>();
                if (NeedValidated && EditEntity.HasChanged)
                {
                    //保存图片
                    #region 

                    if (ReflectionHelper.ExistPropertyName<tb_EOP_WaterStorage>(nameof(EditEntity.RowImage)) && EditEntity.RowImage != null)
                    {
                        if (EditEntity.RowImage.image != null)
                        {
                            //if (!EditEntity.RowImage.oldhash.Equals(EditEntity.RowImage.newhash, StringComparison.OrdinalIgnoreCase)
                            // && EditEntity.PaymentImagePath == EditEntity.RowImage.ImageFullName)
                            //{
                            //    HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                            //    //如果服务器有旧文件 。可以先删除
                            //    if (!string.IsNullOrEmpty(EditEntity.RowImage.oldhash))
                            //    {
                            //        string oldfileName = EditEntity.RowImage.Dir + EditEntity.RowImage.realName + "-" + EditEntity.RowImage.oldhash;
                            //        string deleteRsult = await httpWebService.DeleteImageAsync(oldfileName, "delete123");
                            //        MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
                            //    }
                            //    string newfileName = EditEntity.RowImage.GetUploadfileName();
                            //    ////上传新文件时要加后缀名
                            //    string uploadRsult = await httpWebService.UploadImageAsync(newfileName + ".jpg", EditEntity.RowImage.ImageBytes, "upload");
                            //    if (uploadRsult.Contains("UploadSuccessful"))
                            //    {
                            //        //重要
                            //        EditEntity.RowImage.ImageFullName = EditEntity.RowImage.UpdateImageName(EditEntity.RowImage.newhash);
                            //        EditEntity.PaymentImagePath = EditEntity.RowImage.ImageFullName;

                            //        //成功后。旧文件名部分要和上传成功后新文件名部分一致。后面修改只修改新文件名部分。再对比
                            //        MainForm.Instance.PrintInfoLog("UploadSuccessful for base List:" + newfileName);
                            //    }
                            //    else
                            //    {
                            //        MainForm.Instance.LoginWebServer();
                            //    }
                            //}
                        }
                    }
                    #endregion
                    //保存路径

                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        EditEntity.AcceptChanges();
                        MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_EOP_WaterStorage>("保存", EditEntity);
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.WSRNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;
            }
            return false;
        }


        private void UCCustomerVendorEdit_Load(object sender, EventArgs e)
        {

        }


    }
}
