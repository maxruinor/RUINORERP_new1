using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Security
{
    /// <summary>
    /// 好像可以生成 暂时不有。用hlh.lib。因为要公共调用
    /// </summary>
    public class SecurityService
    {
        public static void RegisterMachine(tb_sys_RegistrationInfo regInfo, string key)
        {
            string generatedCode = SecurityService.GenerateRegistrationCode(key, regInfo.MachineCode);
            if (SecurityService.ValidateRegistrationCode(generatedCode, key, regInfo.MachineCode))
            {
                regInfo.RegistrationCode = generatedCode;
                regInfo.IsRegistered = true;
                // 这里应该将注册信息保存到数据库
                Console.WriteLine("Registration successful.");
            }
            else
            {
                Console.WriteLine("Registration failed.");
            }
        }

        public static string GenerateRegistrationCode(string key, string machineCode)
        {
            using (var algorithm = SHA256.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] machineCodeBytes = Encoding.UTF8.GetBytes(machineCode);
                byte[] combinedBytes = new byte[keyBytes.Length + machineCodeBytes.Length];
                Array.Copy(keyBytes, combinedBytes, keyBytes.Length);
                Array.Copy(machineCodeBytes, 0, combinedBytes, keyBytes.Length, machineCodeBytes.Length);

                byte[] hash = algorithm.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool ValidateRegistrationCode(string providedCode, string key, string machineCode)
        {
            string generatedCode = GenerateRegistrationCode(key, machineCode);
            return generatedCode == providedCode;
        }
    }
}
