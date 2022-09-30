using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;
using SignalRChat.Hubs.Interface;
using SignalRChat.Models;

namespace SignalRChat.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;

        public HomeController(IHubContext<ChatHub, IChatClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("Send")]
        public IActionResult SendMessage()
        {
            _hubContext.Clients.All.ReceiveMessage(new Info() { User = "admin", Message = "You all are banned" });
            return Ok();
        }
    }
}
