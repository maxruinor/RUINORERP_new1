using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public class BusinessHelper
    {
        private static BusinessHelper m_instance;

        public static BusinessHelper Instance
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
            m_instance = new BusinessHelper();
        }

        public void SetContext(ApplicationContext appContext)
        {
            _appContext = appContext;
        }
        public static ApplicationContext _appContext;
        public BusinessHelper(ApplicationContext appContext = null)
        {
            _appContext = appContext;
        }

        #region 处理实体的公共行为

        public void InitEntity(object entity)
        {
            var curUser = _appContext.CurUserInfo;
            // if (entity.ContainsProperty("Id"))
            //    entity.SetPropertyValue("Id", IdHelper.GetLongId());
            if (entity.ContainsProperty("Created_at"))
                entity.SetPropertyValue("Created_at", DateTime.Now);
            //if (entity.ContainsProperty("UpdateTime"))
            //    entity.SetPropertyValue("UpdateTime", DateTime.Now);
            if (entity.ContainsProperty("Created_by"))
                entity.SetPropertyValue("Created_by", curUser?.Id);
            if (entity.ContainsProperty("isdeleted"))
                entity.SetPropertyValue("isdeleted", false);
        }


        /// <summary>
        /// 编辑实体信息，应该人为才记录。程序回写这种不操作
        /// </summary>
        /// <param name="entity"></param>
        public void EditEntity(object entity)
        {
            var curUser = _appContext.CurUserInfo;
            if (entity.ContainsProperty("Modified_at"))
                entity.SetPropertyValue("Modified_at", DateTime.Now);
            if (entity.ContainsProperty("Modified_by"))
                entity.SetPropertyValue("Modified_by", curUser?.Id);
        }

        /// <summary>
        /// 审核单据信息
        /// </summary>
        /// <param name="entity"></param>
        public void ApproverEntity(object entity)
        {
            var curUser = _appContext.CurUserInfo;
            if (entity.ContainsProperty("Approver_at"))
                entity.SetPropertyValue("Approver_at", DateTime.Now);
            if (entity.ContainsProperty("Approver_by"))
                entity.SetPropertyValue("Approver_by", curUser?.Id);
        }

        /// <summary>
        /// 设置为 未审核，未打印 etc
        /// </summary>
        /// <param name="entity"></param>
        public void InitStatusEntity(object entity)
        {
            var curUser = _appContext.CurUserInfo;
            //if (entity.ContainsProperty("ApprovalStatus"))
            //    entity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.未审核);
            if (entity.ContainsProperty("PrintStatus"))
                entity.SetPropertyValue("PrintStatus", 0);
            if (entity.ContainsProperty("DataStatus"))
                entity.SetPropertyValue("DataStatus", (int)DataStatus.草稿);

        }

        /// <summary>
        /// 编辑实体信息
        /// </summary>
        /// <param name="entity"></param>
        public void DelEntityBylogic(object entity)
        {
            var curUser = _appContext.CurUserInfo;
            if (entity.ContainsProperty("Modified_at"))
                entity.SetPropertyValue("Modified_at", DateTime.Now);
            if (entity.ContainsProperty("Modified_by"))
                entity.SetPropertyValue("Modified_by", curUser?.Id);
            if (entity.ContainsProperty("isdeleted"))
                entity.SetPropertyValue("isdeleted", true);
        }
        #endregion
    }
}
