using System;
using System.Collections.Generic;
using System.Text;

namespace RUINOR.WinFormsUI.RegTextBox
{
    public class RegularAuthenticationSettings
    {
        /// <summary>
        /// 正则描述
        /// </summary>
        public string RegDescription { get; set; }


        /// <summary>
        /// 正则表达式
        /// </summary>
        public string Regularly { get; set; }

        /// <summary>
        /// 验证不过时消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 如果设置为不能空时，相应的提示信息
        /// </summary>
        public string EmptyPopMessage { get; set; }

        public RegularAuthenticationSettings()
        {

        }

        public RegularAuthenticationSettings(string reg, string regDesc, string errorMsg, string emptymessage)
        {
            Regularly = reg;
            RegDescription = regDesc;
            ErrorMessage = errorMsg;
            EmptyPopMessage = emptymessage;
        }
        public RegularAuthenticationSettings(string reg, string regDesc)
        {
            Regularly = reg;
            RegDescription = regDesc;
            ErrorMessage = string.Format("请输入正确的{0}格式数据", regDesc);
            EmptyPopMessage = "不能为空";
        }


        private List<RegularAuthenticationSettings> regList = new List<RegularAuthenticationSettings>();



        public List<RegularAuthenticationSettings> CreateRegList()
        {

            regList.Add(new RegularAuthenticationSettings(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "Email地址"));
            regList.Add(new RegularAuthenticationSettings(@"^[\u4e00-\u9fa5]{0,}$", "汉字"));
            regList.Add(new RegularAuthenticationSettings(@"^[0-9]*$", "数字"));
            regList.Add(new RegularAuthenticationSettings(@"^[1-9]\d*$", "非零的正整数"));
            regList.Add(new RegularAuthenticationSettings(@"^(\-|\+)?\d+(\.\d+)?$", "正数、负数、和小数"));
            regList.Add(new RegularAuthenticationSettings(@"^([1-9]\d{0,9}|0)([.]?|(\.\d{1,2})?)$", "金额"));
            regList.Add(new RegularAuthenticationSettings(@"^[0-9]*$", "数字"));
            regList.Add(new RegularAuthenticationSettings(@"^[0-9]*$", "数字"));
            regList.Add(new RegularAuthenticationSettings(@"^[0-9]*$", "数字"));
            return regList;
        }



        public override string ToString()
        {
            return string.Format("选择格式[{0}]", this.RegDescription);
        }
    }
}
