using Microsoft.Extensions.Logging;
using Moq;
using RUINORERP.Business.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RUINORERP.Business.Tests.Cache
{
    public class BaseTableCacheManagerTests
    {
        private readonly Mock<IEntityCacheManager> _mockCacheManager;
        private readonly Mock<ICacheSyncMetadata> _mockCacheSyncMetadata;
        private readonly Mock<ILogger<BaseTableCacheManager>> _mockLogger;
        private readonly BaseTableCacheManager _baseTableCacheManager;

        public BaseTableCacheManagerTests()
        {
            // 初始化模拟对象
            _mockCacheManager = new Mock<IEntityCacheManager>();
            _mockCacheSyncMetadata = new Mock<ICacheSyncMetadata>();
            _mockLogger = new Mock<ILogger<BaseTableCacheManager>>();

            // 创建测试对象
            _baseTableCacheManager = new BaseTableCacheManager(
                _mockCacheManager.Object,
                _mockCacheSyncMetadata.Object,
                _mockLogger.Object);
        }

        [Fact]
        public void GetAllBaseTablesCacheInfo_ShouldReturnAllTablesInfo()
        {
            // 准备测试数据
            var syncInfos = new Dictionary<string, CacheSyncInfo>
            {
                { "Employees", new CacheSyncInfo("Employees", 100, DateTime.Now, 1024, DateTime.Now.AddHours(1)) },
                { "Departments", new CacheSyncInfo("Departments", 10, DateTime.Now, 512, DateTime.Now.AddHours(1)) },
                { "Products", new CacheSyncInfo("Products", 500, DateTime.Now, 2048, DateTime.Now.AddHours(1)) }
            };

            _mockCacheSyncMetadata.Setup(m => m.GetAllTableSyncInfo()).Returns(syncInfos);

            // 执行测试
            var result = _baseTableCacheManager.GetAllBaseTablesCacheInfo();

            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, t => t.TableName == "Employees" && t.DataCount == 100);
            Assert.Contains(result, t => t.TableName == "Departments" && t.DataCount == 10);
            Assert.Contains(result, t => t.TableName == "Products" && t.DataCount == 500);

            _mockCacheSyncMetadata.Verify(m => m.GetAllTableSyncInfo(), Times.Once);
        }

        [Fact]
        public void GetBaseTableCacheInfo_ShouldReturnCorrectTableInfo()
        {
            // 准备测试数据
            var syncInfo = new CacheSyncInfo("Employees", 100, DateTime.Now, 1024, DateTime.Now.AddHours(1));
            _mockCacheSyncMetadata.Setup(m => m.GetTableSyncInfo("Employees")).Returns(syncInfo);

            // 执行测试
            var result = _baseTableCacheManager.GetBaseTableCacheInfo("Employees");

            // 验证结果
            Assert.NotNull(result);
            Assert.Equal("Employees", result.TableName);
            Assert.Equal(100, result.DataCount);
            Assert.Equal(syncInfo.LastUpdateTime, result.LastUpdateTime);
            Assert.Equal(syncInfo.ExpirationTime, result.ExpirationTime);

            _mockCacheSyncMetadata.Verify(m => m.GetTableSyncInfo("Employees"), Times.Once);
        }

        [Fact]
        public void ValidateTableCacheIntegrity_ShouldReturnTrueWhenCacheIsComplete()
        {
            // 准备测试数据
            var syncInfo = new CacheSyncInfo("Employees", 100, DateTime.Now, 1024, DateTime.Now.AddHours(1));
            _mockCacheSyncMetadata.Setup(m => m.GetTableSyncInfo("Employees")).Returns(syncInfo);

            // 模拟缓存中的实体列表
            var mockEntities = Enumerable.Range(1, 100).Select(i => new { Id = i }).ToList<object>();
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Employees")).Returns(mockEntities);

            // 执行测试
            bool result = _baseTableCacheManager.ValidateTableCacheIntegrity("Employees");

            // 验证结果
            Assert.True(result);
            _mockCacheSyncMetadata.Verify(m => m.GetTableSyncInfo("Employees"), Times.Once);
            _mockCacheManager.Verify(m => m.GetEntityList<object>("Employees"), Times.Once);
        }

        [Fact]
        public void ValidateTableCacheIntegrity_ShouldReturnFalseWhenCacheCountMismatch()
        {
            // 准备测试数据
            var syncInfo = new CacheSyncInfo("Employees", 100, DateTime.Now, 1024, DateTime.Now.AddHours(1));
            _mockCacheSyncMetadata.Setup(m => m.GetTableSyncInfo("Employees")).Returns(syncInfo);

            // 模拟缓存中的实体列表数量不匹配
            var mockEntities = Enumerable.Range(1, 50).Select(i => new { Id = i }).ToList<object>();
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Employees")).Returns(mockEntities);

            // 执行测试
            bool result = _baseTableCacheManager.ValidateTableCacheIntegrity("Employees");

            // 验证结果
            Assert.False(result);
        }

        [Fact]
        public void ValidateTableCacheIntegrity_ShouldReturnFalseWhenCacheIsEmpty()
        {
            // 准备测试数据
            var syncInfo = new CacheSyncInfo("Employees", 100, DateTime.Now, 1024, DateTime.Now.AddHours(1));
            _mockCacheSyncMetadata.Setup(m => m.GetTableSyncInfo("Employees")).Returns(syncInfo);

            // 模拟空缓存
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Employees")).Returns(new List<object>());

            // 执行测试
            bool result = _baseTableCacheManager.ValidateTableCacheIntegrity("Employees");

            // 验证结果
            Assert.False(result);
        }

        [Fact]
        public void GetTablesWithIncompleteCache_ShouldReturnIncompleteTables()
        {
            // 准备测试数据
            var syncInfos = new Dictionary<string, CacheSyncInfo>
            {
                { "Employees", new CacheSyncInfo("Employees", 100, DateTime.Now, 1024, DateTime.Now.AddHours(1)) },
                { "Departments", new CacheSyncInfo("Departments", 10, DateTime.Now, 512, DateTime.Now.AddHours(1)) },
                { "Products", new CacheSyncInfo("Products", 500, DateTime.Now, 2048, DateTime.Now.AddHours(1)) }
            };

            _mockCacheSyncMetadata.Setup(m => m.GetAllTableSyncInfo()).Returns(syncInfos);

            // 设置不同的缓存状态
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Employees")).Returns(Enumerable.Range(1, 100).Select(i => new { Id = i }).ToList<object>());
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Departments")).Returns(new List<object>()); // 空缓存
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Products")).Returns(Enumerable.Range(1, 400).Select(i => new { Id = i }).ToList<object>()); // 数量不匹配

            // 执行测试
            var result = _baseTableCacheManager.GetTablesWithIncompleteCache();

            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains("Departments", result);
            Assert.Contains("Products", result);
            Assert.DoesNotContain("Employees", result);
        }

        [Fact]
        public void UpdateBaseTableCache_ShouldReturnTrueWhenUpdateAndValidationSucceed()
        {
            // 准备测试数据
            var employees = Enumerable.Range(1, 100).Select(i => new { Id = i, Name = $"Employee {i}" }).ToList();

            // 设置UpdateEntityList不抛出异常
            _mockCacheManager.Setup(m => m.UpdateEntityList("Employees", employees)).Verifiable();

            // 模拟验证时的缓存状态
            var syncInfo = new CacheSyncInfo("Employees", 100, DateTime.Now, 1024, DateTime.Now.AddHours(1));
            _mockCacheSyncMetadata.Setup(m => m.GetTableSyncInfo("Employees")).Returns(syncInfo);
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Employees")).Returns(employees.Cast<object>().ToList());

            // 执行测试
            bool result = _baseTableCacheManager.UpdateBaseTableCache("Employees", employees);

            // 验证结果
            Assert.True(result);
            _mockCacheManager.Verify(m => m.UpdateEntityList("Employees", employees), Times.Once);
        }

        [Fact]
        public void RefreshIncompleteTables_ShouldRefreshOnlyIncompleteTables()
        {
            // 准备测试数据
            var syncInfos = new Dictionary<string, CacheSyncInfo>
            {
                { "Employees", new CacheSyncInfo("Employees", 100, DateTime.Now, 1024, DateTime.Now.AddHours(1)) },
                { "Departments", new CacheSyncInfo("Departments", 10, DateTime.Now, 512, DateTime.Now.AddHours(1)) }
            };

            _mockCacheSyncMetadata.Setup(m => m.GetAllTableSyncInfo()).Returns(syncInfos);

            // 设置初始缓存状态
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Employees")).Returns(new List<object>()); // 空缓存
            _mockCacheManager.Setup(m => m.GetEntityList<object>("Departments")).Returns(Enumerable.Range(1, 10).Select(i => new { Id = i }).ToList<object>()); // 完整缓存

            // 跟踪刷新操作的调用
            int refreshCallCount = 0;
            string lastRefreshedTable = null;

            Action<string> refreshAction = (tableName) =>
            {
                refreshCallCount++;
                lastRefreshedTable = tableName;

                // 模拟刷新后的数据
                if (tableName == "Employees")
                {
                    _mockCacheManager.Setup(m => m.GetEntityList<object>("Employees")).Returns(
                        Enumerable.Range(1, 100).Select(i => new { Id = i }).ToList<object>());
                    
                    // 更新缓存
                    _mockCacheManager.Object.UpdateEntityList(tableName, Enumerable.Range(1, 100).Select(i => new { Id = i }).ToList());
                }
            };

            // 执行测试
            int result = _baseTableCacheManager.RefreshIncompleteTables(refreshAction);

            // 验证结果
            Assert.Equal(1, refreshCallCount); // 只有Employees表需要刷新
            Assert.Equal("Employees", lastRefreshedTable);
            Assert.Equal(1, result); // 成功刷新了1个表
        }

        [Fact]
        public void BaseTableCacheInfo_PropertiesShouldWorkCorrectly()
        {
            // 创建测试对象
            var cacheInfo = new BaseTableCacheInfo
            {
                TableName = "TestTable",
                DataCount = 50,
                LastUpdateTime = DateTime.Now,
                ExpirationTime = DateTime.Now.AddHours(1),
                HasExpiration = true,
                EstimatedSize = 2048,
                SourceInfo = "TestSource"
            };

            // 验证属性
            Assert.Equal("TestTable", cacheInfo.TableName);
            Assert.Equal(50, cacheInfo.DataCount);
            Assert.True(cacheInfo.HasExpiration);
            Assert.False(cacheInfo.IsExpired); // 尚未过期
            Assert.False(cacheInfo.IsEmpty); // 有数据
            Assert.Equal("2.00 KB", cacheInfo.ReadableSize);
            Assert.Equal("正常", cacheInfo.StatusDescription);

            // 测试空缓存
            cacheInfo.DataCount = 0;
            Assert.True(cacheInfo.IsEmpty);
            Assert.Equal("空缓存", cacheInfo.StatusDescription);

            // 测试过期缓存
            cacheInfo.DataCount = 50;
            cacheInfo.ExpirationTime = DateTime.Now.AddHours(-1);
            Assert.True(cacheInfo.IsExpired);
            Assert.Equal("已过期", cacheInfo.StatusDescription);
        }
    }
}