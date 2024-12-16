using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.UI.ATechnologyStack
{
  public  interface  ICustomValidationRule
    {
        bool Validate(string input);
    }

    // 2. 实现具体的验证规则类
    public class EmailValidationRule : ICustomValidationRule
    {
        public bool Validate(string input)
        {
            // 使用正则表达式验证邮箱格式
            return Regex.IsMatch(input, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
    }

    public class AmountValidationRule : ICustomValidationRule
    {
        public bool Validate(string input)
        {
            // 使用正则表达式验证金额格式
            return Regex.IsMatch(input, @"^\d+(\.\d{1,2})?$");
        }
    }


}
