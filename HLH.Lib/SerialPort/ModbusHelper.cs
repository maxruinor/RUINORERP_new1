namespace IPS.Lib
{
    /// <summary>
    /// modbus协议类
    /// </summary>
    public class ModbusHelper
    {


        //BuildMessage方法：这个方法中message数组中的[0]~[7]的值是对应固定类型信息的（协议里的规定）。例如[0]代表地址、[1]代表功能。 
        /// <summary>
        /// 构建查询命令
        /// <para>这个方法中message数组中的[0]~[7]的值是对应固定类型信息的（协议里的规定）。例如[0]代表地址、[1]代表功能。 </para>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type">查询命令为03</param>
        /// <param name="start"></param>
        /// <param name="registers"></param>
        /// <param name="message"></param>
        public static void BuildMessage(byte address, byte type, ushort start, ushort registers, ref byte[] message)
        {
            byte[] CRC = new byte[2];//声明两个字节的CRC数组 

            message[0] = address;//设备地址为address 
            message[1] = type;// 这条消息是要读取什么功能码，这里传进来的是03功能码
            message[2] = (byte)(start >> 8);//起始寄存器地址高8位
            message[3] = (byte)start;//起始寄存器地址低8位 
            message[4] = (byte)(registers >> 8);//读取的寄存器数高8位
            message[5] = (byte)registers;//读取的寄存器数低8位 

            //test
            //Lib.CRC16 cr16 = new CRC16();
            //cr16.Crc(message);


            ToolHelper.GetCRC(message, ref CRC);//GetCRC方法内容详见后面的GetCRC方法代码 
            message[message.Length - 2] = CRC[0];//CRC校验的低8位 
            message[message.Length - 1] = CRC[1];//CRC校验的高8位 
        }
    }
}
