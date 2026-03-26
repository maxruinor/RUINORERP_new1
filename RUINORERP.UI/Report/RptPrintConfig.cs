using Krypton.Toolkit;
using FastReport.Service;
using HLH.Lib.Helper;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using FastReport;
using RUINORERP.Business.CommService;
using RUINORERP.UI.BaseForm;
using AutoMapper;
using RUINORERP.Global;
using RUINORERP.Common.Extensions;
using FastReport.Export.Pdf;
using FastReport.Preview;
using System.Diagnostics;
using RUINORERP.Business.BizMapperService;

namespace RUINORERP.UI.Report
{
    /// <summary>
    /// 每个打印业务都是来自于菜单的按钮1
    /// 菜单对应的是一个表 一个实体。一个实体对应一个业务类型
    /// </summary>
    public partial class RptPrintConfig : KryptonForm
    {
        /// <summary>
        /// 当前菜单ID
        /// </summary>
        private long _currentMenuId;

        /// <summary>
        /// 是否为个人配置
        /// </summary>
        private bool _isPersonalConfig;

        /// <summary>
        /// 打印配置
        /// </summary>
        public RUINORERP.Model.tb_PrintConfig printConfig { get; set; }

        /// <summary>
        /// 要打印的数据
        /// </summary>
        public List<object> PrintDataSources { get; set; }

        /// <summary>
        /// PrintDataSources 打印对象的主键字段名，为了更新打印状态和次数
        /// </summary>
        public string PKFieldName { get; set; }


        public List<tb_Company> companyInfos { get; set; }

        public List<CurrentUserInfo> currUserInfos { get; set; }

        public RptPrintConfig()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                //MessageBox.Show("您没有删除打印配置的权限，请联系管理员。");
                //return;
                try
                {
                    if (bindingSourcePrintTemplate.Current != null && bindingSourcePrintTemplate.Current is tb_PrintTemplate template)
                    {
                        if (template.Created_by != null)
                        {
                            if (template.Created_by.Value != MainForm.Instance.AppContext.CurUserInfo.EmpID)
                            {
                                MessageBox.Show("只有创建人，才能删除打印配置。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            if (MessageBox.Show("打印模板删除后无法恢复\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (bindingSourcePrintTemplate.Current != null)
                {
                    int d = MainForm.Instance.AppContext.Db.Deleteable<tb_PrintTemplate>(bindingSourcePrintTemplate.Current).ExecuteCommand();
                    if (d > 0)
                    {
                        bindingSourcePrintTemplate.RemoveCurrent();
                    }
                }
            }
        }

        private void btnDesign_Click(object sender, EventArgs e)
        {
            if (bindingSourcePrintTemplate.Current != null)
            {
                RptDesignForm frm = new RptDesignForm();
                var printTemplate = bindingSourcePrintTemplate.Current as tb_PrintTemplate;
                printTemplate.ActionStatus = ActionStatus.加载;
                frm.bindingSourceTemplateDesign = bindingSourcePrintTemplate;
                frm.PrintDataSources = this.PrintDataSources;
                frm.currUserInfos = currUserInfos;
                frm.companyInfos = companyInfos;
                frm.ShowDialog();
            }
        }





        private void btnCreate_Click(object sender, EventArgs e)
        {
            //新建适用于这种类型的报表
            //RptNewTemplate frmnew = new RptNewTemplate();
            //if (frmnew.ShowDialog() == DialogResult.OK)
            //{
            //    if (frmnew.txtTemplateName.Text.Trim().Length > 0)
            //    {


            tb_PrintTemplate reportTemplate = new tb_PrintTemplate();
            reportTemplate.PrintConfigID = printConfig.PrintConfigID;
            reportTemplate.Template_Name = "新建";// frmnew.txtTemplateName.Text.Trim();
            if (reportTemplate.ID > 0)
            {
                BusinessHelper.Instance.EditEntity(reportTemplate);
            }
            else
            {
                BusinessHelper.Instance.InitEntity(printConfig);
            }
          
            if (this.PrintDataSources.Count > 0)
            {
                CommBillData cbd = EntityMappingHelper.GetBillData(this.PrintDataSources[0].GetType(), this.PrintDataSources[0]);
                if (cbd.BizName != null)
                {
                    reportTemplate.BizName = cbd.BizName;
                    reportTemplate.BizType = (int)cbd.BizType;
                    reportTemplate.Template_Name = "新建" + reportTemplate.BizName;
                }
            }
            BusinessHelper.Instance.InitEntity(reportTemplate);
            reportTemplate.ActionStatus = ActionStatus.加载;
            reportTemplate.IsDefaultTemplate = true;
            byte[] dail = new byte[1];
            dail = null;
            reportTemplate.TemplateFileStream = dail;
            if (printConfig.tb_PrintTemplates == null)
            {
                printConfig.tb_PrintTemplates = new List<tb_PrintTemplate>();
            }
            //reportTemplate = MainForm.Instance.AppContext.Db.Insertable<tb_PrintTemplate>(reportTemplate).ExecuteReturnEntity();
            bindingSourcePrintTemplate.Add(reportTemplate);
            bindingSourcePrintTemplate.Position++;
            //    }
            //}

            RptDesignForm frm = new RptDesignForm();
            //frm.printTemplate = bindingSourcePrintTemplate.Current as tb_PrintTemplate;
            frm.bindingSourceTemplateDesign = bindingSourcePrintTemplate;
            frm.PrintDataSources = this.PrintDataSources;
            frm.currUserInfos = currUserInfos;
            frm.companyInfos = companyInfos;
            if (frm.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {
                //没有保存过
                if (reportTemplate.ActionStatus == ActionStatus.新增)
                {
                    bindingSourcePrintTemplate.RemoveCurrent();
                }

            }
        }




        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintReport(RptMode.PRINT);
            ////////打印////////
            //设置默认打印机
            //report.PrintPrepared();
            //report.PrintSettings.ShowDialog = false;
            //report.Print();
            ////释放资源
            //report.Dispose();
        }

        private void btnPreView_Click(object sender, EventArgs e)
        {
            PrintReport(RptMode.PREVIEW);
        }




        private void btnQueryBill_Click(object sender, EventArgs e)
        {

        }

      
     
        //设计报表
        private void PrintReport(RptMode rptMode)
        {
            FastReport.Report TargetReport = new FastReport.Report();

            try
            {


                //TargetReport.FileName = RptName;
                tb_PrintTemplate printTemplate = new tb_PrintTemplate();
                if (newSumDataGridView1.CurrentRow != null)
                {
                    printTemplate = newSumDataGridView1.CurrentRow.DataBoundItem as tb_PrintTemplate;
                }
                else
                {
                    MessageBox.Show("请选择对应的模板.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //打印次数提醒
                if (PrintDataSources.Count > 0 && PrintDataSources[0].ContainsProperty("PrintStatus"))
                {
                    BizType bizType = Business.BizMapperService.EntityMappingHelper.GetBizType(PrintDataSources[0].GetType().Name);
                    int printCounter = PrintDataSources[0].GetPropertyValue("PrintStatus").ToString().ToInt();
                    if (printCounter > 0)
                    {
                        if (MessageBox.Show($"当前【{bizType.ToString()}】已经打印过【{printCounter}】次,你确定要重新打印吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }


                TargetReport.RegisterData(PrintDataSources, "rd");
                List<CurrentUserInfo> currUserInfo = new List<CurrentUserInfo>();
                currUserInfo.Add(MainForm.Instance.AppContext.CurUserInfo);

                List<tb_Company> companyInfo = new List<tb_Company>();
                companyInfo.Add(MainForm.Instance.AppContext.CompanyInfo);

                TargetReport.RegisterData(currUserInfo, "currUserInfo");
                TargetReport.RegisterData(companyInfo, "companyInfo");

                if (printTemplate.TemplateFileStream != null)
                {
                    byte[] ReportBytes = (byte[])printTemplate.TemplateFileStream;
                    using (System.IO.MemoryStream Stream = new System.IO.MemoryStream(ReportBytes))
                    {
                        TargetReport.Load(Stream);
                    }
                }

                //准备
                //TargetReport.Prepare();
                //准备合并上次的 多页时候才需要1
                TargetReport.Prepare(true);

                // 使用新的优先级配置获取打印机
                string printerNameRpt = PrintHelper<object>.GetPrinterWithPriority(printConfig, printConfig?.BizName);
                if (!string.IsNullOrEmpty(printerNameRpt))
                {
                    TargetReport.PrintSettings.ShowDialog = false;
                    TargetReport.PrintSettings.Printer = printerNameRpt;
                }

                //操作方式：DESIGN-设计;PREVIEW-预览;PRINT-打印
                if (rptMode == RptMode.DESIGN)
                {
                    TargetReport.Design();
                    ////释放资源
                    TargetReport.Dispose();
                }
                else if (rptMode == RptMode.PREVIEW)
                {


                    //PDFExport export = new PDFExport();
                    //export.EmbeddingFonts = true;
                    //export.TextInCurves = true;
                    //TargetReport.Export(export, "result2025.pdf");


                    RptPreviewForm frm = new RptPreviewForm();
                    frm.Text = printConfig.BizName + "打印预览";
                    frm.MyReport = TargetReport;
                    frm.ShowDialog();
                    //TargetReport.ShowPrepared();
                }
                else if (rptMode == RptMode.PRINT)
                {
                    //打印准备好的内容
                    TargetReport.PrintPrepared();

                    //打印状态更新
                    PrintHelper<object>.UpdatePrintStatus(PrintDataSources, PKFieldName);
                }
                else if (rptMode == RptMode.ToPDF)
                {
                    using (var dialog = new SaveFileDialog())
                    {
                        dialog.Filter = "PDF Files (*.pdf)|*.pdf";
                        string pdfName = Business.BizMapperService.EntityMappingHelper.GetBizType(PrintDataSources[0].GetType().Name).ToString() + ".pdf";
                        if (listboxBIll.Items.Count > 0)
                        {
                            pdfName = Business.BizMapperService.EntityMappingHelper.GetBizType(PrintDataSources[0].GetType().Name).ToString() + listboxBIll.Items[0].ToString() + ".pdf";
                        }
                        dialog.FileName = pdfName;
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            var stopwatch = Stopwatch.StartNew();

                            using (PDFExport pdfExport = new PDFExport())
                            {
                                // 1. 禁用字体嵌入 - 核心解决方案
                                pdfExport.EmbeddingFonts = false;  // 避免内存溢出的关键设置

                                pdfExport.TextInCurves = true;

                                // 2. 替代方案：使用基本字体代替
                                //pdfExport.UseTrueTypeFonts = false;  // 使用基本PDF字体
                                //PDFExport export = new PDFExport();
                                //export.EmbeddingFonts = false;
                                //export.TextInCurves = true;
                                ////export.CurvesInterpolation=PDFExport.CurvesInterpolationEnum.Curves;
                                ////export.CurvesInterpolationText = PDFExport.CurvesInterpolationEnum.Curves;
                                ////export.GradientQuality = PDFExport.GradientQualityEnum.High;
                                ///
                                // 3. 强制使用通用字体（确保客户端有此字体）
                                TargetReport.Styles.Add(new Style());
                                TargetReport.Styles[0].Font = new Font("Arial", 10);

                                // 4. 减少内存使用的其他设置
                                //pdfExport.ImageDpi = 150;  // 降低DPI减少内存占用
                                pdfExport.Compressed = true;  // 启用压缩
                                pdfExport.OpenAfterExport = false;  // 避免额外内存开销

                                // 5. 分页处理模式（对于超大报表）
                                //pdfExport.ExportMode = PDFExportMode.Batch;
                                //pdfExport.BatchSize = 10;  // 每批处理10页

                                // 6. 清理报表资源
                                TargetReport.Clear();

                                // 7. 显式垃圾回收（谨慎使用）
                                GC.Collect();
                                GC.WaitForPendingFinalizers();

                                // 导出报表
                                TargetReport.Export(pdfExport, dialog.FileName);

                            }




                            stopwatch.Stop();

                            if (MessageBox.Show($"成功导出 {dialog}，耗时 {stopwatch.Elapsed.TotalSeconds:F2} 秒。\n是否立即打开文件？",
                                    "导出完成", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex);
            }
            finally
            {    ////释放资源
                TargetReport.Dispose();
            }

        }



        private void RptPrintConfig_Load(object sender, EventArgs e)
        {
            try
            {
                _currentMenuId = GetCurrentMenuId();
                var personalConfig = GetMenuPersonalPrintConfig(_currentMenuId);
                _isPersonalConfig = personalConfig != null && personalConfig.UsePersonalPrintConfig == true;

                LoadPrinterSettings(personalConfig);

                UpdateFormTitle();
                UpdateConfigStatusIndicator();

                listboxBIll.Items.Clear();
                if (PrintDataSources != null)
                {
                  
                    foreach (var item in PrintDataSources)
                    {
                        try
                        {
                            CommBillData cbd = EntityMappingHelper.GetBillData(item.GetType(), item);
                            if (cbd.BillNo != null)
                            {
                                listboxBIll.Items.Add(cbd.BillNo);
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger.Error(ex);
                        }
                    }
                }

                newSumDataGridView1.NeedSaveColumnsXml = true;
                newSumDataGridView1.XmlFileName = typeof(tb_PrintTemplate).Name;
                newSumDataGridView1.FieldNameList = Common.UIHelper.GetFieldNameColList(typeof(tb_PrintTemplate));

                newSumDataGridView1.Use是否使用内置右键功能 = true;
                //要这样处理，不然数据源不联动
                if (printConfig.tb_PrintTemplates == null)
                {
                    printConfig.tb_PrintTemplates = new List<tb_PrintTemplate>();
                }
                bindingSourcePrintTemplate.DataSource = printConfig.tb_PrintTemplates;
                // 绑定数据 不修改绑定
                newSumDataGridView1.DataSource = null;
                newSumDataGridView1.DataSource = bindingSourcePrintTemplate;

              

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("打印配置加载异常", ex);
            }
        }

        /// <summary>
        /// 加载打印机设置（支持个人配置和系统配置）
        /// </summary>
        private void LoadPrinterSettings(tb_UIMenuPersonalization personalConfig)
        {
            cmbPrinterList.Items.Clear();
            var printers = LocalPrinter.GetLocalPrinters();
            foreach (var item in printers)
            {
                cmbPrinterList.Items.Add(item);
            }

            string printerName = null;
            bool printerSelected = false;

            if (_isPersonalConfig && personalConfig?.PrintConfigDict != null)
            {
                printerName = personalConfig.PrintConfigDict.PrinterName;
                printerSelected = personalConfig.PrintConfigDict.PrinterSelected;
            }
            else
            {
                printerName = printConfig.PrinterName;
                printerSelected = printConfig.PrinterSelected ?? false;
            }

      
            GroupBoxSelectPrinter.Visible = printerSelected;

            if (printerSelected && !string.IsNullOrEmpty(printerName))
            {
                cmbPrinterList.SelectedIndex = cmbPrinterList.FindString(printerName);
            }
        }

        /// <summary>
        /// 获取当前菜单ID
        /// </summary>
        private long GetCurrentMenuId()
        {
            if (PrintDataSources != null && PrintDataSources.Count > 0)
            {
                var firstItem = PrintDataSources[0];
                var billData = EntityMappingHelper.GetBillData(firstItem.GetType(), firstItem);
                var menuInfo = MainForm.Instance.AppContext.Db.Queryable<tb_MenuInfo>()
                    .Where(m => m.BizType == (int)billData.BizType)
                    .First();
                return menuInfo?.MenuID ?? 0;
            }
            return 0;
        }

        /// <summary>
        /// 获取菜单个人打印配置
        /// </summary>
        private tb_UIMenuPersonalization GetMenuPersonalPrintConfig(long menuId)
        {
            if (menuId == 0) return null;

            var userPersonalizedId = MainForm.Instance.AppContext.CurrentUser_Role_Personalized?.UserPersonalizedID;
            if (userPersonalizedId == null || userPersonalizedId == 0) return null;

            return MainForm.Instance.AppContext.Db.Queryable<tb_UIMenuPersonalization>()
                .Where(m => m.MenuID == menuId && m.UserPersonalizedID == userPersonalizedId)
                .First();
        }

        /// <summary>
        /// 更新窗体标题
        /// </summary>
        private void UpdateFormTitle()
        {
            if (_isPersonalConfig)
            {
                this.Text = "打印操作 - 【个人独有】";
            }
            else
            {
                this.Text = "打印操作";
            }
        }

        /// <summary>
        /// 更新配置状态指示器
        /// </summary>
        private void UpdateConfigStatusIndicator()
        {
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStates()
        {
            if (_isPersonalConfig)
            {
                btnSavePersonalConfig.Enabled = false;
                btnSavePersonalConfig.Values.Text = "已为个人独有";
                btnRevertToSystem.Enabled = true;
                btnRevertToSystem.Values.Text = "恢复系统配置";
            }
            else
            {
                btnSavePersonalConfig.Enabled = true;
                btnSavePersonalConfig.Values.Text = "保存为个人配置";
                btnRevertToSystem.Enabled = false;
                btnRevertToSystem.Values.Text = "使用系统配置";
            }
        }

        /// <summary>
        /// 保存为个人配置
        /// </summary>
        private async Task SaveAsPersonalConfigAsync()
        {
            try
            {
                var userPersonalizedId = MainForm.Instance.AppContext.CurrentUser_Role_Personalized?.UserPersonalizedID;
                if (userPersonalizedId == null || userPersonalizedId == 0)
                {
                    MessageBox.Show("无法获取用户个性化设置ID，请重新登录后重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var menuPersonalization = GetMenuPersonalPrintConfig(_currentMenuId);
                if (menuPersonalization == null)
                {
                    menuPersonalization = new tb_UIMenuPersonalization
                    {
                        MenuID = _currentMenuId,
                        UserPersonalizedID = userPersonalizedId.Value,
                        QueryConditionCols = 4,
                        Sort = 150
                    };
                    await MainForm.Instance.AppContext.Db.Insertable<tb_UIMenuPersonalization>(menuPersonalization).ExecuteReturnIdentityAsync();
                }

                var configData = new MenuPrintConfigData
                {
                    PrinterName = printConfig.PrinterName ?? string.Empty,
                    PrinterSelected = printConfig.PrinterSelected ?? false,
                    Landscape = printConfig.Landscape ?? false,
                    TemplateId = 0,
                    TemplateName = string.Empty,
                    IsDefaultTemplate = true,
                    BizType = printConfig.BizType,
                    BizName = printConfig.BizName ?? string.Empty,
                    LastModified = DateTime.Now
                };

                if (printConfig.tb_PrintTemplates != null && printConfig.tb_PrintTemplates.Count > 0)
                {
                    var defaultTemplate = printConfig.tb_PrintTemplates.FirstOrDefault(t => t.IsDefaultTemplate == true) ?? printConfig.tb_PrintTemplates[0];
                    configData.TemplateId = defaultTemplate.ID;
                    configData.TemplateName = defaultTemplate.Template_Name ?? string.Empty;
                    configData.IsDefaultTemplate = defaultTemplate.IsDefaultTemplate ?? false;
                }

                menuPersonalization.PrintConfigDict = configData;
                menuPersonalization.UsePersonalPrintConfig = true;

                await MainForm.Instance.AppContext.Db.Updateable<tb_UIMenuPersonalization>(menuPersonalization).ExecuteCommandAsync();

                _isPersonalConfig = true;
                UpdateFormTitle();
                UpdateButtonStates();

                MessageBox.Show("已成功保存为个人独有配置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex, "保存个人配置失败");
                MessageBox.Show($"保存个人配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 恢复使用系统配置
        /// </summary>
        private async Task RevertToSystemConfigAsync()
        {
            try
            {
                var menuPersonalization = GetMenuPersonalPrintConfig(_currentMenuId);
                if (menuPersonalization == null)
                {
                    MessageBox.Show("当前使用的是系统配置，无需恢复。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show("确定要恢复使用系统配置吗？恢复后将删除个人独有配置。", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    menuPersonalization.UsePersonalPrintConfig = false;
                    menuPersonalization.PrintConfigJson = null;

                    await MainForm.Instance.AppContext.Db.Updateable<tb_UIMenuPersonalization>(menuPersonalization).ExecuteCommandAsync();

                    _isPersonalConfig = false;
                    UpdateFormTitle();
                    UpdateButtonStates();

                    MessageBox.Show("已成功恢复使用系统配置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex, "恢复系统配置失败");
                MessageBox.Show($"恢复系统配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取当前打印配置（优先使用个人配置）
        /// </summary>
        public tb_PrintConfig GetEffectivePrintConfig()
        {
            if (_isPersonalConfig && printConfig != null)
            {
                var menuPersonalization = GetMenuPersonalPrintConfig(_currentMenuId);
                if (menuPersonalization?.PrintConfigDict != null)
                {
                    var personalConfig = menuPersonalization.PrintConfigDict;
                    printConfig.PrinterName = personalConfig.PrinterName;
                    printConfig.PrinterSelected = personalConfig.PrinterSelected;
                    printConfig.Landscape = personalConfig.Landscape;
                }
            }
            return printConfig;
        }

        private async void btnPrinter_Click(object sender, EventArgs e)
        {


            if (cmbPrinterList.SelectedItem != null)
            {
                printConfig.PrinterName = cmbPrinterList.SelectedItem.ToString();
            }
        
            if (printConfig.PrintConfigID > 0)
            {
                BusinessHelper.Instance.EditEntity(printConfig);
            }
            else
            {
                BusinessHelper.Instance.InitEntity(printConfig);
            }
            await MainForm.Instance.AppContext.Db.Updateable<tb_PrintConfig>(printConfig).ExecuteCommandAsync();

        }

        private void 设为默认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //设置一下默认，只能一行
            //改一行，就会修改其他行
            List<tb_PrintTemplate> Templates = bindingSourcePrintTemplate.DataSource as List<tb_PrintTemplate>;
            if (Templates != null && newSumDataGridView1.CurrentRow != null)
            {
                //找到当前选中的，要设置为默认的行。
                long currentID = (newSumDataGridView1.CurrentRow.DataBoundItem as tb_PrintTemplate).ID;

                //指定行为默认
                tb_PrintTemplate printTemplate = Templates.Where(t => t.ID == currentID).FirstOrDefault();
                printTemplate.IsDefaultTemplate = true;

                //其他行是否
                Templates.Where(t => t.ID != currentID).ToList().ForEach(w => w.IsDefaultTemplate = false);
                //保存
                MainForm.Instance.AppContext.Db.Updateable<tb_PrintTemplate>(Templates).ExecuteCommand();
                bindingSourcePrintTemplate.DataSource = Templates;
                bindingSourcePrintTemplate.EndEdit();
            }
        }

        private async void 复制当前模板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tb_PrintTemplate printTemplateOld = (bindingSourcePrintTemplate.Current as tb_PrintTemplate);

            RptNewTemplate rptNew = new RptNewTemplate();
            if (rptNew.ShowDialog() == DialogResult.OK)
            {
                tb_PrintTemplate printTemplate = new tb_PrintTemplate();
                printTemplate = printTemplateOld.Clone() as tb_PrintTemplate;
                printTemplate.Template_Name = rptNew.txtTemplateName.Text.Trim();
                printTemplate.ID = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                printTemplate.ActionStatus = ActionStatus.新增;
                bindingSourcePrintTemplate.Add(printTemplate);
                await MainForm.Instance.AppContext.Db.Storageable<tb_PrintTemplate>(printTemplate).ExecuteReturnEntityAsync();
                newSumDataGridView1.ReadOnly = false;
                newSumDataGridView1.Columns["Template_Name"].ReadOnly = false;
            }

        }

        private void 保存模板设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<tb_PrintTemplate> Templates = bindingSourcePrintTemplate.DataSource as List<tb_PrintTemplate>;
            MainForm.Instance.AppContext.Db.Updateable<tb_PrintTemplate>(Templates).ExecuteCommand();
        }

        private void btnToPDF_Click(object sender, EventArgs e)
        {
            PrintReport(RptMode.ToPDF);
        }

        private void btnSavePersonalConfig_Click(object sender, EventArgs e)
        {
            SaveAsPersonalConfigAsync();
        }

        private void btnRevertToSystem_Click(object sender, EventArgs e)
        {
            RevertToSystemConfigAsync();
        }

        private void ShowCustomExportDialog(FastReport.Report report)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "PDF Files (*.pdf)|*.pdf";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ExportHighQualityPdf(report, dialog.FileName);
                    MessageBox.Show("PDF exported successfully!");
                }
            }
        }

        // 高质量PDF导出方法
        public void ExportHighQualityPdf(FastReport.Report report, string fileName)
        {
            using (PDFExport pdfExport = new PDFExport())
            {
                // 关键优化设置
                pdfExport.EmbeddingFonts = true;
                pdfExport.GradientQuality = PDFExport.GradientQualityEnum.High;
                pdfExport.TextInCurves = true;
                //pdfExport.Compressed = false; // 关闭压缩提高清晰度
                // 应用设置并导出
                report.Export(pdfExport, fileName);
            }
        }


    }
}
