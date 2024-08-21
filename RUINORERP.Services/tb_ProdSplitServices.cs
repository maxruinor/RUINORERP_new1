
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/20/2024 20:30:04
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
    /// 产品拆分单
    /// </summary>
    public partial class tb_ProdSplitServices : BaseServices<tb_ProdSplit>, Itb_ProdSplitServices
    {
        IMapper _mapper;
        public tb_ProdSplitServices(IMapper mapper, IBaseRepository<tb_ProdSplit> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}