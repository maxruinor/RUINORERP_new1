
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:29
// **************************************
using AutoMapper;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using System.Threading.Tasks;
using System;
﻿using SqlSugar;
using System.Collections.Generic;


namespace RUINORERP.Services
{
    /// <summary>
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容
    /// </summary>
    public partial class tb_ProductTypeServices : BaseServices<tb_ProductType>, Itb_ProductTypeServices
    {
        IMapper _mapper;
        public tb_ProductTypeServices(IMapper mapper, IBaseRepository<tb_ProductType> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}