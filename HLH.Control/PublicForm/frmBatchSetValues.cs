using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static HLH.WinControl.MyDataGrid.NewSumDataGridView;

namespace HLH.WinControl.PublicForm
{
    public partial class frmBatchSetValues : Form
    {
        private List<KeyValuePair<string, string>> columnsName = new List<KeyValuePair<string, string>>();


        public enum BatchSetValueType
        {
            文本,
            数值,
            下拉选项,
            复选框

        }


        private BatchSetValueType defaultValueType;


        public frmBatchSetValues()
        {
            InitializeComponent();
        }
        public frmBatchSetValues(DataCallBackEventHandler dataCallBackEvent)
        {
            InitializeComponent();
            _dataCallBackEvent = dataCallBackEvent;
        }

        private object resultValue;

        /// <summary>
        /// 返回值
        /// </summary>
        public object ResultValue
        {
            get
            {
                return resultValue;
            }

            set
            {
                resultValue = value;
            }
        }

        private string selectColumnName = string.Empty;
        private bool modifyAllInTheCol = false;


        public List<KeyValuePair<string, string>> ColumnsName { get => columnsName; set => columnsName = value; }

        /// <summary>
        /// 要修改的列 选中的列名
        /// </summary>
        public string SelectColumnName { get => selectColumnName; set => selectColumnName = value; }

        public BatchSetValueType DefaultValueType { get => defaultValueType; set => defaultValueType = value; }
        public bool ModifyAllInTheCol { get => modifyAllInTheCol; set => modifyAllInTheCol = value; }



        DataCallBackEventHandler _dataCallBackEvent;

        private void frmBatcheSetValues_Load(object sender, EventArgs e)
        {
            HLH.Lib.Helper.DropDownListHelper.InitDropList(columnsName, cmbCurrentCols, true);
            cmbCurrentCols.SelectedIndex = cmbCurrentCols.FindString(selectColumnName);


            HLH.Lib.Helper.DropDownListHelper.InitDropListForWin(cmbValueType, typeof(BatchSetValueType));

            //设置默认的控件
            cmbValueType.SelectedIndex = cmbValueType.FindString(defaultValueType.ToString());

        }

        private void cmbValueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValue.Visible = false;
            cmbSetValue.Visible = false;
            checkBox1.Visible = false;
            BatchSetValueType vt = (BatchSetValueType)Enum.Parse(typeof(BatchSetValueType), cmbValueType.SelectedItem.ToString());
            switch (vt)
            {
                case BatchSetValueType.文本:
                    txtValue.Visible = true;
                    break;
                case BatchSetValueType.数值:
                    txtValue.Visible = true;
                    break;
                case BatchSetValueType.下拉选项:
                    cmbSetValue.Visible = true;
                    break;
                case BatchSetValueType.复选框:
                    checkBox1.Visible = true;
                    break;
                default:
                    break;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            txtValue.Visible = false;
            cmbSetValue.Visible = false;

            BatchSetValueType vt = (BatchSetValueType)Enum.Parse(typeof(BatchSetValueType), cmbValueType.SelectedItem.ToString());
            switch (vt)
            {
                case BatchSetValueType.文本:
                    txtValue.Visible = true;
                    ResultValue = txtValue.Text;
                    break;
                case BatchSetValueType.数值:
                    txtValue.Visible = true;
                    ResultValue = txtValue.Text;
                    break;
                case BatchSetValueType.下拉选项:
                    cmbSetValue.Visible = true;
                    ResultValue = cmbSetValue.SelectedItem;
                    break;
                case BatchSetValueType.复选框:
                    cmbSetValue.Visible = false;
                    txtValue.Visible = false;
                    ResultValue = checkBox1.Checked;

                    break;
                default:
                    break;
            }

            ///是否要修改整个列的值
            ModifyAllInTheCol = chkAll.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmbCurrentCols_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectColumnName = cmbCurrentCols.SelectedItem.ToString();
            if (_dataCallBackEvent != null)
            {
                //设置默认的控件
                cmbValueType.SelectedIndex = cmbValueType.FindString(_dataCallBackEvent(SelectColumnName, chkAll.Checked));      //通过委托变量调用函数

            }



        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            SelectColumnName = cmbCurrentCols.SelectedItem.ToString();
            if (_dataCallBackEvent != null)
            {
                //设置默认的控件
                cmbValueType.SelectedIndex = cmbValueType.FindString(_dataCallBackEvent(SelectColumnName, chkAll.Checked));      //通过委托变量调用函数

            }
        }
    }
}
