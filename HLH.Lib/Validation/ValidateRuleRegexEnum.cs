namespace HLH.Lib.Validation
{

    public enum ValidateRuleRegexEnum
    {
        /// <summary>
        ///网址
        /// </summary>
        URL,
        /// <summary>
        /// 4-11位QQ号
        /// </summary>
        QQ,

        /// <summary>
        /// 邮箱格式
        /// </summary>
        EMAIL,

        /// <summary>
        /// 数字
        /// </summary>
        Number,

        /// <summary>
        /// 正整数
        /// </summary>
        PositiveInteger,

        /// <summary>
        /// 负整数
        /// </summary>
        NegativeInteger,

        /// <summary>
        /// 合法日期（包括YYYY-MM-DD，YYYY/MM/DD，YYYY-MM-DD HH:MM:SS，YYYY-MM-DD HH:MM，HH:MM:SS等）
        /// </summary>
        DataTime,

        /// <summary>
        /// 中文
        /// </summary>
        Chinese,

        /// <summary>
        /// 非中文（数字和英文字母）
        /// </summary>
        NoChinese
    }
}
