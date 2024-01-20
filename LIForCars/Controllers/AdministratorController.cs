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

        [HttpGet("{id}")]
        public ActionResult<Administrator> GetById(int id)
        {
            var administrator = _repository.GetById(id);
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
            return CreatedAtAction(nameof(GetById), new { id = administrator.Id }, administrator);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Administrator administrator)
        {
            if (id != administrator.Id)
            {
                return BadRequest();
            }

            _repository.Update(administrator);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}