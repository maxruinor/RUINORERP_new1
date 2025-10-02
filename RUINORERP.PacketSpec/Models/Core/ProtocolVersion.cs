namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 协议版本控制
    /// </summary>
    public static class ProtocolVersion
    {
        /// <summary>
        /// 主版本号 - 不兼容的协议变更
        /// </summary>
        public const int Major = 1;

        /// <summary>
        /// 次版本号 - 向后兼容的功能性新增
        /// </summary>
        public const int Minor = 0;

        /// <summary>
        /// 修订号 - 向后兼容的问题修正
        /// </summary>
        public const int Patch = 0;

        /// <summary>
        /// 获取当前协议版本
        /// </summary>
        public static string Current => $"{Major}.{Minor}.{Patch}";

        /// <summary>
        /// 协议版本标识头
        /// </summary>
        public const string VersionHeader = "RERP-Packet";

        /// <summary>
        /// 检查版本兼容性
        /// </summary>
        public static bool IsCompatible(string otherVersion)
        {
            if (string.IsNullOrEmpty(otherVersion))
                return false;

            var parts = otherVersion.Split('.');
            if (parts.Length != 3)
                return false;

            // 主版本必须相同
            if (int.TryParse(parts[0], out int otherMajor) && otherMajor == Major)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 生成完整的协议标识
        /// </summary>
        public static string GetFullProtocolIdentifier()
        {
            return $"{VersionHeader}-v{Current}";
        }
    }
}
