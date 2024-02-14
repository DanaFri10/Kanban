using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    /// <summary>
    /// This class consists of tests to the UserService part of the application.
    /// </summary>
    internal class TestUserService
    {
        UserService _userService;

        public TestUserService(UserService userService)
        {
            _userService = userService;
        }

        internal void Test()
        {

            Console.WriteLine("\nTesting UserService:");
            TestCreateUser();
            TestGetUser();
            TestLogin();
            TestLogout();
            TestChangePassword();
            TestUserExists();
        }

        /// <summary>
        /// This function tests Requirements 1,2,3,7: Registration of new users using an email and password.
        /// 
        /// The function tests the CreateUser(..) method in the UserService class.
        /// The function attempts to create users with invalid and valid credentials,
        /// and checks for the expected errors and the validity of the created users.
        /// </summary>
        private void TestCreateUser()
        {
            Console.WriteLine("\nTesting CreateUser():");
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
            Response response = Response.FromJson(_userService.CreateUser(testEmail1, testPassword1));
            if (response.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should not return an error when creating a valid user.
            Response response2 = Response.FromJson(_userService.CreateUser(testEmail1, testPassword2));
            if (response2.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating user. " + response2.ErrorMessage);
            else
            {
                // The created user should exist in the system, have the same email address as the input,
                // and be logged-in to the system.
                Console.WriteLine("User has been created. Checking user credentials:");
                Console.WriteLine(CheckUser(testEmail1, false, true));
            }

            // Should return an error when creating another user with the same email address.
            Response response3 = Response.FromJson(_userService.CreateUser(testEmail1, testPassword2));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating another user with the same email address.");

            // Should not return an error when creating a valid user.
            Response response4 = Response.FromJson(_userService.CreateUser(testEmail2, testPassword2));
            if (response4.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating user. " + response4.ErrorMessage);
            else
            {
                // The created user should exist in the system, have the same email address as the input,
                // and be logged-in to the system.
                Console.WriteLine("User has been created. Checking user credentials:");
                Console.WriteLine(CheckUser(testEmail2, false, true));
            }

            // Should return an error when creating a user with invalid password.
            Response response5 = Response.FromJson(_userService.CreateUser(testEmail3, testPassword4));
            if (response5.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response5.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid password.
            Response response6 = Response.FromJson(_userService.CreateUser(testEmail3, testPassword5));
            if (response6.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response6.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid password.
            Response response7 = Response.FromJson(_userService.CreateUser(testEmail3, testPassword6));
            if (response7.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response7.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid password.
            Response response8 = Response.FromJson(_userService.CreateUser(testEmail3, testPassword7));
            if (response8.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response8.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid password.
            Response response9 = Response.FromJson(_userService.CreateUser(testEmail3, testPassword8));
            if (response9.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response9.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid password.");

            // Should return an error when creating a user with invalid email address.
            Response response10 = Response.FromJson(_userService.CreateUser(testEmail4, testPassword2));
            if (response10.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response10.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should return an error when creating a user with invalid email address.
            Response response11 = Response.FromJson(_userService.CreateUser(testEmail5, testPassword2));
            if (response11.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response11.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should return an error when creating a user with invalid email address.
            Response response12 = Response.FromJson(_userService.CreateUser(testEmail6, testPassword2));
            if (response12.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response12.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should return an error when creating a user with invalid email address.
            Response response13 = Response.FromJson(_userService.CreateUser(testEmail7, testPassword2));
            if (response13.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response13.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should return an error when creating a user with invalid email address.
            Response response14 = Response.FromJson(_userService.CreateUser(testEmail8, testPassword2));
            if (response14.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response14.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after creating a user with invalid email address.");

            // Should not return an error when creating a valid user.
            Response response15 = Response.FromJson(_userService.CreateUser(testEmail3, testPassword3));
            if (response15.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while creating user. " + response15.ErrorMessage);
            else
            {
                // The created user should exist in the system, have the same email address as the input,
                // and be logged-in to the system.
                Console.WriteLine("User has been created. Checking user credentials:");
                Console.WriteLine(CheckUser(testEmail3, false, true));
            }
        }

        /// <summary>
        /// This function tests Requirement 1: Identifying a user by its email address.
        /// 
        /// This function tests the GetUser(..) method in the UserService class.
        /// The function attempts to get users with valid and invalid email addresses, and checks 
        /// for the expected errors and the validity of the users' credentials, using a helper function.
        /// </summary>
        private void TestGetUser()
        {
            Console.WriteLine("\nTesting GetUser():");
            string testEmail0 = null;
            string testEmail1 = "galpinto@hotmail.com";
            string testEmail2 = "gal@gmail.com";

            // Should return an error when getting a user with null email address.
            Console.WriteLine(CheckUser(testEmail0, true, false));

            // Should return an error when getting a user with non-existent email address.
            Console.WriteLine(CheckUser(testEmail1, true, false));

            // The user should exist in the system, have the same email address as the input,
            // and be logged-in to the system.
            Console.WriteLine(CheckUser(testEmail2, false, true));
        }

        /// <summary>
        /// This function is a helper function for checking the validity of a user in the system.
        /// The function attempts to get the user with the specified email address, and checks 
        /// for the expected errors and if the returned user has the expected credentials.
        /// </summary>
        /// <param name="expectedEmail">The user's email address.</param>
        /// <param name="expectedError">True if expects an error, false otherwise.</param>
        /// <param name="expectedLoggedIn">True if expects the user to be logged-in, false otherwise.</param>
        /// <returns>A string containing details of the test's result.</returns>
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
        /// This function tests Requirements 1,8: logging in using an email and password.
        /// 
        /// The function tests the Login(..) method in the UserService class.
        /// The function attempts to login into users with valid and invalid credentials,
        /// and checks for the expected errors and the necessary user updates.
        /// </summary>
        private void TestLogin()
        {
            string testEmail1 = "galpinto@hotmail.com";
            string testEmail2 = "gal@gmail.com";
            string testEmail3 = "testing@tests.com";
            string testPassword1 = "12345678";
            string testPassword2 = "Abc123456";

            Console.WriteLine("\nSetting up TestLogin() using Logout().");
            try
            {
                _userService.Logout(testEmail2);
                _userService.Logout(testEmail3);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed: An error has occured while setting up TestLogin() using Logout().");
            }

            Console.WriteLine("\nTesting Login():");

            // Should return an error when attempting to login into a non-existent user.
            Response response = Response.FromJson(_userService.Login(testEmail1, testPassword1));
            if (response.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to login into a non-existent user.");

            // Should return an error when attempting to login with incorrect password.
            Response response2 = Response.FromJson(_userService.Login(testEmail2, testPassword1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successfull: Expected error has occured.");
            else
                Console.WriteLine("Test failed: Expected an error after attempting to login with incorrect password.");

            // Should not return an error when logging in to a user with valid email and password.
            Response response3 = Response.FromJson(_userService.Login(testEmail2, testPassword2));
            if (response3.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while logging in to user. " + response3.ErrorMessage);
            else
            {
                // The user should be logged in to the system.
                Console.WriteLine("Logged in to user. Checking user update:");
                Console.WriteLine(CheckUser(testEmail2, false, true));
            }

            // Should return an error when attempting to login into a logged in user.
            Response response4 = Response.FromJson(_userService.Login(testEmail2, testPassword2));
            if (response4.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response4.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to login into a logged in user.");
        }

        /// <summary>
        /// This function tests Requirement 8: logging out of a user.
        /// 
        /// This function tests the Logout(..) method in UserService.
        /// The function attempts to log out of valid and invalid users,
        /// and checks for the expected errors and the necessary user updates. 
        /// </summary>
        private void TestLogout()
        {
            Console.WriteLine("\nTesting Logout():");
            string testEmail1 = "galpinto@hotmail.com";
            string testEmail2 = "gal@gmail.com";

            // Should return an error when attempting to log out from a non-existent user.
            Response response = Response.FromJson(_userService.Logout(testEmail1));
            if (response.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to log-out from a non-existent user.");

            // Should not return an error when logging out from a valid user.
            Response response2 = Response.FromJson(_userService.Logout(testEmail2));
            if (response2.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while logging out of a user. " + response2.ErrorMessage);
            else
            {
                // The user should be logged out of the system.
                Console.WriteLine("Logged out of the user. Checking user update:");
                Console.WriteLine(CheckUser(testEmail2, false, false));
            }

            // Should return an error when attempting to log out from a logged-out user.
            Response response3 = Response.FromJson(_userService.Logout(testEmail2));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to log out from a logged-out user.");
        }

        /// <summary>
        /// This function tests Requirement 2: Validy checks of password update.
        /// 
        /// The function tests the ChangePassword(..) method in the UserService class.
        /// The function attempts to change passwords of valid and invalid users,
        /// and checks for the expected errors and the necessary user updates.
        /// </summary>
        private void TestChangePassword()
        {
            Console.WriteLine("\nTesting ChangePassword():");
            string testEmail1 = "galpinto@hotmail.com";
            string testEmail2 = "gal@gmail.com";
            string testPassword1 = "Abc123456";
            string testPassword2 = "Abcd12345";
            string testPassword3 = "1235";

            _userService.Login(testEmail2, testPassword1);

            // Should return an error when attempting to change password of a non-existent user.
            Response response = Response.FromJson(_userService.ChangePassword(testEmail1, testPassword1, testPassword2));
            if (response.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to change password of a non-existent user.");

            // Should return an error when attempting to change password to an invalid password.
            Response response2 = Response.FromJson(_userService.ChangePassword(testEmail2, testPassword1, testPassword3));
            if (response2.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response2.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to change password to an invalid password.");

            // Should return an error when attempting to change password with incorrect old password.
            Response response3 = Response.FromJson(_userService.ChangePassword(testEmail2, testPassword2, testPassword1));
            if (response3.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response3.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after attempting to change password with incorrect old password.");

            // Should not return an error when attempting to change password with valid parameters.
            Response response4 = Response.FromJson(_userService.ChangePassword(testEmail2, testPassword1, testPassword2));
            if (response4.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while changing password of a user. " + response4.ErrorMessage);
            else
            {
                // The user's password should be updated.
                Console.WriteLine("Changed the password of the user. Checking user update:");
                _userService.Logout(testEmail2);

                // Should return an error when attempting to login into the user with the old password.
                Response retryOldPassword = Response.FromJson(_userService.Login(testEmail2, testPassword1));
                if (retryOldPassword.ErrorOccured)
                    Console.WriteLine("Test successful: Expected error has occured. " + retryOldPassword.ErrorMessage);
                else
                    Console.WriteLine("Test failed: Expected an error after attempting to login " +
                                      "into the user with the old password after changing it.");

                // Should not return an error when attempting to login into the user with the new password.
                Response retryNewPassword = Response.FromJson(_userService.Login(testEmail2, testPassword2));
                if (retryNewPassword.ErrorOccured)
                    Console.WriteLine("Test failed: An error has occured when attempting to login " +
                                      "into a user with the new password after changing it." + retryNewPassword.ErrorMessage);
                else
                {
                    User user = retryNewPassword.DeserializeReturnValue<User>();
                    if (user.EmailAddress != testEmail2)
                        Console.WriteLine($"Test failed: Expected the returned email to be {testEmail2}, but received {user.EmailAddress}.");
                    else if (!user.IsLoggedIn)
                        Console.WriteLine("Test failed: Expected the returned user to be logged in after logging with the updated password.");
                    else
                        Console.WriteLine("Test successful: Logged in successfully after changing the password of the user.");
                }
            }
        }

        /// <summary>
        /// This function tests Requirement 1: Identifying a user by its email address.
        /// 
        /// This function tests the UserExists(..) method in the UserService class.
        /// The function attempts to check if valid and invalid users are existing in the system,
        /// and checks for the expected errors and return values.
        /// </summary>
        private void TestUserExists()
        {
            Console.WriteLine("\nTesting UserExists():");
            string testEmail0 = null;
            string testEmail1 = "galpinto@hotmail.com";
            string testEmail2 = "gal@gmail.com";

            // Should return an error when attempting to send a null email address.
            Response response = Response.FromJson(_userService.UserExists(testEmail0));
            if (response.ErrorOccured)
                Console.WriteLine("Test successful: Expected error has occured. " + response.ErrorMessage);
            else
                Console.WriteLine("Test failed: Expected an error after executing UserExists() with null email address.");

            // Should not return an error when attempting to check user existence of a user.
            Response response2 = Response.FromJson(_userService.UserExists(testEmail1));
            if (response2.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while executing UserExists(). " + response2.ErrorMessage);
            else
            {
                // Should return 'false' because the specified user doesn't exist.
                bool result = response2.DeserializeReturnValue<bool>();
                if (result == false)
                    Console.WriteLine("Test successful: UserExists() returned 'false' as expected.");
                else
                    Console.WriteLine("Test failed: After executing UserExists(), expected return value of 'false' but received: " + result);
            }

            // Should not return an error when attempting to check user existence of a user.
            Response response3 = Response.FromJson(_userService.UserExists(testEmail2));
            if (response3.ErrorOccured)
                Console.WriteLine("Test failed: An error has occured while executing UserExists(). " + response3.ErrorMessage);
            else
            {
                // Should return 'true' because the specified user exists.
                bool result = response3.DeserializeReturnValue<bool>();
                if (result == true)
                    Console.WriteLine("Test successful: UserExists() returned 'true' as expected.");
                else
                    Console.WriteLine("Test failed: After executing UserExists(), expected return value of 'true' but received: " + result);
            }
        }
    }
}