// **************************************
// 文件：EntityRegistry.cs
// 项目：RUINORERP.Model
// 作者：AI Assistant
// 时间：2026-04-21
// 描述：实体注册中心，提供结构化的实体映射和统一查询方法
// **************************************

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    /// <summary>
    /// 实体注册中心
    /// 提供结构化的实体映射、分类管理和统一查询方法
    /// </summary>
    public static class EntityRegistry
    {
        /// <summary>
        /// 所有实体的元数据列表
        /// </summary>
        public static readonly List<EntityMetadata> Entities = new List<EntityMetadata>();

        /// <summary>
        /// 所有分类列表（去重）
        /// </summary>
        public static readonly List<string> Categories = new List<string>();

        /// <summary>
        /// 静态构造函数：初始化实体注册表
        /// </summary>
        static EntityRegistry()
        {
            InitializeEntities();
            Categories = Entities.Select(e => e.Category).Distinct().OrderBy(c => c).ToList();
        }

        /// <summary>
        /// 初始化实体映射
        /// </summary>
        private static void InitializeEntities()
        {
            // ==================== 基础主数据 ====================
            
            // 客户与供应商
            Entities.Add(CreateMetadata("基础主数据", "客户/供应商", typeof(tb_CustomerVendor), "客户厂商表"));

            // 产品相关
            Entities.Add(CreateMetadata("基础主数据", "产品类目", typeof(tb_ProdCategories), "产品类目表"));
            Entities.Add(CreateMetadata("基础主数据", "产品基本信息", typeof(tb_Prod), "货品基本信息表"));
            Entities.Add(CreateMetadata("基础主数据", "产品详情", typeof(tb_ProdDetail), "产品详情信息表"));
            Entities.Add(CreateMetadata("基础主数据", "产品属性", typeof(tb_ProdProperty), "产品属性表"));
            Entities.Add(CreateMetadata("基础主数据", "产品属性值", typeof(tb_ProdPropertyValue), "产品属性值表"));
            Entities.Add(CreateMetadata("基础主数据", "产品类型", typeof(tb_ProductType), "产品类型表"));

            // 库存位置
            Entities.Add(CreateMetadata("基础主数据", "库位", typeof(tb_Location), "库位表"));
            Entities.Add(CreateMetadata("基础主数据", "货架", typeof(tb_StorageRack), "货架表"));

            // 单位
            Entities.Add(CreateMetadata("基础主数据", "单位", typeof(tb_Unit), "单位表"));

            // 组织架构
            Entities.Add(CreateMetadata("基础主数据", "部门", typeof(tb_Department), "部门表"));
            Entities.Add(CreateMetadata("基础主数据", "员工", typeof(tb_Employee), "员工表"));
            Entities.Add(CreateMetadata("基础主数据", "公司", typeof(tb_Company), "公司表"));
            Entities.Add(CreateMetadata("基础主数据", "职位", typeof(tb_Position), "职位表"));

            // CRM相关
            Entities.Add(CreateMetadata("基础主数据", "CRM客户", typeof(tb_CRM_Customer), "CRM客户表"));
            Entities.Add(CreateMetadata("基础主数据", "CRM联系人", typeof(tb_CRM_Contact), "CRM联系人表"));
            Entities.Add(CreateMetadata("基础主数据", "CRM线索", typeof(tb_CRM_Leads), "CRM线索表"));

            // ==================== 业务单据 - 销售模块 ====================
            Entities.Add(CreateMetadata("业务单据", "销售订单", typeof(tb_SaleOrder), "销售订单"));
            Entities.Add(CreateMetadata("业务单据", "销售订单明细", typeof(tb_SaleOrderDetail), "销售订单明细"));
            Entities.Add(CreateMetadata("业务单据", "销售出库", typeof(tb_SaleOut), "销售出库单"));
            Entities.Add(CreateMetadata("业务单据", "销售出库明细", typeof(tb_SaleOutDetail), "销售出库明细"));
            Entities.Add(CreateMetadata("业务单据", "销售退货", typeof(tb_SaleOutRe), "销售退货单"));
            Entities.Add(CreateMetadata("业务单据", "销售退货明细", typeof(tb_SaleOutReDetail), "销售退货明细"));
            Entities.Add(CreateMetadata("业务单据", "销售合同", typeof(tb_SO_Contract), "销售合同"));
            Entities.Add(CreateMetadata("业务单据", "销售合同明细", typeof(tb_SO_ContractDetail), "销售合同明细"));

            // ==================== 业务单据 - 采购模块 ====================
            Entities.Add(CreateMetadata("业务单据", "采购订单", typeof(tb_PurOrder), "采购订单"));
            Entities.Add(CreateMetadata("业务单据", "采购订单明细", typeof(tb_PurOrderDetail), "采购订单明细"));
            Entities.Add(CreateMetadata("业务单据", "采购入库", typeof(tb_PurEntry), "采购入库单"));
            Entities.Add(CreateMetadata("业务单据", "采购入库明细", typeof(tb_PurEntryDetail), "采购入库明细"));
            Entities.Add(CreateMetadata("业务单据", "采购退货", typeof(tb_PurReturnEntry), "采购退货单"));
            Entities.Add(CreateMetadata("业务单据", "采购退货明细", typeof(tb_PurReturnEntryDetail), "采购退货明细"));
            Entities.Add(CreateMetadata("业务单据", "采购申请", typeof(tb_BuyingRequisition), "采购申请表"));
            Entities.Add(CreateMetadata("业务单据", "采购申请明细", typeof(tb_BuyingRequisitionDetail), "采购申请明细"));
            Entities.Add(CreateMetadata("业务单据", "采购合同", typeof(tb_PO_Contract), "采购合同"));
            Entities.Add(CreateMetadata("业务单据", "采购合同明细", typeof(tb_PO_ContractDetail), "采购合同明细"));

            // ==================== 业务单据 - 库存模块 ====================
            Entities.Add(CreateMetadata("业务单据", "入库单", typeof(tb_StockIn), "入库单"));
            Entities.Add(CreateMetadata("业务单据", "入库单明细", typeof(tb_StockInDetail), "入库单明细"));
            Entities.Add(CreateMetadata("业务单据", "出库单", typeof(tb_StockOut), "出库单"));
            Entities.Add(CreateMetadata("业务单据", "出库单明细", typeof(tb_StockOutDetail), "出库单明细"));
            Entities.Add(CreateMetadata("业务单据", "库存调拨", typeof(tb_StockTransfer), "库存调拨单"));
            Entities.Add(CreateMetadata("业务单据", "库存调拨明细", typeof(tb_StockTransferDetail), "库存调拨明细"));
            Entities.Add(CreateMetadata("业务单据", "盘点单", typeof(tb_Stocktake), "盘点单"));
            Entities.Add(CreateMetadata("业务单据", "盘点单明细", typeof(tb_StocktakeDetail), "盘点单明细"));
            Entities.Add(CreateMetadata("业务单据", "库存", typeof(tb_Inventory), "库存表"));
            Entities.Add(CreateMetadata("业务单据", "库存交易记录", typeof(tb_InventoryTransaction), "库存交易记录"));

            // ==================== 业务单据 - 生产模块 ====================
            Entities.Add(CreateMetadata("业务单据", "生产订单", typeof(tb_ManufacturingOrder), "生产订单"));
            Entities.Add(CreateMetadata("业务单据", "生产订单明细", typeof(tb_ManufacturingOrderDetail), "生产订单明细"));
            Entities.Add(CreateMetadata("业务单据", "BOM", typeof(tb_BOM_S), "BOM表"));
            Entities.Add(CreateMetadata("业务单据", "BOM明细", typeof(tb_BOM_SDetail), "BOM明细"));
            Entities.Add(CreateMetadata("业务单据", "生产计划", typeof(tb_ProductionPlan), "生产计划"));
            Entities.Add(CreateMetadata("业务单据", "生产计划明细", typeof(tb_ProductionPlanDetail), "生产计划明细"));
            Entities.Add(CreateMetadata("业务单据", "领料单", typeof(tb_MaterialRequisition), "领料单"));
            Entities.Add(CreateMetadata("业务单据", "领料单明细", typeof(tb_MaterialRequisitionDetail), "领料单明细"));
            Entities.Add(CreateMetadata("业务单据", "退料单", typeof(tb_MaterialReturn), "退料单"));
            Entities.Add(CreateMetadata("业务单据", "退料单明细", typeof(tb_MaterialReturnDetail), "退料单明细"));
            Entities.Add(CreateMetadata("业务单据", "MRP返工入库", typeof(tb_MRP_ReworkEntry), "MRP返工入库单"));
            Entities.Add(CreateMetadata("业务单据", "MRP返工退货", typeof(tb_MRP_ReworkReturn), "MRP返工退货单"));

            // ==================== 业务单据 - 售后模块 ====================
            Entities.Add(CreateMetadata("业务单据", "售后申请", typeof(tb_AS_AfterSaleApply), "售后申请表"));
            Entities.Add(CreateMetadata("业务单据", "维修单", typeof(tb_AS_RepairOrder), "维修单"));

            // ==================== 财务相关 ====================
            Entities.Add(CreateMetadata("财务相关", "收付款记录", typeof(tb_FM_PaymentRecord), "收付款记录表"));
            Entities.Add(CreateMetadata("财务相关", "收付款记录明细", typeof(tb_FM_PaymentRecordDetail), "收付款记录明细"));
            Entities.Add(CreateMetadata("财务相关", "应收应付", typeof(tb_FM_ReceivablePayable), "应收应付表"));
            Entities.Add(CreateMetadata("财务相关", "费用报销", typeof(tb_FM_ExpenseClaim), "费用报销单"));
            Entities.Add(CreateMetadata("财务相关", "其他费用", typeof(tb_FM_OtherExpense), "其他费用单"));
            Entities.Add(CreateMetadata("财务相关", "发票", typeof(tb_FM_Invoice), "发票表"));
            Entities.Add(CreateMetadata("财务相关", "对账单", typeof(tb_FM_Statement), "对账单"));
            Entities.Add(CreateMetadata("财务相关", "总账", typeof(tb_FM_GeneralLedger), "总账表"));
            Entities.Add(CreateMetadata("财务相关", "科目", typeof(tb_FM_Subject), "科目表"));
            Entities.Add(CreateMetadata("财务相关", "账户", typeof(tb_FM_Account), "账户表"));
            Entities.Add(CreateMetadata("财务相关", "预收款", typeof(tb_FM_PreReceivedPayment), "预收款表"));

            // ==================== 系统配置 ====================
            Entities.Add(CreateMetadata("系统配置", "菜单", typeof(tb_MenuInfo), "菜单信息表"));
            Entities.Add(CreateMetadata("系统配置", "按钮", typeof(tb_ButtonInfo), "按钮信息表"));
            Entities.Add(CreateMetadata("系统配置", "角色", typeof(tb_RoleInfo), "角色信息表"));
            Entities.Add(CreateMetadata("系统配置", "用户", typeof(tb_UserInfo), "用户信息表"));
            Entities.Add(CreateMetadata("系统配置", "系统配置", typeof(tb_SystemConfig), "系统配置表"));
            Entities.Add(CreateMetadata("系统配置", "用户角色表", typeof(tb_User_Role), "用户角色表"));
            Entities.Add(CreateMetadata("系统配置", "审批流程", typeof(tb_Approval), "审批流程表"));
        }

        /// <summary>
        /// 创建并自动提取元数据
        /// </summary>
        private static EntityMetadata CreateMetadata(string category, string displayName, Type entityType, string description)
        {
            var tableAttr = entityType.GetCustomAttribute<SugarTable>();
            var tableName = tableAttr?.TableName ?? entityType.Name;

            // 找主键
            var pkProp = entityType.GetProperties().FirstOrDefault(p => p.GetCustomAttribute<SugarColumn>()?.IsPrimaryKey == true);
            if (pkProp == null) pkProp = entityType.GetProperty("PrimaryKeyID");
            
            var pkName = pkProp?.GetCustomAttribute<SugarColumn>()?.ColumnName ?? pkProp?.Name;

            // 找子表关系
            var childRelations = new List<ChildRelationInfo>();
            foreach (var prop in entityType.GetProperties())
            {
                var navAttr = prop.GetCustomAttribute<Navigate>();
                if (navAttr != null && navAttr.GetNavigateType() == NavigateType.OneToMany)
                {
                    var childType = prop.PropertyType.IsGenericType ? prop.PropertyType.GetGenericArguments()[0] : prop.PropertyType;
                    childRelations.Add(new ChildRelationInfo
                    {
                        ChildTableName = childType.Name,
                        ForeignKeyColumn = navAttr.GetName(),
                        NavigationProperty = prop.Name
                    });
                }
            }

            return new EntityMetadata
            {
                Category = category,
                DisplayName = displayName,
                EntityName = entityType.Name,
                EntityType = entityType,
                Description = description,
                TableName = tableName,
                PrimaryKeyName = pkName,
                PrimaryKeyProperty = pkProp?.Name,
                ChildRelations = childRelations
            };
        }

        /// <summary>
        /// 根据分类获取实体列表
        /// </summary>
        /// <param name="category">分类名称</param>
        /// <returns>该分类下的实体元数据列表</returns>
        public static List<EntityMetadata> GetByCategory(string category)
        {
            return Entities.Where(e => e.Category == category).ToList();
        }

        /// <summary>
        /// 根据显示名称查找实体
        /// </summary>
        /// <param name="displayName">显示名称</param>
        /// <returns>实体元数据，未找到返回null</returns>
        public static EntityMetadata GetByDisplayName(string displayName)
        {
            return Entities.FirstOrDefault(e => e.DisplayName == displayName);
        }

        /// <summary>
        /// 获取分组后的实体字典
        /// </summary>
        /// <returns>Key为分类名，Value为该分类下的实体列表</returns>
        public static Dictionary<string, List<EntityMetadata>> GetAllGrouped()
        {
            return Entities.GroupBy(e => e.Category)
                          .OrderBy(g => g.Key)
                          .ToDictionary(g => g.Key, g => g.OrderBy(e => e.DisplayName).ToList());
        }

        /// <summary>
        /// 统一查询方法（泛型版本）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库客户端</param>
        /// <returns>实体列表</returns>
        public static async Task<List<T>> QueryAsync<T>(ISqlSugarClient db) where T : class, new()
        {
            return await db.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 统一查询方法(反射版本,用于动态调用)
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="db">数据库客户端</param>
        /// <returns>对象列表</returns>
        public static async Task<List<object>> QueryAsync(Type entityType, ISqlSugarClient db)
        {
            try
            {
                // 获取 SqlSugarScopeProvider 类型
                var providerType = db.GetType();
                        
                // 获取 Queryable<T> 泛型方法 - 使用精确的参数类型匹配
                var queryableMethod = providerType.GetMethods()
                    .FirstOrDefault(m => 
                        m.Name == "Queryable" && 
                        m.IsGenericMethodDefinition &&
                        m.GetParameters().Length == 0); // 无参版本
        
                if (queryableMethod == null)
                {
                    throw new InvalidOperationException($"无法找到 Queryable 方法 for type: {entityType.Name}");
                }
        
                // 构造泛型方法 Queryable<T>()
                var genericQueryable = queryableMethod.MakeGenericMethod(entityType);
        
                // 调用 Queryable<T>()
                var queryable = genericQueryable.Invoke(db, null);
        
                if (queryable == null)
                {
                    throw new InvalidOperationException($"Queryable<{entityType.Name}> 返回 null");
                }
        
                // 获取 ToListAsync 方法
                var toListAsyncMethod = queryable.GetType()
                    .GetMethods()
                    .FirstOrDefault(m => 
                        m.Name == "ToListAsync" && 
                        m.GetParameters().Length == 0); // 无参版本
        
                if (toListAsyncMethod == null)
                {
                    throw new InvalidOperationException($"无法找到 ToListAsync 方法 for type: {entityType.Name}");
                }
        
                // 调用 ToListAsync()
                var task = (Task)toListAsyncMethod.Invoke(queryable, null);
        
                if (task == null)
                {
                    throw new InvalidOperationException($"ToListAsync<{entityType.Name}> 返回 null");
                }
        
                // 等待任务完成
                await task;
        
                // 获取结果
                var resultProperty = task.GetType().GetProperty("Result");
                var resultList = resultProperty?.GetValue(task) as System.Collections.IEnumerable;
        
                if (resultList == null)
                {
                    return new List<object>();
                }
        
                // 转换为 List<object>
                return resultList.Cast<object>().ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"查询实体 {entityType.Name} 失败: {ex.Message}", ex);
            }
        }
    }
}
