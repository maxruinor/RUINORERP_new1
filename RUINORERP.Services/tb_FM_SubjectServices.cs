
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:39:08
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
    /// 会计科目表，财务系统中使用
    /// </summary>
    public partial class tb_FM_SubjectServices : BaseServices<tb_FM_Subject>, Itb_FM_SubjectServices
    {
        IMapper _mapper;
        public tb_FM_SubjectServices(IMapper mapper, IBaseRepository<tb_FM_Subject> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}