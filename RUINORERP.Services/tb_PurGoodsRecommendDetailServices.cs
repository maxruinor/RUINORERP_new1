﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/19/2024 12:18:09
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
    /// 采购商品建议
    /// </summary>
    public partial class tb_PurGoodsRecommendDetailServices : BaseServices<tb_PurGoodsRecommendDetail>, Itb_PurGoodsRecommendDetailServices
    {
        IMapper _mapper;
        public tb_PurGoodsRecommendDetailServices(IMapper mapper, IBaseRepository<tb_PurGoodsRecommendDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}