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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Context;

namespace RUINORERP.Business.LogicaService
{
    public class UnitController
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        public IUnitServices _unitServices { get; set; }
        private readonly ILogger<UnitController> _logger;
        public Itb_UnitServices _Itb_UnitServices { get; set; }
        private readonly ApplicationContext _appContext;
        public UnitController(ILogger<UnitController> logger, Itb_UnitServices Itb_UnitServices, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _Itb_UnitServices = Itb_UnitServices;
            _appContext = appContext;
        }

        public ValidationResult Validator(tb_Unit info)
        {
            tb_UnitValidator validator = _appContext.GetRequiredService<tb_UnitValidator>();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        public async Task<List<tb_Unit>> Query()
        {
            List<tb_Unit> list = await _Itb_UnitServices.QueryAsync();
            return list;
        }

        public async Task<tb_Unit> AddReturnEntity(tb_Unit entity)
        {
            tb_Unit unit = await _Itb_UnitServices.AddReEntityAsync(entity);
            //string key = "tb_Unit:Unit_ID:" + entity.Unit_ID.ToString();
            //Extensions.Middlewares.MyCacheManager.Instance.Cache.Add(key, entity.UnitName);
            MyCacheManager.Instance.UpdateEntityList<tb_Unit>(entity);
            return unit;
        }

        public async Task<long> Add(tb_Unit entity)
        {
            long id = await _Itb_UnitServices.Add(entity);
            return id;
        }

        public async Task<bool> Update(tb_Unit entity)
        {
            bool rs = await _Itb_UnitServices.Update(entity);
            if (rs)
            {
                MyCacheManager.Instance.UpdateEntityList<tb_Unit>(entity);
            }
            return rs;
        }

        public async Task<bool> Delete(tb_Unit entity)
        {
            bool rs = await _Itb_UnitServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Unit>(entity);
            }
            return rs;
        }

        public async Task<bool> TransTest()
        {
            //var data = new MessageModel<string>() { success = true, msg = "" };
            bool rs = true;
            if (true)
            {
                try
                {
                    Model.tb_Unit unit = new tb_Unit();
                    unit.Unit_ID = 6;
                    unit.UnitName = "transtest";
                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();
                    //先添加再删除
                    //await _unitServices.Add(unit);
                    //await _unitServices.DeleteById(1);
                    ////先删除再添加
                    ///
                    Model.tb_Unit unit1 = new tb_Unit();
                    unit1.Unit_ID = 4;
                    unit1.UnitName = "transtest44";

                    await _unitServices.Add(unit);

                    await _unitServices.Add(unit1);

                    await _unitServices.DeleteById(110);

                    // 注意信息的完整性
                    _unitOfWorkManage.CommitTran();
                    _logger.Error("事务成功");
                    rs = true;
                }
                catch (Exception ex)
                {

                    _unitOfWorkManage.RollbackTran();
                    _logger.Error(ex);
                }
            }
            else
            {
                //data.success = false;
                //data.msg = "当前不处于开发模式，代码生成不可用！";
            }

            return rs;
        }


    }
}
