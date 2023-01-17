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
using Microsoft.AspNetCore.Http;
using UserService.DTO;
using UserService.Mappers;
using System.Security.Claims;

namespace UserService.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        public UserControllerTests() {}

        //[TestMethod]
        //public void GetAllUsers_ReturnsOk()
        //{
        //    //Setup
        //    List<User> lUsers = new List<User>
        //    {
        //        new User() { UserId = 1, Nom = "Demo name", Email = "toto@gmail.com", ProfilePictureUrl = "toto_picture.png" },
        //        new User() { UserId = 2, Nom = "Demo name", Email = "toto2@gmail.com", ProfilePictureUrl = "toto_picture.png" },
        //        new User() { UserId = 3, Nom = "Demo name", Email = "toto3@gmail.com", ProfilePictureUrl = "toto_picture.png" }
        //    };

        //    // Arrange
        //    var _userRepository = Mock.Of<IDataRepository<User>>();
        //    Mock.Get(_userRepository).Setup(m => m.GetAllAsync()).ReturnsAsync(lUsers);
        //    var controller = new UserController(_userRepository);

        //    CollectionAssert.AreEqual(lUsers, controller.GetAllUsers().Result.Value.ToList());
        //    //CollectionAssert.AreEqual(lUsers, _userRepository.GetAllAsync().Result.Value.ToList());
        //}

        //[TestMethod()]
        //public void GetUserByIdTest_ReturnsOk()
        //{
        //    var _userRepository = Mock.Of<IDataRepository<User>>();
        //    var demoUser = GetDemoUser();
        //    Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(1)).ReturnsAsync(demoUser);

        //    var controller = new UserController(_userRepository);

        //    var result = controller.GetUserById(1);
        //    Assert.AreEqual(result.Result.Value, demoUser);
        //}

        //[TestMethod()]
        //public void GetUserByIdTest_ReturnsNotOk()
        //{
        //    var _userRepository = Mock.Of<IDataRepository<User>>();
        //    Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(4)).ThrowsAsync(new UserNotFoundException());

        //    var controller = new UserController(_userRepository);

        //    var result = controller.GetUserById(4);
        //    Assert.IsInstanceOfType(result.Result.Result, typeof(NotFoundObjectResult));
        //}

        //[TestMethod()]
        //public void PostUser_CreationOk()
        //{
        //    var _userRepository = Mock.Of<IDataRepository<User>>();
        //    var controller = new UserController(_userRepository);
        //    var demoUser = GetDemoUser();

        //    var actionResult = controller.PostUser(demoUser).Result.Result;
        //    Assert.IsInstanceOfType(actionResult, typeof(CreatedAtActionResult));
        //}

        //[TestMethod()]
        //public void PostUser_CreationNotOk()
        //{
        //    var _userRepository = Mock.Of<IDataRepository<User>>();
        //    var controller = new UserController(_userRepository);
        //    var demoUser = GetDemoUser();
        //    Mock.Get(_userRepository).Setup(m => m.AddAsync(demoUser)).ThrowsAsync(new UserDBCreationException());

        //    var actionResult = (ObjectResult) controller.PostUser(demoUser).Result.Result;
        //    Assert.AreEqual(actionResult.StatusCode, (int)HttpStatusCode.InternalServerError);
        //}

        // Can't test model not ok as it's managed by annotations: not our code therefore not our responsibility

        [TestMethod()]
        public void GetUserByEmailTest_ReturnsOk()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();

            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByStringAsync("demo_email@gmail.com")).ReturnsAsync(demoUser);

            var controller = new UserController(_userRepository);

            var result = controller.CreateOrGetUser(demoUserDTO);
            Assert.AreEqual(result.Result.Value, demoUserDTO);
        }

        [TestMethod()]
        public void GetUserByEmailTest_ReturnsNotOk()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();

            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByStringAsync("demo_email@gmail.com")).ThrowsAsync(new UserNotFoundException());

            var controller = new UserController(_userRepository);

            var result = controller.CreateOrGetUser(demoUserDTO);
            Assert.IsInstanceOfType(result.Result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod()]
        public void PutUser_ReturnsOk()
        {
            var claims = new List<Claim>()
            {
                new Claim("UserId", "3")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository);

            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(3)).ReturnsAsync(demoUser);

            var actionResult = controller.PutUser(demoUserDTO).Result;
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod()]
        public void PutUser_ReturnsBadRequest()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository);
            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);
            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(3)).ReturnsAsync(demoUser);
            Mock.Get(_userRepository).Setup(m => m.UpdateAsync(demoUser, demoUser)).ThrowsAsync(new UserDBUpdateException());

            var actionResult = (ObjectResult)controller.PutUser(demoUserDTO).Result;
            //Assert.AreEqual(actionResult.StatusCode, (int)HttpStatusCode.BadR);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod()]
        public void PutUser_ReturnsNotFound()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository);
            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);
            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(3)).ThrowsAsync(new UserNotFoundException());

            var actionResult = (ObjectResult)controller.PutUser(demoUserDTO).Result;
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void DeleteUser_ReturnsOk()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository);
            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);
            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(1)).ReturnsAsync(demoUser);
            //Mock.Get(_userRepository).Setup(m => m.DeleteAsync(demoUser));

            var actionResult = controller.DeleteUser().Result.Result;
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod()]
        public void DeleteUser_ReturnsNotFound()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository);
            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(1)).ThrowsAsync(new UserNotFoundException());
            //Mock.Get(_userRepository).Setup(m => m.DeleteAsync(demoUser));

            var actionResult = controller.DeleteUser().Result.Result;
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void DeleteUser_ReturnsNotOk()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository);
            var demoUser = GetDemoUser();
            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(1)).ReturnsAsync(demoUser);
            Mock.Get(_userRepository).Setup(m => m.DeleteAsync(demoUser)).ThrowsAsync(new UserDBDeletionException());

            var actionResult = (ObjectResult) controller.DeleteUser().Result.Result;
            //Assert.IsInstanceOfType(actionResult, );
            Assert.AreEqual(actionResult.StatusCode, (int) HttpStatusCode.InternalServerError);
        }

        User GetDemoUser()
        {
            return new User() { UserId = 3, Nom = "Demo name", Email = "demo_email@gmail.com", ProfilePictureUrl = "demo_picture.png" };
        }
        UserDTO GetSecondDemoUser()
        {
            return new UserDTO() { UserId = 5, Nom = "Second Demo name", Email = "second_demo_email@gmail.com", ProfilePictureUrl = "second_demo_picture.png" };
        }
    }
}