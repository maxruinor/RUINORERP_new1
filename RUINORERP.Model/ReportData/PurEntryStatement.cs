using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReportData
{


    /// <summary>
    /// 依客户对账单实体
    /// </summary>
    public partial class PurEntryStatementByCV
    {
        private tb_CustomerVendor _CustomerVendor;
        private List<View_PurEntryItems> _PurEntryItems;

        public tb_CustomerVendor CustomerVendor { get => _CustomerVendor; set => _CustomerVendor = value; }
        public List<View_PurEntryItems> PurEntryItems { get => _PurEntryItems; set => _PurEntryItems = value; }
    }
}
