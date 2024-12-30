using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using ToDoList.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Db.Data;
using ToDoList.Models.Request;
using ToDoList.Models.Response;


namespace ToDoList.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly Database _database;
        public TodoItemsController(Database database)
        {
            _database = database;
        }
        private static List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Title = "Learn ASP.NET Core",
                    Description = "Learn how to build web apps with ASP.NET Core",
                    IsCompleted = false,
                    IsDelete = false },
                new TodoItem { Title = "Build ToDoList",
                    Description = "Build a todo list app with ASP.NET Core",
                    IsCompleted = false,
                    IsDelete = false } };


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetTodoItemsByUserId(int userId)
        {
            var todoItems = await _database.ToDoLists
                .Where(t => t.UserId == userId)
                .Select(t => new Request
                {
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    IsDeleted = t.IsDelete
                })
                .ToListAsync();

            if (todoItems == null || !todoItems.Any())
            {
                return NotFound(new { Message = "No tasks found for the specified user." });
            }

            return Ok(todoItems);
        }






        [HttpPost]
        public async Task<ActionResult<Response>> PostTodoItems([FromBody] List<TodoItem> todoItems)
        {
            foreach (var item in todoItems)
            {
                var toDoItem = new TodoItem
                {
                    Title = item.Title,
                    Description = item.Description,
                    IsCompleted = item.IsCompleted,
                    IsDelete = item.IsDelete
                };
                _database.Set<TodoItem>().Add(toDoItem);
            }
            await _database.SaveChangesAsync();
            var response = new Response
            {
                Success = true,
                Message = "All static ToDoItems added successfully"
            };
            return CreatedAtAction(nameof(GetTodoItemsByUserId), response);
        }

        
    }
}
