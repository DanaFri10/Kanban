using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// Class Column represents a board column in the Kanban system.
    /// </summary>
    public class Column
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Dictionary<int, Task> Tasks { get; private set; }
        public int ColumnNumber { get; }
        public int TasksLimit { get => _tasksLimit;
            private set {
                ColumnDTO.TasksLimit = value;
                _tasksLimit = value;
            } 
        }
        private int _tasksLimit;
        public ColumnDTO ColumnDTO { get; }

        //MAGIC NUMBER
        private static readonly int UNLIMITED_TASKS = -1;


        /// <summary>
        /// Initializes a new instance of the Column class, with the given board id and column number.
        /// </summary>
        /// <param name="boardId">The id of the board.</param>
        /// <param name="columnNumber">The column's number.</param>
        /// <exception cref="ArgumentException">If the board id is not valid.</exception>
        public Column(int boardId, int columnNumber)
        {
            if (boardId<0)
            {
                log.Error("Error: The board id is invalid.");
                throw new ArgumentException("Error: The board id is invalid.");
            }

            Tasks = new Dictionary<int, Task>();
            ColumnNumber = columnNumber;
            _tasksLimit = UNLIMITED_TASKS; // Initializing tasks limit to -1 indicating there's currently no limit.
            ColumnDTO = new ColumnDTO(boardId, columnNumber);
            ColumnDTO.Insert();
            log.Debug($"Created column {columnNumber} to board {boardId}.");
        }

        /// <summary>
        /// Initializes a new instance of the Column class, according to the given ColumnDTO object.
        /// </summary>
        /// <param name="boardDTO">The given ColumnDTO object.</param>
        /// <exception cref="ArgumentNullException">If the given ColumnDTO object is null.</exception>
        public Column(ColumnDTO columnDTO)
        {
            if (columnDTO == null)
            {
                log.Error("Error: The columnDTO is null.");
                throw new ArgumentNullException("Error: The columnDTO is null.");
            }

            ColumnDTO = columnDTO;
            Tasks = new Dictionary<int, Task>();
            columnDTO.Tasks.ForEach(t => Tasks[t.TaskID] = new Task(t));
            ColumnNumber = columnDTO.ColumnNumber;
            _tasksLimit = columnDTO.TasksLimit;
            log.Debug($"Created column {ColumnNumber} to board {ColumnDTO.BoardID}.");
        }

        
        /// <summary>
        /// Limits the column to the given maximum tasks limit.
        /// </summary>
        /// <param name="tasksLimit">The required limit for the maximum number of tasks in this column.</param>
        /// <exception cref="ArgumentException">If the given tasks limit is invalid.</exception>
        /// <exception cref="Exception">If the column already contains a greater amount of tasks.</exception>
        public void LimitColumn(int tasksLimit)
        {
            if (tasksLimit <= 0 && TasksLimit != UNLIMITED_TASKS)
            {
                log.Error("Error: Invalid tasks limit: " + tasksLimit + ".");
                throw new ArgumentException("Error: Invalid tasks limit: " + tasksLimit + ".");
            }
            if (tasksLimit > 0 && Tasks.Count > tasksLimit)
            {
                log.Error("Error: Cannot limit the column with a tasks limit of " + tasksLimit +
                                    " since there are already " + Tasks.Count + " tasks in the column.");
                throw new Exception("Error: Cannot limit the column with a tasks limit of " + tasksLimit +
                                    " since there are already " + Tasks.Count + " tasks in the column.");
            }
            TasksLimit = tasksLimit;
            log.Debug($"Limit column {ColumnNumber} with limit of {tasksLimit}.");
        }

        /// <summary>
        /// Returns a list of all the Tasks in the column.
        /// </summary>
        /// <returns>A list of all the Tasks in the column.</returns>
        public List<Task> GetTasks()
        {
            return Tasks.Values.ToList();
        }

        /// <summary>
        /// Returns a list of all the tasks in the column that are assigned to the given user.
        /// </summary>
        /// <param name="userEmail">The board's member email.</param>
        /// <returns>A list of all the Tasks in the column that assignee to the user.</returns>
        /// 
        public List<Task> GetTasksOfAssignee(string userEmail)
        {
            List<Task> filteredTasks = Tasks.Values.ToList().Where((t) => t.IsAssignee(userEmail)).ToList();
            return filteredTasks;
        }

        /// <summary>
        /// Unassign all the tasks in the column that are assigned to the given user.
        /// </summary>
        /// <param name="userEmail">The board's member email.</param>
        public void UnassignTasks(string userEmail)
        {
            List<Task> filteredTasks = GetTasksOfAssignee(userEmail);
            filteredTasks.ForEach((t) => t.Unassign());
            log.Debug($"Unassign all Tasks in column {ColumnNumber} of user {userEmail}");
        }

        /// <summary>
        /// This method adds the given Task to the column, if possible.
        /// </summary>
        /// <param name="task">The Task object needed to be added to the column.</param>
        /// <exception cref="ArgumentNullException">Throws an exception if the given Task object is null.</exception>
        /// <exception cref="Exception">If the column already contains a Task with the given taskID,
        /// or if the column has already reached its maximum tasks limit.</exception>
        public void AddTask(Task task)
        {
            if (task == null)
            {
                log.Error("Error: Invalid task: null.");
                throw new ArgumentNullException("Error: Invalid task: null.");
            }
            if (TasksLimit != UNLIMITED_TASKS && Tasks.Count >= TasksLimit)
            {
                log.Error("Couldn't add task, column reached the tasks limit.");
                throw new Exception("Error: Couldn't add task, column reached the tasks limit.");
            }
            if (TaskExists(task.TaskID))
            {
                log.Error("Given task ID already exists in this column.");
                throw new Exception("Error: Given task ID already exists in this column.");
            }
            Tasks[task.TaskID] = task;
            ColumnDTO.Tasks.Add(task.TaskDTO);
            log.Debug($"add Task to column {ColumnNumber}");
        }
        
        /// <summary>
        /// Removes the task with the given task ID from the column, if exists.
        /// </summary>
        /// <param name="taskID">The unique ID of the task needed to be removed from the board.</param>
        /// <returns>The removed Task, if the removal succeeded.</returns>
        /// <exception cref="Exception">If the column doesn't have a task with the given task ID.</exception>
        public Task RemoveTask(int taskID)
        {
            if (!TaskExists(taskID))
            {
                log.Error("Column doesn't have a task with this task ID.");
                throw new Exception("Error: Column doesn't have a task with this task ID.");
            }
            Task task = Tasks[taskID];
            Tasks.Remove(taskID);
            ColumnDTO.Tasks.Remove(task.TaskDTO);
            log.Debug($"remove Task from column {ColumnNumber}");
            return task;
        }

        /// <summary>
        /// Returns the task with the given task ID in the column, if exists.
        /// </summary>
        /// <param name="taskID">The unique ID of the task.</param>
        /// <returns>The Task with the given task ID in the column, if exists.</returns>
        /// <exception cref="Exception">If the column doesn't have a task with the given task ID.</exception>
        public Task GetTask(int taskID)
        {
            if (!TaskExists(taskID))
                throw new Exception("Error: Column doesn't have a task with this task ID.");
            return Tasks[taskID];
        }

        /// <summary>
        /// Checks if there exists a task with the given task ID, in this column.
        /// </summary>
        /// <param name="taskID">The unique ID of the task.</param>
        /// <returns>Returns true if there exists a task with the given task ID in this column, false otherwise.</returns>
        internal bool TaskExists(int taskID)
        {
            return Tasks.ContainsKey(taskID);
        }
    }
}
