using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoListService.Infrastructure;
using TodoListService.Infrastructure.Exceptions;
using TodoListService.Models;


namespace TodoListService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodoListController : ControllerBase
    {

        private readonly AppSettings _appSettings;
        private readonly TodoListDBContext _dbContext;
        private ILogger<TodoListController> _logger;
        public TodoListController(TodoListDBContext dbContext, IOptions<AppSettings> appSettings, ILogger<TodoListController> logger)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        // GET: api/<TodoListController>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Get all ToDo list started.");

            var result = _dbContext.ToDo.Where(x => x.isActive == 1);

            _logger.LogInformation("Get all ToDo list completed.");

            return Ok(result);
        }

        // GET: api/<TodoListController>
        [HttpGet]
        [Route("getbyuser")]
        public IActionResult GetByUser()
        {
            _logger.LogInformation($"Get all ToDo list for user {UserId} started.");

            IQueryable<Todo> result = _dbContext.ToDo.Where(x => x.userId == UserId && x.isActive == 1);

            _logger.LogInformation($"Get all ToDo list for user {UserId} completed.");

            return Ok(result);
        }


        // POST api/<TodoListController>
        [HttpPost]
        public IActionResult Post(Todo todo)
        {
            todo.userId = UserId;

            _logger.LogInformation($"Save ToDo item by {todo.userId} started.");

            _dbContext.ToDo.Add(todo);
            _dbContext.SaveChanges();

            _logger.LogInformation($"Save ToDo item by {todo.userId} completed.");

            return Ok(todo);
        }

        // PUT api/<TodoListController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Todo todo)
        {
            
            todo.id = id;
            _logger.LogInformation($"Update ToDo item by {todo.userId} started.");

            _dbContext.ToDo.Update(todo);
            _dbContext.SaveChanges();

            _logger.LogInformation($"Save ToDo item by {todo.userId} completed.");

            return Ok(todo);
        }

        //Patch only one column
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] int isCompleted)
        {
            _logger.LogInformation($"Patch ToDo item by {UserId} started.");

            var todo = _dbContext.ToDo.SingleOrDefault(x => x.id == id);
            if (todo == null)
                throw new RecordNotFoundException<int>(id);

            //Update only isCompleted field.
            todo.isCompleted = isCompleted;
            _dbContext.ToDo.Attach(todo);
            _dbContext.Entry(todo).Property(x => x.isCompleted).IsModified = true;
            _dbContext.SaveChanges();

            _logger.LogInformation($"Patch ToDo item by {UserId} completed.");

            return Ok(todo);
        }

        // DELETE api/<TodoListController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Delete ToDo item by {UserId} started.");

            var todo = _dbContext.ToDo.SingleOrDefault(x => x.id == id);
            if (todo == null)
                throw new RecordNotFoundException<int>(id);

            //Soft Delete: Update only isActive field.
            todo.isActive = 0;
            _dbContext.ToDo.Attach(todo);
            _dbContext.Entry(todo).Property(x => x.isActive).IsModified = true;
            _dbContext.SaveChanges();

            _logger.LogInformation($"Delete ToDo item by {UserId} completed.");

            return Ok(todo);
        }


        //Get the userId by Claims token, this will work only for Authorized method of class members.
        private string UserId
        {
            get
            {
                return User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
            }

        }
    }
}
