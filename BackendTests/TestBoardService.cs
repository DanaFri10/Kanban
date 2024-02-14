using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    /// <summary>
    /// This class consists of tests to the BoardService part of the application.
    /// </summary>
    internal class TestBoardService
    {
        BoardService _boardService;
        UserService _userService;

        public TestBoardService(BoardService boardService, UserService userService)
        {
            _boardService = boardService;
            _userService = userService;

            _userService.CreateUser("danafriedman@gmail.com", "Dana12345");

            _userService.CreateUser("eyalooshgerman@gmail.com", "Eyal12345");

            _userService.CreateUser("yonilevi@gmail.com", "Yoni12345");
            _userService.Logout("yonilevi@gmail.com");
        }
        internal void Test()
        {
            Console.WriteLine("\nTesting BoardService:");
            TestCreateBoard();
            TestJoinBoard();
            TestLeaveBoard();
            TestRemoveBoard();
            TestGetBoard();
            TestLimitColumn();
            TestTransferOwner();
        }

        /// <summary>
        /// This function tests requirements 9, 11.
        /// The function attempts to create a new board with valid and invalid parameters,
        /// and checks for the expected errors and the validity of the board's values.
        /// </summary>
        private void TestCreateBoard()
        {
            Console.WriteLine("\nTesting CreateBoard():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "work";
            string boardName2 = "cleaning";

            //A user that does not exist should not be able to create a board
            Response response1 = Response.FromJson(_boardService.CreateBoard(testEmail4, boardName1));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a board for a user that does not exist.");

            //A user that is not logged should not be able to create a board
            Response response2 = Response.FromJson(_boardService.CreateBoard(testEmail3, boardName1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a board for a user that is not logged in.");

            //Creating a board for a user that exists and logged in and checking the created board is valid
            Response response3 = Response.FromJson(_boardService.CreateBoard(testEmail1, boardName1));
            if (response3.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating task. " + response3.ErrorMessage);
            else
                Console.WriteLine(CheckBoard(response3.DeserializeReturnValue<Board>(), testEmail1, boardName1, true));

            //A user should not be able to create two different boards with the same name
            Response response4 = Response.FromJson(_boardService.CreateBoard(testEmail1, boardName1));
            if (response4.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error when one user creates two boards with the same name");

            //A user should be able to create a new board with a new name, checking the created board is valid
            Response response5 = Response.FromJson(_boardService.CreateBoard(testEmail1, boardName2));
            if (response5.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating board. " + response5.ErrorMessage);
            else
                Console.WriteLine(CheckBoard(response5.DeserializeReturnValue<Board>(), testEmail1, boardName2, true));

            //A different user should be able to create a new board with the same name as another user, checking the created board is valid
            Response response6 = Response.FromJson(_boardService.CreateBoard(testEmail2, boardName1));
            if (response6.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating board. " + response6.ErrorMessage);
            else
                Console.WriteLine(CheckBoard(response6.DeserializeReturnValue<Board>(), testEmail2, boardName1, true));
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
                return "Test failed: The board should have the board creater as the board owner.";
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
        /// This function tests requierment 12.
        /// The function tests a user joining a board and checks for the expected errors.
        /// </summary>
        public void TestJoinBoard()
        {
            Console.WriteLine("\nTesting JoinBoard():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "work"; //boardId=1 for userEmail1, boardId=3 for userEmail2
            string boardName2 = "cleaning"; //boardId=2 for userEmail1
            int boardId1 = 1; //"work", members: userEmail1
            int boardId2 = 2; //"cleaning", members: userEmail1
            int boardId3 = 3; //"work", members: userEmail2
            int boardId4 = 11; //board does not exist 

            //A user that does not exist should not be able to join a board
            Response response1 = Response.FromJson(_boardService.JoinBoard(testEmail4, boardId1));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that does not exist tries to join the board.");

            //A user that is not logged should not be able to join a board
            Response response2 = Response.FromJson(_boardService.JoinBoard(testEmail3, boardId1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that is not logged tries to join the board.");

            //A user can not join a board that does not exist.
            Response response3 = Response.FromJson(_boardService.JoinBoard(testEmail1, boardId4));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after trying to join a board that does not exist.");

            //A user should not be able to join a board that he already joined
            Response response4 = Response.FromJson(_boardService.JoinBoard(testEmail1, boardId1));
            if (response4.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after trying to join a board that was joined already.");

            //A user should not be able to join a board with the same name of a board he already joined
            Response response5 = Response.FromJson(_boardService.JoinBoard(testEmail2, boardId1));
            if (response5.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after trying to join a board with a name that was joined before.");

            //A user should be able to join the board
            Response response6 = Response.FromJson(_boardService.JoinBoard(testEmail2, boardId2));
            if (response6.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while joining board. " + response6.ErrorMessage);
            else
            {
                Console.WriteLine(CheckBoard(response6.DeserializeReturnValue<Board>(), testEmail1, boardName2, false));
                Response response = Response.FromJson(_boardService.GetBoard(testEmail2, boardName2));
                if (response.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured while joining board. " + response.ErrorMessage);
                else
                    Console.WriteLine(CheckBoard(response.DeserializeReturnValue<Board>(), testEmail1, boardName2, false));
            }
        }

        /// <summary>
        /// This function tests requierment 12,14.
        /// The function tests a user leaving a board and checks for the expected errors.
        /// </summary>
        public void TestLeaveBoard()
        {
            Console.WriteLine("\nTesting LeaveBoard():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "work"; //boardId=1 for userEmail1, boardId=3 for userEmail2
            string boardName2 = "cleaning"; //boardId=2 for userEmail1
            string boardName3 = "TV shows"; //board that does not exist

            //A user that does not exist should not be able to leave a board
            Response response1 = Response.FromJson(_boardService.LeaveBoard(testEmail4, boardName1));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that does not exist tries to leave the board.");

            //A user that is not logged should not be able to leave a board
            Response response2 = Response.FromJson(_boardService.LeaveBoard(testEmail3, boardName1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that is not logged tries to leave the board.");

            //A user can not leave a board that does not exist.
            Response response3 = Response.FromJson(_boardService.LeaveBoard(testEmail1, boardName3));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after trying to leave a board that does not exist.");

            //A user should not be able to leave a board he is not a member of
            Response response4 = Response.FromJson(_boardService.LeaveBoard(testEmail2, boardName1));
            if (response4.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after user trying to leave a board he is not a member of.");

            //A user should not be able to leave a board when he is the owner
            Response response5 = Response.FromJson(_boardService.LeaveBoard(testEmail1, boardName1));
            if (response5.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after user trying to leave a board he is the owner of.");

            //A user should be able to leave the board
            Response response6 = Response.FromJson(_boardService.LeaveBoard(testEmail2, boardName2));
            if (response6.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while leaving board. " + response6.ErrorMessage);
            else
            {
                Response response = Response.FromJson(_boardService.GetBoard(testEmail2, boardName2));
                if (response.ErrorOccured)
                    Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
                else
                    Console.WriteLine("Test failed: User should not be in the board after leaving the board. ");
            }
        }

        /// <summary>
        /// This function tests requirement 9.
        /// The function attempts to remove a board with valid and invalid parameters,
        /// and checks for the expected errors.
        /// </summary>
        private void TestRemoveBoard()
        {
            Console.WriteLine("\nTesting RemoveBoard():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "work", "cleaning"
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in, boards: "work" 
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "work";
            string boardName2 = "cleaning";
            string boardName3 = "grocery shopping";

            //A user that does not exist should not be able to remove a board
            Response response1 = Response.FromJson(_boardService.RemoveBoard(testEmail4, boardName1));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after removing a board for a user that does not exist.");

            //A user that is not logged in should not be able to remove a board
            Response response2 = Response.FromJson(_boardService.RemoveBoard(testEmail3, boardName1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after removing a board for a user that is not logged in.");

            //A user should not be able to remove a board he does not have
            Response response3 = Response.FromJson(_boardService.RemoveBoard(testEmail1, boardName3));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after removing a board that does not exist.");

            //Removing a board for a user that exists and logged in
            Response response4 = Response.FromJson(_boardService.RemoveBoard(testEmail1, boardName1));
            if (response4.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while removing board. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test successful: Board has been removed.");

            //A user should not be able te remove a board that has been removed already
            Response response5 = Response.FromJson(_boardService.RemoveBoard(testEmail1, boardName1));
            if (response5.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error when removing the same board twice.");

            //A user should be able to remove another board
            Response response6 = Response.FromJson(_boardService.RemoveBoard(testEmail1, boardName2));
            if (response6.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while removing board. " + response6.ErrorMessage);
            else
                Console.WriteLine("Test successful: Board has been removed.");

            //A different user should be able to remove a board with the same name as another user's board
            Response response7 = Response.FromJson(_boardService.RemoveBoard(testEmail2, boardName1));
            if (response7.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while removing board. " + response7.ErrorMessage);
            else
                Console.WriteLine("Test successful: Board has been removed.");
        }

        /// The function attempts to get a board with valid and invalid parameters,
        /// and checks for the expected errors and the validity of the board's values.
        private void TestGetBoard()
        {
            Console.WriteLine("\nTesting GetBoard():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            _boardService.CreateBoard(testEmail1, boardName1);
            _boardService.CreateBoard(testEmail1, boardName2);
            _boardService.CreateBoard(testEmail2, boardName1);

            //A user that does not exist should not be able to get a board
            Response response1 = Response.FromJson(_boardService.GetBoard(testEmail4, boardName1));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after getting a board for a user that does not exist.");

            //A user that is not logged in should not be able to get a board
            Response response2 = Response.FromJson(_boardService.GetBoard(testEmail3, boardName1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after getting a board for a user that is not logged in.");

            //Users should not be able to get a board they have not created
            Response response3 = Response.FromJson(_boardService.GetBoard(testEmail1, boardName3));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after getting a board that does not exist.");

            //Getting a board for a user that exists and logged in and checking the returned board is valid
            Response response4 = Response.FromJson(_boardService.GetBoard(testEmail1, boardName1));
            if (response4.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating board. " + response4.ErrorMessage);
            else
                Console.WriteLine(CheckBoard(response4.DeserializeReturnValue<Board>(), testEmail1, boardName1, false));

            //Getting a board for a user that exists and logged in and checking the returned board is valid
            Response response5 = Response.FromJson(_boardService.GetBoard(testEmail2, boardName1));
            if (response5.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating board. " + response5.ErrorMessage);
            else
                Console.WriteLine(CheckBoard(response5.DeserializeReturnValue<Board>(), testEmail2, boardName1, false));
        }

        /// <summary>
        /// This function tests requirement 10.
        /// The function attempts to limit columns in different boards with valid and invalid
        /// parameters, and checks for the expected errors and the necessary updates.
        /// </summary>
        private void TestLimitColumn()
        {
            Console.WriteLine("\nTesting LimitColumn():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in, boards: "homework", "chores"
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in, boards: "homework" 
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework";
            string boardName2 = "chores";
            string boardName3 = "family";
            int columnNumber1 = 1;
            int columnNumber2 = 7;
            int limit1 = 10;
            int limit2 = -7;

            //A user that does not exist should not be able to limit a column
            Response response1 = Response.FromJson(_boardService.LimitColumn(testEmail4, boardName1, columnNumber1, limit1));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since user does not exist.");

            //A user that is not logged in should not be able to limit a column
            Response response2 = Response.FromJson(_boardService.LimitColumn(testEmail3, boardName1, columnNumber1, limit1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since user is not logged in.");

            // UPDATED TEST
            //Users should not be able to limit a column in a board they have not created
            Response response3 = Response.FromJson(_boardService.LimitColumn(testEmail2, boardName2, columnNumber1, limit1));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since board does not exist.");

            //Limitting a column for a user that exists and logged in
            Response response4 = Response.FromJson(_boardService.LimitColumn(testEmail1, boardName1, columnNumber1, limit1));
            if (response4.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating board. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test successful: The cloumn has been limited.");

            //Can not limit a cloumn that does not exist in the board
            Response response5 = Response.FromJson(_boardService.LimitColumn(testEmail1, boardName1, columnNumber2, limit1));
            if (response5.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after limitting a column that does not exist.");

            //Can not limit a column to an invalid number
            Response response6 = Response.FromJson(_boardService.LimitColumn(testEmail1, boardName1, columnNumber1, limit2));
            if (response6.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error since the limit number is not valid.");
        }

        // <summary>
        /// This function tests requierment 13.
        /// The function tests a user transfering board ownership and checks for the expected errors.
        /// </summary>
        public void TestTransferOwner()
        {

            Console.WriteLine("\nTesting TransferOwner():");

            string testEmail1 = "danafriedman@gmail.com"; //user exists and logged in
            string testEmail2 = "eyalooshgerman@gmail.com"; //user exists and logged in
            string testEmail3 = "yonilevi@gmail.com"; //user exists and isn't logged in
            string testEmail4 = "dana@gmail.com"; //user doesn't exist
            string boardName1 = "homework"; //boardId=1 for userEmail1, boardId=3 for userEmail2
            string boardName2 = "chores"; //boardId=2 for userEmail1
            string boardName3 = "family"; //board that does not exist
            int boardId1 = 4; //"homework", members: userEmail1
            int boardId2 = 5; //"chores", members: userEmail1, userEmail2
            int boardId3 = 6; //"homework", members: userEmail2
            int boardId4 = 11; //board does not exist 
            _boardService.JoinBoard(testEmail2, boardId2);

            //A user that does not exist should not be able to transfer board ownership
            Response response1 = Response.FromJson(_boardService.TransferOwner(testEmail4, boardName1, testEmail1));
            if (response1.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response1.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that does not exist tries to transfer ownership.");

            //Can't transfer ownership to a user that does not exist
            Response response2 = Response.FromJson(_boardService.TransferOwner(testEmail1, boardName1, testEmail4));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage + "\n");
            else
                Console.WriteLine("Test failed: Expected an error after transfering ownership to a user that does not exist.");

            //A user that is not logged in should not be able to transfer ownership
            Response response3 = Response.FromJson(_boardService.TransferOwner(testEmail3, boardName1, testEmail1));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user that is not logged tries to transfer board ownership.");

            //A user should not be able to transfer ownership of a board that does not exist
            Response response4 = Response.FromJson(_boardService.TransferOwner(testEmail1, boardName3, testEmail2));
            if (response4.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after transfering ownership of a board that does not exist.");

            //A user who is not the owner can not transfer ownership.
            Response response5 = Response.FromJson(_boardService.TransferOwner(testEmail2, boardName2, testEmail1));
            if (response5.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after a user who is not the owner tries to transfer ownership.");

            //Can't transfer ownership to a user who is not a board member.
            Response response6 = Response.FromJson(_boardService.TransferOwner(testEmail1, boardName1, testEmail2));
            if (response6.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after transfering ownership to a user who is not a board member.");

            //A user should be able to transfer board ownership.
            Response response7 = Response.FromJson(_boardService.TransferOwner(testEmail1, boardName2, testEmail2));
            if (response7.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while transfering ownership. " + response7.ErrorMessage);
            else
            {
                Response response = Response.FromJson(_boardService.GetBoard(testEmail2, boardName2));
                if (response.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured while transfering ownership. " + response.ErrorMessage);
                else
                    Console.WriteLine(CheckBoard(response.DeserializeReturnValue<Board>(), testEmail2, boardName2, false));
            }
        }
    }
}
