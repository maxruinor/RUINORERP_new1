using RUINORERP.Common.CustomAttribute;
using RUINORERP.Common.Helper;
using RUINORERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 枚举的类型的数据
    /// </summary>
    public class QueryFieldEnumData : IQueryFieldData
    {
        public QueryFieldEnumData()
        {

        }

        public QueryFieldEnumData(Type _EnumType)
        {
            EnumType = _EnumType;
        }

        public Type EnumType { get; set; }

        /// <summary>
        /// 能默认一次添加的普通字段用这个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_expEnumValueColName"></param>
        public void SetEnumValueColName<T>(Expression<Func<T, object>> _expEnumValueColName,bool AddSelectItem)
        {
            SetEnumValueColName<T>(_expEnumValueColName, AddSelectItem);
        }
        /// <summary>
        /// 能默认一次添加的普通字段用这个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_expEnumValueColName"></param>
        public void SetEnumValueColName<T>(Expression<Func<T, object>> _expEnumValueColName)
        {
            //指定到字符类型，方便使用
            string fieldName = RuinorExpressionHelper.ExpressionToString<T>(_expEnumValueColName);
            EnumValueColName = fieldName;

        }



        /// <summary>
        /// 枚举在实体中的字段名，通过表达设置
        /// </summary>
        public string EnumValueColName { get; set; }


        /// <summary>
        /// 枚举在实体中的字段名，通过表达设置
        /// </summary>
        //public string EnumDisplayColName { get; set; }


        /// <summary>
        /// 是否添加【请选择】下拉结果
        /// </summary>
        public bool AddSelectItem { get; set; } = false;

        /// <summary>
        /// 如果没有指定则
        /// </summary>
        public List<EnumEntityMember> BindDataSource { get; set; }

    }
    /// <summary>
    /// 枚举的类型的数据
    /// </summary>
    public class QueryFieldDateTimeRangeData : IQueryFieldData
    {
        public QueryFieldDateTimeRangeData()
        {

        }

        public QueryFieldDateTimeRangeData(bool _Selected)
        {
            Selected = _Selected;
        }
        /// <summary>
        /// 时间控件是否默认选中
        /// </summary>
        public bool Selected { get; set; }

        

    }


}
