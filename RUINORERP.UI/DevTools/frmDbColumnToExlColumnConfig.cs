using HLH.Lib.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.DevTools
{
    public partial class frmDbColumnToExlColumnConfig : Form
    {
        public frmDbColumnToExlColumnConfig()
        {
            InitializeComponent();
        }

        private string importTargetTableName;
        public string ImportTargetTableName { get => importTargetTableName; set => importTargetTableName = value; }



        public DataGridView dvExcel = new DataGridView();

        //  List<SuperKeyValue> dc = new List<SuperKeyValue>();

        //匹配的结果
        List<SuperKeyValuePair> kvList = new List<SuperKeyValuePair>();


        private void frmLogisticDbColumnToExlColumnConfig_Load(object sender, EventArgs e)
        {

            //为了将主键突出显示，设置绘制模式
            listbox匹配结果.DrawMode = DrawMode.OwnerDrawFixed;

            if (dvExcel.DataSource == null)
            {
                MessageBox.Show("请设置来源数据表格的数据源。");
                return;
            }

            #region 加载字段

            listBoxExcelColumns.Items.Clear();
            foreach (DataGridViewColumn item in dvExcel.Columns)
            {
                SuperValue sv = new SuperValue(item.Name, "");
                listBoxExcelColumns.Items.Add(sv);
            }

            listBoxDbColumns.Items.Clear();


            DataTable dt = MainForm.Instance.AppContext.Db.Ado.GetDataTable(" select COLUMN_NAME, DATA_TYPE from information_schema.columns where table_name = @table_name",
new { table_name = ImportTargetTableName });

            foreach (DataRow dr in dt.Rows)
            {
                SuperValue sv = new SuperValue(dr["COLUMN_NAME"].ToString(), dr["DATA_TYPE"].ToString());
                listBoxDbColumns.Items.Add(sv);
            }

            #endregion

            LoadfileTOlistBox();
            if (txtMatchConfigFileName.Text.Trim().Length == 0)
            {
                txtMatchConfigFileName.Text=ImportTargetTableName+".xml";
            }
        }

        private void listbox匹配结果_DrawItem(object sender, DrawItemEventArgs e)
        {

            if (e.Index >= 0)
            {
                e.DrawBackground();
                Brush myBrush = Brushes.Black; //前景色
                Color bgColor = Color.White;   //背景色

                string kvStr = listbox匹配结果.Items[e.Index].ToString().Replace("[", "").Replace("]", "");
                SuperKeyValuePair kv = kvList.Find(delegate (SuperKeyValuePair k) { return k.Key.superStrValue == kvStr.Split(',')[0]; });
                if (kv.Tag != null)
                {
                    if (kv.Tag.ToString() == "True")
                    {
                        bgColor = Color.RoyalBlue;
                    }
                }

                //if (listbox匹配结果.Items[e.Index].ToString().Contains("成功"))
                //{
                //    bgColor = Color.RoyalBlue;
                //}
                //if (listbox匹配结果.Items[e.Index].ToString().Contains("失败"))
                //{
                //    bgColor = Color.Magenta;
                //}
                //绘制背景
                e.Graphics.FillRectangle(new SolidBrush(bgColor), e.Bounds);
                //绘制文字
                e.Graphics.DrawString(listbox匹配结果.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                //绘制聚焦框
                e.DrawFocusRectangle();
            }

        }
        public string[] LoadConfigFile()
        {
            List<string> files = new List<string>();
            string[] fi = System.IO.Directory.GetFiles(UserGlobalConfig.Instance.MatchColumnsConfigDir);
            return fi;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadfileTOlistBox()
        {
            listBoxExitFile.Items.Clear();
            string[] files = LoadConfigFile();
            foreach (string f in files)
            {
                listBoxExitFile.Items.Add(f);
                //System.IO.FileInfo fi = new System.IO.FileInfo(f);
                if (f == txtMatchConfigFileName.Text)
                {
                    listBoxExitFile.SelectedItem = f;
                    Deserialize();
                }
            }
            if (listBoxExitFile.SelectedItem == null || listBoxExitFile.SelectedIndex == -1)
            {
                MessageBox.Show("没有找到对应的匹配文件，请先配置。");

            }
        }



        private void listBoxExcelColumns_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxDbColumns.SelectedItem != null && listBoxDbColumns.SelectedItem != null)
            {
                if (kvList == null)
                {
                    kvList = new List<SuperKeyValuePair>();
                }
                SuperValue Excelsv = listBoxExcelColumns.SelectedItem as SuperValue;
                SuperValue Dbsv = listBoxDbColumns.SelectedItem as SuperValue;
                kvList.Add(new SuperKeyValuePair(new SuperValue(Dbsv.superStrValue, Dbsv.superDataTypeName), new SuperValue(Excelsv.superStrValue, "")));

                listBoxDbColumns.Items.Remove(listBoxDbColumns.SelectedItem);
                listBoxExcelColumns.Items.Remove(listBoxExcelColumns.SelectedItem);
                LoadListTolistbox匹配结果(kvList);

            }
            else
            {
                MessageBox.Show("请选择配对的字段：【DB列】选择一项，【excel列】选择一项后双击。");
            }
        }



        private void LoadListTolistbox匹配结果(List<SuperKeyValuePair> dc)
        {
            listbox匹配结果.Items.Clear();
            foreach (SuperKeyValuePair item in dc)
            {

                listbox匹配结果.Items.Add(string.Format("[{0},{1}]", item.Key.superStrValue, item.Value.superStrValue));


                //清除db 和 excel 列
                //移除
                //移出的 需要再加到 源和目标
                listBoxDbColumns.Items.Remove(item.Key);
                listBoxExcelColumns.Items.Remove(item.Value);
            }



        }



        private void btnSaveMatchResult_Click(object sender, EventArgs e)
        {

            string xml = HLH.Lib.Xml.XmlUtil.Serializer(typeof(List<SuperKeyValuePair>), kvList);
            string filepath = UserGlobalConfig.Instance.MatchColumnsConfigDir + "/" + txtMatchConfigFileName.Text;
            if (!filepath.ToLower().EndsWith(".xml"))
            {
                filepath = filepath + ".xml";
            }
            HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.SaveFile(filepath, xml, System.Text.Encoding.UTF8);
            LoadfileTOlistBox();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }




        /// <summary>
        /// 反序列化加载
        /// </summary>
        public void Deserialize()
        {
            if (listBoxExitFile.SelectedItem != null)
            {
                string filepath = listBoxExitFile.SelectedItem.ToString();

                if (System.IO.File.Exists(filepath))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(filepath);
                    txtMatchConfigFileName.Text = fi.Name;
                    string xml = HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.ReadFile(filepath, System.Text.Encoding.UTF8);

                    List<SuperKeyValuePair> dclist = new List<SuperKeyValuePair>();
                    //载入配置
                    dclist = HLH.Lib.Xml.XmlUtil.Deserialize(typeof(List<SuperKeyValuePair>), xml) as List<SuperKeyValuePair>;
                    kvList.Clear();
                    kvList = dclist;
                    // listbox匹配结果.Items.Clear();
                    LoadListTolistbox匹配结果(dclist);

                }
            }



        }

        private void listBoxExitFile_DoubleClick(object sender, EventArgs e)
        {
            Deserialize();
        }

        private void listbox匹配结果_DoubleClick(object sender, EventArgs e)
        {
            if (listbox匹配结果.SelectedItem != null)
            {
                //移除
                string kvStr = listbox匹配结果.SelectedItem.ToString().Replace("[", "").Replace("]", "");

                SuperKeyValuePair kv = kvList.Find(delegate (SuperKeyValuePair k) { return k.Key.superStrValue == kvStr.Split(',')[0]; });
                kvList.Remove(kv);



                listbox匹配结果.Items.Remove(listbox匹配结果.SelectedItem);

                //移出的 需要再加到 源和目标
                listBoxDbColumns.Items.Add(kv.Key);
                listBoxExcelColumns.Items.Add(kv.Value);

            }
        }

        private void btmCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void listBoxExcelColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBoxDbColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listbox匹配结果_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 指定为这次操作的主键标识ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (SuperKeyValuePair item in kvList)
            {
                item.Tag = null;
                if (listbox匹配结果.SelectedItem != null)
                {
                    string kvStr = listbox匹配结果.SelectedItem.ToString().Replace("[", "").Replace("]", "");
                    SuperKeyValuePair kv = kvList.Find(delegate (SuperKeyValuePair k) { return k.Key.superStrValue == kvStr.Split(',')[0]; });
                    kv.Tag = true;
                    break;
                }
            }

        }

        private void 清空匹配结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "将匹配结果全部清除，\r\n 你确定要执行吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                foreach (var item in listbox匹配结果.Items)
                {
                    //移除
                    string kvStr = item.ToString().Replace("[", "").Replace("]", "");
                    SuperKeyValuePair kv = kvList.Find(delegate (SuperKeyValuePair k) { return k.Key.superStrValue == kvStr.Split(',')[0]; });
                    kvList.Remove(kv);
                    listbox匹配结果.Items.Remove(item);
                    //移出的 需要再加到 源和目标
                    if (!listBoxDbColumns.Items.Contains(kv.Key))
                    {
                        listBoxDbColumns.Items.Add(kv.Key);
                    }
                    if (!listBoxExcelColumns.Items.Contains(kv.Value))
                    {
                        listBoxExcelColumns.Items.Add(kv.Value);
                    }

                }
            }
        }

        private void 删除选中配置文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxExitFile.SelectedIndex != -1)
            {
                //
                System.IO.File.Delete(listBoxExitFile.SelectedItem.ToString());
                LoadfileTOlistBox();
            }
        }
    }
}
