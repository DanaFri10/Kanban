using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTests
{
    public class Column
    {
        public Dictionary<int, Task> Tasks { get; }
        public int ColumnNumber { get; }
        public int TasksLimit { get; }

        public Column(Dictionary<int, Task> tasks, int columnNumber, int tasksLimit)
        {
            Tasks = tasks;
            ColumnNumber = columnNumber;
            TasksLimit = tasksLimit;
        }
    }
}
