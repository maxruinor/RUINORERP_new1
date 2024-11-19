
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:24
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
    /// 组合明细统计-只管明细
    /// </summary>
    public partial class View_ProdMergeItemsServices : BaseServices<View_ProdMergeItems>, IView_ProdMergeItemsServices
    {
        IMapper _mapper;
        public View_ProdMergeItemsServices(IMapper mapper, IBaseRepository<View_ProdMergeItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}