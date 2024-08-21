
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/20/2024 20:30:05
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
    /// 拆分单明细
    /// </summary>
    public partial class tb_ProdSplitDetailServices : BaseServices<tb_ProdSplitDetail>, Itb_ProdSplitDetailServices
    {
        IMapper _mapper;
        public tb_ProdSplitDetailServices(IMapper mapper, IBaseRepository<tb_ProdSplitDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}