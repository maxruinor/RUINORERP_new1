using FastReport.DevComponents.DotNetBar.Controls;
using HLH.WinControl.MyTypeConverter;
using MathNet.Numerics.LinearAlgebra.Factorization;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.UI.UCSourceGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.FormProperty
{
    /// <summary>
    /// 快速录入数据
    /// </summary>
    public partial class frmQuicklyInputGeneric<C> : Form where C : class
    {
        public frmQuicklyInputGeneric()
        {
            InitializeComponent();
            // 注册事件处理程序
        }

        /// <summary>
        /// 将数据行应用到单据中
        /// </summary>
        public event ApplyQuicklyInputData OnApplyQuicklyInputData;

        public delegate void ApplyQuicklyInputData(List<C> lines);

        public List<C> lines = new List<C>();
        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = null;
            txtMaxRows.Value = 0;
            bindingSourceData.Clear();
        }

        private void frmQuicklyInputData_Load(object sender, EventArgs e)
        {
            InitDataGridView();
            //应该只提供一个结构
            int maxRows = txtMaxRows.Value.ToInt();
            SetDataSourceRows(maxRows);
            bindingSourceData.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            // 滚动到第一行显示
            dataGridView1.FirstDisplayedScrollingRowIndex = 0;
            bindingSourceData.ListChanged += bindingSourceData_ListChanged;
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
        }

        public tb_MenuInfo CurMenuInfo { get; set; }
        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public List<string> InvisibleCols { get; set; } = new List<string>();
        private void InitDataGridView()
        {
            bindingSourceData.DataSource = lines;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.XmlFileName = this.Name + typeof(C).Name + "quicklyinput";
            dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(C));
            ControlChildColumnsInvisible(InvisibleCols);
            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
            }
            dataGridView1.DataSource = bindingSourceData;
            ContextMenuStrip newContextMenuStrip = dataGridView1.GetContextMenu(contextMenuStrip1);
            dataGridView1.ContextMenuStrip = newContextMenuStrip;
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bindingSourceData_ListChanged(object sender, ListChangedEventArgs e)
        {
            C entity = default(C); ;
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    entity = bindingSourceData.List[e.NewIndex] as C;

                    break;
                case ListChangedType.ItemDeleted:
                    if (e.NewIndex < bindingSourceData.Count)
                    {
                        entity = bindingSourceData.List[e.NewIndex] as C;
                        bindingSourceData.Remove(entity);
                    }
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    entity = bindingSourceData.List[e.NewIndex] as C;

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

        public void ControlChildColumnsInvisible(List<string> InvisibleCols)
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
                                    if (!InvisibleCols.Contains(item.tb_fieldinfo.FieldName))
                                    {
                                        InvisibleCols.Add(item.tb_fieldinfo.FieldName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            // 获取剪贴板数据
            //if (Clipboard.ContainsText())
            //{
            //    string textData = Clipboard.GetText();
            //    string[] pasteData = textData.Split('\t'); // 假设使用制表符分隔数据
            //}

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (this.OnApplyQuicklyInputData != null)
            {
                OnApplyQuicklyInputData(lines);
            }
        }

        private void txtMaxRows_ValueChanged(object sender, EventArgs e)
        {
            if (sender is Krypton.Toolkit.KryptonNumericUpDown knudMaxRows)
            {
                int maxRows = Convert.ToInt32(knudMaxRows.Value);
                SetDataSourceRows(maxRows);
            }
        }


        
        //设置数据源中的行数
        public void SetDataSourceRows(int maxRows)
        {
            if (maxRows > 0 && maxRows != bindingSourceData.Count)
            {
                if (maxRows > bindingSourceData.Count)
                {
                    for (decimal i = 0; i < maxRows - bindingSourceData.Count; i++)
                    {
                        bindingSourceData.AddNew();
                    }
                }
                else
                {
                    for (int d = bindingSourceData.Count - 1; d > (maxRows - 1); d--)
                    {
                        bindingSourceData.RemoveAt(d);
                    }
                }
            }
        }
    }
}
