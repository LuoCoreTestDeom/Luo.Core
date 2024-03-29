﻿using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using AutoMapper;
using Luo.Core.DatabaseEntity;
using SqlSugar;
using Org.BouncyCastle.Ocsp;

namespace Luo.Core.Repository
{
   

    public class MemberRepository : SqlSugarRepositoryList<ISqlSugarFactory>, IMemberRepository
    {
        public MemberRepository(ISqlSugarFactory factory, IMapper mapper) : base(factory, mapper)
        {
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public MemberInfoDto QueryMemberInfo(LoginMemberDto req)
        {
            MemberInfoDto res = new MemberInfoDto();
            Factory.GetDbContext((db) =>
            {

                res= db.Queryable<MemberInfo>()
                .Where(x => x.MemberName == req.MemberName && x.Password == req.MemberPassword)
                .Select(x => new MemberInfoDto
                {
                    MemberId=x.Id,
                    MemberName = x.MemberName
                })
                .First();
            });
            return res;
        }

        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonPageDto<List<MemberInfoListDto>> QueryMemberInfoPageList(QueryMemberInfoPageDto req)
        {
            CommonPageDto<List<MemberInfoListDto>> res = new CommonPageDto<List<MemberInfoListDto>>();
            Factory.GetDbContext((db) =>
            {
                int totalCount = 0;
                res.ResultData = db.Queryable<MemberInfo>()
                .WhereIF(!string.IsNullOrWhiteSpace(req.MemberName), x => x.MemberName == req.MemberName)
                .WhereIF(req.TimeEnable, x => SqlFunc.Between(x.CreateTime, req.TimeStart, req.TimeEnd))
                .Select(x => new MemberInfoListDto
                {
                    MemberId = x.Id,
                    MemberName = x.MemberName,
                    CreateTime = x.CreateTime,
                    CreateName = x.CreateName
                })
                .ToPageList(req.PageIndex, req.PageCount, ref totalCount);
                res.TotalCount = totalCount;
            });
            return res;
        }


        /// <summary>
        /// 添加会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto AddMemberInfo(AddMemberInfoDto req)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {
                res.Status = db.Insertable<MemberInfo>(new
                {
                    MemberName = req.MemberName,
                    Password = req.Password,
                    CreateName = req.CreateName,
                    CreateTime = DateTime.Now
                })
                .ExecuteCommand() > 0;
            });
            return res;
        }
        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto UpdateMemberInfo(UpdateMemberInfoDto req)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {
                
                res.Status = db.Updateable<MemberInfo>()
                .SetColumns(x=>x.MemberName==req.MemberName)
                .SetColumnsIF(!string.IsNullOrWhiteSpace(req.Password),x=>x.Password==req.Password)
                .Where(x=>x.Id==req.MemberId)
                .ExecuteCommand() > 0;
            });
            return res;
        }
        /// <summary>
        /// 删除会员通过ids
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto DeleteMemberByMemberIds(List<int> req) 
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {
                try
                {
                    db.BeginTran();
                    foreach (var item in req)
                    {
                        db.Deleteable<MemberInfo>().Where(x => x.Id == item).ExecuteCommand();
                    }
                    db.CommitTran();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    res.Message = ex.Message;
                }
                
            });
            return res;
        }

        /// <summary>
        /// 获取会员密码 通过 会员ID
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public string GetMemberPasswordByMemberId(int memberId) 
        {
            string res = string.Empty;
            Factory.GetDbContext((db) =>
            {
               var memberInfo= db.Queryable<MemberInfo>().Where(x => x.Id == memberId).First();
                res = memberInfo.Password;
            });
            return res;
        }
    }
}
