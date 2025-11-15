using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices;
using RUINORERP.Services;
using RUINORERP.Repository.Base;
using RUINORERP.IRepository.Base;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using System.Linq.Expressions;

using SqlSugar;

namespace RUINORERP.Business
{
    /// <summary>
    /// 单表基础资料操作
    /// </summary>
    public class BaseController
    {

        public BaseController()
        {

        }

        public virtual Task<TT> GetEntityAsync<TT>(long id)
        {
            //子类重写
            throw new Exception("子类要重写GetPrintData，保证打印数据的完整提供。");
        }

        public virtual Task<TT> GetEntityAsync<TT>(string BillNo)
        {
            //子类重写_unitOfWorkManage.GetDbClient().Queryable<T>().Where(whereExp).First();
            throw new Exception("子类要重写GetPrintData，保证打印数据的完整提供。");
        }

        //public virtual Task<List<T>> GetPrintDataSource(long id)
        //{
        //    //子类重写
        //    throw new Exception("子类要重写GetPrintData，保证打印数据的完整提供。");
        //}

    }
}
