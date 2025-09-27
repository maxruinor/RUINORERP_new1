using RUINORERP.PacketSpec.Commands.Lock;
using RUINORERP.PacketSpec.Models;
using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 锁功能使用示例
    /// </summary>
    public class LockUsageExample
    {
        /// <summary>
        /// 锁定销售订单示例
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="orderNo">订单编号</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否锁定成功</returns>
        public async Task<bool> LockSalesOrderAsync(long orderId, string orderNo, long userId)
        {
            try
            {
                // 创建锁定信息
                var billData = new CommBillData
                {
                    BillNo = orderNo,
                    BizName = "销售订单",
                    BizType = 1 // 销售订单业务类型
                };

                // 创建锁定申请命令
                var lockCommand = new DocumentLockApplyCommand(orderId, billData, 1001);

                // 验证命令
                var validationResult = lockCommand.Validate();
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"锁定命令验证失败: {validationResult.ErrorMessage}");
                    return false;
                }

                // 发送命令到服务器
                var commandFactory = new RUINORERP.PacketSpec.Commands.DefaultCommandFactory();
                var response = await commandFactory.ExecuteCommandAsync(lockCommand);

                if (response.IsSuccess)
                {
                    Console.WriteLine($"成功锁定销售订单: {orderNo}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"锁定销售订单失败: {response.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"锁定销售订单异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 解锁销售订单示例
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否解锁成功</returns>
        public async Task<bool> UnlockSalesOrderAsync(long orderId, long userId)
        {
            try
            {
                // 创建解锁命令
                var unlockCommand = new DocumentUnlockCommand(orderId, userId);

                // 验证命令
                var validationResult = unlockCommand.Validate();
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"解锁命令验证失败: {validationResult.ErrorMessage}");
                    return false;
                }

                // 发送命令到服务器
                var commandFactory = new RUINORERP.PacketSpec.Commands.DefaultCommandFactory();
                var response = await commandFactory.ExecuteCommandAsync(unlockCommand);

                if (response.IsSuccess)
                {
                    Console.WriteLine($"成功解锁销售订单: {orderId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"解锁销售订单失败: {response.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解锁销售订单异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 强制解锁销售订单示例（管理员操作）
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>是否强制解锁成功</returns>
        public async Task<bool> ForceUnlockSalesOrderAsync(long orderId)
        {
            try
            {
                // 创建强制解锁命令
                var forceUnlockCommand = new ForceUnlockCommand(orderId);

                // 验证命令
                var validationResult = forceUnlockCommand.Validate();
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"强制解锁命令验证失败: {validationResult.ErrorMessage}");
                    return false;
                }

                // 发送命令到服务器
                var commandFactory = new RUINORERP.PacketSpec.Commands.DefaultCommandFactory();
                var response = await commandFactory.ExecuteCommandAsync(forceUnlockCommand);

                if (response.IsSuccess)
                {
                    Console.WriteLine($"成功强制解锁销售订单: {orderId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"强制解锁销售订单失败: {response.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"强制解锁销售订单异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 查询销售订单锁定状态示例
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>锁定信息</returns>
        public async Task<LockedInfo> QuerySalesOrderLockStatusAsync(long orderId)
        {
            try
            {
                // 创建查询锁状态命令
                var queryCommand = new QueryLockStatusCommand(orderId);

                // 验证命令
                var validationResult = queryCommand.Validate();
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"查询锁状态命令验证失败: {validationResult.ErrorMessage}");
                    return null;
                }

                // 发送命令到服务器
                var commandFactory = new RUINORERP.PacketSpec.Commands.DefaultCommandFactory();
                var response = await commandFactory.ExecuteCommandAsync(queryCommand);

                if (response.IsSuccess)
                {
                    var lockInfo = response.GetMetadata<LockedInfo>("LockInfo");
                    Console.WriteLine($"查询销售订单锁定状态成功: {lockInfo != null}");
                    return lockInfo;
                }
                else
                {
                    Console.WriteLine($"查询销售订单锁定状态失败: {response.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"查询销售订单锁定状态异常: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 请求解锁被锁定的销售订单示例
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="lockedUserId">锁定用户ID</param>
        /// <param name="requestUserId">请求用户ID</param>
        /// <param name="requestUserName">请求用户名</param>
        /// <returns>是否请求成功</returns>
        public async Task<bool> RequestUnlockSalesOrderAsync(long orderId, long lockedUserId, long requestUserId, string requestUserName)
        {
            try
            {
                // 创建请求解锁信息
                var requestInfo = new RequestUnLockInfo
                {
                    BillID = orderId,
                    LockedUserID = lockedUserId,
                    RequestUserID = requestUserId,
                    RequestUserName = requestUserName,
                    BillData = new CommBillData
                    {
                        BillNo = $"SO{orderId}",
                        BizName = "销售订单"
                    }
                };

                // 创建请求解锁命令
                var requestUnlockCommand = new RequestUnlockCommand(requestInfo);

                // 验证命令
                var validationResult = requestUnlockCommand.Validate();
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"请求解锁命令验证失败: {validationResult.ErrorMessage}");
                    return false;
                }

                // 发送命令到服务器
                var commandFactory = new RUINORERP.PacketSpec.Commands.DefaultCommandFactory();
                var response = await commandFactory.ExecuteCommandAsync(requestUnlockCommand);

                if (response.IsSuccess)
                {
                    Console.WriteLine($"成功请求解锁销售订单: {orderId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"请求解锁销售订单失败: {response.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求解锁销售订单异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 拒绝解锁请求示例
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="requestUserId">请求用户ID</param>
        /// <param name="refuseUserId">拒绝用户ID</param>
        /// <param name="refuseUserName">拒绝用户名</param>
        /// <returns>是否拒绝成功</returns>
        public async Task<bool> RefuseUnlockRequestAsync(long orderId, long requestUserId, long refuseUserId, string refuseUserName)
        {
            try
            {
                // 创建拒绝解锁信息
                var refuseInfo = new RefuseUnLockInfo
                {
                    BillID = orderId,
                    RequestUserID = requestUserId,
                    RefuseUserID = refuseUserId,
                    RefuseUserName = refuseUserName,
                    BillData = new CommBillData
                    {
                        BillNo = $"SO{orderId}",
                        BizName = "销售订单"
                    }
                };

                // 创建拒绝解锁命令
                var refuseUnlockCommand = new RefuseUnlockCommand(refuseInfo);

                // 验证命令
                var validationResult = refuseUnlockCommand.Validate();
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"拒绝解锁命令验证失败: {validationResult.ErrorMessage}");
                    return false;
                }

                // 发送命令到服务器
                var commandFactory = new RUINORERP.PacketSpec.Commands.DefaultCommandFactory();
                var response = await commandFactory.ExecuteCommandAsync(refuseUnlockCommand);

                if (response.IsSuccess)
                {
                    Console.WriteLine($"成功拒绝解锁请求: {orderId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"拒绝解锁请求失败: {response.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"拒绝解锁请求异常: {ex.Message}");
                return false;
            }
        }
    }
}