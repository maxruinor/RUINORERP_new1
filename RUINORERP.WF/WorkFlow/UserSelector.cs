using RUINORERP.Model;
using RUINORERP.WF.WorkFlow.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.WorkFlow
{
    [UserSelectorAttribute("从所有用户选择")]
    public class UserSelector : IUserSelector
    {


        public List<tb_UserInfo> UserList { get; set; }


        //public List<long> GetSelections()
        //{
        //    return UserList.Select(u => new long[] { u.User_ID }).ToList();
        //}

        //public List<User> GetUsers(SelectorInput input)
        //{
        //    var result = new List<User>();
        //    switch (input.SelectionId)
        //    {
        //        default:
        //            result.Add(new User { Id = input.SelectionId, Name = UserList.GetUserById(input.SelectionId).Name });
        //            break;
        //    }
        //    return result;
        //}
    }
}
