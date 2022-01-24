using Microsoft.AspNetCore.Mvc;
using Lesson_1_2.Models;
using Lesson_1_2.Repositories;
using Lesson_1_2.Responses;
using AutoMapper;

namespace Lesson_1_2.Controllers
{
    [Route("api/cards")]
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
            var card = Repository.GetById(id);

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
            Repository.Create(new Card { Number = number, HolderName = name, ExpirationDate = date, Type = type });

            return Ok();
        }

        [HttpPut("update/{id}/{number}/{name}/{date}/{type}")]
        public IActionResult Update([FromRoute] int id, [FromRoute] long number, [FromRoute] string name, [FromRoute] DateTimeOffset date, [FromRoute] string type)
        {
            Repository.Update(id, new Card { Number = number, HolderName = name, ExpirationDate = date, Type = type });

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            Repository.Delete(id);

            return Ok();
        }
    }
}

