using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using HLH.Lib.List;
using HLH.Lib.Office.Excel;
using System.IO;
using System.Reflection;
using RUINORERP.Business;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using HLH.Lib.Helper;
using ReflectionHelper = RUINORERP.Common.Helper.ReflectionHelper;
using SqlSugar;
using CommonProcess.StringProcess;

namespace RUINORERP.UI.DevTools
{
    [MenuAttrAssemblyInfo("数据导入", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataImportTool : UserControl
    {

        public UCDataImportTool()
        {
            InitializeComponent();

        }

        private void buttonEx1_Click(object sender, EventArgs e)
        {
            //int a = 5;
            //int b = 6;
            //int c = a + b;
            //MessageBox.Show(c.ToString());
            ImportData();
        }


        private void ImportData()
        {
            string sourcePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Execl files (*.csv,xlsx)|*.csv;*.xlsx|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sourcePath = openFileDialog.FileName;
            }
            else
            {
                return;
            }

            System.IO.FileInfo fi = new FileInfo(sourcePath);
            if (fi.Extension.ToUpper() == ".XLS" || fi.Extension.ToUpper() == ".XLSX")
            {
                DataTable dt = new DataTable();
                dt = NopiExcelOpretaUtil.ExcelToTable(sourcePath);
                dataGridView1.DataSource = dt;
                MainForm.Instance.PrintInfoLog("查询到数据：" + dt.Rows.Count.ToString() + "行");
            }
            else
            {

                int titleRowIndex = 0;
                System.Text.Encoding encoding = UTF8Encoding.UTF8;
                DataTable dt = new DataTable();
                System.IO.FileStream fs = new System.IO.FileStream(sourcePath, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);

                System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

                //这个报告 文件比较特别。日语 英语 都是第8行为标题

                //记录每次读取的一行记录
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine = null;
                string[] tableHead = null;
                //标示列数
                int columnCount = 0;
                //标示是否是读取的第一行
                bool IsFirst = true;
                //逐行读取CSV中的数据
                int rowNum = 0;
                while ((strLine = sr.ReadLine()) != null)
                {
                    rowNum++;
                    if (rowNum < titleRowIndex)
                    {
                        continue;
                    }
                    if (IsFirst == true)
                    {
                        List<KeyValuePair<string, string>> colsList = new List<KeyValuePair<string, string>>();
                        ///string zhStrLine = APIHelper.BaiduFanyi.translate.translatebyBaidu(strLine, "jp", "zh");
                        //翻译了，全角逗号

                        tableHead = strLine.Split(new string[] { "，" }, StringSplitOptions.None);
                        if (tableHead.Length < 3)
                        {
                            tableHead = strLine.Split(new string[] { "," }, StringSplitOptions.None);
                        }
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        //创建列
                        for (int i = 0; i < columnCount; i++)
                        {
                            string dcName = tableHead[i].Trim().Replace("\"", "").Replace("\"", "");
                            colsList.Add(new KeyValuePair<string, string>(dcName, ""));
                            DataColumn dc = new DataColumn(dcName);//去掉"
                            dt.Columns.Add(dc);
                        }

                        //Type t = typeof(tbAM每月交易报告JPEntity);
                        ////检查匹配文件在不在
                        //ServiceMatchColumn sm = new ServiceMatchColumn();
                        //List<MatchColumn> mc = sm.LoadColumnsList(t);
                        //if (mc.Count == 0)
                        //{
                        //    //需要第一次加载
                        //    mc = sm.SetMatchColumns(t, colsList);
                        //    sm.SaveColumnsList(t, mc);
                        //}
                    }
                    else
                    {
                        List<string> rs = FieldValueSplitClass.GetPairsMatch(strLine);
                        //aryLine = strLine.Split(new string[] { "," }, StringSplitOptions.None);//不能移除空白
                        aryLine = rs.ToArray();
                        if (aryLine.Length <= 1)
                        {
                            continue;
                        }


                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {


                            dr[j] = aryLine[j].Trim().Replace("\"", "");//去掉"


                        }
                        dt.Rows.Add(dr);


                    }
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    //dt.DefaultView.Sort = tableHead[0].Trim().Replace("\"", "") + " " + "asc";
                }

                sr.Close();
                fs.Close();

                dataGridView1.DataSource = dt;
                MainForm.Instance.PrintInfoLog("查询到数据：" + dt.Rows.Count.ToString() + "行");

            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewPaintParts paintParts =
                    e.PaintParts & ~DataGridViewPaintParts.Focus;

                e.Paint(e.ClipBounds, paintParts);
                e.Handled = true;
            }

            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }



        private void buttonEx2_Click(object sender, EventArgs e)
        {

            if (cmb导入所属数据表.SelectedValue == null || cmb导入所属数据表.SelectedValue.ToString() == "请选择")
            {
                MessageBox.Show("请选择导入数据的表名。");
                return;
            }
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("请先导入数据，或粘贴来自Excel的数据。");
                return;
            }

            frmDbColumnToExlColumnConfig frm = new frmDbColumnToExlColumnConfig();
            frm.ImportTargetTableName = cmb导入所属数据表.SelectedValue.ToString();
            frm.dvExcel = dataGridView1;
            if (cmbColumnMappingFile.SelectedIndex == -1)
            {
                MessageBox.Show("请选择对应的配置文件。");

                if (MessageBox.Show("如果没有对应配置文件，\r\n将重新创建！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    //if (!System.IO.File.Exists(UserGlobalConfig.Instance.MatchColumnsConfigDir+ frm.ImportTargetTableName))
                    //{
                    //    System.IO.File.Create(UserGlobalConfig.Instance.MatchColumnsConfigDir);
                    //}

                    frm.Show();
                }
            }
            else
            {
                if (cmbColumnMappingFile.SelectedItem is CmbItem)
                {
                    CmbItem ci = cmbColumnMappingFile.SelectedItem as CmbItem;
                    frm.txtMatchConfigFileName.Text = ci.Key;
                    frm.Show();
                }

            }

        }

        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewPaintParts paintParts =
                    e.PaintParts & ~DataGridViewPaintParts.Focus;

                e.Paint(e.ClipBounds, paintParts);
                e.Handled = true;
            }

            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }


        public string[] LoadConfigFile()
        {
            List<string> files = new List<string>();
            if (!System.IO.Directory.Exists(UserGlobalConfig.Instance.MatchColumnsConfigDir))
            {
                System.IO.Directory.CreateDirectory(UserGlobalConfig.Instance.MatchColumnsConfigDir);
            }
            string[] fi = System.IO.Directory.GetFiles(UserGlobalConfig.Instance.MatchColumnsConfigDir);
            return fi;
        }

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:

                        break;
                    case Keys.F1:

                        break;
                }

            }
            return false;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadfileToCmbbox()
        {
            cmbColumnMappingFile.Items.Clear();
            string[] files = LoadConfigFile();
            foreach (string f in files)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(f);
                CmbItem ci = new CmbItem(f, fi.Name);
                cmbColumnMappingFile.Items.Add(ci);

            }
            if (cmbColumnMappingFile.SelectedItem == null || cmbColumnMappingFile.SelectedIndex == -1)
            {
                //MessageBox.Show("没有找到对应的匹配文件，请先配置。");
            }
        }


        private void frmDataImportTools_Load(object sender, EventArgs e)
        {
            this.dataGridView1.SetUseCustomColumnDisplay(false);
            this.dataGridView2.SetUseCustomColumnDisplay(false);
            this.Text = "2023-9-5最新版本修改";

            DataTable dt = MainForm.Instance.AppContext.Db.Ado.GetDataTable("select TABLE_NAME from information_schema.tables where  table_catalog = @table_catalog order by TABLE_NAME",
new { table_catalog = "erpnew" });
            //DataSet ds = new DataSet();
            //ds.Tables.Add(dt);
            HLH.Lib.Helper.DropDownListHelper.InitDropList(dt.DataSet, cmb导入所属数据表, "table_name", "table_name", ComboBoxStyle.DropDown,true, true);
          
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            LoadfileToCmbbox();
        }

        List<SuperKeyValuePair> dclist = new List<SuperKeyValuePair>();
        private void btn查看结果_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            bindingSource结果.DataSource = null;
            dataGridView2.DataSource = bindingSource结果;
            tabControl1.SelectedIndex = 1;
            //先取配置文件 
            string filepath = string.Empty;
            if (cmbColumnMappingFile.SelectedItem is CmbItem)
            {
                CmbItem ci = cmbColumnMappingFile.SelectedItem as CmbItem;
                filepath = ci.Key;
            }

            if (System.IO.File.Exists(filepath))
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(filepath);
                string xml = HLH.Lib.Helper.FileIOHelper.FileDirectoryUtility.ReadFile(filepath, System.Text.Encoding.UTF8);
                //载入配置
                dclist = HLH.Lib.Xml.XmlUtil.Deserialize(typeof(List<SuperKeyValuePair>), xml) as List<SuperKeyValuePair>;
                UpdateDataToDBForView(dclist);



            }

        }

        private void btn保存结果_Click(object sender, EventArgs e)
        {
            SaveRowCounter = 0;
            if (MessageBox.Show(this, "开始将数据保存到系统中\r\n 你确定要执行吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                DateTime dtt = System.DateTime.Now;
                bindingSource结果.DataSource = UpdateDataToDBForSave(dclist);
                TimeSpan ts = DateTime.Now - dtt;
                MainForm.Instance.PrintInfoLog("影响数据行数:" + SaveRowCounter + "导入操作耗时：" + ts.TotalSeconds, Color.Red);
            }
        }


        private Type GetTypeByTableName(string tableName)
        {
            string dllfileTop = System.IO.Path.Combine(Environment.CurrentDirectory, "RUINORERP.Model.dll");
            Assembly assem = Assembly.LoadFile(dllfileTop);
            //string ClassName = GetClassNameOfTableName(tableName);
            Type type = assem.GetType(assem.GetName().Name + "." + tableName, true);
            return type;
        }


        private void UpdateDataToDBForView(List<SuperKeyValuePair> dclist)
        {
            string tableName = cmb导入所属数据表.SelectedValue.ToString();
            BindingSortCollection<object> sortList = new BindingSortCollection<object>();
            Type t = GetTypeByTableName(tableName);
            DataTable dt = new DataTable();
            dt = dataGridView1.DataSource as DataTable;
            if (dt == null)
            {
                return;
            }
            #region 处理数据
            string KeyColName = string.Empty;
            string KeyColNameExcel = string.Empty;
            foreach (var item in dclist)
            {
                if (item.Tag != null)
                {
                    bool iskey = false;
                    bool.TryParse(item.Tag.ToString(), out iskey);
                    if (iskey)
                    {
                        KeyColName = item.Key.ToString();
                        KeyColNameExcel = item.Value.ToString();
                        break;
                    }
                }
            }

            if (KeyColName.Trim().Length == 0 && KeyColNameExcel.Trim().Length == 0)
            {
                if (MessageBox.Show("匹配结果中没有设置逻辑主键！，则只能一次性增加无法更新,是否继续", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            var EntityFieldNameList = UIHelper.GetALLFieldInfoList(t);
            var dtDicUpdateList = new List<Dictionary<string, object>>();
            var dtDicAddList = new List<Dictionary<string, object>>();
            string pkColName = EntityFieldNameList.FirstOrDefault(b => b.SugarCol.IsPrimaryKey == true).SugarCol.ColumnName;
            //真正遍历数据行
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[KeyColNameExcel].ToString().Trim().Length > 0)
                {
                    #region 处理导入的数据
                    try
                    {
                        var dcrow = new Dictionary<string, object>();
                        //创建一个实体对象
                        object si = Activator.CreateInstance(t);
                        //这个列表需要检测  更新的是匹配结果的列的集合了。
                        foreach (SuperKeyValuePair dc in dclist)
                        {
                            ///检测这个列是否存在。
                            //获取属性信息,并判断是否存在
                            PropertyInfo property = t.GetProperty(dc.Key.ToString());
                            if (property != null)
                            {
                                if (dc.Key.superDataTypeName == "datetime")
                                {
                                    string tempDateTime = dr[dc.Value.ToString()].ToString();
                                    /*
                                     查了一下，原因是这个数据的格式是日期。Excel日期格式的数据实际上是一个int，从1900年1月1日开始算起。1900/1/1是1，1900/1/2是2，以此类推。
                                    C#里要将这个整型数据转换成DateTime类型的数据也很方便。有一个方法叫FromOADate。
                                    举例：从44413转成2021/08/05的代码：
                                   https://blog.csdn.net/Allison_Q/article/details/120194579
                                     */
                                    if (tempDateTime.Length == 5)
                                    {
                                        string dateStr = DateTime.FromOADate(Convert.ToInt32(tempDateTime)).ToString("yyyy/MM/dd");
                                        DateTime dte = System.DateTime.MinValue;
                                        if (DateTime.TryParse(dateStr, out dte))
                                        {
                                            dr[dc.Value.ToString()] = dte.ToString();
                                        }
                                        //指定转换格式 
                                        //System.IFormatProvider format = new System.Globalization.CultureInfo("zh-CN", true);
                                        //string strDateFormat = "yyyyMMdd";
                                        //DateTime date = DateTime.ParseExact(dateStr, strDateFormat, format, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                                        //dr[dc.Value.ToString()] = date.ToString();
                                    }
                                    else
                                    {
                                        if (tempDateTime.Length == 8)
                                        {
                                            string dateStr = tempDateTime;
                                            //指定转换格式 
                                            System.IFormatProvider format = new System.Globalization.CultureInfo("zh-CN", true);
                                            string strDateFormat = "yyyyMMdd";
                                            DateTime date = DateTime.ParseExact(dateStr, strDateFormat, format, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                                            dr[dc.Value.ToString()] = date.ToString();
                                        }
                                        else
                                        {
                                            DateTime dte = System.DateTime.MinValue;
                                            if (DateTime.TryParse(tempDateTime, out dte))
                                            {
                                                dr[dc.Value.ToString()] = dte.ToString();
                                            }
                                        }
                                    }

                                }

                                if (dr[dc.Value.ToString()].ToString().Trim().Length > 0)
                                {
                                    //转换合适类型
                                    var col = EntityFieldNameList.Where(c => c.SugarCol != null && c.SugarCol.ColumnName != null && c.FieldName == dc.Key.ToString()).FirstOrDefault();
                                    TypeConverter converter = new TypeConverter();

                                    Type newcolType; ;
                                    // We need to check whether the property is NULLABLE
                                    if (col.ColDataType.IsGenericType && col.ColDataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                                        newcolType = col.ColDataType.GetGenericArguments()[0];
                                    }
                                    else
                                    {
                                        newcolType = col.ColDataType;
                                    }
                                    //var tarobj = converter.ConvertTo(dr[dc.Value.ToString()], newcolType);
                                    var tarobj = dr[dc.Value.ToString()].ToString().Convert(newcolType);
                                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(si, dc.Key.ToString(), tarobj);
                                    //ReflectionHelper.SetPropertyValue(si, dc.Key.ToString(), tarobj);
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format("导入列名:{0}不存在于实体{1}中。", dc.Key.ToString(), t.Name));
                                return;
                            }
                        }
                        ///检测这个列是否存在。
                        //获取属性信息,并判断是否存在
                        PropertyInfo property导入时间 = t.GetProperty("导入时间");
                        if (property导入时间 != null)
                        {
                            ReflectionHelper.SetPropertyValue(si, "导入时间", System.DateTime.Now);
                        }
                        foreach (Model.Base.BaseDtoField baseDtoField in EntityFieldNameList.Where(c => c.SugarCol != null && c.SugarCol.ColumnName != null).ToList())
                        {
                            if (baseDtoField.SugarCol.IsPrimaryKey)
                            {
                                continue;
                            }
                            if (baseDtoField.IsFKRelationAttribute)
                            {
                                object fkcolValue = ReflectionHelper.GetPropertyValue(si, baseDtoField.SugarCol.ColumnName);
                                if (fkcolValue != null)
                                {
                                    dcrow.Add(baseDtoField.SugarCol.ColumnName, fkcolValue);
                                }
                                else
                                {
                                    dcrow.Add(baseDtoField.SugarCol.ColumnName, null);
                                }
                                continue;
                            }
                            //跳过图片类型的列
                            if (baseDtoField.SugarCol.ColumnDataType == "image")
                            {
                                continue;
                            }
                            object dccolValue = ReflectionHelper.GetPropertyValue(si, baseDtoField.SugarCol.ColumnName).ToString();
                            dcrow.Add(baseDtoField.SugarCol.ColumnName, dccolValue);
                        }
                        sortList.Add(si);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                        break;
                    }
                    #endregion
                }
            }

            #endregion
            bindingSource结果.DataSource = sortList;
            //对比数据库中的数据
            #region 对比数据库中的数据

            //为了减少每次操作都查询一下orderid ，思路是将 导入数据的id先 组合起来查出来。再内存找查。 这个只是单系统。分布式。多线 可能会乱

            List<string> logicPKValueList = new List<string>();
            if (KeyColNameExcel.Length > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[KeyColNameExcel].ToString().Trim().Length > 0)
                    {
                        logicPKValueList.Add(dr[KeyColNameExcel].ToString().Trim());
                    }
                }
            }

            var conModels = new List<IConditionalModel>();
            conModels.Add(new ConditionalModel
            {
                FieldName = KeyColName,
                ConditionalType = ConditionalType.In,
                FieldValue = string.Join(",", logicPKValueList)
            });
            object queryList = MainForm.Instance.AppContext.Db.Queryable(tableName, "tn").Where(conModels).ToDataTable();
            List<DataRow> drs = new List<DataRow>();

            //标记一下Excel数据源 查出来的 背景色区别一下 同时真正主键的值 显示的UI
            foreach (DataGridViewRow dvr in dataGridView2.Rows)
            {
                if (dvr.Cells[KeyColName].Value != null)
                {
                    bool hasRow = false;
                    //这里是数据库里的。
                    foreach (System.Data.DataRow dr in (queryList as DataTable).Rows)
                    {
                        #region 真正主键显示到Ui
                        //通过逻辑主键来得到真正主键的值
                        if (dr[KeyColName].ToString() == dvr.Cells[KeyColName].Value.ToString())
                        {
                            Model.Base.BaseDtoField col = new Model.Base.BaseDtoField();
                            //转换合适类型
                            foreach (Model.Base.BaseDtoField item in EntityFieldNameList)
                            {
                                if (item.SugarCol.ColumnName == pkColName)
                                {
                                    col = item;
                                    break;
                                }
                            }

                            TypeConverter converter = new TypeConverter();
                            Type newcolType;
                            if (col.ColDataType.IsGenericType && col.ColDataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                newcolType = col.ColDataType.GetGenericArguments()[0];
                            }
                            else
                            {
                                newcolType = col.ColDataType;
                            }
                            var tarobj = dr[pkColName].ToString().Convert(newcolType);
                            RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dvr.DataBoundItem, pkColName, tarobj);

                            //外面是excel的。对应值不同则颜色为？不同
                            foreach (Model.Base.BaseDtoField item in EntityFieldNameList)
                            {
                                if (dr[item.SugarCol.ColumnName] == null || dvr.Cells[item.SugarCol.ColumnName].Value == null)
                                {
                                    continue;
                                }
                                if ((item.SugarCol.ColumnName != KeyColName && item.SugarCol.ColumnName != pkColName) && dr[item.SugarCol.ColumnName].ToString() != dvr.Cells[item.SugarCol.ColumnName].Value.ToString())
                                {
                                    //值不同，用不同颜色提示
                                    dvr.DefaultCellStyle.BackColor = Color.LightGray;
                                    dvr.DefaultCellStyle.ForeColor = Color.DarkOrange;
                                }
                            }

                            #endregion




                            if (dr[KeyColName].ToString() == dvr.Cells[KeyColName].Value.ToString())
                            {
                                hasRow = true;
                                continue;
                            }
                        }
                    }

                    if (hasRow)
                    {
                        dvr.DefaultCellStyle.BackColor = Color.LightGray;
                        dvr.DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    else
                    {
                        dvr.DefaultCellStyle.BackColor = Color.FloralWhite;
                        dvr.DefaultCellStyle.ForeColor = Color.DarkBlue;
                    }



                }

            }


            #endregion
        }

        int SaveRowCounter = 0;
        private BindingSortCollection<object> UpdateDataToDBForSave(List<SuperKeyValuePair> dclist)
        {

            string tableName = cmb导入所属数据表.SelectedValue.ToString();
            //随便用一个。只有公共方法

            Type t = GetTypeByTableName(tableName);
            var genericListType = typeof(List<>);
            var specificListType = genericListType.MakeGenericType(t);
            var rslist = Activator.CreateInstance(specificListType);
            BindingSortCollection<object> sortList = new BindingSortCollection<object>();
            rslist = bindingSource结果.DataSource;
            var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
            if (lastlist != null)
            {

            }



            var EntityFieldNameList = UIHelper.GetALLFieldInfoList(t);
            var dtDicUpdateList = new List<Dictionary<string, object>>();
            var dtDicAddList = new List<Dictionary<string, object>>();
            string pkColName = EntityFieldNameList.FirstOrDefault(b => b.SugarCol.IsPrimaryKey == true).SugarCol.ColumnName;

            //真正遍历数据行
            foreach (var row in lastlist)
            {
                SaveRowCounter++;
                #region 处理导入的数据
                try
                {


                    var dcrow = new Dictionary<string, object>();
                    //创建一个实体对象

                    foreach (Model.Base.BaseDtoField baseDtoField in EntityFieldNameList.Where(c => c.SugarCol != null && c.SugarCol.ColumnName != null).ToList())
                    {
                        if (baseDtoField.SugarCol.IsPrimaryKey)
                        {

                            continue;
                        }
                        if (baseDtoField.IsFKRelationAttribute)
                        {
                            object fkcolValue = ReflectionHelper.GetPropertyValue(row, baseDtoField.SugarCol.ColumnName);
                            if (fkcolValue != null)
                            {
                                dcrow.Add(baseDtoField.SugarCol.ColumnName, fkcolValue);
                            }
                            else
                            {
                                dcrow.Add(baseDtoField.SugarCol.ColumnName, null);
                            }
                            continue;
                        }
                        //跳过图片类型的列
                        if (baseDtoField.SugarCol.ColumnDataType == "image")
                        {
                            continue;
                        }
                        object dccolValue = ReflectionHelper.GetPropertyValue(row, baseDtoField.SugarCol.ColumnName).ToString();
                        dcrow.Add(baseDtoField.SugarCol.ColumnName, dccolValue);
                    }

                    long pkvalue = long.Parse(ReflectionHelper.GetPropertyValue(row, pkColName).ToString());
                    //===
                    //如果只是看结果 就显示0，如果是直接保存新增 就给ID值
                    if (pkvalue == 0)
                    {
                        long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                        dcrow.Add(pkColName, sid);
                        RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(row, pkColName, sid);

                        dtDicAddList.Add(dcrow);
                    }
                    else
                    {
                        dcrow.Add(pkColName, pkvalue);
                        dtDicUpdateList.Add(dcrow);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                    break;
                }
                #endregion
            }
            MainForm.Instance.AppContext.Db.Updateable(dtDicUpdateList).AS(tableName).WhereColumns(pkColName).ExecuteCommand();
            MainForm.Instance.AppContext.Db.Insertable(dtDicAddList).AS(tableName).ExecuteReturnEntity();

            foreach (var item in lastlist)
            {
                sortList.Add(item);
            }
            return sortList;

        }



        private void 粘贴来自excel的数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            //要把列复制  所以第一行认为是列
            if (string.IsNullOrEmpty(Clipboard.GetText().Trim()))
            {
                MessageBox.Show("剪切板为空！");
                return;
            }

            string copyText = System.Windows.Forms.Clipboard.GetText();

            DataTable DtTable = new DataTable();

            //string[] allRow = copyText.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string[] allRow = copyText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            //复制数据时，建议不需要标题了，因为标题不规则。 
            //判断是否为好的数据 为一行中 每列的值不为空。 如果第一行就这样，则默认给出列名

            //为了处理列名。特别处理第一行
            #region
            string[] ColumnsContent = allRow[0].Split(new string[] { "\t" }, StringSplitOptions.None);
            bool firstRowIsValue = false;//默认第一行为列名。不是值
                                         //处理列名
            for (int c = 0; c < ColumnsContent.Length; c++)
            {
                //判断是否为好的数据 为一行中 每列的值不为空。 如果第一行就这样，则默认给出列名
                foreach (string item in ColumnsContent.ToArray())
                {
                    if (item.Trim().Length == 0)
                    {
                        firstRowIsValue = true;
                        break;
                    }
                }

                //第二个特殊 。一般列名不包括数字
                foreach (string item in ColumnsContent.ToArray())
                {
                    if (HLH.Lib.Helper.RegexProcessHelper.IsContainDdigital(item.ToString(), out int temp))
                    {
                        firstRowIsValue = true;
                        break;
                    }
                }
                DataColumn dc = new DataColumn();

                if (firstRowIsValue)
                {
                    dc.ColumnName = "列" + (c + 1).ToString();
                }
                else
                {
                    if (ColumnsContent[c].Trim().Length > 0)
                    {
                        dc.ColumnName = ColumnsContent[c];
                    }
                }
                DtTable.Columns.Add(dc);
            }
            #endregion
            int row = 0;
            if (firstRowIsValue)
            {
                row = 0;
            }
            else
            {
                row = 1;
            }
            for (int i = row; i < allRow.Length; i++)
            {
                //把每行的数据按单元格截取，放到一个string数组里，第二个参数是不返回空字符  这里要返回空。因为有些没有列名。这时需要程序给出默认值。才能匹配列。
                string[] content = allRow[i].Trim().Split(new string[] { "\t" }, StringSplitOptions.None);

                //新增行
                DataRowView dr = DtTable.DefaultView.AddNew();
                //复制的数据列大于等于当前表格列
                if (content.Length >= DtTable.Columns.Count)
                {
                    for (int j = 0; j < DtTable.Columns.Count; j++)
                    {
                        dr[j] = content[j];
                    }
                }
                //赋值的数据列小于当前表格列
                else if (content.Length < DtTable.Columns.Count)
                {
                    for (int j = 0; j < content.Length; j++)
                    {
                        dr[j] = content[j];
                    }
                }
                dr.EndEdit();
            }
            //如果没有配置控制列的文件，在显示前给一个默认值是全部
            if (string.IsNullOrEmpty(dataGridView1.XmlFileName))
            {
                dataGridView1.XmlFileName = this.Name + "importtool";
            }
            //UseCustomColumnDisplay
            dataGridView1.DataSource = DtTable;


        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void 处理当前列值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<KeyValue<string, string>> processInputResult = new List<KeyValue<string, string>>();
            //进出处理值 。变更到DG中
            foreach (DataGridViewCell item in dataGridView1.SelectedCells)
            {
                if (item.Selected)
                {
                    KeyValue<string, string> kv = new KeyValue<string, string>();
                    kv.Key = item.Value;
                    processInputResult.Add(kv);
                }
            }

            frmTextProcesserTest frm = new frmTextProcesserTest();
            frm.ProcessInputResult = processInputResult;
            frm.OtherEvent += Frm_OtherEvent;
            frm.Show();
        }

        private void Frm_OtherEvent(Form frmPro, object Parameters)
        {

            frmTextProcesserTest frm = frmPro as frmTextProcesserTest;
            List<KeyValue<string, string>> processInputResult = new List<KeyValue<string, string>>();
            processInputResult = Parameters as List<KeyValue<string, string>>;
            //进出处理值 。变更到DG中
            foreach (DataGridViewCell item in dataGridView1.SelectedCells)
            {
                if (item.Selected)
                {
                    if (item.Selected)
                    {
                        item.Value = frm.ProcessData("", item.Value.ToString());
                    }

                }
            }
        }

        private void 从导入结果表格中更新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "开始将DG控件数据更新到系统中\r\n 你确定要执行吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                DateTime dtt = System.DateTime.Now;
                UpdateDatafromDG2ToDB();
                TimeSpan ts = DateTime.Now - dtt;
                MainForm.Instance.PrintInfoLog("导入操作耗时：" + ts.TotalSeconds, Color.Red);
            }
        }



        private BindingSortCollection<object> UpdateDatafromDG2ToDB()
        {

            Type t = GetTypeByTableName(cmb导入所属数据表.SelectedValue.ToString());

            //使用事务处理，加快速度
            List<KeyValuePair<string, List<IDataParameter>>> sqlList = new List<KeyValuePair<string, List<IDataParameter>>>();


            List<object> ExistOrders = new List<object>();

            BindingSortCollection<object> sortList = new BindingSortCollection<object>();

            if (dataGridView2.DataSource is BindingSortCollection<object>)
            {
                sortList = dataGridView2.DataSource as BindingSortCollection<object>;
            }
            else
            {
                ExistOrders = dataGridView2.DataSource as List<object>;
                foreach (var item in ExistOrders)
                {
                    sortList.Add(item);
                }
            }


            int counter = 0;
            #region 处理数据


            //使用事务处理，加快速度

            ///创建实例
            object si = Activator.CreateInstance(t);

            #region 处理数据

            //找更新用的主键
            string KeyColName = string.Empty;

            foreach (var item in dclist)
            {
                if (item.Tag != null)
                {
                    bool iskey = false;
                    bool.TryParse(item.Tag.ToString(), out iskey);
                    if (iskey)
                    {
                        KeyColName = item.Key.ToString();
                        break;
                    }

                }
            }



            if (KeyColName.Trim().Length == 0)
            {
                MessageBox.Show("请在匹配结果中设置逻辑主键！");
                return sortList;
            }

            //###MAXID 先取出。批量事务时。第一次指定，后面自动加1.

            #endregion


            //循环对象行，给值 ，更新
            foreach (object obj in sortList)
            {


                #region 处理导入的数据
                try
                {
                    HLH.Lib.Helper.JsonHelper jsonhelper = new HLH.Lib.Helper.JsonHelper();

                    //必须是更新
                    si = Activator.CreateInstance(t);
                    //si = ExistOrders.Find(delegate (object o) { return ReflectionHelper.GetPropertyValue(o, KeyColName) == dr[KeyColNameExcel].ToString().Trim(); });
                    si = obj;
                    if (si == null)
                    {
                        //实际不应该到这步
                        MainForm.Instance.PrintInfoLog("要更新的对象数据不能为空。");
                        continue;
                    }



                    ///检测这个列是否存在。
                    //获取属性信息,并判断是否存在
                    PropertyInfo property导入时间 = t.GetProperty("导入时间");
                    if (property导入时间 != null)
                    {
                        ReflectionHelper.SetPropertyValue(si, "导入时间", System.DateTime.Now);
                    }


                    if (int.Parse(ReflectionHelper.GetPropertyValue(si, "ID").ToString()) > 0)
                    {
                        KeyValuePair<string, List<IDataParameter>> updatesqlList = new KeyValuePair<string, List<IDataParameter>>();
                        MethodInfo GetUpdateTranSqlByParameter = t.GetMethod("GetUpdateTranSqlByParameter");//加载方法

                        object updatesqlobj = GetUpdateTranSqlByParameter.Invoke(si, null);//执行
                        updatesqlList = (KeyValuePair<string, List<IDataParameter>>)updatesqlobj;

                        sqlList.Add(updatesqlList);
                    }

                    counter++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }
                #endregion


            }

            #endregion

            MethodInfo ExecuteTransactionByParameter = t.GetMethod("ExecuteTransactionByParameter", new Type[] { typeof(List<KeyValuePair<string, List<IDataParameter>>>) });//加载方法
            Object[] LastParas = new Object[] { sqlList };
            ExecuteTransactionByParameter.Invoke(si, LastParas);//执行

            return sortList;

        }

        private void cmbColumnMappingFile_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 生成自定义SQL脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //frmCreateSql frm = new frmCreateSql();
            //frm.dataGridView1 = dataGridView1;
            //if (frm.ShowDialog() == DialogResult.OK)
            //{

            //}
        }
    }
}
