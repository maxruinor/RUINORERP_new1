
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Helper;
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

    public partial class tb_DepartmentProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
    
            queryFilter.SetQueryField<tb_Department>(c => c.DepartmentName);
            queryFilter.SetQueryField<tb_Department>(c => c.DepartmentCode);
            return queryFilter;
        }
    }
}