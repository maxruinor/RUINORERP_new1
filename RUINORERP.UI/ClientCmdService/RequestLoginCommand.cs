﻿using Newtonsoft.Json;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 工作流提醒相关的请求
    /// </summary>
    public class RequestLoginCommand : IClientCommand
    {
        public LoginProcessType requestType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        //public void Execute(object parameters = null)
        //{
        //    ExecuteAsync(CancellationToken.None, parameters).GetAwaiter().GetResult();
        //}

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters = null)
        {

            await Task.Run(
               () =>
            {
                #region 执行方法
                MainForm.Instance.ecs.AddSendData(BuildDataPacket(null));
                #endregion
            }
               ,
            cancellationToken
               );


        }


        public OriginalData BuildDataPacket(object request = null)
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(50);
                string json = JsonConvert.SerializeObject(request,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
               });
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString(Username);
                tx.PushString(Password);
                tx.PushInt((int)requestType);
                //将来再加上提醒配置规则,或加在请求实体中
                gd.cmd = (byte)ClientCmdEnum.复合型登陆请求;
                gd.One = new byte[] { (byte)requestType };
                gd.Two = tx.toByte();
            }
            catch (Exception)
            {

            }
            return gd;
        }

        public bool AnalyzeDataPacket(OriginalData gd)
        {
            throw new NotImplementedException();
        }
    }



}