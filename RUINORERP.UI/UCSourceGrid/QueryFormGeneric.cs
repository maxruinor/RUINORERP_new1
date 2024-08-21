using Krypton.Toolkit;
using RUINORERP.Business.Processor;
using RUINORERP.Model;
using RUINORERP.UI.ProductEAV;
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

namespace RUINORERP.UI.UCSourceGrid
{
    public partial class QueryFormGeneric : KryptonForm
    {
        public QueryFormGeneric()
        {
            InitializeComponent();
            prodQuery = new UCProdQuery();
            prodQuery.Dock = DockStyle.Fill;
        }

        /*
        private long _LocationID = 0;

        /// <summary>
        /// 默认的仓库,由单据UI带过来
        /// </summary>
        public long LocationID { get => _LocationID; set => _LocationID = value; }
        /// <summary>
        /// 是否显示选择列，能多行选中。
        /// </summary>
        public bool MultipleChoices { get; set; } = false;

        private string _queryValue = string.Empty;

        private string _queryField = string.Empty;

        private List<View_ProdDetail> _queryObjects = new List<View_ProdDetail>();
        private View_ProdDetail _queryObject = new View_ProdDetail();

        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }
        //改版
        //返回值将单个值对象等，改为数组
        public string QueryValue { get => _queryValue; set => _queryValue = value; }

        /// <summary>
        ///  明细表格时传进来的查询字段条件
        /// </summary>
        public string QueryField { get => _queryField; set => _queryField = value; }



        /// <summary>
        /// 返回查询的对象
        /// </summary>
        public List<View_ProdDetail> QueryObjects { get => _queryObjects; set => _queryObjects = value; }

        /// <summary>
        /// 要返回的单个对象
        /// </summary>
        public View_ProdDetail QueryObject { get => _queryObject; set => _queryObject = value; }
        */
        public UCProdQuery prodQuery = null;
        private void QueryFormGeneric_Load(object sender, EventArgs e)
        {
            this.kryptonPanelQuery.Controls.Add(prodQuery);

            ////由查询这边传递查询的字段名
            //prodQuery.QueryField = QueryField;
            //prodQuery.LocationID = LocationID;

            ////返回值
            //QueryObjects = prodQuery.QueryObjects;
            //QueryObject = prodQuery.QueryObject;
            //QueryValue = prodQuery.QueryValue;
            //MultipleChoices = prodQuery.MultipleChoices;

        }
    }
}
