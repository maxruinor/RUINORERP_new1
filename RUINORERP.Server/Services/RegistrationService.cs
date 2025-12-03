using HLH.Lib.Helper;
using HLH.Lib.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RUINORERP.Business;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server.Services
{
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

        public RegistrationService(
            ILogger<RegistrationService> logger,
            IHardwareInfoService hardwareInfoService,
            ISecurityService securityService,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _hardwareInfoService = hardwareInfoService;
            _securityService = securityService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取注册信息
        /// </summary>
        /// <returns>注册信息实体</returns>
        public async Task<tb_sys_RegistrationInfo> GetRegistrationInfoAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
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
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();

                    var existingInfo = await db.Queryable<tb_sys_RegistrationInfo>().FirstAsync();
                    if (existingInfo != null)
                    {
                        // 更新现有记录
                        registrationInfo.RegistrationInfoD = existingInfo.RegistrationInfoD;
                        // registrationInfo.Modified_by = "System";
                        registrationInfo.Modified_at = DateTime.Now;

                        return await db.Updateable(registrationInfo).ExecuteCommandAsync() > 0;
                    }
                    else
                    {
                        // 插入新记录
                        // registrationInfo.Created_by = "System";
                        registrationInfo.Created_at = DateTime.Now;

                        return await db.Insertable(registrationInfo).ExecuteCommandAsync() > 0;
                    }
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
            string key = "ruinor1234567890"; // 这应该是一个密钥
            string machineCode = regInfo.MachineCode; // 这可能是计算机的硬件信息或唯一标识符
            // 假设用户输入的注册码
            string userProvidedCode = regInfo.RegistrationCode;
            bool isValid = HLH.Lib.Security.SecurityService.ValidateRegistrationCode(userProvidedCode, key, machineCode);
            Console.WriteLine($"提供的注册码是否有效? {isValid}");
            return isValid;
        }


        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>验证是否通过</returns>
        public async Task<bool> ValidateRegistrationAsync(tb_sys_RegistrationInfo registrationInfo)
        {
            try
            {
                if (registrationInfo == null)
                {
                    _logger.LogWarning("注册信息为空");
                    return false;
                }

                if (string.IsNullOrEmpty(registrationInfo.RegistrationCode))
                {
                    _logger.LogWarning("注册码为空");
                    return false;
                }

                // 获取机器码
                string machineCode = registrationInfo.MachineCode;

                // 验证注册码，参数顺序：提供的注册码，密钥，机器码
                bool isValid = _securityService.ValidateRegistrationCode(registrationInfo.RegistrationCode, "ruinor1234567890", machineCode);

                _logger.LogInformation($"注册码验证结果: {isValid}");
                return isValid;
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
    }
}