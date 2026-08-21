using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;

[ApiController]
[Route("api/messages")]
public class MessageController : ControllerBase
{
    [HttpGet("conversations")]
    public IActionResult GetConversations() => StatusCode(501, "Not Implemented");

    [HttpGet("{conversationWith}")]
    public IActionResult GetMessageHistory(int conversationWith) => StatusCode(501, "Not Implemented");

    [HttpPost]
    public IActionResult SendMessage() => StatusCode(501, "Not Implemented");

    [HttpPut("{id}/read")]
    public IActionResult MarkAsRead(int id) => StatusCode(501, "Not Implemented");
}
