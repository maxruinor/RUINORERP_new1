using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TransInstruction;
using TransInstruction.DataPortal;

namespace TransInstruction
{

    /// <summary>
    /// 服务器动作
    /// </summary>
    public class ActionForServer
    {
        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void OtherHandler(ClientCmdEnum msg, OriginalData gd);

        [Browsable(true), Description("引发外部事件，补充作用而已")]
        public event OtherHandler OtherEvent;

        /*
                  if (OtherEvent != null)
            {
                OtherEvent(gd);
            }
         */

        public static string Try解析封包为实际逻辑(OriginalData gd)
        {

            //调试用 文字部分最有规律 
            string rs = string.Empty;
            try
            {

                #region 第一部分
                if (gd.One != null)
                {

                }
                #endregion

                #region 第二部分
                if (gd.Two != null)
                {
                    int indexOK = 0;
                    int index = 0;

                    //通常最开始是2位指令
                    #region 通常最开始是指令
                    UInt32 cmd = gd.cmd;
                    if (gd.Two != null)
                    {
                        cmd <<= 8;
                        cmd |= gd.Two[1];
                        cmd <<= 8;
                        cmd |= gd.Two[0];
                    }
                    PackageSourceType pst = TransProtocol.CheckType(cmd, out rs);
                    #endregion


                    indexOK = index;
                    //======================================
                    //先分别取4位
                    index = 2;
                    bool success = false;
                    string s1 = ByteDataAnalysis.TryGetString(gd.Two, out success, ref index);
                    string s2 = ByteDataAnalysis.TryGetString(gd.Two, out success, ref index);

                    int value = ByteDataAnalysis.TryGetInt(gd.Two, out success, ref index);
                    //
                    int a = 1;
                }
                #endregion
            }
            catch (Exception ex)
            {


            }
            return rs;
        }

        public static string 解析客户端角色封包(OriginalData gd)
        {
            string rs = string.Empty;
            #region 第一部分
            if (gd.One != null)
            {

            }
            #endregion

            #region 第二部分
            if (gd.Two != null)
            {
                int indexOK = 0;
                int index = 0;

                //通常最开始是2位指令
                #region 通常最开始是指令
                UInt32 cmd = gd.cmd;
                if (gd.Two != null)
                {
                    cmd <<= 8;
                    cmd |= gd.Two[1];
                    cmd <<= 8;
                    cmd |= gd.Two[0];
                }
                //Int16 cmd = ByteDataAnalysis.GetInt16(gd.Two, ref index);
                //尝试转枚举
                ServerCmdEnum sm = (ServerCmdEnum)cmd;
                int tempMj = 0;
                if (int.TryParse(sm.ToString(), out tempMj))
                {
                    if (tempMj > 0)
                    {
                        ClientCmdEnum cm = (ClientCmdEnum)cmd;
                        rs += "Client:" + cm.ToString() + "|" + cm.ToString("X") + " ";
                    }
                    else
                    {
                        rs += "Server:" + sm.ToString() + "|" + sm.ToString("X") + " ";
                    }

                }
                else
                {
                    rs += "Server:" + sm.ToString() + "|" + sm.ToString("X") + " ";
                }
                #endregion
                indexOK = index;
                //======================================
                //先分别取4位
                index = 10;
                byte[] UnparsedData = new byte[gd.Two.Length - index];
                bool success = false;
                string 角色名 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string 种族 = ByteDataAnalysis.GetString(gd.Two, ref index);
                byte sex = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                string 职业 = ByteDataAnalysis.GetString(gd.Two, ref index);
                //5*6 5个1位为1，5个0 25位全为0   00000 10000  100000  100000  100000  100000  
                index += 30;//by by watson tx.PushString(f获取行会名称());
                string 发型 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                //1 0000
                index += 5;
                string HunterSuit = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                int t1 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                byte b2 = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                byte b3 = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                byte b4 = ByteDataAnalysis.Getbyte(gd.Two, ref index);

                string 所属地 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                //0000
                int l1 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int 等级 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int l2 = ByteDataAnalysis.GetInt(gd.Two, ref index); //0
                int l3 = ByteDataAnalysis.GetInt(gd.Two, ref index);//5

                Int64 经验值 = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                Int64 总经验值 = ByteDataAnalysis.GetInt64(gd.Two, ref index);//S等级经验.f获取总经验(v等级)

                int 总血量1 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int 总血量2 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int 总蓝量1 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int 总蓝量2 = ByteDataAnalysis.GetInt(gd.Two, ref index);

                /*
                   tx.PushInt64(S等级经验.f获取总经验(v等级));
            tx.PushInt((int)v基础属性[e基础属性.总血量]);
            tx.PushInt((int)v基础属性[e基础属性.总血量]);
            tx.PushInt((int)v基础属性[e基础属性.总蓝量]);
            tx.PushInt((int)v基础属性[e基础属性.总蓝量]);
                 */



                int 力量 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int 敏捷 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int 精神 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                //4个int 0    4*4=16
                int V白魔法 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int V红魔法 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int V蓝魔法 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int V黄魔法 = ByteDataAnalysis.GetInt(gd.Two, ref index);

                int 黑魔法 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int 可分配点数 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int 金币 = ByteDataAnalysis.GetInt(gd.Two, ref index);

                List<string> W物品List = new List<string>();
                for (int i = 0; i < 32; i++)  //身上有29个可以放东西的位置 ？
                {
                    byte 是否穿戴物品 = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                    if (是否穿戴物品 == 0)
                    {
                        continue;
                    }
                    else
                    {
                        //有物品 f详细信息
                        string 物品外观2D = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                        int 物品等级 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        byte 物品颜色 = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                        string 物品外观3D = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                        string v英文名称 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                        //var 合成名称 = V中文名称.Split('\n')[0]; //为了频道 看方法 f详细信息
                        string 合成名称 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                        string 物品v备注 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);

                        int l10 = ByteDataAnalysis.GetInt(gd.Two, ref index);//0
                        int v宽度 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        int v高度 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        string 物品信息v类型 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                        int v持久开关1 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        int v持久开关2 = ByteDataAnalysis.GetInt(gd.Two, ref index);


                        int vi总耐久 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        //这里要修改 by watson
                        //float -> System.Single (单精度浮点型，占 4 个字节，表示32位浮点值)
                        // float v总耐久 = BitConverter.ToSingle(vi总耐久);
                        int vi当前耐久 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        // float v当前耐久 = BitConverter.ToSingle(vi当前耐久);
                        byte 物品信息v数量 = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                        if (物品信息v数量 == 1)
                        {
                            byte ww数量 = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                        }
                        Int64 出售价格 = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                        Int64 维修价格 = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                        int V限时道具 = ByteDataAnalysis.GetInt(gd.Two, ref index);//可能是一个到期时间Unix
                        byte 镶金边 = ByteDataAnalysis.Getbyte(gd.Two, ref index); //tx.PushByte(t); //镶金边  0x01 镶金边 0x02不可交易
                        int v集中力 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        int v分散力 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        byte v封包格式 = ByteDataAnalysis.Getbyte(gd.Two, ref index); //f输出封包
                        if (v封包格式 == 4)
                        {
                            //太复杂了
                        }

                    }

                }



                string s6 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                byte b22 = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                int st7 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                string s7 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                string s8 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                string 武器中文名 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                string 技能1 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);
                //

                int st77 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int st8 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                int st9 = ByteDataAnalysis.GetInt(gd.Two, ref index);
                string s9 = ByteDataAnalysis.GetString(gd.Two, out UnparsedData, ref index);

                int st10 = ByteDataAnalysis.GetInt(gd.Two, ref index);


                int a = 1;
            }
            #endregion

            return rs;
        }


        /// <summary>
        /// 提取封中的文字部分
        /// </summary>
        /// <param name="gd"></param>
        /// <returns>返回文字列表和对应的byte[i]的i</returns>
        public static Dictionary<int, string> Try提取封包中文字部分(OriginalData gd)
        {
            Dictionary<int, string> rsList = new Dictionary<int, string>();
            string rs = string.Empty;
            if (gd.Two == null)
            {
                return new Dictionary<int, string>();
            }
            try
            {
                byte[] buffer = new byte[gd.Two.Length];
                Array.Copy(gd.Two, buffer, buffer.Length);
                bool success = false;
                int indexFalg = 0;
                int tempLen = 0;
                byte[] UnparsedData = new byte[gd.Two.Length - indexFalg];
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (i == 439)
                    {
                        //调试
                    }
                    indexFalg = i;
                    //如果后面总长度都不够 没有意义再取了。
                    if (gd.Two.Length - i < 4)
                    {
                        break;
                    }
                    int strLen = ByteDataAnalysis.Getint(buffer, out UnparsedData, ref indexFalg);
                    //如果得到一个首位数字，但是不可能大于总长度-4
                    if (strLen > gd.Two.Length - 4)
                    {
                        continue;
                    }
                    if (strLen < 0)
                    {
                        continue;
                    }
                    //如果是最后4位是个数字而已则退出
                    if (indexFalg == gd.Two.Length)
                    {
                        break;
                    }

                    tempLen = 0;
                    //字符长度实际不可能上千吧
                    if (strLen > 0 && strLen < gd.Two.Length - 4)
                    {
                        #region 判断后面长度

                        for (int l = indexFalg; l < strLen + indexFalg; l++)
                        {
                            //接下来的每个 暂时不能为0
                            int txtValue = buffer[l];
                            if (txtValue > 30)
                            {
                                success = true;
                                tempLen++;
                                if (tempLen == strLen - 1)//有一位是0为结束符
                                {
                                    if (rsList == null)
                                    {
                                        rsList = new Dictionary<int, string>();
                                    }
                                    //找到了。
                                    //byte[] sz = new byte[tempLen];
                                    //sz = buffer.Skip(indexFalg).Take(tempLen).ToArray();
                                    //rs = System.Text.Encoding.GetEncoding("GB2312").GetString(sz);
                                    indexFalg = indexFalg - 4;
                                    rs = ByteDataAnalysis.GetString(buffer, out UnparsedData, ref indexFalg);
                                    rsList.Add(i + 1, rs);
                                    i = indexFalg - 1;//因为外面循环会自动加1这里用少1的
                                    break;
                                }
                                continue;
                            }
                            else
                            {
                                success = false;
                                break;
                            }

                        }

                        #endregion

                    }
                    if (strLen == 0)
                    {
                        //跳四4位  只有字节都为0才为0
                        i += 3;//因为外面循环会自动加1这里用少1的
                    }
                }

            }
            catch (Exception ex)
            {
                rsList.Add(0, ex.Message);

            }
            return rsList;
        }


        /// <summary>
        /// 提取封中的文字部分
        /// </summary>
        /// <param name="gd"></param>
        /// <returns>返回文字列表和对应的byte[i]的i,以及这个文字的长度</returns>
        public static Dictionary<KeyValuePair<int, int>, string> Try提取封包中文字部分List(OriginalData gd)
        {
            Dictionary<KeyValuePair<int, int>, string> rsList = new Dictionary<KeyValuePair<int, int>, string>();
            string rs = string.Empty;
            if (gd.Two == null)
            {
                return new Dictionary<KeyValuePair<int, int>, string>();
            }
            try
            {
                byte[] buffer = new byte[gd.Two.Length];
                Array.Copy(gd.Two, buffer, buffer.Length);
                bool success = false;
                int indexFalg = 0;
                int tempLen = 0;
                byte[] UnparsedData = new byte[gd.Two.Length - indexFalg];
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (i == 439)
                    {
                        //调试
                    }
                    indexFalg = i;
                    //如果后面总长度都不够 没有意义再取了。
                    if (gd.Two.Length - i < 4)
                    {
                        break;
                    }
                    int strLen = ByteDataAnalysis.Getint(buffer, out UnparsedData, ref indexFalg);
                    //如果得到一个首位数字，但是不可能大于总长度-4
                    if (strLen > gd.Two.Length - 4)
                    {
                        continue;
                    }
                    if (strLen < 0)
                    {
                        continue;
                    }
                    //如果是最后4位是个数字而已则退出
                    if (indexFalg == gd.Two.Length)
                    {
                        break;
                    }

                    tempLen = 0;
                    //字符长度实际不可能上千吧
                    if (strLen > 0 && strLen < gd.Two.Length - 4)
                    {
                        #region 判断后面长度

                        for (int l = indexFalg; l < strLen + indexFalg; l++)
                        {
                            //接下来的每个 暂时不能为0
                            int txtValue = buffer[l];
                            if (txtValue > 30)
                            {
                                success = true;
                                tempLen++;
                                if (tempLen == strLen - 1)//有一位是0为结束符
                                {
                                    if (rsList == null)
                                    {
                                        rsList = new Dictionary<KeyValuePair<int, int>, string>();
                                    }
                                    //找到了。
                                    //byte[] sz = new byte[tempLen];
                                    //sz = buffer.Skip(indexFalg).Take(tempLen).ToArray();
                                    //rs = System.Text.Encoding.GetEncoding("GB2312").GetString(sz);
                                    indexFalg = indexFalg - 4;
                                    rs = ByteDataAnalysis.GetString(buffer, out UnparsedData, ref indexFalg);
                                    rsList.Add(new KeyValuePair<int, int>(i + 1, rs.Length + 1), rs);
                                    i = indexFalg - 1;//因为外面循环会自动加1这里用少1的
                                    break;
                                }
                                continue;
                            }
                            else
                            {
                                success = false;
                                break;
                            }

                        }

                        #endregion

                    }
                    if (strLen == 0)
                    {
                        //跳四4位  只有字节都为0才为0
                        i += 3;//因为外面循环会自动加1这里用少1的
                    }
                }

            }
            catch (Exception ex)
            {
                rsList.Add(new KeyValuePair<int, int>(0, -1), ex.Message);

            }
            return rsList;
        }
        public static string 解析客户端发来的心跳包(OriginalData data)
        {

            string rs = string.Empty;
            if (data.One.Length == 0)
            {
                return rs;
            }
            int index = 0;
            List<object> rslist = new List<object>();
            Int16 计数器 = ByteDataAnalysis.GetInt16(data.One, ref index);
            rslist.Add("计数器" + 计数器);
            int Ue4X = ByteDataAnalysis.GetInt(data.One, ref index);
            int _gameX = Ue4X / 100 + 26009;
            rslist.Add(Ue4X + "X(" + _gameX + ")");
            int Ue4Y = ByteDataAnalysis.GetInt(data.One, ref index);
            int _gameY = Ue4Y / 100 + 26009;
            rslist.Add(Ue4Y + "Y(" + _gameY + ")");

            int 必需为01 = ByteDataAnalysis.GetInt(data.One, ref index);
            rslist.Add("必需为0:" + 必需为01);
            //===========================
            index = 0;
            Int16 必需为1 = ByteDataAnalysis.GetInt16(data.Two, ref index);
            rslist.Add("必需为1:" + 必需为1);


            //z负得越多 越低 正数是高
            int Ue4Z = ByteDataAnalysis.GetInt(data.Two, ref index);
            int _gameZ = Ue4Z / 100 + 26009;
            rslist.Add(Ue4Z + "Z(" + _gameZ + ")");

            int 像时间 = ByteDataAnalysis.GetInt(data.Two, ref index);
            rslist.Add("像时间" + "time(" + 像时间 + ")");

            DateTime temp = Tool4DataProcess.GetTime(像时间.ToString());

            int l = ByteDataAnalysis.GetInt(data.Two, ref index);
            rslist.Add("l" + "l(" + l + ")");

            int a = ByteDataAnalysis.GetInt(data.Two, ref index);
            rslist.Add("a" + "a(" + a + ")");



            int b = ByteDataAnalysis.GetInt(data.Two, ref index);
            rslist.Add("b" + "b(" + b + ")");


            Int64 b64 = ByteDataAnalysis.GetInt64(data.Two, ref index);
            rslist.Add("b64" + "b64(" + b64 + ")");

            int c = ByteDataAnalysis.GetInt(data.Two, ref index);
            rslist.Add("float ？c" + "c(" + c + ")");

            int d = ByteDataAnalysis.GetInt(data.Two, ref index);
            rslist.Add("像X(" + d + ")");

            int e = ByteDataAnalysis.GetInt(data.Two, ref index);
            rslist.Add("像Y(" + e + ")");



            //z负得越多 越低 正数是高
            int Ue4Zz = ByteDataAnalysis.GetInt(data.Two, ref index);
            int _gameZz = Ue4Z / 100 + 26009;
            rslist.Add(Ue4Zz + "像Z(" + _gameZz + ")");

            byte Bb = ByteDataAnalysis.Getbyte(data.Two, ref index);



            int 倒数第二个 = ByteDataAnalysis.GetInt(data.Two, ref index);
            rslist.Add("倒数第二个(" + 倒数第二个 + ")");
            DateTime temp2 = Tool4DataProcess.GetTime(倒数第二个.ToString());


            long ss = Tool4DataProcess.ConvertDateTimeInt(DateTime.Now);

            rslist.Add("当前时间的时间搓(" + ss.ToString() + ")");


            //DateTime.Now获取的是电脑上的当前时间
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string 获取时间戳 = Convert.ToInt64(ts.TotalSeconds).ToString();//精确到秒
            rslist.Add("获取当前时间戳(" + 获取时间戳.ToString() + ")");


            float f = ByteDataAnalysis.GetFloat(data.Two, ref index);
            rslist.Add("f(" + f + ")");



            //=====================================
            var bw = new ByteBuff(data.Two);
            //old 2 4 4 4 4 4 4 
            //new 2 4  2  1 1   4 4 
            if (bw.GetUint16() != 0x01)
            {
                throw new Exception("必须是1");
            }

            var 位置Z = bw.GetInt();//z
            //rslist.Add(位置Z);
            var test = bw.GetUint16(); //这里有变动。对比原始优化了。
            //rslist.Add(test);

            var counterTow1 = bw.GetByte();
            //rslist.Add(counterTow1);
            var counterTow2 = bw.GetByte();
            //rslist.Add(counterTow2);

            var 未知2 = bw.GetInt(); //0
            //rslist.Add(未知2);
            var 方向 = bw.GetInt();  //4B 0E 00 00
            //rslist.Add(方向);
            if (bw.GetInt() != 0)
            {
                throw new Exception("必须是0 2");
            }
            bw.GetInt(); //跳的时候是0 不跳的时候是50 37 1A 10 是一个固定值
            bw.GetInt(); //跳的时候是0 不跳的时候是FFFFFFFF

            var 未知7 = bw.GetInt(); //04 F9 18 00
            //rslist.Add(未知7);
            var 方向1 = bw.GetInt(); //方向
            //rslist.Add(方向1);
            var 未知8 = bw.GetInt(); //36 A1 0C 00
            //rslist.Add(未知8);
            var 未知9 = bw.GetInt(); //CC E7 FF FF
            //rslist.Add(未知9);
            var 类型 = bw.GetByte();
            switch (类型)
            {
                case 0:
                    //bw.GetFloat();  // 54 B3 16 44
                    //if (bw.GetFloat() != 0x01)
                    //{
                    //    throw new Exception("必须是0x01");
                    //}
                    break;
                case 1:
                    bw.GetInt();
                    bw.GetInt();
                    bw.GetInt();
                    //bw.GetFloat();//00 00 16 44
                    break;
                default:
                    throw new Exception("");
            }
            /*
            if (PlayerSession._Player == null)
            {
                return;
            }*/
            //PlayerSession._Player.v位置.z = 位置Z;

            //                if ((newx != PlayerSession._Player.v位置.x) || (newy != PlayerSession._Player.v位置.y) || (类型 != 0))
            //                {
            //                    /*
            //                if(m.V内容.(*S角色列表).V名称 == "GM" {
            //                    坐标x, 坐标y:= F位置_UE4位置转游戏位置(m.X, m.Y)

            //                    if 坐标x < 坐标.X0 {
            //                        坐标.X0 = 坐标x

            //                    }
            //                    if 坐标y < 坐标.Y0 {
            //                        坐标.Y0 = 坐标y

            //                    }
            //                    if 坐标x > 坐标.X1 {
            //                        坐标.X1 = 坐标x

            //                    }
            //                    if 坐标y > 坐标.Y1 {
            //                        坐标.Y1 = 坐标y

            //                    }
            //                }
            //                if m.V空间.F计算两点距离(m.X, m.Y, newx, newy) < 10 { //如果两次间距离过长，很可能角色移动了，但客户端还没反应过来
            //                    m.F内容移动(newx, newy, 位置Z, false) //通知地图 移动位置

            //                    newx = m.X
            //                    newy = m.Y
            //                    //通知其他角色人物移动
            //tx:= CreateBuff(100)

            //                    tx.AddInt32(m.Id) //添加自己的ID
            //                    tx.AddInt32(m.X)

            //                    tx.AddInt32(m.Y)
            //                    tx.AddInt32(0)
            //                    //开始添加后面的数据，需要原封不动
            //tx.PushInt16(0x01)

            //                    tx.AddInt32(位置Z)
            //                    tx.AddInt32(未知1)

            //                    tx.AddInt32(未知2)
            //                    tx.AddInt32(方向)

            //                    tx.AddInt32(0)

            //                    if 类型 == 0 {
            //                        tx.AddInt32(0x101A3750)

            //                        tx.AddInt32(-1)

            //                    }
            //                    else
            //                    {
            //                        tx.AddInt32(0x00)

            //                        tx.AddInt32(0x00)

            //                    }
            //                    tx.AddInt32(未知7)
            //                    tx.AddInt32(方向1)

            //                    tx.AddInt32(未知8)
            //                    tx.AddInt32(未知9)

            //                    tx.AddByte(类型)

            //                    switch 类型 {
            //                        case 0:
            //                            tx.PushFloat(600)

            //                        tx.PushFloat(0x01)

            //                    case 1:
            //                            tx.AddInt32(0x00)

            //                        tx.AddInt32(0x00)

            //                        tx.AddInt32(0x00)

            //                        tx.PushFloat(600)

            //                    }
            //                    m.向周边发送消息(0x0E, 16, tx.Buff)

            //                } */
            //                }
            //              /*  {*/


            foreach (var item in rslist)
            {
                rs += item.ToString() + " |\r\n ";
            }

            return rs;

        }
        public static string 反解析服务器的心跳包(OriginalData gd)
        {
            string rs = string.Empty;
            byte[] source = gd.One;
            try
            {
                /*
                 tx.PushInt16(累加数); //累加数
                tx.PushInt(base.v位置.x);
                tx.PushInt(base.v位置.y);
                tx.PushInt(0);
                tx.PushInt(0);
                var d = new KxSocket.KxData();
                d.cmd = 0x0D;
                d.One = tx.toByte();
                d.Two = new byte[0];
                 */
                /*
                int counterx1 = 0;
                byte[] counterBytex1 = new byte[2];
                counterBytex1 = source.Skip(0).Take(2).ToArray();
                counterx1 = BitConverter.ToUInt16(counterBytex1, 0);
                */
                //one   2,4,4,4,
                UInt16 counter = 0;
                byte[] counterByte = new byte[2];
                counterByte = source.Skip(0).Take(2).ToArray();
                counter = BitConverter.ToUInt16(counterByte, 0);



                int x = 0;
                byte[] xByte = new byte[0];
                xByte = source.Skip(2).Take(4).ToArray();
                x = BitConverter.ToUInt16(xByte, 0);


                //b = a - ((a <= 32767) ? 0 : 65536);


                int y = 0;
                byte[] yByte = new byte[0];
                yByte = source.Skip(6).Take(4).ToArray();
                y = BitConverter.ToUInt16(yByte, 0);

                int z = 0;
                byte[] zByte = new byte[0];
                zByte = source.Skip(10).Take(4).ToArray();
                z = BitConverter.ToUInt16(zByte, 0);


                rs += "counter:" + counter;
                rs += "x:" + x;
                rs += "y:" + y;
                rs += "z:" + z;
            }
            catch (Exception ex)
            {
                rs = ex.Message;

            }
            return rs;
        }
        public static string 反解析服务器名称(OriginalData gd)
        {
            string rs = string.Empty;
            byte[] source = gd.Two;
            try
            {
                //tx.PushInt16(0x00E1);//2位
                byte[] a = new byte[2];
                Array.Copy(source, a, 2);
                Int16 a16;
                a16 = BitConverter.ToInt16(a, 0);
                // tx.PushByte(0);//1位 byte直接减少
                //tx.PushInt(0);//占4位 0000
                //tx.PushString(名称);//12位开始 名称.Length + 1
                //tx.PushString(""); //4位len,加结束符0=5
                //上面有7位，
                byte[] strbyte = new byte[source.Length - 7 - 5];
                Array.Copy(source, 7, strbyte, 0, strbyte.Length);
                rs = TransPackProcess.PopAnalysisString(strbyte);
            }
            catch (Exception ex)
            {
                rs = ex.Message;

            }
            return rs;
        }
        public static string 反解析角色位置定义(OriginalData gd)
        {
            string rs = string.Empty;
            byte[] source = gd.One;
            try
            {
                /*
                var tx = new ByteBuff(18);
                tx.PushBytes4String("25C203000100");
                tx.PushInt(adduress.x);
                tx.PushInt(adduress.y);
                tx.PushInt(0x00);
                pp.WriteClientData(0x07, tx.toByte(), null); //发送错误消息
                */
                byte[] strbyte = new byte[source.Length - 12];
                Array.Copy(source, 0, strbyte, 0, strbyte.Length);
                rs = TransPackProcess.PopAnalysisString(strbyte);


                byte[] x = new byte[4];
                x = source.Skip(6).Take(4).ToArray();
                int xx = BitConverter.ToInt32(x, 0);

                byte[] y = new byte[4];
                y = source.Skip(10).Take(4).ToArray();
                int yy = BitConverter.ToInt32(y, 0);

                byte[] z = new byte[4];
                z = source.Skip(14).Take(4).ToArray();
                int zz = BitConverter.ToInt32(z, 0);


            }
            catch (Exception ex)
            {
                rs = ex.Message;

            }
            return rs;
        }


        public static string 反解析服务端显示NPC(OriginalData gd)
        {
            string rs = string.Empty;
            //one 部分有数据感觉是一个对象ID，因为转发无效，没有one的就有效果？
            byte[] buffer;
            byte[] source = gd.Two;
            try
            {
                int GameId = 0;
                buffer = new byte[4];
                buffer = gd.One.Skip(0).Take(4).ToArray();
                GameId = BitConverter.ToInt32(buffer, 0);
                rs += "GameId:" + GameId + "|";

                UInt32 x = 0;
                buffer = new byte[4];
                buffer = gd.One.Skip(4).Take(4).ToArray();
                x = BitConverter.ToUInt32(buffer, 0);
                int x1 = BitConverter.ToInt32(buffer, 0);
                //rs += "x:" + x + "|";

                int y = 0;
                buffer = new byte[4];
                buffer = gd.One.Skip(12).Take(4).ToArray();
                y = BitConverter.ToInt32(buffer, 0);
                //rs += "y:" + y + "|";

                //======================
                //two 2 2 4(消息长度)
                byte[] a = new byte[2];
                a = source.Skip(0).Take(2).ToArray();
                Int16 a16;
                a16 = BitConverter.ToInt16(a, 0);

                byte[] b = new byte[2];
                b = source.Skip(2).Take(4).ToArray();
                Int16 b16;
                b16 = BitConverter.ToInt16(b, 0);

                int msglen = 0;
                byte[] msglenByte = new byte[4];
                msglenByte = source.Skip(4).Take(4).ToArray();
                msglen = BitConverter.ToInt32(msglenByte, 0);

                byte[] msg = new byte[msglen];
                //msg.Length-1减1是去掉结束符
                msg = source.Skip(8).Take(msglen - 1).ToArray();
                //rs += System.Text.Encoding.Default.GetString(msg);

                msglenByte = new byte[4];
                int passlen = msglen + 8;
                msglenByte = source.Skip(passlen).Take(4).ToArray();
                msglen = BitConverter.ToInt32(msglenByte, 0);

                msg = new byte[msglen];
                //msg.Length-1减1是去掉结束符
                msg = source.Skip(passlen + 4).Take(msg.Length - 1).ToArray();
                rs += System.Text.Encoding.GetEncoding("GB2312").GetString(msg);



            }
            catch (Exception ex)
            {
                rs = ex.Message;

            }
            return rs;
        }

        public static List<string> 反解析服务端显示角色列表(OriginalData gd)
        {
            List<string> RoleList = new List<string>();
            //one 部分有数据感觉是一个对象ID，因为转发无效，没有one的就有效果？
            byte[] buffer = gd.Two;

            try
            {
                int index = 0;
                int cmd = ByteDataAnalysis.GetInt16(buffer, ref index);
                byte counterForRole = ByteDataAnalysis.Getbyte(buffer, ref index);
                for (int i = 0; i < (int)counterForRole; i++)
                {
                    string roleName = ByteDataAnalysis.GetString(buffer, ref index);
                    string str种族 = ByteDataAnalysis.GetString(buffer, ref index);
                    int sex = ByteDataAnalysis.GetInt(buffer, ref index);
                    string str职业 = ByteDataAnalysis.GetString(buffer, ref index);
                    int xx = ByteDataAnalysis.GetInt(buffer, ref index);
                    int time = ByteDataAnalysis.GetInt(buffer, ref index);

                    //...还有
                    RoleList.Add(roleName);
                }
                // rs += System.Text.Encoding.GetEncoding("GB2312").GetString(msg);

            }
            catch (Exception ex)
            {
                // rs = ex.Message;
            }
            return RoleList;
        }

        public static string 反解析服务端角色显示状态(OriginalData gd)
        {
            string rs = string.Empty;
            //one 部分有数据感觉是一个对象ID，因为转发无效，没有one的就有效果？
            byte[] source = gd.Two;
            try
            {
                //前面17位是固定的？
                // 1 1  4(消息长度)
                byte[] a = new byte[2];
                Array.Copy(source, a, 2);
                Int16 a16;
                a16 = BitConverter.ToInt16(a, 0);

                int msglen = 0;
                byte[] msglenByte = new byte[4];
                msglenByte = source.Skip(2).Take(4).ToArray();
                msglen = BitConverter.ToInt32(msglenByte, 0);
                byte[] msg = new byte[source.Length - 6];
                //msg.Length-1减1是去掉结束符
                msg = source.Skip(6).Take(msglen - 1).ToArray();
                rs += System.Text.Encoding.Default.GetString(msg);
            }
            catch (Exception ex)
            {
                rs = ex.Message;

            }
            return rs;
        }

        public static string 反解析服务端回应登记位置(OriginalData gd)
        {
            string rs = string.Empty;
            //one 部分有数据感觉是一个对象ID，因为转发无效，没有one的就有效果？
            //2 4（计数)? 4 (长+坐标文字长0)，4（点名长)=存名
            //先反向分析出确定的值
            byte[] source = gd.Two;
            try
            {
                byte[] a = new byte[2];
                Array.Copy(source, a, 2);
                Int16 a16;
                a16 = BitConverter.ToInt16(a, 0);

                byte[] x = new byte[4];
                x = source.Skip(2).Take(4).ToArray();
                int xweizhi = BitConverter.ToInt32(x, 0);


                byte[] msglenByte = new byte[4];
                msglenByte = source.Skip(6).Take(4).ToArray();
                int 坐标文字长 = BitConverter.ToInt32(msglenByte, 0);


                byte[] msg = new byte[坐标文字长];
                //msg.Length-1减1是去掉结束符
                msg = source.Skip(10).Take(坐标文字长 - 1).ToArray();
                rs += System.Text.Encoding.Default.GetString(msg);
                //rs = GamePackProcess.PopAnalysisString(msg);

                byte[] 保存点名称长lenByte = new byte[4];
                保存点名称长lenByte = source.Skip(10 + msg.Length + 1).Take(4).ToArray();
                int 保存点名称长 = BitConverter.ToInt32(保存点名称长lenByte, 0);

                byte[] 保存点名称Byte = new byte[保存点名称长];
                //msg.Length-1减1是去掉结束符
                保存点名称Byte = source.Skip(14 + msg.Length + 1).Take(保存点名称长 - 1).ToArray();
                rs += System.Text.Encoding.Default.GetString(保存点名称Byte);
                //rs = GamePackProcess.PopAnalysisString(msg);

            }
            catch (Exception ex)
            {
                rs = ex.Message;

            }
            return rs;
        }

        public static string 反解析服务端语言指令(OriginalData gd)
        {
            string rs = string.Empty;
            //one -》base.GameId 一个对象ID，所以转发无效，修改为正确ID就能有效果
            byte[] source = gd.Two;
            try
            {
                //前面17位是固定的？
                // 2 1 1(类型) 4 4 1  4(消息长度)
                byte[] a = new byte[2];
                Array.Copy(source, a, 2);
                Int16 a16;
                a16 = BitConverter.ToInt16(a, 0);

                byte[] model = new byte[1];
                model = source.Skip(3).Take(1).ToArray();
                byte mtype = model[0];
                MessageType mt = new MessageType();
                mt = (MessageType)mtype;
                //1是说话，2，是呼喊  3是黄色警告，150 是提示右下角灰色文字
                rs += mt.ToString() + "|";
                int msglen = 0;
                byte[] msglenByte = new byte[4];
                msglenByte = source.Skip(13).Take(4).ToArray();
                msglen = BitConverter.ToInt32(msglenByte, 0);

                byte[] msg = new byte[source.Length - 17];
                //msg.Length-1减1是去掉结束符
                msg = source.Skip(17).Take(msg.Length - 1).ToArray();
                rs += System.Text.Encoding.Default.GetString(msg);
                //rs = GamePackProcess.PopAnalysisString(msg);
            }
            catch (Exception ex)
            {
                rs = ex.Message;

            }
            return rs;
        }

        public static string 解析客户端语言指令(OriginalData gd)
        {
            string rs = string.Empty;
            byte[] source = gd.Two;
            try
            {

                //(2 +1+ 4 +  msg.Length);
                int index = 0;
                Int16 cmd = ByteDataAnalysis.GetInt16(source, ref index);

                int mtype = ByteDataAnalysis.Getbyte(source, ref index);
                MessageType mt = new MessageType();
                mt = (MessageType)mtype;
                //1是说话，2，是呼喊  3是黄色警告，150 是提示右下角灰色文字
                rs += mt.ToString() + "|";

                string msg = ByteDataAnalysis.GetString(gd.Two, ref index);
                rs += msg;

            }
            catch (Exception ex)
            {
                rs = ex.Message;

            }
            return rs;
        }
        public static string 反解析地图_显示内容(OriginalData gd)
        {
            string rs = string.Empty;
            if (gd.One == null)
            {
                return rs;
            }
            ByteBuff tx = new ByteBuff(400);
            // SNPC npc = 内容 as SNPC;
            //4*4=16?
            // tx.PushInt(npc.GameId);
            // tx.PushInt(npc.X);
            // tx.PushInt(npc.Y);
            // tx.PushFloat(float.Parse(npc.v方向));

            //two 部分
            //tx.PushInt16(0x00B3);//2位
            //tx.PushInt16(0x0001);
            //tx.PushString(npc.v外观);  //添加样式长度
            //  tx.PushString(npc.v中文名); //添加名称长度
            //  tx.PushString("0000000001000000000000803F"); //添加最后几位未知数据
            //  tx.PushString(npc.v外观);
            //two 2 2 4(消息长度)
            int index = 0;
            Int16 subCmd = ByteDataAnalysis.GetInt16(gd.Two, ref index);
            switch (subCmd)
            {
                case 0xB0:
                    //人物
                    byte r = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                    string rw = ByteDataAnalysis.GetString(gd.Two, ref index);
                    rs += rw;
                    break;
                case 0x00B1:
                    byte g = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                    string gaiwu = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string gaiwuz = ByteDataAnalysis.GetString(gd.Two, ref index);
                    rs += gaiwuz + gaiwu;
                    //怪物
                    break;
                case 0x00B2:
                    // 未知
                    break;
                case 0x00B3:
                    byte n = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                    string npc = ByteDataAnalysis.GetString(gd.Two, ref index);
                    rs += npc;
                    //npc
                    break;
                case 0x00B4:
                    string wp = ByteDataAnalysis.GetString(gd.Two, ref index);
                    rs += wp;
                    //物品
                    break;

                default:
                    break;
            }

            //(0x11, bone, btwo);
            // rt.AddHeadSend10(0x11, [][]byte{ tx.Buff[:16], tx.Buff[16:]})
            //https://blog.csdn.net/qq_33679504/article/details/102372356
            //tx.Buff[:16] 取0到15位的意思 
            //tx.Buff[16:]  表示取16到最后一位
            return rs;
        }
        public static string 反解析发送凯旋时间(OriginalData gd)
        {
            string rs = string.Empty;
            byte[] source = gd.Two;
            try
            {
                //tx.PushInt16(0xBB);//2位
                byte[] a = new byte[2];
                Array.Copy(source, a, 2);
                Int16 a16;
                a16 = BitConverter.ToInt16(a, 0);

                byte[] timebyte = new byte[4];
                timebyte = source.Skip(2).Take(4).ToArray();
                Int32 timeStamp = BitConverter.ToInt32(timebyte, 0);
                //这里解析在游戏端还有一个算法 。目前不需要处理。只要知道传入规则
                DateTime dtStart = new DateTime(1, 1, 1); //得到1年1月1日的时间戳
                DateTime dtGameTime = dtStart.AddHours(timeStamp);
                rs = dtGameTime.ToString();
            }
            catch (Exception ex)
            {
                rs = ex.Message;
            }
            return rs;
        }



    }
}
