using Microsoft.AspNetCore.Mvc;
using LIForCars.Models;
using LIForCars.Data.Interfaces;

namespace LIForCars.Controllers {

    [Route("api/coworker")]
    [ApiController]
    public class CoworkerController : ControllerBase
    {
        private readonly ICoworkerRepository _repository;

        public CoworkerController(ICoworkerRepository repository)
        {
            _repository = repository;
        }

        //GET aрі/cowerker
        [HttpGet]

        public ActionResult<IEnumerable<Coworker>> GetAll()
        {
            var result = _repository.GetAll();

            return Ok(result);
        }
            

        //GET api/coworker/{id}
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Coworker>> GetById(int id)
        {
            var result = _repository.GetById(id);

            return Ok(result);
        }

        //POST api/coworker
        [HttpPost("{id}")]
        public ActionResult<Coworker> Create(Coworker newCoworker)
        {
            try
            {
                _repository.Create(newCoworker);
                _repository.SaveChanges();

                return Ok(newCoworker);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/coworker/{id}
        [HttpPut("{id}")]
        public ActionResult<Coworker> Update(int id, Coworker coworker)
        {
            try
            {
                var existingCoworker = _repository.GetById(id);
                if (existingCoworker == null)
                {
                    return NotFound();
                }

                existingCoworker.FirstName = coworker.FirstName;
                existingCoworker.LastName = coworker.LastName;
                existingCoworker.Nif = coworker.Nif;

                _repository.Update(existingCoworker);
                _repository.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/coworker/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var coworker = _repository.GetById(id);
            if (coworker == null)
            {
                return NotFound();
            }

            _repository.Delete(coworker);
            _repository.SaveChanges();

            return NoContent();
        }

    }
}