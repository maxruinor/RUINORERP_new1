using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.BaseForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StackExchange.Redis.Role;

namespace RUINORERP.UI.Report
{

    public class PrintHelper<M> where M : class
    {
        /// <summary>
        /// 获取打印机（支持优先级配置）
        /// 优先级: 菜单个人配置 > 用户业务类型配置 > 用户全局配置 > 系统配置 > 本地默认
        /// </summary>
        /// <param name="printConfig">系统打印配置</param>
        /// <param name="bizName">业务名称</param>
        /// <returns>打印机名称</returns>
        public static string GetPrinterWithPriority(tb_PrintConfig printConfig, string bizName)
        {
            var userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
            if (userPersonalized == null)
            {
                return GetSystemPrinter(printConfig);
            }

            // 优先级1: 菜单个人打印配置(tb_UIMenuPersonalization)
            var menuPrinter = GetMenuPersonalPrinter(printConfig);
            if (!string.IsNullOrEmpty(menuPrinter))
            {
                MainForm.Instance.uclog.AddLog($"使用菜单个人打印机: {menuPrinter}");
                return menuPrinter;
            }
         

            // 优先级4: 系统配置的该单据类型打印机
            var systemPrinter = GetSystemPrinter(printConfig);
            if (!string.IsNullOrEmpty(systemPrinter))
            {
                MainForm.Instance.uclog.AddLog($"使用系统打印机: {systemPrinter}");
                return systemPrinter;
            }

            // 优先级5: 客户端本地默认打印机
            var localPrinter = LocalPrinter.DefaultPrinter();
            MainForm.Instance.uclog.AddLog($"使用本地默认打印机: {localPrinter}");
            return localPrinter;
        }

        /// <summary>
        /// 获取菜单个人打印配置中的打印机
        /// </summary>
        private static string GetMenuPersonalPrinter(tb_PrintConfig printConfig)
        {
            try
            {
                if (printConfig == null) return null;

                var menuInfo = MainForm.Instance.AppContext.Db.Queryable<tb_MenuInfo>()
                    .Where(m => m.BizType == printConfig.BizType )
                    .First();
                if (menuInfo == null) return null;

                var userPersonalizedId = MainForm.Instance.AppContext.CurrentUser_Role_Personalized?.UserPersonalizedID;
                if (userPersonalizedId == null || userPersonalizedId == 0) return null;

                var menuPersonalization = MainForm.Instance.AppContext.Db.Queryable<tb_UIMenuPersonalization>()
                    .Where(m => m.MenuID == menuInfo.MenuID && m.UserPersonalizedID == userPersonalizedId)
                    .First();

                if (menuPersonalization?.UsePersonalPrintConfig == true && menuPersonalization.PrintConfigDict != null)
                {
                    var config = menuPersonalization.PrintConfigDict;
                    if (config.PrinterSelected && !string.IsNullOrEmpty(config.PrinterName))
                    {
                        return config.PrinterName;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogWarning(ex, "获取菜单个人打印机失败");
            }
            return null;
        }

        /// <summary>
        /// 从系统配置获取打印机
        /// </summary>
        private static string GetSystemPrinter(tb_PrintConfig printConfig)
        {
            if (printConfig?.PrinterSelected == true && !string.IsNullOrEmpty(printConfig.PrinterName))
            {
                return printConfig.PrinterName;
            }
            return null;
        }

        /// <summary>
        /// 获取个人配置的打印模板
        /// 优先级: 菜单个人配置模板 > 系统默认模板
        /// </summary>
        private static tb_PrintTemplate GetPersonalPrintTemplate(tb_PrintConfig printConfig)
        {
            try
            {
                if (printConfig == null) return null;

                var menuInfo = MainForm.Instance.AppContext.Db.Queryable<tb_MenuInfo>()
                    .Where(m => m.BizType == printConfig.BizType)
                    .First();
                if (menuInfo == null) return null;

                var userPersonalizedId = MainForm.Instance.AppContext.CurrentUser_Role_Personalized?.UserPersonalizedID;
                if (userPersonalizedId == null || userPersonalizedId == 0) return null;

                var menuPersonalization = MainForm.Instance.AppContext.Db.Queryable<tb_UIMenuPersonalization>()
                    .Where(m => m.MenuID == menuInfo.MenuID && m.UserPersonalizedID == userPersonalizedId)
                    .First();

                if (menuPersonalization?.UsePersonalPrintConfig == true && menuPersonalization.PrintConfigDict != null)
                {
                    var config = menuPersonalization.PrintConfigDict;
                    if (config.TemplateId > 0)
                    {
                        // 根据个人配置中的模板ID获取模板
                        var template = MainForm.Instance.AppContext.Db.Queryable<tb_PrintTemplate>()
                            .Where(t => t.ID == config.TemplateId)
                            .First();
                        if (template != null && template.TemplateFileStream != null)
                        {
                            return template;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogWarning(ex, "获取菜单个人打印模板失败");
            }
            return null;
        }

        /// <summary>
        /// 检查打印机是否可用
        /// </summary>
        private static bool CheckPrinterAvailable(string printerName)
        {
            if (string.IsNullOrEmpty(printerName))
                return false;

            var printers = LocalPrinter.GetLocalPrinters();
            return printers.Any(p => p.Equals(printerName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 打印指定主表明细
        /// </summary>
        /// <param name="EditEntitys"></param>
        /// <param name="rptMode"></param>
        /// <param name="printConfig"></param>
        public static async Task<bool> Print(List<M> EditEntitys, RptMode rptMode, tb_PrintConfig printConfig)
        {
            bool rs = false;
            try
            {
                if (!await CheckPrinterAndDataAsync(EditEntitys, rptMode))
                {
                    return rs;
                }
                
                // 在后台线程执行打印操作，避免阻塞UI
                rs = await Task.Run(async () =>
                {
                    try
                    {
                        return await ExecutePrintAsync(EditEntitys, rptMode, printConfig);
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.LogError(ex, "后台打印任务执行失败");
                        MessageBox.Show($"打印失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "打印任务启动失败");
                MessageBox.Show($"打印失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return rs;
        }

        /// <summary>
        /// 在后台线程执行实际的打印操作
        /// </summary>
        private static async Task<bool> ExecutePrintAsync(List<M> EditEntitys, RptMode rptMode, tb_PrintConfig printConfig)
        {
            bool rs = false;
            List<CurrentUserInfo> currUserInfos = new List<CurrentUserInfo>();
            currUserInfos.Add(MainForm.Instance.AppContext.CurUserInfo);

            List<tb_Company> companyInfos = new List<tb_Company>();
            companyInfos.Add(MainForm.Instance.AppContext.CompanyInfo);
            BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");
            #region 要打印的数据源

            List<object> objlist = new List<object>();
            List<M> mlist = new List<M>();

            FastReport.Report FReport = new FastReport.Report();
            int counter = 0;

            try
            {
                foreach (var item in EditEntitys)
                {
                    counter++;
                    mlist = new List<M>();
                    long keyid = (item as BaseEntity).PrimaryKeyID;
                    //没有取到主键值时
                    string PrimaryKey = RUINORERP.UI.Common.UIHelper.GetPrimaryKeyColName(typeof(M));
                    if (keyid == 0 && !string.IsNullOrEmpty(PrimaryKey))
                    {
                        long.TryParse(item.GetPropertyValue(PrimaryKey).ToString(), out keyid);
                    }
                    var PrintData = await ctr.GetPrintDataSource(keyid).ConfigureAwait(false) as List<M>;
                    if (PrintData.Count == 0)
                    {
                        MessageBox.Show("请联系管理员，确认打印的数据是否能正常查询到。");
                        return false;
                    }
                    mlist.Add(PrintData[0]);//按主键查的。一行一个
                    if (rptMode == RptMode.DESIGN)
                    {
                        objlist.Add(mlist[0]);
                        string PKFieldName = BaseUIHelper.GetEntityPrimaryKey(PrintData[0].GetType());
                        // 设计模式需要在UI线程执行
                        MainForm.Instance.Invoke(new Action(() => 
                        {
                            DESIGN(printConfig, objlist, companyInfos, currUserInfos, PKFieldName);
                        }));
                        ////释放资源
                        FReport.Dispose();
                        return rs;
                    }
                    else
                    {
                        //注意  数据源中的子对象 必须是   [Browsable(false)] 不能有这个特性。否则无法显示，
                        //报表控件注册数据
                        FReport.RegisterData(mlist, "rd");
                        FReport.RegisterData(currUserInfos, "currUserInfo");

                        //如果打印的实际中有项目小组，就去项目小组找他的上级部门，他的部分属性哪个公司，就打印哪个公司的抬头
                        //TODO by watson

                        FReport.RegisterData(companyInfos, "companyInfo");
                        if (printConfig.tb_PrintTemplates == null)
                        {
                            MessageBox.Show("请先配置正确的打印模板！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            FReport.Dispose();
                            return rs;
                        }

                        // 优先使用个人配置的模板
                        var printTemplate = GetPersonalPrintTemplate(printConfig);
                        if (printTemplate == null)
                        {
                            // 使用系统默认模板
                            printTemplate = printConfig.tb_PrintTemplates.Where(t => t.IsDefaultTemplate == true).FirstOrDefault();
                            if (printTemplate == null && printConfig.tb_PrintTemplates.Count > 0)
                            {
                                printTemplate = printConfig.tb_PrintTemplates[0];
                                MainForm.Instance.uclog.AddLog(string.Format("当前打印配置:{0}的没有指定默认值，系统使用了第一个模板{1}打印。", printConfig.BizName, printTemplate.Template_Name));
                            }
                        }
                        else
                        {
                            MainForm.Instance.uclog.AddLog($"使用个人打印模板: {printTemplate.Template_Name}");
                        }

                        if (printTemplate != null && printTemplate.TemplateFileStream != null)
                        {
                            byte[] ReportBytes = (byte[])printTemplate.TemplateFileStream;
                            using (System.IO.MemoryStream Stream = new System.IO.MemoryStream(ReportBytes))
                            {
                                FReport.Load(Stream);
                            }
                        }
                        else
                        {
                            MessageBox.Show("请先配置正确的打印模板！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ////释放资源
                            FReport.Dispose();
                            return rs;
                        }
                        if (counter == 1)
                        {
                            //准备
                            FReport.Prepare();
                        }
                        else
                        {
                            //准备合并上次的
                            FReport.Prepare(true);
                        }
                            ////////打印////////
                            // 使用新的优先级配置获取打印机
                            string printerName = GetPrinterWithPriority(printConfig, printConfig?.BizName);
                            if (!string.IsNullOrEmpty(printerName))
                            {
                                FReport.PrintSettings.ShowDialog = false;
                                FReport.PrintSettings.Printer = printerName;
                            }





                        }
                    }

                if (rptMode == RptMode.PREVIEW)
                {
                    // 预览需要在UI线程显示窗体
                    MainForm.Instance.Invoke(new Action(() =>
                    {
                        try
                        {
                            RptPreviewForm frm = new RptPreviewForm();
                            frm.Text = printConfig.BizName + "打印预览";
                            frm.MyReport = FReport;
                            frm.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger.LogError(ex, "打印预览窗体显示失败");
                            MessageBox.Show($"预览失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }));
                }
                else
                {
                    try
                    {
                        // 实际打印操作
                        FReport.PrintPrepared();
                        await UpdatePrintStatusAsync(EditEntitys).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.LogError(ex, "实际打印操作失败");
                        MessageBox.Show($"打印失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError("打印报表异常。", ex);
                MessageBox.Show($"打印过程发生异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ////释放资源
                FReport?.Dispose();
            }


            rs = true;
            #endregion
            
            return rs;
        }


        //更新打印状态，打印次数加1
        public static async Task UpdatePrintStatus(List<M> EditEntitys, string PKFieldName = null)
        {
            // 在后台线程更新打印状态
            await Task.Run(async () =>
            {
                //打印状态更新
                foreach (var editEntity in EditEntitys)
                {
                    if (editEntity.ContainsProperty("PrintStatus"))
                    {
                        int PrintStatus = (int)ReflectionHelper.GetPropertyValue(editEntity, "PrintStatus");
                        PrintStatus++;
                        editEntity.SetPropertyValue("PrintStatus", PrintStatus);
                        var dt = new Dictionary<string, object>();
                        string PKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
                        if (string.IsNullOrEmpty(PKCol))
                        {
                            PKCol = PKFieldName;
                            //PKCol = "PrimaryKeyID";
                        }
                        //视图就会传空过来
                        if (string.IsNullOrEmpty(PKCol))
                        {
                            continue;
                        }
                        long pkid = (long)ReflectionHelper.GetPropertyValue(editEntity, PKCol);
                        dt.Add(PKCol, pkid);
                        dt.Add("PrintStatus", PrintStatus);
                        await MainForm.Instance.AppContext.Db.Updateable(dt).AS(editEntity.GetType().Name).WhereColumns(PKCol).ExecuteCommandAsync();
                    }
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 异步更新打印状态，避免阻塞UI线程
        /// </summary>
        public static async Task UpdatePrintStatusAsync(List<M> EditEntitys, string PKFieldName = null)
        {
            await UpdatePrintStatus(EditEntitys, PKFieldName).ConfigureAwait(false);
        }





        /// <summary>
        /// 设计时取了传入数据指定对象结构的数据 来打印。数据 在前面已经构建好了。
        /// 打印状态如果要处理则在调用成功这个方法后自行处理
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="CustomData"></param>
        /// <param name="rptMode"></param>
        /// <param name="printConfig"></param>
        /// <returns></returns>
        public static bool PrintCustomData<C>(object CustomData, RptMode rptMode, tb_PrintConfig printConfig)
        {
            bool rs = false;
            List<CurrentUserInfo> currUserInfo = new List<CurrentUserInfo>();
            currUserInfo.Add(MainForm.Instance.AppContext.CurUserInfo);

            List<tb_Company> companyInfo = new List<tb_Company>();
            companyInfo.Add(MainForm.Instance.AppContext.CompanyInfo);

            #region 要打印的数据源

            List<object> objlist = new List<object>();
            objlist.Add(CustomData);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            //添加主表内容

            if (rptMode == RptMode.DESIGN)
            {
                //视图的打印不要更新打印次数
                DESIGN(printConfig, objlist, companyInfo, currUserInfo);
                ////释放资源
                FReport.Dispose();
                return rs;
            }
            else
            {
                FReport.RegisterData(objlist, "rd");
                FReport.RegisterData(currUserInfo, "currUserInfo");
                FReport.RegisterData(companyInfo, "companyInfo");

                // 优先使用个人配置的模板
                var printTemplate = GetPersonalPrintTemplate(printConfig);
                if (printTemplate == null)
                {
                    printTemplate = printConfig.tb_PrintTemplates.Where(t => t.IsDefaultTemplate == true).FirstOrDefault();
                    if (printTemplate == null && printConfig.tb_PrintTemplates.Count > 0)
                    {
                        printTemplate = printConfig.tb_PrintTemplates[0];
                        MainForm.Instance.uclog.AddLog(string.Format("当前打印配置:{0}的没有指定默认值，系统使用了第一个模板{1}打印。", printConfig.BizName, printTemplate.Template_Name));
                    }
                }
                else
                {
                    MainForm.Instance.uclog.AddLog($"使用个人打印模板: {printTemplate.Template_Name}");
                }

                if (printTemplate != null && printTemplate.TemplateFileStream != null)
                {
                    byte[] ReportBytes = (byte[])printTemplate.TemplateFileStream;
                    using (System.IO.MemoryStream Stream = new System.IO.MemoryStream(ReportBytes))
                    {
                        FReport.Load(Stream);
                    }
                }
                else
                {
                    MessageBox.Show("请先配置正确的打印模板！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ////释放资源
                    FReport.Dispose();
                    return rs;
                }
                //if (counter == 1)
                //{
                //    //准备
                FReport.Prepare();
                //}
                //else
                //{
                //    //准备合并上次的1
                //    FReport.Prepare(true);
                //}
                ////////打印////////
                // 使用新的优先级配置获取打印机
                string printerNameCustom = GetPrinterWithPriority(printConfig, printConfig?.BizName);
                if (!string.IsNullOrEmpty(printerNameCustom))
                {
                    FReport.PrintSettings.ShowDialog = false;
                    FReport.PrintSettings.Printer = printerNameCustom;
                }

                }


            


            if (rptMode == RptMode.PREVIEW)
            {
                RptPreviewForm frm = new RptPreviewForm();
                frm.Text = printConfig.BizName + "打印预览";
                frm.MyReport = FReport;
                frm.ShowDialog();
            }
            else
            {
                FReport.PrintPrepared();

            }

            ////释放资源
            FReport.Dispose();
            #endregion
            return rs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="EditEntitys"></param>
        /// <returns></returns>
        public static tb_PrintConfig GetPrintConfig<C>()
        {
            
            //这里只是取打印配置信息
            CommBillData cbd = new CommBillData();
            cbd = EntityMappingHelper.GetBillData(typeof(C), null);
            return GetPrintConfig(cbd);
        }


        public static tb_PrintConfig GetPrintConfig(List<M> EditEntitys)
        {
            
            //这里只是取打印配置信息
            CommBillData cbd = new CommBillData();
            if (EditEntitys != null && EditEntitys.Count > 0)
            {
                cbd = EntityMappingHelper.GetBillData(typeof(M), EditEntitys[0]);
            }
            else
            {
                cbd = EntityMappingHelper.GetBillData(typeof(M), null);
            }
;
            return GetPrintConfig(cbd);
        }

        /// <summary>
        /// BizName 通过这两个来决定打印配置的 BizType
        /// </summary>
        /// <param name="cbd"></param>
        /// <returns></returns>
        private static tb_PrintConfig GetPrintConfig(CommBillData cbd)
        {
            tb_PrintConfig printConfig = MainForm.Instance.AppContext.Db.Queryable<tb_PrintConfig>()
                .Includes(t => t.tb_PrintTemplates)
                .Where(c => c.BizName == cbd.BizName && c.BizType == (int)cbd.BizType)
                .First();
            if (printConfig == null || printConfig.tb_PrintTemplates == null)
            {
                printConfig = new tb_PrintConfig();
                printConfig.Config_Name = "系统配置" + cbd.BizName;
                printConfig.PrinterSelected = false;
                printConfig.BizName = cbd.BizName;
                printConfig.BizType = (int)cbd.BizType;
                //新建一个配置
                printConfig = MainForm.Instance.AppContext.Db.Insertable<tb_PrintConfig>(printConfig).ExecuteReturnEntity();
            }
            else
            {
                // 使用优先级配置获取实际会使用的打印机（支持个人配置）
                string effectivePrinterName = GetPrinterWithPriority(printConfig, printConfig?.BizName);

                if (printConfig.tb_PrintTemplates.Count == 0 || string.IsNullOrEmpty(effectivePrinterName))
                {
                    MessageBox.Show("请先配置打印参数后再重新打印！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (!CheckPrinter(effectivePrinterName))
                    {
                        MessageBox.Show($"系统检测到打印机{effectivePrinterName}的状态无法正常工作，检查确认!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // 检查是否有个人配置，如果有则更新模板信息
                var personalTemplate = GetPersonalPrintTemplate(printConfig);
                if (personalTemplate != null)
                {
                    MainForm.Instance.uclog.AddLog($"使用个人打印模板: {personalTemplate.Template_Name}");
                }
                else
                {
                    var printTemplate = printConfig.tb_PrintTemplates.Where(t => t.IsDefaultTemplate == true).FirstOrDefault();
                    if (printTemplate == null && printConfig.tb_PrintTemplates.Count > 0)
                    {
                        printTemplate = printConfig.tb_PrintTemplates[0];
                        MainForm.Instance.uclog.AddLog(string.Format("当前打印配置:{0}的没有指定默认值，系统使用了第一个模板{1}打印。", printConfig.BizName, printTemplate.Template_Name));
                    }
                }
            }
            return printConfig;
        }


        /// <summary>
        /// 异步打印前的检测工作，避免阻塞UI线程
        /// </summary>
        private static async Task<bool> CheckPrinterAndDataAsync(List<M> EditEntitys, RptMode rptMode)
        {
            bool rs = false;
            if (EditEntitys == null || EditEntitys.Count == 0)
            {
                MessageBox.Show("没有要打印的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return rs;
            }
            foreach (var item in EditEntitys)
            {
                if (item == null)
                {
                    MessageBox.Show("打印的数据不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rs = false;
                    break;
                }
            }
            
            // 如果是打印模式，检查打印机状态
            if (rptMode == RptMode.PRINT && EditEntitys != null && EditEntitys.Count > 0)
            {
                // 获取打印配置
                CommBillData cbd = EntityMappingHelper.GetBillData(typeof(M), EditEntitys[0]);
                var printConfig = await Task.Run(() => GetPrintConfig(cbd)).ConfigureAwait(false);
                
                // 使用优先级配置获取实际会使用的打印机（支持个人配置）
                string effectivePrinterName = GetPrinterWithPriority(printConfig, printConfig?.BizName);
                
                if (!string.IsNullOrEmpty(effectivePrinterName))
                {
                    // 在后台线程检查实际打印机状态，避免阻塞UI
                    bool printerOk = await Task.Run(() => CheckPrinter(effectivePrinterName)).ConfigureAwait(false);
                    if (!printerOk)
                    {
                        MessageBox.Show($"系统检测到打印机 {effectivePrinterName} 的状态无法正常工作，请检查确认！", 
                            "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            
            rs = true;
            return rs;
        }

 



        private static void DESIGN(tb_PrintConfig printConfig,
            List<object> objlist,
            List<tb_Company> companies = null,
            List<CurrentUserInfo> currentUsers = null,
        string PKFieldName = null
        )
        {
            RptPrintConfig frmPrintConfig = new RptPrintConfig();
            frmPrintConfig.printConfig = printConfig;
            frmPrintConfig.PrintDataSources = objlist;
            frmPrintConfig.PKFieldName = PKFieldName;
            frmPrintConfig.companyInfos = companies;
            frmPrintConfig.currUserInfos = currentUsers;
            frmPrintConfig.ShowDialog();
        }




        public static bool CheckPrinter(string printerName1)
        {
            if (string.IsNullOrEmpty(printerName1))
            {
                return false;
            }

            try
            {
                // 增加超时控制，避免WMI查询长时间阻塞
                using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    return Task.Run(() =>
                    {
                        try
                        {
                            System.Management.ManagementScope scope = new System.Management.ManagementScope(@"\root\cimv2");
                            scope.Connect();

                            // Select Printers from WMI Object Collections
                            System.Management.ManagementObjectSearcher searcher = new
                             System.Management.ManagementObjectSearcher("SELECT * FROM Win32_Printer");

                            string printerName = "";
                            foreach (System.Management.ManagementObject printer in searcher.Get())
                            {
                                printerName = printer["Name"].ToString().ToLower();
                                if (printerName.IndexOf(printerName1.ToLower()) > -1)
                                {

                                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                                    {
                                        return false;
                                        // printer is offline by user

                                    }
                                    else
                                    {
                                        // printer is not offline

                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger.LogWarning(ex, $"检查打印机状态时发生异常: {printerName1}");
                            // 如果检查失败，默认返回true，允许继续打印
                            return true;
                        }
                    }, cts.Token).Result;
                }
            }
            catch (TimeoutException)
            {
                MainForm.Instance.logger.LogWarning($"检查打印机 {printerName1} 超时，默认认为打印机可用");
                return true; // 超时情况下默认允许打印，避免阻塞业务
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, $"检查打印机状态失败: {printerName1}");
                return true; // 发生异常时默认允许打印
            }
        }
    }
}
