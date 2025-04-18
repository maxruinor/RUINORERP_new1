﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace RUINORERP.UI.BaseForm
{
    public partial class UCBaseQuery : UserControl
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
        /// 系统设置为不可用，或程序中控制了不可见的列
        /// </summary>
        public HashSet<string> InvisibleCols { get; set; } = new HashSet<string>();


        /// <summary>
        /// 保存默认隐藏的列  
        /// HashSet比List性能更好
        /// 为了提高性能，特别是当 InvisibleCols 和 DefaultHideCols 列表较大时，可以使用 HashSet<string> 替代 List<string>。HashSet<string> 的查找性能更高（平均时间复杂度为 O(1)），而 List<string> 的查找性能为 O(n)。
        /// </summary>
        public HashSet<string> DefaultHideCols { get; set; } = new HashSet<string>();




        /// <summary>
        /// 关联单据的列,前面是引用单号列名，后面是 表名+原始单号列名
        /// 例如：如果在出库单中打开订单：则入订单类型，出库表中的引用订单的单号列名|订单自己的列名
        /// </summary>
        public ConcurrentDictionary<string, string> RelatedBillCols { get; set; } = new ConcurrentDictionary<string, string>();


        public UCBaseQuery()
        {
            InitializeComponent();
        }
    }
}
