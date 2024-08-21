
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2024 13:38:38
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
    /// 归还单，如果部分无法归还，则强制结案借出单。生成一个财务数据做记录。
    /// </summary>
    public partial class tb_ProdReturningServices : BaseServices<tb_ProdReturning>, Itb_ProdReturningServices
    {
        IMapper _mapper;
        public tb_ProdReturningServices(IMapper mapper, IBaseRepository<tb_ProdReturning> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}