using System;
using System.Linq;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
 
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using System.IO;
using RUINORERP.Server.Commands;
using RUINORERP.Server.ServerSession;
using TransInstruction;
using RUINORERP.Server.Lib;

namespace RUINORERP.Server
{
    /// <summary>
    /// 特殊处理的情况
    /// </summary>
    [Command(Key = "XT")]
    [AsyncKeyUpperCommandFilter]
    public class XTCommand : IAsyncCommand<BizPackageInfo>
    {
        public async ValueTask ExecuteAsync(IAppSession session, BizPackageInfo package)
        {

            await Task.Delay(0);
            SessionforBiz PlayerSession = session as SessionforBiz;
            switch (package.ecode)
            {
                case SpecialOrder.固定256:
                    // cmd服务指令集合.SendfixData(PlayerSession);
                    break;
                case SpecialOrder.长度等于18:
                    PacketProcess ap = new PacketProcess(PlayerSession);
                    //服务端发送字符串 22
                    //ap.WriteClientData(PlayerSession, 0x03, new byte[] { 0x01, 0x00, 0x01, 0x00 }, null);
                    OriginalData gdd = new OriginalData();
                    gdd.cmd = 3;
                    gdd.One = new byte[] { 0x01, 0x00, 0x01, 0x00 };
                    gdd.Two = null;
                    PlayerSession.AddSendData(gdd);

                    byte[] buffer = Tools.HexStrTobyte("CDE704FF85575FDB1BD5FC83EA7CB9F61F6FCFF301FF");
                    TransPackProcess gpp = new TransPackProcess();
                    OriginalData gd = gpp.UnServerPack(buffer);

                    // PlayerSession.AddSendData(gd);
                    break;
                case SpecialOrder.长度小于18:
                    break;
                case SpecialOrder.正常:

                    break;
                default:
                    break;
            }
            /*
     //先接收固定的256字节
     socket.ReadWithLength(256, 60000);

     //服务端发送字符串 22
     socket.WriteClientData(0x03, new byte[] { 0x01, 0x00, 0x01, 0x00 }, null);
     */

            if (package.Key == "XT")
            {
                // DataBuilder db = new DataBuilder();
                // KxData kd = new KxData();
                // kd.cmd = 0x03;
                // kd.One = new byte[] { 0x01, 0x00, 0x01, 0x00 };
                // kd.Two = null;
                //PacketProcess ap = new PacketProcess(PlayerSession);
                //服务端发送字符串 22
                //ap.WriteClientData(PlayerSession, 0x03, new byte[] { 0x01, 0x00, 0x01, 0x00 }, null);
                //监控输出的数据
                // string str1 = Tools.byteToHexStr(newkd.head, true);
                // string str2 = Tools.byteToHexStr(newkd.One, true);
                // string str3 = Tools.byteToHexStr(newkd.Two, true);
                //ServiceforGame<SephirothServer.Server.GamePackageInfo> sgs = (this as IAppSession).Server as ServiceforGame<SephirothServer.Server.GamePackageInfo>;
                //if (GlobalSettings._debug == DebugController.Debug)
                //{
                //    log4netHelper.debug("XT:" + newkd.head.Length + "\r\n " + str1);
                //    log4netHelper.debug("XT:" + newkd.One.Length + "\r\n " + str2);
                //    log4netHelper.debug("XT:" + newkd.Two.Length + "\r\n " + str3);
                //}
                //监控输出的数据end
                PlayerSession.UseXT = true;
            }
            else
            {
                //这不应该执行
            }

        }



        // 16进制字符串转字节数组   格式为 string sendMessage = "00 01 00 00 00 06 FF 05 00 64 00 00";
        private static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }


        // 字节数组转16进制字符串   
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");//ToString("X2") 为C#中的字符串格式控制符
                }
            }
            return returnStr;
        }
        //字节数组转16进制更简单的，利用BitConverter.ToString方法
        //string str0x = BitConverter.ToString(result, 0, result.Length).Replace("-"," ");


    }
}
