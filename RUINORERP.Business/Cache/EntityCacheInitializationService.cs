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
using RUINORERP.Business.CommService;
using System.Data.SqlClient;
using System.Threading;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 优化的缓存初始化服务，专门为新的优化缓存管理器设计
    /// </summary>
    public class EntityCacheInitializationService
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly IEntityCacheManager _cacheManager;
        private readonly ILogger<EntityCacheInitializationService> _logger;
        private readonly SemaphoreSlim _dbSemaphore; // 限制并发数据库查询数量
        private readonly int _maxRetryAttempts = 3; // 最大重试次数
        private readonly int _retryDelayMs = 1000; // 重试延迟毫秒数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        /// <param name="cacheManager">优化的缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        public EntityCacheInitializationService(
            IUnitOfWorkManage unitOfWorkManage,
            IEntityCacheManager cacheManager,
            ILogger<EntityCacheInitializationService> logger)
        {
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // 限制并发数据库查询数量，避免连接池耗尽
            _dbSemaphore = new SemaphoreSlim(5, 5); // 最多同时5个查询
        }

        /// <summary>
        /// 初始化所有缓存
        /// </summary>
        /// <returns>初始化任务</returns>
        public async Task InitializeAllCacheAsync()
        {
            try
            {
                _logger.LogInformation("开始初始化所有缓存数据（优化版）");

                // 初始化所有表结构信息
                InitializeAllTableSchemas();

                // 只获取需要缓存的表名（基于已注册的表结构信息进行过滤）
                var tableNames = TableSchemaManager.Instance.GetCacheableTableNamesList();

                // 使用SqlSugar并行查询多个表，但限制并发数量
                var tasks = new List<Task>();

                foreach (var tableName in tableNames)
                {
                    // 使用本地变量避免闭包问题
                    var tableNameCopy = tableName;
                    tasks.Add(InitializeCacheForTableAsync(tableNameCopy));
                }

                // 等待所有任务完成，但限制并发执行
                await Task.WhenAll(tasks);

                _logger.LogInformation($"所有缓存初始化完成（优化版），共初始化 {tableNames.Count} 个表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化所有缓存时发生错误（优化版）");
                throw;
            }
        }

        /// <summary>
        /// 初始化所有表结构信息（通用方法）
        /// 客户端启动时加载。不加载缓存。缓存按需从服务器取值
        /// </summary>
        public void InitializeAllTableSchemas()
        {
            try
            {
                // 注册所有需要缓存的表结构信息
                // 基础数据表
                RegistInformation<tb_Company>(k => k.ID, v => v.CNName);
                RegistInformation<tb_Currency>(k => k.Currency_ID, v => v.CurrencyName);
                RegistInformation<tb_CurrencyExchangeRate>(k => k.ExchangeRateID, v => v.ConversionName);
                RegistInformation<tb_BOM_S>(k => k.BOM_ID, v => v.BOM_Name);
                RegistInformation<tb_ProductType>(k => k.Type_ID, v => v.TypeName);
                RegistInformation<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name);
                RegistInformation<tb_Unit>(k => k.Unit_ID, v => v.UnitName);
                RegistInformation<tb_Department>(k => k.DepartmentID, v => v.DepartmentName);
                RegistInformation<tb_LocationType>(k => k.LocationType_ID, v => v.TypeName);
                RegistInformation<tb_Location>(k => k.Location_ID, v => v.Name);

                // 供应商相关
                RegistInformation<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName);
                RegistInformation<tb_CustomerVendorType>(k => k.Type_ID, v => v.TypeName);
                RegistInformation<tb_ProdCategories>(k => k.Category_ID, v => v.Category_name);
                RegistInformation<tb_Prod>(k => k.ProdBaseID, v => v.CNName);

                // 视图
                RegistInformation<View_ProdDetail>(k => k.ProdDetailID, v => v.CNName, isView: true);
                RegistInformation<View_ProdInfo>(k => k.ProdBaseID, v => v.CNName, isView: true);

                // 产品相关表
                RegistInformation<tb_ProdPropertyType>(k => k.PropertyType_ID, v => v.PropertyTypeName);
                RegistInformation<tb_ProdProperty>(k => k.Property_ID, v => v.PropertyName);
                RegistInformation<tb_ProdPropertyValue>(k => k.PropertyValueID, v => v.PropertyValueName);
                RegistInformation<tb_ProdBundle>(k => k.BundleID, v => v.BundleName);
                RegistInformation<tb_Packing>(k => k.Pack_ID, v => v.PackagingName);

                // 员工和权限相关
                RegistInformation<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name);
                RegistInformation<tb_UserInfo>(k => k.User_ID, v => v.UserName);
                RegistInformation<tb_RoleInfo>(k => k.RoleID, v => v.RoleName);
                RegistInformation<tb_MenuInfo>(k => k.MenuID, v => v.MenuName);
                RegistInformation<tb_ModuleDefinition>(k => k.ModuleID, v => v.ModuleName);

                // 业务类型
                RegistInformation<tb_BizType>(k => k.Type_ID, v => v.TypeName);
                RegistInformation<tb_StorageRack>(k => k.Rack_ID, v => v.RackName);
                RegistInformation<tb_OutInStockType>(k => k.Type_ID, v => v.TypeName);
                RegistInformation<tb_OnlineStoreInfo>(k => k.Store_ID, v => v.StoreName);
                RegistInformation<tb_ProjectGroup>(k => k.ProjectGroup_ID, v => v.ProjectGroupName);

                // 财务相关
                RegistInformation<tb_FM_Account>(k => k.Account_id, v => v.Account_name);
                RegistInformation<tb_FM_ExpenseType>(k => k.ExpenseType_id, v => v.Expense_name);
                RegistInformation<tb_FM_Subject>(k => k.Subject_id, v => v.Subject_name);
                RegistInformation<tb_FM_PayeeInfo>(k => k.PayeeInfoID, v => v.Account_name);

                // 其他基础数据
                RegistInformation<tb_BoxRules>(k => k.BoxRules_ID, v => v.BoxRuleName);
                RegistInformation<tb_CartoonBox>(k => k.CartonID, v => v.CartonName);
                RegistInformation<tb_Files>(k => k.Doc_ID, v => v.FileName);

                // CRM相关
                RegistInformation<tb_CRM_Customer>(k => k.Customer_id, v => v.CustomerName);
                RegistInformation<tb_CRM_Leads>(k => k.LeadID, v => v.CustomerName);
                RegistInformation<tb_CRM_Region>(k => k.Region_ID, v => v.Region_Name);
                RegistInformation<tb_CRM_Contact>(k => k.Contact_id, v => v.Contact_Name);

                // 地理信息
                RegistInformation<tb_Provinces>(k => k.ProvinceID, v => v.ProvinceCNName);
                RegistInformation<tb_Cities>(k => k.CityID, v => v.CityCNName);

                // 配置相关
                RegistInformation<tb_BOMConfigHistory>(k => k.BOM_S_VERID, v => v.VerNo);
                RegistInformation<tb_RolePropertyConfig>(k => k.RolePropertyID, v => v.RolePropertyName);
                RegistInformation<tb_Unit_Conversion>(k => k.UnitConversion_ID, v => v.UnitConversion_Name);

                // 补充缺失的表
                RegistInformation<tb_FieldInfo>(k => k.FieldInfo_ID, v => v.FieldName);
            }
            catch (Exception ex)
            {
                // 初始化表结构信息时的异常处理
                _logger.LogError(ex, "初始化表结构信息时发生错误");
            }
        }

        /// <summary>
        /// 注册表结构信息（通用方法）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="primaryKeyExpression">主键字段表达式</param>
        /// <param name="displayFieldExpression">显示字段表达式</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        private void RegistInformation<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null) where T : class
        {
            _cacheManager.InitializeTableSchema(primaryKeyExpression, displayFieldExpression, isView, isCacheable, description);
        }

        /// <summary>
        /// 初始化指定表的缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>初始化任务</returns>
        private async Task InitializeCacheForTableAsync(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning("表名为空，跳过初始化");
                return;
            }

            try
            {
                _logger.LogDebug($"开始初始化表 {tableName} 的缓存（优化版）");

                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 对应的实体类型");
                    return;
                }

                // 使用信号量控制并发数量
                await _dbSemaphore.WaitAsync();
                try
                {
                    // 使用SqlSugar直接从数据库加载数据
                    // 为每个查询创建独立的数据库上下文，避免连接冲突
                    using (var scopedDb = _unitOfWorkManage.GetDbClient())
                    {
                        await LoadDataAndUpdateCacheAsync(entityType, tableName, scopedDb);
                    }
                }
                finally
                {
                    _dbSemaphore.Release();
                }

                _logger.LogInformation($"表 {tableName} 的缓存初始化完成（优化版）");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化表 {tableName} 的缓存时发生错误（优化版）");
                // 即使出现错误，也要确保缓存中有一个空列表
                try
                {
                    var entityType = _cacheManager.GetEntityType(tableName);
                    if (entityType != null)
                    {
                        var listType = typeof(List<>).MakeGenericType(entityType);
                        var emptyList = Activator.CreateInstance(listType);
                        _cacheManager.UpdateEntityList(tableName, emptyList);
                    }
                }
                catch (Exception cacheEx)
                {
                    _logger.LogError(cacheEx, $"更新缓存失败: {cacheEx.Message}");
                }
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
        private async Task LoadDataAndUpdateCacheAsync(Type entityType, string tableName, ISqlSugarClient db)
        {
            // 检查参数
            if (entityType == null || string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning($"实体类型或表名为空: entityType={entityType}, tableName={tableName}");
                return;
            }

            // 尝试重试机制
            for (int attempt = 0; attempt <= _maxRetryAttempts; attempt++)
            {
                try
                {
                    await LoadDataAndUpdateCacheInternalAsync(entityType, tableName, db);
                    return; // 成功则退出重试循环
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 2 || sqlEx.Message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (attempt < _maxRetryAttempts)
                    {
                        _logger.LogWarning($"表 {tableName} 查询时连接超时，将在 {_retryDelayMs}ms 后进行第 {attempt + 1} 次重试: {sqlEx.Message}");
                        await Task.Delay(_retryDelayMs * (attempt + 1)); // 递增延迟
                    }
                    else
                    {
                        _logger.LogError(sqlEx, $"表 {tableName} 查询重试 {_maxRetryAttempts} 次后仍然失败: {sqlEx.Message}");
                        // 确保缓存中有一个空列表
                        var listType = typeof(List<>).MakeGenericType(entityType);
                        var emptyList = Activator.CreateInstance(listType);
                        _cacheManager.UpdateEntityList(tableName, emptyList);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"从数据库加载 {tableName} 数据并更新缓存时发生错误（优化版）");
                    // 确保缓存中有一个空列表，即使出现错误也要保持缓存一致性
                    try
                    {
                        var listType = typeof(List<>).MakeGenericType(entityType);
                        var emptyList = Activator.CreateInstance(listType);
                        _cacheManager.UpdateEntityList(tableName, emptyList);
                    }
                    catch (Exception cacheEx)
                    {
                        _logger.LogError(cacheEx, $"更新缓存失败: {cacheEx.Message}");
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// 内部实际执行数据加载的方法
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="tableName">表名</param>
        /// <param name="db">数据库客户端</param>
        /// <returns>加载任务</returns>
        private async Task LoadDataAndUpdateCacheInternalAsync(Type entityType, string tableName, ISqlSugarClient db)
        {
            // 检查参数
            if (entityType == null || string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning($"实体类型或表名为空: entityType={entityType}, tableName={tableName}");
                return;
            }

            try
            {
                // 使用SqlSugar直接查询数据库
                var queryable = db.Queryable(entityType.Name, tableName);

                // 执行查询
                var result = queryable.ToList();

                if (result != null && result.Count > 0)
                {
                    // 修复：确保传递给缓存管理器的是正确类型的列表
                    // 创建正确类型的List
                    var listType = typeof(List<>).MakeGenericType(entityType);
                    var typedList = Activator.CreateInstance(listType);

                    // 使用反射获取Add方法并添加元素
                    var addMethod = listType.GetMethod("Add");
                    foreach (var item in result)
                    {
                        // 确保item是正确的类型，如果不是则进行转换
                        object typedItem = item;
                        if (item != null && !entityType.IsInstanceOfType(item))
                        {
                            // 如果item是ExpandoObject，转换为具体类型
                            if (item is System.Dynamic.ExpandoObject)
                            {
                                try
                                {
                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                                    typedItem = Newtonsoft.Json.JsonConvert.DeserializeObject(json, entityType);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, $"转换ExpandoObject到类型 {entityType.Name} 时发生错误");
                                }
                            }
                        }
                        addMethod?.Invoke(typedList, new[] { typedItem });
                    }

                    _cacheManager.UpdateEntityList(tableName, typedList);

                    _logger.LogInformation($"已从数据库加载 {result.Count} 条 {tableName} 记录到缓存（优化版）");
                }
                else
                {
                    _logger.LogInformation($"表 {tableName} 没有数据或数据为空");
                    // 即使没有数据，也要确保缓存中有一个空列表
                    var listType = typeof(List<>).MakeGenericType(entityType);
                    var emptyList = Activator.CreateInstance(listType);
                    _cacheManager.UpdateEntityList(tableName, emptyList);
                }
            }
            catch (SqlException sqlEx) when (sqlEx.Message.Contains("Invalid attempt to read when no data is present"))
            {
                _logger.LogWarning($"表 {tableName} 查询时出现数据读取错误，可能是表不存在或没有数据: {sqlEx.Message}");
                // 确保缓存中有一个空列表
                var listType = typeof(List<>).MakeGenericType(entityType);
                var emptyList = Activator.CreateInstance(listType);
                _cacheManager.UpdateEntityList(tableName, emptyList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"从数据库加载 {tableName} 数据并更新缓存时发生内部错误（优化版）");
                // 确保缓存中有一个空列表，即使出现错误也要保持缓存一致性
                try
                {
                    var listType = typeof(List<>).MakeGenericType(entityType);
                    var emptyList = Activator.CreateInstance(listType);
                    _cacheManager.UpdateEntityList(tableName, emptyList);
                }
                catch (Exception cacheEx)
                {
                    _logger.LogError(cacheEx, $"更新缓存失败: {cacheEx.Message}");
                }
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
        public byte[] SerializeCacheDataForTransmission<T>(T data, CacheSerializationHelper.SerializationType serializationType = CacheSerializationHelper.SerializationType.Json)
        {
            return _cacheManager.SerializeCacheData(data, serializationType);
        }

        /// <summary>
        /// 反序列化从网络接收到的缓存数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">接收到的字节数组</param>
        /// <param name="serializationType">序列化方式</param>
        /// <returns>反序列化后的数据</returns>
        public T DeserializeCacheDataFromTransmission<T>(byte[] data, CacheSerializationHelper.SerializationType serializationType = CacheSerializationHelper.SerializationType.Json)
        {
            return _cacheManager.DeserializeCacheData<T>(data, serializationType);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _dbSemaphore?.Dispose();
        }
    }
}