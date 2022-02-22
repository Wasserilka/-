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
    [Route("api/books")]
    [Authorize]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBooksRepository Repository;
        private readonly IMapper Mapper;
        private ICreateBookRequestValidator CreateBookRequestValidator;
        private IUpdateBookRequestValidator UpdateBookRequestValidator;
        private IDeleteBookRequestValidator DeleteBookRequestValidator;
        private IGetByTitleBookRequestValidator GetByTitleBookRequestValidator;
        private IGetByAuthorBookRequestValidator GetByAuthorBookRequestValidator;
        public BooksController(
            IBooksRepository repository, 
            IMapper mapper,
            ICreateBookRequestValidator createBookRequestValidator,
            IUpdateBookRequestValidator updateBookRequestValidator,
            IDeleteBookRequestValidator deleteBookRequestValidator,
            IGetByTitleBookRequestValidator getByTitleBookRequestValidator,
            IGetByAuthorBookRequestValidator getByAuthorBookRequestValidator)
        {
            Mapper = mapper;
            Repository = repository;
            CreateBookRequestValidator = createBookRequestValidator;
            UpdateBookRequestValidator = updateBookRequestValidator;
            DeleteBookRequestValidator = deleteBookRequestValidator;
            GetByTitleBookRequestValidator = getByTitleBookRequestValidator;
            GetByAuthorBookRequestValidator = getByAuthorBookRequestValidator;
        }

        [HttpGet("get/all")]
        public IActionResult GetAll()
        {
            var books = Repository.GetAll();

            var response = new GetAllBooksResponse();

            foreach (var book in books)
            {
                response.Books.Add(Mapper.Map<BookDto>(book));
            }

            return Ok(response);
        }

        [HttpGet("get/title/{title}")]
        public IActionResult GetByTitle([FromRoute] string title)
        {
            var request = new GetByTitleBookRequest(title);
            var validation = new OperationResult<GetByTitleBookRequest>(GetByTitleBookRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            var book = Repository.GetByTitle(request);

            var response = new GetByTitleBookResponse(Mapper.Map<BookDto>(book));

            return Ok(response);
        }

        [HttpGet("get/author/{author}")]
        public IActionResult GetByAuthor([FromRoute] string author)
        {
            var request = new GetByAuthorBookRequest(author);
            var validation = new OperationResult<GetByAuthorBookRequest>(GetByAuthorBookRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            var book = Repository.GetByAuthor(request);

            var response = new GetByAuthorBookResponse(Mapper.Map<BookDto>(book));

            return Ok(response);
        }

        [HttpPost("create/{title}/{author}/{date}")]
        public IActionResult Create([FromRoute] string title, [FromRoute] string author, [FromRoute] DateTimeOffset date)
        {
            var request = new CreateBookRequest(title, author, date);
            var validation = new OperationResult<CreateBookRequest>(CreateBookRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            Repository.Create(request);

            return Ok();
        }

        [HttpPut("update/{title}/{new_title}/{new_author}/{new_date}")]
        public IActionResult Update([FromRoute] string title, [FromRoute] string new_title, [FromRoute] string new_author, [FromRoute] DateTimeOffset new_date)
        {
            var request = new UpdateBookRequest(title, new_title, new_author, new_date);
            var validation = new OperationResult<UpdateBookRequest>(UpdateBookRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            Repository.Update(request);

            return Ok();
        }

        [HttpDelete("delete/{title}")]
        public IActionResult Delete([FromRoute] string title)
        {
            var request = new DeleteBookRequest(title);
            var validation = new OperationResult<DeleteBookRequest>(DeleteBookRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            Repository.Delete(request);

            return Ok();
        }
    }
}

