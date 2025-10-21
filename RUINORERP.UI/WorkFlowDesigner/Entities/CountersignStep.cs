using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.UI.WorkFlowDesigner.Entities
{
    /// <summary>
    /// 会签步骤实体类 - 需要所有审批人都通过才能继续流程
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CountersignStep
    {
        private string _id;
        private string _name;
        private string _nextStepId;
        private List<ApprovalUser> _approvers;
        private int _approvedCount;
        private DateTime? _createTime;
        private DateTime? _completeTime;
        private string _status; // Pending, Approved, Rejected

        public CountersignStep()
        {
            _id = Guid.NewGuid().ToString();
            _approvers = new List<ApprovalUser>();
            _status = "Pending";
        }

        [JsonProperty("Id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonProperty("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [JsonProperty("NextStepId")]
        public string NextStepId
        {
            get { return _nextStepId; }
            set { _nextStepId = value; }
        }

        [JsonProperty("Approvers")]
        public List<ApprovalUser> Approvers
        {
            get { return _approvers; }
            set { _approvers = value; }
        }

        [JsonProperty("ApprovedCount")]
        public int ApprovedCount
        {
            get { return _approvedCount; }
            set { _approvedCount = value; }
        }

        [JsonProperty("CreateTime")]
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        [JsonProperty("CompleteTime")]
        public DateTime? CompleteTime
        {
            get { return _completeTime; }
            set { _completeTime = value; }
        }

        [JsonProperty("Status")]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 检查是否所有审批人都已审批通过
        /// </summary>
        /// <returns></returns>
        public bool IsFullyApproved()
        {
            return _approvers.Count > 0 && _approvedCount >= _approvers.Count;
        }

        /// <summary>
        /// 添加审批人
        /// </summary>
        /// <param name="user"></param>
        public void AddApprover(ApprovalUser user)
        {
            if (!_approvers.Exists(u => u.Id == user.Id))
            {
                _approvers.Add(user);
            }
        }

        /// <summary>
        /// 移除审批人
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveApprover(int userId)
        {
            _approvers.RemoveAll(u => u.Id == userId);
            if (_approvedCount > _approvers.Count)
            {
                _approvedCount = _approvers.Count;
            }
        }

        /// <summary>
        /// 审批通过
        /// </summary>
        public void Approve()
        {
            if (_approvedCount < _approvers.Count)
            {
                _approvedCount++;
                if (IsFullyApproved())
                {
                    _status = "Approved";
                    _completeTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 审批拒绝
        /// </summary>
        public void Reject()
        {
            _status = "Rejected";
            _completeTime = DateTime.Now;
        }
    }
}