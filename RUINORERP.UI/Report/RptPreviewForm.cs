using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport;
using FastReport.Data;

namespace RUINORERP.UI.Report
{
    public partial class RptPreviewForm : KryptonForm
    {
        private string reprotfileName = string.Empty;

        public RptPreviewForm()
        {
            InitializeComponent();
        }
        private FastReport.Report myReport; //新建一个私有变量

        public FastReport.Report MyReport { get => myReport; set => myReport = value; }

        private void RptPreviewForm_Load(object sender, EventArgs e)
        {
            //pReport = new FastReport.Report();   //实例化一个Report报表
           // pReport.RegisterData(MainList, "Main");
           // pReport.RegisterData(SubList, "Sub");

            //给DataBand(主表数据)绑定数据源 
           // DataBand masterBand = pReport.FindObject("Main") as DataBand;
          //  masterBand.DataSource = MainList; //主表 

            //给DataBand(明细数据)绑定数据源 
           // DataBand detailBand = pReport.FindObject("Sub") as DataBand;
            //detailBand.DataSource = SubList; //明细表 

            //重要！！给明细表设置主外键关系！
            //detailBand.Relation = new Relation();
            //detailBand.Relation.ParentColumns = new string[] { "SONO" };
            //detailBand.Relation.ParentDataSource = pReport.GetDataSource("tb_SO"); //主表
            //detailBand.Relation.ChildColumns = new string[] { "SONO" };
            //detailBand.Relation.ChildDataSource = pReport.GetDataSource("tb_SOs"); //明细表

          //  String reportFile =string.Format("ReportTemplate/{0}", ReprotfileName);
          //  if (System.IO.File.Exists(Application.StartupPath + "\\" + reportFile))
          //  {
          //      MyReport.Load(reportFile);  //载入报表文件    
          //  }
          ////  MyReport.Load(reportFile);  //载入报表文件
            MyReport.Preview = previewControl1; //设置报表的Preview控件（这里的previewControl1就是我们之前拖进去的那个）
            MyReport.ShowPrepared();  //显示
        }
    }
}
