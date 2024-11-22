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
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.UI.UCSourceGrid;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Common.CollectionExtension;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using RUINORERP.Model.Dto;
using DevAge.Windows.Forms;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.UI.Report;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.AdvancedUIModule;
using Krypton.Navigator;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using System.Collections;
using TransInstruction;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Model.CommonModel;
using FluentValidation;
using FluentValidation.Results;
using Krypton.Toolkit;
using System.IO;
using System.Diagnostics;
using SqlSugar;
using RUINORERP.Business.Processor;
using ExCSS;
using RUINORERP.Business.CommService;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using MySqlX.XDevAPI.Common;
using RUINORERP.Business.Security;
using RUINORERP.UI.CommonUI;
using ImageHelper = RUINORERP.UI.Common.ImageHelper;
using Netron.GraphLib;
using Newtonsoft.Json;
using RUINORERP.UI.SS;
using MathNet.Numerics.LinearAlgebra.Factorization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using SourceGrid;
using RUINORERP.UI.FormProperty;
using SourceGrid.Cells.Models;
using FastReport.Table;
using FastReport.DevComponents.AdvTree;
using Newtonsoft.Json.Linq;
using System.Web.Caching;
using Microsoft.Extensions.Caching.Memory;



namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 单据类型的编辑 主表T子表C
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseBillEditGeneric<T, C> : BaseBillEdit where T : class where C : class
    {
        public BaseBillEditGeneric()
        {
            InitializeComponent();

            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    QueryConditionBuilder();

                }
            }
            this.OnBindDataToUIEvent += BindData;

            KryptonButton button保存当前单据 = new KryptonButton();
            button保存当前单据.Text = "保存当前单据";
            button保存当前单据.Click += button保存当前单据_Click;
            frm.flowLayoutPanelButtonsArea.Controls.Add(button保存当前单据);

            KryptonContextMenu kcm加载最新数据 = new KryptonContextMenu();
            KryptonContextMenuItem menuItem选择要加载的数据 = new KryptonContextMenuItem("选择数据");
            menuItem选择要加载的数据.Text = "选择数据";
            menuItem选择要加载的数据.Click += MenuItem选择要加载的数据_Click;

            KryptonContextMenuItems kryptonContextMenuItems1 = new KryptonContextMenuItems();

            kcm加载最新数据.Items.AddRange(new KryptonContextMenuItemBase[] {
            kryptonContextMenuItems1});

            kryptonContextMenuItems1.Items.AddRange(new KryptonContextMenuItemBase[] {
            menuItem选择要加载的数据});

            KryptonDropButton button加载最新数据 = new KryptonDropButton();
            button加载最新数据.Text = "加载数据";
            button加载最新数据.Click += button加载最新数据_Click;
            button加载最新数据.KryptonContextMenu = kcm加载最新数据;
            frm.flowLayoutPanelButtonsArea.Controls.Add(button加载最新数据);

            KryptonButton button快速录入数据 = new KryptonButton();

            button快速录入数据.Text = "快速录入数据";
            button快速录入数据.Click += button快速录入数据_Click;

            frm.flowLayoutPanelButtonsArea.Controls.Add(button快速录入数据);

            KryptonButton button请求协助处理 = new KryptonButton();
            button请求协助处理.Text = "请求协助处理";
            button请求协助处理.Click += button请求协助处理_Click;

            frm.flowLayoutPanelButtonsArea.Controls.Add(button请求协助处理);



        }



        private void MenuItem选择要加载的数据_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "cache files (*.cache)|*.cache|All files (*.*)|*.*";
            //加载最新数据
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                EditEntity = manager.Deserialize<T>(openFileDialog1.FileName);
                OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
                MainForm.Instance.uclog.AddLog("成功加载选择的数据。");
            }
        }

        private async void button请求协助处理_Click(object sender, EventArgs e)
        {
            //弹出一个弹出框，让用户输入协助处理的内容。
            //再把单据相关内容发送到服务器转发到管理员那里

            frmInputContent frm = new frmInputContent();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //发送协助处理请求
                //先获取当前单据的ID
                #region
                try
                {
                    if (EditEntity != null)
                    {
                        #region  单据数据  后面优化可以多个单?限制5个？
                        await Save(false);
                        string json = JsonConvert.SerializeObject(EditEntity,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                            });
                        OriginalData odforCache = ActionForClient.请求协助处理(MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID,
                          MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name, frm.Content, json, typeof(T).Name);
                        byte[] buffer = TransInstruction.CryptoProtocol.EncryptClientPackToServer(odforCache);
                        MainForm.Instance.ecs.client.Send(buffer);
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "请求协助处理");
                }
                #endregion
            }

        }

        private void button快速录入数据_Click(object sender, EventArgs e)
        {
            frm.Close();
            frmQuicklyInputGeneric<C> frmQuicklyInput = new frmQuicklyInputGeneric<C>();
            frmQuicklyInput.OnApplyQuicklyInputData += OnApplyQuicklyInputData;
            frmQuicklyInput.CurMenuInfo = CurMenuInfo;
            if (EditEntity == null)
            {
                Add();
            }
            var details = EditEntity.GetPropertyValue(typeof(C).Name + "s");
            if (details == null)
            {
                details = new List<C>();
            }
            frmQuicklyInput.lines = details as List<C>;
            if (frmQuicklyInput.ShowDialog() == DialogResult.OK)
            {
                EditEntity.SetPropertyValue(typeof(C).Name + "s", frmQuicklyInput.lines);
                OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
            }
        }

        private void OnApplyQuicklyInputData(List<C> lines)
        {
            EditEntity.SetPropertyValue(typeof(C).Name + "s", lines);
            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
        }

        RUINORERP.Common.Helper.XmlHelper manager = new RUINORERP.Common.Helper.XmlHelper();
        private void button加载最新数据_Click(object sender, EventArgs e)
        {
            //RUINORERP.Common.Helper.XmlHelper manager = new RUINORERP.Common.Helper.XmlHelper();
            EditEntity = manager.Deserialize<T>(CurMenuInfo.CaptionCN + ".cache");
            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
            MainForm.Instance.uclog.AddLog("成功加载上次的数据。");
        }

        private async void button保存当前单据_Click(object sender, EventArgs e)
        {
            await AutoSaveDataAsync();
        }


        /// <summary>
        /// 绑定数据到UI
        /// </summary>
        /// <param name="entity"></param>
        public virtual void BindData(T entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            ToolBarEnabledControl(entity);

            //如果单据被锁定，则不能修改
        }

        protected override void ToolBarEnabledControl(object entity)
        {
            base.ToolBarEnabledControl(entity);
            ControlMasterColumnsInvisible();
        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as T);
            ToolBarEnabledControl(Entity);
        }

        /// <summary>
        /// 控制字段是否显示，添加到里面的是不显示的
        /// </summary>
        /// <param name="InvisibleCols"></param>
        public void ControlChildColumnsInvisible(List<SourceGridDefineColumnItem> listCols)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Fields != null)
                {
                    foreach (var item in CurMenuInfo.tb_P4Fields)
                    {
                        if (item != null)
                        {
                            if (item.tb_fieldinfo != null)
                            {
                                //设置不可见
                                if (!item.IsVisble && item.tb_fieldinfo.IsChild)
                                {
                                    SourceGridDefineColumnItem defineColumnItem = listCols.Where(w => w.ColName == item.tb_fieldinfo.FieldName).FirstOrDefault();
                                    if (defineColumnItem != null)
                                    {
                                        defineColumnItem.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                                    }
                                }
                                //设置默认隐藏
                                if (item.HideValue && item.tb_fieldinfo.IsChild)
                                {
                                    SourceGridDefineColumnItem defineColumnItem = listCols.Where(w => w.ColName == item.tb_fieldinfo.FieldName).FirstOrDefault();
                                    if (defineColumnItem != null)
                                    {
                                        defineColumnItem.SetCol_DefaultHide(item.tb_fieldinfo.FieldName);
                                    }

                                }
                            }
                        }

                    }

                }
            }
        }


        /// <summary>
        /// 设置主表字段是否显示
        /// </summary>
        /// <param name="kryptonPanelMainInfo"></param>
        public void ControlMasterColumnsInvisible()
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Fields != null)
                {
                    foreach (var item in CurMenuInfo.tb_P4Fields)
                    {
                        if (item != null)
                        {
                            if (item.tb_fieldinfo != null)
                            {
                                //设置不可见
                                if (!item.IsVisble && !item.tb_fieldinfo.IsChild)
                                {
                                    KryptonTextBox txtTextBox = FindTextBox(this, item.tb_fieldinfo.FieldName);
                                    if (txtTextBox != null)
                                    {
                                        txtTextBox.Visible = false;
                                    }
                                    KryptonLabel lbl = FindLabel(this, item.tb_fieldinfo.FieldText, item.tb_fieldinfo.FieldName);
                                    if (lbl != null)
                                    {
                                        lbl.Visible = false;
                                    }
                                }


                            }
                        }
                    }
                }
            }
        }

        #region 查找控件

        private KryptonTextBox FindTextBox(Control parentControl, string dataMember)
        {
            // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
            if (parentControl is KryptonTextBox textBox)
            {
                if (textBox.DataBindings.Count > 0)
                {
                    foreach (Binding binding in textBox.DataBindings)
                    {
                        if (binding.BindingMemberInfo.BindingMember == dataMember)
                        {
                            return textBox;
                        }
                    }
                }
                else
                {
                    //按名称来查找
                    if (textBox.Name == "txt" + dataMember)
                    {
                        return textBox;
                    }
                }
            }

            // 递归检查所有子控件
            KryptonTextBox foundTextBox = FindTextBoxInControls(parentControl.Controls, dataMember);
            if (foundTextBox != null)
            {
                return foundTextBox;
            }

            return null;
        }

        private KryptonTextBox FindTextBoxInControls(Control.ControlCollection controls, string dataMember)
        {
            foreach (Control control in controls)
            {
                // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
                if (control is KryptonTextBox textBox)
                {
                    if (textBox.DataBindings.Count > 0)
                    {
                        foreach (Binding binding in textBox.DataBindings)
                        {
                            if (binding.BindingMemberInfo.BindingMember == dataMember)
                            {
                                return textBox;
                            }
                        }
                    }
                    else
                    {
                        //按名称来查找
                        if (textBox.Name == "txt" + dataMember)
                        {
                            return textBox;
                        }
                    }
                }

                // 如果当前控件有子控件，继续递归检查
                if (control.HasChildren)
                {
                    KryptonTextBox foundTextBox = FindTextBoxInControls(control.Controls, dataMember);
                    if (foundTextBox != null)
                    {
                        return foundTextBox;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 显示的文字找不到就按名称的规则查找
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="LabelText"></param>
        /// <param name="LabelName"></param>
        /// <returns></returns>
        private KryptonLabel FindLabel(Control parentControl, string LabelText, string LabelName)
        {
            // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
            if (parentControl is KryptonLabel lbl && lbl.Text.Length > 0)
            {
                if (lbl.Text == LabelText)
                {
                    return lbl;
                }
                if (lbl.Name == "lbl" + LabelName)
                {
                    return lbl;
                }
            }
            // 递归检查所有子控件
            KryptonLabel foundLabel = FindLabelInControls(parentControl.Controls, LabelText, LabelName);
            if (foundLabel != null)
            {
                return foundLabel;
            }

            return null;
        }

        private KryptonLabel FindLabelInControls(Control.ControlCollection controls, string LabelText, string LabelName)
        {
            foreach (Control control in controls)
            {
                // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
                if (control is KryptonLabel lbl && lbl.Text.Length > 0)
                {
                    if (lbl.Text == LabelText)
                    {
                        return lbl;
                    }
                    if (lbl.Name == "lbl" + LabelName)
                    {
                        return lbl;
                    }
                }

                // 如果当前控件有子控件，继续递归检查
                if (control.HasChildren)
                {
                    KryptonLabel foundTextBox = FindLabelInControls(control.Controls, LabelText, LabelName);
                    if (foundTextBox != null)
                    {
                        return foundTextBox;
                    }
                }
            }
            return null;
        }


        #endregion

        public virtual void ShowPrintStatus(KryptonLabel lblPrintStatus, BaseEntity entity)
        {
            //可以修改
            if (entity.ContainsProperty(typeof(PrintStatus).Name))
            {
                PrintStatus printStatus = (PrintStatus)int.Parse(entity.GetPropertyValue(typeof(PrintStatus).Name).ToString());
                switch (printStatus)
                {
                    case PrintStatus.未打印:
                        lblPrintStatus.Text = "未打印";
                        break;
                    default:
                        // lblPrintStatus.Text = "已打印";
                        lblPrintStatus.Text = $"打印{entity.GetPropertyValue(typeof(PrintStatus).Name).ToString()}次";
                        break;
                }
            }
        }

        /*
        /// <summary>
        /// 控制字段是否为默认隐藏
        /// </summary>
        /// <param name="InvisibleCols"></param>
        public void ControlColumnsDefaultHide(List<SourceGridDefineColumnItem> listCols)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Fields != null)
                {
                    foreach (var item in CurMenuInfo.tb_P4Fields)
                    {
                        if (item != null)
                        {
                            if (item.tb_fieldinfo != null)
                            {
                                if (item.HideValue && item.tb_fieldinfo.IsChild)
                                {
                                    SourceGridDefineColumnItem defineColumnItem = listCols.Where(w => w.ColName == item.tb_fieldinfo.FieldName).FirstOrDefault();
                                    if (defineColumnItem != null)
                                    {
                                        defineColumnItem.SetCol_DefaultHide(item.tb_fieldinfo.FieldName);
                                    }

                                }
                            }
                        }

                    }

                }
            }
        }
        */

        #region 帮助信息提示


        public void InitHelpInfoToControl(System.Windows.Forms.Control.ControlCollection Controls)
        {
            foreach (var item in Controls)
            {
                if (item is Control)
                {
                    if (item.GetType().Name == "KryptonTextBox")
                    {
                        KryptonTextBox ktb = item as KryptonTextBox;
                        if ((item as Control).DataBindings.Count > 0)
                        {
                            if (GetHelpInfoByBinding(ktb.DataBindings).Length > 0)
                            {
                                ButtonSpecAny bsa = new ButtonSpecAny();
                                bsa.Image = System.Drawing.Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                bsa.Tag = ktb;
                                bsa.Click += Bsa_Click;
                                ktb.ButtonSpecs.Add(bsa);

                                //可以边框为红色不？
                                //或必填项目有特别提示？
                            }
                        }
                    }
                }

            }
        }





        private void Bsa_Click(object sender, EventArgs e)
        {
            ProcessHelpInfo(true, sender);
        }




        public new void ProcessHelpInfo(bool fromBtn, object sender)
        {
            string tipTxt = string.Empty;
            //时长timeout默认值设置的是3000ms(也就是3秒)
            int timeout = 3000;
            _timeoutTimer4tips = new System.Threading.Timer(OnTimerElapsed, null, timeout, System.Threading.Timeout.Infinite);
            toolTipBase.Hide(this);
            if (fromBtn)
            {
                ButtonSpecAny bsa = sender as ButtonSpecAny;
                tipTxt = GetHelpInfoByBinding((bsa.Owner as KryptonTextBox).DataBindings);
                Control ctl = bsa.Owner as Control;
                if (string.IsNullOrEmpty(tipTxt))
                {
                    return;
                }
                toolTipBase.SetToolTip(ctl, tipTxt);
                toolTipBase.Show(tipTxt, ctl);
            }
            else
            {
                #region F1
                if (ActiveControl.GetType().ToString() == "Krypton.Toolkit.KryptonTextBox+InternalTextBox")
                {
                    KryptonTextBox txt = ActiveControl.Parent as KryptonTextBox;
                    tipTxt = GetHelpInfoByBinding(txt.DataBindings);
                    //if (txt.DataBindings.Count > 0)
                    //{
                    //    string filedName = txt.DataBindings[0].BindingMemberInfo.BindingField;
                    //    string[] cns = txt.DataBindings[0].BindingManagerBase.Current.ToString().Split('.');
                    //    string className = cns[cns.Length - 1];
                    //    var obj = Startup.AutoFacContainer.ResolveNamed<BaseEntity>(className);
                    //    if (obj.HelpInfos.ContainsKey(filedName))
                    //    {
                    //        tipTxt = "【" + obj.FieldNameList.Find(f => f.Key == filedName).Value.Trim() + "】";
                    //        tipTxt += obj.HelpInfos[filedName].ToString();
                    //    }

                    //}
                }
                else
                {

                }
                if (string.IsNullOrEmpty(tipTxt))
                {
                    return;
                }
                toolTipBase.SetToolTip(ActiveControl, tipTxt);
                toolTipBase.Show(tipTxt, ActiveControl);
                #endregion
            }





        }


        /// <summary>
        /// 获取帮助信息集合对应的值
        /// </summary>
        /// <param name="cbc"></param>
        /// <returns></returns>
        private string GetHelpInfoByBinding(ControlBindingsCollection cbc)
        {
            string tipTxt = string.Empty;
            if (cbc.Count > 0)
            {
                string filedName = cbc[0].BindingMemberInfo.BindingField;
                if (cbc[0].BindingManagerBase == null)
                {
                    return tipTxt;
                }
                string[] cns = cbc[0].BindingManagerBase.Current.ToString().Split('.');
                string className = cns[cns.Length - 1];

                var obj = Startup.GetFromFacByName<BaseEntity>(className);
                if (obj.HelpInfos != null)
                {
                    if (obj.HelpInfos.ContainsKey(filedName))
                    {
                        tipTxt = "【" + obj.FieldNameList[filedName].Trim() + "】";
                        tipTxt += obj.HelpInfos[filedName].ToString();
                    }
                }


            }
            return tipTxt;
        }



        public void OnTimerElapsed(object state)
        {
            toolTipBase.Hide(this);
            _timeoutTimer4tips.Dispose();

        }
        private void toolTipBase_Popup(object sender, PopupEventArgs e)
        {
            //ToolTip tool = (ToolTip)sender;
            //if (e.AssociatedControl.Name == "textBox1")//e代表我们要触发的事件，我们是在textBox1触发
            //{
            //    tool.ToolTipTitle = "提示信息";//修改标题
            //    tool.ToolTipIcon = ToolTipIcon.Info;//修改图标
            //}
            //else
            //{
            //    tool.ToolTipTitle = "";
            //    tool.ToolTipIcon = ToolTipIcon.Info;
            //}
        }
        private void timerForToolTip_Tick(object sender, EventArgs e)
        {

        }
        #endregion


        #region 特殊显示必填项


        /// <summary>
        /// 特殊显示必填项 
        /// </summary>
        /// <typeparam name="T">要验证必填的类型</typeparam>
        /// <param name="rules"></param>
        /// <param name="Controls"></param>
        public void InitRequiredToControl(AbstractValidator<T> rules, System.Windows.Forms.Control.ControlCollection Controls)
        {
            List<string> notEmptyList = new List<string>();
            List<string> checkList = new List<string>();
            foreach (var item in rules)
            {
                string colName = item.PropertyName;
                var rr = item.Components;
                foreach (var com in item.Components)
                {
                    if (com.Validator.Name == "NotEmptyValidator")
                    {
                        //这里找到了不能为空的验证器。为了体验在UI
                        notEmptyList.Add(colName);
                    }
                    else
                    if (com.Validator.Name == "PredicateValidator")
                    {
                        checkList.Add(colName);
                    }
                }
            }


            foreach (var item in Controls)
            {
                if (item is Control)
                {
                    if (item is VisualControlBase)
                    {
                        if (item.GetType().Name == "KryptonTextBox")
                        {
                            KryptonTextBox ktb = item as KryptonTextBox;
                            if ((item as Control).DataBindings.Count > 0)
                            {
                                #region 找到绑定的字段
                                if (ktb.DataBindings.Count > 0)
                                {
                                    string filedName = ktb.DataBindings[0].BindingMemberInfo.BindingField;
                                    string col = notEmptyList.FirstOrDefault(c => c == filedName);
                                    if (col.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.Border.Color1 = Color.FromArgb(255, 128, 128);
                                    }

                                    string colchk = checkList.FirstOrDefault(c => c == filedName);
                                    if (colchk.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.Border.Color1 = Color.FromArgb(255, 0, 204);
                                    }

                                }
                                #endregion


                            }
                        }
                        if (item.GetType().Name == "KryptonComboBox")
                        {
                            KryptonComboBox ktb = item as KryptonComboBox;
                            if ((item as Control).DataBindings.Count > 0)
                            {
                                //ButtonSpecAny bsa = new ButtonSpecAny();
                                // bsa.Image = Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                // bsa.Tag = ktb;
                                //bsa.Click += Bsa_Click;
                                // ktb.ButtonSpecs.Add(bsa);
                                // ktb.StateCommon.Border.Color1 =  Color.FromArgb(255, 128, 128);
                                //可以边框为红色不？
                                //或必填项目有特别提示？
                                #region 找到绑定的字段
                                if (ktb.DataBindings.Count > 0)
                                {
                                    string filedName = ktb.DataBindings[0].BindingMemberInfo.BindingField;
                                    string col = notEmptyList.FirstOrDefault(c => c == filedName);
                                    if (col.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(255, 128, 128);
                                    }
                                    string colchk = checkList.FirstOrDefault(c => c == filedName);
                                    if (colchk.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(255, 0, 204);
                                    }
                                }
                                #endregion


                            }
                        }
                    }
                }
            }
        }

        #endregion



        #region 基础资料下拉添加编辑项





        Expression<Func<TParent, bool>> Wrap<TParent, TElement>(Expression<Func<TParent, IEnumerable<TElement>>> collection, Expression<Func<TElement, bool>> isOne, Expression<Func<IEnumerable<TElement>, Func<TElement, bool>, bool>> isAny)
        {
            var parent = Expression.Parameter(typeof(TParent), "parent");

            return
                (Expression<Func<TParent, bool>>)Expression.Lambda
                (
                    Expression.Invoke
                    (
                        isAny,
                        Expression.Invoke
                        (
                            collection,
                            parent
                        ),
                        isOne
                    ),
                    parent
                );
        }


        #endregion

        public delegate void BindDataToUIHander(T entity, ActionStatus actionStatus);

        [Browsable(true), Description("绑定数据对象到UI")]
        public event BindDataToUIHander OnBindDataToUIEvent;

        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected async override void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
            // this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);


            MainForm.Instance.AppContext.log.ActionName = menuItem.ToString();
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                /*
                tb_MenuInfo menuInfo = MainForm.Instance.AppContext.CurUserInfo.UserMenuList.Where(c => c.MenuType == "行为菜单").Where(c => c.FormName == this.Name).FirstOrDefault();
                if (menuInfo == null)
                {
                    MessageBox.Show($"没有使用【{menuInfo.MenuName}】的权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<tb_ButtonInfo> btnList = MainForm.Instance.AppContext.CurUserInfo.UserButtonList.Where(c => c.MenuID == menuInfo.MenuID).ToList();
                if (!btnList.Where(b => b.BtnText == menuItem.ToString()).Any())
                {
                    MessageBox.Show($"没有使用【{menuItem.ToString()}】的权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }*/
            }
            switch (menuItem)
            {
                case MenuItemEnums.新增:
                    Add();
                    break;
                case MenuItemEnums.取消:
                    Cancel();
                    break;
                case MenuItemEnums.复制性新增:
                    AddByCopy();
                    break;
                case MenuItemEnums.删除:
                    await Delete();
                    break;
                case MenuItemEnums.修改:
                    Modify();
                    break;
                case MenuItemEnums.查询:
                    Query();
                    break;
                case MenuItemEnums.保存:
                    //操作前将数据收集
                    this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
                    if (EditEntity != null)
                    {
                        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                        long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
                        if (pkid > 0)
                        {
                            var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                            if (dataStatus == DataStatus.完结 || dataStatus == DataStatus.确认)
                            {
                                toolStripbtnSubmit.Enabled = false;
                                if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                                {
                                    MainForm.Instance.uclog.AddLog("已经是【完结】或【确认】状态，保存失败。");
                                }
                            }
                            else
                            {
                                await Save(true);
                            }
                        }
                        else
                        {
                            await Save(true);
                        }
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("单据不能为空，保存失败。");
                    }


                    break;
                case MenuItemEnums.提交:
                    //操作前将数据收集
                    this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
                    await Submit();
                    break;
                //case MenuItemEnums.高级查询:
                //    AdvQuery();
                //    break;
                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.刷新:
                    Refreshs();
                    break;
                case MenuItemEnums.属性:
                    Property();
                    break;
                case MenuItemEnums.审核:
                    await Review();
                    break;
                case MenuItemEnums.反审:
                    await ReReview();
                    break;
                case MenuItemEnums.结案:
                    await CloseCaseAsync();
                    break;
                case MenuItemEnums.打印:
                    Print();
                    toolStripbtnPrint.Enabled = false;
                    break;
                case MenuItemEnums.预览:
                    Preview();
                    break;
                case MenuItemEnums.设计:
                    PrintDesigned();
                    break;
                case MenuItemEnums.导出:
                    break;
                case MenuItemEnums.付款调整:
                    UpdatePaymentStatus();
                    break;
                default:
                    break;
            }


        }



        //泛型名称有一个尾巴，这里处理掉，但是总体要保持不能同时拥有同名的 泛型 和非泛型控制类
        //否则就是调用解析时用加小尾巴
        //注册时处理了所以用上面不加小尾巴
        //BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller`1");

        /// <summary>
        /// 结案处理
        /// 一般会自动结案，但是有些需要人工结案
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            frm.ShowCloseCaseImage = ReflectionHelper.ExistPropertyName<T>("CloseCaseImagePath");
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<T>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<T> needCloseCases = new List<T>();
                if (ReflectionHelper.ExistPropertyName<T>("CloseCaseOpinions"))
                {
                    EditEntity.SetPropertyValue("CloseCaseOpinions", frm.txtOpinion.Text);
                }
                //已经审核的并且通过的情况才能结案
                if (ReflectionHelper.ExistPropertyName<T>("DataStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
                {
                    // 确认状态下 已经审核并且通过
                    if (EditEntity.GetPropertyValue("DataStatus").ToInt() == (int)DataStatus.确认
                        && EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核
                        && EditEntity.GetPropertyValue("ApprovalResults") != null
                        && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                        )
                    {
                        needCloseCases.Add(EditEntity);
                    }
                }

                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
                if (rs.Succeeded)
                {
                    if (ReflectionHelper.ExistPropertyName<T>("CloseCaseImagePath"))
                    {
                        string strCloseCaseImagePath = System.DateTime.Now.ToString("yy") + "/" + System.DateTime.Now.ToString("MM") + "/" + Ulid.NewUlid().ToString();
                        byte[] bytes = UI.Common.ImageHelper.imageToByteArray(frm.CloseCaseImage);
                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                        ////上传新文件时要加后缀名
                        string uploadRsult = await httpWebService.UploadImageAsync(strCloseCaseImagePath + ".jpg", bytes, "upload");
                        if (uploadRsult.Contains("UploadSuccessful"))
                        {
                            EditEntity.SetPropertyValue("CloseCaseImagePath", strCloseCaseImagePath);
                            //这里更新数据库
                            await ctr.BaseSaveOrUpdate(EditEntity);
                        }
                    }

                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{ae.BillNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 审核 注意后面还需要加很多业务逻辑。
        /// 比方出库单，审核就会减少库存修改成本
        /// （如果有月结动作，则在月结时统计修改成本，更科学，因为如果退单等会影响成本）
        /// </summary>
        protected async override Task<ApprovalEntity> Review()
        {
            if (EditEntity == null)
            {
                return null;
            }
            ApprovalEntity ae = new ApprovalEntity();
            if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
            {
                //审核过，并且通过了，不能再次审核
                if ((EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核 || EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.结案)
                    && EditEntity.GetPropertyValue("ApprovalResults") != null
                    && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                    )
                {
                    MainForm.Instance.uclog.AddLog("未审核，或已审核且【未通过】的单据才能再次审核。");
                    return ae;
                }
            }
            //如果已经审核并且审核通过，则不能再次审核
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmApproval frm = new CommonUI.frmApproval();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<T>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            await Task.Delay(1);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                Command command = new Command();
                //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>(EditEntity);

                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                    CloneHelper.SetValues<T>(EditEntity, oldobj);
                };

                //审核了。数据状态要更新为
                EditEntity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.确认);




                //中间中的所有字段，都给值到单据主表中，后面需要处理审核历史这种再完善
                PropertyInfo[] array_property = ae.GetType().GetProperties();
                {
                    foreach (var property in array_property)
                    {
                        //保存审核结果 将审核中间值给到单据中，是否做循环处理？
                        //Expression<Func<ApprovalEntity, object>> PNameExp = t => t.ApprovalStatus;
                        //MemberInfo minfo = PNameExp.GetMemberInfo();
                        //string propertyName = minfo.Name;
                        if (ReflectionHelper.ExistPropertyName<T>(property.Name))
                        {
                            object aeValue = ReflectionHelper.GetPropertyValue(ae, property.Name);
                            //if (aeValue.Equals(true))
                            //{
                            //    aeValue = 1;
                            //}
                            //if (aeValue.Equals(false))
                            //{
                            //    aeValue = 0;
                            //}
                            ReflectionHelper.SetPropertyValue(EditEntity, property.Name, aeValue);
                        }
                    }
                }

                ReturnResults<T> rmr = new ReturnResults<T>();
                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                // AdjustingInventoryAsync
                //因为只需要更新主表
                rmr = await ctr.ApprovalAsync(EditEntity);
                if (rmr.Succeeded)
                {
                    //如果是出库单审核，则上传到服务器 锁定订单无法修改
                    if (ae.bizType == BizType.销售出库单)
                    {
                        //锁定对应的订单
                        if (EditEntity is tb_SaleOut saleOut)
                        {
                            if (saleOut.tb_saleorder != null)
                            {
                                OriginalData od = ActionForClient.销售出库审批(saleOut.tb_saleorder.SOrder_ID,
                                    MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name,
                                    (int)BizType.销售订单, ae.ApprovalResults);
                                MainForm.Instance.ecs.AddSendData(od);
                            }
                        }

                    }

                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);

                    //审核成功
                    base.ToolBarEnabledControl(MenuItemEnums.审核);
                    //如果审核结果为不通过时，审核不是灰色。
                    if (!ae.ApprovalResults)
                    {
                        toolStripbtnReview.Enabled = true;
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核成功。");
                    }
                    ae.ApprovalResults = true;
                    AuditLogHelper.Instance.CreateAuditLog<T>("审核", EditEntity, $"审核结果：{ae.ApprovalResults}");
                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    ae.ApprovalResults = false;
                    ToolBarEnabledControl(EditEntity);




                    AuditLogHelper.Instance.CreateAuditLog<T>("审核", EditEntity, $"审核结果{ae.ApprovalResults},{rmr.ErrorMsg}");
                    MainForm.Instance.logger.LogError($"{ae.bizName}:{ae.BillNo}审核失败{rmr.ErrorMsg}");
                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败{rmr.ErrorMsg},请联系管理员！", Color.Red);

                }
            }
            return ae;
        }

        /// <summary>
        /// 反审核 与审核相反
        /// </summary>
        protected async override Task<ApprovalEntity> ReReview()
        {
            ApprovalEntity ae = new ApprovalEntity();
            if (EditEntity == null)
            {
                return ae;
            }
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            if (pkid > 0)
            {
                //判断是否锁定
                BillLockInfo bli = MainForm.Instance.CacheLockTheOrder.Get<BillLockInfo>(pkid);
                if (bli != null)
                {
                    MainForm.Instance.uclog.AddLog($"单据已被{bli.LockedName}锁定，请刷新后再试");
                    return ae;
                }
            }


            if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
            {
                //反审，要审核过，并且通过了，才能反审。
                if (EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核
                    && EditEntity.GetPropertyValue("ApprovalResults") != null
                    && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                    )
                {

                }
                else
                {
                    MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审。");
                    return ae;
                }
            }

            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmReApproval frm = new CommonUI.frmReApproval();


            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<T>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //中间中的所有字段，都给值到单据主表中，后面需要处理审核历史这种再完善
                PropertyInfo[] array_property = ae.GetType().GetProperties();
                {
                    foreach (var property in array_property)
                    {
                        //保存审核结果 将审核中间值给到单据中，是否做循环处理？
                        //Expression<Func<ApprovalEntity, object>> PNameExp = t => t.ApprovalStatus;
                        //MemberInfo minfo = PNameExp.GetMemberInfo();
                        //string propertyName = minfo.Name;
                        if (ReflectionHelper.ExistPropertyName<T>(property.Name))
                        {
                            object aeValue = ReflectionHelper.GetPropertyValue(ae, property.Name);
                            ReflectionHelper.SetPropertyValue(EditEntity, property.Name, aeValue);
                        }
                    }
                }

                Command command = new Command();
                //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果取消反审，内存中反审核的数据要变为空白（之前的样子）
                    CloneHelper.SetValues<T>(EditEntity, oldobj);
                };

                ReturnResults<T> rmr = new ReturnResults<T>();
                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                rmr = await ctr.AntiApprovalAsync(EditEntity);
                if (rmr.Succeeded)
                {
                    //如果是出库单审核，则上传到服务器 锁定订单无法修改
                    if (ae.bizType == BizType.销售出库单)
                    {
                        //锁定对应的订单
                        if (EditEntity is tb_SaleOut saleOut)
                        {
                            if (saleOut.tb_saleorder != null)
                            {
                                OriginalData od = ActionForClient.销售出库反审(saleOut.tb_saleorder.SOrder_ID,
                                    MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name,
                                    (int)BizType.销售订单, ae.ApprovalResults);
                                MainForm.Instance.ecs.AddSendData(od);
                            }
                        }

                    }

                    ToolBarEnabledControl(MenuItemEnums.反审);
                    //这里推送到审核，启动工作流
                    AuditLogHelper.Instance.CreateAuditLog<T>("反审", EditEntity, $"反审原因{ae.ApprovalOpinions}");
                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    ToolBarEnabledControl(EditEntity);
                    AuditLogHelper.Instance.CreateAuditLog<T>("反审失败", EditEntity, $"反审原因{ae.ApprovalOpinions},{rmr.ErrorMsg}");
                    MainForm.Instance.logger.LogError($"{cbd.BillNo}反审失败{rmr.ErrorMsg}");
                    MainForm.Instance.PrintInfoLog($"{cbd.BillNo}反审失败{rmr.ErrorMsg},请联系管理员！", Color.Red);
                }
            }
            return ae;
        }


        private T editEntity;
        public T EditEntity { get => editEntity; set => editEntity = value; }


        public List<T> PrintData { get; set; }

        /// <summary>
        /// 取消添加 取消修改
        /// </summary>
        protected virtual void Cancel()
        {
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();
                //OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
            }

            ToolBarEnabledControl(MenuItemEnums.取消);
            bindingSourceSub.CancelEdit();

        }

        frmFormProperty frm = new frmFormProperty();
        protected override void Property()
        {
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //保存属性
                ToolBarEnabledControl(MenuItemEnums.属性);
                //AuditLogHelper.Instance.CreateAuditLog<T>("属性", EditEntity);
            }
            base.Property();
        }

        /*
        /// <summary>
        /// 删除远程的图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<bool> DeleteRemoteImages()
        {
            if (EditEntity == null || EditEntity.GetPropertyValue(typeof(T) + "Details") == null)
            {
                return false;
            }
            bool result = true;
            List<C> details = EditEntity.GetPropertyValue(typeof(T) + "Details") as List<C>;
            if (details == null)
            {
                return false;
            }
            foreach (C detail in details)
            {
                PropertyInfo[] props = typeof(C).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];//子类提上来用？
                    if (col != null)
                    {
                        if (col.CustomFormat == CustomFormatType.WebPathImage)
                        {
                            if (detail.GetPropertyValue(prop.Name) != null
                                && detail.GetPropertyValue(prop.Name).ToString().Contains("-"))
                            {
                                string imageNameValue = detail.GetPropertyValue(prop.Name).ToString();
                                //比较是否更新了图片数据
                                //old_new 无后缀文件名
                                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = new SourceGrid.Cells.Models.ValueImageWeb();
                                valueImageWeb.CellImageHashName = imageNameValue;
                                string oldfileName = valueImageWeb.GetOldRealfileName();
                                string newfileName = valueImageWeb.GetNewRealfileName();
                                string TempFileName = string.Empty;
                                //fileName = System.IO.Path.Combine(Application.StartupPath + @"\temp\", fileName);
                                //保存在本地临时目录 删除
                                if (System.IO.File.Exists(TempFileName))
                                {
                                    System.IO.File.Delete(TempFileName);
                                }
                                //上传到服务器，删除本地
                                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                string deleteRsult = await httpWebService.DeleteImageAsync(newfileName, "delete123");
                                MainForm.Instance.PrintInfoLog(deleteRsult);
                            }
                        }
                    }

                }
            }
            return result;
        }
        */


        /// <summary>
        /// 保存图片到服务器。所有图片都保存到服务器。即使草稿换电脑还可以看到
        /// </summary>
        /// <param name="RemoteSave"></param>
        /// <returns></returns>
        public async Task<bool> SaveFileToServer(SourceGridDefine sgd, List<C> Details)
        {
            bool result = true;
            List<SourceGridDefineColumnItem> ImgCols = new List<SourceGridDefineColumnItem>();
            foreach (C detail in Details)
            {
                PropertyInfo[] props = typeof(C).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];
                    if (col != null)
                    {
                        if (col.CustomFormat == CustomFormatType.WebPathImage && !ImgCols.Contains(col))
                        {
                            ImgCols.Add(col);
                        }
                    }
                }
            }
            try
            {
                result = await UploadImageAsync(ImgCols, sgd.grid, Details);
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
            }
            return result;
        }

        private async Task<bool> UploadImageAsync(List<SourceGridDefineColumnItem> ImgCols, Grid grid, List<C> Details)
        {
            bool rs = true;
            //保存图片到本地临时目录，图片数据保存在grid1控件中，所以要循环控件的行，控件真实数据行以1为起始
            int totalRowsFlag = grid.RowsCount;
            if (grid.HasSummary)
            {
                totalRowsFlag--;//减去一行总计行
            }
            for (int i = 1; i < totalRowsFlag; i++)
            {
                foreach (var col in ImgCols)
                {
                    if (grid[i, col.ColIndex].Value == null)
                    {
                        continue;
                    }
                    var model = grid[i, col.ColIndex].Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                    SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
                    //比较是否更新了图片数据
                    string newhash = valueImageWeb.GetImageNewHash();
                    if (valueImageWeb.CellImageBytes != null)
                    {
                        #region 需要上传

                        if (!valueImageWeb.GetImageoldHash().Equals(newhash, StringComparison.OrdinalIgnoreCase)
                        && grid[i, col.ColIndex].Value.ToString() == valueImageWeb.CellImageHashName)
                        {
                            string oldfileName = valueImageWeb.GetOldRealfileName();
                            string newfileName = valueImageWeb.GetNewRealfileName();
                            HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                            //如果服务器有旧文件 。可以先删除
                            if (!string.IsNullOrEmpty(valueImageWeb.GetImageoldHash()))
                            {
                                string deleteRsult = await httpWebService.DeleteImageAsync(oldfileName, "delete123");
                                MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
                            }
                            ////上传新文件时要加后缀名
                            string uploadRsult = await httpWebService.UploadImageAsync(newfileName + ".jpg", valueImageWeb.CellImageBytes, "upload");
                            if (uploadRsult.Contains("UploadSuccessful"))
                            {
                                valueImageWeb.UpdateImageName(newhash);
                                grid[i, col.ColIndex].Value = valueImageWeb.CellImageHashName;

                                string detailPKName = UIHelper.GetPrimaryKeyColName(typeof(C));
                                object PKValue = grid[i, col.ColIndex].Row.RowData.GetPropertyValue(detailPKName);
                                var detail = Details.Where(x => x.GetPropertyValue(detailPKName).ToString().Equals(PKValue.ToString())).FirstOrDefault();
                                detail.SetPropertyValue(col.ColName, valueImageWeb.CellImageHashName);
                                rs = true;
                                //成功后。旧文件名部分要和上传成功后新文件名部分一致。后面修改只修改新文件名部分。再对比
                                MainForm.Instance.PrintInfoLog("UploadSuccessful:" + newfileName);
                            }
                            else
                            {
                                rs = false;
                            }
                        }
                        #endregion
                    }
                }
            }
            return rs;
        }

        protected override void Add()
        {
            List<T> list = new List<T>();
            EditEntity = Activator.CreateInstance(typeof(T)) as T;
            BusinessHelper.Instance.InitEntity(EditEntity);
            BusinessHelper.Instance.InitStatusEntity(EditEntity);
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();
                OnBindDataToUIEvent(EditEntity, ActionStatus.新增);
            }

            if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
            {
                ReflectionHelper.SetPropertyValue(EditEntity, typeof(ActionStatus).Name, (int)ActionStatus.新增);
            }

            ToolBarEnabledControl(MenuItemEnums.新增);
            //bindingSourceEdit.CancelEdit();
            ControlMasterColumnsInvisible();
        }

        protected override void Modify()
        {
            if (editEntity == null)
            {
                return;
            }
            var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
            if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
            {
                BusinessHelper.Instance.EditEntity(EditEntity);
                EditEntity.SetPropertyValue(typeof(ActionStatus).Name, ActionStatus.修改);
                base.Modify();
                AuditLogHelper.Instance.CreateAuditLog<T>("修改", EditEntity);
            }
            else
            {
                EditEntity.SetPropertyValue(typeof(ActionStatus).Name, ActionStatus.修改);
                toolStripbtnModify.Enabled = false;
            }
            ToolBarEnabledControl(MenuItemEnums.修改);
        }



        protected override void AddByCopy()
        {
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();

                T NewEditEntity = Activator.CreateInstance(typeof(T)) as T;
                NewEditEntity = EditEntity.DeepCloneByjson();
                //复制性新增 时  PK要清空，单据编号类的
                string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
                if (pkid > 0)
                {
                    NewEditEntity.SetPropertyValue(PKCol, 0);
                }
                if (ReflectionHelper.ExistPropertyName<T>(typeof(ApprovalStatus).Name))
                {
                    ReflectionHelper.SetPropertyValue(NewEditEntity, typeof(ApprovalStatus).Name, (int)ApprovalStatus.未审核);
                }
                //设置为审核状态为否
                if (ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
                {
                    NewEditEntity.SetPropertyValue("ApprovalResults", false);
                }
                OnBindDataToUIEvent(NewEditEntity, ActionStatus.复制);
            }

            ToolBarEnabledControl(MenuItemEnums.新增);
        }

        protected override void Clear(SourceGridDefine sgd)
        {
            SourceGrid.Grid grid1 = sgd.grid;
            EditEntity = Activator.CreateInstance(typeof(T)) as T;
            BusinessHelper.Instance.InitEntity(EditEntity);
            BusinessHelper.Instance.InitStatusEntity(EditEntity);
            bindingSourceSub.Clear();
            //清空明细表格
            #region
            //先清空 不包含 列头和总计
            SourceGrid.RangeRegion rr = new SourceGrid.RangeRegion(new SourceGrid.Position(grid1.Rows.Count, grid1.Columns.Count));
            for (int ii = 0; ii < grid1.Rows.Count; ii++)
            {
                grid1.Rows[ii].RowData = null;
            }
            grid1.ClearValues(rr);


            #endregion
        }
        protected bool Validator(T EditEntity)
        {
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            bool vd = base.ShowInvalidMessage(ctr.BaseValidator(EditEntity));
            return vd;
        }

        /// <summary>
        /// 验证明细
        /// </summary>
        /// <typeparam name="Child"></typeparam>
        /// <param name="details"></param>
        /// <returns></returns>
        protected bool Validator<Child>(List<Child> details) where Child : class
        {
            List<bool> subrs = new List<bool>();
            var lastlist = ((IEnumerable<dynamic>)details).ToList();
            foreach (var item in lastlist)
            {
                BaseController<Child> ctr = Startup.GetFromFacByName<BaseController<Child>>(typeof(Child).Name + "Controller");
                bool sub_bool = base.ShowInvalidMessage(ctr.BaseValidator(item as Child));
                subrs.Add(sub_bool);
            }
            if (subrs.Where(c => c.Equals(false)).Any())
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 更新式保存，有一些单据，实在要修改，并且明细没有删除和添加时候执行
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<ReturnMainSubResults<T>> UpdateSave(T entity)
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            if (pkid == 0)
            {
                entity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.草稿);
            }
            ReturnMainSubResults<T> rmr = new ReturnMainSubResults<T>();
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            rmr = await ctr.BaseUpdateWithChild(entity);
            if (rmr.Succeeded)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
                {
                    //注意這里保存的是枚举
                    ReflectionHelper.SetPropertyValue(entity, typeof(ActionStatus).Name, (int)ActionStatus.加载);
                }

                ToolBarEnabledControl(MenuItemEnums.保存);
                MainForm.Instance.uclog.AddLog("更新式保存成功");
                AuditLogHelper.Instance.CreateAuditLog<T>("更新式保存成功", rmr.ReturnObject);
            }
            else
            {
                MainForm.Instance.uclog.AddLog("更新式保存成功失败，请重试;或联系管理员。" + rmr.ErrorMsg, UILogType.错误);
            }
            return rmr;
        }


        protected async Task<ReturnMainSubResults<T>> Save(T entity)
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            if (pkid == 0)
            {
                entity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.草稿);
            }
            else
            {
                if (MainForm.Instance.CacheLockTheOrder != null)
                {
                    BillLockInfo bli = MainForm.Instance.CacheLockTheOrder.Get<BillLockInfo>(pkid);
                    if (bli != null)
                    {
                        return new ReturnMainSubResults<T>()
                        {
                            Succeeded = false,
                            ErrorMsg = $"单据已被{bli.LockedName}锁定，请刷新后再试"
                        };
                    }
                }
            }

            ReturnMainSubResults<T> rmr = new ReturnMainSubResults<T>();
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            rmr = await ctr.BaseSaveOrUpdateWithChild<T>(entity);
            if (rmr.Succeeded)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
                {
                    //注意這里保存的是枚举
                    ReflectionHelper.SetPropertyValue(entity, typeof(ActionStatus).Name, (int)ActionStatus.加载);
                }

                ToolBarEnabledControl(MenuItemEnums.保存);
                MainForm.Instance.uclog.AddLog("保存成功");

                //var sw = new Stopwatch();
                //sw.Start();
                //var resultContext = await next();
                //sw.Stop();

                //审计日志
                AuditLogHelper.Instance.CreateAuditLog<T>("保存", rmr.ReturnObject);
            }
            else
            {
                MainForm.Instance.uclog.AddLog("保存失败，请重试;或联系管理员。" + rmr.ErrorMsg, UILogType.错误);
            }
            return rmr;
        }

        /// <summary>
        /// 删除远程的图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<bool> DeleteRemoteImages()
        {
            await Task.Delay(0);
            return false;
        }


        protected async virtual Task<ReturnResults<T>> Delete()
        {
            ReturnResults<T> rss = new ReturnResults<T>();
            if (editEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if (dataStatus == DataStatus.新建 && !AppContext.IsSuperUser)
                    {
                        if (ReflectionHelper.ExistPropertyName<T>("Created_by") && ReflectionHelper.GetPropertyValue(editEntity, "Created_by").ToString() != AppContext.CurUserInfo.Id.ToString())
                        {
                            MessageBox.Show("只有创建人才能删除提交的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只有创建人才能删除提交的单据。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                    bool rs = await ctr.BaseDeleteByNavAsync(editEntity as T);
                    object PKValue = editEntity.GetPropertyValue(UIHelper.GetPrimaryKeyColName(typeof(T)));
                    rss.Succeeded = rs;
                    rss.ReturnObject = editEntity;
                    if (rs)
                    {
                        AuditLogHelper.Instance.CreateAuditLog<T>("删除", editEntity);
                        if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                        {
                            //MainForm.Instance.logger.Debug($"单据显示中删除:{typeof(T).Name}，主键值：{PKValue.ToString()} "); //如果要生效 要将配置文件中 <add key="log4net.Internal.Debug" value="true " /> 也许是：logn4net.config <log4net debug="false"> 改为true
                        }
                        bindingSourceSub.Clear();

                        //删除远程图片及本地图片
                        await DeleteRemoteImages();

                        //提示一下删除成功
                        MainForm.Instance.uclog.AddLog("提示", "删除成功");

                        //加载一个空的显示的UI
                        bindingSourceSub.Clear();
                        OnBindDataToUIEvent(Activator.CreateInstance(typeof(T)) as T, ActionStatus.删除);
                    }
                }
                else
                {
                    //
                    MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                }
            }
            return rss;
        }


        /// <summary>
        /// 提交
        /// </summary>
        protected async override Task<bool> Submit()
        {
            if (EditEntity == null)
            {
                return false;
            }
            bool submitrs = false;
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            if (pkid > 0)
            {
                var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.完结 || dataStatus == DataStatus.确认)
                {
                    toolStripbtnSubmit.Enabled = false;
                    if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                    {
                        MainForm.Instance.uclog.AddLog("单据已经是【完结】或【确认】状态，提交失败。");
                    }
                    return false;
                }
                else
                {
                    if (ReflectionHelper.ExistPropertyName<T>(typeof(DataStatus).Name))
                    {
                        ReflectionHelper.SetPropertyValue(EditEntity, typeof(DataStatus).Name, (int)DataStatus.新建);
                    }
                    ReturnResults<T> rmr = new ReturnResults<T>();
                    BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");

                    rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                    if (rmr.Succeeded)
                    {
                        ToolBarEnabledControl(MenuItemEnums.提交);
                        AuditLogHelper.Instance.CreateAuditLog<T>("提交", rmr.ReturnObject);

                        //这里推送到审核，启动工作流 后面优化
                        // OriginalData od = ActionForClient.工作流提交(pkid, (int)BizType.盘点单);
                        // MainForm.Instance.ecs.AddSendData(od);]

                        //如果是销售订单或采购订单可以自动审核，有条件地执行？
                        CommBillData cbd = new CommBillData();
                        BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
                        cbd = bcf.GetBillData<T>(EditEntity as T);
                        ApprovalEntity ae = new ApprovalEntity();
                        ae.ApprovalOpinions = "自动审核";
                        ae.ApprovalResults = true;
                        ae.ApprovalStatus = (int)ApprovalStatus.已审核;

                        if (cbd.BizType == BizType.销售订单 && AppContext.SysConfig.AutoApprovedSaleOrderAmount > 0)
                        {
                            if (EditEntity is tb_SaleOrder saleOrder)
                            {
                                if (saleOrder.TotalAmount < AppContext.SysConfig.AutoApprovedSaleOrderAmount)
                                {
                                    Command command = new Command();
                                    //缓存当前编辑的对象。如果撤销就回原来的值
                                    tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
                                    command.UndoOperation = delegate ()
                                    {
                                        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                                        CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
                                    };
                                    BusinessHelper.Instance.ApproverEntity(EditEntity);
                                    tb_SaleOrderController<tb_SaleOrder> ctrSO = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                                    ReturnResults<tb_SaleOrder> rmrs = await ctrSO.ApprovalAsync(saleOrder);
                                    if (rmrs.Succeeded)
                                    {
                                        //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                                        //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                                        //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                                        //MainForm.Instance.ecs.AddSendData(od);
                                        //审核成功
                                        ToolBarEnabledControl(MenuItemEnums.审核);
                                        AuditLogHelper.Instance.CreateAuditLog<T>("审核", rmr.ReturnObject, "满足金额设置条件，自动审核通过");
                                        //如果审核结果为不通过时，审核不是灰色。
                                        if (!ae.ApprovalResults)
                                        {
                                            toolStripbtnReview.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        command.Undo();
                                        //审核失败 要恢复之前的值
                                        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,{rmrs.ErrorMsg}请联系管理员！", Color.Red);
                                    }
                                }
                            }
                        }

                        if (cbd.BizType == BizType.采购订单 && AppContext.SysConfig.AutoApprovedPurOrderAmount > 0)
                        {
                            if (EditEntity is tb_PurOrder purOrder)
                            {
                                if (purOrder.TotalAmount < AppContext.SysConfig.AutoApprovedPurOrderAmount)
                                {
                                    Command command = new Command();
                                    //缓存当前编辑的对象。如果撤销就回原来的值
                                    tb_PurOrder oldobj = CloneHelper.DeepCloneObject<tb_PurOrder>(EditEntity);
                                    command.UndoOperation = delegate ()
                                    {
                                        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                                        CloneHelper.SetValues<tb_PurOrder>(EditEntity, oldobj);
                                    };
                                    BusinessHelper.Instance.ApproverEntity(EditEntity);
                                    tb_PurOrderController<tb_PurOrder> ctrSO = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                                    ReturnResults<tb_PurOrder> rmrs = await ctrSO.ApprovalAsync(purOrder);
                                    if (rmrs.Succeeded)
                                    {
                                        //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                                        //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                                        //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                                        //MainForm.Instance.ecs.AddSendData(od);

                                        //审核成功
                                        ToolBarEnabledControl(MenuItemEnums.审核);
                                        AuditLogHelper.Instance.CreateAuditLog<T>("审核", rmr.ReturnObject, "满足金额设置条件，自动审核通过");
                                        //如果审核结果为不通过时，审核不是灰色。
                                        if (!ae.ApprovalResults)
                                        {
                                            toolStripbtnReview.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        command.Undo();
                                        //审核失败 要恢复之前的值
                                        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,{rmrs.ErrorMsg},请联系管理员！", Color.Red);

                                    }
                                }
                            }
                        }
                        submitrs = true;
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog($"提交失败，请重试;或联系管理员。\r\n 错误信息：{rmr.ErrorMsg}", UILogType.错误);
                        submitrs = false;
                    }
                }

            }
            else
            {
                bool rs = await this.Save(true);
                if (rs)
                {
                    if (ReflectionHelper.ExistPropertyName<T>(typeof(DataStatus).Name))
                    {
                        ReflectionHelper.SetPropertyValue(EditEntity, typeof(DataStatus).Name, (int)DataStatus.新建);
                    }
                    //先保存再提交
                    if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                    {
                        MainForm.Instance.uclog.AddLog("提交时,单据没有保存,将状态修改为提交状态后直接保存了。");
                    }
                    ReturnResults<T> rmr = new ReturnResults<T>();
                    BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                    rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                    if (rmr.Succeeded)
                    {
                        ToolBarEnabledControl(MenuItemEnums.提交);
                        AuditLogHelper.Instance.CreateAuditLog<T>("保存-提交", rmr.ReturnObject);
                        //这里推送到审核，启动工作流 后面优化
                        // OriginalData od = ActionForClient.工作流提交(pkid, (int)BizType.盘点单);
                        // MainForm.Instance.ecs.AddSendData(od);
                        submitrs = true;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return submitrs;
        }





        /// <summary>
        /// 优化后的查询条件
        /// </summary>
        public QueryFilter QueryConditionFilter { get; set; } = new QueryFilter();

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public virtual void QueryConditionBuilder()
        {
            //添加默认全局的
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(T).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }



        protected override void Query()
        {
            if (base.Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }

            // 如果没有条件列表 直接查全部。
            // CommonUI.frmQuery<T> frm = new CommonUI.frmQuery<T>();
            //frm.QueryConditions = QueryConditions;
            //frm.LoadQueryConditionToUI(false);
            //frm.OnSelectDataRow += UcAdv_OnSelectDataRow;
            if (QueryConditionFilter.QueryFields.Count == 0)
            {
                QueryConditionBuilder();
            }

            //暂时认为基础数据都是这个基类出来的 否则可以根据菜单中的基类类型来判断生成
            UCAdvFilterGeneric<T> ucBaseList = new UCAdvFilterGeneric<T>(); // Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);

            ucBaseList.QueryConditionFilter = QueryConditionFilter;
            ucBaseList.CurMenuInfo = CurMenuInfo;
            ucBaseList.KeyValueTypeForDgv = typeof(T);
            ucBaseList.Runway = BaseListRunWay.选中模式;
            //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
            frmBaseEditList frm = new frmBaseEditList();
            frm.StartPosition = FormStartPosition.CenterScreen;
            ucBaseList.Dock = DockStyle.Fill;
            ucBaseList.Tag = frm;
            frm.kryptonPanel1.Controls.Add(ucBaseList);
            ucBaseList.OnSelectDataRow += UcBaseList_OnSelectDataRow;
            BizTypeMapper mapper = new BizTypeMapper();
            var BizTypeText = mapper.GetBizType(typeof(T).Name).ToString();
            frm.Text = "关联查询" + "-" + BizTypeText;
            frm.Show();

        }



        private void UcBaseList_OnSelectDataRow(object entity)
        {
            if (entity == null)
            {
                return;
            }
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();
                OnBindDataToUIEvent(entity as T, ActionStatus.加载);

                //如果 查出来的数据 能审核 能打印 等显示各种状态 TODO
                ToolBarEnabledControl(entity);

                ToolBarEnabledControl(MenuItemEnums.查询);
                //thisform
            }
            //使用了导航查询 entity包括了明细
            //details = (entity as tb_Stocktake).tb_StocktakeDetails;
            //LoadDataToGrid(details);
        }





        protected async override void Refreshs()
        {
            if (editEntity == null)
            {
                return;
            }
            if (true)//CanRefresh()
            {
                using (StatusBusy busy = new StatusBusy("刷新中..."))
                {
                    //这里应该是重新加载单据内容 而不是查询
                    //但是，查询才是对的，因为数据会修改变化缓存。
                    if (!Edited)
                    {
                        if (OnBindDataToUIEvent != null)
                        {
                            BaseEntity pkentity = (editEntity as T) as BaseEntity;
                            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                            editEntity = await ctr.BaseQueryByIdNavAsync(pkentity.PrimaryKeyID) as T;
                            if (editEntity == null)
                            {
                                editEntity = Activator.CreateInstance<T>();
                            }
                            bindingSourceSub.Clear();
                            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
                            //if (pkentity.PrimaryKeyID > 0)
                            //{
                            //    //可以修改
                            //    if (pkentity.ContainsProperty(typeof(DataStatus).Name))
                            //    {
                            //        if (pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.草稿).ToString() ||
                            //               pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.新建).ToString())
                            //        {
                            //            toolStripbtnModify.Enabled = true;
                            //            toolStripbtnSubmit.Enabled = true;
                            //            toolStripbtnReview.Enabled = true;
                            //        }
                            //    }
                            //}

                            ToolBarEnabledControl(pkentity);
                        }
                        else
                        {
                            //
                            MessageBox.Show("请实现数据绑定的事件。用于显示数据详情。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (MessageBox.Show(this, "有数据没有保存\r\n你确定要重新加载吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            if (OnBindDataToUIEvent != null)
                            {
                                BaseEntity pkentity = (editEntity as T) as BaseEntity;
                                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                                editEntity = await ctr.BaseQueryByIdNavAsync(pkentity.PrimaryKeyID) as T;
                                bindingSourceSub.Clear();
                                if (editEntity == null)
                                {
                                    editEntity = Activator.CreateInstance<T>();
                                }
                                OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
                                //if (pkentity.PrimaryKeyID > 0)
                                //{
                                //    //可以修改
                                //    if (pkentity.ContainsProperty(typeof(DataStatus).Name))
                                //    {
                                //        if (pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.草稿).ToString() ||
                                //               pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.新建).ToString())
                                //        {
                                //            toolStripbtnModify.Enabled = true;
                                //            toolStripbtnSubmit.Enabled = true;
                                //            toolStripbtnReview.Enabled = true;
                                //        }
                                //    }
                                //}
                                //刷新了。不再提示编辑状态了
                                Edited = false;
                                ToolBarEnabledControl(pkentity);
                            }
                        }
                    }
                }
            }
        }

        protected override void Exit(object thisform)
        {
            base.Exit(this);
        }


        #region 打印相关
        #region 为了性能 打印认为打印时 检测过的打印机相关配置在一个窗体下成功后。即可不每次检测
        private tb_PrintConfig printConfig = null;
        public tb_PrintConfig PrintConfig
        {
            get
            {
                return printConfig;
            }
            set
            {
                printConfig = value;

            }
        }

        #endregion
        public async void PrintDesigned()
        {
            if (EditEntity == null)
            {
                return;
            }
            List<T> list = new List<T>();
            list.Add(EditEntity);
            if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
            {
                PrintConfig = PrintHelper<T>.GetPrintConfig(list);
            }
            bool rs = await PrintHelper<T>.Print(list, RptMode.DESIGN, PrintConfig);
        }


        public async void Print()
        {
            if (EditEntity == null)
            {
                return;
            }
            List<T> list = new List<T>();
            list.Add(EditEntity);
            foreach (var item in list)
            {
                if (item == null)
                {
                    continue;
                }
                if (item.ContainsProperty("DataStatus"))
                {
                    if (item.GetPropertyValue("DataStatus").ToString() == ((int)DataStatus.草稿).ToString() || item.GetPropertyValue("DataStatus").ToString() == ((int)DataStatus.新建).ToString())
                    {
                        if (MessageBox.Show("没有审核的数据无法打印,你确定要打印吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return;
                        }

                    }
                }
            }
            if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
            {
                PrintConfig = PrintHelper<T>.GetPrintConfig(list);
            }
            bool rs = await PrintHelper<T>.Print(list, RptMode.PRINT, PrintConfig);
            AuditLogHelper.Instance.CreateAuditLog<T>("打印", EditEntity);
        }



        public async void Preview()
        {
            if (EditEntity == null)
            {
                return;
            }
            List<T> list = new List<T>();
            list.Add(EditEntity);
            if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
            {
                PrintConfig = PrintHelper<T>.GetPrintConfig(list);
            }
            bool rs = await PrintHelper<T>.Print(list, RptMode.PREVIEW, PrintConfig);
        }

        #endregion

        private void BaseBillEditGeneric_Load(object sender, EventArgs e)
        {
            timerAutoSave.Start();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    #region 请求缓存
                    //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
                    UIBizSrvice.RequestCache<T>();
                    UIBizSrvice.RequestCache<C>();
                    UIBizSrvice.RequestCache<tb_Prod>();
                    #endregion
                }
            }
        }

        private async void timerAutoSave_Tick(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    //自动保存的时间秒数  30秒
                    if (MainForm.Instance.AppContext.OnlineUser.静止时间 > 30 && MainForm.Instance.AppContext.IsOnline)
                    {
                        bool result = await AutoSaveDataAsync();
                        if (!result)
                        {
                            //如果保存失败，则停止自动保存？
                            timerAutoSave.Stop();
                        }
                    }
                }
            }

        }



        private async Task<bool> AutoSaveDataAsync()
        {
            bool result = false;
            try
            {
                if (EditEntity != null)
                {
                    #region 自动保存单据数据  后面优化可以多个单?限制5个？Cache
                    await Save(false);
                    string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\FormProperty\\Data", CurMenuInfo.CaptionCN + ".cache");
                    System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
                    //判断目录是否存在
                    if (!System.IO.Directory.Exists(fi.Directory.FullName))
                    {
                        System.IO.Directory.CreateDirectory(fi.Directory.FullName);
                    }
                    string json = JsonConvert.SerializeObject(EditEntity,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                        });


                    /*
                     var settings = new JsonSerializerSettings
        {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
        };

        string jsonString = JsonConvert.SerializeObject(myObject, settings);
                     */


                    File.WriteAllText(PathwithFileName, json);
                    MainForm.Instance.uclog.AddLog("缓存数据保存成功。");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "缓存数据保存时出错");
            }
            return result;
        }
    }
}