using RUINORERP.Model;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.Business.CommService;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;

namespace RUINORERP.Business.LogicaService
{
    /// <summary>
    /// 提醒对象链路引擎
    /// 根据单据状态变化匹配相应的提醒规则，并确定提醒目标
    /// </summary>
    public class ReminderObjectLinkEngine
    {
        private readonly ILogger<ReminderObjectLinkEngine> _logger;
        private readonly ApplicationContext _appContext;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly IMessageNotificationService _messageNotificationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReminderObjectLinkEngine(ApplicationContext appContext = null, IUnitOfWorkManage unitOfWorkManage = null)
        {
            _appContext = appContext;
            _unitOfWorkManage = unitOfWorkManage;
            _logger = null;
            _messageNotificationService = null;
        }

        /// <summary>
        /// 构造函数（带消息通知服务）
        /// </summary>
        public ReminderObjectLinkEngine(IMessageNotificationService messageNotificationService, ApplicationContext appContext = null, IUnitOfWorkManage unitOfWorkManage = null)
        {
            _appContext = appContext;
            _unitOfWorkManage = unitOfWorkManage;
            _messageNotificationService = messageNotificationService;
            _logger = null;
        }

        /// <summary>
        /// 根据单据信息和操作类型匹配链路规则
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <param name="actionType">操作类型</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="sourceId">源ID（如创建人ID）</param>
        /// <returns>匹配到的链路规则列表</returns>
        public async Task<List<tb_ReminderObjectLink>> MatchLinkRulesAsync(int bizType, int actionType, int billStatus, long sourceId)
        {
            try
            {
                _logger?.LogDebug($"匹配链路规则: 业务类型={bizType}, 操作类型={actionType}, 单据状态={billStatus}, 源ID={sourceId}");

                if (_unitOfWorkManage == null)
                {
                    _logger?.LogWarning("UnitOfWorkManage未初始化，无法查询规则");
                    return new List<tb_ReminderObjectLink>();
                }

                var db = _unitOfWorkManage.GetDbClient();

                // 查询匹配的链路规则
                var matchedLinks = await db.Queryable<tb_ReminderObjectLink>()
                    .Where(link => link.BizType.Value == (int)bizType
                        && link.IsEnabled == true
                        && (link.ActionType == 0 || link.ActionType == actionType)
                        && (link.BillStatus == 0 || link.BillStatus == billStatus))
                    .Includes(link => link.tb_ReminderLinkRuleRelations)
                    .ToListAsync();

                // 进一步过滤源匹配
                var filteredLinks = matchedLinks.Where(link => IsSourceMatch(link, sourceId)).ToList();

                _logger?.LogDebug($"匹配到 {filteredLinks.Count} 条链路规则");
                return filteredLinks;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "匹配链路规则时发生错误");
                return new List<tb_ReminderObjectLink>();
            }
        }

        /// <summary>
        /// 检查源ID是否匹配链路规则的源配置
        /// </summary>
        /// <param name="linkRule">链路规则</param>
        /// <param name="sourceId">源ID</param>
        /// <returns>是否匹配</returns>
        private bool IsSourceMatch(tb_ReminderObjectLink linkRule, long sourceId)
        {
            // 根据源类型检查是否匹配
            switch ((SourceTargetType)linkRule.SourceType)
            {
                case SourceTargetType.人员:
                    return linkRule.SourceValue == sourceId;
                case SourceTargetType.角色:
                    // 这里需要实现角色匹配逻辑，检查用户是否具有该角色
                    return IsUserInRole(sourceId, linkRule.SourceValue.Value);
                case SourceTargetType.部门:
                    // 这里需要实现部门匹配逻辑，检查用户是否属于该部门
                    return IsUserInDepartment(sourceId, linkRule.SourceValue.Value);
                default:
                    return false;
            }
        }

        /// <summary>
        /// 检查用户是否具有指定角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns>是否具有该角色</returns>
        private bool IsUserInRole(long userId, long roleId)
        {
            try
            {
                if (_unitOfWorkManage == null) return false;

                var db = _unitOfWorkManage.GetDbClient();
                var hasRole = db.Queryable<tb_User_Role>()
                    .Where(ru => ru.User_ID == userId && ru.RoleID == roleId)
                    .Any();

                return hasRole;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查用户角色时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 检查用户是否属于指定部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns>是否属于该部门</returns>
        private bool IsUserInDepartment(long userId, long departmentId)
        {
            try
            {
                if (_unitOfWorkManage == null) return false;

                var db = _unitOfWorkManage.GetDbClient();
                var inDept = db.Queryable<tb_Employee>()
                    .Where(e => e.Employee_ID == userId && e.DepartmentID == departmentId)
                    .Any();

                return inDept;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查用户部门时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 根据链路规则获取通知目标用户
        /// </summary>
        /// <param name="linkRules">匹配到的链路规则列表</param>
        /// <returns>通知目标用户ID列表</returns>
        public List<long> GetNotificationTargets(List<tb_ReminderObjectLink> linkRules)
        {
            try
            {
                var targetIds = new List<long>();

                foreach (var linkRule in linkRules)
                {
                    switch ((SourceTargetType)linkRule.TargetType)
                    {
                        case SourceTargetType.人员:
                            // 直接添加人员ID
                            if (linkRule.TargetValue.HasValue)
                            {
                                targetIds.Add(linkRule.TargetValue.Value);
                            }
                            break;
                        case SourceTargetType.角色:
                            // 添加该角色下的所有用户ID
                            if (linkRule.TargetValue.HasValue)
                            {
                                var roleUsers = GetUsersByRole(linkRule.TargetValue.Value);
                                targetIds.AddRange(roleUsers);
                            }
                            break;
                        case SourceTargetType.部门:
                            // 添加该部门下的所有用户ID
                            if (linkRule.TargetValue.HasValue)
                            {
                                var deptUsers = GetUsersByDepartment(linkRule.TargetValue.Value);
                                targetIds.AddRange(deptUsers);
                            }
                            break;
                    }
                }

                // 去重并返回
                return targetIds.Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取通知目标时发生错误");
                return new List<long>();
            }
        }

        /// <summary>
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>用户ID列表</returns>
        private List<long> GetUsersByRole(long roleId)
        {
            try
            {
                if (_unitOfWorkManage == null) return new List<long>();

                var db = _unitOfWorkManage.GetDbClient();
                var userIds = db.Queryable<tb_User_Role>()
                    .Where(ru => ru.RoleID == roleId)
                    .Select(ru => ru.User_ID)
                    .ToList();

                return userIds;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "根据角色获取用户列表时发生错误");
                return new List<long>();
            }
        }

        /// <summary>
        /// 根据部门获取用户列表
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>用户ID列表</returns>
        private List<long> GetUsersByDepartment(long departmentId)
        {
            try
            {
                if (_unitOfWorkManage == null) return new List<long>();

                var db = _unitOfWorkManage.GetDbClient();
                var userIds = db.Queryable<tb_Employee>()
                    .Where(e => e.DepartmentID == departmentId)
                    .Select(e => e.Employee_ID)
                    .ToList();

                return userIds;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "根据部门获取用户列表时发生错误");
                return new List<long>();
            }
        }

        /// <summary>
        /// 根据链路规则发送提醒
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <param name="linkRules">匹配到的链路规则</param>
        public async Task SendReminderByLinkRulesAsync(MessageData messageData, List<tb_ReminderObjectLink> linkRules)
        {
            try
            {
                // 获取通知目标
                var targetIds = GetNotificationTargets(linkRules);
                if (targetIds.Count == 0)
                {
                    _logger?.LogDebug("没有匹配到通知目标，跳过提醒发送");
                    return;
                }

                _logger?.LogDebug($"匹配到 {targetIds.Count} 个通知目标，准备发送提醒");

                // 通过消息通知服务发送提醒
                if (_messageNotificationService != null)
                {
                    foreach (var targetId in targetIds)
                    {
                        // 为每个目标用户发送消息
                        await _messageNotificationService.SendMessageToUserAsync(
                            targetId,
                            messageData.Title,
                            messageData.Content,
                            messageData.MessageType);
                    }
                }
                else if (_appContext != null)
                {
                    // 降级方案：通过AppContext获取服务（向后兼容）
                    _logger?.LogDebug("消息通知服务未注入，使用降级方案");
                    // 注意：这里不直接使用MessageService，因为业务层不能依赖UI层
                    // 实际使用时应该注入IMessageNotificationService的实现
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送提醒时发生错误");
            }
        }

        /// <summary>
        /// 处理单据状态变化，匹配规则并发送提醒
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <param name="actionType">操作类型</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="sourceId">源ID（如创建人ID）</param>
        /// <param name="messageData">消息数据</param>
        public async Task ProcessBillStatusChangeAsync(int bizType, int actionType, int billStatus, long sourceId, MessageData messageData)
        {
            try
            {
                _logger.LogDebug($"开始处理单据状态变化提醒: 业务类型={bizType}, 操作类型={actionType}, 单据状态={billStatus}, 源ID={sourceId}");

                // 匹配链路规则
                var matchedRules = await MatchLinkRulesAsync(bizType, actionType, billStatus, sourceId);
                if (matchedRules.Count == 0)
                {
                    _logger.LogDebug("没有匹配到链路规则，跳过提醒发送");
                    return;
                }

                // 发送提醒
                await SendReminderByLinkRulesAsync(messageData, matchedRules);
                _logger.LogDebug("单据状态变化提醒处理完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理单据状态变化提醒时发生错误");
            }
        }

        /// <summary>
        /// 处理安全库存提醒
        /// 检查所有库存记录，当库存低于预警值时发送提醒
        /// </summary>
        public async Task ProcessSafetyStockRemindersAsync()
        {
            try
            {
                _logger.LogDebug("开始处理安全库存提醒");

                // 暂时返回，实际实现时需要通过依赖注入获取服务
                _logger.LogDebug("安全库存提醒处理完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理安全库存提醒时发生错误");
            }
        }

        /// <summary>
        /// 发送安全库存提醒
        /// </summary>
        /// <param name="inventory">库存记录</param>
        private async Task SendSafetyStockReminderAsync(tb_Inventory inventory)
        {
            try
            {
                if (inventory == null)
                {
                    _logger.LogWarning("无法发送安全库存提醒：库存记录为空");
                    return;
                }

                // 暂时简化实现，移除错误的属性使用
                _logger.LogDebug("准备发送安全库存提醒 - 库存ID: {InventoryId}", inventory.Inventory_ID);

                // 实际实现时需要创建正确的MessageData对象并发送
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送安全库存提醒时发生错误 - 库存ID: {InventoryId}", inventory?.Inventory_ID);
            }
        }

        /// <summary>
        /// 获取库存管理人员ID列表
        /// 这里可以根据系统配置或角色权限获取
        /// </summary>
        /// <returns>库存管理人员ID列表</returns>
        private List<long> GetInventoryManagerIds()
        {
            // 暂时返回一个空列表，实际实现中应根据系统配置获取
            // 例如：从角色表中获取具有库存管理权限的用户
            return new List<long>();
        }

        /// <summary>
        /// 处理CRM跟进计划提醒
        /// 检查所有即将到期的跟进计划，发送提醒给相关人员
        /// </summary>
        public async Task ProcessCRMFollowUpRemindersAsync()
        {
            try
            {
                _logger.LogDebug("开始处理CRM跟进计划提醒");

                // 暂时返回，实际实现时需要通过依赖注入获取服务
                _logger.LogDebug("CRM跟进计划提醒处理完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理CRM跟进计划提醒时发生错误");
            }
        }

        /// <summary>
        /// 发送CRM跟进计划提醒
        /// </summary>
        /// <param name="plan">跟进计划</param>
        private async Task SendCRMFollowUpReminderAsync(tb_CRM_FollowUpPlans plan)
        {
            try
            {
                if (plan == null)
                {
                    _logger.LogWarning("无法发送CRM跟进计划提醒：计划信息为空");
                    return;
                }

                // 暂时简化实现，移除错误的属性使用
                _logger.LogDebug($"准备发送CRM跟进计划提醒：计划ID {plan.PlanID} 给执行人 {plan.Employee_ID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送CRM跟进计划提醒时发生错误 - 计划ID: {PlanId}", plan.PlanID);
            }
        }
    }
}