using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Task
    {
        public DateTime CreationTime { get; set; }
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TaskID { get; set; }
        public string AssigneeUser { get; set; }


        public Task() { }

        public Task(DateTime dueDate, string title, string description, int TaskID, DateTime creationTime, string assigneeUser)
        {
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.TaskID = TaskID;
            this.AssigneeUser = assigneeUser;
        }

        public Task(BusinessLayer.Task t)
        {
            DueDate = t.DueDate;
            Title = t.Title;
            Description = t.Description;
            TaskID = t.TaskID;
            CreationTime = t.CreationTime;
            AssigneeUser = t.AssigneeUser;
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
