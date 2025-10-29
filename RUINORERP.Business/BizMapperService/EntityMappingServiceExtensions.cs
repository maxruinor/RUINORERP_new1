using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizMapperService
{

    /// <summary>
    /// 实体映射服务静态访问类
    /// 提供静态方法直接访问实体映射服务，避免在每个类中都需要注入服务实例
    /// </summary>
    public static class EntityMappingHelper
    {
        private static IEntityMappingService _currentService;

        /// <summary>
        /// 设置当前使用的实体映射服务实例
        /// 此方法应在应用程序启动时调用，通常在依赖注入容器配置完成后
        /// </summary>
        /// <param name="service">实体映射服务实例</param>
        public static void SetCurrent(IEntityMappingService service)
        {
            _currentService = service;
        }

        /// <summary>
        /// 获取当前实体映射服务实例
        /// 如果尚未设置，则会抛出异常
        /// </summary>
        public static IEntityMappingService Current
        {
            get
            {
                if (_currentService == null)
                {
                    throw new InvalidOperationException("实体映射服务未初始化，请先调用SetCurrent方法设置服务实例");
                }
                return _currentService;
            }
        }

        /// <summary>
        /// 初始化实体映射服务
        /// </summary>
        public static void Initialize()
        {
            Current.Initialize();
        }

        /// <summary>
        /// 根据业务类型获取实体信息
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>实体信息</returns>
        public static BizEntityInfo GetEntityInfo(BizType bizType)
        {
            return Current.GetEntityInfo(bizType);
        }

        /// <summary>
        /// 根据实体类型获取实体信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体信息</returns>
        public static BizEntityInfo GetEntityInfo(Type entityType)
        {
            return Current.GetEntityInfo(entityType);
        }

        /// <summary>
        /// 根据实体类型和枚举标志获取实体信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="enumFlag">枚举标志</param>
        /// <returns>实体信息</returns>
        public static BizEntityInfo GetEntityInfo(Type entityType, int enumFlag)
        {
            return Current.GetEntityInfo(entityType, enumFlag);
        }

        /// <summary>
        /// 获取指定实体类型的实体信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>实体信息</returns>
        public static BizEntityInfo GetEntityInfo<TEntity>() where TEntity : class
        {
            return Current.GetEntityInfo(typeof(TEntity));
        }

        /// <summary>
        /// 根据枚举标志获取指定实体类型的实体信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="enumFlag">枚举标志</param>
        /// <returns>实体信息</returns>
        public static BizEntityInfo GetEntityInfo<TEntity>(int enumFlag) where TEntity : class
        {
            return Current.GetEntityInfo(typeof(TEntity), enumFlag);
        }

        /// <summary>
        /// 根据表名获取实体信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体信息</returns>
        public static BizEntityInfo GetEntityInfoByTableName(string tableName)
        {
            return Current.GetEntityInfoByTableName(tableName);
        }

        /// <summary>
        /// 获取所有注册的实体信息
        /// </summary>
        /// <returns>实体信息集合</returns>
        public static IEnumerable<BizEntityInfo> GetAllEntityInfos()
        {
            return Current.GetAllEntityInfos();
        }

        /// <summary>
        /// 根据业务类型获取实体类型
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>实体类型</returns>
        public static Type GetEntityType(BizType bizType)
        {
            return Current.GetEntityType(bizType);
        }

        /// <summary>
        /// 根据实体类型获取业务类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entity">实体对象（可选）</param>
        /// <returns>业务类型</returns>
        public static BizType GetBizType(Type entityType, object entity = null)
        {
            return Current.GetBizType(entityType, entity);
        }

        /// <summary>
        /// 根据实体对象获取业务类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>业务类型</returns>
        public static BizType GetBizTypeByEntity(object entity)
        {
            return Current.GetBizTypeByEntity(entity);
        }

        /// <summary>
        /// 根据实体对象获取业务类型（泛型版本）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>业务类型</returns>
        public static BizType GetBizType<T>(T entity) where T : class
        {
            if (entity == null)
                return BizType.无对应数据;
            return Current.GetBizType(typeof(T), entity);
        }

        /// <summary>
        /// 根据表名获取实体类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体类型</returns>
        public static Type GetEntityTypeByTableName(string tableName)
        {
            return Current.GetEntityTypeByTableName(tableName);
        }

        /// <summary>
        /// 检查业务类型是否已注册
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>是否已注册</returns>
        public static bool IsRegistered(BizType bizType)
        {
            return Current.IsRegistered(bizType);
        }

        /// <summary>
        /// 检查实体类型是否已注册
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>是否已注册</returns>
        public static bool IsRegistered(Type entityType)
        {
            return Current.IsRegistered(entityType);
        }

        /// <summary>
        /// 检查表名是否已注册
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否已注册</returns>
        public static bool IsRegisteredByTableName(string tableName)
        {
            return Current.IsRegisteredByTableName(tableName);
        }

        /// <summary>
        /// 获取实体对象的ID和名称
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>ID和名称的元组</returns>
        public static (long Id, string Name) GetIdAndName(object entity)
        {
            return Current.GetIdAndName(entity);
        }

        /// <summary>
        /// 得到相关单据信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public static CommBillData GetBillData(Type type, object Entity)
        {
            CommBillData cbd = new CommBillData();
            var entityInfo = EntityMappingHelper.GetEntityInfo(type);
            BizType bizType = EntityMappingHelper.GetBizType(type, Entity);
            Entity = Entity ?? Activator.CreateInstance(type);
            cbd.BillNo = Entity.GetPropertyValue(entityInfo.NoField).ToString();
            cbd.BillID = Entity.GetPropertyValue(entityInfo.IdField).ToLong();
            cbd.BillNoColName = entityInfo.NoField;
            cbd.BizName = bizType.ObjToString();
            return cbd;
        }
        public static CommBillData GetBillData<T>(object Entity)
        {
            return EntityMappingHelper.GetBillData(typeof(T), Entity);
        }

    }
}