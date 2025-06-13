
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/12/2025 18:16:13
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
    /// 采购退货统计
    /// </summary>
    public partial class View_PurEntryReItemsServices : BaseServices<View_PurEntryReItems>, IView_PurEntryReItemsServices
    {
        IMapper _mapper;
        public View_PurEntryReItemsServices(IMapper mapper, IBaseRepository<View_PurEntryReItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}