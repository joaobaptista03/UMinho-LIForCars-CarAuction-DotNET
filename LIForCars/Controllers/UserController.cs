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

        //POST api/User
        [HttpPost]
        public ActionResult<User> Create([FromForm] User newUser)
        {
            try
            {
                _repository.Create(newUser);
                _repository.SaveChanges();

                return Ok(newUser);
            } catch (Exception)
            {
                return BadRequest();
            }
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

    }
}