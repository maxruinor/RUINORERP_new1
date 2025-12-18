using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLH.Lib.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class RegistrationCodeGenerator
    {
        private static readonly string LicenseKey = "your_license_key_here11"; // 替换为你的注册码

        public static string GenerateMachineCode(string hardwareInfo, string registrationInfo)
        {
            // 将硬件标识和注册信息组合
            string combinedInfo = hardwareInfo + registrationInfo;

            // 使用HMACSHA256和注册码生成机器码
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(LicenseKey)))
            {
                byte[] keyByte = hmac.ComputeHash(Encoding.UTF8.GetBytes(combinedInfo));
                return BitConverter.ToString(keyByte).Replace("-", "").ToLower(); // 转换为字符串
            }
        }

        public static bool ValidateRegistrationCode(string registrationCode, string hardwareInfo, string registrationInfo)
        {
            // 生成预期的机器码
            string expectedMachineCode = GenerateMachineCode(hardwareInfo, registrationInfo);

            // 比较输入的注册码和预期的机器码
            return registrationCode.Equals(expectedMachineCode);
        }


        // 使用示例

        public static void test()
        {
            string hardwareInfo = "CPU-1234567890"; // 假设的硬件标识
            string registrationInfo = "Users:5, Expires:2024-12-31"; // 假设的注册信息

            // 生成注册码
            string registrationCode = RegistrationCodeGenerator.GenerateMachineCode(hardwareInfo, registrationInfo);
            System.Diagnostics.Debug.WriteLine("Generated Registration Code: " + registrationCode);

            // 验证注册码
            bool isValid = RegistrationCodeGenerator.ValidateRegistrationCode(registrationCode, hardwareInfo, registrationInfo);
            System.Diagnostics.Debug.WriteLine("Is the registration code valid? " + isValid);
        }
    }
}
