using System.Threading.Tasks;
using DCC.API.Dtos;

namespace DCC.API.Services
{
    public interface IWebSocketService
    {
        Task SendMessage(MessageToReturnDto  message);
         Task SendMessageAsync(MessageToReturnDto messageToSend);
       Task SendMessage(MessageToReturnDto messageToSend, string id);
    }
}
