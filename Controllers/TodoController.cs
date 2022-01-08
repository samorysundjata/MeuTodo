using System.Collections.Generic;
using System.Threading.Tasks;
using MeuTodo.Data;
using MeuTodo.Models;
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
       public async Task<IActionResult> Get(
           [FromServices] AppDbContext context)
       {
           var todos = await context
            .Todos
            .AsNoTracking()
            .ToListAsync();

           return Ok(todos);
       }
    }
}