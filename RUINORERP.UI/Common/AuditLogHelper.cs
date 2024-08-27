using FastReport.DevComponents.DotNetBar;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.BaseForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public class AuditLogHelper
    {
        private static AuditLogHelper m_instance;

        public static AuditLogHelper Instance
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
            m_instance = new AuditLogHelper();
        }
        public void CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            //将操作记录保存到数据库中

            BizTypeMapper mapper = new BizTypeMapper();
            //var BizTypeText = mapper.GetBizType(typeof(T).Name).ToString();
            var BizType = mapper.GetBizType(typeof(T).Name);

            BillConverterFactory bcf = MainForm.Instance.AppContext.GetRequiredService<BillConverterFactory>();
            CommBillData cbd = bcf.GetBillData<T>(entity);
            //将操作记录保存到数据库中
            tb_AuditLogs auditLog = new tb_AuditLogs();
            auditLog.UserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;
            auditLog.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            auditLog.ActionType = action;
            auditLog.ObjectType = (int)BizType;

            //string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            //long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            //auditLog.ObjectId = pkid;
            auditLog.ObjectId = cbd.BillID;
            auditLog.ObjectNo = cbd.BillNo;
            if (description.IsNotEmptyOrNull())
            {
                auditLog.Notes = description;
            }
            auditLog.ActionTime = DateTime.Now;
            try
            {
                MainForm.Instance.AppContext.Db.Insertable<tb_AuditLogs>(auditLog).ExecuteReturnEntityAsync();
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
        public void CreateAuditLog(string action, string description)
        {

            //将操作记录保存到数据库中
            //BizTypeMapper mapper = new BizTypeMapper();
            ////var BizTypeText = mapper.GetBizType(typeof(T).Name).ToString();
            //var BizType = mapper.GetBizType(typeof(T).Name);

            BillConverterFactory bcf = MainForm.Instance.AppContext.GetRequiredService<BillConverterFactory>();
            // CommBillData cbd = bcf.GetBillData<T>(entity);
            //将操作记录保存到数据库中
            tb_AuditLogs auditLog = new tb_AuditLogs();
            auditLog.UserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;
            auditLog.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            auditLog.ActionType = action;
            auditLog.ObjectType = -1;// (int)BizType;

            //string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            //long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            //auditLog.ObjectId = pkid;
            // auditLog.ObjectId = cbd.BillID;
            // auditLog.ObjectNo = cbd.BillNo;
            if (description.IsNotEmptyOrNull())
            {
                auditLog.Notes = description;
            }
            auditLog.ActionTime = DateTime.Now;
            try
            {
                MainForm.Instance.AppContext.Db.Insertable<tb_AuditLogs>(auditLog).ExecuteReturnEntityAsync();
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
                            var value = property.GetValue(auditLog);
                            sb.Append($"{property.Name}: {value}");
                        }
                    }
                }
            }
            rs = sb.ToString();
            return rs;
        }

    }
}
