using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTests
{
    public class Task
    {
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; }
        public string Title { get; }
        public string Description { get; }
        public int TaskID { get; }
        public string AssigneeUser { get; }

        public Task(DateTime creationTime, DateTime dueDate, string title, string description, int taskID, string assigneeUser)
        {
            CreationTime = creationTime;
            DueDate = dueDate;
            Title = title;
            Description = description;
            TaskID = taskID;
            AssigneeUser = assigneeUser;
        }

        public override bool Equals(Object o)
        {
            if ((o == null) || !this.GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                Task task = (Task)o;
                return CreationTime == task.CreationTime && DueDate == task.DueDate && Title == task.Title
                    && Description == task.Description && TaskID == task.TaskID && AssigneeUser == task.AssigneeUser;
            }
        }
    }
}
