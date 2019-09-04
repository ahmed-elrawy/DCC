using System.Collections.Generic;
using System.Threading.Tasks;
using DCC.API.Dtos;
using DCC.API.Helper;
using DCC.API.Model;

namespace DCC.API.Hubs
{
      #region snippet_IChatClient
    public interface IChatHub
    {
        Task ReceiveMessage(string v);
        Task ReceiveMessage(MessageToReturnDto messageToSend);
    }
    #endregion
}
