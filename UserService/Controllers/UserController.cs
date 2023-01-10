using Microsoft.AspNetCore.Mvc;
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
            return dataRepository.GetAll();
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
            //var utilisateur = await _context.Utilisateurs.Include(u => u.NotesUtilisateur).SingleOrDefaultAsync(u => u.UtilisateurId == id);
            var user = dataRepository.GetById(id);

            if (user.Value == null)
            {
                return NotFound("User not found");
            }

            return user.Value;
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="200">Successfully return the user</response>
        /// <response code="404">User not found</response>
        // GET: api/users/toto@toto.fr
        [Route("[action]/{email}")]
        [HttpGet]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            //var utilisateur = await _context.Utilisateurs.Include(u => u.NotesUtilisateur).SingleOrDefaultAsync(u => u.Mail == email);
            var user = dataRepository.GetByString(email);

            if (user.Value == null)
            {
                return NotFound("User not found");
            }

            return user.Value;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="utilisateur">User to create</param>
        /// <returns>Http response</returns>
        /// <response code="201">When the user is successfully created</response>
        /// <response code="400">Invalid user data</response>
        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(user);

            return CreatedAtAction("GetUserById", new { id = user.UserId }, user);
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
        public async Task<IActionResult> PutUtilisateur(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest("Invalid user data");
            }

            var userToUpdate = dataRepository.GetById(id);

            if (userToUpdate.Value == null)
            {
                return NotFound("User not found");

            }

            await dataRepository.UpdateAsync(userToUpdate.Value, user);

            return NoContent();
        }

        /// <summary>
        /// Delete an user
        /// </summary>
        /// <returns>The related user</returns>
        /// <response code="204">Successfully deleted the user</response>
        /// <response code="404">User not found</response>
        // DELETE: api/users/5
        [ProducesResponseType(typeof(User), 204)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {

            var user = dataRepository.GetById(id);

            if (user.Value == null)
            { 
                return NotFound("User not found");
            }

            await dataRepository.DeleteAsync(user.Value);

            return user.Value;
        }
    }
}
