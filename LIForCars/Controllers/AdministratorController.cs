using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;

namespace LIForCars.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministratorController : ControllerBase {
        private readonly IAdministratorRepository _repository;

        public AdministratorController(IAdministratorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Administrator>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{username}")]
        public ActionResult<Administrator> GetByUsername(string username)
        {
            var administrator = _repository.GetByUsername(username);
            if (administrator == null)
            {
                return NotFound();
            }
            return Ok(administrator);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Administrator administrator)
        {
            if (_repository.ContractNrExists(administrator.ContractNr)) return BadRequest("ContractNr already exists");
            if (_repository.NifExists(administrator.Nif)) return BadRequest("Nif already exists");
            if (_repository.CCExists(administrator.CC)) return BadRequest("CC already exists");
            if (_repository.PhoneExists(administrator.Phone)) return BadRequest("Phone already exists");
            if (_repository.UsernameExists(administrator.Username)) return BadRequest("Username already exists");
            if (_repository.EmailExists(administrator.Email)) return BadRequest("Email already exists");
            if (administrator.Password.Contains(' ')) return BadRequest("Password cannot contain spaces");

            _repository.Create(administrator);
            return CreatedAtAction(nameof(GetByUsername), new { username = administrator.Username }, administrator);
        }

        [HttpPut("{username}")]
        public ActionResult Update(string username, [FromBody] Administrator administrator)
        {
            if (username != administrator.Username)
            {
                return BadRequest();
            }

            _repository.Update(administrator);
            return NoContent();
        }

        [HttpDelete("{username}")]
        public ActionResult Delete(string username)
        {
            _repository.Delete(username);
            return NoContent();
        }
    }
}