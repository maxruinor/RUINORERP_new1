using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
    /*

    /// <summary>
    /// 从指定实体中搜索出结果展现并给出目标字段结果
    /// 比方产品详情查询出来展现一些字段，并且要给 详情ID给值，一般单据的内部就是产品ID值
    /// </summary>
    /// <typeparam name="P"></typeparam>

    public class DependencyQuery
    {
        private List<object> sourceList = new List<object>();
        private object Result;
        private Type _DependencyType;
        public DependencyQuery()
        {

        }

        /// <summary>
        /// 设置产品的相关列,包括了目标列
        /// </summary>
        /// <typeparam name="S">产品共享部分</typeparam>
        public List<DependColumn> SetDependencys<S>()
        {
            List<DependColumn> m_relatedColumns = new List<DependColumn>();
            foreach (PropertyInfo field in typeof(S).GetProperties())
            {
                DependColumn dcol = new DependColumn();
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is FKRelationAttribute)
                    {
                        dcol.IsFKRelationColumn = true;
                    }
                    if (attr is SugarColumn)
                    {
                        SugarColumn entityAttr = attr as SugarColumn;
                        if (null != entityAttr)
                        {
                            if (entityAttr.ColumnDescription == null)
                            {
                                continue;
                            }
                            if (entityAttr.IsIdentity)
                            {
                                continue;
                            }
                            if (entityAttr.IsPrimaryKey)
                            {
                                continue;
                            }
                            if (entityAttr.ColumnDescription.Trim().Length > 0)
                            {

                                dcol.ColName = field.Name;
                                dcol.ColCaption = entityAttr.ColumnDescription;
                            }

                        }
                    }
                }
                if (!string.IsNullOrEmpty(dcol.ColName))
                {
                    m_relatedColumns.Add(dcol);
                }

            }

            //要把目标列也加入到这个部分。到时只循环这个组合中等于目标列名的时候改值
            //DependColumn dcTag = new DependColumn();
            //dcTag.ValueObjType = typeof(Model.<tb_Prod>);
            //dcTag.ColName = TargetCol.TargetColumnName;
            //dcTag.ColCaption = TargetCol.TargetCaption;
            //dcTag.Visible = false;//目标列隐藏
            // m_relatedColumns.Add(dcTag);
            return m_relatedColumns;
        }


        //public object GetValue<T>(List<T> product, Expression<Func<T, int>> expkey)
        //{
        //    //Lazy<>
        //    //if (CellValue == null)
        //    //{
        //    //    return null;
        //    //}
        //    RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(Result, "");
        //    var mb = expkey.GetMemberInfo();
        //    string key = mb.Name;
        //    string tableName = expkey.Parameters[0].Type.Name;
        //    // key = tableName + ":" + key + ":" + CellValue.ToString();
        //    //Dict.TryGetValue(key, out value);
        //    //if (Manager.Cache.Exists(key))
        //    //{
        //    //    value = Manager.Cache.Get(key);
        //    //}
        //    //return CellValue;
        //    return null;
        //}


        //public object SetTargetValue<T>(List<T> product, Expression<Func<T, int>> expkey)
        //{

        //    RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(Result, "");
        //    var mb = expkey.GetMemberInfo();
        //    string key = mb.Name;
        //    string tableName = expkey.Parameters[0].Type.Name;
        //    // key = tableName + ":" + key + ":" + CellValue.ToString();
        //    //Dict.TryGetValue(key, out value);
        //    //if (Manager.Cache.Exists(key))
        //    //{
        //    //    value = Manager.Cache.Get(key);
        //    //}
        //    return null;
        //}


        private List<DependColumn> relatedCols;

        /// <summary>
        /// 关联的列 其中必须有一列是主要标识目标列
        /// </summary>
        public List<DependColumn> RelatedCols { get => relatedCols; set => relatedCols = value; }
        public Type DependType { get => _DependencyType; set => _DependencyType = value; }

        public List<object> SourceList { get => sourceList; set => sourceList = value; }



        /// <summary>
        /// 缓存的产品列表
        /// </summary>
        //public List<P> SourceList { get => sourceList; set => sourceList = value; }
    }
    */
    /*
    /// <summary>
    /// 关联列 通过一个列查出一个对象（公共部分productshare)，将其他列的值自动给出
    /// 并且部分指定的可以指定到目标区(目标列是同时存在于，单据明细中。要从这个对象中带过去。）
    /// 意思是程序控制的列
    /// </summary>
    public class DependColumn
    {
        private string colName;
        private int colIndex;
        private string colTitle;
        /// <summary>
        /// 只读模式
        /// </summary>
        private bool _ReadOnly = false;

        /// <summary>
        /// 列只读
        /// </summary>
        public bool ReadOnly { get => _ReadOnly; set => _ReadOnly = value; }
        /// <summary>
        /// 列可见
        /// </summary>
        public bool Visible { get => visible; set => visible = value; }
        private bool visible = true;
        public string ColName { get => colName; set => colName = value; }
        public string ColCaption { get => colTitle; set => colTitle = value; }
        public int ColIndex { get => colIndex; set => colIndex = value; }
        //public Type ValueObjType { get => valueObjType; set => valueObjType = value; }

        /// <summary>
        /// 识别是否为主要的目标列  标识列,只有一个列是主键
        /// </summary>
        public bool IsPrimaryBizKeyColumn { get; set; }
        /// <summary>
        /// 是否为键值类型的列  暂时只用bool来标记。如果后面扩展需要取特性里面的值 也可以在这里扩展
        /// 比方公共部分产品视图带出的实体中有单位。实际单位只是主键值，要显示名称时，这时单位标记录FK外键
        /// </summary>
        public bool IsFKRelationColumn { get; set; }

        /// <summary>
        /// 指定目标的列，意思是公共部分的列也存在于单据明细中。要查出来的值给到明细对象中去
        /// </summary>
        public bool GuideToTargetColumn { get; set; }
    }
    */
      


}
