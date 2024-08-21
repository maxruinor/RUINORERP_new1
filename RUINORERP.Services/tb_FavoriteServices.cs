
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:03
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
    /// 收藏表 收藏订单 产品 库存报警等
    /// </summary>
    public partial class tb_FavoriteServices : BaseServices<tb_Favorite>, Itb_FavoriteServices
    {
        IMapper _mapper;
        public tb_FavoriteServices(IMapper mapper, IBaseRepository<tb_Favorite> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}