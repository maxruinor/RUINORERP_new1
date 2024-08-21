using RUINORERP.IServices.BASE;
using RUINORERP.Model.Models;
using System.Threading.Tasks;
using RUINORERP.Model;
namespace RUINORERP.IServices
{
    /// <summary>
    /// ISupplierServices
    /// </summary>	
    public interface IUnitServices : IBaseServices<tb_Unit>
	{
        Task<tb_Unit> SaveRole(tb_Unit Supplier);
        Task<string> GetSupplierNameByRid(int rid);

    }
}
