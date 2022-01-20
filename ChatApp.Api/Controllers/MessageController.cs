#nullable disable
namespace ChatApp.Api.Controllers;

using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ChatAppDbContext _context;

    public MessageController(ChatAppDbContext context)
    {
        _context = context;
    }

    // GET: api/Message
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
    {
        int userId = GetCurrentRequestUserId();

        if (userId == -1)
        {
            return Unauthorized();
        }

        return await _context.Messages.Where(message => message.PostedByUserId == userId).ToListAsync();
    }

    // PUT: api/Message/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMessage(int id, Message message)
    {
        int userId = GetCurrentRequestUserId();

        if (userId == -1)
        {
            return Unauthorized();
        }
        else if (userId != message.PostedByUserId)
        {
            return BadRequest();
        }

        if (id != message.Id)
        {
            return BadRequest();
        }

        _context.Entry(message).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MessageExists(id))
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

    // POST: api/message/private
    [HttpPost("Private")]
    public async Task<ActionResult<Message>> PostPrivateMessage([FromBody] PostMessageRequest<int> request)
    {
        var userId = GetCurrentRequestUserId();

        var SendToUser = await _context.Users.FirstOrDefaultAsync(user => user.Id == request.Receiver);
        if (SendToUser == null)
        {
            return BadRequest();
        }
        // if not friend
        if (!SendToUser.Friends.Any(friend => friend.Id == userId))
        {
            return BadRequest();
        }

        var message = new Message()
        {
            PostedByUserId = userId,
            Content = request.Content
        };
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        var privateMessage = new PrivateMessage()
        {
            SendToUserId = SendToUser.Id,
            MessageId = message.Id
        };
        await _context.PrivateMessages.AddAsync(privateMessage);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMessage", new { id = message.Id }, message);
    }

    [HttpPost("Group")]
    public async Task<ActionResult<Message>> PostGroupMessage([FromBody] PostMessageRequest<int> request)
    {
        var userId = GetCurrentRequestUserId();


        var SendToGroup = await _context.Groups.FirstOrDefaultAsync(group => group.Id == request.Receiver);
        if (SendToGroup == null)
        {
            return BadRequest();
        }
        // if not member
        if (!SendToGroup.GroupMembers.Any(user => user.UserId == userId))
        {
            return BadRequest();
        }

        var message = new Message()
        {
            PostedByUserId = userId,
            Content = request.Content
        };
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var groupMessage = new GroupMessage()
        {
            GroupId = SendToGroup.Id,
            MessageId = message.Id
        };
        _context.GroupMessages.Add(groupMessage);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMessage", new { id = message.Id }, message);
    }

    // DELETE: api/Message/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        int userId = GetCurrentRequestUserId();

        if (userId == -1)
        {
            return Unauthorized();
        }
        
        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }
        // Only author can delete message
        else if (userId != message.PostedByUserId)
        {
            return BadRequest();
        }

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MessageExists(int id)
    {
        return _context.Messages.Any(e => e.Id == id);
    }

    private int GetCurrentRequestUserId()
    {
        string rawUserId = HttpContext.User.FindFirstValue("id");
        if (!int.TryParse(rawUserId, out int userId))
        {
            return -1;
        }
        return userId;
    }
}

