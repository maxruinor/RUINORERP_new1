namespace TransInstruction
{
    /// <summary>
    /// 加密前的数据
    /// </summary>
    public struct OriginalData
    {
        public byte cmd;
        public byte[] One;
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
