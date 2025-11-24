// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/24/2025 14:16:27
// **************************************
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using RUINORERP.Services;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;

using SqlSugar;
using System.Threading;

namespace RUINORERP.Business
{
    /// <summary>
    /// 导航节点控制器
    /// </summary>
    public partial class tb_ProcessNavigationNodeController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 获取导航节点列表
        /// </summary>
        /// <param name="navigationId">导航ID</param>
        /// <returns>导航节点列表</returns>
        public async Task<List<tb_ProcessNavigationNode>> GetNavigationNodesAsync(long navigationId)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                var nodes = await db.Queryable<tb_ProcessNavigationNode>()
                    .Where(n => n.ProcessNavID == navigationId)
                    .OrderBy(n => n.SortOrder)
                    .ToListAsync();
                return nodes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取导航节点列表失败(NavigationId:{navigationId})");
                throw new Exception("获取导航节点列表失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 添加导航节点
        /// </summary>
        /// <param name="node">导航节点信息</param>
        /// <returns>添加的导航节点</returns>
        public async Task<tb_ProcessNavigationNode> AddNodeAsync(tb_ProcessNavigationNode node)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                await db.Insertable(node).ExecuteCommandAsync();
                return node;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"添加导航节点失败(NodeName:{node.NodeName})");
                throw new Exception("添加导航节点失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 更新导航节点
        /// </summary>
        /// <param name="node">导航节点信息</param>
        /// <returns>更新结果</returns>
        public async Task<tb_ProcessNavigationNode> UpdateNodeAsync(tb_ProcessNavigationNode node)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                await db.Updateable(node).ExecuteCommandAsync();
                return node;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新导航节点失败(NodeId:{node.NodeID})");
                throw new Exception("更新导航节点失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 删除导航节点
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteNodeAsync(long nodeId)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                
                // 获取节点信息以确定导航ID
                var node = await db.Queryable<tb_ProcessNavigationNode>()
                    .Where(n => n.NodeID == nodeId)
                    .FirstAsync();
                
                if (node == null)
                {
                    return false;
                }
                
                var result = await db.Deleteable<tb_ProcessNavigationNode>(nodeId).ExecuteCommandAsync() > 0;
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除导航节点失败(NodeId:{nodeId})");
                throw new Exception("删除导航节点失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 清空导航的所有节点
        /// </summary>
        /// <param name="navigationId">导航ID</param>
        /// <returns>清空结果</returns>
        public async Task<bool> ClearNavigationNodesAsync(long navigationId)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                var result = await db.Deleteable<tb_ProcessNavigationNode>()
                    .Where(n => n.ProcessNavID == navigationId)
                    .ExecuteCommandAsync() > 0;
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"清空导航节点失败(NavigationId:{navigationId})");
                throw new Exception("清空导航节点失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 验证导航节点
        /// </summary>
        /// <param name="node">导航节点信息</param>
        /// <returns>验证结果</returns>
        public bool ValidateNode(tb_ProcessNavigationNode node)
        {
            try
            {
                if (node == null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                if (string.IsNullOrEmpty(node.NodeName))
                {
                    throw new ArgumentException("节点名称不能为空");
                }

                if (node.ProcessNavID <= 0)
                {
                    throw new ArgumentException("导航ID必须大于0");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"验证导航节点失败");
                throw new Exception("验证导航节点失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 获取导航节点的父节点
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>父节点信息</returns>
        public async Task<tb_ProcessNavigationNode> GetParentNodeAsync(long nodeId)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                var node = await db.Queryable<tb_ProcessNavigationNode>()
                    .Where(n => n.NodeID == nodeId)
                    .FirstAsync();

                // 实体类中没有ParentNodeId属性，返回null表示没有父节点
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取父节点失败(NodeId:{nodeId})");
                throw new Exception("获取父节点失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 获取导航节点的子节点
        /// </summary>
        /// <param name="parentNodeId">父节点ID</param>
        /// <returns>子节点列表</returns>
        public async Task<List<tb_ProcessNavigationNode>> GetChildNodesAsync(long parentNodeId)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                // 实体类中没有ParentNodeId属性，返回空列表
                var nodes = await db.Queryable<tb_ProcessNavigationNode>()
                    .Where(n => n.ProcessNavID > 0) // 仅作为占位条件，实际应该根据业务需求调整
                    .OrderBy(n => n.SortOrder)
                    .ToListAsync();
                return nodes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取子节点列表失败(ParentNodeId:{parentNodeId})");
                throw new Exception("获取子节点列表失败，请联系管理员", ex);
            }
        }
    }
}



