using DCC.API.Dtos;
using DCC.API.Model;
using DCC.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
namespace DCC.API.Hubs
{
   
    [Authorize]

    public class ChatHub : Hub<IChatHub>
    {
        #region HubMethods
        // public Task SendMessage(string user, string message)
        // {
        //     return Clients.All.SendAsync("ReceiveMessage", user, message);
        // }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.ReceiveMessage("ReceiveMessage");
        }

        // public Task SendMessageToGroup(string message)
        // {
        //     return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", message);
        // }
        #endregion

        #region HubMethodName
        // [HubMethodName("SendMessageToUser")]
        // public Task DirectMessage(string user, string message)
        // {
        //     return Clients.User(user).SendAsync("ReceiveMessage", message);
        // }
        #endregion

        #region ThrowHubException
        public Task ThrowException()
        {
            throw new HubException("This error will be sent to the client!");
        }
        #endregion

        #region OnConnectedAsync
        public override async Task OnConnectedAsync()
        {
            await Clients.All.ReceiveMessage("Welcome To Out App" + Context.ConnectionId + " id "
            + Context.UserIdentifier);

            //  await Clients.All.ReceiveMessage($"User has connected, server-side message. & connectionId : {Context.ConnectionId}");
        }
        #endregion

        #region OnDisconnectedAsync
        // public override async Task OnDisconnectedAsync(Exception exception)
        // {
        //     await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
        //     await base.OnDisconnectedAsync(exception);
        // }
        #endregion



    }
}