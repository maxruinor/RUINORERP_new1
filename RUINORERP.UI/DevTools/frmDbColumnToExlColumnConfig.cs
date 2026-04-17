using HLH.Lib.Helper;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

            // 获取实体类型以提取元数据
            Type entityType = GetEntityTypeByName(ImportTargetTableName);
            var properties = entityType?.GetProperties() ?? Array.Empty<System.Reflection.PropertyInfo>();

            DataTable dt = MainForm.Instance.AppContext.Db.Ado.GetDataTable(" select COLUMN_NAME, DATA_TYPE from information_schema.columns where table_name = @table_name",
new { table_name = ImportTargetTableName });

            foreach (DataRow dr in dt.Rows)
            {
                string colName = dr["COLUMN_NAME"].ToString();
                // 尝试从实体特性中获取中文描述
                var prop = properties.FirstOrDefault(p => 
                    p.GetCustomAttribute<SqlSugar.SugarColumn>()?.ColumnName == colName || 
                    p.Name == colName);
                
                string description = prop?.GetCustomAttribute<SqlSugar.SugarColumn>()?.ColumnDescription ?? colName;
                SuperValue sv = new SuperValue(colName, dr["DATA_TYPE"].ToString());
                sv.Tag = description; // 将中文描述存入 Tag 以便智能匹配
                listBoxDbColumns.Items.Add(sv);
            }

            #endregion

            LoadfileTOlistBox();
            if (txtMatchConfigFileName.Text.Trim().Length == 0)
            {
                txtMatchConfigFileName.Text=ImportTargetTableName+".xml";
            }

            // 【新增】执行智能自动匹配
            AutoMatchColumns();
        }

        /// <summary>
        /// 根据表名获取对应的实体类型
        /// </summary>
        private Type GetEntityTypeByName(string tableName)
        {
            try
            {
                var assembly = System.Reflection.Assembly.Load("RUINORERP.Model");
                return assembly.GetTypes().FirstOrDefault(t => 
                    t.GetCustomAttribute<SqlSugar.SugarTable>()?.TableName == tableName || 
                    t.Name == tableName);
            }
            catch { return null; }
        }

        /// <summary>
        /// 应用导入模板 - 自动完成列匹配
        /// </summary>
        public void ApplyTemplate(RUINORERP.Business.ImportTemplate template)
        {
            if (template == null || template.ColumnMappings == null) return;

            kvList.Clear();
            listBoxExcelColumns.Items.Clear();
            listBoxDbColumns.Items.Clear();

            // 重新加载所有列
            foreach (DataGridViewColumn item in dvExcel.Columns)
            {
                listBoxExcelColumns.Items.Add(new SuperValue(item.Name, ""));
            }

            DataTable dt = MainForm.Instance.AppContext.Db.Ado.GetDataTable(
                "select COLUMN_NAME, DATA_TYPE from information_schema.columns where table_name = @tn", 
                new { tn = ImportTargetTableName });

            var dbItems = new List<SuperValue>();
            foreach (DataRow dr in dt.Rows)
            {
                var sv = new SuperValue(dr["COLUMN_NAME"].ToString(), dr["DATA_TYPE"].ToString());
                dbItems.Add(sv);
                listBoxDbColumns.Items.Add(sv);
            }

            // 根据模板进行自动匹配
            foreach (var mapping in template.ColumnMappings)
            {
                string excelCol = mapping.Key;
                string dbCol = mapping.Value;

                var excelItem = listBoxExcelColumns.Items.Cast<SuperValue>().FirstOrDefault(e => e.superStrValue == excelCol);
                var dbItem = dbItems.FirstOrDefault(d => d.superStrValue == dbCol);

                if (excelItem != null && dbItem != null)
                {
                    var kvp = new SuperKeyValuePair(
                        new SuperValue(dbItem.superStrValue, dbItem.superDataTypeName),
                        new SuperValue(excelItem.superStrValue, "")
                    );
                    
                    // 标记逻辑主键
                    if (dbCol == template.LogicalKeyField)
                    {
                        kvp.Tag = true;
                    }

                    kvList.Add(kvp);
                    listBoxExcelColumns.Items.Remove(excelItem);
                    listBoxDbColumns.Items.Remove(dbItem);
                }
            }

            LoadListTolistbox匹配结果(kvList);
            MainForm.Instance.PrintInfoLog($"已根据模板 [{template.TemplateName}] 自动匹配 {kvList.Count} 个字段。");
        }

        private void AutoMatchColumns()
        {
            var excelItems = listBoxExcelColumns.Items.Cast<SuperValue>().ToList();
            var dbItems = listBoxDbColumns.Items.Cast<SuperValue>().ToList();

            // 尝试获取实体元数据以增强匹配精度
            Type entityType = GetEntityTypeByName(ImportTargetTableName);
            BaseEntity sampleEntity = null;
            List<ImportFieldInfo> fieldInfos = null;
            if (entityType != null)
            {
                sampleEntity = Activator.CreateInstance(entityType) as BaseEntity;
                fieldInfos = sampleEntity?.ImportableFields;
            }

            foreach (var excelItem in excelItems.ToList())
            {
                string excelName = excelItem.superStrValue;
                SuperValue bestMatch = null;
                double maxScore = 0;

                foreach (var dbItem in dbItems)
                {
                    double score = CalculateSmartSimilarity(excelName, dbItem, fieldInfos);
                    if (score > maxScore)
                    {
                        maxScore = score;
                        bestMatch = dbItem;
                    }
                }

                // 设定阈值，防止错误匹配
                if (bestMatch != null && maxScore > 0.65)
                {
                    var kvp = new SuperKeyValuePair(
                        new SuperValue(bestMatch.superStrValue, bestMatch.superDataTypeName),
                        new SuperValue(excelItem.superStrValue, "")
                    );

                    // 智能识别逻辑主键：如果该字段是物理主键或包含“编号/代码”等关键字
                    if (fieldInfos != null)
                    {
                        var info = fieldInfos.FirstOrDefault(f => f.ColumnName == bestMatch.superStrValue || f.PropertyName == bestMatch.superStrValue);
                        if (info != null && (info.IsPrimaryKey || IsLogicalKeyCandidate(info)))
                        {
                            kvp.Tag = true; // 标记为逻辑主键
                        }
                    }

                    kvList.Add(kvp);
                    listBoxExcelColumns.Items.Remove(excelItem);
                    listBoxDbColumns.Items.Remove(bestMatch);
                }
            }
            LoadListTolistbox匹配结果(kvList);
        }

        /// <summary>
        /// 综合计算相似度（结合编辑距离和语义描述）
        /// </summary>
        private double CalculateSmartSimilarity(string excelName, SuperValue dbItem, List<ImportFieldInfo> fieldInfos)
        {
            if (string.IsNullOrEmpty(excelName)) return 0;

            string dbColName = dbItem.superStrValue;
            string dbDesc = dbItem.Tag?.ToString() ?? "";

            // 1. 精确匹配或包含匹配
            if (excelName.Equals(dbColName, StringComparison.OrdinalIgnoreCase) || 
                excelName.Equals(dbDesc, StringComparison.OrdinalIgnoreCase)) return 1.0;
            if (dbDesc.Contains(excelName) || excelName.Contains(dbDesc)) return 0.9;

            // 2. 基于描述的编辑距离匹配
            int distance = LevenshteinDistance(excelName.ToLower(), dbDesc.ToLower());
            int maxLen = Math.Max(excelName.Length, dbDesc.Length);
            double descScore = maxLen == 0 ? 1.0 : 1.0 - (double)distance / maxLen;

            // 3. 列名相似度
            distance = LevenshteinDistance(excelName.ToLower(), dbColName.ToLower());
            maxLen = Math.Max(excelName.Length, dbColName.Length);
            double colScore = maxLen == 0 ? 1.0 : 1.0 - (double)distance / maxLen;

            return Math.Max(descScore, colScore) * 0.8; // 给予一定权重
        }

        /// <summary>
        /// 判断是否为潜在的逻辑主键字段
        /// </summary>
        private bool IsLogicalKeyCandidate(ImportFieldInfo info)
        {
            string[] keywords = { "code", "no", "number", "编号", "代码", "单号" };
            string nameLower = (info.Description + info.ColumnName).ToLower();
            return keywords.Any(k => nameLower.Contains(k));
        }

        /// <summary>
        /// 计算两个字符串的简单相似度 (0-1)
        /// </summary>
        private double CalculateSimilarity(string excelName, SuperValue dbItem)
        {
            if (string.IsNullOrEmpty(excelName) || string.IsNullOrEmpty(dbItem.superStrValue)) return 0;

            string dbDesc = dbItem.Tag?.ToString() ?? dbItem.superStrValue;
            
            // 1. 完全匹配（最高优先级）
            if (excelName.Equals(dbItem.superStrValue, StringComparison.OrdinalIgnoreCase)) return 1.0;
            if (excelName.Equals(dbDesc, StringComparison.OrdinalIgnoreCase)) return 1.0;

            // 2. 包含关系（次高优先级）
            if (dbDesc.Contains(excelName) || excelName.Contains(dbDesc)) return 0.85;

            // 3. 编辑距离相似度（更准确的重合度计算）
            int distance = LevenshteinDistance(excelName.ToLower(), dbDesc.ToLower());
            int maxLen = Math.Max(excelName.Length, dbDesc.Length);
            if (maxLen == 0) return 1.0;
            
            return 1.0 - (double)distance / maxLen;
        }

        /// <summary>
        /// 计算莱文斯坦距离（编辑距离）
        /// </summary>
        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
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
                
                if (kv != null && kv.Tag != null)
                {
                    bool isKey = false;
                    bool.TryParse(kv.Tag.ToString(), out isKey);
                    if (isKey)
                    {
                        bgColor = Color.LightGreen; // 逻辑主键用绿色高亮
                        myBrush = Brushes.DarkGreen;
                    }
                }

                // 绘制背景
                e.Graphics.FillRectangle(new SolidBrush(bgColor), e.Bounds);
                // 绘制文字
                e.Graphics.DrawString(listbox匹配结果.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                // 绘制聚焦框
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

        /// <summary>
        /// AI 智能匹配按钮点击事件
        /// </summary>
        private async void btnAiMatch_Click(object sender, EventArgs e)
        {
            if (dvExcel.DataSource == null || string.IsNullOrEmpty(ImportTargetTableName))
            {
                MessageBox.Show("请先加载数据并选择目标表。");
                return;
            }

            try
            {
                // 1. 准备数据
                var excelHeaders = dvExcel.Columns.Cast<DataGridViewColumn>().Select(c => c.Name).ToList();
                
                // 获取实体元数据
                Type entityType = GetEntityTypeByName(ImportTargetTableName);
                if (entityType == null) return;
                
                BaseEntity sampleEntity = Activator.CreateInstance(entityType) as BaseEntity;
                var dbFields = sampleEntity?.ImportableFields;

                if (dbFields == null || !dbFields.Any())
                {
                    MessageBox.Show("无法获取目标表的字段信息。");
                    return;
                }

                // 2. 显示加载状态
                btnAiMatch.Enabled = false;
                btnAiMatch.Text = "AI 分析中...";
                MainForm.Instance.PrintInfoLog("正在调用 AI 引擎进行语义分析...");

                // 3. 调用 AI 服务
                var aiService = new RUINORERP.Business.AIServices.DataImport.ColumnMappingService();
                var result = await aiService.AnalyzeWithMetadataAsync(excelHeaders, dbFields);

                // 4. 应用结果
                if (result.Mappings.Any())
                {
                    ApplyAiMappingResult(result, dbFields);
                    MainForm.Instance.PrintInfoLog($"AI 匹配完成：成功匹配 {result.Mappings.Count} 个字段。");
                    MessageBox.Show($"AI 智能匹配已完成！\n建议逻辑主键: {result.SuggestedLogicalKey ?? "无"}", "匹配成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("AI 未能找到合适的映射关系，请尝试手动配置。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AI 匹配失败: {ex.Message}");
                MainForm.Instance.PrintInfoLog($"AI 匹配异常: {ex.Message}");
            }
            finally
            {
                btnAiMatch.Enabled = true;
                btnAiMatch.Text = "🤖 AI 智能匹配";
            }
        }

        /// <summary>
        /// 将 AI 返回的结果应用到 UI 列表中
        /// </summary>
        private void ApplyAiMappingResult(RUINORERP.Business.AIServices.DataImport.IntelligentMappingResult result, List<RUINORERP.Model.ImportFieldInfo> dbFields)
        {
            kvList.Clear();
            listBoxExcelColumns.Items.Clear();
            listBoxDbColumns.Items.Clear();

            // 重新加载所有列
            foreach (DataGridViewColumn item in dvExcel.Columns)
            {
                listBoxExcelColumns.Items.Add(new SuperValue(item.Name, ""));
            }

            DataTable dt = MainForm.Instance.AppContext.Db.Ado.GetDataTable(
                "select COLUMN_NAME, DATA_TYPE from information_schema.columns where table_name = @tn",
                new { tn = ImportTargetTableName });

            var dbItems = new Dictionary<string, SuperValue>();
            foreach (DataRow dr in dt.Rows)
            {
                var sv = new SuperValue(dr["COLUMN_NAME"].ToString(), dr["DATA_TYPE"].ToString());
                dbItems[sv.superStrValue] = sv;
                listBoxDbColumns.Items.Add(sv);
            }

            // 应用 AI 匹配
            foreach (var mapping in result.Mappings)
            {
                string excelCol = mapping.Key;
                string targetField = mapping.Value.TargetField;

                var excelItem = listBoxExcelColumns.Items.Cast<SuperValue>().FirstOrDefault(e => e.superStrValue == excelCol);
                if (excelItem != null && dbItems.ContainsKey(targetField))
                {
                    var dbItem = dbItems[targetField];
                    var kvp = new SuperKeyValuePair(
                        new SuperValue(dbItem.superStrValue, dbItem.superDataTypeName),
                        new SuperValue(excelItem.superStrValue, "")
                    );

                    // 标记逻辑主键
                    if (targetField == result.SuggestedLogicalKey)
                    {
                        kvp.Tag = true;
                    }

                    kvList.Add(kvp);
                    listBoxExcelColumns.Items.Remove(excelItem);
                    listBoxDbColumns.Items.Remove(dbItem);
                }
            }

            LoadListTolistbox匹配结果(kvList);
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
