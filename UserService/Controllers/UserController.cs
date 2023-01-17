using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using UserService.DTO;
using UserService.Exceptions;
using UserService.Mappers;
using UserService.Models.EntityFramework;
using UserService.Models.Repository;

namespace UserService.Controllers
{

    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IDataRepository<User> dataRepository; 

        public UserController(IDataRepository<User> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="utilisateur">User to create</param>
        /// <returns>Http response</returns>
        /// <response code="201">When the user is successfully created</response>
        /// <response code="400">Invalid user data</response>
        /// <response code="500">Failed to add the user to the database</response>
        // POST: api/users
        [ActionName("create")]
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                await dataRepository.AddAsync(user);
                return CreatedAtAction("get", new { id = user.UserId }, user);
            }
            catch (UserDBCreationException e)
            {
                return StatusCode(500, "Failed to add the user: is the e-mail unique?");
            }

        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Http response with all users</returns>
        /// <response code="200">Successfully return all users</response>
        // GET: api/User/all
        [ActionName("all")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await dataRepository.GetAllAsync();
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="200">Successfully return the user</response>
        /// <response code="404">User not found</response>
        //GET: api/User/get/{"id"}
        [ActionName("get")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await dataRepository.GetByIdAsync(id);
                return user.Value;
            }
            catch (UserNotFoundException)
            {
                return NotFound("User not found");
            }
        }

        /// <summary>
        /// Get user info by Token
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="200">Successfully return the user's information</response>
        /// <response code="404">User not found</response>
        // GET: api/User/profile
        [ActionName("profile")]
        [HttpGet]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            var id = GetUserIdFromClaims();
            try
            {
                var user = await dataRepository.GetByIdAsync(id);
                return UserMapper.ModelToDto(user.Value);
            }
            catch (UserNotFoundException)
            {
                return NotFound("User not found");
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="200">Successfully return the user</response>
        /// <response code="201">Successfully create and return the user</response>
        // GET: api/User/CreateOrGetUser/toto@toto.fr
        [ActionName("CreateOrGetUser")]
        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDTO>> CreateOrGetUser(UserDTO userDto)
        {
            try
            {
                var user = await dataRepository.GetByStringAsync(userDto.Email);
                return UserMapper.ModelToDto(user.Value);
            }
            catch (UserNotFoundException)
            {
                User user = UserMapper.DtoToModel(userDto);
                await dataRepository.AddAsync(user);
                return CreatedAtAction("get", new { id = user.UserId }, user);
            }
        }

        /// <summary>
        /// Update an user
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="204">Successfully updated the user</response>
        /// <response code="400">Invalid user data</response>
        /// <response code="404">User not found</response>
        // PUT: api/User/UpdateUser/
        [ActionName("update")]
        [HttpPut]
        [ProducesResponseType(typeof(UserDTO), 204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize]
        public async Task<IActionResult> PutUser(UserDTO modifiedUser)
        {
            var id = GetUserIdFromClaims();
            try
            {
                var user = await dataRepository.GetByIdAsync(id);
                await dataRepository.UpdateAsync(user.Value, UserMapper.DtoToModel(modifiedUser));
                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return NotFound("User not found");
            }
            catch (UserDBUpdateException)
            {
                return BadRequest("Could not update database");
            }
        }

        /// <summary>
        /// Delete the current user retrieved in the token
        /// </summary>
        /// <returns>202</returns>
        /// <response code="204">Successfully deleted the user</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Failed to delete the user</response>
        // DELETE: api/User/delete
        [ActionName("delete")]
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<ActionResult<User>> DeleteUser()
        {
            var id = GetUserIdFromClaims();
            try
            {
                var user = await dataRepository.GetByIdAsync(id);
                await dataRepository.DeleteAsync(user.Value);
                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return NotFound("User not found");
            }
            catch (UserDBDeletionException)
            {
                return StatusCode(500, "Failed to delete the user");
            }
        }

        private int GetUserIdFromClaims()
        {
            var id = User.Claims.First(c => c.Type == "UserId").Value;
            return int.Parse(id);
        }
    }
}
