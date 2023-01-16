using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using UserService.Exceptions;
using UserService.Mappers;
using UserService.Models.EntityFramework;
using UserService.Models.Repository;

namespace UserService.Controllers
{

    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IDataRepository<User> dataRepository; 

        public UserController(IDataRepository<User> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Http response with all users</returns>
        /// <response code="200">Successfully return all users</response>
        // GET: api/users
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
        // GET: api/users/5
        [Route("[action]/{id}")]
        [HttpGet]
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
        /// Get user by email
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="200">Successfully return the user</response>
        /// <response code="404">User not found</response>
        // GET: api/users/toto@toto.fr
        [Route("[action]")]
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
                return CreatedAtAction("GetUserById", new { id = user.UserId }, user);
            }
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
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                await dataRepository.AddAsync(user);
                return CreatedAtAction("GetUserById", new { id = user.UserId }, user);
            }
            catch (UserDBCreationException e)
            {
                return StatusCode(500, "Failed to add the user: is the e-mail unique?");
            }

        }

        /// <summary>
        /// Update an user
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="204">Successfully updated the user</response>
        /// <response code="400">Invalid user data</response>
        /// <response code="404">User not found</response>
        // PUT: api/users/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), 204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest("Invalid user data");
            }
            try
            {
                var userToUpdate = await dataRepository.GetByIdAsync(id);
                var userToReturn = await dataRepository.UpdateAsync(userToUpdate.Value, user);
                return new OkObjectResult(userToReturn.Value);
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
        /// Delete an user
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="204">Successfully deleted the user</response>
        /// <response code="404">User not found</response>
        // DELETE: api/users/5
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
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
    }
}
