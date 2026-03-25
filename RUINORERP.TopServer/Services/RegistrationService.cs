using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Logging;
using RUINORERP.TopServer.Model;

namespace RUINORERP.TopServer.Services
{
    /// <summary>
    /// 注册服务
    /// 负责公司注册、机器码生成、注册码生成和验证
    /// </summary>
    public class RegistrationService
    {
        private readonly ConcurrentDictionary<Guid, CompanyRegistration> _registrations;
        private readonly ConcurrentDictionary<string, Guid> _machineCodeToCompanyMap;
        private readonly ILogger<RegistrationService>? _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RegistrationService(ILogger<RegistrationService>? logger = null)
        {
            _registrations = new ConcurrentDictionary<Guid, CompanyRegistration>();
            _machineCodeToCompanyMap = new ConcurrentDictionary<string, Guid>();
            _logger = logger;
        }

        #region 注册码生成和验证

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <param name="machineCode">机器码</param>
        /// <param name="expiryDate">到期日期</param>
        /// <param name="licensedUsers">许可用户数</param>
        /// <returns>注册码</returns>
        public string GenerateRegistrationCode(string machineCode, DateTime expiryDate, int licensedUsers = 10)
        {
            // 机器码 + 到期日期 + 许可用户数 + 密钥
            var secretKey = "RUINORERP-2026-TOPSERVER-SECRET";
            var data = $"{machineCode}|{expiryDate:yyyyMMdd}|{licensedUsers}|{secretKey}";

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(data);
                var hash = sha256.ComputeHash(bytes);
                
                // 格式化为注册码：XXXX-XXXX-XXXX-XXXX-XXXX-XXXX-XXXX-XXXX
                var hashStr = BitConverter.ToString(hash).Replace("-", "");
                var segments = new string[8];
                for (int i = 0; i < 8; i++)
                {
                    segments[i] = hashStr.Substring(i * 4, 4);
                }
                return string.Join("-", segments);
            }
        }

        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="machineCode">机器码</param>
        /// <param name="registrationCode">注册码</param>
        /// <param name="expiryDate">预期的到期日期</param>
        /// <param name="licensedUsers">预期的许可用户数</param>
        /// <returns>是否有效</returns>
        public bool VerifyRegistrationCode(string machineCode, string registrationCode, DateTime expiryDate, int licensedUsers)
        {
            var expectedCode = GenerateRegistrationCode(machineCode, expiryDate, licensedUsers);
            return expectedCode.Equals(registrationCode, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region 公司注册管理

        /// <summary>
        /// 注册新公司
        /// </summary>
        /// <param name="registration">注册信息</param>
        /// <returns>注册结果</returns>
        public RegistrationServiceResult RegisterCompany(CompanyRegistration registration)
        {
            try
            {
                // 检查机器码是否已注册
                if (_machineCodeToCompanyMap.ContainsKey(registration.MachineCode))
                {
                    return RegistrationServiceResult.Failed("该机器码已注册，请先更新现有注册信息");
                }

                // 生成注册码
                registration.RegistrationCode = GenerateRegistrationCode(
                    registration.MachineCode,
                    registration.ExpiryDate,
                    registration.LicensedUsers
                );

                registration.CompanyId = Guid.NewGuid();
                registration.RegistrationDate = DateTime.Now;
                registration.CreatedAt = DateTime.Now;
                registration.UpdatedAt = DateTime.Now;
                registration.Status = RegistrationStatus.Normal;

                // 保存注册信息
                _registrations.TryAdd(registration.CompanyId, registration);
                _machineCodeToCompanyMap.TryAdd(registration.MachineCode, registration.CompanyId);

                _logger?.LogInformation("公司注册成功 - CompanyId: {CompanyId}, CompanyName: {CompanyName}, MachineCode: {MachineCode}",
                    registration.CompanyId, registration.CompanyName, registration.MachineCode);

                return RegistrationServiceResult.Succeed(
                    $"公司注册成功，注册码：{registration.RegistrationCode}",
                    registration.RegistrationCode
                );
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "公司注册失败 - CompanyName: {CompanyName}", registration.CompanyName);
                return RegistrationServiceResult.Failed($"注册失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 更新公司注册信息（用于硬件更换）
        /// </summary>
        /// <param name="oldMachineCode">旧机器码</param>
        /// <param name="newMachineCode">新机器码</param>
        /// <param name="reason">更换原因</param>
        /// <returns>更新结果</returns>
        public RegistrationServiceResult UpdateMachineCode(string oldMachineCode, string newMachineCode, string reason)
        {
            try
            {
                // 查找旧机器码对应的注册信息
                if (!_machineCodeToCompanyMap.TryGetValue(oldMachineCode, out var companyId))
                {
                    return RegistrationServiceResult.Failed("未找到该机器码的注册信息");
                }

                if (!_registrations.TryGetValue(companyId, out var registration))
                {
                    return RegistrationServiceResult.Failed("注册信息不存在");
                }

                // 检查新机器码是否已被使用
                if (_machineCodeToCompanyMap.ContainsKey(newMachineCode))
                {
                    return RegistrationServiceResult.Failed("新机器码已被其他公司使用");
                }

                // 更新机器码和注册码
                var oldMachineCodeSaved = registration.MachineCode;
                registration.MachineCode = newMachineCode;
                registration.RegistrationCode = GenerateRegistrationCode(
                    newMachineCode,
                    registration.ExpiryDate,
                    registration.LicensedUsers
                );
                registration.UpdatedAt = DateTime.Now;
                registration.Remarks = $"{registration.Remarks}\n[更新记录] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - 机器码更新: {oldMachineCode} -> {newMachineCode}, 原因: {reason}";

                // 更新映射
                _machineCodeToCompanyMap.TryRemove(oldMachineCode, out _);
                _machineCodeToCompanyMap.TryAdd(newMachineCode, companyId);

                _logger?.LogInformation("机器码更新成功 - CompanyId: {CompanyId}, OldMachineCode: {OldMachineCode}, NewMachineCode: {NewMachineCode}",
                    companyId, oldMachineCode, newMachineCode);

                return RegistrationServiceResult.Succeed(
                    $"机器码更新成功，新注册码：{registration.RegistrationCode}",
                    registration.RegistrationCode
                );
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "机器码更新失败 - OldMachineCode: {OldMachineCode}", oldMachineCode);
                return RegistrationServiceResult.Failed($"更新失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 续费（延长到期日期）
        /// </summary>
        /// <param name="machineCode">机器码</param>
        /// <param name="newExpiryDate">新的到期日期</param>
        /// <param name="licensedUsers">新的许可用户数（可选）</param>
        /// <returns>续费结果</returns>
        public RegistrationServiceResult RenewRegistration(string machineCode, DateTime newExpiryDate, int? licensedUsers = null)
        {
            try
            {
                if (!_machineCodeToCompanyMap.TryGetValue(machineCode, out var companyId))
                {
                    return RegistrationServiceResult.Failed("未找到该机器码的注册信息");
                }

                if (!_registrations.TryGetValue(companyId, out var registration))
                {
                    return RegistrationServiceResult.Failed("注册信息不存在");
                }

                // 更新到期日期和注册码
                registration.ExpiryDate = newExpiryDate;
                if (licensedUsers.HasValue)
                {
                    registration.LicensedUsers = licensedUsers.Value;
                }
                registration.RegistrationCode = GenerateRegistrationCode(
                    machineCode,
                    newExpiryDate,
                    registration.LicensedUsers
                );
                registration.UpdatedAt = DateTime.Now;
                registration.Status = RegistrationStatus.Normal;

                _logger?.LogInformation("续费成功 - CompanyId: {CompanyId}, NewExpiryDate: {NewExpiryDate}",
                    companyId, newExpiryDate);

                return RegistrationServiceResult.Succeed(
                    $"续费成功，新到期日期：{newExpiryDate:yyyy-MM-dd}，新注册码：{registration.RegistrationCode}",
                    registration.RegistrationCode
                );
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "续费失败 - MachineCode: {MachineCode}", machineCode);
                return RegistrationServiceResult.Failed($"续费失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 根据机器码查询注册信息
        /// </summary>
        /// <param name="machineCode">机器码</param>
        /// <returns>注册信息</returns>
        public RegistrationServiceResult GetRegistrationByMachineCode(string machineCode)
        {
            try
            {
                if (!_machineCodeToCompanyMap.TryGetValue(machineCode, out var companyId))
                {
                    return RegistrationServiceResult.Failed("未找到该机器码的注册信息");
                }

                if (!_registrations.TryGetValue(companyId, out var registration))
                {
                    return RegistrationServiceResult.Failed("注册信息不存在");
                }

                // 检查是否过期
                if (registration.IsExpired && registration.Status == RegistrationStatus.Normal)
                {
                    registration.Status = RegistrationStatus.Expired;
                    registration.UpdatedAt = DateTime.Now;
                }

                return RegistrationServiceResult.QueryResult(registration);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "查询注册信息失败 - MachineCode: {MachineCode}", machineCode);
                return RegistrationServiceResult.Failed($"查询失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="machineCode">机器码</param>
        /// <param name="registrationCode">注册码</param>
        /// <returns>验证结果</returns>
        public RegistrationServiceResult ValidateRegistration(string machineCode, string registrationCode)
        {
            try
            {
                var result = GetRegistrationByMachineCode(machineCode);
                if (!result.Success)
                {
                    return result;
                }

                var registration = result.Registration!;
                
                // 验证注册码
                if (!registration.RegistrationCode.Equals(registrationCode, StringComparison.OrdinalIgnoreCase))
                {
                    return RegistrationServiceResult.Failed("注册码不匹配");
                }

                // 检查状态
                if (registration.Status == RegistrationStatus.Disabled)
                {
                    return RegistrationServiceResult.Failed("该注册已被停用");
                }

                if (registration.IsExpired)
                {
                    return new RegistrationServiceResult
                    {
                        Success = false,
                        Message = $"注册已过期，到期日期：{registration.ExpiryDate:yyyy-MM-dd}",
                        IsExpired = true,
                        RemainingDays = 0
                    };
                }

                return RegistrationServiceResult.Succeed(
                    $"验证成功，到期日期：{registration.ExpiryDate:yyyy-MM-dd}，剩余 {registration.RemainingDays} 天"
                );
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证注册码失败 - MachineCode: {MachineCode}", machineCode);
                return RegistrationServiceResult.Failed($"验证失败：{ex.Message}");
            }
        }

        #endregion

        #region 查询和管理

        /// <summary>
        /// 获取所有注册
        /// </summary>
        public IEnumerable<CompanyRegistration> GetAllRegistrations()
        {
            return _registrations.Values.ToList();
        }

        /// <summary>
        /// 获取即将过期的注册（30天内）
        /// </summary>
        public IEnumerable<CompanyRegistration> GetExpiringSoonRegistrations()
        {
            var threshold = DateTime.Now.AddDays(30);
            return _registrations.Values
                .Where(r => r.ExpiryDate <= threshold && r.Status == RegistrationStatus.Normal)
                .ToList();
        }

        /// <summary>
        /// 获取已过期的注册
        /// </summary>
        public IEnumerable<CompanyRegistration> GetExpiredRegistrations()
        {
            return _registrations.Values
                .Where(r => r.IsExpired)
                .ToList();
        }

        /// <summary>
        /// 删除注册
        /// </summary>
        public bool DeleteRegistration(Guid companyId)
        {
            if (_registrations.TryRemove(companyId, out var registration))
            {
                _machineCodeToCompanyMap.TryRemove(registration.MachineCode, out _);
                _logger?.LogInformation("注册删除成功 - CompanyId: {CompanyId}", companyId);
                return true;
            }
            return false;
        }

        #endregion
    }
}
