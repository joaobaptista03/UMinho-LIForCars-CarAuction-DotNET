using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;

namespace LIForCars.Controllers
{
    [ApiController]
    [Route("api/Car")]
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

        [HttpGet("checkUnique")]
        public ActionResult<bool> CheckUnique(string field, string value)
        {
            bool isUnique = false;

            switch (field.ToLower())
            {
                case "plate":
                    isUnique = !_repository.PlateExists(value);
                    break;
                case "certificatenr":
                    isUnique = !_repository.CertificateNrExists(int.Parse(value));
                    break;
                default:
                    return BadRequest("Invalid field specified.");
            }

            return Ok(isUnique);
        }

        [HttpPost("create")]
    public ActionResult Create([FromForm] Car car)
    {
        if (_repository.PlateExists(car.Plate)) 
        {
            ModelState.AddModelError("Plate", "Plate already exists");
            return BadRequest(ModelState);
        }

        if (_repository.CertificateNrExists(car.CertificateNr)) 
        {
            ModelState.AddModelError("CertificateNr", "CertificateNr already exists");
            return BadRequest(ModelState);
        }

        if (!ModelState.IsValid)
        {
            // Log validation errors here
            return BadRequest(ModelState);
        }

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
