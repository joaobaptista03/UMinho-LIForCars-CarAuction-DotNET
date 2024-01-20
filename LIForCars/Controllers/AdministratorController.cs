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