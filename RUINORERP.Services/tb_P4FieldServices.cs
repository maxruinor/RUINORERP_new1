
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:00
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
    /// 字段权限表
    /// </summary>
    public partial class tb_P4FieldServices : BaseServices<tb_P4Field>, Itb_P4FieldServices
    {
        IMapper _mapper;
        public tb_P4FieldServices(IMapper mapper, IBaseRepository<tb_P4Field> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}