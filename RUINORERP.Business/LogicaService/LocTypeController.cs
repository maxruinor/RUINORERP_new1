using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;

namespace RUINORERP.Business.LogicaService
{
    public class LocTypeController
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        public Itb_LocationTypeServices _Itb_LocationTypeServices { get; set; }
        private readonly ILogger<LocTypeController> _logger;
        public LocTypeController(ILogger<LocTypeController> logger, Itb_LocationTypeServices tb_LocationTypeServices, IUnitOfWorkManage unitOfWorkManage)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _Itb_LocationTypeServices = tb_LocationTypeServices;
        }

        public ValidationResult Validator(tb_LocationType info)
        {
            tb_LocationTypeValidator validator = new tb_LocationTypeValidator();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        public async Task<List<tb_LocationType>> Query()
        {
            List<tb_LocationType> list = await _Itb_LocationTypeServices.QueryAsync();
            return list;
        }
        public async Task<int> Add(tb_LocationType entity)
        {
            int id = await _Itb_LocationTypeServices.Add(entity);
            return id;
        }

        public async Task<bool> Update(tb_LocationType entity)
        {
            bool rs = await _Itb_LocationTypeServices.Update(entity);
            return rs;
        }

        public async Task<bool> Delete(tb_LocationType entity)
        {
            bool rs = await _Itb_LocationTypeServices.Delete(entity);
            return rs;
        }

    }
}
