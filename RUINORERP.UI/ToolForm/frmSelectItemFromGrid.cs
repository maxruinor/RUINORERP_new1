using RUINORERP.Model;
using RUINORERP.UI.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Lex;

namespace RUINORERP.UI.ToolForm
{
    public partial class frmSelectItemFromGrid : Krypton.Toolkit.KryptonForm
    {
        public frmSelectItemFromGrid()
        {
            InitializeComponent();
        }

        public List<Type> ColDisplayTypes { get; set; } = new List<Type>();
        public List<string> InvisibleCols { get; set; } = new List<string>();
        public List<string> DefaultHideCols { get; set; } = new List<string>();
        public string xmlfilename = string.Empty;
        public Type tagrgetType = null;

        public ConcurrentDictionary<string, KeyValuePair<string, bool>> _FieldNameList = new ConcurrentDictionary<string, KeyValuePair<string, bool>>();

        private void frmSelectItemFromGrid_Load(object sender, EventArgs e)
        {
            newSumDataGridViewPlanChildItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridViewPlanChildItems.XmlFileName = this.Name + tagrgetType.Name + "_PPD";
            newSumDataGridViewPlanChildItems.FieldNameList = _FieldNameList;// UIHelper.GetFieldNameColList(typeof(tb_ProductionPlanDetail));
            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewPlanChildItems.FieldNameList.TryRemove(item, out kv);
            }

            //这里设置指定列默认隐藏。可以手动配置显示
            foreach (var item in DefaultHideCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewPlanChildItems.FieldNameList.TryRemove(item, out kv);
                KeyValuePair<string, bool> Newkv = new KeyValuePair<string, bool>(kv.Key, false);
                newSumDataGridViewPlanChildItems.FieldNameList.TryAdd(item, Newkv);
                //newSumDataGridViewMaster.FieldNameList.TryUpdate(item, Newkv, kv);
            }
            bindingSourcePlanChildItems.DataSource = new List<tb_ProductionPlanDetail>();
            newSumDataGridViewPlanChildItems.DataSource = bindingSourcePlanChildItems;
        }

        private void newSumDataGridViewPlanChildItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridViewPlanChildItems.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //动态字典值显示
            string colName = string.Empty;
            string colDbName = newSumDataGridViewPlanChildItems.Columns[e.ColumnIndex].Name;
            if (ColDisplayTypes != null && ColDisplayTypes.Count > 0)
            {
                colName = UIHelper.ShowGridColumnsNameValue(ColDisplayTypes.ToArray(), colDbName, e.Value);
            }
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }
        }


        public void SetDataSource<T>(IList<T> list)
        {
            //       var SourceBill = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>().Where(c => c.PPID == sourceid)
            //.Includes(a => a.tb_ProductionPlanDetails, b => b.tb_proddetail, c => c.tb_prod)
            //.Includes(a => a.tb_ProductionPlanDetails, b => b.tb_proddetail, c => c.tb_bom_s)
            //.SingleAsync();
            bindingSourcePlanChildItems.DataSource = list;
            newSumDataGridViewPlanChildItems.UseSelectedColumn = false;
            newSumDataGridViewPlanChildItems.Use是否使用内置右键功能 = false;
            newSumDataGridViewPlanChildItems.IsShowSumRow = false;
        }

    }

}

