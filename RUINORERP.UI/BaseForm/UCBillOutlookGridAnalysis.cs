using RUINORERP.Common.Helper;
using RUINORERP.UI.UControls.Outlook;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using RUINORERP.Common.Extensions;

using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;
using Krypton.Toolkit.Suite.Extended.Outlook.Grid;
using Token = Krypton.Toolkit.Suite.Extended.Outlook.Grid.Token;
using RUINORERP.Model;
using FastReport.DevComponents.DotNetBar.Controls;


namespace RUINORERP.UI.BaseForm
{
    public partial class UCBillOutlookGridAnalysis : UCBaseQuery
    {
        public UCBillOutlookGridAnalysis()
        {
            InitializeComponent();
             GridRelated = new GridViewRelated();
        }

        /// <summary>
        /// 保存列控制信息的列表 ，这个值设计时不生成
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public List<ColDisplayController> ColumnDisplays { get; set; } = new List<ColDisplayController>();



        private ConcurrentDictionary<string, KeyValuePair<string, bool>> _FieldNameList = new ConcurrentDictionary<string, KeyValuePair<string, bool>>();


        /// <summary>
        /// 列的显示，unitName,<单位,true>
        /// 列名，列中文，是否显示
        /// </summary>
        public ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList { get => _FieldNameList; set => _FieldNameList = value; }



        public delegate void LoadDataDelegate(object rowObj);
        /// <summary>
        /// 加载数据
        /// </summary>
        public event LoadDataDelegate OnLoadData;


        private void UCBillOutlookGridAnalysis_Load(object sender, EventArgs e)
        {
            try
            {

                //OutlookGridGeneralStrings generalStrings=new OutlookGridGeneralStrings();
                //generalStrings.ClearGrouping = "清除分組";

                kryptonOutlookGrid1.AllowDrop = true;
                kryptonOutlookGrid1.AllowUserToAddRows = false;
                kryptonOutlookGrid1.GroupBox = kryptonOutlookGridGroupBox1;
                //kryptonOutlookGridLanguageManager1.OutlookGridGeneralStrings = generalStrings;
                kryptonOutlookGrid1.SubtotalColumns = SummaryCols;
                kryptonOutlookGrid1.RegisterGroupBoxEvents();


                kryptonOutlookGrid1.ShowLines = true;
                //LoadData();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog("结果分析时加载数据出错。" + ex.Message);

            }

        }

        private List<KeyValuePair<string, string>> _subtotalColumns;

        ///// <summary>
        ///// 小计列<colName,元>
        ////  列名，显示单位
        ///// </summary>
        public List<KeyValuePair<string, string>> SubtotalColumns { get; set; } = new List<KeyValuePair<string, string>>();




        public void LoadDataToGrid<T>(List<T> list)
        {
            try
            {
                DataGridViewSetup setup = new DataGridViewSetup();
                setup.FieldNameList = FieldNameList;

                //如果显示列没有指定，就默认全部显示
                if (ColumnDisplays == null || ColumnDisplays.Count == 0)
                {
                    List<ColDisplayController> columnDisplayControllers = new List<ColDisplayController>();
                    foreach (var item in setup.FieldNameList)
                    {
                        //ConcurrentDictionary<string, KeyValuePair<string, bool>>
                        ColDisplayController cdc = ColumnDisplays.Where(s => s.ColName == item.Key).FirstOrDefault();
                        if (cdc != null)
                        {
                            cdc.ColDisplayText = item.Value.Key;
                            //因为使用了开源的UI框架 krypton 在关闭时还会执行ColumnDisplayIndexChanged，将顺序打乱了。所以这里有一些问题
                            //所以通过添加一个属性和一个方法来判断，并且拖放时立即保存到List中。保存时就不从dg中取显示顺序了
                            //cdc.ColDisplayIndex = dc.DisplayIndex; 这个特别处理
                            cdc.ColName = item.Key;
                            cdc.Visible = true;
                            cdc.DataPropertyName = item.Key;
                            columnDisplayControllers.Add(cdc);
                        }
                    }
                    setup.ColumnDisplays = columnDisplayControllers;
                }
                else
                {
                    setup.ColumnDisplays = ColumnDisplays;
                }



                setup.SetupDataGridView<T>(this.kryptonOutlookGrid1, true);
                //LoadData();
                LoadDataNew(list);
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog("结果分析时加载数据出错。" + ex.Message);

            }
        }



        /*
        private void LoadData()
        {
            //Setup Rows
            OutlookGridRow row = new OutlookGridRow();
            List<OutlookGridRow> l = new List<OutlookGridRow>();
            OutlookGrid1.SuspendLayout();
            OutlookGrid1.ClearInternalRows();
            OutlookGrid1.FillMode = FillMode.GroupsAndNodes;

            List<Token> tokensList = new List<Token>();
            tokensList.Add(new Token("Best seller", Color.Orange, Color.Black));
            tokensList.Add(new Token("New", Color.LightGreen, Color.Black));
            tokensList.Add(null);
            tokensList.Add(null);
            tokensList.Add(null);

            Random random = new Random();
            //.Next permet de retourner un nombre aléatoire contenu dans la plage spécifiée entre parenthèses.
            XmlDocument doc = new XmlDocument();
            doc.Load("invoices.xml");
            IFormatProvider culture = new CultureInfo("en-US", true);
            foreach (XmlNode customer in doc.SelectNodes("//invoice")) //TODO for instead foreach for perfs...
            {
                try
                {
                    row = new OutlookGridRow();
                    row.CreateCells(OutlookGrid1, new object[] {
                        customer["CustomerID"].InnerText,
                        customer["CustomerName"].InnerText,
                        customer["Address"].InnerText,
                        customer["City"].InnerText,
                        new TextAndImage(customer["Country"].InnerText,GetFlag(customer["Country"].InnerText)),
                        DateTime.Parse(customer["OrderDate"].InnerText,culture),
                        customer["ProductName"].InnerText,
                        double.Parse(customer["Price"].InnerText, CultureInfo.InvariantCulture), //We put a float the formatting in design does the rest
                        (double)random.Next(101) /100,
                        tokensList[random.Next(5)]
                    });
                    if (random.Next(2) == 1)
                    {
                        //Sub row
                        OutlookGridRow row2 = new OutlookGridRow();
                        row2.CreateCells(OutlookGrid1, new object[] {
                            customer["CustomerID"].InnerText + " 2",
                            customer["CustomerName"].InnerText + " 2",
                            customer["Address"].InnerText + "2",
                            customer["City"].InnerText + " 2",
                            new TextAndImage(customer["Country"].InnerText,GetFlag(customer["Country"].InnerText)),
                            DateTime.Now,
                            customer["ProductName"].InnerText + " 2",
                            (double)random.Next(1000),
                            (double)random.Next(101) /100,
                            tokensList[random.Next(5)]
                        });
                        row.Nodes.Add(row2);
                        ((KryptonDataGridViewTreeTextCell)row2.Cells[1]).UpdateStyle(); //Important : after added to the parent node
                    }
                    l.Add(row);
                    ((KryptonDataGridViewTreeTextCell)row.Cells[1]).UpdateStyle(); //Important : after added to the rows list
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gasp...Something went wrong ! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



            OutlookGrid1.ResumeLayout();
            OutlookGrid1.AssignRows(l);
            OutlookGrid1.ForceRefreshGroupBox();
            OutlookGrid1.Fill();
        }
        */

        private void LoadData()
        {
            //Setup Rows
            OutlookGridRow row = new OutlookGridRow();
            List<OutlookGridRow> l = new List<OutlookGridRow>();
            kryptonOutlookGrid1.SuspendLayout();
            kryptonOutlookGrid1.ClearInternalRows();
            kryptonOutlookGrid1.FillMode = FillMode.GroupsAndNodes;

            List<Token> tokensList = new List<Token>();
            tokensList.Add(new Token("Best seller", Color.Orange, Color.Black));
            tokensList.Add(new Token("New", Color.LightGreen, Color.Black));
            tokensList.Add(null);
            tokensList.Add(null);
            tokensList.Add(null);

            Random random = new Random();
            //.Next permet de retourner un nombre aléatoire contenu dans la plage spécifiée entre parenthèses.
            XmlDocument doc = new XmlDocument();
            doc.Load("invoices.xml");
            IFormatProvider culture = new CultureInfo("en-US", true);
            foreach (XmlNode customer in doc.SelectNodes("//invoice")) //TODO for instead foreach for perfs...
            {
                try
                {
                    row = new OutlookGridRow();
                    row.CreateCells(kryptonOutlookGrid1, new object[] {
                        customer["CustomerID"].InnerText,
                        customer["CustomerName"].InnerText,
                        customer["Address"].InnerText,
                        customer["City"].InnerText,
                        DateTime.Parse(customer["OrderDate"].InnerText,culture),
                        customer["ProductName"].InnerText,
                        double.Parse(customer["Price"].InnerText, CultureInfo.InvariantCulture), //We put a float the formatting in design does the rest
                        (double)random.Next(101) /100,
                        tokensList[random.Next(5)]
                    });
                    if (random.Next(2) == 1)
                    {
                        //Sub row
                        OutlookGridRow row2 = new OutlookGridRow();
                        row2.CreateCells(kryptonOutlookGrid1, new object[] {
                            customer["CustomerID"].InnerText + " 2",
                            customer["CustomerName"].InnerText + " 2",
                            customer["Address"].InnerText + "2",
                            customer["City"].InnerText + " 2",
                            DateTime.Now,
                            customer["ProductName"].InnerText + " 2",
                            (double)random.Next(1000),
                            (double)random.Next(101) /100,
                            tokensList[random.Next(5)]
                        });
                        row.Nodes.Add(row2);
                        ((KryptonDataGridViewTreeTextCell)row2.Cells[1]).UpdateStyle(); //Important : after added to the parent node
                    }
                    l.Add(row);
                    ((KryptonDataGridViewTreeTextCell)row.Cells[1]).UpdateStyle(); //Important : after added to the rows list
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gasp...Something went wrong ! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



            kryptonOutlookGrid1.ResumeLayout();
            kryptonOutlookGrid1.AssignRows(l);
            kryptonOutlookGrid1.ForceRefreshGroupBox();
            kryptonOutlookGrid1.Fill();
        }

        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// </summary>
        public Type entityType { get; set; }

        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// 视图可能来自多个表的内容，所以显示不一样
        /// </summary>
        public List<Type> ColDisplayTypes { get; set; } = new List<Type>();


        /// <summary>
        /// 加载数据
        /// </summary>                   
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void LoadDataNew<T>(List<T> list)
        {
            //Setup Rows
            OutlookGridRow row = new OutlookGridRow();
            List<OutlookGridRow> l = new List<OutlookGridRow>();
            kryptonOutlookGrid1.SuspendLayout();
            kryptonOutlookGrid1.ClearInternalRows();
            kryptonOutlookGrid1.FillMode = FillMode.GroupsAndNodes;
            //IFormatProvider culture = new CultureInfo("en-CN", true);
            List<Token> tokensList = new List<Token>();
            tokensList.Add(new Token("Best seller", Color.Orange, Color.Black));
            tokensList.Add(new Token("New", Color.LightGreen, Color.Black));
            tokensList.Add(null);
            tokensList.Add(null);
            tokensList.Add(null);

            foreach (var item in list)
            {
                row = new OutlookGridRow();
                //保存了這個數據對象
                row.Tag = item;

                object[] values = new object[kryptonOutlookGrid1.ColumnCount];
                for (int i = 0; i < kryptonOutlookGrid1.Columns.Count; i++)
                {

                    object value = item.GetPropertyValue(kryptonOutlookGrid1.Columns[i].Name);
                    if (value != null)
                    {
                        if (kryptonOutlookGrid1.Columns[i].Name == "数量" && value.ToString() == "-1")
                        {

                        }
                        if (value.GetType().Name == "DateTime")
                        {
                            values[i] = value;
                        }
                        else
                        {
                            //取关联值来显示
                            //string displayText = UIHelper.GetDisplayText(kryptonOutlookGrid1.Columns[i].Name, value);
                            string displayText = UIHelper.GetDisplayText(ColNameDataDictionary, kryptonOutlookGrid1.Columns[i].Name, value, ColDisplayTypes, entityType).ToString();
                            if (!string.IsNullOrEmpty(displayText))
                            {
                                values[i] = displayText;
                            }
                            else
                            {
                                values[i] = value;
                            }
                        }


                    }
                    else
                    {

                    }

                }

                row.CreateCells(kryptonOutlookGrid1, values);



                //row.CreateCells(OutlookGrid1,item.to)
                //row.CreateCells(OutlookGrid1, new object[] {
                //        customer["CustomerID"].InnerText,
                //        customer["CustomerName"].InnerText,
                //        customer["Address"].InnerText,
                //        customer["City"].InnerText,
                //        DateTime.Parse(customer["OrderDate"].InnerText,culture),
                //        customer["ProductName"].InnerText,
                //        double.Parse(customer["Price"].InnerText, CultureInfo.InvariantCulture), //We put a float the formatting in design does the rest
                //        (double)random.Next(101) /100,
                //        tokensList[random.Next(5)]
                //    });
                l.Add(row);
                //((KryptonDataGridViewTreeTextCell)row.Cells[1]).UpdateStyle(); //Important : after added to the rows list
            }
            /*
            Random random = new Random();
            //.Next permet de retourner un nombre aléatoire contenu dans la plage spécifiée entre parenthèses.
            XmlDocument doc = new XmlDocument();
            doc.Load("invoices.xml");
            IFormatProvider culture = new CultureInfo("en-US", true);
            foreach (XmlNode customer in doc.SelectNodes("//invoice")) //TODO for instead foreach for perfs...
            {
                try
                {
                    row = new OutlookGridRow();
                    row.CreateCells(OutlookGrid1, new object[] {
                        customer["CustomerID"].InnerText,
                        customer["CustomerName"].InnerText,
                        customer["Address"].InnerText,
                        customer["City"].InnerText,
                        DateTime.Parse(customer["OrderDate"].InnerText,culture),
                        customer["ProductName"].InnerText,
                        double.Parse(customer["Price"].InnerText, CultureInfo.InvariantCulture), //We put a float the formatting in design does the rest
                        (double)random.Next(101) /100,
                        tokensList[random.Next(5)]
                    });
                    if (random.Next(2) == 1)
                    {
                        //Sub row
                        OutlookGridRow row2 = new OutlookGridRow();
                        row2.CreateCells(OutlookGrid1, new object[] {
                            customer["CustomerID"].InnerText + " 2",
                            customer["CustomerName"].InnerText + " 2",
                            customer["Address"].InnerText + "2",
                            customer["City"].InnerText + " 2",
                            DateTime.Now,
                            customer["ProductName"].InnerText + " 2",
                            (double)random.Next(1000),
                            (double)random.Next(101) /100,
                            tokensList[random.Next(5)]
                        });
                        row.Nodes.Add(row2);
                        ((KryptonDataGridViewTreeTextCell)row2.Cells[1]).UpdateStyle(); //Important : after added to the parent node
                    }
                    l.Add(row);
                    ((KryptonDataGridViewTreeTextCell)row.Cells[1]).UpdateStyle(); //Important : after added to the rows list
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gasp...Something went wrong ! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            */

            kryptonOutlookGrid1.ResumeLayout();
            kryptonOutlookGrid1.AssignRows(l);
            kryptonOutlookGrid1.ForceRefreshGroupBox();
            kryptonOutlookGrid1.Fill();
        }


        private void OutlookGrid1_Resize(object sender, EventArgs e)
        {
            int PreferredTotalWidth = 1;
            //Calculate the total preferred width
            foreach (DataGridViewColumn c in kryptonOutlookGrid1.Columns)
            {
                try
                {
                    PreferredTotalWidth += Math.Min(c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true), 250);
                }
                catch (Exception ex)
                {

                }

            }

            if (kryptonOutlookGrid1.Width > PreferredTotalWidth)
            {
                kryptonOutlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                kryptonOutlookGrid1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            else
            {
                kryptonOutlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                foreach (DataGridViewColumn c in kryptonOutlookGrid1.Columns)
                {
                    c.Width = Math.Min(c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true), 250);
                }
            }
        }

        private void OutlookGrid1_GroupImageClick(object sender, OutlookGridGroupImageEventArgs e)
        {
            MessageBox.Show("Group Image clicked for group row : " + e.Row.Group.Text);
        }

        private void buttonSpecHeaderGroup1_Click(object sender, EventArgs e)
        {
            // DataGridViewSetup setup = new DataGridViewSetup();
            //setup.SetupDataGridView<T>(this.OutlookGrid1, true);
            //LoadData();

        }

        private void buttonSpecHeaderGroup2_Click(object sender, EventArgs e)
        {
            kryptonOutlookGrid1.PersistConfiguration(Application.StartupPath + "/grid.xml", StaticInfos._GRIDCONFIG_VERSION.ToString());
        }

        bool expand = true;

        private void buttonSpecHeaderGroup3_Click(object sender, EventArgs e)
        {
            if (expand)
                kryptonOutlookGrid1.ExpandAllNodes();
            else
                kryptonOutlookGrid1.CollapseAllNodes();

            expand = !expand;
        }

        public GridViewRelated GridRelated { get; set; } 


        private void kryptonOutlookGrid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                return;
            }
            if (kryptonOutlookGrid1.CurrentRow != null && kryptonOutlookGrid1.CurrentCell != null)
            {
                if (kryptonOutlookGrid1.CurrentRow.Tag is BaseEntity entity)
                {
                    //特殊情况处理 当前行的业务类型：销售出库  库存盘点 对应一个集合，再用原来的方法来处理
                    GridRelated.GuideToForm(kryptonOutlookGrid1.Columns[e.ColumnIndex].Name, entity);
                }
            }
        }
        private void kryptonOutlookGrid1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (kryptonOutlookGrid1.RowCount > 0)
            //{
            //    kryptonOutlookGrid1.TopLeftHeaderCell.Value = (kryptonOutlookGrid1.RowCount - 1).ToString("#");
            //}
        }
    }
}
