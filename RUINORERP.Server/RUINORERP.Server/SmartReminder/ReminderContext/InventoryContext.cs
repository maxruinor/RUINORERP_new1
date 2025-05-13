using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.ReminderContext
{
    public class InventoryContext : IReminderContext
    {
        private readonly tb_Inventory _stock;

        public InventoryContext(tb_Inventory stock) => _stock = stock;

        public Type DataType => typeof(tb_Inventory);
        public object GetData() => _stock;
    }

}
