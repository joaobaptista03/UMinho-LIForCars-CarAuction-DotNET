using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;

namespace LIForCars.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase {
        private readonly IBidRepository _repository;

        public BidController(IBidRepository repository) {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Bid>> GetAll() {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Bid> GetById(int id) {
            var bid = _repository.GetById(id);
            if (bid == null) {
                return NotFound();
            }
            return Ok(bid);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Bid bid) {
            _repository.Create(bid);
            return CreatedAtAction(nameof(GetById), new { id = bid.Id }, bid);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Bid bid) {
            if (id != bid.Id) {
                return BadRequest();
            }

            _repository.Update(bid);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            _repository.Delete(id);
            return NoContent();
        }
    }
}