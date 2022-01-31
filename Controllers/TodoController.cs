using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class TodoController : ControllerBase
    {
       [HttpGet]
       [Route("todos")]
       public async Task<IActionResult> GetAsync(
           [FromServices] AppDbContext context)
       {
           var todos = await context
            .Todos
            .AsNoTracking()
            .ToListAsync();

           return Ok(todos);
       }

       [HttpGet]
       [Route("todos/{id}")]
       public async Task<IActionResult> GetByIdAsync(
           [FromServices] AppDbContext context,  
           [FromRoute] int id)
       {
           var todo = await context
            .Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

           return todo == null 
                ? NotFound() 
                : Ok(todo);
       }

       [HttpPost(template:"todos")]
       public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, 
           [FromBody] CreateTodoViewModel model)
       {
           if(!ModelState.IsValid)
               return BadRequest();

           var todo = new Todo
           {
               Date = DateTime.Now,
               Done = false,
               Title = model.Title
           };

           try
           {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();  

                return Created(uri:$"v1/todos/{todo.Id}", todo);
           }
           catch (Exception)
            {
               return BadRequest();
           }
             
       }

    }
}