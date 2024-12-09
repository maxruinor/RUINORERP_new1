
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Processor
{

    public partial class tb_CRM_LeadsProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.CustomerName);
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.Contact_Name);
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.LeadsStatus, QueryFieldType.CmbEnum, typeof(LeadsStatus));
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.SocialTools);
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.GetCustomerSource);
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.InterestedProducts);
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.SalePlatform);
            queryFilter.SetQueryField<tb_CRM_Leads>(c => c.Created_at);
            return queryFilter;
        }
    }
}