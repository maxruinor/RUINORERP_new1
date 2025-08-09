using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel
{

 
        /// <summary>
        /// 验证结果类，包含验证状态和错误消息
        /// </summary>
        public class RuleValidationResult
        {
            private readonly List<string> _errorMessages = new List<string>();

            /// <summary>
            /// 验证是否通过
            /// </summary>
            public bool IsValid => _errorMessages.Count == 0;

            /// <summary>
            /// 错误消息集合
            /// </summary>
            public IReadOnlyCollection<string> ErrorMessages => _errorMessages.AsReadOnly();

            /// <summary>
            /// 添加错误消息
            /// </summary>
            /// <param name="message">错误消息</param>
            public void AddError(string message)
            {
                if (!string.IsNullOrWhiteSpace(message) && !_errorMessages.Contains(message))
                {
                    _errorMessages.Add(message);
                }
            }

            /// <summary>
            /// 合并另一个验证结果的错误消息
            /// </summary>
            /// <param name="other">其他验证结果</param>
            public void Merge(RuleValidationResult other)
            {
                if (other == null || other.IsValid)
                    return;

                foreach (var message in other.ErrorMessages)
                {
                    AddError(message);
                }
            }

            /// <summary>
            /// 将所有错误消息组合成一个字符串，每行一条消息
            /// </summary>
            /// <param name="prefix">每条消息前的前缀（如"• "、"- "）</param>
            /// <returns>组合后的字符串</returns>
            public string GetCombinedErrors(string prefix = "• ")
            {
                if (IsValid)
                    return string.Empty;

                var sb = new StringBuilder();
                foreach (var message in _errorMessages)
                {
                    sb.Append(prefix);
                    sb.AppendLine(message);
                }
                // 移除最后一个多余的换行符
                if (sb.Length > 0)
                    sb.Length -= Environment.NewLine.Length;

                return sb.ToString();
            }

            /// <summary>
            /// 转换为字符串表示（所有错误消息）
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return GetCombinedErrors();
            }
        }
   



}
