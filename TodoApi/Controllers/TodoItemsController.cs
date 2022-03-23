using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // 情報を全部取得する
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        //{
        //    var todoItem = await _context.TodoItems.FindAsync(id);

        //    if (todoItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return todoItem;
        //}

        // GET: api/TodoItems/Takuya
        [HttpGet("{name}")]
        public async Task<ActionResult<TodoItem>> GetUser(string name)
        {
            //一応解決 FindAsyncが主キー(id)しか拾えない？？からかエラーになってしまう
            // → 一度リストで拾ってしまって、最初に名前が一致するEntityを拾い上げるような仕様にした(応急処置)
            //var todoItem = await _context.TodoItems.FindAsync(name);
            var list = await _context.TodoItems.ToListAsync();
            var todoItem = list.Where(e => e.Name.Contains(name)).FirstOrDefault();

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        public async Task<IActionResult> PutTodoItem(string name, TodoItem todoItem)
        {
            //if (id != todoItem.Id)
            if (name != todoItem.Name)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // ユーザー情報の登録
        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { name = todoItem.Name }, todoItem);
            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            //return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTodoItem(long id)
        public async Task<IActionResult> DeleteTodoItem(string name)
        {
            var todoItem = await _context.TodoItems.FindAsync(name);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool TodoItemExists(long id)
        private bool TodoItemExists(string name)
        {
            return _context.TodoItems.Any(e => e.Name == name);
        }
    }
}
