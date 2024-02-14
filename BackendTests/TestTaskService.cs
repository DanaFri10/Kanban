using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    internal class TestTaskService
    {
        UserService _userService;
        BoardService _boardService;
        TaskService _taskService;

        public TestTaskService(BoardService boardService, UserService userService, TaskService taskService)
        {
            _boardService = boardService;
            _userService = userService;
            _taskService = taskService;
        }
        internal void Test()
        {
            Console.WriteLine("\nTesting TaskService:");
            TestCreateTask();
            TestGetTask();
            TestMoveTask();
            TestListInProgressTasks();
            TestEditTask();
            TestAssignTask();
        }


        /// <summary>
        /// This function tests requirement 12.
        /// The function attempts to create a new task with valid and invalid parameters,
        /// and checks for the expected errors and the validity of the task's values.
        /// </summary>
        private void TestCreateTask()
        {
            Console.WriteLine("\nTesting CreateTask():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            DateTime dueDate = new DateTime(2022, 7, 1);
            string title = "calculus homework";
            string description = "Second question in page 123";
            DateTime dueDate1 = new DateTime(2022, 8, 1);
            string title1 = "clean apartment";
            string description1 = "Vacuum and wash floors";

            //A user that does not exist should not be able to create a task
            Response response1 = Response.FromJson(_taskService.CreateTask(testEmail4, boardName2, dueDate, title, description));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a task for a user that does not exist.");

            //A user that is not logged in should not be able to create a task
            Response response2 = Response.FromJson(_taskService.CreateTask(testEmail3, boardName2, dueDate, title, description));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a task for a user the is not logged in.");

            //A user should not be a able to create a task in a board that does not exist
            Response response3 = Response.FromJson(_taskService.CreateTask(testEmail1, boardName3, dueDate, title, description));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a task in a board that does not exist.");

            ////A user should not be a able to create a task in a board that belongs to another user
            //Response response4 = Response.FromJson(_taskService.CreateTask(testEmail2, boardName2, dueDate, title, description));
            //if (response4.ErrorOccured)
            //    Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            //else
            //    Console.WriteLine("Test failed: Expected an error after creating a task in a board that do not belong to the user.");

            //Creating a task
            Response response5 = Response.FromJson(_taskService.CreateTask(testEmail1, boardName1, dueDate, title, description));
            if (response5.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating task. " + response5.ErrorMessage);
            else
            {
                Task task = response5.DeserializeReturnValue<Task>();
                Console.WriteLine(CheckTask(task, dueDate, title, description));
            }

            //Creating a task with the same title
            Response response6 = Response.FromJson(_taskService.CreateTask(testEmail1, boardName1, dueDate, title, description));
            if (response6.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating task. " + response6.ErrorMessage);
            else
            {
                Task task = response6.DeserializeReturnValue<Task>();
                Console.WriteLine(CheckTask(task, dueDate, title, description));
            }

            //Creating a task in a different board
            Response response7 = Response.FromJson(_taskService.CreateTask(testEmail1, boardName2, dueDate1, title1, description1));
            if (response7.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating task. " + response7.ErrorMessage);
            else
            {
                Task task = response7.DeserializeReturnValue<Task>();
                Console.WriteLine(CheckTask(task, dueDate1, title1, description1));
            }
        }

        /// <summary>
        /// This function checks if a task is valid, using a helper function.
        /// </summary>
        private string CheckTask(int taskId, string testEmail, string boardName, DateTime dueDate, string title, string description)
        {
            Response response = Response.FromJson(_taskService.GetTask(testEmail, boardName, taskId));
            if (response.ErrorOccured)
                return "Test failed: " + response.ErrorMessage;
            else
            {
                Task task = response.DeserializeReturnValue<Task>();
                return CheckTask(task, dueDate, title, description);
            }
        }

        // This function checks if the given task is valid, according to the given parameters.
        private string CheckTask(Task task, DateTime dueDate, string title, string description)
        {
            if (task.DueDate != dueDate)
                return "Test failed: Did not return the expected task due date.";
            if (task.Title != title)
                return "Test failed: Did not return the expected task title.";
            if (task.Description != description)
                return "Test failed: Did not return the expected task description.";
            return "Test successful: The task is valid";
        }

        /// The function attempts to get a task with valid and invalid parameters,
        /// and checks for the expected errors and the validity of the task's values.
        private void TestGetTask()
        {
            Console.WriteLine("\nTesting GetTask():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string boardName1 = "homework";
            string boardName2 = "chores";
            DateTime dueDate = new DateTime(2022, 7, 1);
            string title = "calculus homework";
            string description = "Second question in page 123";
            DateTime dueDate1 = new DateTime(2022, 8, 1);
            string title1 = "clean apartment";
            string description1 = "Vacuum and wash floors";

            Console.WriteLine(CheckTask(2, testEmail1, boardName1, dueDate, title, description));
            Console.WriteLine(CheckTask(3, testEmail1, boardName2, dueDate1, title1, description1));
        }

        /// <summary>
        /// This function tests requirement 13.
        /// The function attempts to move a task between board columns, with valid
        /// and invalid parameters, and checks for the expected errors and the necessary updates.
        /// </summary>
        private void TestMoveTask()
        {
            Console.WriteLine("\nTesting MoveTask():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework (tasks 0,1)", "chores"(task 2)
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            int columnNumber1 = 0;
            int columnNumber2 = 1;
            int columnNumber3 = 2;
            int columnNumber4 = -6;
            int taskId1 = 1; //task exists
            int taskId2 = 2; //task exists
            int taskId3 = 3; //task exists
            int taskId4 = 10; //task does not exist

            //A user that does not exist should not be able to move a task
            Response response1 = Response.FromJson(_taskService.MoveTask(testEmail4, boardName1, columnNumber1, taskId1));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after moving a task of a user that does not exist.");

            //A user that is not logged in should not be able to move a task
            Response response2 = Response.FromJson(_taskService.MoveTask(testEmail3, boardName1, columnNumber1, taskId1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after moving a task of a user that is not logged in.");

            //A user should not be able to move a task in a board that does not exist
            Response response3 = Response.FromJson(_taskService.MoveTask(testEmail1, boardName3, columnNumber1, taskId1));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after moving a task in a board that does not exist.");

            //A user should not be able to move a task that does not exist
            Response response4 = Response.FromJson(_taskService.MoveTask(testEmail1, boardName1, columnNumber1, taskId4));
            if (response4.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after moving a task that does not exist.");

            //A user should not be able to move a task that is not in the right column
            Response response5 = Response.FromJson(_taskService.MoveTask(testEmail1, boardName1, columnNumber2, taskId1));
            if (response5.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after moving a task that is not in the right column.");

            //A user should not be able to move a task that does not belong the the board
            Response response6 = Response.FromJson(_taskService.MoveTask(testEmail1, boardName2, columnNumber1, taskId2));
            if (response6.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after moving a task that does not belong to the board.");

            // TODO: Refactor
            _taskService.AssignTask(testEmail1, boardName1, taskId1, testEmail1);

            //Moving a task
            Response response7 = Response.FromJson(_taskService.MoveTask(testEmail1, boardName1, columnNumber1, taskId1));
            if (response7.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while moving task. " + response7.ErrorMessage);
            else
                Console.WriteLine(IsTaskMoved(testEmail1, boardName1, columnNumber1, taskId1));

            //Moving a task
            Response response8 = Response.FromJson(_taskService.MoveTask(testEmail1, boardName1, columnNumber2, taskId1));
            if (response8.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while moving task. " + response8.ErrorMessage);
            else
                Console.WriteLine(IsTaskMoved(testEmail1, boardName1, columnNumber2, taskId1));

            //Can not move a task from the "done" column
            Response response9 = Response.FromJson(_taskService.MoveTask(testEmail1, boardName1, columnNumber3, taskId1));
            if (response9.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response9.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error - can't move a task from the last column.");

            //Can not move a task from an invalid column number
            Response response10 = Response.FromJson(_taskService.MoveTask(testEmail1, boardName1, columnNumber4, taskId1));
            if (response10.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response10.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error - can't move a task from an invalid column.");
        }

        /// <summary>
        /// This function checks if a task has been moved correctly
        /// </summary>
        private string IsTaskMoved(string testEmail, string boardName, int columnNumber, int taskId)
        {
            Response response = Response.FromJson(_boardService.GetBoard(testEmail, boardName));
            if (response.ErrorOccured)
                return "Test failed." + response.ErrorMessage;
            else
            {
                Board board = response.DeserializeReturnValue<Board>();
                if (!board.Columns[columnNumber + 1].Tasks.ContainsKey(taskId))
                    return "Test failed: Task has not been moved correctly.";
                return "Test successful: Task has been moved correctly.";
            }
        }

        /// <summary>
        /// This function tests requirement 16.
        /// The function attempts to get a list of the user's in progress tasks, 
        /// with valid and invalid parameters, and checks for the expected errors, 
        /// and the validity of the returned list.
        /// </summary>
        private void TestListInProgressTasks()
        {
            Console.WriteLine("\nTesting ListInProgressTasks()");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";

            //A user that does not exist should not be able to list tasks in progress
            Response response1 = Response.FromJson(_taskService.ListInProgressTasks(testEmail4));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to list tasks in progress
            Response response2 = Response.FromJson(_taskService.ListInProgressTasks(testEmail3));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since user is not logged in.");

            // TODO: Refactor
            _taskService.AssignTask(testEmail1, boardName1, 2, testEmail1);
            _taskService.AssignTask(testEmail1, boardName2, 3, testEmail1);

            //Listing the tasks before moving them to In Progress column
            Response getTask1 = Response.FromJson(_taskService.GetTask(testEmail1, boardName1, 2));
            if (getTask1.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured when attempted to get a valid task.");
            else
            {
                Task task1 = getTask1.DeserializeReturnValue<Task>();
                Response getTask2 = Response.FromJson(_taskService.GetTask(testEmail1, boardName2, 3));
                if (getTask2.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured when attempted to get a valid task.");
                Task task2 = getTask2.DeserializeReturnValue<Task>();
                List<Task> expected = new List<Task>();
                expected.Add(task1);
                expected.Add(task2);
            
                // Move the tasks to In Progress columns
                _taskService.MoveTask(testEmail1, boardName1, 0, 2);
                _taskService.MoveTask(testEmail1, boardName2, 0, 3);

                Response response3 = Response.FromJson(_taskService.ListInProgressTasks(testEmail1));
                if (response3.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured while creating list of tasks in progress." + response3.ErrorMessage);
                else
                    Console.WriteLine(CheckInProgressList(testEmail1, expected));
            }
        }

        /// <summary>
        /// This function checks if a list of tasks in progress is valid
        /// </summary>
        private string CheckInProgressList(string testEmail, List<Task> expected)
        {
            Response response = Response.FromJson(_taskService.ListInProgressTasks(testEmail));
            if (response.ErrorOccured)
                return "Test failed." + response.ErrorMessage;
            else
            {
                List<Task> inProgressList = response.DeserializeReturnValue<List<Task>>();
                if (!(inProgressList.All(expected.Contains) && expected.All(inProgressList.Contains)))
                    return "Test failed: Expected list was not returned";
            }
            return "Test successful: The list is valid";
        }

        /// <summary>
        /// This function tests requirement 14, 15.
        /// The function attempts to edit tasks with valid and invalid parameters,
        /// and checks for the expected errors and the validity of the task's values.
        /// </summary>
        private void TestEditTask()
        {
            Console.WriteLine("\nTesting EditTask():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework (calculus homework task(taskId=0, column "done"), calculus homework task(taskId=1, column "backlog"))", "chores"
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            int taskId1 = 1;
            int taskId2 = 10;
            int taskId3 = 2;
            DateTime dueDate = new DateTime(2022, 10, 12);
            string title = "introSE homework";
            string description = "Milestone 1";

            //A user that does not exist should not be able to edit a task
            Response response1 = Response.FromJson(_taskService.EditTask(testEmail4, boardName1, taskId3, dueDate, title, description));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after editing a task when user does not exist.");

            //A user that is not logged in should not be able to edit a task
            Response response2 = Response.FromJson(_taskService.EditTask(testEmail3, boardName1, taskId3, dueDate, title, description));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after editing a task when user is not logged in.");

            //A user should not be able to edit a task in a board that does not exist
            Response response3 = Response.FromJson(_taskService.EditTask(testEmail1, boardName3, taskId3, dueDate, title, description));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after editing a task when board does not exist.");

            //A user should not be able to edit a task that does not exist
            Response response4 = Response.FromJson(_taskService.EditTask(testEmail1, boardName1, taskId2, dueDate, title, description));
            if (response4.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after editing a task when task does not exist.");

            //A user should not be able to edit a task that is in the "done" column
            Response response5 = Response.FromJson(_taskService.EditTask(testEmail1, boardName1, taskId1, dueDate, title, description));
            if (response5.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after editing a task that is done (column number 3).");

            //A task that belongs to a different board should not be moved
            Response response6 = Response.FromJson(_taskService.EditTask(testEmail1, boardName2, taskId3, dueDate, title, description));
            if (response6.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after editing a task that belongs to a different board.");

            //Editing a task
            Response response7 = Response.FromJson(_taskService.EditTask(testEmail1, boardName1, taskId3, dueDate, title, description));
            if (response7.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while editing task. " + response7.ErrorMessage);
            else
                Console.WriteLine(CheckTask(taskId3, testEmail1, boardName1, dueDate, title, description));

            //A user should not be able to edit a task that is in the "done" column
            Response response8 = Response.FromJson(_taskService.EditTask(testEmail1, boardName1, taskId1, dueDate, title, description));
            if (response8.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response8.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after editing a task that is done (column number 3).");
        }


        /// <summary>
        /// This function tests requirement 23.
        /// The function attempts to edit tasks with valid and invalid parameters,
        /// and checks for the expected errors and the validity of the task's values.
        /// </summary>
        private void TestAssignTask()
        {
            Console.WriteLine("\nTesting AssignTask():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework (calculus homework task(taskId=1, column "done"), calculus homework task(taskId=2, column "in progress"))", "chores task(taskId=3, column "backlog")"
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            int taskId1 = 1;
            int taskId2 = 10;
            int taskId3 = 2;
            int taskId4 = 3;
            int boardID1 = 4;
            int boardID2 = 5;

            //_boardService.JoinBoard(testEmail2, boardID2);

            //A user that does not exist should not be able to assign a task
            Response response1 = Response.FromJson(_taskService.AssignTask(testEmail4, boardName2, taskId4, testEmail4));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to assign a task when user does not exist.");

            //A user that is not logged in should not be able to assign a task
            Response response2 = Response.FromJson(_taskService.AssignTask(testEmail3, boardName2, taskId4, testEmail3));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to assign a task when user is not logged in.");

            //A user should not be able to assign a task in a board that does not exist
            Response response3 = Response.FromJson(_taskService.AssignTask(testEmail1, boardName3, taskId4, testEmail1));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to assign a task when board does not exist.");

            //A user should not be able to assign a task that does not exist
            Response response4 = Response.FromJson(_taskService.AssignTask(testEmail1, boardName1, taskId2, testEmail1));
            if (response4.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to assign a task when task does not exist.");

            //Assigning a task
            Response response7 = Response.FromJson(_taskService.AssignTask(testEmail1, boardName2, taskId4, testEmail1));
            if (response7.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while assigning a task. " + response7.ErrorMessage);
            else
                Console.WriteLine(CheckTaskAssignee(taskId3, testEmail1, boardName1, testEmail1));

            // A user should not be able to assign a task to a user that is not a board member
            Response response8 = Response.FromJson(_taskService.AssignTask(testEmail1, boardName1, taskId3, testEmail2));
            if (response8.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response8.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to assign a task when task does not exist.");

            // A user should not be able to assign a task that is assigned by someone else
            Response response9 = Response.FromJson(_taskService.AssignTask(testEmail2, boardName2, taskId4, testEmail2));
            if (response9.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response9.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to assign that is assigned by someone else.");

            // Assigning a task to a board member
            Response response10 = Response.FromJson(_taskService.AssignTask(testEmail1, boardName2, taskId4, testEmail2));
            if (response10.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while assigning a task. " + response10.ErrorMessage);
            else
                Console.WriteLine(CheckTaskAssignee(taskId4, testEmail1, boardName2, testEmail2));
        }

        /// <summary>
        /// This function checks if a task has the correct task assignee, using a helper function.
        /// </summary>
        private string CheckTaskAssignee(int taskId, string testEmail, string boardName, string assigneeUser)
        {
            Response response = Response.FromJson(_taskService.GetTask(testEmail, boardName, taskId));
            if (response.ErrorOccured)
                return "Test failed: " + response.ErrorMessage;
            else
            {
                Task task = response.DeserializeReturnValue<Task>();
                if (task.AssigneeUser != assigneeUser)
                    return "Test failed: Task assignee was not updated correctly.";
                return "Test successful: The task is valid";
            }
        }
    }
}
