using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel
{
    public class InventoryContext : IReminderContext
    {
        private readonly List<tb_Inventory> _stocks;

        public InventoryContext(List<tb_Inventory> stocks) => _stocks = stocks;

        public Type DataType => typeof(List<tb_Inventory>);
        public object GetData() => _stocks;
    }
}
