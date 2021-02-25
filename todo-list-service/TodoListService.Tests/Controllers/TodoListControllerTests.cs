using Xunit;
using Microsoft.EntityFrameworkCore;
using TodoListService.Models;
using TodoListService.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TodoListService.Infrastructure.Exceptions;

namespace TodoListService.Controllers.Tests
{
    public class TodoListControllerTests
    {
        private readonly DbContextOptions<TodoListDBContext> _dbContextOptions;
        private readonly TodoListDBContext _dbContext;
        private readonly TodoListController _controller;

        //Database path and deafault username to mock Identity Claims are harcoded for now, we can move it to configuration file.
        private readonly string _connectionString = "Data Source = D:\\invw\\demo\\todo-list\\todo-list-service\\TodoListService.Tests\\App_Data\\TodoListDB.Test.db";
        private readonly string _defaultUser = "ajay.verma";

        
        public TodoListControllerTests()
        {
            //Setup the db context
            _dbContextOptions = new DbContextOptionsBuilder<TodoListDBContext>()
                .UseSqlite(_connectionString)
                .Options;
            _dbContext = new TodoListDBContext(_dbContextOptions);

            //Mock logger object and AppSettings
            var mock = new Mock<ILogger<TodoListController>>();
            ILogger<TodoListController> logger = mock.Object;
            logger = Mock.Of<ILogger<TodoListController>>();
            IOptions<AppSettings> appSettings = Options.Create(new AppSettings());

            _controller = new TodoListController(_dbContext, appSettings, logger);

            //Mock claim identity and passing default user from settings
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, _defaultUser),
            }, "mock"));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }


        [Fact()]//Get: get all active todo list
        public void GetTest()
        {
            var result = _controller.Get();

            var value = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<IQueryable<Todo>>(value.Value);
            var count = items.Count();
            Assert.Equal(8, count);
        }

        [Fact()]//Get: get all active todo list by Loggedin User
        public void GetByUserTest()
        {
            //Mocking claim identity and passing default user from settings
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, _defaultUser),
            }, "mock"));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };

            var result = _controller.GetByUser();

            var value = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<IQueryable<Todo>>(value.Value);
            var count = items.Count();
            Assert.Equal(6, count);

        }

        [Fact()]//Post(Todo): add new todo list success test case.
        public void PostAddSuccessTest()
        {
            var result = _controller.Post(new Todo { description="adding to do list from unit test post."});

            var value = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsAssignableFrom<Todo>(value.Value);
            Assert.Equal("adding to do list from unit test post.", item.description);
            Assert.Equal(1, item.isActive);

            //revert changes by soft deleting newly added records
            result = _controller.Delete((int)item.id);
            Assert.IsType<OkObjectResult>(result);
            item = Assert.IsAssignableFrom<Todo>(value.Value);
            Assert.Equal(0, item.isActive);
        }

        [Fact()]//Put(Id, Todo): Update todo list success test case.
        public void PutSuccessTest()
        {
            var todo = new Todo { id = 10, description = "Create Class Diagram", isActive = 1, isCompleted = 1, userId = "lalita.verma" };
            var result = _controller.Put(10, todo);
            var value = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsAssignableFrom<Todo>(value.Value);
            Assert.Equal(1, item.isCompleted);

            //revert changes
            todo.isCompleted = 0;
            result = _controller.Put(10, todo);
            value = Assert.IsType<OkObjectResult>(result);
            item = Assert.IsAssignableFrom<Todo>(value.Value);
            Assert.Equal(0, item.isCompleted);
        }

        [Fact()]//Patch(Id, IsCmpleted): Update todo list success test case.
        public void PatchSuccessTest()
        {
            var result = _controller.Patch(10, 1);
            var value = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsAssignableFrom<Todo>(value.Value);
            Assert.Equal(1, item.isCompleted);

            //revert changes
            result = _controller.Patch(10, 0);
            value = Assert.IsType<OkObjectResult>(result);
            item = Assert.IsAssignableFrom<Todo>(value.Value);
            Assert.Equal(0, item.isCompleted);
        }

        [Fact()]//Patch(Id, IsCmpleted): Update todo list RecordNotFoundException test case.
        public void PatchRecordsNoFoundTest()
        {
            var ex = Assert.Throws<RecordNotFoundException<int>>(() => _controller.Patch(10000, 1));
            Assert.Equal($"No Records for passed ID={10000}", ex.Message);
        }

        [Fact()]//Delete(Id): Delete todo list success test case.(soft delete)
        public void DeleteSucsessTest()
        {
            var result = _controller.Delete(10);
            var value = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsAssignableFrom<Todo>(value.Value);
            Assert.Equal(0, item.isActive);

            //revert changes
            item.isActive = 1;
            result = _controller.Put(10, item);
            value = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(result);
            item = Assert.IsAssignableFrom<Todo>(value.Value);
            Assert.Equal(1, item.isActive);
        }

        [Fact()]//Delete(Id): Update todo list RecordNotFoundException test case.
        public void DeleteRecordsNoFoundTest()
        {
            var ex = Assert.Throws<RecordNotFoundException<int>>(() => _controller.Delete(10000));
            Assert.Equal($"No Records for passed ID={10000}", ex.Message);
        }
    }
}

