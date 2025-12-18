using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using SqlSugar;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// 序号生成服务使用示例
    /// 展示如何正确使用业务类型功能
    /// </summary>
    public class UsageExample
    {
        private DatabaseSequenceService sequenceService;
        private BNRFactory bnrFactory;
        
        public UsageExample()
        {
            // 初始化数据库连接
            var sqlSugarClient = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "YourConnectionStringHere",
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });
            
            // 初始化缓存管理器
            var cacheManager = CacheManager.Core.CacheFactory.Build("default", settings =>
            {
                settings.WithDictionaryHandle();
            });
            
            // 初始化序号服务
            sequenceService = new DatabaseSequenceService(sqlSugarClient);
            
            // 初始化BNR工厂
            bnrFactory = new BNRFactory(sequenceService, cacheManager);
        }
        
        /// <summary>
        /// 销售订单编号生成示例
        /// </summary>
        public void GenerateSalesOrderNumber()
        {
            // 设置当前业务类型
            BNRFactory.SetCurrentBusinessType("SalesOrder");
            
            try
            {
                // 生成销售订单编号
                // 格式: SO + 年月日 + 5位序号
                string rule = "{S:SO}{D:yyyyMMdd}{DB:SALES_ORDER/00000}";
                string orderNumber = bnrFactory.Create(rule);
                
                System.Diagnostics.Debug.WriteLine($"生成的销售订单编号: {orderNumber}");
                
                // 查询该业务类型的所有序号
                var sequences = sequenceService.GetSequencesByBusinessType("SalesOrder");
                System.Diagnostics.Debug.WriteLine($"SalesOrder业务类型共有 {sequences.Count} 个序号记录");
            }
            finally
            {
                // 清除业务类型设置
                BNRFactory.SetCurrentBusinessType(null);
            }
        }
        
        /// <summary>
        /// 采购订单编号生成示例
        /// </summary>
        public void GeneratePurchaseOrderNumber()
        {
            // 设置当前业务类型
            BNRFactory.SetCurrentBusinessType("PurchaseOrder");
            
            try
            {
                // 生成采购订单编号
                // 格式: PO + 年月日 + 5位序号
                string rule = "{S:PO}{D:yyyyMMdd}{DB:PURCHASE_ORDER/00000}";
                string orderNumber = bnrFactory.Create(rule);
                
                System.Diagnostics.Debug.WriteLine($"生成的采购订单编号: {orderNumber}");
                
                // 查询该业务类型的所有序号
                var sequences = sequenceService.GetSequencesByBusinessType("PurchaseOrder");
                System.Diagnostics.Debug.WriteLine($"PurchaseOrder业务类型共有 {sequences.Count} 个序号记录");
            }
            finally
            {
                // 清除业务类型设置
                BNRFactory.SetCurrentBusinessType(null);
            }
        }
        
        /// <summary>
        /// 按天重置的序号示例
        /// </summary>
        public void GenerateDailyResetNumber()
        {
            // 设置当前业务类型
            BNRFactory.SetCurrentBusinessType("DailyReport");
            
            try
            {
                // 生成按天重置的序号
                // 格式: DR + 年月日 + 3位序号，每天从1开始
                string rule = "{S:DR}{D:yyyyMMdd}{DB:DAILY_REPORT/000/daily}";
                string reportNumber = bnrFactory.Create(rule);
                
                System.Diagnostics.Debug.WriteLine($"生成的日报编号: {reportNumber}");
            }
            finally
            {
                // 清除业务类型设置
                BNRFactory.SetCurrentBusinessType(null);
            }
        }
        
        /// <summary>
        /// 直接使用序号服务生成序号
        /// </summary>
        public void DirectSequenceServiceUsage()
        {
            // 直接调用序号服务生成序号，并指定业务类型
            long nextValue = sequenceService.GetNextSequenceValue(
                "INVENTORY_CHECK", 
                "None", 
                "0000", 
                "库存盘点序号", 
                "InventoryCheck");
                
            string inventoryCheckNumber = $"IC{DateTime.Now:yyyyMMdd}{nextValue:0000}";
            System.Diagnostics.Debug.WriteLine($"生成的库存盘点编号: {inventoryCheckNumber}");
            
            // 更新序号信息，添加业务类型
            sequenceService.UpdateSequenceInfo(
                "INVENTORY_CHECK", 
                null, 
                "0000", 
                "库存盘点序号", 
                "InventoryCheck");
                
            // 重置序号值，并指定业务类型
            sequenceService.ResetSequenceValue("INVENTORY_CHECK", 1, "InventoryCheck");
        }
        
        /// <summary>
        /// 查询特定业务类型的序号
        /// </summary>
        public void QuerySequencesByBusinessType()
        {
            // 查询销售订单相关的序号
            var salesSequences = sequenceService.GetSequencesByBusinessType("SalesOrder");
            System.Diagnostics.Debug.WriteLine($"销售订单相关序号:");
            foreach (var seq in salesSequences)
            {
                System.Diagnostics.Debug.WriteLine($"  键: {seq.SequenceKey}, 当前值: {seq.CurrentValue}, 描述: {seq.Description}");
            }
            
            // 查询采购订单相关的序号
            var purchaseSequences = sequenceService.GetSequencesByBusinessType("PurchaseOrder");
            System.Diagnostics.Debug.WriteLine($"采购订单相关序号:");
            foreach (var seq in purchaseSequences)
            {
                System.Diagnostics.Debug.WriteLine($"  键: {seq.SequenceKey}, 当前值: {seq.CurrentValue}, 描述: {seq.Description}");
            }
        }
        
        /// <summary>
        /// 测试序号服务功能
        /// </summary>
        public void TestSequenceService()
        {
            string testResult = sequenceService.TestSequenceTable();
            System.Diagnostics.Debug.WriteLine($"序号服务测试结果:\n{testResult}");
        }
        
        /// <summary>
        /// 获取所有序号列表
        /// </summary>
        public void ListAllSequences()
        {
            var allSequences = sequenceService.GetAllSequences();
            System.Diagnostics.Debug.WriteLine($"所有序号列表:");
            foreach (var seq in allSequences)
            {
                System.Diagnostics.Debug.WriteLine($"  键: {seq.SequenceKey}, 当前值: {seq.CurrentValue}, 业务类型: {seq.BusinessType}, 重置类型: {seq.ResetType}");
            }
        }
    }
}