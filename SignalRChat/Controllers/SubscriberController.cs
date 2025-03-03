using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Models;
using SignalRChat.Utils;

namespace SignalRChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {

        private readonly IHubContext<SubscriberChat> _hubContext;

        public SubscriberController(IHubContext<SubscriberChat> hubContext)
        {
            _hubContext = hubContext;
        }


        [HttpPost]
        public IActionResult Post([FromBody] string message)
        {
            Console.WriteLine("Start Send Success!");

            // 通过 SignalR 发送消息

            _hubContext.Clients.All.SendAsync("ReceiveMessage", ProducerTypeEnum.Test.ToString(), message);

            Console.WriteLine("Stop Send Success!");
            return Ok();
        }
    }
}
