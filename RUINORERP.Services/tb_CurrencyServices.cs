
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:56:54
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
    /// 币别资料表-备份第一行数据后删除重建 如果不行则直接修改字段删除字段
    /// </summary>
    public partial class tb_CurrencyServices : BaseServices<tb_Currency>, Itb_CurrencyServices
    {
        IMapper _mapper;
        public tb_CurrencyServices(IMapper mapper, IBaseRepository<tb_Currency> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}