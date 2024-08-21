
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;

namespace WinLib.RegTextBox
{
    /// <summary>
    /// 通过正则式来验证各种数据格式
    /// </summary>
    public class MyRegTextBox : TextBox, ITzhtecControl
    {
        #region ITzhtecControl 成员
        private ButtonBase btn;
        ////// 获取或设置验证控件的按钮
        [Description("获取或设置验证控件的按钮"), Category("验证"), DefaultValue(true)]
        public ButtonBase Button
        {
            get
            {
                return btn;
            }
            set
            {
                if (!DesignMode && hasCreate)
                {
                    if (value == null) btn.RemoveControl(this);
                    else btn.AddControl(this);
                }
                btn = value;
            }
        }


        private string _ver = "20170723";
        ////// 获取或设置是否允许空值
        [Description("版本号"), Category("版本信息")]
        public string ver
        {
            get { return _ver; }
        }

        private RegularAuthenticationSettings _selectRegExp = null;

        [Description("设置用于验证控件值的正则表达式"), Category("验证")]
        [Editor(typeof(RegDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public RegularAuthenticationSettings SelectRegexExpression
        {
            get
            {
                return _selectRegExp;
            }
            set
            {
                _selectRegExp = value;
                if (_selectRegExp != null)
                {
                    RegexExpression = _selectRegExp.Regularly;
                    ErrorMessage = _selectRegExp.ErrorMessage;
                    EmptyMessage = _selectRegExp.EmptyPopMessage;
                }


            }
        }


        private string _regExp = string.Empty;
        ////// 获取或设置用于验证控件值的正则表达式
        [Description("获取或设置用于验证控件值的正则表达式，不能可以自定义"), Category("验证"), DefaultValue("")]
        //[Editor(typeof(RegDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string RegexExpression
        {
            get { return _regExp; }
            set { _regExp = value; }
        }

 

        private bool _allEmpty = false;
        ////// 获取或设置是否允许空值
        [Description("获取或设置是否允许空值"), Category("验证"), DefaultValue(true)]
        public bool AllowEmpty
        {
            get { return _allEmpty; }
            set { _allEmpty = value; }
        }

        private bool _removeSpace = false;
        ////// 获取或设置验证的时候是否除去头尾空格
        [Description("获取或设置验证的时候是否除去头尾空格"), Category("验证"), DefaultValue(false)]
        public bool RemoveSpace
        {
            get { return _removeSpace; }
            set { _removeSpace = value; }
        }

        private string _empMsg = string.Empty;
        ////// 获取或设置当控件的值为空的时候显示的信息
        [Description("获取或设置当控件的值为空的时候显示的信息"), Category("验证"), DefaultValue("")]
        public string EmptyMessage
        {
            get { return _empMsg; }
            set { _empMsg = value; }
        }

        private string _errMsg = string.Empty;
        ////// 获取或设置当不满足正则表达式结果的时候显示的错误信息
        [Description("获取或设置当不满足正则表达式结果的时候显示的错误信息"), Category("验证"), DefaultValue("")]
        public string ErrorMessage
        {
            get { return _errMsg; }
            set { _errMsg = value; }
        }

        public event CustomerValidatedHandler CustomerValidated;

        public void SelectAll()
        {
            base.SelectAll();
        }
        #endregion

        private bool hasCreate = false;
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (btn != null) btn.AddControl(this);
            hasCreate = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (btn != null) btn.RemoveControl(this);
            base.Dispose(disposing);
        }
    }
}

