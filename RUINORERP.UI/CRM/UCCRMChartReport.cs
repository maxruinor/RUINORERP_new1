using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;
using RUINORERP.UI.Common;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Business.Processor;
using RUINORERP.UI.ChartFramework.Models;

namespace RUINORERP.UI.CRM
{
    [MenuAttrAssemblyInfo("绩效分析", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.绩效分析)]
    public partial class UCCRMChartReport : BaseChartReport
    {
        public UCCRMChartReport()
        {
            InitializeComponent();
        }

        private void UCCRMChartReport_Load(object sender, EventArgs e)
        {

        }
        public override void QueryConditionBuilder()
        {
           // QueryConditionFilter.SetQueryField<DataRequest>(c => c.StartTime, Global.QueryFieldType.DateTime,
           // QueryConditionFilter.SetQueryField<DataRequest>(c => c.EndTime);
           // QueryConditionFilter.FilterLimitExpressions.Clear();
        }

    }
}
