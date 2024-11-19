
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:44
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
    /// 缴库明细统计
    /// </summary>
    public partial class View_FinishedGoodsInvItemsServices : BaseServices<View_FinishedGoodsInvItems>, IView_FinishedGoodsInvItemsServices
    {
        IMapper _mapper;
        public View_FinishedGoodsInvItemsServices(IMapper mapper, IBaseRepository<View_FinishedGoodsInvItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}