#nullable disable
using Microsoft.AspNetCore.Mvc;
using Lesson_1_2.Interfaces;
using Lesson_1_2.Models;
using Lesson_1_2.Repositories;
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
    }
}

