using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;

namespace LIForCars.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _repository;

        public CarController(ICarRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Car>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Car> GetById(int id)
        {
            var car = _repository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Car car)
        {
            if (_repository.PlateExists(car.Plate)) return BadRequest("Plate already exists");
            if (_repository.CertificateNrExists(car.CertificateNr)) return BadRequest("CertificateNr already exists");

            _repository.Create(car);
            return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _repository.Update(car);
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
