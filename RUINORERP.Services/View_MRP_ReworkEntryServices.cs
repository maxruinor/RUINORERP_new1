
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 22:02:19
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
    /// 返工入库统计
    /// </summary>
    public partial class View_MRP_ReworkEntryServices : BaseServices<View_MRP_ReworkEntry>, IView_MRP_ReworkEntryServices
    {
        IMapper _mapper;
        public View_MRP_ReworkEntryServices(IMapper mapper, IBaseRepository<View_MRP_ReworkEntry> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}