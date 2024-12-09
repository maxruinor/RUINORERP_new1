
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:42
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
    /// 协作人记录表-记录内部人员介绍客户的情况
    /// </summary>
    public partial class tb_CRM_CollaboratorServices : BaseServices<tb_CRM_Collaborator>, Itb_CRM_CollaboratorServices
    {
        IMapper _mapper;
        public tb_CRM_CollaboratorServices(IMapper mapper, IBaseRepository<tb_CRM_Collaborator> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}