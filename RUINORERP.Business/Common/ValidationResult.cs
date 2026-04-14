using System;

namespace RUINORERP.Business.Common
{
    /// <summary>
    /// 可确认的验证结果类
    /// 用于业务层向UI层传递验证结果，支持用户确认机制，避免在业务层直接弹出MessageBox
    /// </summary>
    public class ConfirmableValidationResult
    {
        /// <summary>
        /// 是否需要用户确认
        /// </summary>
        public bool NeedUserConfirm { get; set; }

        /// <summary>
        /// 确认对话框标题
        /// </summary>
        public string ConfirmTitle { get; set; }

        /// <summary>
        /// 确认对话框消息
        /// </summary>
        public string ConfirmMessage { get; set; }

        /// <summary>
        /// 确认按钮文本（可选）
        /// </summary>
        public string ConfirmButtonText { get; set; }

        /// <summary>
        /// 取消按钮文本（可选）
        /// </summary>
        public string CancelButtonText { get; set; }

        /// <summary>
        /// 验证是否通过（不需要用户确认时有效）
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误信息（验证失败时）
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 创建通过的验证结果
        /// </summary>
        public static ConfirmableValidationResult Pass()
        {
            return new ConfirmableValidationResult
            {
                IsValid = true,
                NeedUserConfirm = false
            };
        }

        /// <summary>
        /// 创建失败的验证结果
        /// </summary>
        public static ConfirmableValidationResult Fail(string errorMessage)
        {
            return new ConfirmableValidationResult
            {
                IsValid = false,
                NeedUserConfirm = false,
                ErrorMessage = errorMessage
            };
        }

        /// <summary>
        /// 创建需要用户确认的验证结果
        /// </summary>
        /// <param name="title">确认对话框标题</param>
        /// <param name="message">确认对话框消息</param>
        /// <param name="confirmButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">取消按钮文本</param>
        public static ConfirmableValidationResult NeedConfirm(
            string title,
            string message,
            string confirmButtonText = "确定",
            string cancelButtonText = "取消")
        {
            return new ConfirmableValidationResult
            {
                IsValid = false, // 需要用户确认前视为未通过
                NeedUserConfirm = true,
                ConfirmTitle = title,
                ConfirmMessage = message,
                ConfirmButtonText = confirmButtonText,
                CancelButtonText = cancelButtonText
            };
        }

        /// <summary>
        /// 判断是否需要显示确认对话框
        /// </summary>
        public bool ShouldShowConfirmDialog()
        {
            return NeedUserConfirm && !string.IsNullOrEmpty(ConfirmMessage);
        }
    }
}
