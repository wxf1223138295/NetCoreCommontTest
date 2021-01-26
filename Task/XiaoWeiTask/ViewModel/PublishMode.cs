using System;
using System.Collections.Generic;
using System.Text;

namespace XiaoWeiTask.ViewModel
{
    public class PublishMode
    {
        public int code { get; set; }
        public string eventid { get; set; }
        public string message { get; set; }
        public TaskObj tasks { get; set; }
        public int taskid { get; set; }
        public string projectcode { get; set; }
    }

    public class TaskObj
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        public int TaskVersion { get; set; }
        public string TaskName { get; set; }
    }
}
