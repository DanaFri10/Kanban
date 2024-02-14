using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Frontend.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        public int ColumnNumber { get; }

        private int _tasksLimit;
        public int TasksLimit
        {
            get => _tasksLimit;
            set
            {
                _tasksLimit = value;
                RaisePropertyChanged("TasksLimit");
            }
        }

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                RaisePropertyChanged("SelectedTask");
            }
        }

        public ObservableCollection<TaskModel> Tasks { get; set; }

        public ColumnModel(BackendController controller, int columnNumber, int tasksLimit, ObservableCollection<TaskModel> tasks) : base(controller)
        {
            ColumnNumber = columnNumber;
            TasksLimit = tasksLimit;
            Tasks = tasks;
        }
    }
}
