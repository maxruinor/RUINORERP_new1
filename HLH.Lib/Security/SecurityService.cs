using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HLH.Lib.Security
{
    public class SecurityService
    {

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
