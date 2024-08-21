using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.DataModel
{
    /// <summary>
    /// 意思这里是服务器上所有登陆的用户
    /// 可以发送离线消息？
    /// </summary>
    public class OnlineUserInfo
    {
        private bool online = false;
        private string sessionId;
        private int userID;
        private string userName;
        private string empName;

        /// <summary>
        /// 服务器是否授权通过
        /// </summary>
        private bool _ServerAuthentication = false;
        public string SessionId { get => sessionId; set => sessionId = value; }
        public int UserID { get => userID; set => userID = value; }
        public string UserName { get => userName; set => userName = value; }
        public string EmpName { get => empName; set => empName = value; }

        public bool Online { get => online; set => online = value; }


        public bool ServerAuthentication { get => _ServerAuthentication; set => _ServerAuthentication = value; }


        public ClientInformation ClientInfo { get; set; }=new ClientInformation();

        public override string ToString()
        {
            return EmpName;
        }
    }

}
