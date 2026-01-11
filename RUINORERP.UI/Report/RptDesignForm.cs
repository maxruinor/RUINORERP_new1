using Krypton.Toolkit;
using FastReport.Design;
using FastReport.Design.StandardDesigner;
using FastReport.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport.Data;
using FastReport;
using RUINORERP.Model;
using FastReport.Web;
using RUINORERP.Business;

namespace RUINORERP.UI.Report
{
    /// <summary>
    /// 报表设计窗体
    /// </summary>
    public partial class RptDesignForm : KryptonForm
    {
        //https://blog.csdn.net/c_10086_/article/details/126271447  添加图片
        //https://www.cnblogs.com/sky-gfan/p/12034144.html 导出数据


        /// <summary>
        /// 要打印的数据
        /// </summary>
        public List<object> PrintDataSources { get; set; }


        public List<tb_Company> companyInfos { get; set; }

        public List<CurrentUserInfo> currUserInfos { get; set; }


        string reportTemplateFile = string.Empty;

        public string ReportTemplateFile { get => reportTemplateFile; set => reportTemplateFile = value; }
        public string RptName { get; private set; }

        private bool saveAs = false;

        /// <summary>
        /// 是否为另存为的点击
        /// </summary>
        public bool SaveAs { get => saveAs; set => saveAs = value; }

        public RptDesignForm()
        {
            InitializeComponent();
        }


        private void RptDesignForm_Load(object sender, EventArgs e)
        {
            InitializeReport(RptMode.DESIGN);
            //MyLoad();
        }


        private void MyLoad()
        {
            FastReport.Utils.Config.DesignerSettings.ShowInTaskbar = true;
            FastReport.Utils.Config.SplashScreenEnabled = true;
            // create a new empty report and attach it to the designer
            FastReport.Report report = new FastReport.Report();
            string fullpath = System.IO.Path.Combine(Application.StartupPath + "\\", string.Format("ReportTemplate\\{0}", ReportTemplateFile));
            if (!string.IsNullOrEmpty(ReportTemplateFile))
            {
                if (System.IO.File.Exists(fullpath))
                {
                    report.Load(fullpath);
                }
            }
            designerControl1.Report = report;
            designerControl1.ShowMainMenu = true;
            // restore the design layout. Without this code, the designer tool windows will be unavailable
            designerControl1.UIStyle = FastReport.Utils.UIStyle.Office2010Blue;
            designerControl1.RefreshLayout();
            this.Text = "设计器-" + string.Format("ReportTemplate\\{0}", ReportTemplateFile);
            report.Design();

            // Config.DesignerSettings.DesignerLoaded += DesignerSettings_DesignerLoaded;
            //dReport = new FastReport.Report();
            //string reportFile = "ReportTemplate/test.frx";
            //dReport.Load(reportFile);
            //this.designerControl1.Report = dReport;
            //dReport.Prepare();
            //dReport.Design();
        }

        private void designerControl1_UIStateChanged(object sender, EventArgs e)
        {
            // update Enabled state of our buttons
            ////btnSave.Enabled = designerControl1.cmdSave.Enabled;
            //btnUndo.Enabled = designerControl1.cmdUndo.Enabled;
            //btnRedo.Enabled = designerControl1.cmdRedo.Enabled;
        }


        //菜单事件注册
        private void RegisterDesignerEvents()
        {

            //SaveReport,表示保存报表时发生，意思就是点击保存按钮发生的事件
            environmentSettings1.DesignerSettings.CustomSaveReport += DesignerSettings_CustomSaveReport;


            environmentSettings1.DesignerSettings.CustomOpenDialog += DesignerSettings_CustomOpenDialog;

            designerControl1.cmdSaveAs.CustomAction += CmdSaveAs_CustomAction;

            environmentSettings1.DesignerSettings.CustomOpenReport += DesignerSettings_CustomOpenReport;

            //CutomSaveDialog,表示保存的对话框，这个只有另存为才有
            environmentSettings1.DesignerSettings.CustomSaveDialog += DesignerSettings_CustomSaveDialog;
        }


        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DesignerSettings_CustomSaveDialog(object sender, OpenSaveDialogEventArgs e)
        {

            tb_PrintTemplate printTemplate = (bindingSourceTemplateDesign.Current as tb_PrintTemplate);

            //当printTemplate的流不为空时，可以另存为。
            //如果报表内容有变化，则不是加存为，只是保存而已，所以要保存一个副本流的长度？

            if (printTemplate.ActionStatus == ActionStatus.加载 && printTemplate.TemplateFileStream != null)
            {
                ////有内容，并且内容不变时可以另存为
                //if (flag == printTemplate.TemplateFileStream.Length)
                //{
                //    SaveAs = true;
                //}

                if (SaveAs)
                {
                    using (SaveFileDialog dialog = new SaveFileDialog())
                    {
                        dialog.Filter = "Report files (*.frx)|*.frx";
                        e.FileName = printTemplate.Template_Name + ".frx";
                        //从e.FileName中获取默认文件名。
                        dialog.FileName = e.FileName;
                        // 如果 dialog.FileName = e.FileName； 
                        // 如果 dialog.FileName = e.Cancel，则设置 e.Cancel 为 false
                        e.Cancel = dialog.ShowDialog() != DialogResult.OK;
                        //将e.FileName设置为选定的文件名。
                        e.FileName = dialog.FileName;
                    }
                }

            }
            /* 下面是之前代码
            
            tb_PrintTemplate printTemplate = (bindingSourceTemplateDesign.Current as tb_PrintTemplate);
            if (printTemplate.actionStatus == ActionStatus.新增)
            {
                RptNewTemplate rptNew = new RptNewTemplate();
                if (rptNew.ShowDialog() == DialogResult.OK)
                {
                    printTemplate.Template_Name = rptNew.txtTemplateName.Text.Trim();
                    printTemplate.ID = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                    e.Cancel = false;
                }
                else
                {
                    isSaveAs = false;
                    e.Cancel = true;
                }

            }
            else
            {
                if (printTemplate.actionStatus == ActionStatus.加载)
                {

                }
                else
                {
                    #region 另存为

                    RptNewTemplate rptNew = new RptNewTemplate();
                    if (rptNew.ShowDialog() == DialogResult.OK)
                    {
                        isSaveAs = true;
                        tb_PrintTemplate saveAsCopy = RUINORERP.Common.Helper.CloneHelper.DeepCloneObject<tb_PrintTemplate>((bindingSourceTemplateDesign.Current as tb_PrintTemplate));
                        saveAsCopy.Template_Name = rptNew.txtTemplateName.Text.Trim();
                        saveAsCopy.ID = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                        bindingSourceTemplateDesign.Add(saveAsCopy);
                        bindingSourceTemplateDesign.Position++;
                        e.Cancel = false;
                    }
                    else
                    {
                        isSaveAs = false;
                        e.Cancel = true;
                    }

                    #endregion
                }

            }

            */
        }

        private void CmdSaveAs_CustomAction(object sender, EventArgs e)
        {

            SaveReport((sender as DesignerControl).Report);
            SaveAs = true;
            if (SaveAs)
            {
                tb_PrintTemplate printTemplate = (bindingSourceTemplateDesign.Current as tb_PrintTemplate);
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "Report files (*.frx)|*.frx";
                    (sender as DesignerControl).Report.FileName = printTemplate.Template_Name + ".frx";
                    //从e.FileName中获取默认文件名。
                    dialog.FileName = (sender as DesignerControl).Report.FileName;
                    // 如果 dialog.FileName = e.FileName； 
                    // 如果 dialog.FileName = e.Cancel，则设置 e.Cancel 为 false
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        (sender as DesignerControl).Report.Save(dialog.FileName);
                        SaveAs = false;
                    }
                    //将e.FileName设置为选定的文件名。
                    //(sender as DesignerControl).Report.FileName = dialog.FileName;
                    SaveAs = false;
                }
            }
        }


        #region 人民币转换
        decimal amount = 0;
        private static string StrTran(string s, string oldv, string newv)
        {
            return s.Replace(oldv, newv);
        }
        /// <summary>/// 转换大写人民币/// </summary>/// <param name="r"></param>/// <returns></returns>
        public static string RMBToString(decimal r)
        {
            decimal r1;
            string s1 = "零壹贰叁肆伍陆柒捌玖";
            string s2 = "分角元拾佰仟万拾佰仟亿拾佰仟万";
            string dx, s; r1 = r; dx = ""; if (r1 < 0) { r1 *= -1; dx = "负"; }
            s = String.Format("{0:f0}", r1 * 100);
            int len = s.Length; for (int i = 0; i < len; i++) { dx = dx + s1.Substring(s[i] - '0', 1) + s2.Substring(len - i - 1, 1); }
            dx = StrTran(StrTran(StrTran(StrTran(StrTran(dx, "零仟", "零"), "零佰", "零"), "零拾", "零"), "零角", "零"), "零分", "整");
            dx = StrTran(StrTran(StrTran(StrTran(StrTran(dx, "零零", "零"), "零零", "零"), "零亿", "亿"), "零万", "万"), "零元", "元");
            if (dx == "整")
                return "零元整";
            else
                return StrTran(StrTran(StrTran(dx, "亿万", "亿零"), "零整", "整"), "零零", "零");
        }
        #endregion   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DesignerSettings_CustomOpenReport(object sender, OpenSaveReportEventArgs e)
        {
            if (e == null) return;
            if (System.IO.File.Exists(e.FileName))
            {
                //从给定的e.FileName中加载报告。
                e.Report.Load(e.FileName);
            }
        }

        private void DesignerSettings_CustomOpenDialog(object sender, OpenSaveDialogEventArgs e)
        {
            var printTemplate = bindingSourceTemplateDesign.Current as tb_PrintTemplate;
            //if (printTemplate == null || printTemplate.actionStatus != ActionStatus.加载)
            //{
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Report files (*.frx)|*.frx";
                //设置默认文件类型显示顺序  
                dialog.FilterIndex = 1;
                //是否自动在文件名中添加扩展名
                dialog.AddExtension = true;
                //是否记忆上次打开的目录
                dialog.RestoreDirectory = true;

                // 如果对话框中的 "报告文件"，则将e.Cancel设置为false

                // 已成功执行
                e.Cancel = dialog.ShowDialog() != DialogResult.OK;
                //将e.FileName设置为选定的文件名。
                e.FileName = dialog.FileName;
                //}
            }



            /* 这里注释了原来的代码
            if (MainForm.Instance.AppContext.IsSuperUser)
            {
                using (System.Windows.Forms.OpenFileDialog form = new OpenFileDialog())
                {
                    // show dialog
                    e.Cancel = form.ShowDialog() != DialogResult.OK;
                    // return the selected report in the e.FileName
                    e.FileName = form.FileName;


                    FastReport.Utils.Config.DesignerSettings.ShowInTaskbar = true;
                    FastReport.Utils.Config.SplashScreenEnabled = true;
                    // create a new empty report and attach it to the designer
                    FastReport.Report report = new FastReport.Report();
                    if (!string.IsNullOrEmpty(e.FileName))
                    {
                        if (System.IO.File.Exists(e.FileName))
                        {
                            report.Load(e.FileName);
                        }
                    }
                    designerControl1.Report = report;
                    designerControl1.ShowMainMenu = true;
                    // restore the design layout. Without this code, the designer tool windows will be unavailable
                    designerControl1.UIStyle = FastReport.Utils.UIStyle.Office2010Blue;
                    designerControl1.RefreshLayout();
                    //this.Text = "设计器-" + string.Format("ReportTemplate\\{0}", ReportTemplateFile);
                    report.Design();



                }
            }
            */

        }

        private void DesignerSettings_CustomSaveReport(object sender, OpenSaveReportEventArgs e)
        {
            // 将报告保存到给定的e.FileName中。
            if (SaveAs)
            {
                e.Report.Save(e.FileName);
                SaveAs = false;
            }
            else
            {
                //保存直接到这，
                //保存为 则先上面保存对话框，再到这里
                SaveReport(e.Report);
            }

        }

        private void SaveReport(FastReport.Report TargetReport)
        {
            FastReport.Report report = new FastReport.Report();
            try
            {
                using (MemoryStream msStream = new MemoryStream())
                {
                    //保存
                    TargetReport.Save(msStream);
                    var pt = (bindingSourceTemplateDesign.Current as tb_PrintTemplate);
                    pt.TemplateFileStream = msStream.ToArray();
                    if (pt.ActionStatus == ActionStatus.新增)
                    {
                        long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                        pt.ID = sid;
                        MainForm.Instance.AppContext.Db.Insertable<tb_PrintTemplate>(pt).ExecuteCommand();
                    }
                    else
                    {
                        MainForm.Instance.AppContext.Db.Updateable<tb_PrintTemplate>(pt).ExecuteCommand();
                    }
                    flag = pt.TemplateFileStream.Length;
                    pt.ActionStatus = ActionStatus.加载;
                    BusinessHelper.Instance.EditEntity(pt);
                    return;
                    if (1 == 2)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        //设置文件类型
                        saveFileDialog.Filter = "报表文件(*.frx)|*.frx";
                        //设置默认文件类型显示顺序  
                        saveFileDialog.FilterIndex = 1;
                        //是否自动在文件名中添加扩展名
                        saveFileDialog.AddExtension = true;
                        //是否记忆上次打开的目录
                        saveFileDialog.RestoreDirectory = true;
                        //设置默认文件名
                        saveFileDialog.FileName = RptName;
                        //按下确定选择的按钮  
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            //获得文件路径 
                            string localFilePath = saveFileDialog.FileName.ToString();
                            //文件保存
                            FileStream fsStream = new FileStream(localFilePath, FileMode.Create);
                            msStream.WriteTo(fsStream);
                            //资源释放      
                            fsStream.Close();
                            fsStream = null;
                        }
                        //赋初始值
                        // isSaveAs = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        //报表内容的标记，以长度保存
        int flag = 0;

        //设计报表
        private void DesignReport(RptMode rptMode)
        {
            FastReport.Report TargetReport = new FastReport.Report();
            TargetReport.FileName = RptName;
            var printTemplate = bindingSourceTemplateDesign.Current as tb_PrintTemplate;
            if (printTemplate.TemplateFileStream != null)
            {
                byte[] ReportBytes = (byte[])printTemplate.TemplateFileStream;
                using (MemoryStream Stream = new MemoryStream(ReportBytes))
                {
                    TargetReport.Load(Stream);
                    printTemplate.ActionStatus = ActionStatus.加载;
                }
                flag = ReportBytes.Length;
            }
            else
            {
                //string ReprotfileName = "tb_PurEntry" + ".frx";
                //String reportFile = string.Format("ReportTemplate/{0}", ReprotfileName);
                //if (System.IO.File.Exists(Application.StartupPath + "\\" + reportFile))
                //{
                //    TargetReport.Load(reportFile);  //载入报表文件    
                //}
                //新建的时候内容就会为空，这时可以打开文件，这个文件可以是其他模板
                printTemplate.ActionStatus = ActionStatus.新增;
            }
            TargetReport.RegisterData(PrintDataSources, "rd");
            //报表控件注册数据
            TargetReport.RegisterData(currUserInfos, "currUserInfo");
            TargetReport.RegisterData(companyInfos, "companyInfo");
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    // 在这里更新 UI
                    designerControl1.Report = TargetReport;
                    designerControl1.ShowMainMenu = true;
                    // restore the design layout. Without this code, the designer tool windows will be unavailable
                    designerControl1.UIStyle = FastReport.Utils.UIStyle.Office2010Blue;
                    designerControl1.RefreshLayout();
                    this.Text = "ERP系统-打印设计器----------" + printTemplate.Template_Name;// + string.Format("ReportTemplate\\{0}", ReportTemplateFile);


                }));
            }
            else
            {
                // 直接更新 UI
                designerControl1.Report = TargetReport;
                designerControl1.ShowMainMenu = true;
                // restore the design layout. Without this code, the designer tool windows will be unavailable
                designerControl1.UIStyle = FastReport.Utils.UIStyle.Office2010Blue;
                designerControl1.RefreshLayout();
                //this.Text = "ERP系统-打印设计器----------" + printTemplate.Template_Name;// + string.Format("ReportTemplate\\{0}", ReportTemplateFile);

            }



            //dReport.Design();

            //操作方式：DESIGN-设计;PREVIEW-预览;PRINT-打印
            if (rptMode == RptMode.DESIGN)
            {
                environmentSettings1.DesignerSettings.Restrictions.DontEditCode = false;
                TargetReport.Design();

            }
            else if (rptMode == RptMode.PREVIEW)
            {
                TargetReport.Prepare();
                TargetReport.ShowPrepared();
            }
            else if (rptMode == RptMode.PRINT)
            {
                TargetReport.Print();
            }

        }

        private void EnvironmentSettings1_DesignerLoaded(object sender, EventArgs e)
        {

        }




        //初始化报表
        private void InitializeReport(RptMode rptMode)
        {
            RegisterDesignerEvents();
            bindingSourceTemplateDesign.ListChanged += BindingSourceTemplateDesign_ListChanged;
            DesignReport(rptMode);
        }

        private void BindingSourceTemplateDesign_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    //如果这里为空出错， 需要先查询一个空的。绑定一下数据源的类型。之前是默认查询了所有

                    break;
                case ListChangedType.ItemDeleted:

                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:

                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    break;
                default:
                    break;
            }
        }

        private void environmentSettings1_DesignerLoaded_1(object sender, EventArgs e)
        {

        }

        private void environmentSettings1_ReportLoaded(object sender, ReportLoadedEventArgs e)
        {

        }
    }

    public enum RptMode
    {
        DESIGN,
        PREVIEW,
        PRINT,
        ToPDF,
    }
}
