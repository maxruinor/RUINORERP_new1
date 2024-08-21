using System;
using System.ComponentModel;

namespace TransInstruction
{
        /// <summary>
        /// 通过子指令的描述来对应
        /// </summary>

        public enum ServerMainCmd : byte
        {
            特殊3 = 0x3,
            发送角色信息4 = 0x4,
            角色位置定义7 = 0x7,
            设置方面9 = 0x9,
            [Description("主动角色方面")]
            主动10 = 0xA,//10
            地图11 = 0xB,//11  地图？
            回应客户端心跳包13 = 0xD,//13
            其他人物心跳14 = 0xE,//14
            消失16 = 0x10,//16
            显示方面17 = 0x11//17
        }


        /// <summary>
        /// 0x9001D 
        /// </summary>
        [Description("用户相关")]
        public enum ISSubMsgForUser
        {
            设置物理技能快捷栏 = 0x1D,
            设置宠物名称 = 0x3E
        }

        /// <summary>
        /// 0xA00DC
        /// </summary>
        [Description("主动10")]
        public enum ISSubMsg主动10 : Int16
        {
            NPC对话框选项 = 0xA0,
            创建队伍 = 0xA1,
            踢出玩家组队 = 0xA2,
            玩家申请入队 = 0xA3,
            玩家邀请入队 = 0xA6,
            同意加入组队1 = 0xA9,
            同意加入组队2 = 0xAD,
            组队说话 = 0xAE,
            同意加入组队3 = 0xAF,
      
            准备摆摊 = 0x191,
            查看摆位 = 0x1A0,
        }


        public enum ISSubMsg
        {
            未知指令 = 0,
            用户连接上发的数据 = 0x30001,
            发送角色信息 = 0x40096,
        }
    }



