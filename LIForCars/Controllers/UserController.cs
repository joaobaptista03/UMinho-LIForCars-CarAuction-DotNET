using Microsoft.AspNetCore.Mvc;
using LIForCars.Models;
using LIForCars.Data.Interfaces;


namespace LIForCars.Controllers {

    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        //GET aрі/User <- MUDAR PERMISSÕES PARA QUE APENAS ADMINS POSSAM ACEDER
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var result = _repository.GetAll();

            return Ok(result);
        }

        //GET api/User/ByUsername/{username} <- MUDAR PERMISSÕES PARA QUE APENAS ADMINS POSSAM ACEDER
        [HttpGet("ByUsername/{username}")]
        public ActionResult<User> GetByUsername(string username)
        {
            var result = _repository.GetByUsername(username);

            return Ok(result);
        }

        //GET api/User/ById/{id} <- MUDAR PERMISSÕES PARA QUE APENAS ADMINS POSSAM ACEDER
        [HttpGet("ById/{id}")]
        public ActionResult<User> GetById(int id)
        {
            var result = _repository.GetById(id);

            return Ok(result);
        }


        // PUT api/User/{username}  <- MUDAR PERMISSÕES PARA QUE APENAS ADMINS POSSAM ACEDER
        [HttpPut("{username}")]
        public ActionResult<User> Update(string username, User User)
        {
            if (username != User.Username)
            {
                return BadRequest();
            }

            _repository.Update(User);
            return NoContent();
        }

        // DELETE api/User/{username}  <- MUDAR PERMISSÕES PARA QUE APENAS ADMINS POSSAM ACEDER
        [HttpDelete("{username}")]
        public ActionResult Delete(string username)
        {
            var User = _repository.GetByUsername(username);
            if (User == null)
            {
                return NotFound();
            }

            _repository.Delete(User);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpGet("checkUnique")]
        public ActionResult<bool> CheckUnique(string field, string value)
        {
            bool isUnique = false;

            switch (field.ToLower())
            {
                case "nif":
                    isUnique = !_repository.NifExists(int.Parse(value));
                    break;
                case "cc":
                    isUnique = !_repository.CcExists(int.Parse(value));
                    break;
                case "phone":
                    isUnique = !_repository.PhoneExists(int.Parse(value));
                    break;
                case "username":
                    isUnique = !_repository.UsernameExists(value);
                    break;
                case "email":
                    isUnique = !_repository.EmailExists(value);
                    break;
                default:
                    return BadRequest("Invalid field specified.");
            }

            return Ok(isUnique);
        }

        [HttpGet("checkPassword")]
        public ActionResult<bool> CheckPassword(string username, string password)
        {
            bool isCorrect = _repository.CheckPassword(username, password);

            if (!isCorrect) return BadRequest("Incorrect password.");

            return Ok(isCorrect);
        }

        [HttpPost("login")]
        public ActionResult<User> Login([FromForm] string username, [FromForm] string password)
        {
            if (!_repository.CheckPassword(username, password)) return BadRequest("Incorrect password.");

            var user = _repository.GetByUsername(username);
            if (user == null) return BadRequest("User not found.");

            return Ok(user);
        }

        [HttpPost("register")]
        public ActionResult<User> Register([FromForm] User newUser)
        {
            if (_repository.NifExists(newUser.Nif)) return BadRequest("NIF already exists.");
            if (_repository.CcExists(newUser.CC)) return BadRequest("CC already exists.");
            if (_repository.PhoneExists(newUser.Phone)) return BadRequest("Phone already exists.");
            if (_repository.UsernameExists(newUser.Username)) return BadRequest("Username already exists.");
            if (_repository.EmailExists(newUser.Email)) return BadRequest("Email already exists.");
            if (newUser.Password.Contains(' ')) return BadRequest("Username cannot contain spaces.");

            try
            {
                _repository.Create(newUser);
                _repository.SaveChanges();
                return Ok(newUser);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("userPage/{userId}")]
        public async Task<ActionResult<User>> GetUserById([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid userId.");
            }

            var user = await _repository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
    
            return Ok(user);
        }

    }
}
