// 测试模式：如果定义了SKIP_REGISTRATION_VALIDATION，则跳过注册码验证
#define SKIP_REGISTRATION_VALIDATION

using HLH.Lib.Helper;
using HLH.Lib.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server.Services
{

    /// <summary>
    /// 注册服务实现
    /// 提供系统注册相关的业务逻辑实现
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<RegistrationService> _logger;
        private readonly IHardwareInfoService _hardwareInfoService;
        private readonly ISecurityService _securityService;
        private readonly IServiceProvider _serviceProvider;
        public readonly IUnitOfWorkManage _unitOfWorkManage;

        /// <summary>
        /// 注册信息内存缓存
        /// </summary>
        private tb_sys_RegistrationInfo _registrationInfoCache;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        private DateTime _lastUpdateTime;

        /// <summary>
        /// 缓存锁对象
        /// </summary>
        private readonly object _cacheLock = new object();

        public RegistrationService(
            ILogger<RegistrationService> logger,
            IHardwareInfoService hardwareInfoService,
            ISecurityService securityService,
            IServiceProvider serviceProvider, IUnitOfWorkManage unitOfWorkManage)
        {
            _logger = logger;
            _hardwareInfoService = hardwareInfoService;
            _securityService = securityService;
            _serviceProvider = serviceProvider;
            _unitOfWorkManage = unitOfWorkManage;
            
            // 启动时加载注册信息到内存
            InitializeRegistrationInfoCache();
        }

        /// <summary>
        /// 获取注册信息
        /// </summary>
        /// <returns>注册信息实体</returns>
        public async Task<tb_sys_RegistrationInfo> GetRegistrationInfoAsync()
        {
            try
            {
                // 使用CopyNew()创建独立的数据库连接上下文，避免IsAutoCloseConnection=true导致的连接提前关闭问题
                var db = _unitOfWorkManage.GetDbClient().CopyNew();
                var registrationInfo = await db.Queryable<tb_sys_RegistrationInfo>().FirstAsync();

                if (registrationInfo == null)
                {
                    // 如果没有注册信息，创建默认信息
                    registrationInfo = new tb_sys_RegistrationInfo
                    {
                        CompanyName = "",
                        ContactName = "",
                        PhoneNumber = "",
                        LicenseType = "试用版",
                        ExpirationDate = DateTime.Now.AddDays(30),
                        ConcurrentUsers = 1,
                        ProductVersion = Application.ProductVersion,
                        MachineCode = "",
                        RegistrationCode = "",
                        PurchaseDate = DateTime.Now,
                        RegistrationDate = DateTime.MinValue,
                        IsRegistered = false,
                        FunctionModule = "",
                        Remarks = ""
                    };
                }

                return registrationInfo;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取注册信息失败");
                throw;
            }
        }

        /// <summary>
        /// 保存注册信息
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SaveRegistrationInfoAsync(tb_sys_RegistrationInfo registrationInfo)
        {
            try
            {
                // 使用CopyNew()创建独立的数据库连接上下文，避免IsAutoCloseConnection=true导致的连接提前关闭问题
                var db = _unitOfWorkManage.GetDbClient().CopyNew();
                var existingInfo = await db.Queryable<tb_sys_RegistrationInfo>().FirstAsync();
                if (existingInfo != null)
                {
                    // 更新现有记录
                    registrationInfo.RegistrationInfoD = existingInfo.RegistrationInfoD;
                    registrationInfo.Modified_at = DateTime.Now;
                    return await db.Updateable(registrationInfo).ExecuteCommandAsync() > 0;
                }
                else
                {
                    // 插入新记录
                    registrationInfo.Created_at = DateTime.Now;
                    var regInfo = await db.Storageable(registrationInfo).ExecuteReturnEntityAsync();
                    return regInfo.RegistrationInfoD > 0;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存注册信息失败");
                return false;
            }
        }

        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>机器码</returns>
        public string CreateMachineCode(tb_sys_RegistrationInfo registrationInfo)
        {
            try
            {
                // 指定关键字段，这些字段生成加密的机器码
                List<string> cols = new List<string>();
                cols.Add("CompanyName");
                cols.Add("ContactName");
                cols.Add("PhoneNumber");
                cols.Add("ConcurrentUsers");
                cols.Add("ExpirationDate");
                cols.Add("ProductVersion");
                cols.Add("LicenseType");
                cols.Add("FunctionModule");
                // 确保机器码包含人数限制和时间等关键信息
                cols.Add("PurchaseDate");

                // 只序列化指定的列
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new SelectiveContractResolver(cols),
                    Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" } }
                };

                // 序列化对象
                string jsonString = JsonConvert.SerializeObject(registrationInfo, settings);

                // 获取硬件信息
                string hardwareInfo = _hardwareInfoService.GetUniqueHardwareInfo();
                string originalInfo = hardwareInfo + jsonString;

                // 使用固定密钥加密生成机器码
                string key = "ruinor1234567890";
                return HLH.Lib.Security.EncryptionHelper.AesEncrypt(originalInfo, key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成机器码失败");
                throw;
            }
        }

        /// <summary>
        /// 检查注册信息
        /// </summary>
        /// <param name="regInfo">注册信息</param>
        /// <returns>注册是否有效</returns>
        public bool CheckRegistered(tb_sys_RegistrationInfo regInfo)
        {
            if (regInfo == null)
            {
                return false;
            }

            string key = "ruinor1234567890"; // 这应该是一个密钥
            string machineCode = regInfo.MachineCode; // 这可能是计算机的硬件信息或唯一标识符
            // 假设用户输入的注册码
            string userProvidedCode = regInfo.RegistrationCode;
            bool isValid = HLH.Lib.Security.SecurityService.ValidateRegistrationCode(userProvidedCode, key, machineCode);
            #if DEBUG
            System.Diagnostics.Debug.WriteLine($"提供的注册码是否有效? {isValid}");
            #endif
            return isValid;
        }


        /// <summary>
        /// 验证注册码11
        /// </summary>
        /// <param name="registrationCode">注册码</param>
        /// <returns>验证是否通过</returns>
        public async Task<bool> ValidateRegistrationAsync(string registrationCode)
        {
            try
            {
                // 测试模式：如果定义了SKIP_REGISTRATION_VALIDATION常量，则跳过注册码验证
                #if SKIP_REGISTRATION_VALIDATION
                _logger.LogWarning("测试模式：跳过注册码验证");
                return true;
                #endif

                var regInfo = await GetRegistrationInfoAsync();
                // 获取当前注册信息
                if (regInfo == null)
                {
                    regInfo = new tb_sys_RegistrationInfo();
                }

                // 解密注册码
                string key = "ruinor1234567890";
                var decryptedData = HLH.Lib.Security.EncryptionHelper.AesDecrypt(registrationCode, key);
                if (string.IsNullOrEmpty(decryptedData))
                {
                    _logger.LogWarning("注册码解密失败");
                    return false;
                }

                // 解析注册信息
                var parts = decryptedData.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 10)
                {
                    _logger.LogWarning("注册码格式不正确");
                    return false;
                }

                // 验证机器码是否匹配
                if (parts[0] != regInfo.MachineCode)
                {
                    _logger.LogWarning("机器码不匹配");
                    return false;
                }

                // 验证产品版本是否匹配
                if (parts[1] != Application.ProductVersion)
                {
                    _logger.LogWarning("产品版本不匹配");
                    return false;
                }

                // 设置注册信息
                regInfo.IsRegistered = true;
                regInfo.RegistrationCode = registrationCode;
                regInfo.CompanyName = parts[2];
                regInfo.ContactName = parts[3];
                regInfo.PhoneNumber = parts[4];
                regInfo.LicenseType = parts[5];
                regInfo.ConcurrentUsers = int.Parse(parts[6]);
                regInfo.PurchaseDate = DateTime.Parse(parts[7]);
                regInfo.RegistrationDate = regInfo.RegistrationDate == DateTime.MinValue ? DateTime.Now : regInfo.RegistrationDate;
                regInfo.ExpirationDate = DateTime.Parse(parts[8]);
                regInfo.FunctionModule = string.Join(",", parts[9].Split(','));
                // 保存注册信息
                await SaveRegistrationInfoAsync(regInfo);
                _logger.LogInformation("注册信息验证并保存成功");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "注册码验证失败");
                return false;
            }
        }


        /// <summary>
        /// 检查注册是否过期
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>是否过期</returns>
        public bool IsRegistrationExpired(tb_sys_RegistrationInfo registrationInfo)
        {
            if (registrationInfo == null)
            {
                return true;
            }

            // 检查授权到期日期
            if (registrationInfo.ExpirationDate < DateTime.Now)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 初始化注册信息缓存（启动时调用）
        /// </summary>
        private async void InitializeRegistrationInfoCache()
        {
            try
            {
                _logger.LogInformation("开始加载注册信息到内存缓存");
                await UpdateRegistrationInfoCacheAsync();
                _logger.LogInformation("注册信息内存缓存加载完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化注册信息缓存失败");
            }
        }

        /// <summary>
        /// 获取注册状态（从内存获取）
        /// </summary>
        /// <returns>注册状态</returns>
        public async Task<RegistrationStatus> GetRegistrationStatusAsync()
        {
            await Task.CompletedTask;

            lock (_cacheLock)
            {
                if (_registrationInfoCache == null)
                {
                    return RegistrationStatus.Expired;
                }

                if (!_registrationInfoCache.IsRegistered)
                {
                    return RegistrationStatus.Expired;
                }

                if (IsRegistrationExpired(_registrationInfoCache))
                {
                    return RegistrationStatus.Expired;
                }

                // 检查是否即将到期（30天内）
                var daysUntilExpiration = (_registrationInfoCache.ExpirationDate - DateTime.Now).Days;
                if (daysUntilExpiration <= 30 && daysUntilExpiration > 0)
                {
                    return RegistrationStatus.ExpiringSoon;
                }

                return RegistrationStatus.Normal;
            }
        }

        /// <summary>
        /// 检查是否需要到期提醒（从内存判断）
        /// </summary>
        /// <param name="reminderDays">提醒提前天数</param>
        /// <returns>是否需要提醒</returns>
        public async Task<bool> CheckExpirationReminderAsync(int reminderDays = 30)
        {
            await Task.CompletedTask;

            lock (_cacheLock)
            {
                if (_registrationInfoCache == null || !_registrationInfoCache.IsRegistered)
                {
                    return false;
                }

                var daysUntilExpiration = (_registrationInfoCache.ExpirationDate - DateTime.Now).Days;
                return daysUntilExpiration <= reminderDays && daysUntilExpiration > 0;
            }
        }

        /// <summary>
        /// 获取到期提醒信息（从内存获取）
        /// </summary>
        /// <param name="reminderDays">提醒提前天数</param>
        /// <returns>到期提醒信息</returns>
        public async Task<ExpirationReminder> GetExpirationReminderInfoAsync(int reminderDays = 30)
        {
            await Task.CompletedTask;

            lock (_cacheLock)
            {
                if (_registrationInfoCache == null || !_registrationInfoCache.IsRegistered)
                {
                    return new ExpirationReminder();
                }

                var daysUntilExpiration = (_registrationInfoCache.ExpirationDate - DateTime.Now).Days;

                if (daysUntilExpiration <= reminderDays && daysUntilExpiration > 0)
                {
                    return ExpirationReminder.Create(
                        daysUntilExpiration,
                        _registrationInfoCache.ExpirationDate,
                        "请联系软件提供商",
                        _registrationInfoCache.PhoneNumber
                    );
                }

                return new ExpirationReminder();
            }
        }

        /// <summary>
        /// 更新注册信息缓存（从数据库重新加载到内存）
        /// </summary>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateRegistrationInfoCacheAsync()
        {
            try
            {
                _logger.LogInformation("开始更新注册信息缓存");

                // 从数据库加载注册信息
                var registrationInfo = await GetRegistrationInfoAsync();

                lock (_cacheLock)
                {
                    _registrationInfoCache = registrationInfo;
                    _lastUpdateTime = DateTime.Now;
                }

                _logger.LogInformation($"注册信息缓存更新成功，到期时间：{_registrationInfoCache?.ExpirationDate:yyyy-MM-dd}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新注册信息缓存失败");
                return false;
            }
        }

        /// <summary>
        /// 验证系统注册状态（统一验证方法）
        /// 此方法整合了所有注册验证逻辑，包括：注册状态、过期检查、用户数限制等
        /// </summary>
        /// <returns>注册验证结果</returns>
        public async Task<RegistrationValidationResult> ValidateSystemRegistrationAsync()
        {
            try
            {
                // 测试模式：如果定义了SKIP_REGISTRATION_VALIDATION，则跳过注册验证
                #if SKIP_REGISTRATION_VALIDATION
                _logger.LogWarning("测试模式：跳过注册验证");
                var registrationInfo = await GetRegistrationInfoAsync();
                return RegistrationValidationResult.CreateSuccess(registrationInfo);
                #endif

                // 获取注册信息
                var regInfo = await GetRegistrationInfoAsync();
                if (regInfo == null)
                {
                    _logger.LogError("无法从数据库获取注册信息");
                    return RegistrationValidationResult.CreateValidationFailed("无法从数据库获取注册信息");
                }

                // 检查是否已注册
                if (!regInfo.IsRegistered)
                {
                    _logger.LogWarning("系统未注册");
                    return RegistrationValidationResult.CreateNotRegistered();
                }

                // 检查注册码是否有效
                bool isRegistered = CheckRegistered(regInfo);
                if (!isRegistered)
                {
                    // 检查是否过期
                    if (IsRegistrationExpired(regInfo))
                    {
                        _logger.LogWarning("注册许可已过期");
                        return RegistrationValidationResult.CreateExpired(regInfo);
                    }
                    else
                    {
                        _logger.LogWarning("注册信息验证失败");
                        return RegistrationValidationResult.CreateValidationFailed("注册信息验证失败，请检查注册码是否正确");
                    }
                }

                // 验证用户数限制
                if (regInfo.ConcurrentUsers <= 0)
                {
                    _logger.LogWarning("注册许可的用户数配置无效");
                    return RegistrationValidationResult.CreateInvalidUserCount();
                }

                // 检查注册到期时间
                var daysUntilExpiration = (regInfo.ExpirationDate - DateTime.Now).TotalDays;
                if (daysUntilExpiration <= 0)
                {
                    _logger.LogWarning("注册许可已过期");
                    return RegistrationValidationResult.CreateExpired(regInfo);
                }
                else if (daysUntilExpiration <= 7)
                {
                    _logger.LogInformation($"警告：注册许可将在 {daysUntilExpiration:F0} 天后到期");
                    return RegistrationValidationResult.CreateExpiringSoon(regInfo, (int)daysUntilExpiration);
                }

                _logger.LogInformation($"系统注册验证成功，许可用户数: {regInfo.ConcurrentUsers}");
                _logger.LogInformation($"注册到期时间: {regInfo.ExpirationDate:yyyy-MM-dd HH:mm:ss}");
                return RegistrationValidationResult.CreateSuccess(regInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证系统注册状态时发生错误");
                return RegistrationValidationResult.CreateValidationFailed($"验证系统注册状态时发生错误: {ex.Message}");
            }
        }

    }

    /// <summary>
    /// 选择性序列化解析器
    /// </summary>
    public class SelectiveContractResolver : DefaultContractResolver
    {
        private readonly List<string> _propertyNamesToSerialize;

        public SelectiveContractResolver(List<string> propertyNamesToSerialize)
        {
            _propertyNamesToSerialize = propertyNamesToSerialize;

        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            return properties.Where(p => _propertyNamesToSerialize.Contains(p.PropertyName)).ToList();
        }
    }

}