using System;

namespace DCC.API.Dtos
{
    public class RequestForCreationDto
    {
        public int RequestId { get; set; }
        public int SymptomId { get; set; }
        public string UserId { get; set; }
        public int DrugId { get; set; }
        public int BodyAreasId { get; set; }
        public DateTime TimeCreated { get; set; }
        public RequestForCreationDto()
        {
            TimeCreated = DateTime.Now;
        }

    }
}