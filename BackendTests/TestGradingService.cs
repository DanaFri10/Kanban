using IntroSE.Kanban.Backend.ServiceLayer;
using Task = IntroSE.Kanban.BackendTests.Task;
using GradingTask = IntroSE.Kanban.BackendTests.GradingTask;

namespace IntroSE.Kanban.BackendTests
{
    internal class TestGradingService
    {
        private GradingService _gradingService;
        private UserService _userService;
        private BoardService _boardService;
        private TaskService _taskService;

        public TestGradingService(GradingService gradingService, UserService userService, BoardService boardService, TaskService taskService)
        {
            _gradingService = gradingService;
            _userService = userService;
            _boardService = boardService;
            _taskService = taskService;
        }

        internal void Test()
        {
            Console.WriteLine("Testing GradingService:");
            Console.WriteLine("Testing DeleteData():");
            _gradingService.DeleteData();

            //Console.WriteLine("Testing LoadData():");
            //_gradingService.LoadData();
            //LoginUsers();

            TestRegister();
            TestLogin();
            TestLogout();
            TestAddBoard();
            TestJoinBoard(); //
            TestLeaveBoard(); //
            TestRemoveBoard();
            TestGetBoardName(); //
            TestGetUserBoards(); //
            TestLimitColumn();
            TestGetColumnLimit();
            TestGetColumnName();
            TestTransferOwnership(); //
            TestAddTask();
            TestAdvanceTask();
            TestInProgressTasks();
            TestGetColumn();
            TestUpdateTaskDueDate();
            TestUpdateTaskTitle();
            TestUpdateTaskDescription();
            TestAssignTask(); //
        }

        /// <summary>
        /// GradingService.Register(string email, string password):
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>

        private void TestRegister()
        {
            Console.WriteLine("Testing Register():");
            // Valid email addresses:
            string testEmail1 = "galpinto@gmail.com";
            string testEmail2 = "gal@gmail.com";
            string testEmail3 = "testing@tests.com";
            // Invalid email addresses:
            string testEmail4 = null;
            string testEmail5 = "test";
            string testEmail6 = "test @gmail.com";
            string testEmail7 = "test@gm";
            string testEmail8 = "test@gmail.com@";
            // Valid passwords:
            string testPassword2 = "Abc123456";
            string testPassword3 = "Aa123456789123456789"; // length of 20
            // Invalid passwords:
            string testPassword1 = "1235";
            string testPassword4 = null;
            string testPassword5 = "A123456";               // no lowercase letter
            string testPassword6 = "a123456";               // no uppercase letter
            string testPassword7 = "Aa123";                 // length of 5, too short
            string testPassword8 = "Aa1234567891234567890"; // length of 21, too long


            // Should return an error when creating a user with invalid password.
            string json1 = _gradingService.Register(testEmail1, testPassword1);
            GradingResponse<object> response = GradingResponse<object>.FromJson(json1);
            if (json1 != "{}" && response.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should not return an error when creating a valid user.
            string json2 = _gradingService.Register(testEmail1, testPassword2);
            if (json2 != "{}")
            {
                GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
                Console.WriteLine("Test failed: An error has occured while creating user. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected to receive an error message but received null.");
            }
            else
            {
                // The created user should exist in the system, have the same email address as the input,
                // and be logged in to the system.
                Console.WriteLine("User has been created. Checking user credentials:");
                Console.WriteLine(CheckUser(testEmail1, false, true));
            }

            // Should return an error when creating another user with the same email address.
            string json3 = _gradingService.Register(testEmail1, testPassword2);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                if (response3.ErrorMessage != null)
                    Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
                else
                    Console.WriteLine("Test failed: Expected to receive an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after creating another user with the same email address.");

            // Should not return an error when creating a valid user.
            string json4 = _gradingService.Register(testEmail2, testPassword2);
            if (json4 != "{}")
            {
                GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
                Console.WriteLine("Test failed: An error has occured while creating user. " + response4.ErrorMessage);
                if (response4.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected to receive an error message but received null.");
            }
            else
            {
                // The created user should exist in the system, have the same email address as the input,
                // and be logged in to the system.
                Console.WriteLine("User has been created. Checking user credentials:");
                Console.WriteLine(CheckUser(testEmail2, false, true));
            }

            // Should return an error when creating a user with invalid password.
            string json5 = _gradingService.Register(testEmail3, testPassword4);
            GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
            if (json5 != "{}" && response5.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid password.
            string json6 = _gradingService.Register(testEmail3, testPassword5);
            GradingResponse<object> response6 = GradingResponse<object>.FromJson(json6);
            if (json6 != "{}" && response6.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid password.
            string json7 = _gradingService.Register(testEmail3, testPassword6);
            GradingResponse<object> response7 = GradingResponse<object>.FromJson(json7);
            if (json7 != "{}" && response7.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response7.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid password.
            string json8 = _gradingService.Register(testEmail3, testPassword7);
            GradingResponse<object> response8 = GradingResponse<object>.FromJson(json8);
            if (json8 != "{}" && response8.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response8.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid password.
            string json9 = _gradingService.Register(testEmail3, testPassword8);
            GradingResponse<object> response9 = GradingResponse<object>.FromJson(json9);
            if (json9 != "{}" && response9.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response9.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid email address.
            string json10 = _gradingService.Register(testEmail4, testPassword2);
            GradingResponse<object> response10 = GradingResponse<object>.FromJson(json10);
            if (json10 != "{}" && response10.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response10.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should return an error when creating a user with invalid email address.
            string json11 = _gradingService.Register(testEmail5, testPassword2);
            GradingResponse<object> response11 = GradingResponse<object>.FromJson(json11);
            if (json11 != "{}" && response11.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response11.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should return an error when creating a user with invalid email address.
            string json12 = _gradingService.Register(testEmail6, testPassword2);
            GradingResponse<object> response12 = GradingResponse<object>.FromJson(json12);
            if (json12 != "{}" && response12.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response12.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should return an error when creating a user with invalid email address.
            string json13 = _gradingService.Register(testEmail7, testPassword2);
            GradingResponse<object> response13 = GradingResponse<object>.FromJson(json13);
            if (json13 != "{}" && response13.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response13.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should return an error when creating a user with invalid email address.
            string json14 = _gradingService.Register(testEmail8, testPassword2);
            GradingResponse<object> response14 = GradingResponse<object>.FromJson(json14);
            if (json14 != "{}" && response14.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response14.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should not return an error when creating a valid user.
            string json15 = _gradingService.Register(testEmail3, testPassword3);
            if (json15 != "{}")
            {
                GradingResponse<object> response15 = GradingResponse<object>.FromJson(json15);
                Console.WriteLine("Test failed: An error has occured while creating user. " + response15.ErrorMessage);
                if (response15.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected to receive an error message but received null.");
            }
            else
            {
                // The created user should exist in the system, have the same email address as the input,
                // and be logged-out of the system.
                Console.WriteLine("User has been created. Checking user credentials:");
                Console.WriteLine(CheckUser(testEmail3, false, true));
            }
        }

        private string CheckUser(string expectedEmail, bool expectedError, bool expectedLoggedIn)
        {
            Response checkUser = Response.FromJson(_userService.GetUser(expectedEmail));
            if (checkUser.ErrorOccured)
                if (expectedError)
                    return "Test successful: Expected error has occured. " + checkUser.ErrorMessage;
                else
                    return "Test failed: An error has occured while getting a user. " + checkUser.ErrorMessage;
            User returnedUser = checkUser.DeserializeReturnValue<User>();
            if (returnedUser.EmailAddress != expectedEmail)
                return "Test failed: Expected the user email to be " + expectedEmail + ".";
            if (returnedUser.IsLoggedIn != expectedLoggedIn)
                return "Test failed: Expected the user.isLoggedIn to be " + expectedLoggedIn + ".";
            return "Test successful: The user returned as expected.";
        }

        /// <summary>
        ///  GradingService.Login(string email, string password):
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>Response with user email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestLogin()
        {
            Console.WriteLine("\nLogging out of the registered users:");
            try
            {
                string[] emails = { "galpinto@gmail.com", "gal@gmail.com", "testing@tests.com" };
                foreach (string email in emails)
                {
                    _gradingService.Logout(email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed: An error has occured after logging out of newly registered users.");
            }

            Console.WriteLine("\nTesting Login():");
            string testEmail1 = "test@test.com"; //"galpinto@gmail.com";
            string testEmail2 = "gal@gmail.com";
            string testPassword1 = "12345678";
            string testPassword2 = "Abc123456";

            // Should return an error when attempting to login into a non-existent user.
            GradingResponse<string> response = GradingResponse<string>.FromJson(_gradingService.Login(testEmail1, testPassword1));
            if (response.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to login into a non-existent user.");

            // Should return an error when attempting to login with incorrect password.
            GradingResponse<string> response2 = GradingResponse<string>.FromJson(_gradingService.Login(testEmail2, testPassword1));
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to login with incorrect password.");

            // Should not return an error when logging in to a user with valid email and password.
            GradingResponse<string> response3 = GradingResponse<string>.FromJson(_gradingService.Login(testEmail2, testPassword2));
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while logging in to user. " + response3.ErrorMessage);
            else
            {
                // The user should be logged in to the system.
                Console.WriteLine("Logged in to user. Checking user update:");
                Console.WriteLine(CheckUser(testEmail2, false, true));

                // The response should return the user email.
                string returnedEmail = response3.ReturnValue;
                if (returnedEmail == testEmail2)
                    Console.WriteLine("Test successful: Login(..) returned the user email successfully.");
                else
                    Console.WriteLine("Test failed: Expected Login(..) to return the user email after logging in.");
            }

            // Should return an error when attempting to login into a logged in user.
            GradingResponse<string> response4 = GradingResponse<string>.FromJson(_gradingService.Login(testEmail2, testPassword2));
            if (response4.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to login into a logged in user.");
        }

        /// <summary>
        /// GradingService.Logout(string email):
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>

        private void TestLogout()
        {
            Console.WriteLine("\nTesting Logout():");
            string testEmail1 = "test@gmail.com";
            string testEmail2 = "gal@gmail.com";

            // Should return an error when attempting to log out from a non-existent user.
            GradingResponse<object> response = GradingResponse<object>.FromJson(_gradingService.Logout(testEmail1));
            if (response.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to log-out from a non-existent user.");

            // Should not return an error when logging out from a valid user.
            GradingResponse<object> response2 = GradingResponse<object>.FromJson(_gradingService.Logout(testEmail2));
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while logging out of a user. " + response2.ErrorMessage);
            else
            {
                // The user should be logged out of the system.
                Console.WriteLine("Logged out of the user. Checking user update:");
                Console.WriteLine(CheckUser(testEmail2, false, false));
            }

            // Should return an error when attempting to log out from a logged-out user.
            GradingResponse<object> response3 = GradingResponse<object>.FromJson(_gradingService.Logout(testEmail2));
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to log out from a logged-out user.");
        }

        /// <summary>
        /// GradingService.AddBoard(string email, string name):
        /// This method adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void TestAddBoard()
        {

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in
            string testEmail2 = "gal@gmail.com"; //user exists and logged in
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "work";
            string boardName2 = "cleaning";
            string testPassword1 = "Abc123456";
            string testPassword2 = "Abc123456";
            string testPassword3 = "Abcd1234";

            Console.WriteLine("\nSetting up TestAddBoard() using Login,Register,Logout.");
            try
            {
                _gradingService.Login(testEmail1, testPassword1);
                _gradingService.Login(testEmail2, testPassword2);
                _gradingService.Register(testEmail3, testPassword3);
                _gradingService.Logout(testEmail3);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed: An error has occured when setting up TestAddBoard() using Login,Register,Logout.");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nTesting AddBoard():");

            //A user that does not exist should not be able to create a board
            string json1 = _gradingService.AddBoard(testEmail4, boardName1);
            GradingResponse<object> response1 = GradingResponse<object>.FromJson(json1);
            if (json1 != "{}" && response1.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a board for a user that does not exist.");

            //A user that is not logged should not be able to create a board
            string json2 = _gradingService.AddBoard(testEmail3, boardName1);
            GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
            if (json2 != "{}" && response2.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a board for a user that is not logged in.");

            //Creating a board for a user that exists and logged in and checking the created board is valid
            string json3 = _gradingService.AddBoard(testEmail1, boardName1);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                Console.WriteLine("Test failed: An error has occured while creating task. " + response3.ErrorMessage);
                if (response3.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine(CheckBoard(testEmail1, boardName1, true));

            //A user should not be able to create two different boards with the same name
            string json4 = _gradingService.AddBoard(testEmail1, boardName1);
            GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
            if (json4 != "{}" && response4.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error when one user creates two boards with the same name");

            //A user should be able to create a new board with a new name, checking the created board is valid
            string json5 = _gradingService.AddBoard(testEmail1, boardName2);
            if (json5 != "{}")
            {
                GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
                Console.WriteLine("Test failed: An error has occured while creating board. " + response5.ErrorMessage);
                if (response5.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine(CheckBoard(testEmail1, boardName2, true));

            //A different user should be able to create a new board with the same name as another user, checking the created board is valid
            string json6 = _gradingService.AddBoard(testEmail2, boardName1);
            if (json6 != "{}")
            {
                GradingResponse<object> response6 = GradingResponse<object>.FromJson(json6);
                Console.WriteLine("Test failed: An error has occured while creating board. " + response6.ErrorMessage);
                if (response6.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine(CheckBoard(testEmail2, boardName1, true));
        }

        private string CheckBoard(string testEmail, string boardName, bool isNew)
        {
            Board board = Response.FromJson(_boardService.GetBoard(testEmail, boardName)).DeserializeReturnValue<Board>();
            return CheckBoard(board, testEmail, boardName, isNew);
        }

        /// <summary>
        /// This function checks if a board is valid, according to the given parameters.
        /// </summary>
        /// 
        private string CheckBoard(Board board, string boardOwner, string boardName, bool isNew)
        {
            if (board.BoardName != boardName)
                return "Test failed: Did not return the expected board name.";
            if (board.BoardOwner != boardOwner)
                return "Test failed: A new board should have the board creater as the board owner.";
            else
            {
                List<Column> Columns = board.Columns;
                if (Columns.Count != 3)
                    return "Test failed: A board should have 3 columns.";
                if (isNew) //A board that has just been created should be empty
                {
                    foreach (Column c in Columns)
                    {
                        if (c.Tasks.Count != 0)
                            return "Test failed: A new board should have 0 tasks.";
                        if (c.TasksLimit != -1)
                            return "Test failed: A new board's columns should not be limitted.";
                    }
                }
            }
            return "Test successful: The board is valid";
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void TestJoinBoard()
        {
            Console.WriteLine("\nTesting JoinBoard():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in
            string testEmail2 = "gal@gmail.com"; //user exists and logged in
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist

            //string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in
            //string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in
            //string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            //string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "work"; //boardId=1 for userEmail1, boardId=3 for userEmail2
            string boardName2 = "cleaning"; //boardId=2 for userEmail1
            int boardId1 = 1; //"work", members: userEmail1
            int boardId2 = 2; //"cleaning", members: userEmail1
            int boardId3 = 3; //"work", members: userEmail2
            int boardId4 = 11; //board does not exist 

            //A user that does not exist should not be able to join a board
            GradingResponse<object> response1 = GradingResponse<object>.FromJson(_gradingService.JoinBoard(testEmail4, boardId1));
            if (response1.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that does not exist tries to join the board.");

            //A user that is not logged should not be able to join a board
            GradingResponse<object> response2 = GradingResponse<object>.FromJson(_gradingService.JoinBoard(testEmail3, boardId1));
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that is not logged tries to join the board.");

            //A user can not join a board that does not exist.
            GradingResponse<object> response3 = GradingResponse<object>.FromJson(_gradingService.JoinBoard(testEmail1, boardId4));
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after trying to join a board that does not exist.");

            //A user should not be able to join a board that he already joined
            GradingResponse<object> response4 = GradingResponse<object>.FromJson(_gradingService.JoinBoard(testEmail1, boardId1));
            if (response4.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after trying to join a board that was joined already.");

            //A user should not be able to join a board with the same name of a board he already joined
            GradingResponse<object> response5 = GradingResponse<object>.FromJson(_gradingService.JoinBoard(testEmail2, boardId1));
            if (response5.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after trying to join a board with a name that was joined before.");

            //A user should be able to join the board
            GradingResponse<object> response6 = GradingResponse<object>.FromJson(_gradingService.JoinBoard(testEmail2, boardId2));
            if (response6.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while joining board. " + response6.ErrorMessage);
            else
            {
                Response response = Response.FromJson(_boardService.GetBoard(testEmail2, boardName2));
                if (response.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured while joining board. " + response.ErrorMessage);
                else
                    Console.WriteLine(CheckBoard(response.DeserializeReturnValue<Board>(), testEmail1, boardName2, false));
            }
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void TestLeaveBoard()
        {
            Console.WriteLine("\nTesting LeaveBoard():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in
            string testEmail2 = "gal@gmail.com"; //user exists and logged in
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            //string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in
            //string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in
            //string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            //string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "work"; //boardId=1 for userEmail1, boardId=3 for userEmail2
            string boardName2 = "cleaning"; //boardId=2 for userEmail1
            string boardName3 = "TV shows"; //board that does not exist
            int boardId1 = 1;  //"work", members: userEmail1
            int boardId2 = 2;  //"cleaning", members: userEmail1 (, userEmail2 ?)
            int boardId3 = 3;  //"work", members: userEmail2
            int boardId4 = 11; //board does not exist 

            //A user that does not exist should not be able to leave a board
            GradingResponse<object> response1 = GradingResponse<object>.FromJson(_gradingService.LeaveBoard(testEmail4, boardId1));
            if (response1.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that does not exist tries to leave the board.");

            //A user that is not logged should not be able to leave a board
            GradingResponse<object> response2 = GradingResponse<object>.FromJson(_gradingService.LeaveBoard(testEmail3, boardId3));
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that is not logged tries to leave the board.");

            //A user can not leave a board that does not exist.
            GradingResponse<object> response3 = GradingResponse<object>.FromJson(_gradingService.LeaveBoard(testEmail1, boardId4));
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after trying to leave a board that does not exist.");

            //A user should not be able to leave a board he is not a member of
            GradingResponse<object> response4 = GradingResponse<object>.FromJson(_gradingService.LeaveBoard(testEmail2, boardId1));
            if (response4.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after user trying to leave a board he is not a member of.");

            //A user should not be able to leave a board when he is the owner
            GradingResponse<object> response5 = GradingResponse<object>.FromJson(_gradingService.LeaveBoard(testEmail1, boardId1));
            if (response5.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after user trying to leave a board he is the owner of.");

            //A user should be able to leave the board
            GradingResponse<object> response6 = GradingResponse<object>.FromJson(_gradingService.LeaveBoard(testEmail2, boardId2));
            if (response6.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while leaving board. " + response6.ErrorMessage);
            else
            {
                Response response = Response.FromJson(_boardService.GetBoard(boardId2));
                if (response.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured while joining board. " + response.ErrorMessage);
                else
                    Console.WriteLine(CheckBoard(response.DeserializeReturnValue<Board>(), testEmail1, boardName2, false));
            }
        }


        /// <summary>
        /// GradingService.RemoveBoard(string email, string name):
        /// This method removes a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the board</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>

        private void TestRemoveBoard()
        {
            Console.WriteLine("\nTesting RemoveBoard():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "work", "cleaning"
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "work" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "work";
            string boardName2 = "cleaning";
            string boardName3 = "grocery shopping";

            //A user that does not exist should not be able to remove a board
            string json1 = _gradingService.RemoveBoard(testEmail4, boardName1);
            if (json1 != "{}")
            {
                GradingResponse<object> response1 = GradingResponse<object>.FromJson(json1);
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
                if (response1.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after removing a board for a user that does not exist.");

            //A user that is not logged in should not be able to remove a board
            string json2 = _gradingService.RemoveBoard(testEmail3, boardName1);
            if (json2 != "{}")
            {
                GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after removing a board for a user that is not logged in.");

            //A user should not be able to remove a board he does not have
            string json3 = _gradingService.RemoveBoard(testEmail1, boardName3);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
                if (response3.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after removing a board that does not exist.");

            //Removing a board for a user that exists and logged in
            string json4 = _gradingService.RemoveBoard(testEmail1, boardName1);
            if (json4 != "{}")
            {
                GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
                Console.WriteLine("Test failed: An error has occured while removing board. " + response4.ErrorMessage);
                if (response4.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test successful: Board has been removed.");

            //A user should not be able te remove a board that has been removed already
            string json5 = _gradingService.RemoveBoard(testEmail1, boardName1);
            if (json5 != "{}")
            {
                GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
                if (response5.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error when removing the same board twice.");

            //A user should be able to remove another board
            string json6 = _gradingService.RemoveBoard(testEmail1, boardName2);
            if (json6 != "{}")
            {
                GradingResponse<object> response6 = GradingResponse<object>.FromJson(json6);
                Console.WriteLine("Test failed: An error has occured while removing board. " + response6.ErrorMessage);
                if (response6.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test successful: Board has been removed.");

            //A different user should be able to remove a board with the same name as another user's board
            string json7 = _gradingService.RemoveBoard(testEmail2, boardName1);
            if (json7 != "{}")
            {
                GradingResponse<object> response7 = GradingResponse<object>.FromJson(json7);
                Console.WriteLine("Test failed: An error has occured while removing board. " + response7.ErrorMessage);
                if (response7.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test successful: Board has been removed.");
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// 
        /// The function attempts to get a board with valid and invalid parameters,
        /// and checks for the expected errors and the validity of the board's values.
        private void TestGetBoardName()
        {
            Console.WriteLine("\nSetting up GetBoardName() using CreateBoard:");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in
            //string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string boardName0 = "tests";
            string boardName1 = "homework";
            string boardName2 = "chores";
            int boardId0 = -1, boardId1 = -1, boardId2 = -1, boardId3 = 20;
            try
            {
                boardId0 = Response.FromJson(_boardService.CreateBoard(testEmail1, boardName0)).DeserializeReturnValue<Board>().BoardID;
                boardId1 = Response.FromJson(_boardService.CreateBoard(testEmail1, boardName1)).DeserializeReturnValue<Board>().BoardID;
                boardId2 = Response.FromJson(_boardService.CreateBoard(testEmail1, boardName2)).DeserializeReturnValue<Board>().BoardID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed: An error has occured when setting up TestGetBoardName() using CreateBoard.");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nTesting GetBoardName():");

            // Should return boardName0
            GradingResponse<string> response0 = GradingResponse<string>.FromJson(_gradingService.GetBoardName(boardId0));
            if (response0.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured after executing GetBoardName with valid board ID.");
            else
            {
                string boardName = response0.ReturnValue;
                if (boardName != boardName0)
                    Console.WriteLine($"Test failed: Expected the board name '{boardName0}' but received '{boardName}'.");
                else
                    Console.WriteLine("Test successful: Received the expected board name after executing GetBoardName().");
            }

            // Should return boardName1
            GradingResponse<string> response1 = GradingResponse<string>.FromJson(_gradingService.GetBoardName(boardId1));
            if (response1.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured after executing GetBoardName with valid board ID.");
            else
            {
                string boardName = response1.ReturnValue;
                if (boardName != boardName1)
                    Console.WriteLine($"Test failed: Expected the board name '{boardName1}' but received '{boardName}'.");
                else
                    Console.WriteLine("Test successful: Received the expected board name after executing GetBoardName().");
            }

            // Should return boardName2
            GradingResponse<string> response2 = GradingResponse<string>.FromJson(_gradingService.GetBoardName(boardId2));
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured after executing GetBoardName with valid board ID.");
            else
            {
                string boardName = response2.ReturnValue;
                if (boardName != boardName2)
                    Console.WriteLine($"Test failed: Expected the board name '{boardName2}' but received '{boardName}'.");
                else
                    Console.WriteLine("Test successful: Received the expected board name after executing GetBoardName().");
            }

            // Should return an error message since the board ID doesn't exist.
            GradingResponse<string> response3 = GradingResponse<string>.FromJson(_gradingService.GetBoardName(boardId3));
            if (response3.ErrorMessage == null)
                Console.WriteLine("Test failed: Expected an error after getting a board name of a non-existing board.");
            else
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestGetUserBoards()
        {
            Console.WriteLine("\nSetting up GetBoardName() using GetBoard:");
            string testEmail1 = "galpinto@gmail.com"; // user exists and logged in
            string testEmail2 = "gal@gmail.com"; // user exists and logged in
            string testEmail3 = "";
            string boardName0 = "tests";
            string boardName1 = "homework";
            string boardName2 = "chores";
            int boardId0 = -1, boardId1 = -1, boardId2 = -1;
            try
            {
                boardId0 = Response.FromJson(_boardService.GetBoard(testEmail1, boardName0)).DeserializeReturnValue<Board>().BoardID;
                boardId1 = Response.FromJson(_boardService.GetBoard(testEmail1, boardName1)).DeserializeReturnValue<Board>().BoardID;
                boardId2 = Response.FromJson(_boardService.GetBoard(testEmail1, boardName2)).DeserializeReturnValue<Board>().BoardID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed: An error has occured when setting up TestGetBoardName() using CreateBoard.");
                Console.WriteLine(ex.Message);
            }

            List<int> expected1 = new List<int> { boardId0, boardId1, boardId2 };
            string json1 = _gradingService.GetUserBoards(testEmail1);
            GradingResponse<List<int>> response1 = GradingResponse<List<int>>.FromJson(json1);
            if (response1.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while executing GetUserBoards(). " + response1.ErrorMessage);
            else
                Console.WriteLine(CheckUserBoardIds(response1.ReturnValue, expected1));

            List<int> expected2 = new List<int>();
            string json2 = _gradingService.GetUserBoards(testEmail2);
            GradingResponse<List<int>> response2 = GradingResponse<List<int>>.FromJson(json2);
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while executing GetUserBoards(). " + response2.ErrorMessage);
            else
                Console.WriteLine(CheckUserBoardIds(response2.ReturnValue, expected2));

            string json3 = _gradingService.GetUserBoards(testEmail3);
            GradingResponse<List<int>> response3 = GradingResponse<List<int>>.FromJson(json3);
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after executing GetUserBoards() with non-existing email address.");
        }

        /// <summary>
        /// This function checks if a list of UserBoardIds is valid
        /// </summary>
        private string CheckUserBoardIds(List<int> actual, List<int> expected)
        {
            if (!(actual.All(expected.Contains) && expected.All(expected.Contains)))
                return "Test failed: Expected list was not returned.";
            return "Test successful: The list is valid.";
        }

        /// <summary>
        /// GradingService.LimitColumn(string email, string boardName, int columnOrdinal, int limit):
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestLimitColumn()
        {
            Console.WriteLine("\nTesting LimitColumn():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            int columnNumber1 = 1;
            int columnNumber2 = 7;
            int limit1 = 10;
            int limit2 = -7;

            _gradingService.AddBoard(testEmail1, boardName1);
            _gradingService.AddBoard(testEmail1, boardName2);
            _gradingService.AddBoard(testEmail2, boardName1);

            //A user that does not exist should not be able to limit a column
            string json1 = _gradingService.LimitColumn(testEmail4, boardName1, columnNumber1, limit1);
            if (json1 != "{}")
            {
                GradingResponse<object> response1 = GradingResponse<object>.FromJson(json1);
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
                if (response1.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to limit a column
            string json2 = _gradingService.LimitColumn(testEmail3, boardName1, columnNumber1, limit1);
            if (json2 != "{}")
            {
                GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user is not logged in.");

            //Users should not be able to limit a column in a board they have not created
            string json3 = _gradingService.LimitColumn(testEmail2, boardName2, columnNumber1, limit1);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
                if (response3.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since board does not exist.");

            //Limiting a column for a user that exists and logged in
            string json4 = _gradingService.LimitColumn(testEmail1, boardName1, columnNumber1, limit1);
            if (json4 != "{}")
            {
                GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
                Console.WriteLine("Test failed: An error has occured while limiting a column. " + response4.ErrorMessage);
                if (response4.ErrorMessage != null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test successful: The cloumn has been limited.");

            //Can not limit a cloumn that does not exist in the board
            string json5 = _gradingService.LimitColumn(testEmail1, boardName1, columnNumber2, limit1);
            if (json5 != "{}")
            {
                GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
                if (response5.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after limitting a column that does not exist.");

            //Can not limit a column to an invalid number
            string json6 = _gradingService.LimitColumn(testEmail1, boardName1, columnNumber1, limit2);
            if (json6 != "{}")
            {
                GradingResponse<object> response6 = GradingResponse<object>.FromJson(json6);
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
                if (response6.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since the limit number is not valid.");
        }

        /// <summary>
        /// GradingService.GetColumnLimit(string email, string boardName, int columnOrdinal)
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column limit value, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestGetColumnLimit()
        {
            Console.WriteLine("\nTesting GetColumnLimit():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            int columnNumber0 = 0;
            int columnNumber1 = 1;
            int columnNumber2 = 7;
            int limit1 = 10;
            int limit2 = -1;

            //A user that does not exist should not be able to get a column limit
            string json1 = _gradingService.GetColumnLimit(testEmail4, boardName1, columnNumber1);
            GradingResponse<object> response1 = GradingResponse<object>.FromJson(json1);
            if (response1.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to get a column limit
            string json2 = _gradingService.GetColumnLimit(testEmail3, boardName1, columnNumber1);
            GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since user is not logged in.");

            //Users should not be able to limit a column in a board they have not created
            string json3 = _gradingService.GetColumnLimit(testEmail2, boardName2, columnNumber1);
            GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since board does not exist.");

            //Can not get the limit of a column that does not exist in the board
            string json4 = _gradingService.GetColumnLimit(testEmail1, boardName1, columnNumber2);
            GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
            if (response4.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after getting a limit of a column that does not exist.");

            //Getting a column limit for a user that exists and logged in (10)
            string json5 = _gradingService.GetColumnLimit(testEmail1, boardName1, columnNumber1);
            GradingResponse<int> response5 = GradingResponse<int>.FromJson(json5);
            if (response5.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while getting a valid column limit. " + response5.ErrorMessage);
            else if (response5.ReturnValue != limit1)
                Console.WriteLine($"Test failed: Expected a column limit of {limit1}, but received: {response5.ReturnValue}.");
            else
                Console.WriteLine("Test successful: Check column limit.");

            //Getting a valid column limit (-1)
            string json6 = _gradingService.GetColumnLimit(testEmail1, boardName1, columnNumber0);
            GradingResponse<int> response6 = GradingResponse<int>.FromJson(json6);
            if (response6.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while getting a valid column limit. " + response6.ErrorMessage);
            else if (response6.ReturnValue != limit2)
                Console.WriteLine($"Test failed: Expected a column limit of {limit2}, but received: {response5.ReturnValue}.");
            else
                Console.WriteLine("Test successful: Check column limit.");
        }


        /// <summary>
        /// GradingService.GetColumnName(string email, string boardName, int columnOrdinal):
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column name value, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestGetColumnName()
        {
            Console.WriteLine("\nTesting GetColumnName():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            int columnNumber0 = 0;
            int columnNumber1 = 1;
            int columnNumber2 = 2;
            int columnNumber5 = 5;
            string[] columnNames = { "backlog", "in progress", "done" };

            //A user that does not exist should not be able to get a column name
            GradingResponse<string> response1 = GradingResponse<string>.FromJson(_gradingService.GetColumnName(testEmail4, boardName1, columnNumber0));
            if (response1.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after getting a board for a user that does not exist.");

            //A user that is not logged in should not be able to get a column name
            GradingResponse<string> response2 = GradingResponse<string>.FromJson(_gradingService.GetColumnName(testEmail3, boardName1, columnNumber1));
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after getting a board for a user that is not logged in.");

            //Users should not be able to get a column name of a board they have not created
            GradingResponse<string> response3 = GradingResponse<string>.FromJson(_gradingService.GetColumnName(testEmail1, boardName3, columnNumber0));
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after getting a board that does not exist.");

            //Getting a column name for a user that exists and logged in and checking the returned column name is valid
            GradingResponse<string> response4 = GradingResponse<string>.FromJson(_gradingService.GetColumnName(testEmail1, boardName1, columnNumber0));
            if (response4.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while creating board. " + response4.ErrorMessage);
            else
            {
                if (response4.ReturnValue == columnNames[columnNumber0])
                    Console.WriteLine($"Test successful: Returned column name {columnNames[columnNumber0]} as expected.");
                else
                    Console.WriteLine($"Test failed: Expected column name to be {columnNames[columnNumber0]}, but received {response4.ReturnValue}.");
            }

            //Getting a column name for a user that exists and logged in and checking the returned column name is valid
            GradingResponse<string> response5 = GradingResponse<string>.FromJson(_gradingService.GetColumnName(testEmail2, boardName1, columnNumber1));
            if (response5.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while creating board. " + response5.ErrorMessage);
            else
            {
                if (response5.ReturnValue == columnNames[columnNumber1])
                    Console.WriteLine($"Test successful: Returned column name {columnNames[columnNumber1]} as expected.");
                else
                    Console.WriteLine($"Test failed: Expected column name to be {columnNames[columnNumber1]}, but received {response5.ReturnValue}.");

            }

            //Getting a column name for a user that exists and logged in and checking the returned column name is valid
            GradingResponse<string> response6 = GradingResponse<string>.FromJson(_gradingService.GetColumnName(testEmail2, boardName1, columnNumber2));
            if (response6.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while creating board. " + response6.ErrorMessage);
            else
            {
                if (response6.ReturnValue == columnNames[columnNumber2])
                    Console.WriteLine($"Test successful: Returned column name {columnNames[columnNumber2]} as expected.");
                else
                    Console.WriteLine($"Test failed: Expected column name to be {columnNames[columnNumber2]}, but received {response6.ReturnValue}.");

            }

            //Should not be able to get a column name of an invalid column number
            GradingResponse<string> response7 = GradingResponse<string>.FromJson(_gradingService.GetColumnName(testEmail2, boardName1, columnNumber5));
            if (response7.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response7.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after getting a column name of an invalid column number.");
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestTransferOwnership()
        {
            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in
            string testEmail2 = "gal@gmail.com"; //user exists and logged in
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework"; //boardId=1 for userEmail1, boardId=3 for userEmail2
            string boardName2 = "chores"; //boardId=2 for userEmail1
            string boardName3 = "family"; //board that does not exist
            string boardName4 = "missions";
            int boardId1 = 5; //"homework", members: userEmail1
            int boardId2 = 6; //"chores", members: userEmail1, userEmail2
            int boardId3 = 7; //"homework", members: userEmail2
            int boardId4 = 21; //board does not exist 
            int boardId5 = -1;

            Console.WriteLine("\nSetting up TestTransferOwner() using CreateBoard and JoinBoard.");
            try
            {
                boardId5 = Response.FromJson(_boardService.CreateBoard(testEmail1, boardName4)).DeserializeReturnValue<Board>().BoardID;
                _boardService.JoinBoard(testEmail2, boardId5);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed: An error has occured while setting up TestTransferOwner() using CreateBoard and JoinBoard.");
            }

            Console.WriteLine("\nTesting TransferOwner():");

            //A user that does not exist should not be able to transfer board ownership
            GradingResponse<object> response1 = GradingResponse<object>.FromJson(_gradingService.TransferOwnership(testEmail4, testEmail1, boardName1));
            if (response1.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that does not exist tries to transfer ownership.");

            //Can't transfer ownership to a user that does not exist
            GradingResponse<object> response2 = GradingResponse<object>.FromJson(_gradingService.TransferOwnership(testEmail1, testEmail4, boardName1));
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage + "\n");
            else
                Console.WriteLine("Test failed: Expected an error after transfering ownership to a user that does not exist.");

            //A user that is not logged in should not be able to transfer ownership
            GradingResponse<object> response3 = GradingResponse<object>.FromJson(_gradingService.TransferOwnership(testEmail3, testEmail1, boardName1));
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that is not logged tries to transfer board ownership.");

            //A user should not be able to transfer ownership of a board that does not exist
            GradingResponse<object> response4 = GradingResponse<object>.FromJson(_gradingService.TransferOwnership(testEmail1, testEmail2, boardName3));
            if (response4.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after transfering ownership of a board that does not exist.");

            //A user who is not the owner can not transfer ownership.
            GradingResponse<object> response5 = GradingResponse<object>.FromJson(_gradingService.TransferOwnership(testEmail2, testEmail1, boardName2));
            if (response5.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user who is not the owner tries to transfer ownership.");

            //Can't transfer ownership to a user who is not a board member.
            GradingResponse<object> response6 = GradingResponse<object>.FromJson(_gradingService.TransferOwnership(testEmail1, testEmail2, boardName1));
            if (response6.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after transfering ownership to a user who is not a board member.");

            //A user should be able to transfer board ownership.
            GradingResponse<object> response7 = GradingResponse<object>.FromJson(_gradingService.TransferOwnership(testEmail1, testEmail2, boardName4));
            if (response7.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while transfering ownership. " + response7.ErrorMessage);
            else
            {
                Response response = Response.FromJson(_boardService.GetBoard(testEmail2, boardName4));
                if (response.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured while transfering ownership. " + response.ErrorMessage);
                else
                    Console.WriteLine(CheckBoard(response.DeserializeReturnValue<Board>(), testEmail2, boardName4, false));
            }
        }


        private void LoginUsers()
        {
            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testPassword1 = "Abc123456";
            string testPassword2 = "Abc123456";

            _gradingService.Login(testEmail1, testPassword1);
            _gradingService.Login(testEmail2, testPassword2);
        }

        /// <summary>
        /// GradingService.AddTask(string email, string boardName, string title, string description, DateTime dueDate):
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>Response with user-email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestAddTask()
        {
            Console.WriteLine("\nTesting AddTask():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            DateTime dueDate = new DateTime(2022, 7, 1);
            string title = "calculus homework";
            string description = "Second question in page 123";
            DateTime dueDate1 = new DateTime(2022, 8, 1);
            //DateTime dueDate2 = new DateTime(2022, 5, 10); // Earlier than the current date
            string title1 = "clean apartment";
            string description1 = "Vacuum and wash floors";
            int taskId0 = 1;
            int taskId1 = 2;
            int taskId2 = 3;

            //A user that does not exist should not be able to create a task
            GradingResponse<object> response1 = GradingResponse<object>.FromJson(_gradingService.AddTask(testEmail4, boardName2, title, description, dueDate));
            if (response1.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a task for a user that does not exist.");

            //A user that is not logged in should not be able to create a task
            GradingResponse<object> response2 = GradingResponse<object>.FromJson(_gradingService.AddTask(testEmail3, boardName2, title, description, dueDate));
            if (response2.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a task for a user the is not logged in.");

            //A user should not be a able to create a task in a board that does not exist
            GradingResponse<object> response3 = GradingResponse<object>.FromJson(_gradingService.AddTask(testEmail1, boardName3, title, description, dueDate));
            if (response3.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a task in a board that does not exist.");

            //A user should not be a able to create a task in a board that belongs to another user
            GradingResponse<object> response4 = GradingResponse<object>.FromJson(_gradingService.AddTask(testEmail2, boardName2, title, description, dueDate));
            if (response4.ErrorMessage != null)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a task in a board that do not belong to the user.");

            //Creating a task
            GradingResponse<object> response5 = GradingResponse<object>.FromJson(_gradingService.AddTask(testEmail1, boardName1, title, description, dueDate));
            if (response5.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while creating task. " + response5.ErrorMessage);
            else
            {
                Console.WriteLine(CheckTask(taskId0, testEmail1, boardName1, dueDate, title, description));
            }

            //Creating a task with the same title
            GradingResponse<object> response6 = GradingResponse<object>.FromJson(_gradingService.AddTask(testEmail1, boardName1, title, description, dueDate));
            if (response6.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while creating task. " + response6.ErrorMessage);
            else
            {
                Console.WriteLine(CheckTask(taskId1, testEmail1, boardName1, dueDate, title, description));
            }

            //Creating a task in a different board
            GradingResponse<object> response7 = GradingResponse<object>.FromJson(_gradingService.AddTask(testEmail1, boardName2, title1, description1, dueDate1));
            if (response7.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while creating task. " + response7.ErrorMessage);
            else
            {
                Console.WriteLine(CheckTask(taskId2, testEmail1, boardName2, dueDate1, title1, description1));
            }


            ////Creating a task with a due date which is earlier than the current date
            //GradingResponse<string> response8 = GradingResponse<string>.FromJson(_gradingService.AddTask(testEmail1, boardName2, title1, description1, dueDate2));
            //if (response8.ErrorMessage != null)
            //    Console.WriteLine("Test successful: Expected error has occured. " + response8.ErrorMessage);
            //else
            //    Console.WriteLine("Test failed: Expected an error after creating a task with a due date which is earlier than the current date.");
        }

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

        /// <summary>
        /// GradingService.AdvanceTask(string email, string boardName, int columnOrdinal, int taskId):
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestAdvanceTask()
        {
            Console.WriteLine("\nTesting AdvanceTask():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework (tasks 0,1)", "chores"(task 2)
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
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
            string json1 = _gradingService.AdvanceTask(testEmail4, boardName1, columnNumber1, taskId1);
            if (json1 != "{}")
            {
                GradingResponse<object> response1 = GradingResponse<object>.FromJson(json1);
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
                if (response1.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task of a user that does not exist.");

            //A user that is not logged in should not be able to move a task
            string json2 = _gradingService.AdvanceTask(testEmail3, boardName1, columnNumber1, taskId1);
            if (json2 != "{}")
            {
                GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task of a user that is not logged in.");

            //A user should not be able to move a task in a board that does not exist
            string json3 = _gradingService.AdvanceTask(testEmail1, boardName3, columnNumber1, taskId1);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
                if (response3.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task in a board that does not exist.");

            //A user should not be able to move a task that does not exist
            string json4 = _gradingService.AdvanceTask(testEmail1, boardName1, columnNumber1, taskId4);
            if (json4 != "{}")
            {
                GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
                if (response4.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task that does not exist.");

            //A user should not be able to move a task that is not in the right column
            string json5 = _gradingService.AdvanceTask(testEmail1, boardName1, columnNumber2, taskId1);
            if (json5 != "{}")
            {
                GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
                if (response5.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task that is not in the right column.");

            //A user should not be able to move a task that does not belong the the board
            string json6 = _gradingService.AdvanceTask(testEmail1, boardName2, columnNumber1, taskId2);
            if (json6 != "{}")
            {
                GradingResponse<object> response6 = GradingResponse<object>.FromJson(json6);
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
                if (response6.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task that does not belong to the board.");

            // TODO: Refactor
            _taskService.AssignTask(testEmail1, boardName1, taskId1, testEmail1);

            //Moving a task
            string json7 = _gradingService.AdvanceTask(testEmail1, boardName1, columnNumber1, taskId1);
            if (json7 != "{}")
            {
                GradingResponse<object> response7 = GradingResponse<object>.FromJson(json7);
                Console.WriteLine("Test failed: An error has occured while moving task. " + response7.ErrorMessage);
                if (response7.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine(IsTaskMoved(testEmail1, boardName1, columnNumber1, taskId1));

            //Moving a task
            string json8 = _gradingService.AdvanceTask(testEmail1, boardName1, columnNumber2, taskId1);
            if (json8 != "{}")
            {
                GradingResponse<object> response8 = GradingResponse<object>.FromJson(json8);
                Console.WriteLine("Test failed: An error has occured while moving task. " + response8.ErrorMessage);
                if (response8.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine(IsTaskMoved(testEmail1, boardName1, columnNumber2, taskId1));

            //Can not move a task from the "done" column
            string json9 = _gradingService.AdvanceTask(testEmail1, boardName1, columnNumber3, taskId1);
            if (json9 != "{}")
            {
                GradingResponse<object> response9 = GradingResponse<object>.FromJson(json9);
                Console.WriteLine("Test successful: Expected error has occured. " + response9.ErrorMessage);
                if (response9.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't move a task from the last column.");

            //Can not move a task from an invalid column number
            string json10 = _gradingService.AdvanceTask(testEmail1, boardName1, columnNumber4, taskId1);
            if (json10 != "{}")
            {
                GradingResponse<object> response10 = GradingResponse<object>.FromJson(json10);
                Console.WriteLine("Test successful: Expected error has occured. " + response10.ErrorMessage);
                if (response10.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't move a task from the last column.");
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
        /// GradingService.InProgressTasks(string email):
        /// This method returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>Response with  a list of the in progress tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestInProgressTasks()
        {
            Console.WriteLine("\nTesting InProgressTasks():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "gal@gmail.com"; //user exists and logged in
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";

            //A user that does not exist should not be able to list tasks in progress
            string json1 = _gradingService.InProgressTasks(testEmail4);
            if (json1 != "{}")
            {
                GradingResponse<List<GradingTask>> response1 = GradingResponse<List<GradingTask>>.FromJson(json1);
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
                if (response1.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to list tasks in progress
            string json2 = _gradingService.InProgressTasks(testEmail3);
            if (json2 != "{}")
            {
                GradingResponse<List<GradingTask>> response2 = GradingResponse<List<GradingTask>>.FromJson(json2);
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user is not logged in.");

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
                List<GradingTask> expected = new List<GradingTask>();
                expected.Add(new GradingTask(task1));
                expected.Add(new GradingTask(task2));

                // Move the tasks to In Progress columns
                _gradingService.AdvanceTask(testEmail1, boardName1, 0, 2);
                _gradingService.AdvanceTask(testEmail1, boardName2, 0, 3);

                string json3 = _gradingService.InProgressTasks(testEmail1);
                GradingResponse<List<GradingTask>> response3 = GradingResponse<List<GradingTask>>.FromJson(json3);
                if (response3.ErrorMessage != null)
                    Console.WriteLine("Test failed: An error has occured while creating list of tasks in progress." + response3.ErrorMessage);
                else
                    Console.WriteLine(CheckInProgressList(testEmail1, expected));
            }
        }

        /// <summary>
        /// This function checks if a list of tasks in progress is valid
        /// </summary>
        private string CheckInProgressList(string testEmail, List<GradingTask> expected)
        {
            GradingResponse<List<GradingTask>> response = GradingResponse<List<GradingTask>>.FromJson(_gradingService.InProgressTasks(testEmail));
            if (response.ErrorMessage != null)
                return "Test failed." + response.ErrorMessage;
            else
            {
                List<GradingTask> inProgressList = response.ReturnValue;
                if (!(inProgressList.All(expected.Contains) && expected.All(inProgressList.Contains)))
                    return "Test failed: Expected list was not returned.";
            }
            return "Test successful: The list is valid.";
        }

        /// <summary>
        /// GradingService.GetColumn(string email, string boardName, int columnOrdinal)
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with  a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestGetColumn()
        {
            Console.WriteLine("\nTesting GetColumn():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework (tasks 0,1)", "chores"(task 2)
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
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

            //A user that does not exist should not be able to list tasks in progress
            string json1 = _gradingService.GetColumn(testEmail4, boardName1, columnNumber1);
            if (json1 != "{}")
            {
                GradingResponse<List<GradingTask>> response1 = GradingResponse<List<GradingTask>>.FromJson(json1);
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
                if (response1.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to list tasks in progress
            string json2 = _gradingService.GetColumn(testEmail3, boardName1, columnNumber1);
            if (json2 != "{}")
            {
                GradingResponse<List<GradingTask>> response2 = GradingResponse<List<GradingTask>>.FromJson(json2);
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user is not logged in.");

            //A user should not be able to get a column of a board that does not exist
            string json3 = _gradingService.GetColumn(testEmail1, boardName3, columnNumber1);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
                if (response3.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task in a board that does not exist.");

            //A user should not be able to get a column of a board with null boardname
            string json4 = _gradingService.GetColumn(testEmail1, null, columnNumber1);
            if (json4 != "{}")
            {
                GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
                if (response4.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task that does not exist.");

            //A user should not be able to get a column that does not exist
            string json5 = _gradingService.GetColumn(testEmail1, boardName1, columnNumber4);
            if (json5 != "{}")
            {
                GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
                if (response5.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after moving a task that does not exist.");

            //Should not return an error when getting a valid column.
            string json6 = _gradingService.GetColumn(testEmail1, boardName1, columnNumber2);
            //Console.WriteLine($"Json:\n{json6}"); // Remove later
            GradingResponse<List<GradingTask>> response6 = GradingResponse<List<GradingTask>>.FromJson(json6);
            if (response6.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while getting a valid column. " + response6.ErrorMessage);
            else
            {
                //Listing the expected column tasks
                List<GradingTask> expectedList = new List<GradingTask>();
                Response getTask = Response.FromJson(_taskService.GetTask(testEmail1, boardName1, taskId2));
                if (getTask.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured when attempted to get a valid task.");
                else
                {
                    expectedList.Add(new GradingTask(getTask.DeserializeReturnValue<Task>()));
                    //Checking the returned column
                    Console.WriteLine(CheckColumn(response6.ReturnValue, expectedList));
                }
            }

            //Should not return an error when getting a valid column.
            string json7 = _gradingService.GetColumn(testEmail1, boardName1, columnNumber3);
            //Console.WriteLine($"Json:\n{json7}"); // Remove later
            GradingResponse<List<GradingTask>> response7 = GradingResponse<List<GradingTask>>.FromJson(json7);
            if (response7.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while getting a valid column. " + response7.ErrorMessage);
            else
            {
                //Listing the expected column tasks
                List<GradingTask> expectedList = new List<GradingTask>();
                Response getTask = Response.FromJson(_taskService.GetTask(testEmail1, boardName1, taskId1));
                if (getTask.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured when attempted to get a valid task.");
                else
                {
                    expectedList.Add(new GradingTask(getTask.DeserializeReturnValue<Task>()));
                    //Checking the returned column
                    Console.WriteLine(CheckColumn(response7.ReturnValue, expectedList));
                }
            }

            //Should not return an error when getting a valid column.
            string json8 = _gradingService.GetColumn(testEmail1, boardName2, columnNumber2);
            //Console.WriteLine($"Json:\n{json8}"); // Remove later
            GradingResponse<List<GradingTask>> response8 = GradingResponse<List<GradingTask>>.FromJson(json8);
            if (response8.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while getting a valid column. " + response8.ErrorMessage);
            else
            {
                //Listing the expected column tasks
                List<GradingTask> expectedList = new List<GradingTask>();
                Response getTask = Response.FromJson(_taskService.GetTask(testEmail1, boardName2, taskId3));
                if (getTask.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured when attempted to get a valid task.");
                else
                {
                    expectedList.Add(new GradingTask(getTask.DeserializeReturnValue<Task>()));
                    //Checking the returned column
                    Console.WriteLine(CheckColumn(response8.ReturnValue, expectedList));
                }
            }

            //Should not return an error when getting a valid column.
            string json9 = _gradingService.GetColumn(testEmail1, boardName2, columnNumber3);
            //Console.WriteLine($"Json:\n{json9}"); // Remove later
            GradingResponse<List<GradingTask>> response9 = GradingResponse<List<GradingTask>>.FromJson(json9);
            if (response9.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while getting a valid column. " + response9.ErrorMessage);
            else
            {
                //Listing the expected column tasks
                List<GradingTask> expectedList = new List<GradingTask>();
                //Checking the returned column
                Console.WriteLine(CheckColumn(response9.ReturnValue, expectedList));
            }

            //Should not return an error when getting a valid column.
            string json10 = _gradingService.GetColumn(testEmail2, boardName1, columnNumber2);
            //Console.WriteLine($"Json:\n{json10}"); // Remove later
            GradingResponse<List<GradingTask>> response10 = GradingResponse<List<GradingTask>>.FromJson(json10);
            if (response10.ErrorMessage != null)
                Console.WriteLine("Test failed: An error has occured while getting a valid column. " + response10.ErrorMessage);
            else
            {
                //Listing the expected column tasks
                List<GradingTask> expectedList = new List<GradingTask>();
                //Checking the returned column
                Console.WriteLine(CheckColumn(response10.ReturnValue, expectedList));
            }
        }

        private string CheckColumn(List<GradingTask> returnedList, List<GradingTask> expectedList)
        {
            if (!(returnedList.All(expectedList.Contains) && returnedList.All(expectedList.Contains)))
                return "Test failed: Expected column was not returned.";
            return "Test successful: The returned column is valid.";
        }


        /// <summary>
        /// GradingService.UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate):
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestUpdateTaskDueDate()
        {
            Console.WriteLine("\nTesting UpdateTaskDueDate():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework (tasks 0,1)", "chores"(task 2)
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
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
            int taskId3 = 10; //task does not exist
            int taskId4 = 3; //task exists
            DateTime dueDate1 = new DateTime(2023, 10, 10);
            DateTime dueDate2 = new DateTime(2024, 12, 5);
            //DateTime dueDate3 = new DateTime(2022, 5, 10); // Earlier than the current date

            //A user that does not exist should not be able to update task's due date
            string json1 = _gradingService.UpdateTaskDueDate(testEmail4, boardName1, columnNumber1, taskId1, dueDate1);
            if (json1 != "{}")
            {
                GradingResponse<List<GradingTask>> response1 = GradingResponse<List<GradingTask>>.FromJson(json1);
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
                if (response1.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to update a task
            string json2 = _gradingService.UpdateTaskDueDate(testEmail3, boardName1, columnNumber1, taskId1, dueDate1);
            if (json2 != "{}")
            {
                GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task of a user that is not logged in.");

            //A user should not be able to update a task in a board that does not exist
            string json3 = _gradingService.UpdateTaskDueDate(testEmail1, boardName3, columnNumber1, taskId1, dueDate1);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
                if (response3.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task in a board that does not exist.");

            //A user should not be able to update a task that does not exist
            string json4 = _gradingService.UpdateTaskDueDate(testEmail1, boardName1, columnNumber1, taskId3, dueDate1);
            if (json4 != "{}")
            {
                GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
                if (response4.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task that does not exist.");

            //Can not update a task from an invalid column number
            string json5 = _gradingService.UpdateTaskDueDate(testEmail1, boardName1, columnNumber4, taskId1, dueDate1);
            if (json5 != "{}")
            {
                GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
                if (response5.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a task from an invalid column.");

            //Should return an error when updating a task in the last column
            string json6 = _gradingService.UpdateTaskDueDate(testEmail1, boardName1, columnNumber3, taskId1, dueDate1);
            if (json6 != "{}")
            {
                GradingResponse<object> response6 = GradingResponse<object>.FromJson(json6);
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
                if (response6.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a task from the last column.");

            //Should return an error when updating a non-existing task
            string json7 = _gradingService.UpdateTaskDueDate(testEmail2, boardName1, columnNumber2, taskId1, dueDate2);
            if (json7 != "{}")
            {
                GradingResponse<object> response7 = GradingResponse<object>.FromJson(json7);
                Console.WriteLine("Test successful: Expected error has occured. " + response7.ErrorMessage);
                if (response7.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a non-existing task.");

            // TODO: Refactor:
            _taskService.AssignTask(testEmail1, boardName1, taskId2, testEmail1);
            _taskService.AssignTask(testEmail1, boardName2, taskId4, testEmail1);

            //Should not return an error when updating a valid task
            string json8 = _gradingService.UpdateTaskDueDate(testEmail1, boardName1, columnNumber2, taskId2, dueDate1);
            GradingResponse<object> response8 = GradingResponse<object>.FromJson(json8);
            if (json8 != "{}")
                Console.WriteLine("Test failed: Expected an empty json after updating a valid task. " + response8.ErrorMessage);
            else
                Console.WriteLine(CheckUpdatedDueDate(testEmail1, boardName1, taskId2, dueDate1));

            //Should not return an error when updating a valid task
            string json9 = _gradingService.UpdateTaskDueDate(testEmail1, boardName2, columnNumber2, taskId4, dueDate2);
            GradingResponse<object> response9 = GradingResponse<object>.FromJson(json9);
            if (json9 != "{}")
                Console.WriteLine("Test failed: Expected an empty json after updating a valid task. " + response9.ErrorMessage);
            else
                Console.WriteLine(CheckUpdatedDueDate(testEmail1, boardName2, taskId4, dueDate2));

            ////Should return an error when updating a task to have a due date which is earlier than the current date
            //string json10 = _gradingService.UpdateTaskDueDate(testEmail1, boardName2, columnNumber2, taskId1, dueDate3);
            //GradingResponse<object> response10 = GradingResponse<object>.FromJson(json10);
            //if (response10.ErrorMessage != null)
            //    Console.WriteLine("Test successful: Expected error has occured. " + response10.ErrorMessage);
            //else
            //    Console.WriteLine("Test failed: Expected an error after updating a task to have a due date which is earlier than the current date.");
        }

        private string CheckUpdatedDueDate(string email, string boardName, int taskId, DateTime updatedDueDate)
        {
            Response getUpdatedTask = Response.FromJson(_taskService.GetTask(email, boardName, taskId));
            if (getUpdatedTask.ErrorOccured)
                return "Test failed: An error has occured after getting an updated task. " + getUpdatedTask.ErrorMessage;
            GradingTask updatedTask = new GradingTask(getUpdatedTask.DeserializeReturnValue<Task>());
            if (updatedTask.DueDate != updatedDueDate)
                return "Test failed: Expected an update to the task after executing UpdateTaskDueDate().";
            return "Test successful: Updated task successfully.";
        }


        /// <summary>
        /// GradingService.UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title):
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestUpdateTaskTitle()
        {
            Console.WriteLine("\nTesting UpdateTaskTitle():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework (tasks 0,1)", "chores"(task 2)
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
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
            int taskId3 = 10; //task does not exist
            int taskId4 = 3; //task exists
            // Invalid titles:
            string title1 = null;
            string title2 = "";
            string title3 = "012345678901234567890123456789012345678901234567890"; // 51 characters, too long
            // Valid titles:
            string title4 = "Milestone 2";
            string title5 = "Fifty 01234567890123456789012345678901234567890123"; // 50 characters, valid

            //A user that does not exist should not be able to update task's title
            string json1 = _gradingService.UpdateTaskTitle(testEmail4, boardName1, columnNumber1, taskId1, title4);
            if (json1 != "{}")
            {
                GradingResponse<List<GradingTask>> response1 = GradingResponse<List<GradingTask>>.FromJson(json1);
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
                if (response1.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to update a task
            string json2 = _gradingService.UpdateTaskTitle(testEmail3, boardName1, columnNumber1, taskId1, title4);
            if (json2 != "{}")
            {
                GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task of a user that is not logged in.");

            //A user should not be able to update a task in a board that does not exist
            string json3 = _gradingService.UpdateTaskTitle(testEmail1, boardName3, columnNumber1, taskId1, title4);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
                if (response3.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task in a board that does not exist.");

            //A user should not be able to update a task that does not exist
            string json4 = _gradingService.UpdateTaskTitle(testEmail1, boardName1, columnNumber1, taskId3, title4);
            if (json4 != "{}")
            {
                GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
                if (response4.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task that does not exist.");

            //Can not update a task from an invalid column number
            string json5 = _gradingService.UpdateTaskTitle(testEmail1, boardName1, columnNumber4, taskId1, title4);
            if (json5 != "{}")
            {
                GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
                if (response5.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a task from an invalid column.");

            //Should return an error when updating a task in the last column
            string json6 = _gradingService.UpdateTaskTitle(testEmail1, boardName1, columnNumber3, taskId1, title4);
            if (json6 != "{}")
            {
                GradingResponse<object> response6 = GradingResponse<object>.FromJson(json6);
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
                if (response6.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a task from the last column.");

            //Should return an error when updating a non-existing task
            string json7 = _gradingService.UpdateTaskTitle(testEmail2, boardName1, columnNumber2, taskId1, title4);
            if (json7 != "{}")
            {
                GradingResponse<object> response7 = GradingResponse<object>.FromJson(json7);
                Console.WriteLine("Test successful: Expected error has occured. " + response7.ErrorMessage);
                if (response7.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a non-existing task.");

            //Should return an error when updating task with invalid title: null
            string json8 = _gradingService.UpdateTaskTitle(testEmail1, boardName1, columnNumber2, taskId2, title1);
            if (json8 != "{}")
            {
                GradingResponse<object> response8 = GradingResponse<object>.FromJson(json8);
                Console.WriteLine("Test successful: Expected error has occured. " + response8.ErrorMessage);
                if (response8.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - invalid task title: null.");

            //Should return an error when updating task with invalid title: ""
            string json9 = _gradingService.UpdateTaskTitle(testEmail1, boardName1, columnNumber2, taskId2, title2);
            if (json9 != "{}")
            {
                GradingResponse<object> response9 = GradingResponse<object>.FromJson(json9);
                Console.WriteLine("Test successful: Expected error has occured. " + response9.ErrorMessage);
                if (response9.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - invalid task title: \"\".");

            //Should return an error when updating task with invalid title: 51 characters long
            string json10 = _gradingService.UpdateTaskTitle(testEmail1, boardName1, columnNumber2, taskId2, title3);
            if (json10 != "{}")
            {
                GradingResponse<object> response10 = GradingResponse<object>.FromJson(json10);
                Console.WriteLine("Test successful: Expected error has occured. " + response10.ErrorMessage);
                if (response10.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - invalid task title: 51 characters long.");

            //Should not return an error when updating a valid task
            string json11 = _gradingService.UpdateTaskTitle(testEmail1, boardName1, columnNumber2, taskId2, title4);
            GradingResponse<object> response11 = GradingResponse<object>.FromJson(json11);
            if (json11 != "{}")
                Console.WriteLine("Test failed: Expected an empty json after updating a valid task. " + response11.ErrorMessage);
            else
                Console.WriteLine(CheckUpdatedTitle(testEmail1, boardName1, taskId2, title4));

            //Should not return an error when updating a valid task
            string json12 = _gradingService.UpdateTaskTitle(testEmail1, boardName2, columnNumber2, taskId4, title5);
            GradingResponse<object> response12 = GradingResponse<object>.FromJson(json12);
            if (json12 != "{}")
                Console.WriteLine("Test failed: Expected an empty json after updating a valid task. " + response12.ErrorMessage);
            else
                Console.WriteLine(CheckUpdatedTitle(testEmail1, boardName2, taskId4, title5));
        }

        private string CheckUpdatedTitle(string email, string boardName, int taskId, string updatedTitle)
        {
            Response getUpdatedTask = Response.FromJson(_taskService.GetTask(email, boardName, taskId));
            if (getUpdatedTask.ErrorOccured)
                return "Test failed: An error has occured after getting an updated task. " + getUpdatedTask.ErrorMessage;
            GradingTask updatedTask = new GradingTask(getUpdatedTask.DeserializeReturnValue<Task>());
            if (updatedTask.Title != updatedTitle)
                return "Test failed: Expected an update to the task after executing UpdateTaskTitle().";
            return "Test successful: Updated task successfully.";
        }

        /// <summary>
        /// GradingService.UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>The string "{}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestUpdateTaskDescription()
        {
            Console.WriteLine("\nTesting UpdateTaskDescription():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework (tasks 0,1)", "chores"(task 2)
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
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
            int taskId3 = 10; //task does not exist
            int taskId4 = 3; //task exists

            // Invalid titles:
            string description1 = null;
            // 301 characters, too long
            string description2 = "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

            // Valid titles:
            string description3 = "";
            string description4 = "Finish Milestone 2";
            // 300 characters, valid
            string description5 = "Three Hundred 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345";

            //A user that does not exist should not be able to update task's description
            string json1 = _gradingService.UpdateTaskDescription(testEmail4, boardName1, columnNumber1, taskId1, description4);
            if (json1 != "{}")
            {
                GradingResponse<List<GradingTask>> response1 = GradingResponse<List<GradingTask>>.FromJson(json1);
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
                if (response1.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to update a task
            string json2 = _gradingService.UpdateTaskDescription(testEmail3, boardName1, columnNumber1, taskId1, description4);
            if (json2 != "{}")
            {
                GradingResponse<object> response2 = GradingResponse<object>.FromJson(json2);
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
                if (response2.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task of a user that is not logged in.");

            //A user should not be able to update a task in a board that does not exist
            string json3 = _gradingService.UpdateTaskDescription(testEmail1, boardName3, columnNumber1, taskId1, description4);
            if (json3 != "{}")
            {
                GradingResponse<object> response3 = GradingResponse<object>.FromJson(json3);
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
                if (response3.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task in a board that does not exist.");

            //A user should not be able to update a task that does not exist
            string json4 = _gradingService.UpdateTaskDescription(testEmail1, boardName1, columnNumber1, taskId3, description4);
            if (json4 != "{}")
            {
                GradingResponse<object> response4 = GradingResponse<object>.FromJson(json4);
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
                if (response4.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error after updating a task that does not exist.");

            //Can not update a task from an invalid column number
            string json5 = _gradingService.UpdateTaskDescription(testEmail1, boardName1, columnNumber4, taskId1, description4);
            if (json5 != "{}")
            {
                GradingResponse<object> response5 = GradingResponse<object>.FromJson(json5);
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
                if (response5.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a task from an invalid column.");

            //Should return an error when updating a task in the last column
            string json6 = _gradingService.UpdateTaskDescription(testEmail1, boardName1, columnNumber3, taskId1, description4);
            if (json6 != "{}")
            {
                GradingResponse<object> response6 = GradingResponse<object>.FromJson(json6);
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
                if (response6.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a task from the last column.");

            //Should return an error when updating a non-existing task
            string json7 = _gradingService.UpdateTaskDescription(testEmail2, boardName1, columnNumber2, taskId1, description4);
            if (json7 != "{}")
            {
                GradingResponse<object> response7 = GradingResponse<object>.FromJson(json7);
                Console.WriteLine("Test successful: Expected error has occured. " + response7.ErrorMessage);
                if (response7.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - can't update a non-existing task.");

            //Should return an error when updating task with invalid description: null
            string json8 = _gradingService.UpdateTaskDescription(testEmail1, boardName1, columnNumber2, taskId2, description1);
            if (json8 != "{}")
            {
                GradingResponse<object> response8 = GradingResponse<object>.FromJson(json8);
                Console.WriteLine("Test successful: Expected error has occured. " + response8.ErrorMessage);
                if (response8.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - invalid task description: null.");

            //Should return an error when updating task with invalid description: 301 characters long
            string json9 = _gradingService.UpdateTaskDescription(testEmail1, boardName1, columnNumber2, taskId2, description2);
            if (json9 != "{}")
            {
                GradingResponse<object> response9 = GradingResponse<object>.FromJson(json9);
                Console.WriteLine("Test successful: Expected error has occured. " + response9.ErrorMessage);
                if (response9.ErrorMessage == null)
                    Console.WriteLine("Test failed: If the GradingService returned a json, expected an error message but received null.");
            }
            else
                Console.WriteLine("Test failed: Expected an error - invalid description title: 301 characters long.");

            //Should not return an error when updating a valid task
            string json10 = _gradingService.UpdateTaskDescription(testEmail1, boardName1, columnNumber2, taskId2, description4);
            GradingResponse<object> response10 = GradingResponse<object>.FromJson(json10);
            if (json10 != "{}")
                Console.WriteLine("Test failed: Expected an empty json after updating a valid task. " + response10.ErrorMessage);
            else
                Console.WriteLine(CheckUpdatedDescription(testEmail1, boardName1, taskId2, description4));

            //Should not return an error when updating a valid task
            string json11 = _gradingService.UpdateTaskDescription(testEmail1, boardName2, columnNumber2, taskId4, description5);
            GradingResponse<object> response11 = GradingResponse<object>.FromJson(json11);
            if (json11 != "{}")
                Console.WriteLine("Test failed: Expected an empty json after updating a valid task. " + response11.ErrorMessage);
            else
                Console.WriteLine(CheckUpdatedDescription(testEmail1, boardName2, taskId4, description5));

            //Should not return an error when updating a valid task
            string json12 = _gradingService.UpdateTaskDescription(testEmail1, boardName2, columnNumber2, taskId4, description3);
            GradingResponse<object> response12 = GradingResponse<object>.FromJson(json12);
            if (json12 != "{}")
                Console.WriteLine("Test failed: Expected an empty json after updating a valid task. " + response12.ErrorMessage);
            else
                Console.WriteLine(CheckUpdatedDescription(testEmail1, boardName2, taskId4, description3));
        }

        private string CheckUpdatedDescription(string email, string boardName, int taskId, string description)
        {
            Response getUpdatedTask = Response.FromJson(_taskService.GetTask(email, boardName, taskId));
            if (getUpdatedTask.ErrorOccured)
                return "Test failed: An error has occured after getting an updated task. " + getUpdatedTask.ErrorMessage;
            GradingTask updatedTask = new GradingTask(getUpdatedTask.DeserializeReturnValue<Task>());
            if (updatedTask.Description != description)
                return "Test failed: Expected an update to the task after executing UpdateTaskDescription().";
            return "Test successful: Updated task successfully.";
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        private void TestAssignTask()
        {
            Console.WriteLine("\nTesting AssignTask():");

            string testEmail1 = "galpinto@gmail.com"; //user exists and logged in, boards: "homework (tasks 0,1)", "chores"(task 2)
            string testEmail2 = "gal@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "test@test.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            int taskId1 = 1;
            int taskId2 = 10;
            int taskId3 = 2;
            int taskId4 = 3;
            int boardID1 = 4;
            int boardID2 = 6;

            _boardService.JoinBoard(testEmail2, boardID2);

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
