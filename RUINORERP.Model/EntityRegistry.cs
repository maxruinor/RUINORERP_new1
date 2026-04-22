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
            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "客户/供应商",
                EntityType = typeof(tb_CustomerVendor),
                Description = "客户厂商表",
                TableName = "tb_CustomerVendor"
            });

            // 产品相关
            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "产品类目",
                EntityType = typeof(tb_ProdCategories),
                Description = "产品类目表",
                TableName = "tb_ProdCategories"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "产品基本信息",
                EntityType = typeof(tb_Prod),
                Description = "货品基本信息表",
                TableName = "tb_Prod"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "产品详情",
                EntityType = typeof(tb_ProdDetail),
                Description = "产品详情信息表",
                TableName = "tb_ProdDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "产品属性",
                EntityType = typeof(tb_ProdProperty),
                Description = "产品属性表",
                TableName = "tb_ProdProperty"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "产品属性值",
                EntityType = typeof(tb_ProdPropertyValue),
                Description = "产品属性值表",
                TableName = "tb_ProdPropertyValue"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "产品类型",
                EntityType = typeof(tb_ProductType),
                Description = "产品类型表",
                TableName = "tb_ProductType"
            });

            // 库存位置
            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "库位",
                EntityType = typeof(tb_Location),
                Description = "库位表",
                TableName = "tb_Location"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "货架",
                EntityType = typeof(tb_StorageRack),
                Description = "货架表",
                TableName = "tb_StorageRack"
            });

            // 单位
            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "单位",
                EntityType = typeof(tb_Unit),
                Description = "单位表",
                TableName = "tb_Unit"
            });

            // 组织架构
            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "部门",
                EntityType = typeof(tb_Department),
                Description = "部门表",
                TableName = "tb_Department"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "员工",
                EntityType = typeof(tb_Employee),
                Description = "员工表",
                TableName = "tb_Employee"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "公司",
                EntityType = typeof(tb_Company),
                Description = "公司表",
                TableName = "tb_Company"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "职位",
                EntityType = typeof(tb_Position),
                Description = "职位表",
                TableName = "tb_Position"
            });

            // CRM相关
            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "CRM客户",
                EntityType = typeof(tb_CRM_Customer),
                Description = "CRM客户表",
                TableName = "tb_CRM_Customer"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "CRM联系人",
                EntityType = typeof(tb_CRM_Contact),
                Description = "CRM联系人表",
                TableName = "tb_CRM_Contact"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "基础主数据",
                DisplayName = "CRM线索",
                EntityType = typeof(tb_CRM_Leads),
                Description = "CRM线索表",
                TableName = "tb_CRM_Leads"
            });

            // ==================== 业务单据 - 销售模块 ====================

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "销售订单",
                EntityType = typeof(tb_SaleOrder),
                Description = "销售订单",
                TableName = "tb_SaleOrder"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "销售订单明细",
                EntityType = typeof(tb_SaleOrderDetail),
                Description = "销售订单明细",
                TableName = "tb_SaleOrderDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "销售出库",
                EntityType = typeof(tb_SaleOut),
                Description = "销售出库单",
                TableName = "tb_SaleOut"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "销售出库明细",
                EntityType = typeof(tb_SaleOutDetail),
                Description = "销售出库明细",
                TableName = "tb_SaleOutDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "销售退货",
                EntityType = typeof(tb_SaleOutRe),
                Description = "销售退货单",
                TableName = "tb_SaleOutRe"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "销售退货明细",
                EntityType = typeof(tb_SaleOutReDetail),
                Description = "销售退货明细",
                TableName = "tb_SaleOutReDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "销售合同",
                EntityType = typeof(tb_SO_Contract),
                Description = "销售合同",
                TableName = "tb_SO_Contract"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "销售合同明细",
                EntityType = typeof(tb_SO_ContractDetail),
                Description = "销售合同明细",
                TableName = "tb_SO_ContractDetail"
            });

            // ==================== 业务单据 - 采购模块 ====================

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购订单",
                EntityType = typeof(tb_PurOrder),
                Description = "采购订单",
                TableName = "tb_PurOrder"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购订单明细",
                EntityType = typeof(tb_PurOrderDetail),
                Description = "采购订单明细",
                TableName = "tb_PurOrderDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购入库",
                EntityType = typeof(tb_PurEntry),
                Description = "采购入库单",
                TableName = "tb_PurEntry"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购入库明细",
                EntityType = typeof(tb_PurEntryDetail),
                Description = "采购入库明细",
                TableName = "tb_PurEntryDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购退货",
                EntityType = typeof(tb_PurReturnEntry),
                Description = "采购退货单",
                TableName = "tb_PurReturnEntry"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购退货明细",
                EntityType = typeof(tb_PurReturnEntryDetail),
                Description = "采购退货明细",
                TableName = "tb_PurReturnEntryDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购申请",
                EntityType = typeof(tb_BuyingRequisition),
                Description = "采购申请表",
                TableName = "tb_BuyingRequisition"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购申请明细",
                EntityType = typeof(tb_BuyingRequisitionDetail),
                Description = "采购申请明细",
                TableName = "tb_BuyingRequisitionDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购合同",
                EntityType = typeof(tb_PO_Contract),
                Description = "采购合同",
                TableName = "tb_PO_Contract"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "采购合同明细",
                EntityType = typeof(tb_PO_ContractDetail),
                Description = "采购合同明细",
                TableName = "tb_PO_ContractDetail"
            });

            // ==================== 业务单据 - 库存模块 ====================

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "入库单",
                EntityType = typeof(tb_StockIn),
                Description = "入库单",
                TableName = "tb_StockIn"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "入库单明细",
                EntityType = typeof(tb_StockInDetail),
                Description = "入库单明细",
                TableName = "tb_StockInDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "出库单",
                EntityType = typeof(tb_StockOut),
                Description = "出库单",
                TableName = "tb_StockOut"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "出库单明细",
                EntityType = typeof(tb_StockOutDetail),
                Description = "出库单明细",
                TableName = "tb_StockOutDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "库存调拨",
                EntityType = typeof(tb_StockTransfer),
                Description = "库存调拨单",
                TableName = "tb_StockTransfer"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "库存调拨明细",
                EntityType = typeof(tb_StockTransferDetail),
                Description = "库存调拨明细",
                TableName = "tb_StockTransferDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "盘点单",
                EntityType = typeof(tb_Stocktake),
                Description = "盘点单",
                TableName = "tb_Stocktake"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "盘点单明细",
                EntityType = typeof(tb_StocktakeDetail),
                Description = "盘点单明细",
                TableName = "tb_StocktakeDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "库存",
                EntityType = typeof(tb_Inventory),
                Description = "库存表",
                TableName = "tb_Inventory"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "库存交易记录",
                EntityType = typeof(tb_InventoryTransaction),
                Description = "库存交易记录",
                TableName = "tb_InventoryTransaction"
            });

            // ==================== 业务单据 - 生产模块 ====================

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "生产订单",
                EntityType = typeof(tb_ManufacturingOrder),
                Description = "生产订单",
                TableName = "tb_ManufacturingOrder"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "生产订单明细",
                EntityType = typeof(tb_ManufacturingOrderDetail),
                Description = "生产订单明细",
                TableName = "tb_ManufacturingOrderDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "BOM",
                EntityType = typeof(tb_BOM_S),
                Description = "BOM表",
                TableName = "tb_BOM_S"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "BOM明细",
                EntityType = typeof(tb_BOM_SDetail),
                Description = "BOM明细",
                TableName = "tb_BOM_SDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "生产计划",
                EntityType = typeof(tb_ProductionPlan),
                Description = "生产计划",
                TableName = "tb_ProductionPlan"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "生产计划明细",
                EntityType = typeof(tb_ProductionPlanDetail),
                Description = "生产计划明细",
                TableName = "tb_ProductionPlanDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "领料单",
                EntityType = typeof(tb_MaterialRequisition),
                Description = "领料单",
                TableName = "tb_MaterialRequisition"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "领料单明细",
                EntityType = typeof(tb_MaterialRequisitionDetail),
                Description = "领料单明细",
                TableName = "tb_MaterialRequisitionDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "退料单",
                EntityType = typeof(tb_MaterialReturn),
                Description = "退料单",
                TableName = "tb_MaterialReturn"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "退料单明细",
                EntityType = typeof(tb_MaterialReturnDetail),
                Description = "退料单明细",
                TableName = "tb_MaterialReturnDetail"
            });

            // MRP返工
            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "MRP返工入库",
                EntityType = typeof(tb_MRP_ReworkEntry),
                Description = "MRP返工入库单",
                TableName = "tb_MRP_ReworkEntry"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "MRP返工退货",
                EntityType = typeof(tb_MRP_ReworkReturn),
                Description = "MRP返工退货单",
                TableName = "tb_MRP_ReworkReturn"
            });

            // ==================== 业务单据 - 售后模块 ====================

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "售后申请",
                EntityType = typeof(tb_AS_AfterSaleApply),
                Description = "售后申请表",
                TableName = "tb_AS_AfterSaleApply"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "业务单据",
                DisplayName = "维修单",
                EntityType = typeof(tb_AS_RepairOrder),
                Description = "维修单",
                TableName = "tb_AS_RepairOrder"
            });

            // ==================== 财务相关 ====================

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "收付款记录",
                EntityType = typeof(tb_FM_PaymentRecord),
                Description = "收付款记录表",
                TableName = "tb_FM_PaymentRecord"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "收付款记录明细",
                EntityType = typeof(tb_FM_PaymentRecordDetail),
                Description = "收付款记录明细",
                TableName = "tb_FM_PaymentRecordDetail"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "应收应付",
                EntityType = typeof(tb_FM_ReceivablePayable),
                Description = "应收应付表",
                TableName = "tb_FM_ReceivablePayable"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "费用报销",
                EntityType = typeof(tb_FM_ExpenseClaim),
                Description = "费用报销单",
                TableName = "tb_FM_ExpenseClaim"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "其他费用",
                EntityType = typeof(tb_FM_OtherExpense),
                Description = "其他费用单",
                TableName = "tb_FM_OtherExpense"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "发票",
                EntityType = typeof(tb_FM_Invoice),
                Description = "发票表",
                TableName = "tb_FM_Invoice"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "对账单",
                EntityType = typeof(tb_FM_Statement),
                Description = "对账单",
                TableName = "tb_FM_Statement"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "总账",
                EntityType = typeof(tb_FM_GeneralLedger),
                Description = "总账表",
                TableName = "tb_FM_GeneralLedger"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "科目",
                EntityType = typeof(tb_FM_Subject),
                Description = "科目表",
                TableName = "tb_FM_Subject"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "账户",
                EntityType = typeof(tb_FM_Account),
                Description = "账户表",
                TableName = "tb_FM_Account"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "财务相关",
                DisplayName = "预收款",
                EntityType = typeof(tb_FM_PreReceivedPayment),
                Description = "预收款表",
                TableName = "tb_FM_PreReceivedPayment"
            });

            // ==================== 系统配置 ====================

            Entities.Add(new EntityMetadata
            {
                Category = "系统配置",
                DisplayName = "菜单",
                EntityType = typeof(tb_MenuInfo),
                Description = "菜单信息表",
                TableName = "tb_MenuInfo"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "系统配置",
                DisplayName = "按钮",
                EntityType = typeof(tb_ButtonInfo),
                Description = "按钮信息表",
                TableName = "tb_ButtonInfo"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "系统配置",
                DisplayName = "角色",
                EntityType = typeof(tb_RoleInfo),
                Description = "角色信息表",
                TableName = "tb_RoleInfo"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "系统配置",
                DisplayName = "用户",
                EntityType = typeof(tb_UserInfo),
                Description = "用户信息表",
                TableName = "tb_UserInfo"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "系统配置",
                DisplayName = "系统配置",
                EntityType = typeof(tb_SystemConfig),
                Description = "系统配置表",
                TableName = "tb_SystemConfig"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "系统配置",
                DisplayName = "权限",
                EntityType = typeof(tb_Permission),
                Description = "权限表",
                TableName = "tb_Permission"
            });

            Entities.Add(new EntityMetadata
            {
                Category = "系统配置",
                DisplayName = "审批流程",
                EntityType = typeof(tb_Approval),
                Description = "审批流程表",
                TableName = "tb_Approval"
            });
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
