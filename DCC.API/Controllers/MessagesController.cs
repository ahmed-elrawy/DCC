using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DCC.API.Dtos;
using DCC.API.Helper;
using DCC.API.Model;
using DCC.API.Repository;
using DCC.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DCC.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]

    [Route("api/users/{userId}/[controller]")]
    [ApiController]

    public class MessagesController : ControllerBase
    {
        #region privateMember
        private readonly IDccRepository _rebo;
        private readonly IMapper _mapper;
        private readonly IWebSocketService _webSocketService;
        #endregion


        #region ctor
        public MessagesController(IDccRepository rebo, IMapper mapper, IWebSocketService webSocketService)
        {
            _mapper = mapper;
            _rebo = rebo;

            _webSocketService = webSocketService;

        }
        #endregion

        #region GetMessage
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(string userId, int id)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            var messageFromRepo = await _rebo.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }
        #endregion

        #region GetMessageForUser
        [HttpGet]
        public async Task<IActionResult> GetMessageForUser(string userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            messageParams.UserId = userId;
            var messageFromRepo = await _rebo.GetMessageForUser(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);
            Response.AddPagination(messageFromRepo.CurrentPage, messageFromRepo.PageSize,
             messageFromRepo.TotalCount, messageFromRepo.TotalPages);
            return Ok(messages);

        }
        #endregion

        #region GetMessageThread
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(string userId, string recipientId)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            var messageFromRepo = await _rebo.GetMessageThread(userId, recipientId);
            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);
            return Ok(messageThread);
        }
        #endregion

        #region CreateMessage
        [HttpPost]
        public async Task<IActionResult> CreateMessage(string userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _rebo.GetUser(userId);
            if (sender.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            messageForCreationDto.SenderId = userId;
            var recipient = await _rebo.GetUser(messageForCreationDto.RecipientId);
            if (recipient == null)
                return BadRequest("Could Not Find User");
            var message = _mapper.Map<Message>(messageForCreationDto);



            _rebo.Add(message);
            if (await _rebo.SaveAll())
            {
                var accesToken = Request.Headers["Authorization"];

                var messageToSend = _mapper.Map<MessageToReturnDto>(message);
                // var theMessage = JsonConvert.SerializeObject(messageToSend);
                await _webSocketService.SendMessage(messageToSend, recipient.Id);
                var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

                request.KeepAlive = true;
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";

                request.Headers.Add("authorization", "Basic OGJjNGEzNGQtODMyZS00MmFkLThkMmUtY2I3ODllMzE1Zjdi");

                byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                        + "\"app_id\": \"1247426f-2d83-4c66-802e-8f8e2440b95f\","
                                                        + "\"contents\": {\"en\": messageToSend.Content},"
                                                        + "\"included_segments\": [\"Active Users\"]}");

                string responseContent = null;

                try
                {
                    using (var writer = request.GetRequestStream())
                    {
                        writer.Write(byteArray, 0, byteArray.Length);
                    }

                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseContent = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                }

                System.Diagnostics.Debug.WriteLine(responseContent);

                return CreatedAtRoute("GetMessage", new { id = message.Id }, messageToSend);

            }


            throw new Exception("Creating Message Failed on sent ");
        }
        #endregion

        #region DeleteMessage 
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, string userId)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            var messageFromRepo = await _rebo.GetMessage(id);
            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _rebo.Delete(messageFromRepo);

            if (await _rebo.SaveAll())
                return NoContent();

            throw new Exception("Error Deleting the Message");
        }
        #endregion

        #region ReadMessage
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(string userId, int id)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            var message = await _rebo.GetMessage(id);
            if (message.RecipientId != userId)
                return Unauthorized();
            message.isRead = true;
            message.DateRead = DateTime.Now;
            await _rebo.SaveAll();
            return NoContent();

        }
        #endregion
    }
}