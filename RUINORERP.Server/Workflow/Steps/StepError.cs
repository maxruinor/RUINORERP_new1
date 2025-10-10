using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.Steps
{
    public class StepError
    {
        public WorkflowInstance Workflow { get; set; }
        public WorkflowStep Step { get; set; }
        public Exception Exception { get; set; }
    }
}
