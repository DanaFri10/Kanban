using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    /// <summary>
    /// TaskModel class represents a Task in the Kanban system.
    /// </summary>
    public class TaskModel : NotifiableModelObject
    {
        public int TaskID { get; }

        public DateTime CreationTime { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private string _assigneeUser;
        public string AssigneeUser
        {
            get => _assigneeUser;
            set
            {
                _assigneeUser = value;
                RaisePropertyChanged("AssigneeUser");
            }
        }

        /// <summary>
        /// Initializes a new instance of the TaskModel class, according to the given parameters.
        /// </summary>
        /// <param name="controller">The backend controller allowing each interaction with the backend service.</param>
        /// <param name="taskID">The ID of the task.</param>
        /// <param name="creationTime">The creation time of the class.</param>
        /// <param name="title">The task's title.</param>
        /// <param name="description">The task's description.</param>
        /// <param name="dueDate">The due date of the task.</param>
        /// <param name="assigneeUser">The assignee of the task.</param>
        public TaskModel(BackendController controller, int taskID, DateTime creationTime, string title, string description, DateTime dueDate, string assigneeUser) : base(controller)
        {
            TaskID = taskID;
            CreationTime = creationTime;
            Title = title;
            Description = description;
            DueDate = dueDate;
            AssigneeUser = assigneeUser;
        }
    }
}
