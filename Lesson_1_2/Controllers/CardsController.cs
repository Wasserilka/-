using Microsoft.AspNetCore.Mvc;
using Lesson_1_2.DAL.Repositories;
using Lesson_1_2.DAL.Responses;
using Lesson_1_2.DAL.DTO;
using Lesson_1_2.Requests;
using Lesson_1_2.Validation.Validators;
using Lesson_1_2.Validation.Service;
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
        private ICreateCardRequestValidator CreateCardRequestValidator;
        private IUpdateCardRequestValidator UpdateCardRequestValidator;
        private IDeleteCardRequestValidator DeleteCardRequestValidator;
        private IGetByIdCardRequestValidator GetByIdCardRequestValidator;
        public CardsController(
            ICardsRepository repository, 
            IMapper mapper,
            ICreateCardRequestValidator createCardRequestValidator,
            IUpdateCardRequestValidator updateCardRequestValidator,
            IDeleteCardRequestValidator deleteCardRequestValidator,
            IGetByIdCardRequestValidator getByIdCardRequestValidator)
        {
            Mapper = mapper;
            Repository = repository;
            CreateCardRequestValidator = createCardRequestValidator;
            UpdateCardRequestValidator = updateCardRequestValidator;
            DeleteCardRequestValidator = deleteCardRequestValidator;
            GetByIdCardRequestValidator = getByIdCardRequestValidator;
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
            var validation = new OperationResult<GetByIdCardRequest>(GetByIdCardRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

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
            var validation = new OperationResult<CreateCardRequest>(CreateCardRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            Repository.Create(request);

            return Ok();
        }

        [HttpPut("update/{id}/{number}/{name}/{date}/{type}")]
        public IActionResult Update([FromRoute] int id, [FromRoute] long number, [FromRoute] string name, [FromRoute] DateTimeOffset date, [FromRoute] string type)
        {
            var request = new UpdateCardRequest(id, number, name, date, type);
            var validation = new OperationResult<UpdateCardRequest>(UpdateCardRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            Repository.Update(request);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var request = new DeleteCardRequest(id);
            var validation = new OperationResult<DeleteCardRequest>(DeleteCardRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            Repository.Delete(request);

            return Ok();
        }
    }
}

