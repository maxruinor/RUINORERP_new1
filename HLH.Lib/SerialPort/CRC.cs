using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IPS.Lib
{


    public sealed class CRC
    {
        private static ushort[] CRC16Table = null;
        private static uint[] CRC32Table = null;



        public static int GetKey(byte[] data)
        {
            int count = data.Length;
            byte[] buf = new byte[data.Length + 2];
            data.CopyTo(buf, 0);
            int ptr = 0;
            int i = 0;
            int crc = 0;
            byte crc1, crc2, crc3;
            crc1 = buf[ptr++];
            crc2 = buf[ptr++];
            buf[count] = 0;
            buf[count + 1] = 0;
            while (--count >= 0)
            {
                crc3 = buf[ptr++];
                for (i = 0; i < 8; i++)
                {
                    if (((crc1 & 0x80) >> 7) == 1)//判断crc1高位是否为1
                    {
                        crc1 = (byte)(crc1 << 1); //移出高位
                        if (((crc2 & 0x80) >> 7) == 1)//判断crc2高位是否为1
                        {
                            crc1 = (byte)(crc1 | 0x01);//crc1低位由0变1
                        }
                        crc2 = (byte)(crc2 << 1);//crc2移出高位
                        if (((crc3 & 0x80) >> 7) == 1) //判断crc3高位是否为1
                        {
                            crc2 = (byte)(crc2 | 0x01); //crc2低位由0变1
                        }
                        crc3 = (byte)(crc3 << 1);//crc3移出高位
                        crc1 = (byte)(crc1 ^ 0x10);
                        crc2 = (byte)(crc2 ^ 0x21);
                    }
                    else
                    {
                        crc1 = (byte)(crc1 << 1); //移出高位
                        if (((crc2 & 0x80) >> 7) == 1)//判断crc2高位是否为1
                        {
                            crc1 = (byte)(crc1 | 0x01);//crc1低位由0变1
                        }
                        crc2 = (byte)(crc2 << 1);//crc2移出高位
                        if (((crc3 & 0x80) >> 7) == 1) //判断crc3高位是否为1
                        {
                            crc2 = (byte)(crc2 | 0x01); //crc2低位由0变1
                        }
                        crc3 = (byte)(crc3 << 1);//crc3移出高位
                    }
                }
            }
            crc = (int)((crc1 << 8) + crc2);
            return crc;
        }

        public byte[] GetCRC(byte[] bufData)
        {
            ushort CRC = 0x0000;
            ushort POLYNOMIAL = 0x8408;
            for (int i = 0; i < bufData.Length; i++)
            {
                CRC ^= bufData[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((CRC & 0x0001) != 0)
                    {
                        CRC >>= 1;
                        CRC ^= POLYNOMIAL;
                    }
                    else
                    {
                        CRC >>= 1;
                    }
                }
            }
            return System.BitConverter.GetBytes(CRC);
        }

        public byte[] sendData(byte[] Str)
        {
            byte temp = Str[0];
            for (int i = 1; i <= Str.Length - 1; i++)
            {
                temp = Convert.ToByte(temp ^ Str[i]);
            }
            List<Byte> bytesList = new List<byte>();
            foreach (byte b in Str)
            {
                bytesList.Add(b);
            }
            bytesList.Add(temp);
            Str = bytesList.ToArray();
            return Str;
        }

        private static void MakeCRC16Table()
        {
            if (CRC16Table != null) return;
            CRC16Table = new ushort[256];
            for (ushort i = 0; i < 256; i++)
            {
                ushort vCRC = i;
                for (int j = 0; j < 8; j++)
                    if (vCRC % 2 == 0)
                        vCRC = (ushort)(vCRC >> 1);
                    else vCRC = (ushort)((vCRC >> 1) ^ 0x8408);
                CRC16Table[i] = vCRC;
            }
        }
        //设计 ZswangY37 2007-02-14
        private static void MakeCRC32Table()
        {
            if (CRC32Table != null) return;
            CRC32Table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint vCRC = i;
                for (int j = 0; j < 8; j++)
                    if (vCRC % 2 == 0)
                        vCRC = (uint)(vCRC >> 1);
                    else vCRC = (uint)((vCRC >> 1) ^ 0xEDB88320);
                CRC32Table[i] = vCRC;
            }
        }
        public static ushort UpdateCRC16(byte AByte, ushort ASeed)
        {
            return (ushort)(CRC16Table[(ASeed & 0x000000FF) ^ AByte] ^ (ASeed >> 8));
        }
        public static uint UpdateCRC32(byte AByte, uint ASeed)
        {
            return (uint)(CRC32Table[(ASeed & 0x000000FF) ^ AByte] ^ (ASeed >> 8));
        }

        public static ushort CRC16(byte[] datas, int beginIdx, int len)
        {
            const ushort polinomio = 0xA001;
            ushort result = 0xFFFF;
            for (int i = beginIdx; i < len; i++)
            {
                byte tmp = datas[i];
                result = (ushort)(tmp ^ result);
                for (int j = 0; j < 8; j++)
                {
                    if ((result & 0x0001) == 1)
                    {
                        result = (ushort)((result >> 1) ^ polinomio);
                    }
                    else
                    {
                        result = (ushort)(result >> 1);
                    }
                }
            }
            return result;
        }


        public static ushort CRC16(byte[] ABytes)
        {
            MakeCRC16Table();
            ushort Result = 0xFFFF;
            foreach (byte vByte in ABytes)
                Result = UpdateCRC16(vByte, Result);
            return (ushort)(~Result);
        }
        public static ushort CRC16(string AString, Encoding AEncoding)
        {
            return CRC16(AEncoding.GetBytes(AString));
        }
        public static ushort CRC16(string AString)
        {
            return CRC16(AString, Encoding.UTF8);
        }
        public static uint CRC32(byte[] ABytes)
        {
            MakeCRC32Table();
            uint Result = 0xFFFFFFFF;
            foreach (byte vByte in ABytes)
                Result = UpdateCRC32(vByte, Result);
            return (uint)(~Result);
        }
        public static uint CRC32(string AString, Encoding AEncoding)
        {
            return CRC32(AEncoding.GetBytes(AString));
        }
        public static uint CRC32(string AString)
        {
            return CRC32(AString, Encoding.UTF8);
        }
    }

    public sealed class Crc32
    {
        #region Fields
        private readonly static uint CrcSeed = 0xFFFFFFFF;

        private readonly static uint[] CrcTable = new uint[] {
   0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419,
   0x706AF48F, 0xE963A535, 0x9E6495A3, 0x0EDB8832, 0x79DCB8A4,
   0xE0D5E91E, 0x97D2D988, 0x09B64C2B, 0x7EB17CBD, 0xE7B82D07,
   0x90BF1D91, 0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE,
   0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7, 0x136C9856,
   0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9,
   0xFA0F3D63, 0x8D080DF5, 0x3B6E20C8, 0x4C69105E, 0xD56041E4,
   0xA2677172, 0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B,
   0x35B5A8FA, 0x42B2986C, 0xDBBBC9D6, 0xACBCF940, 0x32D86CE3,
   0x45DF5C75, 0xDCD60DCF, 0xABD13D59, 0x26D930AC, 0x51DE003A,
   0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423, 0xCFBA9599,
   0xB8BDA50F, 0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924,
   0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D, 0x76DC4190,
   0x01DB7106, 0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F,
   0x9FBFE4A5, 0xE8B8D433, 0x7807C9A2, 0x0F00F934, 0x9609A88E,
   0xE10E9818, 0x7F6A0DBB, 0x086D3D2D, 0x91646C97, 0xE6635C01,
   0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E, 0x6C0695ED,
   0x1B01A57B, 0x8208F4C1, 0xF50FC457, 0x65B0D9C6, 0x12B7E950,
   0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3,
   0xFBD44C65, 0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2,
   0x4ADFA541, 0x3DD895D7, 0xA4D1C46D, 0xD3D6F4FB, 0x4369E96A,
   0x346ED9FC, 0xAD678846, 0xDA60B8D0, 0x44042D73, 0x33031DE5,
   0xAA0A4C5F, 0xDD0D7CC9, 0x5005713C, 0x270241AA, 0xBE0B1010,
   0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
   0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17,
   0x2EB40D81, 0xB7BD5C3B, 0xC0BA6CAD, 0xEDB88320, 0x9ABFB3B6,
   0x03B6E20C, 0x74B1D29A, 0xEAD54739, 0x9DD277AF, 0x04DB2615,
   0x73DC1683, 0xE3630B12, 0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8,
   0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1, 0xF00F9344,
   0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB,
   0x196C3671, 0x6E6B06E7, 0xFED41B76, 0x89D32BE0, 0x10DA7A5A,
   0x67DD4ACC, 0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5,
   0xD6D6A3E8, 0xA1D1937E, 0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1,
   0xA6BC5767, 0x3FB506DD, 0x48B2364B, 0xD80D2BDA, 0xAF0A1B4C,
   0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55, 0x316E8EEF,
   0x4669BE79, 0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236,
   0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F, 0xC5BA3BBE,
   0xB2BD0B28, 0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31,
   0x2CD99E8B, 0x5BDEAE1D, 0x9B64C2B0, 0xEC63F226, 0x756AA39C,
   0x026D930A, 0x9C0906A9, 0xEB0E363F, 0x72076785, 0x05005713,
   0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38, 0x92D28E9B,
   0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21, 0x86D3D2D4, 0xF1D4E242,
   0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1,
   0x18B74777, 0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C,
   0x8F659EFF, 0xF862AE69, 0x616BFFD3, 0x166CCF45, 0xA00AE278,
   0xD70DD2EE, 0x4E048354, 0x3903B3C2, 0xA7672661, 0xD06016F7,
   0x4969474D, 0x3E6E77DB, 0xAED16A4A, 0xD9D65ADC, 0x40DF0B66,
   0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
   0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605,
   0xCDD70693, 0x54DE5729, 0x23D967BF, 0xB3667A2E, 0xC4614AB8,
   0x5D681B02, 0x2A6F2B94, 0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B,
   0x2D02EF8D
  };

        private uint crc = 0; // crc data checksum so far.
        #endregion

        #region Constructors
        public Crc32()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the CRC32 data checksum computed so far.
        /// </summary>
        public uint Value
        {
            get
            {
                return crc;
            }
            set
            {
                crc = value;
            }
        }
        #endregion

        #region Methods

        public static uint GetStreamCRC32(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("stream is not readable.");
            stream.Position = 0;
            Crc32 crc32 = new Crc32();
            byte[] buf = new byte[4096];
            int len = 0;
            while ((len = stream.Read(buf, 0, buf.Length)) != 0)
            {
                crc32.Update(buf, 0, len);
            }
            stream.Position = 0;
            return crc32.Value;
        }

        public static uint GetFileCRC32(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            Crc32 crc32 = new Crc32();
            byte[] buf = new byte[4096];
            int len = 0;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                while ((len = fs.Read(buf, 0, buf.Length)) != 0)
                {
                    crc32.Update(buf, 0, len);
                }
            }
            return crc32.Value;
        }

        /// <summary>
        /// Resets the CRC32 data checksum as if no update was ever called.
        /// </summary>
        public void Reset()
        {
            crc = 0;
        }

        /// <summary>
        /// Updates the checksum with the int bval.
        /// </summary>
        /// <param name = "bval">
        /// the byte is taken as the lower 8 bits of bval
        /// </param>
        public void Update(int bval)
        {
            crc ^= CrcSeed;
            crc = CrcTable[(crc ^ bval) & 0xFF] ^ (crc >> 8);
            crc ^= CrcSeed;
        }

        /// <summary>
        /// Updates the checksum with the bytes taken from the array.
        /// </summary>
        /// <param name="buffer">
        /// buffer an array of bytes
        /// </param>
        public void Update(byte[] buffer)
        {
            Update(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Adds the byte array to the data checksum.
        /// </summary>
        /// <param name = "buf">
        /// the buffer which contains the data
        /// </param>
        /// <param name = "off">
        /// the offset in the buffer where the data starts
        /// </param>
        /// <param name = "len">
        /// the length of the data
        /// </param>
        public void Update(byte[] buf, int off, int len)
        {
            if (buf == null)
            {
                throw new ArgumentNullException("buf");
            }

            if (off < 0 || len < 0 || off + len > buf.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            crc ^= CrcSeed;

            while (--len >= 0)
            {
                crc = CrcTable[(crc ^ buf[off++]) & 0xFF] ^ (crc >> 8);
            }

            crc ^= CrcSeed;
        }
        #endregion
    }



    public interface ICRC
    {

        long Value
        {
            get;
        }

        void Reset();

        void Crc(int bval);

        void Crc(byte[] buffer);

        void Crc(byte[] buf, int off, int len);
    }


    public class HexMath
    {
        private const int CRC_LEN = 2;

        // Table of CRC values for high-order byte
        private readonly byte[] _auchCRCHi = new byte[]
        {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40
        };

        // Table of CRC values for low-order byte
        private readonly byte[] _auchCRCLo = new byte[]
        {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06,
            0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD,
            0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A,
            0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC, 0x14, 0xD4,
            0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3,
            0xF2, 0x32, 0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4,
            0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29,
            0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED,
            0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60,
            0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67,
            0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68,
            0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA, 0xBE, 0x7E,
            0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71,
            0x70, 0xB0, 0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92,
            0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B,
            0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89, 0x4B, 0x8B,
            0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42,
            0x43, 0x83, 0x41, 0x81, 0x80, 0x40
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public HexMath()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public ushort CalculateCrc16(byte[] buffer)
        {
            byte crcHi = 0xff;  // high crc byte initialized
            byte crcLo = 0xff;  // low crc byte initialized

            for (int i = 0; i < buffer.Length - CRC_LEN; i++)
            {
                int crcIndex = crcHi ^ buffer[i]; // calculate the crc lookup index

                crcHi = (byte)(crcLo ^ _auchCRCHi[crcIndex]);
                crcLo = _auchCRCLo[crcIndex];
            }

            return (ushort)(crcHi << 8 | crcLo);
        }
        // end-class

        //CalculateCrc16返回的是一个ushort类型的值，如果要返回Crc高字节和低字节，可重写CalculateCrc16函数为：
        public ushort CalculateCrc16(byte[] buffer, out byte crcHi, out byte crcLo)
        {
            crcHi = 0xff;  // high crc byte initialized
            crcLo = 0xff;  // low crc byte initialized

            for (int i = 0; i < buffer.Length - CRC_LEN; i++)
            {
                int crcIndex = crcHi ^ buffer[i]; // calculate the crc lookup index

                crcHi = (byte)(crcLo ^ _auchCRCHi[crcIndex]);
                crcLo = _auchCRCLo[crcIndex];
            }

            return (ushort)(crcHi << 8 | crcLo);
        }


    }
    /// <summary>
    /// CRC16 的摘要说明。
    /// </summary>
    public class CRC16 : ICRC
    {
        #region CRC 16 位校验表

        /// <summary>
        /// 16 位校验表 Upper 表
        /// </summary>
        public ushort[] uppercrctab = new ushort[]
   {
    0x0000,0x1231,0x2462,0x3653,0x48c4,0x5af5,0x6ca6,0x7e97,
    0x9188,0x83b9,0xb5ea,0xa7db,0xd94c,0xcb7d,0xfd2e,0xef1f
   };
        /// <summary>
        /// 16 位校验表 Lower 表
        /// </summary>
        public ushort[] lowercrctab = new ushort[]
   {
    0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7,
    0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef
   };
        #endregion
        ushort crc = 0;
        /// <summary>
        /// 校验后的结果
        /// </summary>
        public long Value
        {
            get
            {
                return crc;
            }
            set
            {
                crc = (ushort)value;
            }
        }
        /// <summary>
        /// 设置crc 初始值
        /// </summary>
        public void Reset()
        {
            crc = 0;
        }

        /// <summary>
        /// Crc16
        /// </summary>
        /// <param name="ucrc"></param>
        /// <param name="buf"></param>
        public void Crc(ushort ucrc, byte[] buf)
        {
            crc = ucrc;
            Crc(buf);
        }

        /// <summary>
        /// Crc16
        /// </summary>
        /// <param name="bval"></param>
        public void Crc(int bval)
        {
            ushort h = (ushort)((crc >> 12) & 0x0f);
            ushort l = (ushort)((crc >> 8) & 0x0f);
            ushort temp = crc;
            temp = (ushort)(((temp & 0x00ff) << 8) | bval);
            temp = (ushort)(temp ^ (uppercrctab[(h - 1) + 1] ^ lowercrctab[(l - 1) + 1]));
            crc = temp;
        }

        /// <summary>
        /// Crc16
        /// </summary>
        /// <param name="buffer"></param>
        public void Crc(byte[] buffer)
        {
            Crc(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// Crc16
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        public void Crc(byte[] buf, int off, int len)
        {
            if (buf == null)
            {
                throw new ArgumentNullException("buf");
            }

            if (off < 0 || len < 0 || off + len > buf.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = off; i < len; i++)
            {
                Crc(buf[i]);
            }
        }

    }
}