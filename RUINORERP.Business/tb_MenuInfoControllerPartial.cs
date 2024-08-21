
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/22/2023 17:06:12
// **************************************
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
using RUINORERP.Services;
using SqlSugar;
using FluentValidation;

namespace RUINORERP.Business
{
    /// <summary>
    /// 客户厂商表
    /// </summary>
    public partial class tb_MenuInfoController<T>
    {

        //public Itb_MenuInfoServices _Itb_MenuInfoServices { get; set; }
        public Itb_MenuAssemblyInfoServices _Itb_MenuAssemblyInfoServices { get; set; }
        public Itb_FieldInfoServices _Itb_FieldInfoServices { get; set; }
        //private readonly IValidator<tb_MenuInfo> _validator;

        //public tb_MenuInfoController(IValidator<tb_MenuInfo> validator,
        //    ILogger<tb_MenuInfoController<T>> logger,
        //    Itb_MenuAssemblyInfoServices MenuAssemblyInfoServices,
        //           Itb_FieldInfoServices FieldInfoServices,
        //    IUnitOfWorkManage unitOfWorkManage)
        //{
        //    _logger = logger;
        //    _unitOfWorkManage = unitOfWorkManage;
        //    _Itb_MenuAssemblyInfoServices = MenuAssemblyInfoServices;
        //    _Itb_FieldInfoServices = FieldInfoServices;
        //    _validator = validator;
        //}




        /// <summary>
        /// 检测是否进行过初始化
        /// </summary>
        /// <returns></returns>
        public bool CheckMenuInitialized()
        {
            bool rs =   _unitOfWorkManage.GetDbClient().Queryable<tb_MenuInfo>().Any();
            return rs;
        }

        public bool ClearAllMenuItems()
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


        public async Task<bool> AddMenuInfoAsync(tb_MenuInfo info)
        {

            bool rs = false;
            var rsV = Validator(info);
            if (rsV.IsValid)
            {
                rs = true;
            }
            //基类中定义一个 检测是否存在数据
            long flag = await _Itb_MenuAssemblyInfoServices.Add(info);
            if (flag > 1 && rs)
            {
                rs = true;
            }
            return rs;
        }

        public tb_MenuInfo AddMenuInfo(tb_MenuInfo info)
        {
            var rs = Validator(info);
            if (!rs.IsValid)
            {
                return null;
            }
            return _unitOfWorkManage.GetDbClient().Insertable<tb_MenuInfo>(info).ExecuteReturnEntity();
        }

        public async Task<bool> AddMenuInfo(List<tb_MenuInfo> infos)
        {

            bool rs = false;

            //基类中定义一个 检测是否存在数据
            List<long> flag = await _Itb_MenuAssemblyInfoServices.Add(infos);
            if (flag.Count > 1)
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

        public async Task<bool> DeleteMenuInfo(long id)
        {
            bool rs = false;
            rs = await _Itb_MenuAssemblyInfoServices.DeleteById(id);
            return rs;
        }


        public async Task<bool> AddFieldInfo(List<tb_FieldInfo> infos)
        {
            bool rs = false;

            //基类中定义一个 检测是否存在数据
            List<long> flag = await _Itb_FieldInfoServices.Add(infos);
            if (flag.Count > 1)
            {
                rs = true;
            }
            return rs;
        }


    }
}