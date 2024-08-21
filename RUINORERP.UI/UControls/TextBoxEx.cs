using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UControls
{
    [System.Drawing.ToolboxBitmap(typeof(TextBox))]
    public class MyTextBox : TextBox
    {
        public MyTextBox() : base() { }
        //类型枚举
        public enum RegexType
        {
            Custom,         //正常
            Number,         //数字 整数或者小数
            CNString,       //汉字
            Zip,            //邮政编码
            Email,          //电子邮箱
            Phone,          //手机号码
            Integer,        //整数
            NInteger,       //负整数
            Float,          //浮点数
            ENChar,         //英文字符
            NumChar,        //数字和英文字母
            NumLineChar,    //数字、英文字母或下划线
            Url,
            QQ,
            DCard,          //身份证
            IP,
            DateTime,       //日期时间
            Date,           //日期
            Year,
            Month,
            Day,
            Time,
        }

        #region 私有属性

        /// 文本框类型
        private RegexType _controlType;
        //显示的名称
        private string _controlTypeText = "默认";
        //是否可空 默认为可空
        private bool _isNULL = true;
        //验证是否通过
        private bool _isPass = false;
        #endregion

        #region Properties 属性栏添加的属性

        [DefaultValue(RegexType.Custom), Description("文本框类型")]
        public RegexType ControlType
        {
            get { return _controlType; }
            set
            {
                _controlType = value;
                //对应显示的文字
                this.ShowDescription(value);
                //重新绘制控件
                base.Invalidate();
            }
        }

        [DefaultValue("默认"), Description("控件验证描述")]
        public string ControlTypeText
        {
            get { return _controlTypeText; }
        }

        [DefaultValue(typeof(bool), "True"), Description("内容是否可空")]
        public bool IsNULL
        {
            get { return _isNULL; }
            set { _isNULL = value; base.Invalidate(); }
        }
        [DefaultValue(typeof(bool), "False"), Description("填写的内容格式是否正确，只读")]
        public bool IsPass
        {
            get { return _isPass; }
        }
        #endregion
        //判断验证类型
        private void Testing(RegexType value, string text)
        {
            //可空 验证字符串为空
            if (_isNULL && string.IsNullOrEmpty(text.Trim()))
            {
                _isPass = true;
                return;
            }
            //不能为空 验证字符串为空
            if (!_isNULL && string.IsNullOrEmpty(text))
            {
                _isPass = false;
                return;
            }
            //其他的两种情况都需要正则验证
            switch (value)
            {
                case RegexType.Custom:
                    _isPass = true;
                    break;
                case RegexType.Number:
                    _isPass = Proving(text, @"^-?[0-9]+\.{0,1}[0-9]*$");
                    break;
                case RegexType.CNString:
                    _isPass = Proving(text, @"^[\u4e00-\u9fa5]*$");
                    break;
                case RegexType.Zip:
                    _isPass = Proving(text, @"^[1-9]\d{5}$");
                    break;
                case RegexType.Email:
                    _isPass = Proving(text, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                    break;
                case RegexType.Phone:
                    _isPass = Proving(text, @"^1[2-8]{2}\d{8}$");
                    break;
                case RegexType.Integer:
                    _isPass = Proving(text, @"^-?[1-9]\d*$");
                    break;
                case RegexType.NInteger:
                    _isPass = Proving(text, @"^-[1-9]\d*$");
                    break;
                case RegexType.Float:
                    _isPass = Proving(text, @"^(-?\d+)(\.\d+)?$");
                    break;
                case RegexType.ENChar:
                    _isPass = Proving(text, @"^[A-Za-z]+$");
                    break;
                case RegexType.NumChar:
                    _isPass = Proving(text, @"^[A-Za-z0-9]+$");
                    break;
                case RegexType.NumLineChar:
                    _isPass = Proving(text, @"^[A-Za-z0-9_]+$");
                    break;
                case RegexType.Url:
                    _isPass = Proving(text, @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$");
                    break;
                case RegexType.QQ:
                    _isPass = Proving(text, @"^[1-9][0-9]{4,}$");
                    break;
                case RegexType.DCard:
                    _isPass = Proving(text, @"^((1[1-5])|(2[1-3])|(3[1-7])|(4[1-6])|(5[0-4])|(6[1-5])|71|(8[12])|91)\d{4}((19\d{2}(0[13-9]|1[012])(0[1-9]|[12]\d|30))|(19\d{2}(0[13578]|1[02])31)|(19\d{2}02(0[1-9]|1\d|2[0-8]))|(19([13579][26]|[2468][048]|0[48])0229))\d{3}(\d|X|x)?$");
                    break;
                case RegexType.IP:
                    _isPass = Proving(text, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
                    break;
                case RegexType.DateTime:
                    _isPass = Proving(text, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
                    break;
                case RegexType.Date:
                    _isPass = Proving(text, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
                    break;
                case RegexType.Year:
                    _isPass = Proving(text, @"^[1-9]\d{3}$");
                    break;
                case RegexType.Month:
                    _isPass = Proving(text, @"^(0?[123456789]|1[012])$");
                    break;
                case RegexType.Day:
                    _isPass = Proving(text, @"^(0?[1-9]|[12]\d|3[01])$");
                    break;
                case RegexType.Time:
                    _isPass = Proving(text, @"^(20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
                    break;
                default:
                    break;
            }

        }
        //格式是否正确
        private bool Proving(string str, string regexStr)
        {
            Regex regex;
            try
            {
                regex = new Regex(regexStr);
            }
            catch
            {
                return false;
            }
            return regex.IsMatch(str);
        }
        //重写了文本框的失去焦点事件
        protected override void OnLeave(EventArgs e)
        {
            Testing(this.ControlType, this.Text);
            base.OnLeave(e);
        }
        //验证类型的改变 对应改变显示的汉字
        private void ShowDescription(RegexType value)
        {
            switch (value)
            {
                case RegexType.Custom:
                    this._controlTypeText = "默认";
                    break;
                case RegexType.Number:
                    this._controlTypeText = "数字";
                    break;
                case RegexType.CNString:
                    this._controlTypeText = "汉字";
                    break;
                case RegexType.Zip:
                    this._controlTypeText = "邮政编码";
                    break;
                case RegexType.Email:
                    this._controlTypeText = "电子邮件";
                    break;
                case RegexType.Phone:
                    this._controlTypeText = "手机号";
                    break;
                case RegexType.Integer:
                    this._controlTypeText = "整数";
                    break;
                case RegexType.NInteger:
                    this._controlTypeText = "负整数";
                    break;
                case RegexType.Float:
                    this._controlTypeText = "浮点数";
                    break;
                case RegexType.ENChar:
                    this._controlTypeText = "英文字符";
                    break;
                case RegexType.NumChar:
                    this._controlTypeText = "数字和英文字母";
                    break;
                case RegexType.NumLineChar:
                    this._controlTypeText = "数字、英文字母或下划线";
                    break;
                case RegexType.Url:
                    this._controlTypeText = "URL";
                    break;
                case RegexType.QQ:
                    this._controlTypeText = "QQ";
                    break;
                case RegexType.DCard:
                    this._controlTypeText = "身份证";
                    break;
                case RegexType.IP:
                    this._controlTypeText = "IP";
                    break;
                case RegexType.DateTime:
                    this._controlTypeText = "年-月-日 时:分:秒";
                    break;
                case RegexType.Date:
                    this._controlTypeText = "年-月-日";
                    break;
                case RegexType.Year:
                    this._controlTypeText = "年份";
                    break;
                case RegexType.Month:
                    this._controlTypeText = "月份";
                    break;
                case RegexType.Day:
                    this._controlTypeText = "日期";
                    break;
                case RegexType.Time:
                    this._controlTypeText = "时:分:秒";
                    break;
                default:
                    break;
            }
        }
    }
}
