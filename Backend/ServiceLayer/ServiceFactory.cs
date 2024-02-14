using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        public UserService UserService { get; }
        public BoardService BoardService { get; }
        public TaskService TaskService { get; }

        private BoardController _boardController;
        private UserController _userController;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ServiceFactory()
        {
            _boardController = new BoardController();
            _userController = new UserController();
            UserService = new UserService(_userController);
            BoardService = new BoardService(_boardController,_userController);
            TaskService = new TaskService(_boardController,_userController);

            // Load logging configuration and starting the backend log.
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log.");
        }

        /// <summary>
        /// This method loads all the persisted data from the database, in the initial load of the system.
        /// </summary>
        /// <returns>Returns a Json representation of a response containing an error message, if occured.</returns>
        public string LoadData()
        {
            try
            {
                _boardController.LoadData(); 
                _userController.LoadData();
                return new Response<object>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<object>(ex.Message).ToJson();
            }
        }

        /// <summary>
        /// This method deletes all the data from the system and from the database.
        /// </summary>
        /// <returns>Returns a Json representation of a response containing an error message, if occured.</returns>
        public string DeleteData()
        {
            try
            {
                _boardController.DeleteData(); 
                _userController.DeleteData();
                return new Response<object>().ToJson();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Response<object>(ex.Message).ToJson();
            }
        }
    }
}
