using AutoMapper;
using RUINORERP.Common;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.Model.Models;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RUINORERP.Services
{
    public class UnitServices : BaseServices<tb_Unit>, IUnitServices
    {
        IMapper _mapper;
        public UnitServices(IMapper mapper, IBaseRepository<tb_Unit> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }

        public Task<string> GetSupplierNameByRid(int rid)
        {
            throw new System.NotImplementedException();
        }

        public async Task<tb_Unit> SaveRole(tb_Unit supplier)
        {
            //Role role = new Role(roleName);
            tb_Unit model = new tb_Unit();
            //var userList = await base.Query(a => a.Name == role.Name && a.Enabled);
            //if (userList.Count > 0)
            //{
            //    model = userList.FirstOrDefault();
            //}
            //else
            //{
            var id = await base.Add(supplier);
            model = await base.QueryByIdAsync(id);
            //}

            return model;

        }

 

    }
}
