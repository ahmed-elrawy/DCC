using System;
using DCC.API.Model;

namespace DCC.API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderKnownAs { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string RecipientId { get; set; }
        public string RecipientKnownAs { get; set; }
        public string Content { get; set; }
        public bool isRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}