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

        //GET api/User/{id} <- MUDAR PERMISSÕES PARA QUE APENAS ADMINS POSSAM ACEDER
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<User>> GetById(int id)
        {
            var result = _repository.GetById(id);

            return Ok(result);
        }

        // PUT api/User/{id}  <- MUDAR PERMISSÕES PARA QUE APENAS ADMINS POSSAM ACEDER
        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, User User)
        {
            try
            {
                var existingUser = _repository.GetById(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Name = User.Name;
                existingUser.Nif = User.Nif;
                existingUser.CC = User.CC;
                existingUser.Address = User.Address;
                existingUser.Phone = User.Phone;
                existingUser.Gender = User.Gender;
                existingUser.BirthDate = User.BirthDate;
                existingUser.Username = User.Username;
                existingUser.Email = User.Email;
                existingUser.Password = User.Password;

                _repository.Update(existingUser);
                _repository.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/User/{id}  <- MUDAR PERMISSÕES PARA QUE APENAS ADMINS POSSAM ACEDER
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var User = _repository.GetById(id);
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
    }
}