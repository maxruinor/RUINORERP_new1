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
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.UI.SysConfig;
using System.Diagnostics;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.BI;
using Netron.GraphLib;
using RUINORERP.Business.Processor;
using SqlSugar;
using AutoMapper;
using RUINORERP.Global;
using RUINORERP.Business.AutoMapper;

namespace RUINORERP.UI.CRM
{
    [MenuAttrAssemblyInfo("目标客户编辑", true, UIType.单表数据)]
    public partial class UCCRMCustomerEdit : BaseEditGeneric<tb_CRM_Customer>
    {
        public UCCRMCustomerEdit()
        {
            InitializeComponent();
            usedActionStatus = true;
        }

        private tb_CRM_Customer _EditEntity;
        public tb_CRM_Customer EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public async override void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {

            tb_CRM_Customer customer = entity as tb_CRM_Customer;
            if (customer.Customer_id == 0)
            {
                //第一次建的时候 应该是业务建的。分配给本人
                customer.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                customer.CustomerStatus = (int)CustomerStatus.新增客户;
                customer.CustomerLevel = 1;
            }

            cmbCustomerStatus.Enabled = false;
            _EditEntity = customer;

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_CRM_Leads>(entity, k => k.LeadID, v => v.CustomerName, cmbLeadID, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID);
            DataBindingHelper.BindData4Cmb<tb_CRM_Region>(entity, k => k.Region_ID, v => v.Region_Name, cmbRegion_ID);
            DataBindingHelper.BindData4Cmb<tb_Provinces>(entity, k => k.ProvinceID, v => v.ProvinceCNName, cmbProvinceID);
            DataBindingHelper.BindData4Cmb<tb_Cities>(entity, k => k.CityID, v => v.CityCNName, cmbCityID);

            DataBindingHelper.BindData4CmbByEnum<tb_CRM_Customer>(entity, k => k.CustomerStatus, typeof(CustomerStatus), cmbCustomerStatus, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Contact_Phone, txtContact_Phone, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Contact_Email, txtContact_Email, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Contact_Name, txtContact_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerName, txtCustomerName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerAddress, txtCustomerAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_CRM_Customer>(entity, t => t.RepeatCustomer, chkRepeatCustomer, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerTags, txtCustomerTags, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.wwSocialTools, txtwwSocialTools, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.SocialTools, txtSocialTools, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.GetCustomerSource, txtGetCustomerSource, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.SalePlatform, txtSalePlatform, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.PurchaseCount, txtPurchaseCount, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.TotalPurchaseAmount.ToString(), txtTotalPurchaseAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.DaysSinceLastPurchase, txtDaysSinceLastPurchase, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4DataTime<tb_CRM_Customer>(entity, t => t.LastPurchaseDate, dtpLastPurchaseDate, false);
            DataBindingHelper.BindData4DataTime<tb_CRM_Customer>(entity, t => t.FirstPurchaseDate, dtpFirstPurchaseDate, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            txtCustomerLevel.DataBindings.Clear();
            //级别1到10
            txtCustomerLevel.Value = 0;
            var sort = new Binding("Value", entity, "CustomerLevel", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            sort.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            sort.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            txtCustomerLevel.DataBindings.Add(sort);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_CRM_CustomerValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            if (customer.Customer_id > 0)
            {
                btnFastFollowUp.Visible = true;
            }
            else
            {
                btnFastFollowUp.Visible = false;
            }
            entity.PropertyChanged += async (sender, s2) =>
            {
                //如果线索引入相关数据
                if ((customer.ActionStatus == ActionStatus.新增 || customer.ActionStatus == ActionStatus.修改) && customer.LeadID.HasValue && customer.LeadID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_CRM_Customer>(c => c.LeadID))
                {
                    await ToCustomer(customer, customer.LeadID.Value);
                }
            };

            if (CRMConfig == null)
            {
                CRMConfig = await MainForm.Instance.AppContext.Db.Queryable<tb_CRMConfig>().FirstAsync();
                if (CRMConfig != null && CRMConfig.CS_UseLeadsFunction)
                {
                    tb_CRM_LeadsController<tb_CRM_Leads> cvctr = Startup.GetFromFac<tb_CRM_LeadsController<tb_CRM_Leads>>();
                    //创建表达式
                    var lambda = Expressionable.Create<tb_CRM_Leads>()
                                    //.And(t => t.IsCustomer == true)//转过的不再转？
                                    .And(t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                                    .ToExpression();//注意 这一句 不能少
                    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Leads).Name + "Processor");
                    QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                    queryFilterC.FilterLimitExpressions.Add(lambda);
                    DataBindingHelper.InitFilterForControlByExp<tb_CRM_Leads>(entity, cmbLeadID, c => c.CustomerName, queryFilterC);

                }
            }

            base.BindData(entity);
        }


        //===

        private async Task<tb_CRM_Customer> ToCustomer(tb_CRM_Customer entity, long refId)
        {
            tb_CRM_Leads crmLeads;
            ButtonSpecAny bsa = cmbLeadID.ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return null;
            }
            //saleorder = bsa.Tag as tb_SaleOrder;

            crmLeads = await MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Leads>()
            .Where(c => c.LeadID == refId)
            .SingleAsync();

            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            mapper.Map(crmLeads, entity);  // 直接将 crmLeads 的值映射到传入的 entity 对象上，保持了引用
           // entity = mapper.Map<tb_CRM_Customer>(crmLeads);//这个是直接重新生成了对象。
            entity.ActionStatus = ActionStatus.新增;

            List<string> tipsMsg = new List<string>();

            StringBuilder msg = new StringBuilder();
            foreach (var item in tipsMsg)
            {
                msg.Append(item).Append("\r\n");
            }
            if (tipsMsg.Count > 0)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            BusinessHelper.Instance.InitEntity(entity);
            return entity;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator(EditEntity))
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        tb_CRMConfig CRMConfig = null;
        private async void UCLeadsEdit_Load(object sender, EventArgs e)
        {
            // tb_CRMConfigController<tb_CRMConfig> ctr = Startup.GetFromFac<tb_CRMConfigController<tb_CRMConfig>>();
            CRMConfig = await MainForm.Instance.AppContext.Db.Queryable<tb_CRMConfig>().FirstAsync();
            if (CRMConfig != null)
            {
                if (CRMConfig.CS_UseLeadsFunction)
                {
                    lblLeadID.Visible = true;
                    cmbLeadID.Visible = true;

                }
                else
                {
                    lblLeadID.Visible = false;
                    cmbLeadID.Visible = false;
                }
            }

            //通过动态配置得到。
            ConfigManager configManager = Startup.GetFromFac<ConfigManager>();


            //“|”号隔开
            string GetCustomerSource = configManager.GetValue("GetCustomerSource");
            //设置常用获客来源
            if (!string.IsNullOrEmpty(GetCustomerSource))
            {
                string[] GetCustomerSourceArr = GetCustomerSource.Split('|');
                AddCustomerSourceLabelsToPanel(GetCustomerSourceArr);
            }

            string[] enumStrings = Enum.GetNames(typeof(CustomerTags)).Select(x => x).ToArray();
            string combinedString = string.Join("|", enumStrings);
            //设置常用客户标签
            if (!string.IsNullOrEmpty(combinedString))
            {
                string[] CustomerTagsArr = combinedString.Split('|');
                AddCustomerTagsLabelsToPanel(CustomerTagsArr);
            }
        }
        #region 添加客户标签
        private void AddCustomerTagsLabelsToPanel(string[] CustomerTagsArr)
        {
            //在Panel容器内0为起点
            int x = 0;  // 起始 X 坐标
            int y = 0;  // 起始 Y 坐标

            foreach (var item in CustomerTagsArr)
            {
                KryptonLinkLabel klinklbl = new KryptonLinkLabel();
                klinklbl.Text = item;
                klinklbl.Name = "CustomerTags" + item;
                // 根据文本内容计算合适的大小
                using (Graphics g = kPanelCustomerTags.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(klinklbl.Text, klinklbl.Font);
                    klinklbl.Size = new Size((int)textSize.Width + 10, (int)textSize.Height + 5);
                }

                // 根据当前位置设置 Label 的位置
                klinklbl.Location = new Point(x, y);
                klinklbl.LinkClicked += KlinkTagslbl_LinkClicked;
                kPanelCustomerTags.Controls.Add(klinklbl);

                // 更新 X 坐标，以便下一个 Label 排列
                x += klinklbl.Width + 10;

                // 如果超出面板宽度，换行
                if (x + klinklbl.Width > kPanelCustomerTags.Width)
                {
                    x = 10;
                    y += klinklbl.Height + 10;
                }
            }
        }

        private void KlinkTagslbl_LinkClicked(object sender, EventArgs e)
        {
            KryptonLinkLabel klinklbl = sender as KryptonLinkLabel;
            //如果lblGetCustomerSource这个控件值中包含了标签值，则清除，如果没有包含再添加到值的集合里，用逗号隔开
            if (txtCustomerTags.Text.Contains(klinklbl.Text))
            {
                txtCustomerTags.Text = txtCustomerTags.Text.Replace(klinklbl.Text, "");

            }
            else
            {
                if (txtCustomerTags.Text.Length > 0)
                {
                    txtCustomerTags.Text = txtCustomerTags.Text + "," + klinklbl.Text;

                }
                else
                {
                    txtCustomerTags.Text = klinklbl.Text;
                }
            }
            //去掉前导后导,，全角变半角
            txtCustomerTags.Text = txtCustomerTags.Text.Replace(",,", ",");
            txtCustomerTags.Text = txtCustomerTags.Text.Replace("，,", ",");
            txtCustomerTags.Text = txtCustomerTags.Text.Replace(",，", ",");
            txtCustomerTags.Text = txtCustomerTags.Text.Replace("，，", ",");
            txtCustomerTags.Text = txtCustomerTags.Text.Trim(',');
            txtCustomerTags.Text = txtCustomerTags.Text.Trim('，');
            //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
        }


        #endregion

        #region 添加客户来源
        private void AddCustomerSourceLabelsToPanel(string[] GetCustomerSourceArr)
        {
            //在Panel容器内0为起点
            int x = 0;  // 起始 X 坐标
            int y = 0;  // 起始 Y 坐标

            foreach (var item in GetCustomerSourceArr)
            {
                KryptonLinkLabel klinklbl = new KryptonLinkLabel();
                klinklbl.Text = item;
                klinklbl.Name = "GetCustomerSource" + item;
                // 根据文本内容计算合适的大小
                using (Graphics g = kPanelGetCustomerSource.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(klinklbl.Text, klinklbl.Font);
                    klinklbl.Size = new Size((int)textSize.Width + 10, (int)textSize.Height + 5);
                }

                // 根据当前位置设置 Label 的位置
                klinklbl.Location = new Point(x, y);
                klinklbl.LinkClicked += Klinklbl_LinkClicked;
                kPanelGetCustomerSource.Controls.Add(klinklbl);

                // 更新 X 坐标，以便下一个 Label 排列
                x += klinklbl.Width + 10;

                // 如果超出面板宽度，换行
                if (x + klinklbl.Width > kPanelGetCustomerSource.Width)
                {
                    x = 10;
                    y += klinklbl.Height + 10;
                }
            }
        }

        private void Klinklbl_LinkClicked(object sender, EventArgs e)
        {
            KryptonLinkLabel klinklbl = sender as KryptonLinkLabel;
            //如果lblGetCustomerSource这个控件值中包含了标签值，则清除，如果没有包含再添加到值的集合里，用逗号隔开
            if (txtGetCustomerSource.Text.Contains(klinklbl.Text))
            {
                txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace(klinklbl.Text, "");

            }
            else
            {
                if (txtGetCustomerSource.Text.Length > 0)
                {
                    txtGetCustomerSource.Text = txtGetCustomerSource.Text + "," + klinklbl.Text;

                }
                else
                {
                    txtGetCustomerSource.Text = klinklbl.Text;
                }
            }
            //去掉前导后导,，全角变半角
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace(",,", ",");
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace("，,", ",");
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace(",，", ",");
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace("，，", ",");
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Trim(',');
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Trim('，');
            //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
        }


        #endregion

        private void btnAddContactInfo_Click(object sender, EventArgs e)
        {
            object frm = Activator.CreateInstance(typeof(UCCRMContactEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_CRM_Contact> frmaddg = frm as BaseEditGeneric<tb_CRM_Contact>;
                frmaddg.Text = "联系人编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_Contact>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_CRM_Contact ContactInfo = obj as tb_CRM_Contact;
                ContactInfo.Customer_id = _EditEntity.Customer_id;
                BaseEntity bty = ContactInfo as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                BusinessHelper.Instance.EditEntity(bty);
                frmaddg.BindData(bty);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    UIBizSrvice.SaveCRMContact(ContactInfo);
                }
            }
        }

        private async void btnFastFollowUp_Click(object sender, EventArgs e)
        {
            object frm = Activator.CreateInstance(typeof(UCCRMFollowUpRecordsEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_CRM_FollowUpRecords> frmaddg = frm as BaseEditGeneric<tb_CRM_FollowUpRecords>;
                frmaddg.Text = "跟进记录编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_FollowUpRecords>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_CRM_FollowUpRecords NewInfo = obj as tb_CRM_FollowUpRecords;
                NewInfo.Customer_id = _EditEntity.Customer_id;
                NewInfo.Customer_id = _EditEntity.Customer_id;
                NewInfo.Employee_ID = _EditEntity.Employee_ID.Value;
                BaseEntity bty = NewInfo as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                BusinessHelper.Instance.EditEntity(bty);
                frmaddg.BindData(bty, ActionStatus.新增);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    BaseController<tb_CRM_FollowUpRecords> ctr = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpRecords>>(typeof(tb_CRM_FollowUpRecords).Name + "Controller");
                    ReturnResults<tb_CRM_FollowUpRecords> result = await ctr.BaseSaveOrUpdate(NewInfo);
                }
            }
        }

        private void cmbLeadID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
