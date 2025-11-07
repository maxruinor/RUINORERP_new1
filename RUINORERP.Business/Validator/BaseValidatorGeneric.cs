using FluentValidation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.ConfigModel;
using SharpYaml.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public abstract class BaseValidatorGeneric<T> : AbstractValidator<T> where T : class
    {


        // public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;

        //protected BaseValidatorGeneric(IOptionsMonitor<GlobalValidatorConfig> config)
        //{
        // ValidatorConfig = config;
        // 监听配置变化
        // ValidatorConfig.OnChange(updatedConfig =>
        //{
        //    Console.WriteLine($"Configuration has changed: {updatedConfig.SomeSetting}");
        //});
        //}

        protected BaseValidatorGeneric()
        {

        }
        //public abstract void Initialize();

        public virtual void Initialize()
        {
        }


        public bool BeUniqueName(string FiledName, string FieldValue)
        {
            // 在此处实现检查客户名称是否唯一的逻辑
            // 例如，查询数据库或其他数据存储来验证
            // 这里只是一个简单的示例，假设您有一个方法来检查唯一性
            bool isUnique = YourUniqueCheckMethod(FiledName, FieldValue);
            return isUnique;
        }

        private bool YourUniqueCheckMethod(string FiledName, string FieldValue)
        {
            string tableName = typeof(T).Name;
            return true;
        }



        public bool IsValidEmailFormat(string email)
        {
            // 正则表达式，匹配邮箱格式
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        public bool IsValidPhoneOrLandlineFormat(string phone)
        {
            // 正则表达式，匹配11位或12位手机号码
            string mobilePattern = @"^1\d{10,11}$"; // 手机号码
            string landlinePattern = @"^(\d{3,4}-)?\d{7,8}$"; // 座机号码
            return Regex.IsMatch(phone, mobilePattern) || Regex.IsMatch(phone, landlinePattern);
        }



        // 按UTF8字节数计算（中文通常占3字节，英文占1字节）
        public static int CalculateByteLength(string value)
        {
            return Encoding.UTF8.GetByteCount(value);
        }
 

        /// <summary>
        /// 计算混合长度的核心方法
        /// </summary>
        public static int CalculateMixedLength(string value)
        {
            int length = 0;
            foreach (char c in value)
            {
                // 判断是否为中文字符（Unicode 范围）
                if (c >= 0x4E00 && c <= 0x9FA5)
                {
                    length += 2; // 中文字符算2个长度
                }
                else
                {
                    length += 1; // 其他字符算1个长度
                }
            }
            return length;
        }
    }
}
