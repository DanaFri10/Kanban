using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTests
{
    public class GradingTask
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }


        public GradingTask() { }

        public GradingTask(Task task)
        {
            Id = task.TaskID;
            CreationTime = task.CreationTime;
            Title = task.Title;
            Description = task.Description;
            DueDate = task.DueDate;
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                GradingTask task = (GradingTask)obj;
                return CreationTime == task.CreationTime && DueDate == task.DueDate && Title == task.Title
                    && Description == task.Description && Id == task.Id;
            }
        }
    }
}
