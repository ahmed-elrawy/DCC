using DCC.API.Dtos;
using DCC.API.Hubs;
using DCC.API.Repository;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DCC.API.Services
{
    public class SignalRWebSocketService : IWebSocketService
    {
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;
        private readonly IDccRepository _repo;

        public SignalRWebSocketService(IHubContext<ChatHub, IChatHub> hubContext,
         IDccRepository repo)
        {
            _hubContext = hubContext;
            _repo = repo;
        }
             public async Task SendMessage(MessageToReturnDto message)
        {
            await _hubContext.Clients.All.ReceiveMessage(message);
        }

        public async Task SendMessage(MessageToReturnDto messageToSend, string id)
        {
            await _hubContext.Clients.User(id).ReceiveMessage(messageToSend);
        }

        public async Task SendMessageAsync(MessageToReturnDto messageToSend)
        {
            await _hubContext.Clients.All.ReceiveMessage(JsonConvert.SerializeObject(messageToSend));
        }


     }
}
