// Copyright 2006 Herre Kuijpers - <herre@xs4all.nl>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;

namespace RUINOR.WinFormsUI.OutlookGrid
{
    #region DataSourceRowComparer - implementation of abstract comparer class
    /// <summary>
    /// because the DataSourceRow class is a wrapper class around the real data,
    /// the compared object used to sort the real data is wrapped by this DataSourceRowComparer class.
    /// </summary>
    internal class DataSourceRowComparer : IComparer
    {
        IComparer baseComparer;
        /// <summary>
        /// 需传入IComparer的构造函数
        /// </summary>
        /// <param name="baseComparer">比较接口</param>
        public DataSourceRowComparer(IComparer baseComparer)
        {
            this.baseComparer = baseComparer;
        }

        #region IComparer Members

        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x">DataSourceRow对象</param>
        /// <param name="y">DataSourceRow对象</param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            DataSourceRow r1 = (DataSourceRow)x;
            DataSourceRow r2 = (DataSourceRow)y;
            return baseComparer.Compare(r1.BoundItem, r2.BoundItem);
        }

        #endregion
    }
    #endregion DataSourceRowComparer - implementation of abstract comparer class

    #region DataSourceRow - abstract representation of a data item.
    /// <summary>
    /// The DataSourceRow is a wrapper row class around the real bound data. This row is an abstraction
    /// so different types of data can be encaptulated in this class, although for the OutlookGrid it will
    /// simply look as one type of data. 
    /// Note: this class does not implement all row wrappers optimally. It is merely used for demonstration purposes
    /// </summary>
    internal class DataSourceRow : CollectionBase
    {
        DataSourceManager manager;
        object boundItem;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manager">DataSourceManager对象</param>
        /// <param name="boundItem">绑定的数据项</param>
        public DataSourceRow(DataSourceManager manager, object boundItem)
        {
            this.manager = manager;
            this.boundItem = boundItem;
        }

        /// <summary>
        /// 通过索引获取值
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                return List[index];
            }
        }

        /// <summary>
        /// 绑定的数据对象
        /// </summary>
        public object BoundItem
        {
            get
            {
                return boundItem;
            }
        }

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        public int Add(object val)
        {
            if (val.GetType() == typeof(DateTime))
            {
                return List.Add(DateTime.Parse(val.ToString()).ToString("yyyy-M-d"));
            }
            return List.Add(val);
        }

    }
    #endregion DataSourceRow - abstract representation of a data item.

    #region DataSourceManager - manages a bound datasource.
    /// <summary>
    /// the DataDourceManager class is a wrapper class around different types of datasources.
    /// in this case the DataSet, object list using reflection and the OutlooGridRow objects are supported
    /// by this class. Basically the DataDourceManager works like a facade that provides access in a uniform
    /// way to the datasource.
    /// Note: this class is not implemented optimally. It is merely used for demonstration purposes
    /// </summary>
    internal class DataSourceManager
    {
        private object dataSource;
        private string dataMember;
        /// <summary>
        /// 列集合对象
        /// </summary>
        public ArrayList Columns;
        /// <summary>
        /// 行集合对象
        /// </summary>
        public ArrayList Rows;

        /// <summary>
        /// 数据源管理类
        /// </summary>
        /// <param name="dataSource">数据源对象</param>
        /// <param name="dataMember">DataSet中某一表名称</param>
        public DataSourceManager(object dataSource, string dataMember)
        {
            this.dataSource = dataSource;
            this.dataMember = dataMember;
            InitManager();
        }

        /// <summary>
        /// datamember readonly for now
        /// </summary>
        public string DataMember
        {
            get { return dataMember; }
        }

        /// <summary>
        /// datasource is readonly for now
        /// </summary>
        public object DataSource
        {
            get { return dataSource; }
        }

        /// <summary>
        /// this function initializes the DataSourceManager's internal state.
        /// it will analyse the datasource taking the following source into account:
        /// - DataSet
        /// - Object array (must implement IList)
        /// - OutlookGrid
        /// </summary>
        private void InitManager()
        {
            if (dataSource is IListSource || dataSource is DataTable || dataSource is DataView)
                InitDataSet();
            if (dataSource is IList)
                InitList();
            if (dataSource is OutlookGrid)
                InitGrid();
        }

        /// <summary>
        /// 初始化DataSet对象
        /// </summary>
        private void InitDataSet()
        {
            Columns = new ArrayList();
            Rows = new ArrayList();

            DataTable table = null;
            if (dataSource is DataTable)
            {
                table = dataSource as DataTable;
            }
            else if (dataSource is DataView)
            {
                table = (dataSource as DataView).ToTable();
            }
            else
            {
                table = ((DataSet)dataSource).Tables[this.dataMember];
            }
            // use reflection to discover all properties of the object
            foreach (DataColumn c in table.Columns)
                Columns.Add(c.ColumnName);

            foreach (DataRow r in table.Rows)
            {
                DataSourceRow row = new DataSourceRow(this, r);
                for (int i = 0; i < Columns.Count; i++)
                    row.Add(r[i]);
                Rows.Add(row);
            }
        }

        /// <summary>
        /// 初始化OutlookGrid对象
        /// </summary>
        private void InitGrid()
        {
            Columns = new ArrayList();
            Rows = new ArrayList();

            OutlookGrid grid = (OutlookGrid)dataSource;
            // use reflection to discover all properties of the object
            foreach (DataGridViewColumn c in grid.Columns)
                Columns.Add(c.Name);

            foreach (OutlookGridRow r in grid.Rows)
            {
                if (!r.IsGroupRow && !r.IsNewRow)
                {
                    DataSourceRow row = new DataSourceRow(this, r);
                    for (int i = 0; i < Columns.Count; i++)
                        row.Add(r.Cells[i].Value);
                    Rows.Add(row);
                }
            }
        }

        /// <summary>
        /// 初始化List对象
        /// </summary>
        private void InitList()
        {
            Columns = new ArrayList();
            Rows = new ArrayList();
            IList list = (IList)dataSource;

            // use reflection to discover all properties of the object
            BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty;
            PropertyInfo[] props = list[0].GetType().GetProperties();
            foreach (PropertyInfo pi in props)
                    Columns.Add(pi.Name);

            foreach (object obj in list)
            {
                DataSourceRow row = new DataSourceRow(this, obj);
                foreach (PropertyInfo pi in props)
                {
                    object result = obj.GetType().InvokeMember(pi.Name, bf, null, obj, null);
                    row.Add(result);  
                }
                Rows.Add(row);
            }
        }

        /// <summary>
        /// DataSourceRow排序方法
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(System.Collections.IComparer comparer)
        {
            Rows.Sort(new DataSourceRowComparer(comparer));
        }

    }
    #endregion DataSourceManager - manages a bound datasource.
}
