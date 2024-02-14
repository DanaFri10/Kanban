using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Column
    {
        public Dictionary<int, Task> Tasks { get; set; }
        public int ColumnNumber { get; set; }
        public int TasksLimit { get; set; }

        public Column() { }

        public Column(Dictionary<int, Task> tasks, int columnNumber, int tasksLimit)
        {
            Tasks = tasks;
            ColumnNumber = columnNumber;
            TasksLimit = tasksLimit;
        }

        public Column(BusinessLayer.Column c)
        {
            ColumnNumber = c.ColumnNumber;
            TasksLimit = c.TasksLimit;
            Tasks = new Dictionary<int, Task>();
            c.Tasks.Values.ToList().ForEach(t => Tasks.Add(t.TaskID, new Task(t)));
        }
    }
}
