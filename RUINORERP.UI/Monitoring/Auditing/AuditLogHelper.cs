using FastReport.DevComponents.DotNetBar;
using LiveChartsCore.Geo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.BaseForm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Monitoring.Auditing
{
    public class AuditLogHelper_old
    {
        private static AuditLogHelper_old m_instance;

        public static AuditLogHelper_old Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Initialize();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }


        /// <summary>
        /// 对象实例化
        /// </summary>
        public static void Initialize()
        {
            m_instance = new AuditLogHelper_old();
        }
        public async Task CreateAuditLog<T>(string action, T entity, string description) where T : class
        {

            //将操作记录保存到数据库中
            tb_AuditLogs auditLog = new tb_AuditLogs();
            auditLog.UserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;
            auditLog.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            auditLog.ActionType = action;

            // 使用EntityMappingHelper代替BizTypeMapper
            var BizType = EntityMappingHelper.GetBizType(typeof(T).Name);
            try
            {
                // 使用EntityMappingHelper获取实体信息，无需通过BillConverterFactory
                // 直接从实体对象构建审计日志所需信息
                var entityType = typeof(T);
                var primaryKey = entityType.GetProperty("ID")?.GetValue(entity, null)?.ToString() ?? string.Empty;
                auditLog.ObjectType = (int)BizType;
                //auditLog.ObjectID = primaryKey;
                //// 获取实体名称作为对象名称
                //auditLog.ObjectName = entityType.Name;
                // 尝试从实体获取BillNo属性作为对象编号
                auditLog.ObjectNo = entityType.GetProperty("BillNo")?.GetValue(entity, null)?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"审计日志保存记录时失败:{ex.Message}", Global.UILogType.错误);
            }

            if (description.IsNotEmptyOrNull())
            {
                auditLog.Notes = description;
            }
            auditLog.ActionTime = DateTime.Now;
            try
            {
                // 延迟执行日志插入操作 防止死锁？
                await Task.Delay(150); // 延迟100毫秒
                await MainForm.Instance.AppContext.Db.CopyNew().Insertable<tb_AuditLogs>(auditLog).ExecuteReturnEntityAsync();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"审计日志{GetAuditLogProperties(auditLog)}记录失败:{ex.Message}", Global.UILogType.错误);
            }

        }

        public void CreateAuditLog<T>(string action, T entity) where T : class
        {
            CreateAuditLog<T>(action, entity, "");
        }
        //创建一个审计功能的方法，将单据的操作记录下来
        public async Task CreateAuditLog(string action, string description)
        {

            //将操作记录保存到数据库中
            // 使用EntityMappingHelper代替BizTypeMapper
            ////var BizTypeText = EntityMappingHelper.GetBizType(typeof(T).Name).ToString();
            //var BizType = EntityMappingHelper.GetBizType(typeof(T).Name);

           
            //将操作记录保存到数据库中
            tb_AuditLogs auditLog = new tb_AuditLogs();
            if (MainForm.Instance.AppContext.CurUserInfo == null)
            {
                return;
            }
            auditLog.UserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;
            if (auditLog.UserName == null)
            {
                return;
            }
            auditLog.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            auditLog.ActionType = action;
            auditLog.ObjectType = -1;// (int)BizType;

            if (description.IsNotEmptyOrNull())
            {
                auditLog.Notes = description;
            }
            auditLog.ActionTime = DateTime.Now;
            try
            {
                await MainForm.Instance.AppContext.Db.CopyNew().Insertable<tb_AuditLogs>(auditLog).ExecuteReturnEntityAsync();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"审计日志{GetAuditLogProperties(auditLog)}记录失败:{ex.Message}", Global.UILogType.错误);
            }

        }


        public string GetAuditLogProperties(tb_AuditLogs auditLog)
        {
            string rs = string.Empty;
            if (auditLog == null)
            {
                return rs;
            }
            PropertyInfo[] properties = auditLog.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo property in properties)
            {
                foreach (Attribute attr in property.GetCustomAttributes(true))
                {
                    if (attr is SqlSugar.SugarColumn entityAttr)
                    {
                        if (null != entityAttr)
                        {
                            try
                            {
                                var value = property.GetValue(auditLog);
                                sb.Append($"{property.Name}: {value}");
                            }
                            catch (Exception ex)
                            {
                                MainForm.Instance.logger.LogError("GetAuditLogProperties。", ex);
                            }
                        }
                    }
                }
            }
            rs = sb.ToString();
            return rs;
        }

    }




    // 审计日志帮助类（适配原有接口，逐步迁移到服务）
    public class AuditLogHelper
    {
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<AuditLogHelper> _logger;
        
        // 改为公共构造函数，使用依赖注入代替直接引用MainForm
        public AuditLogHelper(IAuditLogService auditLogService, ILogger<AuditLogHelper> logger)
        {
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            try
            {
                await _auditLogService.LogAsync(action, entity, description);
            }
            catch (Exception ex)
            {
                // 使用ILogger代替直接引用MainForm.Instance
                _logger.LogError(ex, "审计日志记录失败");
            }
        }

        public void CreateAuditLog<T>(string action, T entity) where T : class
        {
            CreateAuditLog(action, entity, "");
        }

        public async Task CreateAuditLog(string action, string description)
        {
            try
            {
                await _auditLogService.LogAsync(action, description);
            }
            catch (Exception ex)
            {
                // 使用ILogger代替直接引用MainForm.Instance
                _logger.LogError(ex, "审计日志记录失败");
            }
        }



    }




}
