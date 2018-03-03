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
    public class TopicMessageController : Controller
    {
        private readonly ForumContext _context;

        public TopicMessageController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/TopicMessage
        [HttpGet]
        public IEnumerable<TopicMessage> GetTopicMessage()
        {
            return _context.TopicMessage;
        }

        // GET: api/TopicMessage/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicMessage([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topicMessage = await _context.TopicMessage.SingleOrDefaultAsync(m => m.Id == id);

            if (topicMessage == null)
            {
                return NotFound();
            }

            return Ok(topicMessage);
        }

        // PUT: api/TopicMessage/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopicMessage([FromRoute] long id, [FromBody] TopicMessage topicMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topicMessage.Id)
            {
                return BadRequest();
            }

            _context.Entry(topicMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicMessageExists(id))
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

        // POST: api/TopicMessage
        [HttpPost("{id}")]
        public async Task<IActionResult> PostTopicMessage([FromRoute] long id, [FromBody] TopicMessage topicMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topic = await _context.Topic.FirstAsync(e => e.Id == id);

            if(topic.Id != id)
            {
                return NotFound();
            }

            topic.Messages.Add(topicMessage);
            _context.Entry(topic).State = EntityState.Modified;
            _context.TopicMessage.Add(topicMessage);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTopicMessage", new { id = topicMessage.Id }, topicMessage);
        }

        // DELETE: api/TopicMessage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopicMessage([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topicMessage = await _context.TopicMessage.SingleOrDefaultAsync(m => m.Id == id);
            if (topicMessage == null)
            {
                return NotFound();
            }

            _context.TopicMessage.Remove(topicMessage);
            await _context.SaveChangesAsync();

            return Ok(topicMessage);
        }

        private bool TopicMessageExists(long id)
        {
            return _context.TopicMessage.Any(e => e.Id == id);
        }
    }
}