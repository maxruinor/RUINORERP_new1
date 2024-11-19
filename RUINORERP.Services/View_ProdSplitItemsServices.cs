
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:26
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
    /// 分割明细统计-只管明细产品的数据主表的用查询
    /// </summary>
    public partial class View_ProdSplitItemsServices : BaseServices<View_ProdSplitItems>, IView_ProdSplitItemsServices
    {
        IMapper _mapper;
        public View_ProdSplitItemsServices(IMapper mapper, IBaseRepository<View_ProdSplitItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}