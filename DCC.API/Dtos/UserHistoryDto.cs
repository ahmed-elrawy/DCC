using System;
using DCC.API.Model;

namespace DCC.API.Dtos
{
    public class UserHistoryDto
    {
        public string DrugName { get; set; }
        public int RequestId { get; set; }
        public string BodyAreasName { get; set; }
        public string SymptomName { get; set; }
        public DateTime TimeCreated { get; set; }
        

    }
}