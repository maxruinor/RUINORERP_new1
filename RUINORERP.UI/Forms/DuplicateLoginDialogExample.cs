using RUINORERP.PacketSpec.Models.Authentication;
using System;
using System.Windows.Forms;

namespace RUINORERP.UI.Forms
{
    /// <summary>
    /// 重复登录对话框使用示例
    /// 展示如何正确调用和使用DuplicateLoginDialog
    /// </summary>
    public static class DuplicateLoginDialogExample
    {
        /// <summary>
        /// 显示重复登录确认对话框的示例方法
        /// </summary>
        /// <param name="duplicateLoginResult">重复登录结果信息</param>
        /// <returns>用户选择的处理方式</returns>
        public static async System.Threading.Tasks.Task<DuplicateLoginAction> ShowDuplicateLoginDialogAsync(DuplicateLoginResult duplicateLoginResult)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                // 确保在UI线程上显示对话框
                if (Application.OpenForms.Count > 0)
                {
                    var mainForm = Application.OpenForms[0];
                    return (DuplicateLoginAction)mainForm.Invoke(new Func<DuplicateLoginAction>(() =>
                    {
                        using (var dialog = new DuplicateLoginDialog(duplicateLoginResult))
                        {
                            var result = dialog.ShowDialog();
                            return result == DialogResult.OK ? dialog.SelectedAction : DuplicateLoginAction.Cancel;
                        }
                    }));
                }

                // 如果没有主窗体，返回默认选项
                return DuplicateLoginAction.Cancel;
            });
        }

        /// <summary>
        /// 创建模拟重复登录结果数据（用于测试）
        /// </summary>
        /// <returns>模拟的重复登录结果</returns>
        public static DuplicateLoginResult CreateSampleDuplicateLoginResult()
        {
            return new DuplicateLoginResult
            {
                HasDuplicateLogin = true,
                Message = "您的账号已在其他地方登录，请选择处理方式",
                RequireUserConfirmation = true,
                Type = DuplicateLoginType.RemoteOnly,
                AllowMultipleLocalSessions = false,
                ExistingSessions = new System.Collections.Generic.List<ExistingSessionInfo>
                {
                    new ExistingSessionInfo
                    {
                        SessionId = "session_001",
                        LoginTime = DateTime.Now.AddMinutes(-30),
                        ClientIp = "192.168.1.100",
                        DeviceInfo = "Windows PC - Chrome浏览器",
                        IsLocal = false
                    },
                    new ExistingSessionInfo
                    {
                        SessionId = "session_002", 
                        LoginTime = DateTime.Now.AddHours(-2),
                        ClientIp = "192.168.1.101",
                        DeviceInfo = "Windows PC - Edge浏览器",
                        IsLocal = false
                    }
                }
            };
        }
    }
}