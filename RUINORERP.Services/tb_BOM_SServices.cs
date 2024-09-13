
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:24
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
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。
    /// </summary>
    public partial class tb_BOM_SServices : BaseServices<tb_BOM_S>, Itb_BOM_SServices
    {
        IMapper _mapper;
        public tb_BOM_SServices(IMapper mapper, IBaseRepository<tb_BOM_S> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}