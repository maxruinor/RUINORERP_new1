using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Concurrent;
using System.Linq;

namespace RUINORERP.UI.UControls
{

    //https://my.oschina.net/Tsybius2014/blog/483903



    /// <summary>
    /// 自定义表格的列
    /// </summary>
    [DesignTimeVisible(true)]
    // [Designer(typeof(CSFrameworkComponentDesigner), typeof(IDesigner))]
    public partial class CustomizeGrid : Button
    {

        public CustomizeGrid()
        {
            InitializeComponent();
            //this.Click += CustomizeGrid_Click;

        }


        [Browsable(true)]
        [Description("是否使用内置自定义列显示功能")]
        public bool UseCustomColumnDisplay { get; set; } = true;
        void targetDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            //targetDataGridView.DataSource
            //这里 数据源变化，还没有来得及执行 控制列头变中文。这里要想办法
            //要不 增加回调，要不把控制的列 用"列名|显示中文" 用包括的方式处理。修改列控制from
            if (sender is NewSumDataGridView nsdg)
            {
                this.FieldNameList = nsdg.FieldNameList;
                ColumnDisplays = nsdg.ColumnDisplays;
            }

        }

        /// <summary>
        /// 保存列控制信息的列表
        /// </summary>
        public List<ColDisplayController> ColumnDisplays { get; set; } = new List<ColDisplayController>();



        #region 配置持久化


        /// <summary>
        /// 持久化只能这个格式，所以思路 只保存name->enname bool
        /// </summary>
        /// <param name="qs"></param>
        private void SaveColumnsList(SerializableDictionary<string, bool> qs)
        {
            string rs = string.Empty;
            rs = Serializer(typeof(SerializableDictionary<string, bool>), qs);

            string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\ColumnsConfig", this.XmlFileName.ToString());

            System.IO.FileInfo fi = new FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            if (!System.IO.File.Exists(PathwithFileName))
            {
                System.IO.FileStream f = System.IO.File.Open(PathwithFileName, FileMode.CreateNew, FileAccess.ReadWrite);
                f.Close();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(PathwithFileName, false);
            f2.Write(rs);
            f2.Close();
            f2.Dispose();
        }


        /// <summary>
        /// 将DG的显示顺序等属性保存到list中
        /// </summary>
        /// <param name="ColumnDisplays"></param>
        public List<ColDisplayController> SaveDisplayIndex(List<ColDisplayController> ColumnDisplays)
        {

            foreach (DataGridViewColumn dc in targetDataGridView.Columns)
            {
                ColDisplayController cdc = ColumnDisplays.Where(s => s.ColName == dc.Name).FirstOrDefault();
                if (cdc != null)
                {
                    cdc.ColDisplayText = dc.HeaderText;
                    cdc.ColDisplayIndex = dc.DisplayIndex;
                    cdc.ColWidth = dc.Width;
                    cdc.ColEncryptedName = dc.Name;
                    cdc.ColName = dc.Name;
                    cdc.IsFixed = dc.Frozen;
                    cdc.Visible = dc.Visible;
                    cdc.DataPropertyName = dc.DataPropertyName;
                }
            }
            return ColumnDisplays;
            //var query = from DataGridViewColumn col in Columns
            //            orderby col.DisplayIndex
            //            select col;

        }

        /// <summary>
        /// 是否保存列自定义设置
        /// </summary>
        public bool NeedSaveColumnsXml { get; set; } = false;


        /// <summary>
        /// 持久化只能这个格式，所以思路 只保存name->enname bool
        /// </summary>
        /// <param name="qs"></param>
        public void SaveColumnsList(List<ColDisplayController> columnDisplays)
        {
            if (!NeedSaveColumnsXml)
            {
                return;
            }
            if (this.XmlFileNamecdc == null)
            {
                this.XmlFileNamecdc = "defaultColfilecdc_1.xml";
            }
            string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\ColumnsConfig", this.XmlFileNamecdc.ToString());
            System.IO.FileInfo fi = new FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            manager.serialize_to_xml(PathwithFileName, columnDisplays);
        }


        RUINORERP.Common.Helper.XmlHelper manager = new RUINORERP.Common.Helper.XmlHelper();
        public List<ColDisplayController> LoadColumnsListByCdc()
        {
            List<ColDisplayController> ColumnDisplays = new List<ColDisplayController>();

            string PickConfigPath = string.Empty;
            try
            {
                if (this.XmlFileNamecdc == null)
                {
                    this.XmlFileNamecdc = "defaultColfilecdc.xml";
                }
                string filepath = System.IO.Path.Combine(Application.StartupPath + "\\ColumnsConfig", this.XmlFileNamecdc.ToString());
                //判断目录是否存在
                if (!System.IO.Directory.Exists(Application.StartupPath + "\\ColumnsConfig"))
                {
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "\\ColumnsConfig");
                }
                string s = "";
                if (System.IO.File.Exists(filepath))
                {

                    if (!System.IO.File.Exists(filepath))
                        s = "不存在相应的目录";
                    else
                    {
                        ColumnDisplays = manager.deserialize_from_xml(filepath, typeof(List<ColDisplayController>)) as List<ColDisplayController>;
                    }
                }

            }
            catch (Exception ex)
            {


            }
            if (ColumnDisplays == null)
            {
                ColumnDisplays = new List<ColDisplayController>();
            }
            if (NeedSaveColumnsXml)
            {
                //如果没有值，则加载默认的全部？
                if (ColumnDisplays.Count == 0)
                {
                    foreach (DataGridViewColumn dc in targetDataGridView.Columns)
                    {
                        ColDisplayController cdc = new ColDisplayController();
                        cdc.ColDisplayText = dc.HeaderText;
                        cdc.ColDisplayIndex = dc.DisplayIndex;
                        cdc.ColWidth = dc.Width;
                        cdc.ColEncryptedName = dc.Name;
                        cdc.ColName = dc.Name;
                        cdc.IsFixed = dc.Frozen;
                        cdc.Visible = dc.Visible;
                        cdc.DataPropertyName = dc.DataPropertyName;
                        ColumnDisplays.Add(cdc);
                    }
                }
            }

            return ColumnDisplays;
        }

        private SerializableDictionary<string, bool> LoadColumnsList()
        {
            string PickConfigPath = string.Empty;
            if (this.XmlFileName == null)
            {
                this.XmlFileName = "defaultColfile.xml";
            }
            string filepath = System.IO.Path.Combine(Application.StartupPath + "\\ColumnsConfig", this.XmlFileName.ToString());
            //判断目录是否存在
            if (!System.IO.Directory.Exists(Application.StartupPath + "\\ColumnsConfig"))
            {
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\ColumnsConfig");
            }
            string s = "";
            if (System.IO.File.Exists(filepath))
            {

                if (!System.IO.File.Exists(filepath))
                    s = "不存在相应的目录";
                else
                {
                    StreamReader f2 = new StreamReader(filepath);
                    s = f2.ReadToEnd();
                    f2.Close();
                    f2.Dispose();
                }
            }
            SerializableDictionary<string, bool> qs = new SerializableDictionary<string, bool>();
            qs = Deserialize(typeof(SerializableDictionary<string, bool>), s) as SerializableDictionary<string, bool>;
            if (qs == null)
            {
                qs = new SerializableDictionary<string, bool>();
            }
            return qs;
        }

        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion

        #endregion

        /// <summary>
        /// 应用于复杂的列控制文件
        /// </summary>
        public string XmlFileNamecdc { get; set; }

        private string xmlFileName = string.Empty;

        [Browsable(false)]
        public string XmlFileName
        {
            get
            {
                return xmlFileName;
            }
            set
            {
                xmlFileName = value;
                XmlFileNamecdc = xmlFileName + "_cdcfile";
            }
        }

        private ConcurrentDictionary<string, KeyValuePair<string, bool>> _FieldNameList = new ConcurrentDictionary<string, KeyValuePair<string, bool>>();
        /// <summary>
        /// 列的显示，unitName,<单位,true>
        /// 列名，列中文，是否显示
        /// </summary>
        public ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList { get => _FieldNameList; set => _FieldNameList = value; }



        /// <summary>
        /// 设置列
        /// </summary>
        public void SetColumns()
        {

            if (targetDataGridView != null && targetDataGridView.DataSource != null)
            {
                //为了显示传入带中文的集合
                ForCustomizeGrid.frmColumnsSets set = new ForCustomizeGrid.frmColumnsSets();

                var cols = from ColDisplayController col in ColumnDisplays
                           orderby col.ColDisplayIndex
                           select col;

                set.ColumnDisplays = cols.ToList();

                //SerializableDictionary<string, bool> qr = new SerializableDictionary<string, bool>();
                //qr = LoadColumnsList();
                ////如果加载默认的 为空时，则默认有中文的全选中显示
                //if (qr == null || qr.Count == 0)
                //{
                //    foreach (var item in FieldNameList)
                //    {
                //        if (targetDataGridView.Columns.Contains(item.Key))
                //        {
                //            qr.Add(item.Key, targetDataGridView.Columns[item.Key].Visible);
                //        }
                //    }
                //}
                //将上次保存的带到UI上 如勾选状态
                // set.QueryResult = qr;
                if (set.ShowDialog() == DialogResult.OK)
                {
                    targetDataGridView.BindColumnStyle();
                    SaveColumnsList(ColumnDisplays);

                    //qr = set.QueryResult;
                    //ShowColumns(qr);
                    //SaveColumnsList(ColumnDisplays);
                    /*
                    foreach (var item in FieldNameList)
                    {
                        if (set.QueryResult.ContainsKey(item.Key) && targetDataGridView.Columns.Contains(item.Key))
                        {
                            KeyValuePair<string, bool> kv;
                            if (FieldNameList.TryGetValue(targetDataGridView.Columns[item.Key].Name, out kv))
                            {
                                targetDataGridView.Columns[item.Key].HeaderText = kv.Key;
                                targetDataGridView.Columns[item.Key].Visible = set.QueryResult[item.Key];
                            }
                            else
                            {
                                targetDataGridView.Columns[item.Key].Visible = false;
                            }
                        }
                    }
                    */
                }
            }
            else
            {
                MessageBox.Show("请先查询或设置目标表格及其数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void ShowColumns(SerializableDictionary<string, bool> qr)
        {
            for (int i = 0; i < targetDataGridView.ColumnCount; i++)
            {
                KeyValuePair<string, bool> kv;
                if (qr.ContainsKey(targetDataGridView.Columns[i].Name))
                {
                    if (FieldNameList.TryGetValue(targetDataGridView.Columns[i].Name, out kv))
                    {
                        targetDataGridView.Columns[i].HeaderText = kv.Key;
                        //如果没有描述文字将不显示
                        if (kv.Key.Trim().Length == 0)
                        {
                            targetDataGridView.Columns[i].Visible = false;
                        }
                        else
                        {
                            targetDataGridView.Columns[i].Visible = qr[targetDataGridView.Columns[i].Name];
                        }

                    }
                    else
                    {
                        targetDataGridView.Columns[i].Visible = false;
                    }
                }
                else
                {
                    //初始为显示
                    if (FieldNameList.TryGetValue(targetDataGridView.Columns[i].Name, out kv))
                    {
                        targetDataGridView.Columns[i].HeaderText = kv.Key;
                        if (kv.Key.Trim().Length > 0 && kv.Value)
                        {
                            targetDataGridView.Columns[i].Visible = true;
                        }
                        else
                        {
                            targetDataGridView.Columns[i].Visible = false;
                        }
                        if (!qr.ContainsKey(kv.Key))
                        {
                            qr.Add(kv.Key, kv.Value);
                        }

                    }
                    else
                    {
                        targetDataGridView.Columns[i].Visible = false;
                    }

                }
            }
        }

        //    [DefaultProperty("BtnName")]
        //几个特性（Attribute）
        //1）DefaultEvent和DefaultProperty：指定自定义控件的默认事件和默认属性
        //2）Browsable：设置控件某一属性或事件是否出现在“属性”窗口中
        //BrowsableAttribute
        //3）Description：指定控件某一属性或事件出现在“属性”窗口中的说明文字
        //4）EditorBrowsable：指定某一属性或方法在编辑器中可见
        //5）DesignerSerializationVisibility：代码生成器生成组件相关代码的方式

        private NewSumDataGridView _targetDataGridView = null;

        /// <summary>
        /// 关联的datagridview
        /// </summary>
        public NewSumDataGridView targetDataGridView
        {
            get { return _targetDataGridView; }
            set
            {
                _targetDataGridView = value;
                if (_targetDataGridView != null)
                {
                    _targetDataGridView.DataSourceChanged += targetDataGridView_DataSourceChanged;
                }

            }
        }



        /// <summary>
        /// 事件
        /// </summary>
        [Browsable(true)]
        [Description("测试事件，不用处理")]
        public event EventHandler BtnTestClick;
        /// <summary>
        /// 测试按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            if (targetDataGridView == null)
            {
                MessageBox.Show("请设置关联的表格。");
            }

            if (BtnTestClick != null)
            {
                //TODO
                BtnTestClick(sender, e);
            }
        }

        private String _displayText = "本控制功能为设置指定表格的列。";
        [Browsable(true)]
        [DefaultValue("本控制功能为设置指定表格的列")]
        public String DisplayText
        {
            get
            {
                return _displayText;
            }
            set
            {
                _displayText = value;
                Invalidate();
            }
        }


        //  private static string myUCName = "自定义表格";
        [DefaultValueAttribute("自定义表格列")]
        [Description("请取个名称")]
        public override string Text
        {
            get
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                {
                    return "自定义表格列";
                }
                else
                {
                    return "自定义表格列";
                }

            }
            //set
            //{
            //    base.Text = value;
            //}
        }


        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public string ButtonName { get; set; }

    }
}
