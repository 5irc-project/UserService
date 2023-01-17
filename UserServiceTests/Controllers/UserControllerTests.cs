using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserService.Models.Repository;
using UserService.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Moq;
using UserService.Exceptions;
using UserService.DTO;
using System.Security.Claims;
using AutoMapper;
using UserService.Mappers;
using UserService.Helpers;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

namespace UserService.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        public UserControllerTests()
        {
        }

        

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
        public void CreateOrGetUserTest_ReturnsOk()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();

            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByStringAsync("demo_email@gmail.com")).ReturnsAsync(demoUser);

            var controller = new UserController(_userRepository, GetAutomapper());

            var result = controller.CreateOrGetUser(demoUserDTO);
            Assert.AreEqual(result.Result.Value, demoUserDTO);
        }

        [TestMethod()]
        public void CreateOrGetUserTest_ReturnsNotOk()
        {
            var _userRepository = Mock.Of<IDataRepository<User>>();

            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByStringAsync("demo_email@gmail.com")).ThrowsAsync(new UserNotFoundException());

            var controller = new UserController(_userRepository, GetAutomapper());

            var result = controller.CreateOrGetUser(demoUserDTO);
            Assert.IsInstanceOfType(result.Result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod()]
        public void PutUser_ReturnsOk()
        {
            var user = GetDemoUser();
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository, GetAutomapper());
            MockControllerContext(controller);
            

            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(user.UserId)).ReturnsAsync(demoUser);

            var actionResult = controller.PutUser(demoUserDTO).Result;
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod()]
        public void PutUser_ReturnsBadRequest()
        {
            var user = GetDemoUser();
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository, GetAutomapper());
            MockControllerContext(controller);
            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(user.UserId)).ReturnsAsync(demoUser);
            Mock.Get(_userRepository).Setup(m => m.UpdateAsync(demoUser, demoUser)).ThrowsAsync(new UserDBUpdateException());

            var actionResult = (ObjectResult)controller.PutUser(demoUserDTO).Result;
            //Assert.AreEqual(actionResult.StatusCode, (int)HttpStatusCode.BadR);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [TestMethod()]
        public void PutUser_ReturnsNotFound()
        {
            var user = GetDemoUser();
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository, GetAutomapper());
            MockControllerContext(controller);
            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(user.UserId)).ThrowsAsync(new UserNotFoundException());

            var actionResult = (ObjectResult)controller.PutUser(demoUserDTO).Result;
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void DeleteUser_ReturnsOk()
        {
            var user = GetDemoUser();
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository, GetAutomapper());
            MockControllerContext(controller);
            var demoUser = GetDemoUser();
            var demoUserDTO = UserMapper.ModelToDto(demoUser);

            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(user.UserId)).ReturnsAsync(demoUser);
            //Mock.Get(_userRepository).Setup(m => m.DeleteAsync(demoUser));

            var actionResult = controller.DeleteUser().Result.Result;
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod()]
        public void DeleteUser_ReturnsNotFound()
        {
            var user = GetDemoUser();
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository, GetAutomapper());
            MockControllerContext(controller);
            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(user.UserId)).ThrowsAsync(new UserNotFoundException());
            //Mock.Get(_userRepository).Setup(m => m.DeleteAsync(demoUser));

            var actionResult = controller.DeleteUser().Result.Result;
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
        }

        [TestMethod()]
        public void DeleteUser_ReturnsNotOk()
        {
            var user = GetDemoUser();
            var _userRepository = Mock.Of<IDataRepository<User>>();
            var controller = new UserController(_userRepository, GetAutomapper());
            MockControllerContext(controller);
            var demoUser = GetDemoUser();
            Mock.Get(_userRepository).Setup(m => m.GetByIdAsync(user.UserId)).ReturnsAsync(demoUser);
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

        private ClaimsPrincipal MockUserWithClaims()
        {
            var user = GetDemoUser();
            var claims = new List<Claim>()
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("displayName", user.Nom),
            };
            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        private void MockControllerContext(UserController controller)
        {
            var user = MockUserWithClaims();
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }

        private IMapper GetAutomapper()
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile(new AutoMapperProfiles());
                });

            IMapper mapper = new Mapper(mapperConfig);

            return mapper;
        }
    }
}