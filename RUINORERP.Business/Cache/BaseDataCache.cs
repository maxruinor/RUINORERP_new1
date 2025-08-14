using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Common.Helper;
using RUINORERP.Model.Base;
using Microsoft.Extensions.Logging;


using System.Linq.Expressions;

using CacheManager.Core;
using global::RUINORERP.Model;
using RUINORERP.Business.Cache.Attributes;

namespace RUINORERP.Business.Cache
{


 
        /// <summary>
        /// 基础数据缓存实现
        /// </summary>
        [CacheName("BaseData")]
        public class BaseDataCache : CacheBase, IBaseDataCache
        {
            public BaseDataCache(ICacheManager<object> cacheManager, ILogger<BaseDataCache> logger, CachePolicy defaultPolicy)
                : base(cacheManager, logger, defaultPolicy)
            {
            }

            public override string CacheName => "BaseData";

            /// <summary>
            /// 获取实体类型的缓存键前缀
            /// </summary>
            private string GetEntityTypeKey<T>() where T : BaseEntity
            {
                return typeof(T).Name;
            }

            /// <summary>
            /// 获取实体的缓存键
            /// </summary>
            private string GetEntityKey<T>(long id) where T : BaseEntity
            {
                return $"{GetEntityTypeKey<T>()}:{id}";
            }

            /// <summary>
            /// 获取实体列表的缓存键
            /// </summary>
            private string GetEntityListKey<T>() where T : BaseEntity
            {
                return $"{GetEntityTypeKey<T>()}:List";
            }

            public List<T> GetEntityList<T>() where T : BaseEntity
            {
                return Get<List<T>>(GetEntityListKey<T>()) ?? new List<T>();
            }

            public async Task<List<T>> GetEntityListAsync<T>() where T : BaseEntity
            {
                var result = await GetAsync<List<T>>(GetEntityListKey<T>());
                return result ?? new List<T>();
            }

            public T GetEntity<T>(long id) where T : BaseEntity
            {
                return Get<T>(GetEntityKey<T>(id));
            }

            public async Task<T> GetEntityAsync<T>(long id) where T : BaseEntity
            {
                return await GetAsync<T>(GetEntityKey<T>(id));
            }

            public void SetEntityList<T>(List<T> entities, TimeSpan? expiration = null) where T : BaseEntity
            {
                if (entities == null || !entities.Any())
                    return;

                var listKey = GetEntityListKey<T>();
                Set(listKey, entities, expiration);

                // 同时缓存单个实体，提高查询效率
                foreach (var entity in entities)
                {
                    Set(GetEntityKey<T>(entity.PrimaryKeyID), entity, expiration);
                }
            }

            public async Task SetEntityListAsync<T>(List<T> entities, TimeSpan? expiration = null) where T : BaseEntity
            {
                if (entities == null || !entities.Any())
                    return;

                var listKey = GetEntityListKey<T>();
                await SetAsync(listKey, entities, expiration);

                // 同时缓存单个实体，提高查询效率
                foreach (var entity in entities)
                {
                    await SetAsync(GetEntityKey<T>(entity.PrimaryKeyID), entity, expiration);
                }
            }

            public void UpdateEntity<T>(T entity) where T : BaseEntity
            {
                if (entity == null)
                    return;

                var entityKey = GetEntityKey<T>(entity.PrimaryKeyID);
                Set(entityKey, entity);

                // 更新列表中的实体
                var listKey = GetEntityListKey<T>();
                AddOrUpdate(listKey, new List<T> { entity }, existingList =>
                {
                    if (existingList is List<T> list)
                    {
                        var index = list.FindIndex(e => e.PrimaryKeyID == entity.PrimaryKeyID);
                        if (index >= 0)
                        {
                            list[index] = entity;
                        }
                        else
                        {
                            list.Add(entity);
                        }
                        return list;
                    }
                    return new List<T> { entity };
                });
            }

            public async Task UpdateEntityAsync<T>(T entity) where T : BaseEntity
            {
                if (entity == null)
                    return;

                var entityKey = GetEntityKey<T>(entity.PrimaryKeyID);
                await SetAsync(entityKey, entity);

                // 这里简化处理，实际项目中可能需要更复杂的逻辑
                var list = await GetEntityListAsync<T>();
                var index = list.FindIndex(e => e.PrimaryKeyID == entity.PrimaryKeyID);

                if (index >= 0)
                {
                    list[index] = entity;
                }
                else
                {
                    list.Add(entity);
                }

                await SetEntityListAsync(list);
            }

            public void RemoveEntity<T>(long id) where T : BaseEntity
            {
                var entityKey = GetEntityKey<T>(id);
                Remove(entityKey);

                // 从列表中移除
                var listKey = GetEntityListKey<T>();
                AddOrUpdate(listKey, new List<T>(), existingList =>
                {
                    if (existingList is List<T> list)
                    {
                        list.RemoveAll(e => e.PrimaryKeyID == id);
                        return list;
                    }
                    return new List<T>();
                });
            }

            public async Task RemoveEntityAsync<T>(long id) where T : BaseEntity
            {
                var entityKey = GetEntityKey<T>(id);
                await RemoveAsync(entityKey);

                // 从列表中移除
                var list = await GetEntityListAsync<T>();
                list.RemoveAll(e => e.PrimaryKeyID == id);
                await SetEntityListAsync(list);
            }

            public List<T> QueryEntities<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
            {
                var list = GetEntityList<T>();
                return list.AsQueryable().Where(predicate).ToList();
            }
        }
    }


