using HLH.Lib.Helper;
using System;
using System.Diagnostics;
using System.Media;

namespace IPS.Lib
{
    public class ToolHelper
    {


        /// <summary>
        /// firetrucksiren.wav
        /// </summary>
        public static void PlayWarningSound(string wavFileName)
        {
            SoundPlayer player = new SoundPlayer(wavFileName);
            player.Play();
        }

        /// <summary>
        /// GetCRC方法：在Modbus协议中这个函数叫CRC校验。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="CRC"></param>
        public static void GetCRC(byte[] message, ref byte[] CRC)
        {
            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF;
            byte CRCLow = 0xFF;
            char CRCLSB;
            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }

        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="sourceArray"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] CopyByteArrayFromSourceByteArray(Array sourceArray, ref int startIndex, int length)
        {

            byte[] result = new byte[length];
            try
            {
                // Array.Copy(sourceArray, startIndex, result, 0, length);
                Buffer.BlockCopy(sourceArray, startIndex, result, 0, length);
                startIndex = startIndex + length;
            }
            catch (Exception ex)
            {
                log4netHelper.error("工具类中,复制字节", ex);
            }
            return result;

        }


        /// <summary>
        /// 跳过的字节数
        /// </summary>
        /// <param name="startindex">起点</param>
        /// <param name="skipNum">跳过的字节数</param>
        /// <returns></returns>
        public static void skipBytes(ref int startindex, int skipNum)
        {
            startindex = startindex + skipNum;
        }




        /// <summary>
        /// 注册第三方控件
        /// </summary>
        /// <param name="startPath"></param>
        public static void regsvrOcx(string startPath)
        {
            string targetMsdll = "\"" + startPath + "\\EDSockServer.ocx.dll\"";
            Process.Start("regsvr32.exe", targetMsdll + " -s");
        }

        /// <summary>
        /// 将decimal 按指定小数位取字符返回
        /// </summary>
        /// <returns></returns>
        public static string GetStrFromDecimal(decimal? dData, int iLength)
        {
            if (dData.HasValue)
            {
                return decimal.Round(dData.Value, 2).ToString();
            }
            else
            {
                return decimal.Round(0, 2).ToString();
            }
        }
    }
}
