using System;
using System.Linq;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Network.CommandHandlers;

namespace RUINORERP.Server.Diagnostics
{
    /// <summary>
    /// 登录问题诊断工具
    /// 用于快速排查客户端无法登录的原因
    /// </summary>
    public class LoginDiagnostics
    {
        /// <summary>
        /// 执行完整的登录问题诊断
        /// </summary>
        public static void RunDiagnostics()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║           RUINORERP 登录问题诊断工具                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // 1. 检查黑名单
            CheckBlacklist();
            
            // 2. 检查登录尝试记录
            CheckLoginAttempts();
            
            // 3. 检查系统状态
            CheckSystemStatus();
            
            Console.WriteLine();
            Console.WriteLine("诊断完成！请查看上述结果。");
        }

        /// <summary>
        /// 检查黑名单状态
        /// </summary>
        private static void CheckBlacklist()
        {
            Console.WriteLine("【1】黑名单检查");
            Console.WriteLine("─────────────────────────────────────────");
            
            var bannedList = BlacklistManager.BannedList;
            Console.WriteLine($"黑名单IP数量: {bannedList.Count}");
            
            if (bannedList.Count > 0)
            {
                Console.WriteLine("\n被封禁的IP列表:");
                foreach (var entry in bannedList)
                {
                    var remaining = entry.解封时间 - DateTime.Now;
                    Console.WriteLine($"  • IP: {entry.IP地址}");
                    Console.WriteLine($"    原因: {entry.Reason}");
                    Console.WriteLine($"    封禁时间: {entry.BanTime}");
                    Console.WriteLine($"    解封时间: {entry.解封时间}");
                    Console.WriteLine($"    剩余时间: {remaining.TotalMinutes:F1} 分钟");
                    Console.WriteLine();
                }
                
                Console.WriteLine("⚠️  警告: 发现黑名单记录，可能是导致登录失败的原因！");
                Console.WriteLine("   建议: 使用 BlacklistManager.ClearBlacklist() 清除所有黑名单");
            }
            else
            {
                Console.WriteLine("✓ 黑名单为空，正常");
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// 检查登录尝试记录
        /// </summary>
        private static void CheckLoginAttempts()
        {
            Console.WriteLine("【2】登录尝试记录检查");
            Console.WriteLine("─────────────────────────────────────────");
            
            var lockedUsers = LoginCommandHandler.GetLockedUsernames();
            Console.WriteLine($"被锁定用户数量: {lockedUsers.Count}");
            
            if (lockedUsers.Count > 0)
            {
                Console.WriteLine("\n被锁定的用户列表:");
                foreach (var username in lockedUsers)
                {
                    var attempts = LoginCommandHandler.GetLoginAttempts(username);
                    Console.WriteLine($"  • 用户名: {username}");
                    Console.WriteLine($"    失败次数: {attempts}/5");
                    Console.WriteLine();
                }
                
                Console.WriteLine("⚠️  警告: 发现有用户被锁定，可能是导致登录失败的原因！");
                Console.WriteLine("   建议: 使用 LoginCommandHandler.UnlockUserLogin(\"用户名\") 解除锁定");
            }
            else
            {
                Console.WriteLine("✓ 没有用户被锁定，正常");
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// 检查系统状态
        /// </summary>
        private static void CheckSystemStatus()
        {
            Console.WriteLine("【3】系统状态检查");
            Console.WriteLine("─────────────────────────────────────────");
            
            // 检查BlacklistManager是否初始化
            try
            {
                var list = BlacklistManager.BannedList;
                Console.WriteLine("✓ BlacklistManager 已初始化");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ BlacklistManager 未初始化: {ex.Message}");
                Console.WriteLine("   建议: 确保在frmMainNew.cs中调用了 BlacklistManager.Initialize()");
            }
            
            // 检查配置
            Console.WriteLine($"\n当前配置:");
            Console.WriteLine($"  • 最大登录尝试次数: 5");
            Console.WriteLine($"  • 登录尝试过期时间: 10分钟");
            Console.WriteLine($"  • DDoS防护: 每分钟最多60次连接");
            Console.WriteLine($"  • DDoS防护: 5秒内最多5次连接");
            Console.WriteLine($"  • 最大会话数: 1000");
            
            Console.WriteLine();
        }

        /// <summary>
        /// 清除所有可能导致登录失败的限制
        /// </summary>
        public static void ClearAllRestrictions()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║           清除所有限制 - 紧急恢复模式                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            
            // 1. 清除黑名单
            Console.WriteLine("【1】清除黑名单...");
            BlacklistManager.ClearBlacklist();
            Console.WriteLine("✓ 黑名单已清除");
            
            // 2. 清除所有登录尝试记录
            Console.WriteLine("\n【2】清除登录尝试记录...");
            LoginCommandHandler.ClearAllLoginAttempts();
            Console.WriteLine("✓ 登录尝试记录已清除");
            
            Console.WriteLine("\n✅ 所有限制已清除！客户端现在应该可以登录了。");
            Console.WriteLine("   提示: 这只是临时解决方案，请查找根本原因。");
        }
    }
}
