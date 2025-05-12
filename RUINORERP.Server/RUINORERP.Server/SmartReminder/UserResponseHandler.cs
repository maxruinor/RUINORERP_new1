using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    public class UserResponseHandler
    {
        private readonly SmartReminderService _reminder;

        public UserResponseHandler(SmartReminderService reminder)
        {
            _reminder = reminder;
            SetupHandlers();
        }

        private void SetupHandlers()
        {
            //_reminder.OnUserResponse += async (request, response) =>
            //{
            //    if (request.Type == "OrderApproval")
            //    {
            //        var workflow = _workflowHost.GetWorkflowInstance(response.CorrelationId);
            //        await workflow.ResumeAsync(response);
            //    }
            //};
        }
    }
}
