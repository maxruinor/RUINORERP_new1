
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:42
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单
    /// </summary>
    public partial class tb_FM_OtherExpenseDetailServices : BaseServices<tb_FM_OtherExpenseDetail>, Itb_FM_OtherExpenseDetailServices
    {
        IMapper _mapper;
        public tb_FM_OtherExpenseDetailServices(IMapper mapper, IBaseRepository<tb_FM_OtherExpenseDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}