using Microsoft.Extensions.Logging;
using RUINORERP.Extensions.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using RUINORERP.Repository.UnitOfWorks;
using System.Linq.Expressions;
using RUINORERP.Model;
using System.ComponentModel;
using System.Reflection;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 缓存初始化服务，负责从数据库加载数据并初始化缓存
    /// </summary>
    [Obsolete("此缓存初始化服务已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
    public class CacheInitializationService
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly MyCacheManager _cacheManager;
        private readonly ILogger<CacheInitializationService> _logger;
        private readonly BaseCacheDataList _baseCacheDataList;
        private static BaseCacheDataList _staticBaseCacheDataList;
        
        /// <summary>
        /// 静态构造函数，用于初始化静态字段
        /// </summary>
        static CacheInitializationService()
        {
            _staticBaseCacheDataList = new BaseCacheDataList();
            // 初始化所有表结构信息
            InitializeAllTableSchemas(_staticBaseCacheDataList);
        }

        /// <summary>
        /// 获取所有可缓存的表名列表
        /// </summary>
        /// <returns>可缓存的表名列表</returns>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        public static List<string> GetCacheableTableNames()
        {
            return _staticBaseCacheDataList.GetCacheableTableNamesList();
        }

        /// <summary>
        /// 检查表是否可缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否可缓存</returns>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        public static bool IsCacheableTable(string tableName)
        {
            return _staticBaseCacheDataList.ContainsTable(tableName) && 
                   _staticBaseCacheDataList.GetSchemaInfo(tableName)?.IsCacheable == true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        [Obsolete("此构造函数已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        public CacheInitializationService(
            IUnitOfWorkManage unitOfWorkManage,
            MyCacheManager cacheManager,
            ILogger<CacheInitializationService> logger) : this(unitOfWorkManage, cacheManager, logger, new BaseCacheDataList())
        {
        }

        /// <summary>
        /// 构造函数（支持依赖注入BaseCacheDataList）
        /// </summary>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="baseCacheDataList">基础缓存数据列表</param>
        [Obsolete("此构造函数已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        public CacheInitializationService(
            IUnitOfWorkManage unitOfWorkManage,
            MyCacheManager cacheManager,
            ILogger<CacheInitializationService> logger,
            BaseCacheDataList baseCacheDataList)
        {
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _baseCacheDataList = baseCacheDataList ?? throw new ArgumentNullException(nameof(baseCacheDataList));
            
            // 初始化时注册所有表结构信息
            RegisterAllTableSchemas();
        }

        /// <summary>
        /// 初始化所有缓存
        /// </summary>
        /// <returns>初始化任务</returns>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        public async Task InitializeAllCacheAsync()
        {
            try
            {
                _logger.LogInformation("开始初始化所有缓存数据");

                // 清除现有缓存
                _cacheManager.CacheInfoList.Clear();
                _cacheManager.CacheEntityList.Clear();
                _cacheManager.NewTableList.Clear();

                // 将BaseCacheDataList中的表结构信息同步到MyCacheManager的NewTableList中
                SyncTableSchemasToCacheManager();

                // 获取所有需要缓存的表名
                var tableNames = _baseCacheDataList.GetCacheableTableNamesList();

                // 使用SqlSugar并行查询多个表
                var db = _unitOfWorkManage.GetDbClient();
                var tasks = new List<Task>();

                foreach (var tableName in tableNames)
                {
                    tasks.Add(InitializeCacheForTableAsync(tableName, db));
                }

                await Task.WhenAll(tasks);

                _logger.LogInformation($"所有缓存初始化完成，共初始化 {tableNames.Count} 个表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化所有缓存时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 注册所有表结构信息
        /// </summary>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        private void RegisterAllTableSchemas()
        {
            InitializeAllTableSchemas(_baseCacheDataList);
        }

        /// <summary>
        /// 初始化所有表结构信息（通用方法）
        /// </summary>
        /// <param name="baseCacheDataList">基础缓存数据列表</param>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        private static void InitializeAllTableSchemas(BaseCacheDataList baseCacheDataList)
        {
            try
            {
                // 注册所有需要缓存的表结构信息
                // 基础数据表
                RegistInformation<tb_Company>(baseCacheDataList, k => k.ID, v => v.CNName);
                RegistInformation<tb_Currency>(baseCacheDataList, k => k.Currency_ID, v => v.CurrencyName);
                RegistInformation<tb_CurrencyExchangeRate>(baseCacheDataList, k => k.ExchangeRateID, v => v.ConversionName);
                RegistInformation<tb_BOM_S>(baseCacheDataList, k => k.BOM_ID, v => v.BOM_Name);
                RegistInformation<tb_ProductType>(baseCacheDataList, k => k.Type_ID, v => v.TypeName);
                RegistInformation<tb_PaymentMethod>(baseCacheDataList, k => k.Paytype_ID, v => v.Paytype_Name);
                RegistInformation<tb_Unit>(baseCacheDataList, k => k.Unit_ID, v => v.UnitName);
                RegistInformation<tb_Department>(baseCacheDataList, k => k.DepartmentID, v => v.DepartmentName);
                RegistInformation<tb_LocationType>(baseCacheDataList, k => k.LocationType_ID, v => v.TypeName);
                RegistInformation<tb_Location>(baseCacheDataList, k => k.Location_ID, v => v.Name);
                
                // 供应商相关
                RegistInformation<tb_CustomerVendor>(baseCacheDataList, k => k.CustomerVendor_ID, v => v.CVName);
                RegistInformation<tb_CustomerVendorType>(baseCacheDataList, k => k.Type_ID, v => v.TypeName);
                RegistInformation<tb_ProdCategories>(baseCacheDataList, k => k.Category_ID, v => v.Category_name);
                RegistInformation<tb_Prod>(baseCacheDataList, k => k.ProdBaseID, v => v.CNName);
                
                // 视图
                RegistInformation<View_ProdDetail>(baseCacheDataList, k => k.ProdDetailID, v => v.CNName, isView: true);
                RegistInformation<View_ProdInfo>(baseCacheDataList, k => k.ProdBaseID, v => v.CNName, isView: true);
                
                // 产品相关表
                RegistInformation<tb_ProdPropertyType>(baseCacheDataList, k => k.PropertyType_ID, v => v.PropertyTypeName);
                RegistInformation<tb_ProdProperty>(baseCacheDataList, k => k.Property_ID, v => v.PropertyName);
                RegistInformation<tb_ProdPropertyValue>(baseCacheDataList, k => k.PropertyValueID, v => v.PropertyValueName);
                RegistInformation<tb_ProdBundle>(baseCacheDataList, k => k.BundleID, v => v.BundleName);
                RegistInformation<tb_Packing>(baseCacheDataList, k => k.Pack_ID, v => v.PackagingName);
                
                // 员工和权限相关
                RegistInformation<tb_Employee>(baseCacheDataList, k => k.Employee_ID, v => v.Employee_Name);
                RegistInformation<tb_UserInfo>(baseCacheDataList, k => k.User_ID, v => v.UserName);
                RegistInformation<tb_RoleInfo>(baseCacheDataList, k => k.RoleID, v => v.RoleName);
                RegistInformation<tb_MenuInfo>(baseCacheDataList, k => k.MenuID, v => v.MenuName);
                RegistInformation<tb_ModuleDefinition>(baseCacheDataList, k => k.ModuleID, v => v.ModuleName);
                
                // 业务类型
                RegistInformation<tb_BizType>(baseCacheDataList, k => k.Type_ID, v => v.TypeName);
                RegistInformation<tb_StorageRack>(baseCacheDataList, k => k.Rack_ID, v => v.RackName);
                RegistInformation<tb_OutInStockType>(baseCacheDataList, k => k.Type_ID, v => v.TypeName);
                RegistInformation<tb_OnlineStoreInfo>(baseCacheDataList, k => k.Store_ID, v => v.StoreName);
                RegistInformation<tb_ProjectGroup>(baseCacheDataList, k => k.ProjectGroup_ID, v => v.ProjectGroupName);
                
                // 财务相关
                RegistInformation<tb_FM_Account>(baseCacheDataList, k => k.Account_id, v => v.Account_name);
                RegistInformation<tb_FM_ExpenseType>(baseCacheDataList, k => k.ExpenseType_id, v => v.Expense_name);
                RegistInformation<tb_FM_Subject>(baseCacheDataList, k => k.Subject_id, v => v.Subject_name);
                RegistInformation<tb_FM_PayeeInfo>(baseCacheDataList, k => k.PayeeInfoID, v => v.Account_name);
                
                // 其他基础数据
                RegistInformation<tb_BoxRules>(baseCacheDataList, k => k.BoxRules_ID, v => v.BoxRuleName);
                RegistInformation<tb_CartoonBox>(baseCacheDataList, k => k.CartonID, v => v.CartonName);
                RegistInformation<tb_Files>(baseCacheDataList, k => k.Doc_ID, v => v.FileName);
                
                // CRM相关
                RegistInformation<tb_CRM_Customer>(baseCacheDataList, k => k.Customer_id, v => v.CustomerName);
                RegistInformation<tb_CRM_Leads>(baseCacheDataList, k => k.LeadID, v => v.CustomerName);
                RegistInformation<tb_CRM_Region>(baseCacheDataList, k => k.Region_ID, v => v.Region_Name);
                RegistInformation<tb_CRM_Contact>(baseCacheDataList, k => k.Contact_id, v => v.Contact_Name);
                
                // 地理信息
                RegistInformation<tb_Provinces>(baseCacheDataList, k => k.ProvinceID, v => v.ProvinceCNName);
                RegistInformation<tb_Cities>(baseCacheDataList, k => k.CityID, v => v.CityCNName);
                
                // 配置相关
                RegistInformation<tb_BOMConfigHistory>(baseCacheDataList, k => k.BOM_S_VERID, v => v.VerNo);
                RegistInformation<tb_RolePropertyConfig>(baseCacheDataList, k => k.RolePropertyID, v => v.RolePropertyName);
                RegistInformation<tb_Unit_Conversion>(baseCacheDataList, k => k.UnitConversion_ID, v => v.UnitConversion_Name);
                
                // 补充缺失的表
                RegistInformation<tb_FieldInfo>(baseCacheDataList, k => k.FieldInfo_ID, v => v.FieldName);
            }
            catch (Exception ex)
            {
                // 初始化表结构信息时的异常处理
                System.Diagnostics.Debug.WriteLine($"初始化表结构信息时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 注册表结构信息（通用方法）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="baseCacheDataList">基础缓存数据列表</param>
        /// <param name="primaryKeyExpression">主键字段表达式</param>
        /// <param name="displayFieldExpression">显示字段表达式</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        private static void RegistInformation<T>(
            BaseCacheDataList baseCacheDataList,
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null) where T : class
        {
            baseCacheDataList.RegistInformation(primaryKeyExpression, displayFieldExpression, isView, isCacheable, description);
        }

        /// <summary>
        /// 将BaseCacheDataList中的表结构信息同步到MyCacheManager的NewTableList中
        /// </summary>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        private void SyncTableSchemasToCacheManager()
        {
            try
            {
                _logger.LogInformation("开始同步表结构信息到MyCacheManager");

                // 清空现有的NewTableList
                _cacheManager.NewTableList.Clear();
                _cacheManager.NewTableTypeList.Clear();

                // 遍历所有注册的表结构信息
                foreach (var kvp in _baseCacheDataList.TableSchemas)
                {
                    var schemaInfo = kvp.Value;
                    
                    // 同步到MyCacheManager的NewTableList
                    var keyValuePair = new KeyValuePair<string, string>(
                        schemaInfo.PrimaryKeyField, 
                        schemaInfo.DisplayField);
                    
                    _cacheManager.NewTableList.TryAdd(schemaInfo.TableName, keyValuePair);
                    _cacheManager.NewTableTypeList.TryAdd(schemaInfo.TableName, schemaInfo.EntityType);

                    // 注册外键关系
                    if (schemaInfo.ForeignKeys.Any())
                    {
                        var fkMappings = schemaInfo.ForeignKeys
                            .Select(fk => new KeyValuePair<string, string>(fk.ForeignKeyField, fk.RelatedTableName))
                            .ToList();
                        
                        _cacheManager.ForeignKeyMappings.TryAdd(schemaInfo.TableName, fkMappings);
                    }
                }

                _logger.LogInformation($"成功同步 {_baseCacheDataList.TableSchemas.Count} 个表结构信息到MyCacheManager");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "同步表结构信息到MyCacheManager时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 注册表结构信息（泛型方法）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="primaryKeyExpression">主键字段表达式</param>
        /// <param name="displayFieldExpression">显示字段表达式</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        private void RegistInformation<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null) where T : class
        {
            _baseCacheDataList.RegistInformation(primaryKeyExpression, displayFieldExpression, isView, isCacheable, description);
        }

        /// <summary>
        /// 初始化指定表的缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>初始化任务</returns>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        public async Task InitializeCacheForTableAsync(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning("表名为空，跳过初始化");
                return;
            }

            var db = _unitOfWorkManage.GetDbClient();
            
            try
            {
                _logger.LogInformation($"开始初始化表 {tableName} 的缓存");

                // 获取实体类型
                var entityType = _cacheManager.GetEntityTypeByTableName(tableName);
                if (entityType == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 对应的实体类型");
                    return;
                }

                _cacheManager.SetDictDataSourceByType(tableName, entityType, false);

                // 使用SqlSugar直接从数据库加载数据
                await LoadDataAndUpdateCacheAsync(entityType, tableName, db);

                _logger.LogInformation($"表 {tableName} 的缓存初始化完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化表 {tableName} 的缓存时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 初始化指定表的缓存（内部方法）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="db">数据库客户端</param>
        /// <returns>初始化任务</returns>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        private async Task InitializeCacheForTableAsync(string tableName, ISqlSugarClient db)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning("表名为空，跳过初始化");
                return;
            }

            try
            {
                _logger.LogInformation($"开始初始化表 {tableName} 的缓存");

                // 获取实体类型
                var entityType = _cacheManager.GetEntityTypeByTableName(tableName);
                if (entityType == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 对应的实体类型");
                    return;
                }

                // 使用MyCacheManager的SetDictDataSourceByType方法注册表结构
                _cacheManager.SetDictDataSourceByType(tableName, entityType, false);

                // 使用SqlSugar直接从数据库加载数据
                await LoadDataAndUpdateCacheAsync(entityType, tableName, db);

                _logger.LogInformation($"表 {tableName} 的缓存初始化完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化表 {tableName} 的缓存时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 从数据库加载数据并更新缓存
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="tableName">表名</param>
        /// <param name="db">数据库客户端</param>
        /// <returns>加载任务</returns>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        private async Task LoadDataAndUpdateCacheAsync(Type entityType, string tableName, ISqlSugarClient db)
        {
            try
            {
                // 使用SqlSugar直接查询数据库
                var queryable = db.Queryable(entityType.Name, tableName);

                // 执行查询，直接调用ToList方法（同步）
                var result = queryable.ToList();

                if (result != null)
                {
                    // 将结果转换为List<object>
                    var entityList = result as IEnumerable<object>;
                    if (entityList == null)
                    {
                        // 如果结果不是IEnumerable<object>，尝试转换
                        var listType = typeof(List<>).MakeGenericType(typeof(object));
                        entityList = (IEnumerable<object>)Activator.CreateInstance(listType, result);
                    }

                    // 更新缓存 - 使用非泛型方法
                    _cacheManager.UpdateEntityList(tableName, entityList.ToList());
                    _logger.LogInformation($"已从数据库加载 {entityList.Count()} 条 {tableName} 记录到缓存");
                }
                else
                {
                    _logger.LogWarning($"从数据库获取的 {tableName} 数据为空或格式不正确");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"从数据库加载 {tableName} 数据并更新缓存时发生错误");
                throw;
            }
        }
        
        /// <summary>
        /// 序列化缓存数据以便在网络中传输
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">要序列化的数据</param>
        /// <param name="serializationType">序列化方式</param>
        /// <returns>序列化后的字节数组</returns>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        public byte[] SerializeCacheDataForTransmission<T>(T data, CacheSerializationHelper.SerializationType serializationType = CacheSerializationHelper.SerializationType.Json)
        {
            return CacheSerializationHelper.Serialize(data, serializationType);
        }
        
        /// <summary>
        /// 反序列化从网络接收到的缓存数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">接收到的字节数组</param>
        /// <param name="serializationType">序列化方式</param>
        /// <returns>反序列化后的数据</returns>
        [Obsolete("此方法已过时，请使用 RUINORERP.Business.CommService.OptimizedCacheInitializationService")]
        public T DeserializeCacheDataFromTransmission<T>(byte[] data, CacheSerializationHelper.SerializationType serializationType = CacheSerializationHelper.SerializationType.Json)
        {
            return CacheSerializationHelper.Deserialize<T>(data, serializationType);
        }
    }
}