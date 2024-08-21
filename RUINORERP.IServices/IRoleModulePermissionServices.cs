using RUINORERP.IServices.BASE;
using RUINORERP.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.IServices
{
    /// <summary>
    /// RoleModulePermissionServices
    /// </summary>	
    public interface IRoleModulePermissionServices :IBaseServices<RoleModulePermission>
	{

        Task<List<RoleModulePermission>> GetRoleModule();
        Task<List<TestMuchTableResult>> QueryMuchTable();
        Task<List<RoleModulePermission>> RoleModuleMaps();
        Task<List<RoleModulePermission>> GetRMPMaps();
        /// <summary>
        /// �������²˵���ӿڵĹ�ϵ
        /// </summary>
        /// <param name="permissionId">�˵�����</param>
        /// <param name="moduleId">�ӿ�����</param>
        /// <returns></returns>
        Task UpdateModuleId(int permissionId, int moduleId);
    }
}
