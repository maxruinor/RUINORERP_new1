namespace HLH.Lib.Validation
{

    public enum ValidateRuleRegexEnum
    {
        /// <summary>
        ///��ַ
        /// </summary>
        URL,
        /// <summary>
        /// 4-11λQQ��
        /// </summary>
        QQ,

        /// <summary>
        /// �����ʽ
        /// </summary>
        EMAIL,

        /// <summary>
        /// ����
        /// </summary>
        Number,

        /// <summary>
        /// ������
        /// </summary>
        PositiveInteger,

        /// <summary>
        /// ������
        /// </summary>
        NegativeInteger,

        /// <summary>
        /// �Ϸ����ڣ�����YYYY-MM-DD��YYYY/MM/DD��YYYY-MM-DD HH:MM:SS��YYYY-MM-DD HH:MM��HH:MM:SS�ȣ�
        /// </summary>
        DataTime,

        /// <summary>
        /// ����
        /// </summary>
        Chinese,

        /// <summary>
        /// �����ģ����ֺ�Ӣ����ĸ��
        /// </summary>
        NoChinese
    }
}
