using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplyRencontre.Models;
using SimplyRencontre.Models.Forum;

namespace SimplyRencontre.Controllers.API
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TopicsController : Controller
    {
        private readonly ForumContext _context;

        public TopicsController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/Topics
        [HttpGet]
        public IEnumerable<Topic> GetTopic()
        {
            return _context.Topic;
        }

        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopic([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topic = await _context.Topic.SingleOrDefaultAsync(m => m.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        // PUT: api/Topics/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopic([FromRoute] long id, [FromBody] Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topic.Id)
            {
                return BadRequest();
            }

            _context.Entry(topic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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

        // POST: api/Topics
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostTopic([FromBody] Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Topic.Add(topic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTopic", new { id = topic.Id }, topic);
        }

        // DELETE: api/Topics/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topic = await _context.Topic.SingleOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            _context.Topic.Remove(topic);
            await _context.SaveChangesAsync();

            return Ok(topic);
        }

        private bool TopicExists(long id)
        {
            return _context.Topic.Any(e => e.Id == id);
        }

        // GET: api/Topics/SearchTopic
        [HttpGet("[action]")]
        public IActionResult SearchTopic([FromBody] string title, [FromBody] Category category)
        {
            var results = _context.Topic.Where(e => e.Title.Contains(title) && e.Category == category);
            if(results.Count() == 0)
            {
                return NotFound();
            }
            return Ok(results);
        }

        // GET: api/Topics/GetCategories
        [HttpGet("[action]")]
        public IEnumerable<Category> GetCategories()
        {
            return _context.Category;
        }

        // POST: api/Topics/PostCategory
        [Authorize(Roles = "admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> PostCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategories", new { id = category.Id }, category);
        }

        // GET: api/Topics/GetCategory/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // DELETE: api/Topics/DeleteCategory/5
        [Authorize(Roles = "admin")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        // PUT: api/Topics/PutCategory/5
        [Authorize(Roles = "admin")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutCategory([FromRoute] long id, [FromBody] Category category)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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
    }
}