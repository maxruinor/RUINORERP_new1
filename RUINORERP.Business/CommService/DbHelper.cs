using Microsoft.Extensions.Logging;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    public class DbHelper<T> where T : class, new()
    {
        public ApplicationContext _appContext;

        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public IUnitOfWorkManage _unitOfWorkManage;
        public ILogger<DbHelper<T>> _logger;

        public string BizTypeText { get; set; }
        public int BizTypeInt { get; set; }
        public DbHelper(ILogger<DbHelper<T>> logger, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _appContext = appContext;
            BizTypeMapper mapper = new BizTypeMapper();
            BizType bizType = mapper.GetBizType(typeof(T).Name);
            BizTypeText = bizType.ToString();
            BizTypeInt = (int)bizType;
        }

        /// <summary>
        /// 批量新增或保存
        /// 新增时会用雪花ID
        /// 依赖于外层事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> BaseAddOrUpdateAsync(List<T> list)
        {
            //List<T> results = new List<T>();

            //下面的写法可以做到批量的  插入更新。雪花ID

            // 开启事务，保证数据一致性

            //下面的写法可以做到批量的  插入更新。雪花ID
            var x = _unitOfWorkManage.GetDbClient().Storageable(list).ToStorage();
            await x.AsUpdateable.ExecuteCommandAsync();//存在更新
            await x.AsInsertable.ExecuteReturnSnowflakeIdListAsync();//不存在插入
                                                                     //var Insertlist = x.InsertList.Select(c => c.Item).ToList();
                                                                     //var Updatelist = x.UpdateList.Select(c => c.Item).ToList();
            return list;
        }

        /// <summary>
        /// 批量新增或保存
        /// 新增时会用雪花ID
        /// 依赖于外层事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual async Task<long> BaseDefaultAddElseUpdateAsync(List<T> list)
        {
            //下面的写法可以做到批量的  插入更新。雪花ID
            long counter = 0;
            List<long> ids = new List<long>();
            var x =await _unitOfWorkManage.GetDbClient().Storageable<T>(list).ToStorageAsync();
            ids = await x.AsInsertable.ExecuteReturnSnowflakeIdListAsync();//不存在插入 long 实际不会太长
            counter += await x.AsUpdateable.ExecuteCommandAsync();//存在更新
            return counter + ids.Count;
        }
        /// <summary>
        /// 批量新增或保存
        /// 新增时会用雪花ID
        /// 单个实体也可以
        /// 事务依赖于外层
        /// </summary>
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual async Task<long> BaseDefaultAddElseUpdateAsync(params T[] paralist)
        {
            //下面的写法可以做到批量的  插入更新。雪花ID

            List<T> list = new List<T>();
            foreach (var item in paralist)
            {
                list.Add(item);
            }
            return await BaseDefaultAddElseUpdateAsync(list);
        }

    }
}
