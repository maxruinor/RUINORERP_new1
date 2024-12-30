using HLH.Lib.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityCore
{
    /// <summary>
    /// 数据加密类,为了防止该类被派生,,在声明中使用了sealed关键字
    /// </summary>
    public sealed class GetlicenseCode
    {
        public static string GetCode(string input,string key)
        {
            CreateRegisterCode code = new CreateRegisterCode();
            return code.transform(input, key);
        }
    }
}
