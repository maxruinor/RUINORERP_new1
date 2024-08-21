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

namespace RUINORERP.UI.CommonUI
{
    public partial class frmMultiSelector : Krypton.Toolkit.KryptonForm
    {
        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        private ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }


        /// <summary>
        /// 保存要总计的列
        /// </summary>
        public List<string> SummaryCols { get; set; } = new List<string>();


        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public List<string> InvisibleCols { get; set; } = new List<string>();



        /// <summary>
        /// 保存默认隐藏的列
        /// </summary>
        public List<string> DefaultHideCols { get; set; } = new List<string>();


        /// <summary>
        /// 双击哪一列会跳到单据编辑菜单
        /// </summary>
        public string RelatedBillEditCol { get; set; }

        /// <summary>
        /// 是否多选
        /// </summary>
        public bool MulitSelectRow { get; set; } = false;

        /// <summary>
        /// 指定要显示的实体集合类型
        /// </summary>
        public Type entityType;
        public frmMultiSelector()
        {
            InitializeComponent();
            newSumDataGridViewSelectorLines.CellFormatting += DataGridView1_CellFormatting;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //if (_entity.CloseCaseOpinions.IsNullOrEmpty())
            //{
            //    MessageBox.Show("请填写手动结案原因");
            //    return;
            //}
            //List<tb_ProductionDemandDetail> list = new List<tb_ProductionDemandDetail>();
            //list.AddRange(SourceBill.tb_ProductionDemandDetails.Where(c => c.ParentId == 0).ToList());
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void Loadlines()
        {
            newSumDataGridViewSelectorLines.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridViewSelectorLines.XmlFileName = this.Name + entityType.Name + "MultiSelector";
            newSumDataGridViewSelectorLines.FieldNameList = UIHelper.GetFieldNameColList(entityType);
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewSelectorLines.FieldNameList.TryRemove(item, out kv);
            }

            //这里设置指定列默认隐藏。可以手动配置显示
            foreach (var item in DefaultHideCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewSelectorLines.FieldNameList.TryRemove(item, out kv);
                KeyValuePair<string, bool> Newkv = new KeyValuePair<string, bool>(kv.Key, false);
                newSumDataGridViewSelectorLines.FieldNameList.TryAdd(item, Newkv);
                //newSumDataGridViewMaster.FieldNameList.TryUpdate(item, Newkv, kv);
            }
            newSumDataGridViewSelectorLines.UseSelectedColumn = true;
            newSumDataGridViewSelectorLines.MultiSelect = MulitSelectRow;
            newSumDataGridViewSelectorLines.DataSource = bindingSourceChild;

        }
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridViewSelectorLines.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }


            //固定字典值显示
            string colDbName = newSumDataGridViewSelectorLines.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }

                }
            }



            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue(entityType, colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName) && colName != "System.Object")
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (newSumDataGridViewSelectorLines.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }

        private void frmMultiSelector_Load(object sender, EventArgs e)
        {

        }

        /*
            _UCBillChildQuery_Related.Name = "_UCBillChildQuery_Related";
            _UCBillChildQuery_Related.entityType = ChildRelatedEntityType;
            List<string> childlist = ExpressionHelper.ExpressionListToStringList(ChildRelatedSummaryCols);
            _UCBillChildQuery_Related.InvisibleCols = ExpressionHelper.ExpressionListToStringList(ChildRelatedInvisibleCols);
            _UCBillChildQuery_Related.DefaultHideCols = new List<string>();
            ControlColumnsInvisible(_UCBillChildQuery_Related.InvisibleCols, _UCBillChildQuery_Related.DefaultHideCols);
            _UCBillChildQuery_Related.SummaryCols = childlist;
            _UCBillChildQuery_Related.ColNameDataDictionary = ChildColNameDataDictionary;
         */


    }
}
