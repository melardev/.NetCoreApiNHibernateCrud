using System.Net;
using System.Threading.Tasks;
using ApiCoreNHibernateCrud.Entities;
using ApiCoreNHibernateCrud.Enums;
using ApiCoreNHibernateCrud.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiCoreNHibernateCrud.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private readonly TodoService _todosService;

        public TodosController(TodoService todosService)
        {
            _todosService = todosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var todos = await _todosService.FetchMany();
            return new OkObjectResult(todos);
        }

        [HttpGet]
        [Route("pending")]
        public async Task<IActionResult> GetPending([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var todos = await _todosService.FetchMany(TodoShow.Pending);
            return Ok(todos);
        }

        [HttpGet]
        [Route("completed")]
        public async Task<IActionResult> GetCompleted([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var todos = await _todosService.FetchMany(TodoShow.Completed);
            return StatusCode((int) HttpStatusCode.OK, Json(todos));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTodoDetails(int id)
        {
            var todo = await _todosService.GetById(id);
            if (todo == null)
                // return StatusCode((int) HttpStatusCode.NotFound, new ErrorDtoResponse("Not found"));
                return StatusCode((int) HttpStatusCode.NotFound, new JsonResult(new
                {
                    Success = false,
                    FullMessages = new[]
                    {
                        "Not Found"
                    }
                }));
            return Json(todo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] Todo todo)
        {
            await _todosService.CreateTodo(todo);
            var response = new ObjectResult(todo);
            response.StatusCode = (int) HttpStatusCode.Created;
            return response;
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] Todo todo)
        {
            var todoFromDb = await _todosService.GetById(id);
            if (todoFromDb == null)
                return NotFound(Json(new
                {
                    Success = false,
                    FullMessages = new[]
                    {
                        "Not Found"
                    }
                }));
            return new OkObjectResult(await _todosService.Update(todoFromDb, todo));
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todoFromDb = await _todosService.GetById(id);
            if (todoFromDb == null)
                return new NotFoundObjectResult(Json(new
                {
                    Success = false,
                    FullMessages = new[]
                    {
                        "Not Found"
                    }
                }));
            _todosService.Delete(id);

            return NoContent();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await _todosService.DeleteAll();
            return new NoContentResult();
        }
    }
}