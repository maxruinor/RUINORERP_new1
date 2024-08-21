using System.Text.RegularExpressions;

namespace HLH.Lib.Validation
{
    /// <summary>
    /// validate using Regular expression
    /// </summary>
    public class ValidateRuleRegexMatching : ValidateRuleBase
    {
        #region Data Members

        string _regexExpression = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName">name of property</param>
        /// <param name="regularExpression">regular expression</param>
        public ValidateRuleRegexMatching(string propertyName, string regularExpression)
            : this(propertyName, propertyName, regularExpression)
        {
        }


        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="propertyName">name of property</param>
        /// <param name="regularExpression">regular expression</param>
        public ValidateRuleRegexMatching(string propertyName, ValidateRuleRegexEnum regularExpression)
            : this(propertyName, propertyName)
        {
            //对枚举的验证格式进行匹配
            switch (regularExpression)
            {
                case ValidateRuleRegexEnum.URL:
                    _regexExpression = @"[a-zA-z]+://[^s]*";
                    break;
                case ValidateRuleRegexEnum.QQ:
                    _regexExpression = @"^[0-9]{4,11}$";//四位到十一位数字
                    break;
                case ValidateRuleRegexEnum.EMAIL:
                    _regexExpression = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                    break;
                case ValidateRuleRegexEnum.Number:
                    _regexExpression = @"^-?\d+$";
                    break;
                case ValidateRuleRegexEnum.DataTime:
                    _regexExpression = @"^(?=\d)(?:(?!(?:1582(?:\.|-|\/)10(?:\.|-|\/)(?:0?[5-9]|1[0-4]))|(?:1752(?:\.|-|\/)0?9(?:\.|-|\/)(?:0?[3-9]|1[0-3])))(?=(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:\d\d)(?:[02468][048]|[13579][26]))\D0?2\D29)|(?:\d{4}\D(?!(?:0?[2469]|11)\D31)(?!0?2(?:\.|-|\/)(?:29|30))))(\d{4})([-\/.])(0?\d|1[012])\2((?!00)[012]?\d|3[01])(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$";
                    break;
                case ValidateRuleRegexEnum.NoChinese:
                    _regexExpression = @"^[A-Za-z0-9]+$";
                    break;
                case ValidateRuleRegexEnum.Chinese:
                    _regexExpression = @"[\u4e00-\u9fa5]";

                    break;
                default:
                    break;
            }

        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName">name of property</param>
        /// <param name="friendlyName">friendly name of property</param>
        /// <param name="regularExpression">regular expression</param>
        public ValidateRuleRegexMatching(string propertyName, string friendlyName, string regularExpression)
            : base(propertyName, friendlyName)
        {
            _regexExpression = regularExpression;

        }

        #endregion

        #region Properties

        /// <summary>
        /// get regular expression
        /// </summary>
        public string RegexExpression
        {
            get { return _regexExpression; }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// validate the regular expression
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>true for valid value</returns>
        internal override bool Validate(object value)
        {
            // if (value == null || !Regex.IsMatch((string)value, RegexExpression))
            if (value == null || !Regex.IsMatch(value.ToString(), RegexExpression))
            {
                Description = string.Format("{0} is not valid.", FriendlyName);
                return false;
            }

            Description = string.Empty;
            return true;
        }

        #endregion
    }
}
