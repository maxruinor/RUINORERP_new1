using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache.Attributes;
using RUINORERP.Model.Base;

namespace RUINORERP.Business.Cache.Example
{
    /// <summary>
    /// 部门实体（示例）
    /// 假设这是一个基础数据实体
    /// </summary>
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public long? ParentId { get; set; }
        public string DepartmentCode { get; set; }
        public int OrderIndex { get; set; }
    }

    /// <summary>
    /// 部门缓存接口（示例）
    /// 扩展基础的实体缓存服务接口，添加特定的查询方法
    /// </summary>
    public interface IDepartmentCache : IEntityCacheService<Department>
    {
        /// <summary>
        /// 根据部门代码获取部门
        /// </summary>
        /// <param name="code">部门代码</param>
        /// <returns>部门实体</returns>
        Department GetByCode(string code);

        /// <summary>
        /// 异步根据部门代码获取部门
        /// </summary>
        /// <param name="code">部门代码</param>
        /// <returns>部门实体</returns>
        Task<Department> GetByCodeAsync(string code);

        /// <summary>
        /// 获取子部门列表
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <returns>子部门列表</returns>
        List<Department> GetChildren(long? parentId);

        /// <summary>
        /// 异步获取子部门列表
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <returns>子部门列表</returns>
        Task<List<Department>> GetChildrenAsync(long? parentId);
    }

    /// <summary>
    /// 部门缓存实现（示例）
    /// 展示如何扩展基础数据缓存功能
    /// </summary>
    [CacheName("Department")]
    public class DepartmentCache : EntityCacheServiceBase<Department>, IDepartmentCache
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="defaultPolicy">默认缓存策略</param>
        public DepartmentCache(ICacheManager<object> cacheManager, ILogger<DepartmentCache> logger, CachePolicy defaultPolicy = null)
            : base(cacheManager, logger, "Department", defaultPolicy)
        {}

        /// <summary>
        /// 生成部门代码缓存键
        /// </summary>
        /// <param name="code">部门代码</param>
        /// <returns>缓存键</returns>
        private string GetCodeKey(string code)
        {
            return $"Code:{code}";
        }

        /// <summary>
        /// 生成父部门ID缓存键
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <returns>缓存键</returns>
        private string GetParentKey(long? parentId)
        {
            return $"Parent:{parentId ?? 0}";
        }

        /// <summary>
        /// 根据部门代码获取部门
        /// </summary>
        /// <param name="code">部门代码</param>
        /// <returns>部门实体</returns>
        public Department GetByCode(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return null;

                // 先尝试从代码索引获取部门ID
                var codeKey = GetCodeKey(code);
                var departmentId = Get<long?>(codeKey);

                if (departmentId.HasValue)
                {
                    // 如果有索引，直接获取部门实体
                    return GetEntity(departmentId.Value);
                }

                // 如果没有索引，遍历所有部门查找
                var allDepartments = GetEntityList();
                var department = allDepartments.FirstOrDefault(d => d.DepartmentCode == code);

                if (department != null)
                {
                    // 缓存代码索引
                    Set(codeKey, department.PrimaryKeyID);
                }

                return department;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"根据代码获取部门失败: {code}");
                return null;
            }
        }

        /// <summary>
        /// 异步根据部门代码获取部门
        /// </summary>
        /// <param name="code">部门代码</param>
        /// <returns>部门实体</returns>
        public async Task<Department> GetByCodeAsync(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return null;

                // 先尝试从代码索引获取部门ID
                var codeKey = GetCodeKey(code);
                var departmentId = await GetAsync<long?>(codeKey);

                if (departmentId.HasValue)
                {
                    // 如果有索引，直接获取部门实体
                    return await GetEntityAsync(departmentId.Value);
                }

                // 如果没有索引，遍历所有部门查找
                var allDepartments = await GetEntityListAsync();
                var department = allDepartments.FirstOrDefault(d => d.DepartmentCode == code);

                if (department != null)
                {
                    // 缓存代码索引
                    await SetAsync(codeKey, department.PrimaryKeyID);
                }

                return department;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"异步根据代码获取部门失败: {code}");
                return null;
            }
        }

        /// <summary>
        /// 获取子部门列表
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <returns>子部门列表</returns>
        public List<Department> GetChildren(long? parentId)
        {
            try
            {
                // 生成父部门缓存键
                var parentKey = GetParentKey(parentId);
                var childIds = Get<List<long>>(parentKey);

                if (childIds != null && childIds.Any())
                {
                    // 如果有子部门ID列表，批量获取子部门
                    var children = new List<Department>();
                    foreach (var id in childIds)
                    {
                        var child = GetEntity(id);
                        if (child != null)
                        {
                            children.Add(child);
                        }
                    }
                    return children.OrderBy(c => c.OrderIndex).ToList();
                }

                // 如果没有索引，遍历所有部门查找
                var allDepartments = GetEntityList();
                var childDepartments = allDepartments
                    .Where(d => d.ParentId == parentId)
                    .OrderBy(d => d.OrderIndex)
                    .ToList();

                if (childDepartments.Any())
                {
                    // 缓存子部门ID列表
                    Set(parentKey, childDepartments.Select(d => d.PrimaryKeyID).ToList());
                }

                return childDepartments;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"获取子部门列表失败: ParentId={parentId}");
                return new List<Department>();
            }
        }

        /// <summary>
        /// 异步获取子部门列表
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <returns>子部门列表</returns>
        public async Task<List<Department>> GetChildrenAsync(long? parentId)
        {
            try
            {
                // 生成父部门缓存键
                var parentKey = GetParentKey(parentId);
                var childIds = await GetAsync<List<long>>(parentKey);

                if (childIds != null && childIds.Any())
                {
                    // 如果有子部门ID列表，批量获取子部门
                    var children = new List<Department>();
                    foreach (var id in childIds)
                    {
                        var child = await GetEntityAsync(id);
                        if (child != null)
                        {
                            children.Add(child);
                        }
                    }
                    return children.OrderBy(c => c.OrderIndex).ToList();
                }

                // 如果没有索引，遍历所有部门查找
                var allDepartments = await GetEntityListAsync();
                var childDepartments = allDepartments
                    .Where(d => d.ParentId == parentId)
                    .OrderBy(d => d.OrderIndex)
                    .ToList();

                if (childDepartments.Any())
                {
                    // 缓存子部门ID列表
                    await SetAsync(parentKey, childDepartments.Select(d => d.PrimaryKeyID).ToList());
                }

                return childDepartments;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"异步获取子部门列表失败: ParentId={parentId}");
                return new List<Department>();
            }
        }

        // 重写基类方法，添加特定的缓存维护逻辑

        /// <summary>
        /// 重写设置实体列表方法，添加索引维护
        /// </summary>
        /// <param name="entities">部门列表</param>
        /// <param name="expiration">过期时间</param>
        public override void SetEntityList(List<Department> entities, System.TimeSpan? expiration = null)
        {
            base.SetEntityList(entities, expiration);

            // 维护部门代码和父部门索引
            MaintainIndexes(entities);
        }

        /// <summary>
        /// 重写异步设置实体列表方法，添加索引维护
        /// </summary>
        /// <param name="entities">部门列表</param>
        /// <param name="expiration">过期时间</param>
        public override async Task SetEntityListAsync(List<Department> entities, System.TimeSpan? expiration = null)
        {
            await base.SetEntityListAsync(entities, expiration);

            // 维护部门代码和父部门索引
            MaintainIndexes(entities);
        }

        /// <summary>
        /// 维护部门索引
        /// </summary>
        /// <param name="entities">部门列表</param>
        private void MaintainIndexes(List<Department> entities)
        {
            try
            {
                if (entities == null || !entities.Any())
                    return;

                // 维护部门代码索引
                foreach (var dept in entities)
                {
                    if (!string.IsNullOrEmpty(dept.DepartmentCode))
                    {
                        Set(GetCodeKey(dept.DepartmentCode), dept.PrimaryKeyID);
                    }
                }

                // 维护父部门索引
                var groupedByParent = entities.GroupBy(d => d.ParentId);
                foreach (var group in groupedByParent)
                {
                    var parentKey = GetParentKey(group.Key);
                    var childIds = group.Select(d => d.PrimaryKeyID).ToList();
                    Set(parentKey, childIds);
                }

                _logger.LogInformation("部门缓存索引维护完成");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "维护部门缓存索引失败");
            }
        }

        /// <summary>
        /// 清空缓存，同时清空所有索引
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            _logger.LogInformation("部门缓存及索引已清空");
        }
    }
}