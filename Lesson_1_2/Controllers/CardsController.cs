using Microsoft.AspNetCore.Mvc;
using Lesson_1_2.DAL.Repositories;
using Lesson_1_2.DAL.Responses;
using Lesson_1_2.DAL.DTO;
using Lesson_1_2.Requests;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Lesson_1_2.Controllers
{
    [Route("api/cards")]
    [Authorize]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private ICardsRepository Repository;
        private readonly IMapper Mapper;

        public CardsController(ICardsRepository repository, IMapper mapper)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet("get/all")]
        public IActionResult GetAll()
        {
            var cards = Repository.GetAll();

            var response = new GetAllCardsResponse()
            {
                Cards = new List<CardDto>()
            };

            foreach (var card in cards)
            {
                response.Cards.Add(Mapper.Map<CardDto>(card));
            }

            return Ok(response);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var request = new GetByIdCardRequest(id);
            var card = Repository.GetById(request);

            var response = new GetAllCardsResponse()
            {
                Cards = new List<CardDto>()
            };

            response.Cards.Add(Mapper.Map<CardDto>(card));

            return Ok(response);
        }

        [HttpPost("create/{number}/{name}/{date}/{type}")]
        public IActionResult Create([FromRoute] long number, [FromRoute] string name, [FromRoute] DateTimeOffset date, [FromRoute] string type)
        {
            var request = new CreateCardRequest(number, name, date, type);

            Repository.Create(request);

            return Ok();
        }

        [HttpPut("update/{id}/{number}/{name}/{date}/{type}")]
        public IActionResult Update([FromRoute] int id, [FromRoute] long number, [FromRoute] string name, [FromRoute] DateTimeOffset date, [FromRoute] string type)
        {
            var request = new UpdateCardRequest(id, number, name, date, type);

            Repository.Update(request);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var request = new DeleteCardRequest(id);

            Repository.Delete(request);

            return Ok();
        }
    }
}

