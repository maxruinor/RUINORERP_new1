
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/10/2024 20:24:13
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
    /// View_BOM
    /// </summary>
    public partial class View_BOMServices : BaseServices<View_BOM>, IView_BOMServices
    {
        IMapper _mapper;
        public View_BOMServices(IMapper mapper, IBaseRepository<View_BOM> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}