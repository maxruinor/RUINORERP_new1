using RUINORERP.Business;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using static StackExchange.Redis.Role;

namespace RUINORERP.UI.Report
{

    public class PrintHelper<M> where M : class
    {


        public static async void Print_old(List<M> EditEntitys, RptMode rptMode)
        {
            if (EditEntitys == null || EditEntitys.Count == 0)
            {
                MessageBox.Show("没有要打印的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var editEntity in EditEntitys)
            {
                if (editEntity == null)
                {
                    MessageBox.Show("没有要打印的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
                //这里只是取打印配置信息
                CommBillData cbd = bcf.GetBillData<M>(editEntity);
                string PKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
                long pkid = 0;
                pkid = (long)ReflectionHelper.GetPropertyValue(editEntity, PKCol);
                cbd = bcf.GetBillData<M>(editEntity as M);

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


                    #region 要打印的数据源
                    BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");
                    List<object> objlist = new List<object>();
                    List<M> mlist = new List<M>();
                    foreach (var item in EditEntitys)
                    {
                        long keyid = (item as BaseEntity).PrimaryKeyID;
                        //没有取到主键值时
                        string PrimaryKey = RUINORERP.UI.Common.UIHelper.GetPrimaryKeyColName(typeof(M));
                        if (keyid == 0 && !string.IsNullOrEmpty(PrimaryKey))
                        {
                            long.TryParse(item.GetPropertyValue(PrimaryKey).ToString(), out keyid);
                        }
                        var PrintData = await ctr.GetPrintDataSource(keyid) as List<M>;
                        mlist.Add(PrintData[0]);//按主键查的。一行一个
                    }
                    foreach (var item in mlist)
                    {
                        objlist.Add(item);
                    }
                    List<ICurrentUserInfo> currUserInfo = new List<ICurrentUserInfo>();
                    currUserInfo.Add(MainForm.Instance.AppContext.CurUserInfo);
                    #endregion

                    if (printConfig.tb_PrintTemplates.Count == 0)
                    {
                        MessageBox.Show("请先配置打印模板后再重新打印！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (RptMode.DESIGN == rptMode)
                        {
                            DESIGN(printConfig, objlist);
                        }
                        return;
                    }

                    if (string.IsNullOrEmpty(printConfig.PrinterName))
                    {
                        MessageBox.Show("请先配置打印机后再重新打印！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (RptMode.DESIGN == rptMode)
                        {
                            DESIGN(printConfig, objlist);
                        }
                        return;
                    }

                    if (!CheckPrinter(printConfig.PrinterName))
                    {
                        if (MessageBox.Show($"系统检测到打印机{printConfig.PrinterName}的状态无法正常工作，检查确认!", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                            return;
                    }
                    var printTemplate = printConfig.tb_PrintTemplates.Where(t => t.IsDefaultTemplate == true).FirstOrDefault();
                    if (printTemplate == null)
                    {
                        printTemplate = printConfig.tb_PrintTemplates[0];
                        MainForm.Instance.uclog.AddLog(string.Format("当前打印配置:{0}的没有指定默认值，系统使用了第一个模板{1}打印。", printConfig.BizName, printTemplate.Template_Name));
                    }

                    FastReport.Report FReport;
                    FReport = new FastReport.Report();
                    //报表控件注册数据
                    FReport.RegisterData(objlist, "rd");
                    FReport.RegisterData(currUserInfo, "currUserInfo");
                    if (printTemplate.TemplateFileStream != null)
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
                        return;
                    }
                    switch (rptMode)
                    {
                        case RptMode.DESIGN:
                            DESIGN(printConfig, objlist);
                            break;
                        case RptMode.PREVIEW:
                            RptPreviewForm frm = new RptPreviewForm();
                            frm.Text = cbd.BizName + "打印预览";
                            frm.MyReport = FReport;
                            frm.ShowDialog();
                            break;
                        case RptMode.PRINT:
                            ////////打印////////
                            //设置默认打印机
                            if (printConfig.PrinterSelected)
                            {
                                FReport.PrintSettings.ShowDialog = false;
                                FReport.PrintSettings.Printer = printConfig.PrinterName;
                            }
                            else
                            {
                                FReport.PrintPrepared();
                            }

                            FReport.Prepare();
                            FReport.Print();
                            ////释放资源
                            FReport.Dispose();

                            //打印状态更新
                            if (editEntity.ContainsProperty("PrintStatus"))
                            {
                                int PrintStatus = (int)ReflectionHelper.GetPropertyValue(editEntity, "PrintStatus");
                                PrintStatus++;
                                editEntity.SetPropertyValue("PrintStatus", PrintStatus);
                                //await AppContext.Db.Updateable<T>(EditEntity).UpdateColumns(t => new { t.PrintStatus }).ExecuteCommandAsync();
                                var dt = new Dictionary<string, object>();
                                dt.Add(PKCol, pkid);
                                dt.Add("PrintStatus", PrintStatus);
                                await MainForm.Instance.AppContext.Db.Updateable(dt).AS(typeof(M).Name).WhereColumns(PKCol).ExecuteCommandAsync();
                            }
                            break;
                    }
                }
            }

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
            if (!CheckPrinterAndData(EditEntitys, rptMode))
            {
                return rs;
            }
            List<ICurrentUserInfo> currUserInfo = new List<ICurrentUserInfo>();
            currUserInfo.Add(MainForm.Instance.AppContext.CurUserInfo);
            BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");

            #region 要打印的数据源

            List<object> objlist = new List<object>();
            List<M> mlist = new List<M>();

            FastReport.Report FReport;
            FReport = new FastReport.Report();
            int counter = 0;
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
                var PrintData = await ctr.GetPrintDataSource(keyid) as List<M>;
                if (PrintData.Count == 0)
                {
                    MessageBox.Show("请联系管理员，确认打印的数据是否能正常查询到。");
                    return false;
                }
                mlist.Add(PrintData[0]);//按主键查的。一行一个
                if (rptMode == RptMode.DESIGN)
                {
                    objlist.Add(mlist[0]);
                    DESIGN(printConfig, objlist);
                    ////释放资源
                    FReport.Dispose();
                    return rs;
                }
                else
                {

                    //报表控件注册数据
                    FReport.RegisterData(mlist, "rd");
                    FReport.RegisterData(currUserInfo, "currUserInfo");

                    var printTemplate = printConfig.tb_PrintTemplates.Where(t => t.IsDefaultTemplate == true).FirstOrDefault();
                    if (printTemplate == null && printConfig != null && printConfig.tb_PrintTemplates.Count > 0)
                    {
                        printTemplate = printConfig.tb_PrintTemplates[0];
                        MainForm.Instance.uclog.AddLog(string.Format("当前打印配置:{0}的没有指定默认值，系统使用了第一个模板{1}打印。", printConfig.BizName, printTemplate.Template_Name));
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
                    //设置默认打印机
                    if (printConfig.PrinterSelected)
                    {
                        FReport.PrintSettings.ShowDialog = false;
                        FReport.PrintSettings.Printer = printConfig.PrinterName;
                    }
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
                        long pkid = (long)ReflectionHelper.GetPropertyValue(editEntity, PKCol);
                        dt.Add(PKCol, pkid);
                        dt.Add("PrintStatus", PrintStatus);
                        await MainForm.Instance.AppContext.Db.Updateable(dt).AS(typeof(M).Name).WhereColumns(PKCol).ExecuteCommandAsync();
                    }
                }
            }

            ////释放资源
            FReport.Dispose();
            rs = true;
            #endregion
            return rs;
        }



        /// <summary>
        /// 设计时取了传入数据 源的第一个类型作为单号这些。所以这里master如果不是主要类型来标识业务的。就后面添加
        /// </summary>
        /// <param name="EditEntitys"></param>
        /// <param name="rptMode"></param>
        /// <param name="printConfig"></param>
        public static async Task<bool> PrintMasterChild(object Master, List<M> EditEntitys, RptMode rptMode, tb_PrintConfig printConfig)
        {
            bool rs = false;
            if (!CheckPrinterAndData(EditEntitys, rptMode))
            {
                return rs;
            }
            List<ICurrentUserInfo> currUserInfo = new List<ICurrentUserInfo>();
            currUserInfo.Add(MainForm.Instance.AppContext.CurUserInfo);
            BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");
            #region 要打印的数据源

            List<object> objlist = new List<object>();


            List<M> Lineslist = new List<M>();

            FastReport.Report FReport;
            FReport = new FastReport.Report();
            int counter = 0;
            foreach (var item in EditEntitys)
            {
                counter++;
                Lineslist = new List<M>();
                long keyid = (item as BaseEntity).PrimaryKeyID;
                //没有取到主键值时
                string PrimaryKey = RUINORERP.UI.Common.UIHelper.GetPrimaryKeyColName(typeof(M));
                if (keyid == 0 && !string.IsNullOrEmpty(PrimaryKey))
                {
                    long.TryParse(item.GetPropertyValue(PrimaryKey).ToString(), out keyid);
                    var PrintData = await ctr.GetPrintDataSource(keyid) as List<M>;
                    Lineslist.Add(PrintData[0]);//按主键查的。一行一个
                }
                else
                {
                    Lineslist.Add(item);//直接添加原始明细数据
                }
                //添加主表内容

                if (rptMode == RptMode.DESIGN)
                {
                    objlist.Add(Lineslist[0]);
                    objlist.Add(Master);
                    DESIGN(printConfig, objlist);
                    ////释放资源
                    FReport.Dispose();
                    return rs;
                }
                else
                {
                    objlist.Add(Lineslist[0]);
                    objlist.Add(Master);
                    FReport.RegisterData(objlist, "master");
                    //报表控件注册数据
                    FReport.RegisterData(Lineslist, "rd");
                    FReport.RegisterData(currUserInfo, "currUserInfo");

                    var printTemplate = printConfig.tb_PrintTemplates.Where(t => t.IsDefaultTemplate == true).FirstOrDefault();
                    if (printTemplate == null && printConfig != null && printConfig.tb_PrintTemplates.Count > 0)
                    {
                        printTemplate = printConfig.tb_PrintTemplates[0];
                        MainForm.Instance.uclog.AddLog(string.Format("当前打印配置:{0}的没有指定默认值，系统使用了第一个模板{1}打印。", printConfig.BizName, printTemplate.Template_Name));
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
                    //设置默认打印机
                    if (printConfig.PrinterSelected)
                    {
                        FReport.PrintSettings.ShowDialog = false;
                        FReport.PrintSettings.Printer = printConfig.PrinterName;
                    }
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
                        long pkid = (long)ReflectionHelper.GetPropertyValue(editEntity, PKCol);
                        dt.Add(PKCol, pkid);
                        dt.Add("PrintStatus", PrintStatus);
                        await MainForm.Instance.AppContext.Db.Updateable(dt).AS(typeof(M).Name).WhereColumns(PKCol).ExecuteCommandAsync();
                    }
                }
            }

            ////释放资源
            FReport.Dispose();
            #endregion
            return rs;
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
            List<ICurrentUserInfo> currUserInfo = new List<ICurrentUserInfo>();
            currUserInfo.Add(MainForm.Instance.AppContext.CurUserInfo);
            #region 要打印的数据源

            List<object> objlist = new List<object>();
            objlist.Add(CustomData);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            //添加主表内容

            if (rptMode == RptMode.DESIGN)
            {

                DESIGN(printConfig, objlist);
                ////释放资源
                FReport.Dispose();
                return rs;
            }
            else
            {
                //报表控件注册数据
                FReport.RegisterData(objlist, "rd");
                FReport.RegisterData(currUserInfo, "currUserInfo");

                var printTemplate = printConfig.tb_PrintTemplates.Where(t => t.IsDefaultTemplate == true).FirstOrDefault();
                if (printTemplate == null && printConfig != null && printConfig.tb_PrintTemplates.Count > 0)
                {
                    printTemplate = printConfig.tb_PrintTemplates[0];
                    MainForm.Instance.uclog.AddLog(string.Format("当前打印配置:{0}的没有指定默认值，系统使用了第一个模板{1}打印。", printConfig.BizName, printTemplate.Template_Name));
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
                //    //准备合并上次的
                //    FReport.Prepare(true);
                //}
                ////////打印////////
                //设置默认打印机
                if (printConfig.PrinterSelected)
                {
                    FReport.PrintSettings.ShowDialog = false;
                    FReport.PrintSettings.Printer = printConfig.PrinterName;
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
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            //这里只是取打印配置信息
            CommBillData cbd = new CommBillData();
            cbd = bcf.GetBillData(typeof(C), null);
            return GetPrintConfig(cbd);
        }


        public static tb_PrintConfig GetPrintConfig(List<M> EditEntitys)
        {
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            //这里只是取打印配置信息
            CommBillData cbd = new CommBillData();
            if (EditEntitys != null && EditEntitys.Count > 0)
            {
                cbd = bcf.GetBillData(typeof(M), EditEntitys[0]);
            }
            else
            {
                cbd = bcf.GetBillData(typeof(M), null);
            }

            string PKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
            long pkid = 0;
            if (!string.IsNullOrEmpty(PKCol))
            {
                pkid = (long)ReflectionHelper.GetPropertyValue(EditEntitys[0], PKCol);
            }

            cbd = bcf.GetBillData<M>(EditEntitys[0] as M);
            return GetPrintConfig(cbd);
        }

        /// <summary>
        /// BizName 通过这两个来决定打印配置的 BizType
        /// </summary>
        /// <param name="cbd"></param>
        /// <returns></returns>
        private static tb_PrintConfig GetPrintConfig(CommBillData cbd)
        {
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
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
                if (printConfig.tb_PrintTemplates.Count == 0 || string.IsNullOrEmpty(printConfig.PrinterName))
                {
                    MessageBox.Show("请先配置打印参数后再重新打印！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (!CheckPrinter(printConfig.PrinterName))
                    {
                        // (MessageBox.Show($"系统检测到打印机{printConfig.PrinterName}的状态无法正常工作，检查确认!", "提示", MessageBoxButtons.RetryCancel);  
                        MessageBox.Show($"系统检测到打印机{printConfig.PrinterName}的状态无法正常工作，检查确认!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                var printTemplate = printConfig.tb_PrintTemplates.Where(t => t.IsDefaultTemplate == true).FirstOrDefault();
                if (printTemplate == null && printConfig.tb_PrintTemplates.Count > 0)
                {
                    printTemplate = printConfig.tb_PrintTemplates[0];
                    MainForm.Instance.uclog.AddLog(string.Format("当前打印配置:{0}的没有指定默认值，系统使用了第一个模板{1}打印。", printConfig.BizName, printTemplate.Template_Name));
                }
                //if (printTemplate.TemplateFileStream == null)
                //{
                //    MessageBox.Show("请先配置正确的打印模板！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}

            }
            return printConfig;
        }


        /// <summary>
        /// 打印前的检测工作
        /// 为真才打印
        /// </summary>
        /// <param name="EditEntitys"></param>
        /// <param name="rptMode"></param>
        /// <returns></returns>
        private static bool CheckPrinterAndData(List<M> EditEntitys, RptMode rptMode)
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
            rs = true;
            return rs;
        }

        private static bool PrintExecute(List<M> EditEntitys, RptMode rptMode)
        {
            bool rs = false;



            rs = true;
            return rs;
        }



        private static void DESIGN(tb_PrintConfig printConfig, List<object> objlist)
        {
            RptPrintConfig frmPrintConfig = new RptPrintConfig();
            frmPrintConfig.printConfig = printConfig;
            frmPrintConfig.PrintDataSources = objlist;
            frmPrintConfig.ShowDialog();
        }


        /*
        public static void PrintOperation(List<M> printDatas, RptMode rptMode)
        {
            if (printDatas == null || printDatas.Count == 0)
            {
                MessageBox.Show("没有要打印的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            //这里只是取打印配置信息
            CommBillData cbd = bcf.GetBillData<M>(printDatas[0]);
            tb_PrintConfig printConfig = MainForm.Instance.AppContext.Db.Queryable<tb_PrintConfig>()
                .Includes(t => t.tb_PrintTemplates)
                .Where(c => c.BizName == cbd.BizName && c.BizType == (int)cbd.BizType)
                .First();
            if (printConfig == null || printConfig.tb_PrintTemplates == null)
            {


            }
            else
            {
                FastReport.Report FReport;
                FReport = new FastReport.Report();

                List<object> objlist = new List<object>();
                List<M> mlist = GetPrintDatas(printDatas);
                if (mlist == null)
                {
                    MessageBox.Show(cbd.BizName + "没有提供完整的打印数据，请联系管理员");
                    return;
                }
                foreach (var item in mlist)
                {
                    objlist.Add(item);
                }
                FReport.RegisterData(objlist, "rd");
                List<ICurrentUserInfo> currUserInfo = new List<ICurrentUserInfo>();
                currUserInfo.Add(MainForm.Instance.AppContext.CurUserInfo);
                FReport.RegisterData(currUserInfo, "currUserInfo");
                if (printConfig.tb_PrintTemplates.Count == 0)
                {
                    MessageBox.Show("请先配置打印模板！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var printTemplate = printConfig.tb_PrintTemplates[0];
                if (printTemplate.TemplateFileStream != null)
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
                    return;
                }

                switch (rptMode)
                {
                    case RptMode.DESIGN:
                        break;
                    case RptMode.PREVIEW:
                        RptPreviewForm frm = new RptPreviewForm();
                        frm.Text = cbd.BizName + "打印预览";
                        frm.ReprotfileName = printTemplate.Template_Name;
                        frm.MyReport = FReport;
                        frm.ShowDialog();
                        break;
                    case RptMode.PRINT:

                        ////////打印////////
                        //设置默认打印机
                        FReport.PrintPrepared();
                        if (printConfig.PrinterSelected)
                        {
                            FReport.PrintSettings.ShowDialog = false;
                            FReport.PrintSettings.Printer = printConfig.PrinterName;
                        }

                        FReport.Print();
                        ////释放资源
                        FReport.Dispose();
                        打印状态更新
                        if (editEntity.ContainsProperty("PrintStatus"))
                        {
                            int PrintStatus = (int)ReflectionHelper.GetPropertyValue(editEntity, "PrintStatus");
                            PrintStatus++;
                            editEntity.SetPropertyValue("PrintStatus", PrintStatus);
                            //await AppContext.Db.Updateable<T>(EditEntity).UpdateColumns(t => new { t.PrintStatus }).ExecuteCommandAsync();
                            var dt = new Dictionary<string, object>();
                            dt.Add(PKCol, pkid);
                            dt.Add("PrintStatus", PrintStatus);
                            await MainForm.Instance.AppContext.Db.Updateable(dt).AS(typeof(M).Name).WhereColumns(PKCol).ExecuteCommandAsync();
                        }
                        break;
                    default:
                        break;
                }

            }





        }



        public static void PrintDesigned(List<M> printDatas)
        {
            if (printDatas == null || printDatas.Count == 0)
            {
                MessageBox.Show("请选要处理的数据");
                return;
            }

            BillConverterFactory bcf = MainForm.Instance.AppContext.GetRequiredService<BillConverterFactory>();
            CommBillData cbd = bcf.GetBillData<M>(printDatas[0]);
            tb_PrintConfig printConfig = MainForm.Instance.AppContext.Db.Queryable<tb_PrintConfig>()
                .Includes(t => t.tb_PrintTemplates)
                .Where(c => c.BizName == cbd.BizName && c.BizType == (int)cbd.BizType)
                .First();
            if (printConfig == null)
            {
                printConfig = new tb_PrintConfig();
                printConfig.Config_Name = "系统配置" + cbd.BizName;
                printConfig.PrinterSelected = false;
                printConfig.BizName = cbd.BizName;
                printConfig.BizType = (int)cbd.BizType;
                //新建一个配置
                printConfig = MainForm.Instance.AppContext.Db.Insertable<tb_PrintConfig>(printConfig).ExecuteReturnEntity();
            }
            RptPrintConfig frmPrintConfig = new RptPrintConfig();
            frmPrintConfig.printConfig = printConfig;
            List<object> objlist = new List<object>();
            List<M> mlist = GetPrintDatas(printDatas);
            if (mlist == null)
            {
                MessageBox.Show(cbd.BizName + "没有提供完整的打印数据，请联系管理员");
                return;
            }
            foreach (var item in mlist)
            {
                objlist.Add(item);
            }
            frmPrintConfig.PrintDataSources = objlist;
            //frmPrintConfig.PrintDataSources = printDatas.Cast<object>().ToList(); 
            frmPrintConfig.ShowDialog();
            // RptDesignForm frm = new RptDesignForm();
            // frm.reportPrint = rp;
            // frm.ShowDialog();

            //List<Category> FBusinessObject;
            //FastReport.Report FReport;
            //FReport = new FastReport.Report();

            ////  FReport.RegisterData(FBusinessObject, "Categories");
            //FReport.Design();
        }
        */

        public static bool CheckPrinter(string printerName1)
        {
            if (string.IsNullOrEmpty(printerName1))
            {
                return false;
            }
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
    }
}
