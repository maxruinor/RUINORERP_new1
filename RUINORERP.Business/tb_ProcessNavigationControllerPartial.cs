// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/24/2025 14:16:25
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
using RUINORERP.Business.Cache;
using SqlSugar;


namespace RUINORERP.Business
{


    /// <summary>
    /// 流程导航图控制器部分类
    /// 处理流程导航图相关的数据库操作
    /// </summary>
    public partial class tb_ProcessNavigationController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 获取所有流程导航图
        /// </summary>
        /// <returns>流程导航图列表</returns>
        public async Task<List<tb_ProcessNavigation>> GetAllNavigationsAsync()
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .OrderBy(n => n.IsDefault, OrderByType.Desc)
                    .OrderBy(n => n.SortOrder) // 将ThenBy改为OrderBy
                    .OrderBy(n => n.ProcessNavName) // 将ThenBy改为OrderBy
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有流程导航图失败");
                throw new Exception("获取流程导航图列表失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 根据ID获取流程导航图
        /// </summary>
        /// <param name="id">流程导航图ID</param>
        /// <returns>流程导航图实体</returns>
        public async Task<tb_ProcessNavigation> GetNavigationByIdAsync(long id)
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ProcessNavID == id)
                    .FirstAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取流程导航图失败(ID:{id})");
                throw new Exception("获取流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 创建流程导航图
        /// </summary>
        /// <param name="navigation">流程导航图实体</param>
        /// <returns>创建的流程导航图</returns>
        public async Task<tb_ProcessNavigation> CreateNavigationAsync(tb_ProcessNavigation navigation)
        {
            try
            {
                // 验证数据
                if (string.IsNullOrEmpty(navigation.ProcessNavName))
                {
                    throw new ArgumentException("流程导航图名称不能为空");
                }

                // 检查名称是否重复
                bool exists = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ProcessNavName == navigation.ProcessNavName && n.ParentNavigationID == navigation.ParentNavigationID)
                    .AnyAsync();

                if (exists)
                {
                    throw new ArgumentException("同一层级下流程导航图名称已存在");
                }

                // 设置创建和更新时间
                navigation.CreateTime = DateTime.Now;
                navigation.UpdateTime = DateTime.Now;

                // 如果是默认导航图，先重置其他默认导航图
                if (navigation.IsDefault)
                {
                    await ResetDefaultNavigationsAsync();
                }

                // 插入数据
                await _unitOfWorkManage.GetDbClient().Insertable(navigation).ExecuteCommandIdentityIntoEntityAsync(); // 将ExecuteAsync改为ExecuteCommandIdentityAsync
                _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(navigation);
                return navigation;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"创建流程导航图失败(Name:{navigation.ProcessNavName})");
                throw new Exception("创建流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 更新流程导航图
        /// </summary>
        /// <param name="navigation">流程导航图实体</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateNavigationAsync(tb_ProcessNavigation navigation)
        {
            try
            {
                // 验证数据
                if (string.IsNullOrEmpty(navigation.ProcessNavName))
                {
                    throw new ArgumentException("流程导航图名称不能为空");
                }

                // 检查名称是否重复
                bool exists = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ProcessNavName == navigation.ProcessNavName &&
                               n.ParentNavigationID == navigation.ParentNavigationID &&
                               n.ProcessNavID != navigation.ProcessNavID)
                    .AnyAsync();

                if (exists)
                {
                    throw new ArgumentException("同一层级下流程导航图名称已存在");
                }

                // 设置更新时间
                navigation.UpdateTime = DateTime.Now;

                // 如果是默认导航图，先重置其他默认导航图
                if (navigation.IsDefault)
                {
                    await ResetDefaultNavigationsAsync();
                }

                // 更新数据
                await _unitOfWorkManage.GetDbClient().Updateable(navigation).ExecuteCommandAsync(); // 将ExecuteAsync改为ExecuteCommandAsync
                return true;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新流程导航图失败(ID:{navigation.ProcessNavID})");
                throw new Exception("更新流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 删除流程导航图
        /// </summary>
        /// <param name="id">流程导航图ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteNavigationAsync(long id)
        {
            try
            {
                // 检查是否存在子流程
                bool hasChildren = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ParentNavigationID == id)
                    .AnyAsync();

                if (hasChildren)
                {
                    throw new ArgumentException("该流程导航图下存在子流程，无法删除");
                }

                // 检查是否有流程导航节点
                bool hasNodes = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigationNode>()
                    .Where(n => n.ProcessNavID == id)
                    .AnyAsync();

                if (hasNodes)
                {
                    throw new ArgumentException("该流程导航图下存在节点，无法删除");
                }

                // 删除流程导航图
                var entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>().Where(n => n.ProcessNavID == id).FirstAsync();
                await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProcessNavigation>()
                    .Where(n => n.ProcessNavID == id)
                    .ExecuteCommandAsync(); // 将ExecuteAsync改为ExecuteCommandAsync
                if (entity != null)
                {
                    _eventDrivenCacheManager.UpdateEntity<tb_ProcessNavigation>(entity);
                }
                return true;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除流程导航图失败(ID:{id})");
                throw new Exception("删除流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 获取默认流程导航图
        /// </summary>
        /// <returns>默认流程导航图实体</returns>
        public async Task<tb_ProcessNavigation> GetDefaultNavigationAsync()
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.IsDefault)
                    .FirstAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取默认流程导航图失败");
                throw new Exception("获取默认流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 设置默认流程导航图
        /// </summary>
        /// <param name="id">流程导航图ID</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetDefaultNavigationAsync(long id)
        {
            try
            {
                // 检查流程导航图是否存在
                var navigation = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ProcessNavID == id)
                    .FirstAsync();

                if (navigation == null)
                {
                    throw new ArgumentException("流程导航图不存在");
                }

                // 重置所有默认导航图
                await ResetDefaultNavigationsAsync();

                // 设置新的默认导航图
                navigation.IsDefault = true;
                navigation.UpdateTime = DateTime.Now;
                await _unitOfWorkManage.GetDbClient().Updateable(navigation).ExecuteCommandAsync();



                return true;
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置默认流程导航图失败(ID:{id})");
                throw new Exception("设置默认流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 重置所有默认流程导航图
        /// </summary>
        private async Task ResetDefaultNavigationsAsync()
        {
            try
            {
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ProcessNavigation>()
                    .SetColumns(n => n.IsDefault == false)
                    .Where(n => n.IsDefault)
                    .ExecuteCommandAsync(); // 将ExecuteAsync改为ExecuteCommandAsync
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置默认流程导航图失败");
                throw new Exception("重置默认流程导航图失败，请联系管理员", ex);
            }
        }

        #region 层级关系管理相关方法

        /// <summary>
        /// 获取流程导航图的子流程
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <returns>子流程导航图列表</returns>
        public async Task<List<tb_ProcessNavigation>> GetChildNavigationsAsync(long navigationId)
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ParentNavigationID == navigationId)
                    .OrderBy(n => n.SortOrder)
                    .OrderBy(n => n.ProcessNavName) // 将ThenBy改为OrderBy
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取子流程导航图失败(ID:{navigationId})");
                throw new Exception("获取子流程导航图失败，请联系管理员", ex);
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
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ParentNavigationID == null)
                    .OrderBy(n => n.IsDefault, OrderByType.Desc)
                    .OrderBy(n => n.SortOrder) // 将ThenBy改为OrderBy
                    .OrderBy(n => n.ProcessNavName) // 将ThenBy改为OrderBy
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("获取根级流程导航图失败，请联系管理员", ex);
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
                var navigation = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ProcessNavID == navigationId)
                    .FirstAsync();

                if (navigation == null)
                {
                    throw new ArgumentException("流程导航图不存在");
                }

                return await _unitOfWorkManage.GetDbClient().Queryable<tb_ProcessNavigation>()
                    .Where(n => n.ParentNavigationID == navigation.ParentNavigationID && n.ProcessNavID != navigationId)
                    .OrderBy(n => n.SortOrder)
                    .OrderBy(n => n.ProcessNavName)
                    .ToListAsync();
            }
            catch (ArgumentException ex)
            {
                throw ex; // 验证错误直接抛出
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取同级流程导航图失败(ID:{navigationId})");
                throw new Exception("获取同级流程导航图失败，请联系管理员", ex);
            }
        }

        #endregion

        #region 流程导航图持久化相关方法

        /// <summary>
        /// 保存流程导航图及其图形数据
        /// </summary>
        /// <param name="navigation">流程导航图实体</param>
        /// <param name="graphData">图形数据</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SaveNavigationWithGraphAsync(tb_ProcessNavigation navigation, string graphData)
        {
            try
            {
                // 保存流程导航图
                if (navigation.ProcessNavID == 0)
                {
                    // 新增
                    await CreateNavigationAsync(navigation);
                }
                else
                {
                    // 更新
                    await UpdateNavigationAsync(navigation);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"保存流程导航图及其图形数据失败(ID:{navigation.ProcessNavID})");
                throw new Exception("保存流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 加载流程导航图及其图形数据
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <returns>流程导航图实体</returns>
        public async Task<tb_ProcessNavigation> LoadNavigationWithGraphAsync(long navigationId)
        {
            try
            {
                return await GetNavigationByIdAsync(navigationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"加载流程导航图及其图形数据失败(ID:{navigationId})");
                throw new Exception("加载流程导航图失败，请联系管理员", ex);
            }
        }

        #endregion
    }
}



