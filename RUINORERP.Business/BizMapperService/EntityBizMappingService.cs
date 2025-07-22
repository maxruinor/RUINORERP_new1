using log4net;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Common.Extensions;
using System.Data.Common;

namespace RUINORERP.Business.BizMapperService
{


    /// <summary>
    /// 实体-业务映射服务（优化共用表处理）
    /// </summary>
    public class EntityBizMappingService
    {
        // 业务类型到实体映射配置
        private readonly ConcurrentDictionary<BizType, EntityMappingConfig> _bizMappings = new ConcurrentDictionary<BizType, EntityMappingConfig>();

        // 表名到实体类型映射
        private readonly ConcurrentDictionary<string, Type> _tableNameToType = new ConcurrentDictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        // 实体类型到字段配置映射
        private readonly ConcurrentDictionary<Type, EntityFieldConfig> _entityConfigs = new ConcurrentDictionary<Type, EntityFieldConfig>();

        // 共用表配置（实体类型->区分器配置）
        private readonly ConcurrentDictionary<Type, SharedTableConfig> _sharedTableConfigs = new ConcurrentDictionary<Type, SharedTableConfig>();



        // 新增：存储所有业务类型到实体类型的映射（包括共用表）
        private readonly ConcurrentDictionary<BizType, Type> _bizTypeToEntityMap = new();



        //public EntityBizMappingService()
        //{
        //    // 初始化时注册常见映射
        //    RegisterCommonMappings();
        //}

        /// <summary>
        /// 注册业务类型映射
        /// </summary>
        public void RegisterMapping<TEntity>(
            BizType bizType,
            Expression<Func<TEntity, long>> idSelector,
            Expression<Func<TEntity, string>> noSelector,
            Expression<Func<TEntity, object?>> detailsSelector = null)
            where TEntity : class
        {
            var entityType = typeof(TEntity);
            var fieldConfig = new EntityFieldConfig
            {
                IdField = GetMemberName(idSelector),
                NoField = GetMemberName(noSelector),
                DetailProperty = detailsSelector != null ? GetMemberName(detailsSelector) : null
            };

            _bizMappings[bizType] = new EntityMappingConfig
            {
                EntityType = entityType,
                FieldConfig = fieldConfig
            };

            _entityConfigs[entityType] = fieldConfig;
            _tableNameToType[entityType.Name] = entityType;


        }

        /// <summary>
        /// 注册共用表区分器
        /// </summary>
        public void RegisterSharedTableDiscriminator<TEntity, TValue>(
            Expression<Func<TEntity, object>> discriminatorSelector,
            Func<TValue, BizType> typeResolver,//公共表用来区分时的字段的类型
              Expression<Func<TEntity, object>> detailsSelector = null)
            where TEntity : class
        {
            var entityType = typeof(TEntity);
            var discriminatorField = GetMemberName(discriminatorSelector);
            // 创建类型安全的解析器
            Func<object, BizType> resolver = value =>
            {
                // 处理可空类型
                if (value == null) return BizType.未知类型;

                // 类型转换
                if (value is TValue typedValue)
                    return typeResolver(typedValue);

                // 尝试类型转换
                try
                {
                    var converted = (TValue)Convert.ChangeType(value, typeof(TValue));
                    return typeResolver(converted);
                }
                catch
                {
                    return BizType.未知类型;
                }
            };


            _sharedTableConfigs[entityType] = new SharedTableConfig
            {
                DiscriminatorField = discriminatorField,
                TypeResolver = resolver
            };


            // 注册业务类型到实体类型的映射
            foreach (var bizType in Enum.GetValues(typeof(BizType)).Cast<BizType>())
            {
                try
                {
                    // 测试每个可能的业务类型是否映射到此实体
                    var testValue = default(TValue);
                    var resolvedBizType = typeResolver(testValue);

                    // 如果解析成功，则注册映射
                    if (resolvedBizType != BizType.未知类型)
                    {
                        _bizTypeToEntityMap[bizType] = entityType;
                    }
                }
                catch
                {
                    // 忽略转换错误
                }
            }




        }

        // 从表达式树获取成员名称
        private string GetMemberName<T, TProp>(Expression<Func<T, TProp>> expression)
        {
            if (expression == null)
            {
                return string.Empty;
            }
            var mb = expression.GetMemberInfo();
            if (mb == null)
            {
                return string.Empty;
            }
            return mb.Name;

            //return ((MemberExpression)expression.Body).Member.Name;
        }

        // 获取实体类型
        public Type GetEntityType(BizType bizType)
        {

            // 1. 首先检查显式配置的业务类型
            if (_bizMappings.TryGetValue(bizType, out var config))
            {
                return config.EntityType;
            }

            // 2. 检查共用表映射
            if (_bizTypeToEntityMap.TryGetValue(bizType, out var entityType))
            {
                return entityType;
            }

            throw new KeyNotFoundException($"未找到业务类型 {bizType} 对应的实体类型");
        }

        public bool TryGetEntityType(BizType bizType, out Type entityType)
        {
            try
            {
                entityType = GetEntityType(bizType);
                return true;
            }
            catch
            {
                entityType = null!;
                return false;
            }
        }

        // 获取字段配置
        public EntityFieldConfig GetFieldConfig(Type entityType)
        {
            if (_entityConfigs.TryGetValue(entityType, out var config))
            {
                return config;
            }

            // 自动探测字段
            config = AutoDetectFieldMapping(entityType);
            _entityConfigs[entityType] = config;
            return config;
        }

        public bool TryGetFieldConfig(Type entityType, out EntityFieldConfig cfg) =>
      (_entityConfigs.TryGetValue(entityType, out cfg) || (cfg = AutoDetectFieldMapping(entityType)) != null);

        // 自动探测字段映射
        private EntityFieldConfig AutoDetectFieldMapping(Type entityType)
        {
            var properties = entityType.GetProperties();
            var config = new EntityFieldConfig();

            // 探测ID字段（优先匹配长整型主键）
            config.IdField = properties
                .FirstOrDefault(p => p.PropertyType == typeof(long) &&
                                     (p.Name.EndsWith("ID") || p.Name == "Id"))?.Name;

            // 探测单号字段（优先匹配字符串类型）
            config.NoField = properties
                .FirstOrDefault(p => p.PropertyType == typeof(string) &&
                                    (p.Name.EndsWith("No") || p.Name.Contains("Code")))?.Name;

            // 探测明细属性（匹配集合类型的属性）
            config.DetailProperty = properties
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                    p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))?.Name;

            return config;
        }

        // 通过表名获取实体类型
        public Type GetEntityTypeByTableName(string tableName)
        {
            if (_tableNameToType.TryGetValue(tableName, out var type))
            {
                return type;
            }

            // 动态查找程序集
            var entityType = Assembly.GetAssembly(typeof(BaseEntity))
                .GetTypes()
                .FirstOrDefault(t => t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));

            if (entityType != null)
            {
                _tableNameToType[tableName] = entityType;
                return entityType;
            }

            throw new KeyNotFoundException($"找不到表名对应的实体类型: {tableName}");
        }

        // 获取业务类型
        public BizType GetBizType(Type entityType, object entity)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));


            // 检查是否为共用表
            if (_sharedTableConfigs.TryGetValue(entityType, out var sharedConfig))
            {
                var prop = entityType.GetProperty(sharedConfig.DiscriminatorField);
                if (prop != null)
                {
                    var value = prop.GetValue(entity);  // 直接获取原始值
                    return sharedConfig.TypeResolver(value);  // 使用通用解析器
                }
            }


            // 1. 首先检查显式配置的业务类型
            var mapping = _bizMappings.FirstOrDefault(x => x.Value.EntityType == entityType);
            if (!mapping.Equals(default(KeyValuePair<BizType, EntityMappingConfig>)))
            {
                return mapping.Key;
            }

            // 3. 默认返回未知类型
            return BizType.未知类型;
        }

        public bool TryGetBizType(Type entityType, object entity, out BizType bizType)
        {
            bizType = GetBizType(entityType, entity);
            return bizType != BizType.未知类型;
        }


        /// <summary>
        /// 注册常用映射
        /// </summary>
        public void RegisterCommonMappings()
        {
            // 销售订单
            RegisterMapping<tb_SaleOrder>(
                BizType.销售订单,
                e => e.SOrder_ID,
                e => e.SOrderNo,
                e => e.tb_SaleOrderDetails);


            RegisterMapping<tb_SaleOut>(
            BizType.销售出库单,
            e => e.SaleOut_MainID,
            e => e.SaleOutNo,
            e => e.tb_SaleOutDetails);

            RegisterMapping<tb_SaleOutRe>(
            BizType.销售退回单,
            e => e.SaleOutRe_ID,
            e => e.ReturnNo,
            e => e.tb_SaleOutReDetails);



            RegisterMapping<tb_BOM_S>(
            BizType.BOM物料清单,
            e => e.BOM_ID,
            e => e.BOM_No,
            e => e.tb_BOM_SDetails);

            RegisterMapping<tb_PurOrder>(
    BizType.采购订单,
    e => e.PurOrder_ID,
    e => e.PurOrderNo,
    e => e.tb_PurOrderDetails);

            RegisterMapping<tb_PurEntry>(
    BizType.采购入库单,
    e => e.PurEntryID,
    e => e.PurEntryNo,
    e => e.tb_PurEntryDetails);

            RegisterMapping<tb_PurEntryRe>(
    BizType.采购退货单,
    e => e.PurEntryRe_ID,
    e => e.PurEntryReNo,
    e => e.tb_PurEntryReDetails);

            RegisterMapping<tb_PurReturnEntry>(
    BizType.采购退货入库,
    e => e.PurReEntry_ID,
    e => e.PurReEntryNo,
    e => e.tb_PurReturnEntryDetails);



            RegisterMapping<tb_Stocktake>(
    BizType.盘点单,
    e => e.MainID,
    e => e.CheckNo,
    e => e.tb_StocktakeDetails);

            RegisterMapping<tb_StockIn>(
    BizType.其他入库单,
    e => e.MainID,
    e => e.BillNo,
    e => e.tb_StockInDetails);

            RegisterMapping<tb_StockOut>(
    BizType.其他出库单,
    e => e.MainID,
    e => e.BillNo,
    e => e.tb_StockOutDetails);

            RegisterMapping<tb_FM_ExpenseClaim>(
    BizType.费用报销单,
    e => e.ClaimMainID,
    e => e.ClaimNo,
    e => e.tb_FM_ExpenseClaimDetails);

            RegisterMapping<tb_BuyingRequisition>(
    BizType.请购单,
    e => e.PuRequisition_ID,
    e => e.PuRequisitionNo,
    e => e.tb_BuyingRequisitionDetails);

            RegisterMapping<tb_ManufacturingOrder>(
    BizType.制令单,
    e => e.MOID,
    e => e.MONO,
    e => e.tb_ManufacturingOrderDetails);

            RegisterMapping<tb_ProductionPlan>(
    BizType.生产计划单,
    e => e.PPID,
    e => e.PPNo,
    e => e.tb_ProductionPlanDetails);

            RegisterMapping<tb_MaterialRequisition>(
            BizType.生产领料单,
            e => e.MR_ID,
            e => e.MaterialRequisitionNO,
            e => e.tb_MaterialRequisitionDetails);

            RegisterMapping<tb_MaterialReturn>(
            BizType.生产退料单,
            e => e.MRE_ID,
            e => e.BillNo,
            e => e.tb_MaterialReturnDetails);

            RegisterMapping<tb_ProductionDemand>(
            BizType.需求分析,
            e => e.PDID,
            e => e.PDNo,
            e => e.tb_ProductionDemandDetails);





            RegisterMapping<tb_FinishedGoodsInv>(
            BizType.缴库单,
            e => e.FG_ID,
            e => e.DeliveryBillNo,
            e => e.tb_FinishedGoodsInvDetails);

            RegisterMapping<tb_ProdBorrowing>(
            BizType.借出单,
            e => e.BorrowID,
            e => e.BorrowNo,
            e => e.tb_ProdBorrowingDetails);

            RegisterMapping<tb_ProdReturning>(
            BizType.归还单,
            e => e.ReturnID,
            e => e.ReturnNo,
            e => e.tb_ProdReturningDetails);


            RegisterMapping<tb_ProdMerge>(
            BizType.产品组合单,
            e => e.MergeID,
            e => e.MergeNo,
            e => e.tb_ProdMergeDetails);

            RegisterMapping<tb_ProdSplit>(
            BizType.产品分割单,
            e => e.SplitID,
            e => e.SplitNo,
            e => e.tb_ProdSplitDetails);

            RegisterMapping<tb_ProdBundle>(
            BizType.套装组合,
            e => e.BundleID,
            e => e.BundleName,
            e => e.tb_ProdBundleDetails);

            RegisterMapping<tb_ProdConversion>(
            BizType.产品转换单,
            e => e.ConversionID,
            e => e.ConversionNo,
            e => e.tb_ProdConversionDetails);


            RegisterMapping<tb_StockTransfer>(
BizType.调拨单,
e => e.StockTransferID,
e => e.StockTransferNo,
e => e.tb_StockTransferDetails);


            RegisterMapping<tb_MRP_ReworkReturn>(
BizType.返工退库单,
e => e.ReworkReturnID,
e => e.ReworkReturnNo,
e => e.tb_MRP_ReworkReturnDetails);

            RegisterMapping<tb_MRP_ReworkEntry>(
BizType.返工入库单,
e => e.ReworkEntryID,
e => e.ReworkEntryNo,
e => e.tb_MRP_ReworkEntryDetails);


            RegisterMapping<tb_FM_PaymentApplication>(
            BizType.付款申请单,
            e => e.ApplicationID,
            e => e.ApplicationNo,
            e => null);

            RegisterMapping<tb_AS_AfterSaleApply>(
BizType.售后申请单,
e => e.ASApplyID,
e => e.ASApplyNo,
e => e.tb_AS_AfterSaleApplyDetails);

            RegisterMapping<tb_AS_AfterSaleDelivery>(
BizType.售后交付单,
e => e.ASDeliveryID,
e => e.ASDeliveryNo,
e => e.tb_AS_AfterSaleDeliveryDetails);

            RegisterMapping<tb_AS_RepairOrder>(
BizType.维修工单,
e => e.RepairOrderID,
e => e.RepairOrderNo,
e => e.tb_AS_RepairOrderDetails);


            RegisterMapping<tb_AS_RepairInStock>(
BizType.维修入库单,
e => e.RepairInStockID,
e => e.RepairInStockNo,
e => e.tb_AS_RepairInStockDetails);





            // 付款/收款共用表配置
            RegisterMapping<tb_FM_PaymentRecord>(
                BizType.付款单, // 默认业务类型
                e => e.PaymentId,
                e => e.PaymentNo,
                e => e.tb_FM_PaymentRecordDetails);


            // 字符串型字段
            //RegisterSharedTableDiscriminator<tb_Transaction, string>(
            //    e => e.TransactionType,
            //    value => value switch {
            //        "SALE" => BizType.销售交易,
            //        "PURCHASE" => BizType.采购交易,
            //        _ => BizType.其他交易
            //    });



            RegisterSharedTableDiscriminator<tb_FM_OtherExpense, bool>(
        e => e.EXPOrINC,
        value => value == false ? BizType.其他费用支出 : BizType.其他费用收入);


            RegisterSharedTableDiscriminator<tb_FM_PriceAdjustment, int>(e => e.ReceivePaymentType, value => value == (int)ReceivePaymentType.收款 ? BizType.销售价格调整单 : BizType.采购价格调整单);


            // 添加共用表区分器
            RegisterSharedTableDiscriminator<tb_FM_PaymentRecord, int>(
                e => e.ReceivePaymentType,
                value => value == (int)ReceivePaymentType.收款 ? BizType.收款单 : BizType.付款单);


            // 添加共用表区分器
            RegisterSharedTableDiscriminator<tb_FM_PreReceivedPayment, int>(
                e => e.ReceivePaymentType,
                value => value == (int)ReceivePaymentType.收款 ? BizType.预收款单 : BizType.预付款单);


            // 添加共用表区分器
            RegisterSharedTableDiscriminator<tb_FM_ReceivablePayable, int>(
                e => e.ReceivePaymentType,
                value => value == (int)ReceivePaymentType.收款 ? BizType.应收款单 : BizType.应付款单);


            // 添加共用表区分器
            //RegisterSharedTableDiscriminator<tb_FM_PaymentSettlement, int>(
            //    e => e.ReceivePaymentType,
            //    value => value == (int)ReceivePaymentType.收款 ? BizType.收款核销 : BizType.付款核销);

            // 添加共用表区分器
            RegisterSharedTableDiscriminator<tb_FM_PaymentSettlement, ReceivePaymentType>(
                e => e.ReceivePaymentType,
                value => value == ReceivePaymentType.收款 ? BizType.收款核销 : BizType.付款核销);


            // 显式注册共用表业务类型到实体类型的映射
            _bizTypeToEntityMap[BizType.收款单] = typeof(tb_FM_PaymentRecord);
            _bizTypeToEntityMap[BizType.付款单] = typeof(tb_FM_PaymentRecord);
            _bizTypeToEntityMap[BizType.预收款单] = typeof(tb_FM_PreReceivedPayment);
            _bizTypeToEntityMap[BizType.预付款单] = typeof(tb_FM_PreReceivedPayment);
            _bizTypeToEntityMap[BizType.应收款单] = typeof(tb_FM_ReceivablePayable);
            _bizTypeToEntityMap[BizType.应付款单] = typeof(tb_FM_ReceivablePayable);
            _bizTypeToEntityMap[BizType.收款核销] = typeof(tb_FM_PaymentSettlement);
            _bizTypeToEntityMap[BizType.付款核销] = typeof(tb_FM_PaymentSettlement);
            _bizTypeToEntityMap[BizType.其他费用支出] = typeof(tb_FM_OtherExpense);
            _bizTypeToEntityMap[BizType.其他费用收入] = typeof(tb_FM_OtherExpense);
            _bizTypeToEntityMap[BizType.销售价格调整单] = typeof(tb_FM_PriceAdjustment);
            _bizTypeToEntityMap[BizType.采购价格调整单] = typeof(tb_FM_PriceAdjustment);

            //// 可空整型字段
            //RegisterSharedTableDiscriminator<tb_Refund, int?>(
            //    e => e.RefundReasonCode,
            //    value => value switch {
            //        1 => BizType.质量问题退款,
            //        2 => BizType.七天无理由退款,
            //        null => BizType.未分类退款,
            //        _ => BizType.其他退款
            //    });
        }
    }
}
