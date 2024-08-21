using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UControls
{
    public static class ext
    {
        #region 显示统计列
        /// <summary>
        /// 显示DataGridView的统计信息
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="SummaryColumns">要统计的列名称或数据源绑定列名称</param>
        public static void ShowSummary(this DataGridView dgv, string[] SummaryColumns)
        {
            SummaryControlContainer summaryControl = new SummaryControlContainer(dgv, SummaryColumns);
            dgv.Controls.Add(summaryControl);
            //dgv.Tag = summaryControl;
            summaryControl.BringToFront();
            summaryControl.Show();
        }
        /// <summary>
        /// 显示DataGridView的统计信息
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="DisplaySumRowHeader">是否显示合计行标题</param>
        /// <param name="SumRowHeaderText">合计列标题</param>
        /// <param name="SumRowHeaderTextBold">合计列标题用粗体显示</param>
        /// <param name="SummaryColumns">要统计的列名称或数据源绑定列名称</param>
        public static void ShowSummary(this DataGridView dgv, bool DisplaySumRowHeader, string SumRowHeaderText, bool SumRowHeaderTextBold, string[] SummaryColumns)
        {
            SummaryControlContainer summaryControl = new SummaryControlContainer(dgv, DisplaySumRowHeader, SumRowHeaderText, SumRowHeaderTextBold, SummaryColumns);
            dgv.Controls.Add(summaryControl);
            //dgv.Tag = summaryControl;
            summaryControl.BringToFront();
            summaryControl.Show();
        }
        #endregion
    }
}
