using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Models.Repository;
using UserService.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using UserService.Models.DataManager;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Net;
using Moq;
using UserService.Exceptions;

namespace UserService.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        public UserControllerTests() {}

        [TestMethod]
        public void GetAllUsers_ReturnsOk()
        {
            //Setup
            List<User> lUsers = new List<User>
            {
                new User() { UserId = 1, Nom = "Demo name", Email = "toto@gmail.com", ProfilePictureUrl = "toto_picture.png" },
                new User() { UserId = 2, Nom = "Demo name", Email = "toto2@gmail.com", ProfilePictureUrl = "toto_picture.png" },
                new User() { UserId = 3, Nom = "Demo name", Email = "toto3@gmail.com", ProfilePictureUrl = "toto_picture.png" }
            };

            // Arrange
            var _userRepository = Mock.Of<IDataRepository<User>>();
            Mock.Get(_userRepository).Setup(m => m.GetAllAsync()).ReturnsAsync(lUsers);
            var controller = new UserController(_userRepository);

            CollectionAssert.AreEqual(lUsers, controller.GetAllUsers().Result.Value.ToList());
            //CollectionAssert.AreEqual(lUsers, _userRepository.GetAllAsync().Result.Value.ToList());
        }


        [TestMethod()]
        public void GetUserByIdTest_ReturnsNotOk()
        {
            // Arrange
            var _userRepository = Mock.Of<IDataRepository<User>>();
            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(4)).ThrowsAsync(new UserNotFoundException());

            var controller = new UserController(_userRepository);

            var result = controller.GetUserById(4);
            Assert.IsInstanceOfType(result.Result.Result, typeof(NotFoundObjectResult));
        }

        //[TestMethod()]
        //public void GetUserByIdTest_ReturnsOk()
        //{
        //    _context.Users.Add(GetDemoUser());

        //    User userTest = _context.Users.Single(u => u.UserId == 3);
        //    User? userApi = _controller.GetUserById(3).Result.Value;

        //    Assert.IsNotNull(userApi);
        //    Assert.AreEqual(userTest, userApi);
        //}

        //[TestMethod()]
        //public void GetUserByEmailTest_ReturnsNotOk()
        //{
        //    var result = _controller.GetUserByEmail("test@test.com").Result;

        //    Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        //}

        //[TestMethod()]
        //public void GetUserByEmailTest_ReturnsOk()
        //{
        //    _context.Users.Add(GetDemoUser());

        //    User userTest = _context.Users.Single(u => u.Email == "toto@gmail.com");
        //    User? userApi = _controller.GetUserByEmail("toto@gmail.com").Result.Value;

        //    Assert.IsNotNull(userApi);
        //    Assert.AreEqual(userTest, userApi);
        //}

        //[TestMethod()]
        //public void PostUser_ModelValidated_CreationOk()
        //{
        //    // Arrange
        //    Random rnd = new Random();
        //    int digit = rnd.Next(1, 1000000000);
        //    // Le mail doit être unique donc 2 possibilités :
        //    // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
        //    // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE du WS => la décommenter
        //    User userToTest = new User()
        //    {
        //        Nom = "MACHIN",
        //        Email = "machin" + digit + "@gmail.com",
        //        ProfilePictureUrl = "toto_picture.png"
        //    };
        //    // Act
        //    var result = _controller.PostUser(userToTest).Result; // Result pour appeler la méthode async de manière synchrone, afin d'obtenir le résultat
        //    var result2 = _controller.GetUserByEmail(userToTest.Email);
        //    var actionResult = result2.Result as ActionResult<User>;
        //    // Assert
        //    Assert.IsInstanceOfType(actionResult.Value, typeof(User), "Not a user");
        //    User? userRetrieved = _context.Users.Where(u => u.Email.ToUpper() == userToTest.Email.ToUpper()).FirstOrDefault();
        //    // On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
        //    // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
        //    userToTest.UserId = userRetrieved.UserId;
        //    Assert.AreEqual(userRetrieved, userToTest, "Users are not the same");
        //}

        //[TestMethod()]
        //public void PostUser_ModelNotValidated()
        //{
        //    User userToTestWithoutMail = new User()
        //    {
        //        Nom = "MACHIN",
        //        ProfilePictureUrl = "toto_picture.png"
        //    };
        //    Assert.AreEqual((int)HttpStatusCode.InternalServerError, _controller.PostUser(userToTestWithoutMail).Result);
        //}

        //User GetDemoUser()
        //{
        //    return new User() { UserId = 3, Nom = "Demo name", Email = "toto@gmail.com", ProfilePictureUrl = "toto_picture.png" };
        //}
    }
}