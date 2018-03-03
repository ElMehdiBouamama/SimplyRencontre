using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplyRencontre.Models;
using SimplyRencontre.Models.Forum;

namespace SimplyRencontre.Controllers.API
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TopicMessagesController : Controller
    {
        private readonly ForumContext _context;

        public TopicMessagesController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/TopicMessages
        [HttpGet]
        public IEnumerable<TopicMessages> GetTopicMessages()
        {
            return _context.TopicMessages;
        }

        // GET: api/TopicMessages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicMessages([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topicMessages = await _context.TopicMessages.SingleOrDefaultAsync(m => m.Id == id);

            if (topicMessages == null)
            {
                return NotFound();
            }

            return Ok(topicMessages);
        }

        // PUT: api/TopicMessages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopicMessages([FromRoute] long id, [FromBody] TopicMessages topicMessages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topicMessages.Id)
            {
                return BadRequest();
            }

            _context.Entry(topicMessages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicMessagesExists(id))
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

        // POST: api/TopicMessages
        [HttpPost]
        public async Task<IActionResult> PostTopicMessages([FromBody] TopicMessages topicMessages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TopicMessages.Add(topicMessages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTopicMessages", new { id = topicMessages.Id }, topicMessages);
        }

        // DELETE: api/TopicMessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopicMessages([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topicMessages = await _context.TopicMessages.SingleOrDefaultAsync(m => m.Id == id);
            if (topicMessages == null)
            {
                return NotFound();
            }

            _context.TopicMessages.Remove(topicMessages);
            await _context.SaveChangesAsync();

            return Ok(topicMessages);
        }

        private bool TopicMessagesExists(long id)
        {
            return _context.TopicMessages.Any(e => e.Id == id);
        }
    }
}