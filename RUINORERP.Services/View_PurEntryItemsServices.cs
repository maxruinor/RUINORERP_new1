
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/07/2024 20:12:59
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
    /// 采购入库统计
    /// </summary>
    public partial class View_PurEntryItemsServices : BaseServices<View_PurEntryItems>, IView_PurEntryItemsServices
    {
        IMapper _mapper;
        public View_PurEntryItemsServices(IMapper mapper, IBaseRepository<View_PurEntryItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}