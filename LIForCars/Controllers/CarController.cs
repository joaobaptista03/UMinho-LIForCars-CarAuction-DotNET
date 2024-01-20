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

        [HttpGet("{plate}")]
        public ActionResult<Car> GetByPlate(string plate)
        {
            var car = _repository.GetByPlate(plate);
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
            return CreatedAtAction(nameof(GetByPlate), new { plate = car.Plate }, car);
        }

        [HttpPut("{plate}")]
        public ActionResult Update(string plate, [FromBody] Car car)
        {
            if (plate != car.Plate)
            {
                return BadRequest();
            }

            _repository.Update(car);
            return NoContent();
        }

        [HttpDelete("{plate}")]
        public ActionResult Delete(string plate)
        {
            _repository.Delete(plate);
            return NoContent();
        }
    }
}
