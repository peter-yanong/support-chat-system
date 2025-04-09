using Microsoft.AspNetCore.Mvc;
using MoneyBase.API.Interfaces;
using MoneyBase.Core.Models;

namespace Moneybase.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("request-support")]
        public async Task<IActionResult> RequestSupport()
        {
            var result = await _chatService.QueueSupportRequestAsync();
            return Ok(result);
        }
    }
}
