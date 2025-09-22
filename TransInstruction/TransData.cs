namespace TransInstruction
{
    /// <summary>
    /// 加密前的数据
    /// </summary>
    public struct OriginalData
    {

        /// <summary>
        /// 主指令
        /// </summary>
        public byte cmd;


        /// <summary>
        /// 子指令
        /// </summary>
        public byte[] One;

        /// <summary>
        /// 数据体
        /// </summary>
        public byte[] Two;
    }

    /// <summary>
    /// 加密后的数据
    /// </summary>
    public struct EncryptedData
    {
        public byte[] head;
        public byte[] one;
        public byte[] two;
    }
}
