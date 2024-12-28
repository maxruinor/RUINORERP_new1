
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/28/2024 15:53:20
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
    /// 全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度
    /// </summary>
    public partial class tb_gl_CommentServices : BaseServices<tb_gl_Comment>, Itb_gl_CommentServices
    {
        IMapper _mapper;
        public tb_gl_CommentServices(IMapper mapper, IBaseRepository<tb_gl_Comment> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}