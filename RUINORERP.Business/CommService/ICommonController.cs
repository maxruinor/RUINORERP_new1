using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RUINORERP.Model;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 公共控制器接口
    /// </summary>
    public interface ICommonController
    {
        /// <summary>
        /// 获取绑定数据源
        /// </summary>
        List<T> GetBindSource<T>(string tableName) where T : class;

        /// <summary>
        /// 根据条件获取绑定数据源
        /// </summary>
        List<T> GetBindSource<T>(string tableName, Expression<Func<T, bool>> expCondition);

        /// <summary>
        /// 获取绑定数据源列表
        /// </summary>
        object GetBindSourceList(string tableName);

        /// <summary>
        /// 查询单位列表
        /// </summary>
        Task<List<tb_Unit>> Query();

    }
}