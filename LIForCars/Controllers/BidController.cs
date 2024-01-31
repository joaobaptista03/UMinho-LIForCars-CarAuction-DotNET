using LIForCars.Data.Components;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;

namespace LIForCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase {
        private readonly IBidRepository _repository;
        private readonly IAuctionRepository _auctionRepository;

        private readonly IUserRepository _userRepository;


        public BidController(IBidRepository repository,IAuctionRepository auctionRepository, IUserRepository userRepository) {
            _repository = repository;
            _auctionRepository = auctionRepository;     
            _userRepository = userRepository;   
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

        //[Consumes("application/json")]
        [HttpPost("create")]
        public ActionResult Create([FromBody] Bid bid)
        {
            // Verify the AuctionId in bid is correct

            // Save the bid to the database
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