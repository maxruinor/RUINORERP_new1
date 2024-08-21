using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{



    /// <summary>
    /// 对查询参数设置的一些提供的公用方法
    /// </summary>
    //public class QueryParameterTool<T>
    //{

    //    public static QueryParameter<T> GetFieldEnumPara(QueryFieldType FieldType, Type type, Expression<Func<T, int?>> expEnumValueColName, bool AddSelectItem)
    //    {
    //        QueryParameter<T> para = new QueryParameter<T>();
    //        para.QueryFieldType = FieldType;
    //        QueryFieldEnumData queryFieldData = new QueryFieldEnumData();
    //        queryFieldData.EnumType = type;
    //        queryFieldData.expEnumValueColName = expEnumValueColName;
    //        queryFieldData.AddSelectItem = AddSelectItem;

    //        para.QueryField = queryFieldData.EnumValueColName;

    //        List<string> listStr = new List<string>();
    //        List<EnumEntityMember> list = new List<EnumEntityMember>();
    //        list = type.GetListByEnumtype(1);
    //        queryFieldData.BindDataSource = list;
 
    //        para.QueryFieldDataPara = queryFieldData;
    //        return para;
    //    }


    //}
}
