using System;
using System.Security.Cryptography;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Security
{
    /// <summary>
    /// 加密协议处理器 - 负责数据包的加密和解密操作
    /// </summary>
    public static class CryptoProtocol
    {
        private static readonly byte[] DefaultKey = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
        private static readonly byte[] DefaultIV = new byte[] { 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };

        /// <summary>
        /// 加密服务器数据包发送到客户端
        /// </summary>
        public static EncryptedData EncryptServerToClient(OriginalData data)
        {
            byte[] oneEncrypted = EncryptData(data.One);
            byte[] twoEncrypted = EncryptData(data.Two);

            return new EncryptedData(
                head: new byte[] { data.Cmd },
                one: oneEncrypted,
                two: twoEncrypted
            );
        }

        /// <summary>
        /// 解密服务器数据包
        /// </summary>
        public static OriginalData DecryptServerPack(byte[] encryptedData)
        {
            if (encryptedData == null || encryptedData.Length < 1)
            {
                throw new ArgumentException("Invalid encrypted data");
            }

            byte cmd = encryptedData[0];
            byte[] oneDecrypted = null;
            byte[] twoDecrypted = null;

            // 简单的分割逻辑：假设数据格式为 [cmd][encrypted one][encrypted two]
            if (encryptedData.Length > 1)
            {
                int oneLength = (encryptedData.Length - 1) / 2;
                oneDecrypted = DecryptData(encryptedData[1..(1 + oneLength)]);
                twoDecrypted = DecryptData(encryptedData[(1 + oneLength)..]);
            }

            return new OriginalData(cmd, oneDecrypted, twoDecrypted);
        }

        /// <summary>
        /// 加密客户端数据包发送到服务器
        /// </summary>
        public static byte[] EncryptClientToServer(OriginalData data)
        {
            byte[] packedData = new byte[1 + (data.One?.Length ?? 0) + (data.Two?.Length ?? 0)];
            packedData[0] = data.Cmd;

            int index = 1;
            if (data.One != null)
            {
                byte[] oneEncrypted = EncryptData(data.One);
                Array.Copy(oneEncrypted, 0, packedData, index, oneEncrypted.Length);
                index += oneEncrypted.Length;
            }

            if (data.Two != null)
            {
                byte[] twoEncrypted = EncryptData(data.Two);
                Array.Copy(twoEncrypted, 0, packedData, index, twoEncrypted.Length);
            }

            return packedData;
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        private static byte[] EncryptData(byte[] data)
        {
            if (data == null || data.Length == 0)
                return data;

            using var des = DES.Create();
            using var encryptor = des.CreateEncryptor(DefaultKey, DefaultIV);
            return encryptor.TransformFinalBlock(data, 0, data.Length);
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        private static byte[] DecryptData(byte[] data)
        {
            if (data == null || data.Length == 0)
                return data;

            using var des = DES.Create();
            using var decryptor = des.CreateDecryptor(DefaultKey, DefaultIV);
            return decryptor.TransformFinalBlock(data, 0, data.Length);
        }

        /// <summary>
        /// 生成随机密钥
        /// </summary>
        public static byte[] GenerateRandomKey()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] key = new byte[8];
            rng.GetBytes(key);
            return key;
        }

        /// <summary>
        /// 生成随机初始化向量
        /// </summary>
        public static byte[] GenerateRandomIV()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] iv = new byte[8];
            rng.GetBytes(iv);
            return iv;
        }

        /// <summary>
        /// 设置自定义加密密钥
        /// </summary>
        public static void SetEncryptionKeys(byte[] key, byte[] iv)
        {
            if (key == null || key.Length != 8)
                throw new ArgumentException("Key must be 8 bytes long");
            if (iv == null || iv.Length != 8)
                throw new ArgumentException("IV must be 8 bytes long");

            Array.Copy(key, DefaultKey, 8);
            Array.Copy(iv, DefaultIV, 8);
        }

        /// <summary>
        /// 计算数据哈希值（用于完整性验证）
        /// </summary>
        public static byte[] ComputeHash(byte[] data)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(data);
        }

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        public static bool VerifyIntegrity(byte[] data, byte[] expectedHash)
        {
            byte[] actualHash = ComputeHash(data);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}