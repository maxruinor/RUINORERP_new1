using System;

namespace RUINORERP.TopServer.Model
{
    /// <summary>
    /// 机器硬件信息模型
    /// 用于生成机器码和验证注册码
    /// </summary>
    public class MachineInfo
    {
        /// <summary>
        /// 机器码（基于硬件生成的唯一标识）
        /// </summary>
        public string MachineCode { get; set; } = string.Empty;

        /// <summary>
        /// CPU序列号
        /// </summary>
        public string CpuId { get; set; } = string.Empty;

        /// <summary>
        /// 主板序列号
        /// </summary>
        public string MotherboardId { get; set; } = string.Empty;

        /// <summary>
        /// 硬盘序列号
        /// </summary>
        public string DiskId { get; set; } = string.Empty;

        /// <summary>
        /// MAC地址
        /// </summary>
        public string MacAddress { get; set; } = string.Empty;

        /// <summary>
        /// BIOS序列号
        /// </summary>
        public string BiosId { get; set; } = string.Empty;

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 生成机器码（SHA256哈希）
        /// </summary>
        public string GenerateMachineCode()
        {
            var hardwareString = $"{CpuId}|{MotherboardId}|{DiskId}|{MacAddress}|{BiosId}";
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(hardwareString);
                var hash = sha256.ComputeHash(bytes);
                MachineCode = BitConverter.ToString(hash).Replace("-", "").Substring(0, 32);
                return MachineCode;
            }
        }

        /// <summary>
        /// 验证机器码是否匹配
        /// </summary>
        public bool VerifyMachineCode(string inputCode)
        {
            return GenerateMachineCode().Equals(inputCode, StringComparison.OrdinalIgnoreCase);
        }
    }
}
