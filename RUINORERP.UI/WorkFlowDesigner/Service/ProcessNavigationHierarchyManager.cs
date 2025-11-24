using Microsoft.Extensions.Logging;
using NPOI.POIFS.Properties;
using RUINORERP.Business;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.UI.WorkFlowDesigner.Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlowDesigner.Service
{
    /// <summary>
    /// 流程导航图分层管理器
    /// 负责管理流程导航图的层级关系，支持多级导航结构
    /// </summary>
    public class ProcessNavigationHierarchyManager
    {
        private readonly tb_ProcessNavigationController<tb_ProcessNavigation> _navigationController;
        private readonly tb_ProcessNavigationNodeController<tb_ProcessNavigationNode> _navigationNodeController;
        private readonly ILogger<ProcessNavigationHierarchyManager> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="navigationController">流程导航图控制器</param>
        /// <param name="navigationNodeController">流程导航图节点控制器</param>
        public ProcessNavigationHierarchyManager(tb_ProcessNavigationController<tb_ProcessNavigation> navigationController, tb_ProcessNavigationNodeController<tb_ProcessNavigationNode> navigationNodeController, ILogger<ProcessNavigationHierarchyManager> logger = null)
        {
            _navigationController = navigationController;
            _navigationNodeController = navigationNodeController;
            _logger = logger;
        }

        #region 层级关系管理

        /// <summary>
        /// 设置流程导航图的父流程
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="parentId">父流程导航图ID</param>
        public async Task SetParentNavigationAsync(long navigationId, long? parentId)
        {
            try
            {
                // 验证流程导航图是否存在
                var navigation = await _navigationController.GetNavigationByIdAsync(navigationId);
                if (navigation == null)
                {
                    throw new ArgumentException("流程导航图不存在");
                }

                //// 避免循环引用
                //if (parentId.HasValue && await HasCircularReferenceAsync(parentId.Value, navigationId))
                //{
                //    throw new ArgumentException("设置父流程会导致循环引用");
                //}

                // 获取父流程，验证是否存在
                if (parentId.HasValue)
                {
                    var parentNavigation = await _navigationController.GetNavigationByIdAsync(parentId.Value);
                    if (parentNavigation == null)
                    {
                        throw new ArgumentException("父流程导航图不存在");
                    }
                }

                // 设置父流程
                navigation.ParentNavigationID = parentId;
                await _navigationController.UpdateAsync(navigation);


            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置父流程导航图失败(子ID:{navigationId},父ID:{parentId})");
                throw new Exception("设置父流程失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 获取流程导航图的父流程
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <returns>父流程导航图实体</returns>
        public async Task<tb_ProcessNavigation> GetParentNavigationAsync(long navigationId)
        {
            try
            {
                var navigation = await _navigationController.GetNavigationByIdAsync(navigationId);
                if (navigation == null || !navigation.ParentNavigationID.HasValue)
                {
                    return null;
                }

                return await _navigationController.GetNavigationByIdAsync(navigation.ParentNavigationID.Value);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取父流程导航图失败(ID:{navigationId})");
                throw new Exception("获取父流程失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 获取流程导航图的所有子流程
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <returns>子流程导航图列表</returns>
        public async Task<List<tb_ProcessNavigation>> GetChildNavigationsAsync(long? parentId)
        {
            try
            {
                // 使用业务层控制器获取子流程
                return await _navigationController.QueryByNavAsync(c => c.ParentNavigationID == parentId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取子流程导航图失败(父ID:{parentId})");
                throw new Exception("获取子流程失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 获取流程导航图的所有子流程（递归）
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <returns>所有层级的子流程导航图列表</returns>
        public async Task<List<tb_ProcessNavigation>> GetAllChildNavigationsAsync(long navigationId)
        {
            try
            {
                var allChildren = new List<tb_ProcessNavigation>();
                await GetAllChildNavigationsRecursiveAsync(navigationId, allChildren);
                return allChildren;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "递归获取子流程导航图失败(ID:{navigationId})");
                throw new Exception("递归获取子流程失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 递归获取所有子流程
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="resultList">结果列表</param>
        private async Task GetAllChildNavigationsRecursiveAsync(long navigationId, List<tb_ProcessNavigation> resultList)
        {
            var children = await GetChildNavigationsAsync(navigationId);
            foreach (var child in children)
            {
                resultList.Add(child);
                await GetAllChildNavigationsRecursiveAsync(child.ProcessNavID, resultList);
            }
        }

        /// <summary>
        /// 获取流程导航图的所有父流程路径
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <returns>从根节点到当前节点的路径列表</returns>
        public async Task<List<tb_ProcessNavigation>> GetNavigationPathAsync(long navigationId)
        {
            try
            {
                var path = new List<tb_ProcessNavigation>();
                var current = await _navigationController.GetNavigationByIdAsync(navigationId);

                if (current == null)
                {
                    throw new ArgumentException("流程导航图不存在");
                }

                // 从当前节点向上递归构建路径
                while (current != null)
                {
                    path.Insert(0, current); // 在路径头部插入当前节点

                    if (current.ParentNavigationID.HasValue)
                    {
                        current = await _navigationController.GetNavigationByIdAsync(current.ParentNavigationID.Value);
                    }
                    else
                    {
                        current = null;
                    }
                }

                return path;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取流程导航图路径失败(ID:{navigationId})");
                throw new Exception("获取流程导航图路径失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 递归获取流程导航图路径
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="path">路径列表</param>
        private async Task GetNavigationPathRecursiveAsync(long navigationId, List<tb_ProcessNavigation> path)
        {
            var navigation = await _navigationController.GetNavigationByIdAsync(navigationId);
            if (navigation == null)
            {
                return;
            }

            path.Add(navigation);

            if (navigation.ParentNavigationID.HasValue)
            {
                await GetNavigationPathRecursiveAsync(navigation.ParentNavigationID.Value, path);
            }
        }

        /// <summary>
        /// 获取根级流程导航图（没有父流程的流程）
        /// </summary>
        /// <returns>根级流程导航图列表</returns>
        public async Task<List<tb_ProcessNavigation>> GetRootNavigationsAsync()
        {
            try
            {
                // 使用业务层控制器获取根级流程
                return await _navigationController.QueryByNavAsync(c => c.ParentNavigationID == 0);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取根级流程导航图失败");
                throw new Exception("获取根级流程导航图失败，请联系管理员", ex);
            }
        }

        #endregion

        #region 层级结构操作

        /// <summary>
        /// 更新流程导航图的排序顺序
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="sortOrder">排序序号</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateNavigationSortOrderAsync(long navigationId, int sortOrder)
        {
            try
            {
                var navigation = await _navigationController.GetNavigationByIdAsync(navigationId);
                if (navigation == null)
                {
                    throw new ArgumentException("流程导航图不存在");
                }

                navigation.SortOrder = sortOrder;
                await _navigationController.UpdateAsync(navigation);
                return true;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                throw new Exception("更新排序顺序失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 移动流程导航图到新的父流程下
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="newParentId">新父流程导航图ID</param>
        /// <returns>是否移动成功</returns>
        public async Task<bool> MoveNavigationAsync(long navigationId, long? newParentId)
        {
            try
            {
                // 检查是否移动到自身
                if (newParentId == navigationId)
                {
                    throw new ArgumentException("不能将流程导航图移动到自身下");
                }

                // 设置新的父流程
                await SetParentNavigationAsync(navigationId, newParentId);
                return true;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "移动流程导航图失败(ID:{navigationId}, NewParentID:{newParentId})");
                throw new Exception("移动流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 创建流程导航图的副本作为子流程
        /// </summary>
        /// <param name="sourceId">源流程导航图ID</param>
        /// <param name="parentId">父流程导航图ID</param>
        /// <param name="newName">新流程名称</param>
        /// <returns>新创建的子流程导航图</returns>
        public async Task<tb_ProcessNavigation> CreateChildCopyAsync(long sourceId, long parentId, string newName = null)
        {
            try
            {
                // 获取源流程
                var source = await _navigationController.GetNavigationByIdAsync(sourceId);
                if (source == null)
                {
                    throw new ArgumentException("源流程导航图不存在");
                }

                // 检查父流程是否存在
                var parent = await _navigationController.GetNavigationByIdAsync(parentId);
                if (parent == null)
                {
                    throw new ArgumentException("父流程导航图不存在");
                }

                // 创建副本
                var copy = new tb_ProcessNavigation
                {
                    ProcessNavName = string.IsNullOrEmpty(newName) ? $"{source.ProcessNavName} - 副本" : newName,
                    Description = source.Description,
                    GraphXml = source.GraphXml,
                    ModuleID = source.ModuleID,
                    IsDefault = false,
                    ParentNavigationID = parentId,
                    SortOrder = source.SortOrder,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                // 保存到数据库
                await _navigationController.SaveOrUpdate(copy);
                return copy;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建子流程副本失败(SourceID:{sourceId}, ParentID:{parentId})");
                throw new Exception("创建子流程副本失败，请联系管理员", ex);
            }
        }

        #endregion

        #region 验证与检查

        /// <summary>
        /// 检查是否存在循环引用
        /// </summary>
        /// <param name="parentId">父流程导航图ID</param>
        /// <param name="childId">子流程导航图ID</param>
        /// <returns>是否存在循环引用</returns>
        private async Task<bool> CheckCircularReferenceAsync(long parentId, long childId)
        {
            try
            {
                // 如果子流程已经是父流程的父流程，则存在循环引用
                var current = parentId;
                while (true)
                {
                    var navigation = await _navigationController.GetNavigationByIdAsync(current);
                    if (navigation == null || !navigation.ParentNavigationID.HasValue)
                    {
                        return false; // 到达根节点，没有循环引用
                    }

                    if (navigation.ParentNavigationID.Value == childId)
                    {
                        return true; // 发现循环引用
                    }

                    current = navigation.ParentNavigationID.Value;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查循环引用失败(ParentID:{parentId}, ChildID:{childId})");
                throw new Exception("检查层级关系失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 验证层级深度
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="maxDepth">最大允许深度</param>
        /// <returns>是否在允许的深度范围内</returns>
        public async Task<bool> ValidateHierarchyDepthAsync(long navigationId, int maxDepth = 5)
        {
            try
            {
                var depth = await GetNavigationDepthAsync(navigationId);
                return depth <= maxDepth;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证层级深度失败(ID:{navigationId})");
                throw new Exception("验证层级深度失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 获取流程导航图的深度
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <returns>深度值（根节点深度为0）</returns>
        public async Task<int> GetNavigationDepthAsync(long navigationId)
        {
            try
            {
                int depth = 0;
                var currentId = navigationId;

                while (true)
                {
                    var navigation = await _navigationController.GetNavigationByIdAsync(currentId);
                    if (navigation == null || !navigation.ParentNavigationID.HasValue)
                    {
                        break;
                    }

                    depth++;
                    currentId = navigation.ParentNavigationID.Value;
                }

                return depth;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取流程导航图深度失败(ID:{navigationId})");
                throw new Exception("获取流程导航图深度失败，请联系管理员", ex);
            }
        }

        #endregion

        #region 树形结构构建

        /// <summary>
        /// 构建完整的流程导航图树
        /// </summary>
        /// <returns>流程导航图树节点列表</returns>
        public async Task<List<ProcessNavigationTreeNode>> BuildNavigationTreeAsync()
        {
            try
            {
                // 获取所有流程导航图
                var allNavigations = await _navigationController.GetAllNavigationsAsync();

                // 构建ID到导航图的映射
                var navigationMap = allNavigations.ToDictionary(n => n.ProcessNavID, n => n);

                // 创建树节点列表
                var treeNodes = allNavigations.Select(n => new ProcessNavigationTreeNode
                {
                    Navigation = n,
                    Children = new List<ProcessNavigationTreeNode>()
                }).ToList();

                // 构建ID到树节点的映射
                var nodeMap = treeNodes.ToDictionary(n => n.Navigation.ProcessNavID, n => n);

                // 构建树形结构
                var rootNodes = new List<ProcessNavigationTreeNode>();

                foreach (var node in treeNodes)
                {
                    if (node.Navigation.ParentNavigationID.HasValue && nodeMap.TryGetValue(node.Navigation.ParentNavigationID.Value, out var parentNode))
                    {
                        // 添加到父节点的子节点列表
                        parentNode.Children.Add(node);
                        // 按排序顺序和名称排序子节点
                        parentNode.Children = parentNode.Children
                            .OrderBy(c => c.Navigation.SortOrder)
                            .ThenBy(c => c.Navigation.ProcessNavName)
                            .ToList();
                    }
                    else
                    {
                        // 根节点
                        rootNodes.Add(node);
                    }
                }

                // 按排序顺序和名称排序根节点
                return rootNodes
                    .OrderBy(n => n.Navigation.IsDefault ? 0 : 1) // 默认流程排在前面
                    .ThenBy(n => n.Navigation.SortOrder)
                    .ThenBy(n => n.Navigation.ProcessNavName)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建流程导航图树失败");
                throw new Exception("构建流程导航图树失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 构建指定父流程下的导航图树
        /// </summary>
        /// <param name="parentId">父流程导航图ID</param>
        /// <returns>流程导航图树节点</returns>
        public async Task<ProcessNavigationTreeNode> BuildSubNavigationTreeAsync(long parentId)
        {
            try
            {
                // 获取父流程
                var parent = await _navigationController.GetNavigationByIdAsync(parentId);
                if (parent == null)
                {
                    throw new ArgumentException("父流程导航图不存在");
                }

                // 构建树节点
                var rootNode = new ProcessNavigationTreeNode
                {
                    Navigation = parent,
                    Children = new List<ProcessNavigationTreeNode>()
                };

                // 递归构建子树
                await BuildSubTreeRecursiveAsync(rootNode);

                return rootNode;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建子导航树失败(ParentID:{parentId})");
                throw new Exception("构建子导航树失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 递归构建子树
        /// </summary>
        /// <param name="parentNode">父树节点</param>
        private async Task BuildSubTreeRecursiveAsync(ProcessNavigationTreeNode parentNode)
        {
            // 获取直接子流程
            var children = await GetChildNavigationsAsync(parentNode.Navigation.ProcessNavID);

            foreach (var child in children)
            {
                var childNode = new ProcessNavigationTreeNode
                {
                    Navigation = child,
                    Children = new List<ProcessNavigationTreeNode>()
                };

                parentNode.Children.Add(childNode);

                // 递归构建子节点的子树
                await BuildSubTreeRecursiveAsync(childNode);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取流程导航图的完整路径名称
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="separator">路径分隔符</param>
        /// <returns>完整路径名称</returns>
        public async Task<string> GetFullPathNameAsync(long navigationId, string separator = " / ")
        {
            try
            {
                var path = await GetNavigationPathAsync(navigationId);
                return string.Join(separator, path.Select(n => n.ProcessNavName));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取完整路径名称失败(ID:{navigationId})");
                throw new Exception("获取完整路径名称失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 获取同级流程导航图
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <returns>同级流程导航图列表</returns>
        public async Task<List<tb_ProcessNavigation>> GetSiblingNavigationsAsync(long navigationId)
        {
            try
            {
                var navigation = await _navigationController.GetNavigationByIdAsync(navigationId);
                if (navigation == null)
                {
                    throw new ArgumentException("流程导航图不存在");
                }

                // 使用业务层控制器获取同级流程
                return await _navigationController.QueryByNavAsync(c => c.ParentNavigationID == navigation.ParentNavigationID);

            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取同级流程导航图失败(ID:{navigationId})");
                throw new Exception("获取同级流程导航图失败，请联系管理员", ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// 流程导航图树节点
    /// 用于构建树形结构的数据模型
    /// </summary>
    public class ProcessNavigationTreeNode
    {
        /// <summary>
        /// 流程导航图实体
        /// </summary>
        public tb_ProcessNavigation Navigation { get; set; }

        /// <summary>
        /// 子节点列表
        /// </summary>
        public List<ProcessNavigationTreeNode> Children { get; set; }

        /// <summary>
        /// 是否展开节点
        /// </summary>
        public bool IsExpanded { get; set; } = false;

        /// <summary>
        /// 是否选中节点
        /// </summary>
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// 获取节点深度
        /// </summary>
        public int Depth => GetDepth(this, 0);

        /// <summary>
        /// 递归获取节点深度
        /// </summary>
        private int GetDepth(ProcessNavigationTreeNode node, int currentDepth)
        {
            if (node.Navigation == null || !node.Navigation.ParentNavigationID.HasValue)
            {
                return currentDepth;
            }

            // 这里应该通过父节点计算深度，但为了简化，假设深度为层级数
            // 实际应用中应该通过完整路径计算
            return currentDepth + 1;
        }
    }
}