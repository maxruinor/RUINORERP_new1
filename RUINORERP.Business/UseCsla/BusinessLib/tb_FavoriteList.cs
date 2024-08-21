
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2023 17:39:37
// **************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RUINORERP.Model;
using Csla;
using RUINORERP.Business.UseCsla;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using FluentValidation.Results;
using RUINORERP.Services;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;

namespace RUINORERP.Business.UseCsla
{

    [Serializable]
    /// <summary>
    /// 收藏表
    /// </summary>
    public class tb_FavoriteList : ReadOnlyListBase<tb_FavoriteList, tb_FavoriteEditInfo>
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        public readonly ILogger<tb_FavoriteList> _logger;
        public Itb_CurrencyServices _tb_CurrencyServices { get; set; }


        public tb_FavoriteList(ILogger<tb_FavoriteList> logger, IUnitOfWorkManage unitOfWorkManage, tb_CurrencyServices tb_CurrencyServices)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _tb_CurrencyServices = tb_CurrencyServices;
        }
        public void RemoveChild(int resourceId)
        {
            var iro = IsReadOnly;
            IsReadOnly = false;
            try
            {
                var item = this.Where(r => r.ID == resourceId).FirstOrDefault();
                if (item != null)
                {
                    var index = this.IndexOf(item);
                    Remove(item);
                }
            }
            finally
            {
                IsReadOnly = iro;
            }
        }



        //public static async Task<tb_FavoriteList> GetResourceListAsync()
        //{
        //   // return await DataPortal.FetchAsync<ResourceList>();
        //}

        public tb_FavoriteList GetFavoriteList()
        {
            List<tb_Favorite> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Favorite>().ToList();
            var sortedList = from p in list orderby p.ID select p;
            //this.Count = sortedList.Count;
            //return sortedList.ToList(); 
            foreach (var item in sortedList)
            {
                
                var info = AutoMapper.AutoMapperHelper.Map<tb_FavoriteEditInfo>(item);
                using (LoadListMode)
                {
                    Add(info);
                }

            }
            return this;
            //return DataPortal.Fetch<ResourceList>();

           

        }

    }
}