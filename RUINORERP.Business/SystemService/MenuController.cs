using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using FluentValidation;
using FluentValidation.Results;

namespace RUINORERP.Business.SystemService
{
    public class MenuController
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        //public Itb_MenuInfoServices _Itb_MenuInfoServices { get; set; }
        public Itb_MenuAssemblyInfoServices _Itb_MenuAssemblyInfoServices { get; set; }
        public Itb_FieldInfoServices _Itb_FieldInfoServices { get; set; }


        private readonly IValidator<tb_MenuInfo> _validator;
        private readonly ILogger<MenuController> _logger;
        public MenuController(IValidator<tb_MenuInfo> validator,
            ILogger<MenuController> logger,
            Itb_MenuAssemblyInfoServices MenuAssemblyInfoServices,
                   Itb_FieldInfoServices FieldInfoServices,
            IUnitOfWorkManage unitOfWorkManage)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _Itb_MenuAssemblyInfoServices = MenuAssemblyInfoServices;
            _Itb_FieldInfoServices = FieldInfoServices;
            _validator = validator;
        }

        public ValidationResult Validator(tb_MenuInfo info)
        {
            tb_MenuInfoValidator validator = new tb_MenuInfoValidator();
            ValidationResult results = validator.Validate(info);
            return results;
        }


        /// <summary>
        /// 检测是否进行过初始化
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckMenuInitialized()
        {
            bool rs = false;
            //基类中定义一个 检测是否存在数据
            List<tb_MenuInfo> list = await _Itb_MenuAssemblyInfoServices.QueryAsync();
            int counter = list.Count;
            if (counter > 1)
            {
                rs = true;
            }
            return rs;
        }

        public bool ClearAllMenuItems<T>()
        {
            bool rs = false;
            //基类中定义一个 检测是否存在数据
            rs = _unitOfWorkManage.GetDbClient().DbMaintenance.TruncateTable<T>();
            return rs;
        }

        public bool CheckAssemblyObjectExists(string classPath)
        {
            //db.Queryable<Student>().Where(it => it.Id == id).ToList();

            bool rs = false;
            //基类中定义一个 检测是否存在数据
            //int counter = _Itb_MenuAssemblyInfoServices.Db.Queryable<tb_MenuAssemblyInfo>()
            //    .Where(m => m.ClassPath == classPath).ToList().Count;
            //if (counter > 1)
            //{
            //    rs = true;
            //}
            return rs;
        }

        public async Task<List<tb_MenuInfo>> Query()
        {
            List<tb_MenuInfo> list = await _Itb_MenuAssemblyInfoServices.QueryAsync();
            return list;
        }


        public async Task<bool> AddMenuInfoAsync(tb_MenuInfo info)
        {

            bool rs = false;

            if (_validator.Validate(info).IsValid)
            {
                //
            }
            //基类中定义一个 检测是否存在数据
            int flag = await _Itb_MenuAssemblyInfoServices.Add(info);
            if (flag > 1)
            {
                rs = true;
            }
            return rs;
        }

        public tb_MenuInfo AddMenuInfo(tb_MenuInfo info)
        {
            if (_validator.Validate(info).IsValid)
            {
                //
            }
            return _unitOfWorkManage.GetDbClient().Insertable<tb_MenuInfo>(info).ExecuteReturnEntity();
        }

        public async Task<bool> AddMenuInfo(List<tb_MenuInfo> infos)
        {

            bool rs = false;

            //基类中定义一个 检测是否存在数据
            int flag = await _Itb_MenuAssemblyInfoServices.Add(infos);
            if (flag > 1)
            {
                rs = true;
            }
            return rs;
        }
        public async Task<bool> UpdateMenuInfo(tb_MenuInfo info)
        {
            bool rs = false;
            rs = await _Itb_MenuAssemblyInfoServices.Update(info);
            return rs;
        }

        public async Task<bool> DeleteMenuInfo(int id)
        {
            bool rs = false;
            rs = await _Itb_MenuAssemblyInfoServices.DeleteById(id);
            return rs;
        }


        public async Task<bool> AddFieldInfo(List<tb_FieldInfo> infos)
        {
            bool rs = false;
            //if (_validator.Validate(info).IsValid)
            //{
            // 
            //}
            //基类中定义一个 检测是否存在数据
            int flag = await _Itb_FieldInfoServices.Add(infos);
            if (flag > 1)
            {
                rs = true;
            }
            return rs;
        }


    }

}
