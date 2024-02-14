using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    internal class Tests
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Testing");
            ServiceFactory serviceFactory = new ServiceFactory();
            UserService userService = serviceFactory.UserService;
            BoardService boardService = serviceFactory.BoardService;
            TaskService taskService = serviceFactory.TaskService;
            serviceFactory.DeleteData();

            new TestUserService(userService).Test();
            new TestBoardService(boardService, userService).Test();
            new TestTaskService(boardService, userService, taskService).Test();
            
            serviceFactory.DeleteData();

            GradingService gradingService = new GradingService();
            new TestGradingService(gradingService, gradingService.UserService, gradingService.BoardService, gradingService.TaskService).Test();
        }

    }
}
