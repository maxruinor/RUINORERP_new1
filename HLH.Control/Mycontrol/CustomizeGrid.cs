using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace HLH.WinControl.Mycontrol
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
            this.Click += CustomizeGrid_Click;

        }

        void targetDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            if (targetDataGridView != null && targetDataGridView.DataSource != null)
            {

                SerializableDictionary<string, bool> qr = new SerializableDictionary<string, bool>();
                for (int i = 0; i < targetDataGridView.ColumnCount; i++)
                {
                    //排除已经存在的情况 
                    if (!qr.ContainsKey(targetDataGridView.Columns[i].Name))
                    {
                        qr.Add(targetDataGridView.Columns[i].Name, targetDataGridView.Columns[i].Visible);
                    }

                }

                ////取窗体名称+表格控制名为配置文件名
                //string datasourceName = targetDataGridView.DataSource.GetType().ToString();
                //int s = datasourceName.IndexOf("[");
                //string fileName = datasourceName.Substring(s + 1);
                //fileName = fileName.TrimEnd(']');
                //this.Tag = fileName + ".xml";


                qr = LoadColumnsList();
                SaveColumnsList(qr);
                for (int i = 0; i < targetDataGridView.ColumnCount; i++)
                {
                    if (qr != null && qr.ContainsKey(targetDataGridView.Columns[i].Name))
                    {
                        targetDataGridView.Columns[i].Visible = qr[targetDataGridView.Columns[i].Name];
                    }

                }

            }
        }

        #region 配置持久化

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

        private SerializableDictionary<string, bool> LoadColumnsList()
        {
            string PickConfigPath = string.Empty;
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

        void CustomizeGrid_Click(object sender, EventArgs e)
        {
            SetColumns();
        }


        private string xmlFileName = string.Empty;

        [Browsable(false)]
        public string XmlFileName
        {
            get
            {
                //取窗体名称+表格控制名为配置文件名  
                //2018-12-19 文件名 应该具体到窗体的  可以唯一指定到DataGridView的路径  ，如果用数据源。则在不同地方。控制到一样的。
                // 菜单-窗体-控件【n】-表格-数据实体
                Control ctl = new Control();
                ctl = targetDataGridView.Parent;
                string fileNameHeader = string.Empty;
                while (ctl != null)
                {
                    ctl = ctl.Parent;
                    if (ctl is Form)
                    {
                        fileNameHeader = ctl.ToString().Substring(0, ctl.ToString().IndexOf(","));
                        break;
                    }
                }

                string datasourceName = targetDataGridView.DataSource.GetType().ToString();
                int s = datasourceName.IndexOf("[");

                string fileName = datasourceName.Substring(s + 1);
                fileName = fileName.TrimEnd(']');
                // this.Tag = fileNameHeader + "." + fileName + ".xml";
                xmlFileName = fileNameHeader + "." + fileName + ".xml";
                return xmlFileName;
            }

        }



        public void SetColumns()
        {
            if (targetDataGridView != null && targetDataGridView.DataSource != null)
            {
                ForCustomizeGrid.frmColumnsSets set = new ForCustomizeGrid.frmColumnsSets();


                SerializableDictionary<string, bool> qr = new SerializableDictionary<string, bool>();


                qr = LoadColumnsList();

                if (qr.Count == 0 || qr.Count != targetDataGridView.ColumnCount)
                {
                    for (int i = 0; i < targetDataGridView.ColumnCount; i++)
                    {
                        if (!qr.ContainsKey(targetDataGridView.Columns[i].Name))
                        {
                            qr.Add(targetDataGridView.Columns[i].Name, targetDataGridView.Columns[i].Visible);
                        }
                    }
                }

                set.QueryResult = qr;

                if (set.ShowDialog() == DialogResult.OK)
                {
                    SaveColumnsList(set.QueryResult);
                    for (int i = 0; i < targetDataGridView.ColumnCount; i++)
                    {
                        if (set.QueryResult.ContainsKey(targetDataGridView.Columns[i].Name))
                        {
                            targetDataGridView.Columns[i].Visible = set.QueryResult[targetDataGridView.Columns[i].Name];
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("请先查询或设置目标表格及其数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private DataGridView _targetDataGridView = null;

        /// <summary>
        /// 关联的datagridview
        /// </summary>
        public DataGridView targetDataGridView
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
