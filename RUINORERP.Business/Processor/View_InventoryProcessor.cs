

using Microsoft.Extensions.Logging;
using RUINORERP.Common.Helper;
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

    public partial class View_InventoryProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
    
            queryFilter.SetQueryField<View_Inventory>(c => c.SKU);
            queryFilter.SetQueryField<View_Inventory>(c => c.CNName);
            queryFilter.SetQueryField<View_Inventory>(c => c.Model);
            queryFilter.SetQueryField<View_Inventory>(c => c.Category_ID);
            queryFilter.SetQueryField<View_Inventory>(c => c.Specifications);
            queryFilter.SetQueryField<View_Inventory>(c => c.ProductNo);
            queryFilter.SetQueryField<View_Inventory>(c => c.prop);
            queryFilter.SetQueryField<View_Inventory>(c => c.DepartmentID);
            queryFilter.SetQueryField<View_Inventory>(c => c.Type_ID);
            queryFilter.SetQueryField<View_Inventory>(c => c.Location_ID);
            queryFilter.SetQueryField<View_Inventory>(c => c.Rack_ID);
            queryFilter.SetQueryField<View_Inventory>(c => c.BOM_ID);
            queryFilter.SetQueryField<View_Inventory>(c => c.Unit_ID);
            //queryFilter.SetQueryField<View_Inventory>(c => c.LastInventoryDate);
            //queryFilter.SetQueryField<View_Inventory>(c => c.LatestStorageTime);
            //queryFilter.SetQueryField<View_Inventory>(c => c.LatestOutboundTime);
            return queryFilter;
        }
    }
}