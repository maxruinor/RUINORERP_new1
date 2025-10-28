
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/28/2025 17:14:14
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
    /// 文件业务关联表
    /// </summary>
    public partial class tb_FS_BusinessRelationServices : BaseServices<tb_FS_BusinessRelation>, Itb_FS_BusinessRelationServices
    {
        IMapper _mapper;
        public tb_FS_BusinessRelationServices(IMapper mapper, IBaseRepository<tb_FS_BusinessRelation> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}