using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using RUINORERP.Repository.UnitOfWorks;
using System.Linq.Expressions;
using RUINORERP.Model;
using System.Data.SqlClient;
using Newtonsoft.Json;

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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        public EntityCacheInitializationService(
            IUnitOfWorkManage unitOfWorkManage,
            IEntityCacheManager cacheManager,
            ILogger<EntityCacheInitializationService> logger)
        {
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 初始化所有缓存
        /// 简化实现，直接使用框架能力
        /// </summary>
        /// <returns>初始化任务</returns>
        public async Task InitializeAllCacheAsync()
        {
            try
            {
                _logger.Debug("开始初始化缓存服务");
                
                // 初始化所有表结构信息
                InitializeAllTableSchemas();

                // 获取需要缓存的表名
                var tableNames = TableSchemaManager.Instance.GetCacheableTableNamesList();
          

                int successCount = 0;
                int failedCount = 0;
                
                // 逐个初始化表缓存，简单直接
                foreach (var tableName in tableNames)
                {
                    try
                    {
                        // 直接使用同步方法初始化，避免复杂的异步嵌套
                        InitializeCacheForTable(tableName);
                        successCount++;
                        _logger.Debug($"表 {tableName} 缓存初始化成功 ({successCount}/{tableNames.Count})");
                           // 短暂延迟，减少数据库压力
                        await Task.Delay(123);
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogError(ex, $"表 {tableName} 缓存初始化失败 ({failedCount}个表失败)");
                        // 继续处理下一个表
                    }
                }
                
                _logger.LogInformation($"缓存初始化完成: 成功 {successCount} 个表, 失败 {failedCount} 个表");
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

                // 供应商相关
                RegistInformation<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, tableType: TableType.Business);
                RegistInformation<tb_CustomerVendorType>(k => k.Type_ID, v => v.TypeName, tableType: TableType.Base);
                RegistInformation<tb_ProdCategories>(k => k.Category_ID, v => v.Category_name, tableType: TableType.Base);
                RegistInformation<tb_Prod>(k => k.ProdBaseID, v => v.CNName, tableType: TableType.Business);

                // 视图
                RegistInformation<View_ProdDetail>(k => k.ProdDetailID, v => v.CNName, isView: true);
                RegistInformation<View_ProdInfo>(k => k.ProdBaseID, v => v.CNName, isView: true);

                // 产品相关表
                RegistInformation<tb_ProdPropertyType>(k => k.PropertyType_ID, v => v.PropertyTypeName, tableType: TableType.Base);
                RegistInformation<tb_ProdProperty>(k => k.Property_ID, v => v.PropertyName, tableType: TableType.Base);
                RegistInformation<tb_ProdPropertyValue>(k => k.PropertyValueID, v => v.PropertyValueName, tableType: TableType.Base);
                RegistInformation<tb_ProdBundle>(k => k.BundleID, v => v.BundleName, tableType: TableType.Business);
                RegistInformation<tb_Packing>(k => k.Pack_ID, v => v.PackagingName, tableType: TableType.Base);

                // 员工和权限相关
                RegistInformation<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, tableType: TableType.Business);
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
                RegistInformation<tb_FM_Subject>(k => k.Subject_id, v => v.Subject_name, tableType: TableType.Base);
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
                
                _logger.Debug("表结构信息初始化完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化表结构信息时发生错误");
            }
        }

        /// <summary>
        /// 注册表结构信息
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
            // 先初始化表结构
            _cacheManager.InitializeTableSchema(primaryKeyExpression, displayFieldExpression, isView, isCacheable, description, true, otherDisplayFieldExpressions);
            
            // 设置表类型
            string tableName = typeof(T).Name;
            var schemaInfo = TableSchemaManager.Instance.GetSchemaInfo(tableName);
            if (schemaInfo != null)
            {
                schemaInfo.Type = tableType;
            }
        }

        /// <summary>
        /// 初始化指定表的缓存
        /// 同步方法，避免异步嵌套问题
        /// </summary>
        private void InitializeCacheForTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning("表名为空，跳过初始化");
                return;
            }

            try
            {
                _logger.LogDebug($"开始初始化表 {tableName} 的缓存");

                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 对应的实体类型");
                    return;
                }

                // 移除using块，让SqlSugar自己管理连接生命周期
                var db = _unitOfWorkManage.GetDbClient();
                // 简单直接地加载数据
                LoadDataAndUpdateCache(entityType, tableName, db);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化表 {tableName} 的缓存时发生错误");
                
                // 确保缓存中有一个空列表
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
        /// 实现：根据缓存策略决定加载整行数据或只加载指定字段
        /// </summary>
        private void LoadDataAndUpdateCache(Type entityType, string tableName, ISqlSugarClient db)
        {
            if (entityType == null || string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning($"实体类型或表名为空: entityType={entityType}, tableName={tableName}");
                return;
            }

            try
            {
                // 获取表结构信息，特别是需要缓存的字段
                var schemaInfo = TableSchemaManager.Instance.GetSchemaInfo(tableName);
                if (schemaInfo == null)
                {
                    _logger.LogWarning($"未找到表 {tableName} 的结构信息");
                    // 创建正确类型的List
                    var nullCaseListType = typeof(List<>).MakeGenericType(entityType);
                    var nullCaseTypedList = Activator.CreateInstance(nullCaseListType);
                    _cacheManager.UpdateEntityList(tableName, nullCaseTypedList);
                    return;
                }

                // 创建正确类型的List
                var listType = typeof(List<>).MakeGenericType(entityType);
                var typedList = Activator.CreateInstance(listType);
                var addMethod = listType.GetMethod("Add");

                // 检查是否缓存整行数据
                bool cacheWholeRow = schemaInfo.GetType().GetProperty("CacheWholeRow")?.GetValue(schemaInfo, null) as bool? ?? false;
                _logger.LogDebug($"开始加载表 {tableName} 数据，缓存策略：{(cacheWholeRow ? "缓存整行" : "只缓存指定字段")}");
                
                dynamic result;
                
                // 根据缓存策略决定查询方式
                if (cacheWholeRow)
                {
                    // 缓存整行数据：查询所有字段
                    _logger.LogDebug($"查询策略：缓存整行数据");
                    result = db.Queryable(entityType.Name, tableName).ToList();
                }
                else
                {
                    // 只缓存指定字段：查询主键字段和所有显示字段
                    var fieldsToSelect = new List<string> { schemaInfo.PrimaryKeyField };
                    fieldsToSelect.AddRange(schemaInfo.DisplayFields.Except(new[] { schemaInfo.PrimaryKeyField }));
                    fieldsToSelect = fieldsToSelect.Distinct().ToList();
                    
                    _logger.LogDebug($"查询策略：只缓存指定字段，表 {tableName} 要加载的字段: {string.Join(", ", fieldsToSelect)}");
                    
                    // 使用SqlSugar的动态查询，直接ToList()获取数据
                    result = db.Queryable(entityType.Name, tableName).ToList();
                }

                if (result != null && System.Linq.Enumerable.Count(result) > 0)
                {
                    int resultCount = System.Linq.Enumerable.Count(result);
                    _logger.LogDebug($"已加载表 {tableName} 共 {resultCount} 条数据");

                    // 添加到列表中
                    foreach (var item in result)
                    {
                        try
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
                                        var json = JsonConvert.SerializeObject(item);
                                        typedItem = JsonConvert.DeserializeObject(json, entityType);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogWarning(ex, $"转换ExpandoObject到类型 {entityType.Name} 时发生错误");
                                    }
                                }
                            }
                            addMethod?.Invoke(typedList, new[] { typedItem });
                        }
                        catch (Exception itemEx)
                        {
                            _logger.LogWarning(itemEx, $"添加数据项到列表时发生错误: {itemEx.Message}");
                            // 继续处理下一个项
                        }
                    }
                }

                // 完成加载后更新缓存
                _cacheManager.UpdateEntityList(tableName, typedList);
                _logger.LogDebug($"表 {tableName} 缓存更新完成");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, $"从数据库加载 {tableName} 数据时发生SQL错误");
                // 确保缓存中有一个空列表
                var listType = typeof(List<>).MakeGenericType(entityType);
                var emptyList = Activator.CreateInstance(listType);
                _cacheManager.UpdateEntityList(tableName, emptyList);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"从数据库加载 {tableName} 数据并更新缓存时发生错误");
                // 确保缓存中有一个空列表
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
        /// 初始化指定表的缓存（公共方法）
        /// </summary>
        public Task InitializeSingleTableCacheAsync(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning("表名为空，跳过初始化");
                return Task.CompletedTask;
            }

            try
            {
                InitializeCacheForTable(tableName);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化表 {tableName} 的缓存时发生错误");
                return Task.FromException(ex);
            }
        }

        /// <summary>
        /// 初始化指定类型的表缓存
        /// </summary>
        /// <param name="tableType">表类型</param>
        public async Task InitializeTablesByTypeAsync(TableType tableType)
        {
            try
            {
                _logger.Debug($"开始初始化类型为 {tableType} 的表缓存");
                var tables = TableSchemaManager.Instance.GetCacheableTableNamesByType(tableType);
                
                int successCount = 0;
                int failedCount = 0;
                
                foreach (var tableName in tables)
                {
                    try
                    {
                        InitializeCacheForTable(tableName);
                        successCount++;
                        _logger.Debug($"表 {tableName} 缓存初始化成功 ({successCount}/{tables.Count})");
                        // 短暂延迟，减少数据库压力
                        await Task.Delay(123);
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogError(ex, $"表 {tableName} 缓存初始化失败 ({failedCount}个表失败)");
                    }
                }
                
                _logger.Debug($"类型 {tableType} 的表缓存初始化完成: 成功 {successCount} 个表, 失败 {failedCount} 个表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化类型为 {tableType} 的表缓存时发生错误");
            }
        }

        /// <summary>
        /// 初始化基础业务表缓存（系统启动或用户登录时通常需要加载）
        /// </summary>
        public async Task InitializeBaseBusinessTablesAsync()
        {
            try
            {
                _logger.Debug("开始初始化基础业务表缓存");
                var tables = TableSchemaManager.Instance.GetBaseBusinessTableNames();
                
                int successCount = 0;
                int failedCount = 0;
                
                foreach (var tableName in tables)
                {
                    try
                    {
                        InitializeCacheForTable(tableName);
                        successCount++;
                        _logger.Debug($"基础业务表 {tableName} 缓存初始化成功 ({successCount}/{tables.Count})");
                        // 短暂延迟，减少数据库压力
                        await Task.Delay(123);
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogError(ex, $"基础业务表 {tableName} 缓存初始化失败 ({failedCount}个表失败)");
                    }
                }
                
                _logger.Debug($"基础业务表缓存初始化完成: 成功 {successCount} 个表, 失败 {failedCount} 个表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化基础业务表缓存时发生错误");
            }
        }
    }
}