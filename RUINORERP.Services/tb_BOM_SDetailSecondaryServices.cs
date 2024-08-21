
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/10/2024 19:17:32
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
    /// 标准物料表次级产出明细
    /// </summary>
    public partial class tb_BOM_SDetailSecondaryServices : BaseServices<tb_BOM_SDetailSecondary>, Itb_BOM_SDetailSecondaryServices
    {
        IMapper _mapper;
        public tb_BOM_SDetailSecondaryServices(IMapper mapper, IBaseRepository<tb_BOM_SDetailSecondary> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}