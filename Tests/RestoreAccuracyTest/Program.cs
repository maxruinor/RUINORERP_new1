using System;
using System.Diagnostics;
using RUINORERP.Common.Helper;

namespace EntityStateProtectorTests
{
    /// <summary>
    /// 实体状态保护器 - 恢复准确性测试
    /// </summary>
    class RestoreAccuracyTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 实体状态保护器 - 恢复准确性测试 ===\n");

            // 测试1：基本类型恢复
            TestBasicTypesRestore();

            // 测试2：枚举类型恢复
            TestEnumTypeRestore();

            // 测试3：可空类型恢复
            TestNullableTypesRestore();

            // 测试4：集合类型恢复
            TestCollectionRestore();

            // 测试5：完整实体恢复流程（模拟EntityStateProtector）
            TestFullEntityRestoreFlow();

            Console.WriteLine("\n=== 所有测试完成 ===");
            Console.ReadKey();
        }

        /// <summary>
        /// 测试1：基本类型恢复
        /// </summary>
        static void TestBasicTypesRestore()
        {
            Console.WriteLine("测试1：基本类型恢复");
            Console.WriteLine("------------------");

            var original = new TestEntity
            {
                ID = 1,
                Name = "原始名称",
                Amount = 1000.50m,
                CreateTime = new DateTime(2024, 1, 1),
                IsActive = true
            };

            // 创建快照
            var snapshot = CloneHelper.DeepCloneObject(original);

            // 修改原实体
            original.ID = 999;
            original.Name = "修改后的名称";
            original.Amount = 9999.99m;
            original.CreateTime = DateTime.Now;
            original.IsActive = false;

            Console.WriteLine($"修改后 - 原实体: ID={original.ID}, Name={original.Name}, Amount={original.Amount}");

            // 恢复
            var stopwatch = Stopwatch.StartNew();
            CloneHelper.SetValues<TestEntity>(original, snapshot);
            stopwatch.Stop();

            Console.WriteLine($"恢复耗时: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"恢复后 - 原实体: ID={original.ID}, Name={original.Name}, Amount={original.Amount}");
            Console.WriteLine($"快照值:     ID={snapshot.ID}, Name={snapshot.Name}, Amount={snapshot.Amount}");

            // 验证恢复是否正确
            bool restored = (original.ID == snapshot.ID &&
                            original.Name == snapshot.Name &&
                            original.Amount == snapshot.Amount &&
                            original.CreateTime == snapshot.CreateTime &&
                            original.IsActive == snapshot.IsActive);

            Console.WriteLine($"测试结果: {(restored ? "✅ 通过 - 所有属性正确恢复" : "❌ 失败 - 恢复不完整")}\n");
        }

        /// <summary>
        /// 测试2：枚举类型恢复
        /// </summary>
        static void TestEnumTypeRestore()
        {
            Console.WriteLine("测试2：枚举类型恢复");
            Console.WriteLine("------------------");

            var original = new TestEntity
            {
                Status = TestStatus.Active,
                Priority = TestPriority.High
            };

            var snapshot = CloneHelper.DeepCloneObject(original);

            // 修改
            original.Status = TestStatus.Inactive;
            original.Priority = TestPriority.Low;

            Console.WriteLine($"修改后 - 原实体: Status={original.Status}, Priority={original.Priority}");

            // 恢复
            CloneHelper.SetValues<TestEntity>(original, snapshot);

            Console.WriteLine($"恢复后 - 原实体: Status={original.Status}, Priority={original.Priority}");

            bool restored = (original.Status == snapshot.Status && original.Priority == snapshot.Priority);
            Console.WriteLine($"测试结果: {(restored ? "✅ 通过" : "❌ 失败")}\n");
        }

        /// <summary>
        /// 测试3：可空类型恢复
        /// </summary>
        static void TestNullableTypesRestore()
        {
            Console.WriteLine("测试3：可空类型恢复");
            Console.WriteLine("------------------");

            var original = new TestEntity
            {
                NullableInt = 100,
                NullableDecimal = 500.25m,
                NullableDateTime = new DateTime(2024, 6, 15)
            };

            var snapshot = CloneHelper.DeepCloneObject(original);

            // 修改为null
            original.NullableInt = null;
            original.NullableDecimal = null;
            original.NullableDateTime = null;

            Console.WriteLine($"修改后 - 原实体: NullableInt={original.NullableInt?.ToString() ?? "null"}, NullableDecimal={original.NullableDecimal?.ToString() ?? "null"}");

            // 恢复
            CloneHelper.SetValues<TestEntity>(original, snapshot);

            Console.WriteLine($"恢复后 - 原实体: NullableInt={original.NullableInt?.ToString() ?? "null"}, NullableDecimal={original.NullableDecimal?.ToString() ?? "null"}");

            bool restored = (original.NullableInt == snapshot.NullableInt &&
                            original.NullableDecimal == snapshot.NullableDecimal &&
                            original.NullableDateTime == snapshot.NullableDateTime);

            Console.WriteLine($"测试结果: {(restored ? "✅ 通过" : "❌ 失败")}\n");
        }

        /// <summary>
        /// 测试4：集合类型恢复
        /// </summary>
        static void TestCollectionRestore()
        {
            Console.WriteLine("测试4：集合类型恢复");
            Console.WriteLine("------------------");

            var original = new TestEntity();
            original.Items.Add(new TestItem { ID = 1, Name = "项目A" });
            original.Items.Add(new TestItem { ID = 2, Name = "项目B" });

            var snapshot = CloneHelper.DeepCloneObject(original);

            // 修改集合
            original.Items.Clear();
            original.Items.Add(new TestItem { ID = 3, Name = "项目C" });

            Console.WriteLine($"修改后 - 原实体 Items.Count={original.Items.Count}");
            Console.WriteLine($"快照   Items.Count={snapshot.Items.Count}");

            // 恢复
            CloneHelper.SetValues<TestEntity>(original, snapshot);

            Console.WriteLine($"恢复后 - 原实体 Items.Count={original.Items.Count}");

            bool restored = (original.Items.Count == snapshot.Items.Count);
            if (restored && original.Items.Count > 0)
            {
                restored = (original.Items[0].ID == snapshot.Items[0].ID &&
                           original.Items[0].Name == snapshot.Items[0].Name);
            }

            Console.WriteLine($"测试结果: {(restored ? "✅ 通过" : "❌ 失败")}\n");
        }

        /// <summary>
        /// 测试5：完整实体恢复流程（模拟EntityStateProtector）
        /// </summary>
        static void TestFullEntityRestoreFlow()
        {
            Console.WriteLine("测试5：完整实体恢复流程（模拟EntityStateProtector）");
            Console.WriteLine("----------------------------------------------");

            var entity = new TestEntity
            {
                ID = 1,
                Name = "订单001",
                DataStatus = 2,  // 确认状态
                ApprovalStatus = 1,  // 审核通过
                Amount = 5000.00m
            };

            Console.WriteLine($"初始状态: DataStatus={entity.DataStatus}, ApprovalStatus={entity.ApprovalStatus}");

            // 模拟 BeginTranWithProtection - 保存快照
            var snapshot = CloneHelper.DeepCloneObject(entity);
            Console.WriteLine("已创建快照");

            // 模拟业务逻辑 - 修改状态
            entity.DataStatus = 5;  // 完结
            entity.ApprovalStatus = 2;  // 审核拒绝
            Console.WriteLine($"业务处理后: DataStatus={entity.DataStatus}, ApprovalStatus={entity.ApprovalStatus}");

            // 模拟 RollbackTranWithRestore - 恢复状态
            var stopwatch = Stopwatch.StartNew();
            CloneHelper.SetValues<TestEntity>(entity, snapshot);
            stopwatch.Stop();

            Console.WriteLine($"恢复耗时: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"恢复后: DataStatus={entity.DataStatus}, ApprovalStatus={entity.ApprovalStatus}");

            bool restored = (entity.DataStatus == 2 && entity.ApprovalStatus == 1);
            Console.WriteLine($"测试结果: {(restored ? "✅ 通过 - 状态完全恢复" : "❌ 失败 - 状态恢复错误")}\n");
        }
    }

    #region 测试实体类

    [Serializable]
    public class TestEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsActive { get; set; }
        
        // 枚举
        public TestStatus Status { get; set; }
        public TestPriority Priority { get; set; }
        
        // 可空类型
        public int? NullableInt { get; set; }
        public decimal? NullableDecimal { get; set; }
        public DateTime? NullableDateTime { get; set; }
        
        // 业务状态
        public int DataStatus { get; set; }
        public int ApprovalStatus { get; set; }
        
        // 集合
        public System.Collections.Generic.List<TestItem> Items { get; set; } = new System.Collections.Generic.List<TestItem>();
    }

    [Serializable]
    public class TestItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public enum TestStatus
    {
        Active = 1,
        Inactive = 2,
        Pending = 3
    }

    public enum TestPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    #endregion
}
