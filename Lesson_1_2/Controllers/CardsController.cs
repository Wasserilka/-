#nullable disable
using Microsoft.AspNetCore.Mvc;
using Lesson_1_2.Interfaces;
using Lesson_1_2.Models;
using Lesson_1_2.Repositories;
using Lesson_1_2.Responses;
using System;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

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

        [HttpPost("create/{number}/{name}/{date}/{type}")]
        public IActionResult Create([FromRoute] long number, [FromRoute] string name, [FromRoute] DateTimeOffset date, [FromRoute] string type)
        {
            Repository.Create(new Card { Number = number, HolderName = name, ExpirationDate = date, Type = type });

            return Ok();
        }
    }
}

