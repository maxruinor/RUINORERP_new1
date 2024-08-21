using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    /// <summary>
    /// 包含了所有的自定字段属性
    /// </summary>
    public class CustomLogContent
    {
        public CustomLogContent()
        {

        }
        public CustomLogContent(string macAddress, string computerName, string actionsclick, string description)
        {
            UserIP = macAddress;
            UserName = computerName;
            ActionsClick = actionsclick;
            Message = description;
        }

        /// <summary>
        /// 访问IP
        /// </summary>
        public string UserIP { get; set; }

        /// <summary>
        /// 系统登陆用户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 动作事件
        /// </summary>
        public string ActionsClick { get; set; }

        /// <summary>
        /// 日志描述信息
        /// </summary>
        public string Message { get; set; }


    }


    /// <summary>
    /// 当前用户的行为信息
    /// </summary>
    public class UserActionInfo
    {

    }
}
