using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 简化版缓存初始化服务
    /// 减少不必要的控制和嵌套，直接使用SqlSugar和_unitOfWorkManage的内置管理能力
    /// </summary>
    public class EntityCacheInitializationService
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly IEntityCacheManager _cacheManager;
        private readonly ILogger<EntityCacheInitializationService> _logger;
        private readonly ICacheSyncMetadata _cacheSyncMetadata;
        private readonly DynamicQueryHelper _queryHelper;
        private readonly ITableSchemaManager _tableSchemaManager; // 唯一表结构管理入口

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        public EntityCacheInitializationService(
            IUnitOfWorkManage unitOfWorkManage,
            IEntityCacheManager cacheManager,
            ICacheSyncMetadata cacheSyncMetadata,
            DynamicQueryHelper queryHelper,
            ITableSchemaManager tableSchemaManager,
        ILogger<EntityCacheInitializationService> logger)
        {
            _cacheSyncMetadata = cacheSyncMetadata ?? throw new ArgumentNullException(nameof(cacheSyncMetadata));
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _queryHelper = queryHelper;
            _tableSchemaManager = tableSchemaManager ?? throw new ArgumentNullException(nameof(tableSchemaManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 初始化所有缓存（性能优化版本：并行加载）
        /// 简化实现，直接使用框架能力
        /// 确保基础表缓存信息也被正确初始化和管理
        /// </summary>
        /// <returns>初始化任务</returns>
        public async Task InitializeAllCacheAsync()
        {
            try
            {
                _logger.Debug("开始初始化缓存服务（并行优化版本）");

                // 初始化所有表结构信息
                InitializeAllTableSchemas();

                // 获取需要缓存的表名
                var tableNames = _tableSchemaManager.GetCacheableTableNamesList();

                int successCount = 0;
                int failedCount = 0;
                var failedTables = new ConcurrentBag<string>();
                ConcurrentBag<string> retryTables = new ConcurrentBag<string>();

                // ✅ 优化：限制并行度为连接池大小的50%，避免连接池耗尽
                var maxParallelDegree = Math.Max(1, Math.Min(4, Environment.ProcessorCount / 2));
                
                // 使用并行处理提高性能
                var options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = maxParallelDegree
                };

                _logger.Debug($"使用并行加载，最大并行度: {options.MaxDegreeOfParallelism}，表总数: {tableNames.Count}");

                // 使用并行循环处理缓存初始化
                await Task.Run(() =>
                {
                    Parallel.ForEach(tableNames, options, tableName =>
                    {
                        try
                        {
                            // 同步初始化（在并行线程中）
                            InitializeCacheForTable(tableName);
                            Interlocked.Increment(ref successCount);
                            _logger.Debug($"表 {tableName} 缓存初始化成功 ({successCount}/{tableNames.Count})");
                        }
                        catch (Exception ex)
                        {
                            Interlocked.Increment(ref failedCount);
                            failedTables.Add(tableName);
                            _logger.LogError(ex, $"表 {tableName} 缓存初始化失败 ({failedCount}个表失败)");
                        }
                    });
                });

                // ✅ 优化：对失败的表进行重试（单线程，最多重试2次）
                if (failedTables.Any())
                {
                    _logger.LogWarning($"有 {failedTables.Count} 个表初始化失败，开始重试...");
                    
                    int retrySuccessCount = 0;
                    const int maxRetryAttempts = 2;
                    var tablesToRetry = failedTables.ToList();
                    
                    for (int retryAttempt = 1; retryAttempt <= maxRetryAttempts; retryAttempt++)
                    {
                        var stillFailedTables = new ConcurrentBag<string>();
                        
                        foreach (var tableName in tablesToRetry)
                        {
                            try
                            {
                                InitializeCacheForTable(tableName);
                                retrySuccessCount++;
                                _logger.Debug($"表 {tableName} 第 {retryAttempt} 次重试成功");
                            }
                            catch (Exception ex)
                            {
                                stillFailedTables.Add(tableName);
                                _logger.Debug(ex, $"表 {tableName} 第 {retryAttempt} 次重试失败");
                                
                                if (retryAttempt == maxRetryAttempts)
                                {
                                    _logger.Error(ex, $"表 {tableName} 重试 {maxRetryAttempts} 次后仍然失败，跳过该表");
                                }
                            }
                        }
                        
                        tablesToRetry = stillFailedTables.ToList();
                        
                        if (!tablesToRetry.Any())
                        {
                            break;
                        }
                    }
                    
                    if (retrySuccessCount > 0)
                    {
                        _logger.LogInformation($"重试成功：{retrySuccessCount} 个表");
                        successCount += retrySuccessCount;
                        failedCount -= retrySuccessCount;
                    }
                }

                _logger.LogDebug($"缓存初始化完成: 成功 {successCount} 个表, 失败 {failedCount} 个表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化所有缓存时发生严重错误");
            }
        }

        /// <summary>
        /// 初始化所有表结构信息
        /// </summary>
        public void InitializeAllTableSchemas()
        {
            try
            {
                _logger.Debug("开始初始化表结构信息");

                // 注册所有需要缓存的表结构信息
                // 基础数据表
                RegistInformation<tb_Company>(k => k.ID, v => v.CNName, tableType: TableType.Base);
                RegistInformation<tb_Currency>(k => k.Currency_ID, v => v.CurrencyName, tableType: TableType.Base);
                RegistInformation<tb_CurrencyExchangeRate>(k => k.ExchangeRateID, v => v.ConversionName, tableType: TableType.Base);
                RegistInformation<tb_BOM_S>(k => k.BOM_ID, v => v.BOM_Name, tableType: TableType.Base);
                RegistInformation<tb_ProductType>(k => k.Type_ID, v => v.TypeName, tableType: TableType.Base);
                RegistInformation<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, tableType: TableType.Base);
                RegistInformation<tb_Unit>(k => k.Unit_ID, v => v.UnitName, tableType: TableType.Base);
                RegistInformation<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, tableType: TableType.Base);
                RegistInformation<tb_LocationType>(k => k.LocationType_ID, v => v.TypeName, tableType: TableType.Base);
                RegistInformation<tb_Location>(k => k.Location_ID, v => v.Name, tableType: TableType.Base);

                // 供应商相关 - 开启按需缓存优化
                RegistInformation<tb_CustomerVendor>(k => k.CustomerVendor_ID, false, true, null, TableType.Business, false, v => v.CVName, v => v.CVCode, v => v.IsCustomer, v => v.IsVendor, v => v.IsOther, v => v.IsExclusive);
                RegistInformation<tb_CustomerVendorType>(k => k.Type_ID, v => v.TypeName, tableType: TableType.Base);
                RegistInformation<tb_ProdCategories>(k => k.Category_ID, v => v.Category_name, tableType: TableType.Base);

                // 产品相关 - 重点优化：仅缓存 ID, 名称, 品号, 规格
                RegistInformation<tb_Prod>(k => k.ProdBaseID, false, true, null, TableType.Business, false, v => v.CNName, v => v.ProductNo, v => v.Specifications);

                // 视图
                RegistInformation<View_ProdDetail>(k => k.ProdDetailID, true, true, null, TableType.Other, false, v => v.CNName, v => v.ProductNo);
                RegistInformation<View_ProdInfo>(k => k.ProdBaseID, true, true, null, TableType.Other, false, v => v.CNName, v => v.ProductNo);


                RegistInformation<tb_ProdProperty>(k => k.Property_ID, v => v.PropertyName, tableType: TableType.Base);
                RegistInformation<tb_ProdPropertyValue>(k => k.PropertyValueID, v => v.PropertyValueName, tableType: TableType.Base);
                RegistInformation<tb_ProdBundle>(k => k.BundleID, v => v.BundleName, tableType: TableType.Business);
                RegistInformation<tb_Packing>(k => k.Pack_ID, v => v.PackagingName, tableType: TableType.Base);

                // 员工和权限相关
                //RegistInformation<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, tableType: TableType.Business);
                RegistInformation<tb_Employee>(k => k.Employee_ID, true, true, null, TableType.Business, false, v => v.Employee_Name, v => v.Employee_NO,  v => v.Is_enabled, v => v.DepartmentID);
                RegistInformation<tb_UserInfo>(k => k.User_ID, v => v.UserName, tableType: TableType.Base);
                RegistInformation<tb_RoleInfo>(k => k.RoleID, v => v.RoleName, tableType: TableType.Base);
                RegistInformation<tb_MenuInfo>(k => k.MenuID, v => v.MenuName, tableType: TableType.Base);
                RegistInformation<tb_ModuleDefinition>(k => k.ModuleID, v => v.ModuleName, tableType: TableType.Base);

                // 业务类型
                RegistInformation<tb_BizType>(k => k.Type_ID, v => v.TypeName, tableType: TableType.Base);
                RegistInformation<tb_StorageRack>(k => k.Rack_ID, v => v.RackName, tableType: TableType.Base);
                RegistInformation<tb_OutInStockType>(k => k.Type_ID, v => v.TypeName, tableType: TableType.Base);
                RegistInformation<tb_OnlineStoreInfo>(k => k.Store_ID, v => v.StoreName, tableType: TableType.Base);
                RegistInformation<tb_ProjectGroup>(k => k.ProjectGroup_ID, v => v.ProjectGroupName, tableType: TableType.Business);

                // 财务相关
                RegistInformation<tb_FM_Account>(k => k.Account_id, v => v.Account_name, tableType: TableType.Base);
                RegistInformation<tb_FM_ExpenseType>(k => k.ExpenseType_id, v => v.Expense_name, tableType: TableType.Base);
                RegistInformation<tb_FM_Subject>(k => k.Subject_id, v => v.subject_name, tableType: TableType.Base);
                RegistInformation<tb_FM_PayeeInfo>(k => k.PayeeInfoID, v => v.Account_name, tableType: TableType.Base);

                // 其他基础数据
                RegistInformation<tb_BoxRules>(k => k.BoxRules_ID, v => v.BoxRuleName, tableType: TableType.Base);
                RegistInformation<tb_CartoonBox>(k => k.CartonID, v => v.CartonName, tableType: TableType.Base);
                RegistInformation<tb_Files>(k => k.Doc_ID, v => v.FileName, tableType: TableType.Business);

                // CRM相关
                RegistInformation<tb_CRM_Customer>(k => k.Customer_id, v => v.CustomerName, tableType: TableType.Business);
                RegistInformation<tb_CRM_Leads>(k => k.LeadID, v => v.CustomerName, tableType: TableType.Business);
                RegistInformation<tb_CRM_Region>(k => k.Region_ID, v => v.Region_Name, tableType: TableType.Base);
                RegistInformation<tb_CRM_Contact>(k => k.Contact_id, v => v.Contact_Name, tableType: TableType.Business);

                // 地理信息
                RegistInformation<tb_Provinces>(k => k.ProvinceID, v => v.ProvinceCNName, tableType: TableType.Base);
                RegistInformation<tb_Cities>(k => k.CityID, v => v.CityCNName, tableType: TableType.Base);

                // 配置相关
                RegistInformation<tb_BOMConfigHistory>(k => k.BOM_S_VERID, v => v.VerNo, tableType: TableType.Business);
                RegistInformation<tb_RolePropertyConfig>(k => k.RolePropertyID, v => v.RolePropertyName, tableType: TableType.Base);
                RegistInformation<tb_Unit_Conversion>(k => k.UnitConversion_ID, v => v.UnitConversion_Name, tableType: TableType.Base);

                // 补充缺失的表
                RegistInformation<tb_FieldInfo>(k => k.FieldInfo_ID, v => v.FieldName, tableType: TableType.Base);
                RegistInformation<tb_ButtonInfo>(k => k.ButtonInfo_ID, v => v.BtnName, tableType: TableType.Base);

                // 添加调试信息，跟踪初始化完成情况
                _logger.Debug("表结构信息初始化完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化表结构信息时发生错误");
            }
        }

        /// <summary>
        /// 注册表结构信息（增强版：支持多字段投影配置）
        /// </summary>
        /// <param name="primaryKeyExpression">主键字段</param>
        /// <param name="isView">是否为视图</param>
        /// <param name="isCacheable">是否参与缓存</param>
        /// <param name="description">描述信息</param>
        /// <param name="tableType">表类型</param>
        /// <param name="cacheWholeRow">是否缓存整行（false 则仅缓存下方指定的字段）</param>
        /// <param name="displayFieldExpressions">需要缓存的字段列表（ID, Name, Code 等）</param>
        private void RegistInformation<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            TableType tableType = TableType.Other,
            bool cacheWholeRow = true,
            params Expression<Func<T, object>>[] displayFieldExpressions) where T : class
        {
            if (displayFieldExpressions == null || displayFieldExpressions.Length == 0)
            {
                throw new ArgumentException("至少需要指定一个显示字段（通常是 Name 或 Code）");
            }

            // 第一个参数作为主显示字段，其余作为辅助显示字段
            var mainDisplay = displayFieldExpressions[0];
            var otherDisplays = displayFieldExpressions.Skip(1).ToArray();

            _tableSchemaManager.RegisterTableSchema(
                primaryKeyExpression,
                mainDisplay,
                isView,
                isCacheable,
                description,
                cacheWholeRow, // 关键：控制是否开启按需缓存
                otherDisplays);

            // 设置表类型
            string tableName = typeof(T).Name;
            var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
            if (schemaInfo != null)
            {
                schemaInfo.Type = tableType;
            }
        }

        /// <summary>
        /// 注册表结构信息（兼容旧版本：默认缓存整行）
        /// </summary>
        private void RegistInformation<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            TableType tableType = TableType.Other,
            params Expression<Func<T, object>>[] otherDisplayFieldExpressions) where T : class
        {
            RegistInformation(
                primaryKeyExpression,
                isView,
                isCacheable,
                description,
                tableType,
                true, // 默认全量缓存
                new[] { displayFieldExpression }.Concat(otherDisplayFieldExpressions ?? Array.Empty<Expression<Func<T, object>>>()).ToArray()
            );
        }

        /// <summary>
        /// 创建空列表缓存的辅助方法
        /// 用于异常处理时确保缓存有有效状态
        /// </summary>
        private void CreateEmptyCacheForTable(string tableName)
        {
            try
            {
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType != null)
                {
                    var listType = typeof(List<>).MakeGenericType(entityType);
                    var emptyList = Activator.CreateInstance(listType);
                    _cacheManager.UpdateEntityList(tableName, emptyList);
                    _logger.LogDebug($"为表 {tableName} 创建了空缓存列表");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"为表 {tableName} 创建空缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 初始化指定表的缓存
        /// 同步方法，避免异步嵌套问题
        /// </summary>
        public void InitializeCacheForTable(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    _logger.LogWarning("表名为空，跳过初始化");
                    return;
                }

                _logger.LogDebug($"开始初始化表 {tableName} 的缓存");

                // 添加调试信息：检查TableSchemaManager状态
                System.Diagnostics.Debug.WriteLine($"[InitializeCacheForTable] TableSchemaManager实例ID: {_tableSchemaManager.GetHashCode()}");
                System.Diagnostics.Debug.WriteLine($"[InitializeCacheForTable] TableSchemaManager已注册表数: {_tableSchemaManager.GetAllTableNames().Count}");
                System.Diagnostics.Debug.WriteLine($"[InitializeCacheForTable] 已注册的表: {string.Join(", ", _tableSchemaManager.GetAllTableNames())}");
                System.Diagnostics.Debug.WriteLine($"[InitializeCacheForTable] 尝试获取表: {tableName}");

                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 对应的实体类型");
                    System.Diagnostics.Debug.WriteLine($"[InitializeCacheForTable] GetEntityType返回null, 表名: {tableName}");
                    System.Diagnostics.Debug.WriteLine($"[InitializeCacheForTable] 直接从TableSchemaManager查询: {_tableSchemaManager.GetEntityType(tableName)}");
                    return;
                }



                // 简单直接地加载数据
                // 注意：LoadDataAndUpdateCache方法内部通过_cacheManager.UpdateEntityList已经更新了缓存同步元数据
                // UpdateEntityList方法会通过UpdateCacheSyncMetadataAfterEntityChange间接调用UpdateTableSyncInfo
                LoadDataAndUpdateCache(entityType, tableName, _queryHelper);




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化表 {tableName} 的缓存时发生错误");

                // 使用统一的空缓存创建方法，不再重新抛出异常，确保状态一致性
                CreateEmptyCacheForTable(tableName);
            }
        }

        /// <summary>
        /// 异步初始化指定表的缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public async Task InitializeCacheForTableAsync(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    _logger.LogWarning("表名为空，跳过异步初始化");
                    return;
                }

                _logger.LogDebug($"开始异步初始化表 {tableName} 的缓存");

                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 对应的实体类型");
                    return;
                }

                // 异步加载数据并更新缓存
                await LoadDataAndUpdateCacheAsync(entityType, tableName, _queryHelper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步初始化表 {tableName} 的缓存时发生错误");

                // 使用统一的空缓存创建方法，不再重新抛出异常，确保状态一致性
                CreateEmptyCacheForTable(tableName);
            }
        }

        /// <summary>
        /// 从数据库加载数据并更新缓存
        /// 实现：根据缓存策略决定加载整行数据或只加载指定字段
        /// </summary>
        private void LoadDataAndUpdateCache(Type entityType, string tableName, DynamicQueryHelper _queryHelper)
        {
            if (entityType == null || string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning($"实体类型或表名为空: entityType={entityType}, tableName={tableName}");
                return;
            }

            var list = _queryHelper.QueryAll(tableName, entityType);

            try
            {
                // 获取表结构信息，特别是需要缓存的字段
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                if (schemaInfo == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 的结构信息");
                    // 创建正确类型的List
                    var nullCaseListType = typeof(List<>).MakeGenericType(entityType);
                    var nullCaseTypedList = Activator.CreateInstance(nullCaseListType);
                    _cacheManager.UpdateEntityList(tableName, nullCaseTypedList);
                    return;
                }
                // 完成加载后更新缓存
                _cacheManager.UpdateEntityList(tableName, list);
                _logger.LogDebug($"表 {tableName} 缓存更新完成");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, $"从数据库加载 {tableName} 数据时发生SQL错误");
                // 使用统一的空缓存创建方法，不再重新抛出异常，确保状态一致性
                CreateEmptyCacheForTable(tableName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"从数据库加载 {tableName} 数据并更新缓存时发生错误");
                // 使用统一的空缓存创建方法，不再重新抛出异常，确保状态一致性
                CreateEmptyCacheForTable(tableName);
            }
        }

        /// <summary>
        /// 异步从数据库加载数据并更新缓存
        /// </summary>
        private async Task LoadDataAndUpdateCacheAsync(Type entityType, string tableName, DynamicQueryHelper _queryHelper)
        {
            if (entityType == null || string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning($"实体类型或表名为空: entityType={entityType}, tableName={tableName}");
                return;
            }

            try
            {
                // 获取表结构信息，特别是需要缓存的字段
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                if (schemaInfo == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 的结构信息");
                    // 创建正确类型的List
                    var nullCaseListType = typeof(List<>).MakeGenericType(entityType);
                    var nullCaseTypedList = Activator.CreateInstance(nullCaseListType);
                    _cacheManager.UpdateEntityList(tableName, nullCaseTypedList);
                    return;
                }

                // 使用反射调用EntityCacheManager的异步方法
                var getEntityListAsyncMethod = typeof(EntityCacheManager)
                    .GetMethod("GetEntityListAsync", new[] { typeof(string) })
                    .MakeGenericMethod(entityType);

                if (getEntityListAsyncMethod != null)
                {
                    // 调用异步方法加载数据
                    var task = (Task)getEntityListAsyncMethod.Invoke(_cacheManager, new object[] { tableName });
                    if (task != null)
                    {
                        await task;
                        _logger.LogDebug($"表 {tableName} 异步缓存更新完成");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, $"异步从数据库加载 {tableName} 数据时发生SQL错误");
                // 使用统一的空缓存创建方法，不再重新抛出异常，确保状态一致性
                CreateEmptyCacheForTable(tableName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步从数据库加载 {tableName} 数据并更新缓存时发生错误");
                // 使用统一的空缓存创建方法，不再重新抛出异常，确保状态一致性
                CreateEmptyCacheForTable(tableName);
            }
        }


        /// <summary>
        /// 通用表缓存初始化方法
        /// 提取InitializeTablesByTypeAsync和InitializeBaseBusinessTablesAsync的公共逻辑
        /// </summary>
        /// <param name="tables">要初始化的表名列表</param>
        /// <param name="operationName">操作名称（用于日志）</param>
        private async Task InitializeTablesAsync(IEnumerable<string> tables, string operationName)
        {
            try
            {
                _logger.Debug($"开始{operationName}");
                var tableList = tables.ToList();

                int successCount = 0;
                int failedCount = 0;

                foreach (var tableName in tableList)
                {
                    try
                    {
                        InitializeCacheForTable(tableName);
                        successCount++;
                        _logger.Debug($"{operationName} - 表 {tableName} 缓存初始化成功 ({successCount}/{tableList.Count})");
                        // 短暂延迟，减少数据库压力
                        await Task.Delay(123);
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogError(ex, $"{operationName} - 表 {tableName} 缓存初始化失败 ({failedCount}个表失败)");
                    }
                }

                _logger.Debug($"{operationName}完成: 成功 {successCount} 个表, 失败 {failedCount} 个表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{operationName}时发生错误");
            }
        }

        /// <summary>
        /// 初始化指定类型的表缓存
        /// </summary>
        /// <param name="tableType">表类型</param>
        public async Task InitializeTablesByTypeAsync(TableType tableType)
        {
            var tables = _tableSchemaManager.GetCacheableTableNamesByType(tableType);
            await InitializeTablesAsync(tables, $"初始化类型为 {tableType} 的表缓存");
        }

        /// <summary>
        /// 初始化基础业务表缓存（系统启动或用户登录时通常需要加载）
        /// </summary>
        public async Task InitializeBaseBusinessTablesAsync()
        {

            var tables = _tableSchemaManager.GetBaseBusinessTableNames();
            await InitializeTablesAsync(tables, "初始化基础业务表缓存");
        }


        /// <summary>
        /// 更新基础表缓存信息,即更新元数据信息
        /// 完善的元数据更新机制，添加重试和验证恢复能力
        /// </summary>
        /// <param name="tableName">表名</param>
        private void UpdateBaseTableCacheInfo(string tableName)
        {
            try
            {
                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 对应的实体类型，无法更新缓存信息");
                    return;
                }

                // 获取实际缓存的实体列表以更新元数据
                var entityList = _cacheManager.GetEntityList<object>(tableName);
                int dataCount = entityList?.Count ?? 0;

                // 直接使用缓存同步元数据管理器更新元数据
                // 显式传递数据数量，确保元数据正确更新
                _cacheSyncMetadata.UpdateTableSyncInfo(tableName, dataCount);



                // 验证元数据完整性
                bool isValid = _cacheSyncMetadata.ValidateTableCacheIntegrity(tableName);
                if (!isValid)
                {
                    // 验证失败时尝试恢复：重新加载实体列表并更新元数据
                    _logger.LogWarning("表 {TableName} 的缓存元数据验证失败，尝试恢复", tableName);
                    try
                    {
                        // 重新从数据库加载数据以确保元数据准确性
                        var db = _unitOfWorkManage.GetDbClient();
                        if (entityType != null)
                        {
                            LoadDataAndUpdateCache(entityType, tableName, _queryHelper);
                            // 重新获取并更新元数据
                            entityList = _cacheManager.GetEntityList<object>(tableName);
                            dataCount = entityList?.Count ?? 0;
                            _cacheSyncMetadata.UpdateTableSyncInfo(tableName, dataCount);

                            // 重新验证
                            isValid = _cacheSyncMetadata.ValidateTableCacheIntegrity(tableName);
                            if (isValid)
                            {
                                _logger.LogDebug("表 {TableName} 的缓存元数据恢复成功", tableName);
                            }
                        }
                    }
                    catch (Exception recoveryEx)
                    {
                        _logger.LogError(recoveryEx, "恢复表 {TableName} 的缓存元数据失败", tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新表 {TableName} 的缓存元数据时发生错误", tableName);

                // 异常发生时尝试移除元数据，确保系统状态一致性
                try
                {
                    _cacheSyncMetadata.RemoveTableSyncInfo(tableName);
                    _logger.LogDebug("表 {TableName} 的缓存元数据已移除", tableName);
                }
                catch (Exception resetEx)
                {
                    _logger.LogError(resetEx, "移除表 {TableName} 的缓存元数据失败", tableName);
                }
            }
        }
    }
}