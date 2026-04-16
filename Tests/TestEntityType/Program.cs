using System;
using System.Reflection;
using RUINORERP.Model;
using RUINORERP.Common.Helper;

namespace TestEntityType
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 测试实体类型保持 ===\n");

            // 创建具体的业务实体
            var order = new tb_SaleOrder
            {
                SOrder_ID = 1,
                SOrderNo = "SO20260416001",
                DataStatus = 2,  // 确认
                ApprovalStatus = 0  // 待审核
            };

            Console.WriteLine($"原实体类型: {order.GetType().FullName}");
            Console.WriteLine($"原实体 SOrder_ID: {order.SOrder_ID}, SOrderNo: {order.SOrderNo}");
            Console.WriteLine($"原实体 DataStatus: {order.DataStatus}\n");

            // 克隆（模拟 ProtectEntities 方法）- 使用泛型保持类型
            var snapshot = CloneHelper.DeepCloneObject<tb_SaleOrder>(order);

            Console.WriteLine($"快照类型: {snapshot.GetType().FullName}");
            Console.WriteLine($"快照实际运行时类型: {snapshot.GetType().Name}\n");

            // 测试反射获取属性
            var snapshotType = snapshot.GetType();
            var properties = snapshotType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            Console.WriteLine($"快照的属性数量: {properties.Length}");
            Console.WriteLine("前10个属性:");
            foreach (var prop in properties)
            {
                if (prop.CanRead)
                {
                    try
                    {
                        var value = prop.GetValue(snapshot);
                        Console.WriteLine($"  - {prop.Name}: {value}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  - {prop.Name}: [读取失败: {ex.Message}]");
                    }
                }
            }

            // 修改原实体
            order.DataStatus = 5;  // 完结
            order.ApprovalStatus = 2;  // 审核通过;

            Console.WriteLine($"\n修改后 - 原实体 DataStatus: {order.DataStatus}");
            Console.WriteLine($"修改后 - 快照 DataStatus: {((tb_SaleOrder)snapshot).DataStatus}");

            // 恢复（模拟 RestoreAll）
            var entityType = order.GetType();
            Console.WriteLine($"\n恢复时使用类型: {entityType.FullName}");
            
            var restoreProps = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int restoredCount = 0;
            foreach (var prop in restoreProps)
            {
                if (!prop.CanWrite || !prop.CanRead) continue;
                
                var sugarColumn = prop.GetCustomAttribute<SqlSugar.SugarColumn>();
                if (sugarColumn?.IsIgnore == true) continue;
                
                var navigateAttr = prop.GetCustomAttribute<SqlSugar.Navigate>();
                if (navigateAttr != null) continue;

                try
                {
                    var value = prop.GetValue(snapshot);
                    prop.SetValue(order, value);
                    restoredCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"恢复属性 {prop.Name} 失败: {ex.Message}");
                }
            }

            Console.WriteLine($"\n恢复了 {restoredCount} 个属性");
            Console.WriteLine($"恢复后 - 原实体 DataStatus: {order.DataStatus}");
            Console.WriteLine($"恢复后 - 原实体 ApprovalStatus: {order.ApprovalStatus}");
            Console.WriteLine($"恢复后 - 原实体 SOrderNo: {order.SOrderNo}");

            Console.WriteLine("\n=== 测试完成 ===");
            Console.ReadKey();
        }
    }
}
