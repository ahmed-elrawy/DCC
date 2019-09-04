using System;
using System.ComponentModel.DataAnnotations;

namespace DCC.API.Model
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
        public int SymptomId { get; set; }
        public string UserId { get; set; }
        public int DrugId { get; set; }
         public int BodyAreasId { get; set; }
         public DateTime TimeCreated { get; set; }
        public Drug Drug { get; set; }
        public User User { get; set; }
        public BodyAreas BodyAreas { get; set; }
        public Symptom Symptom { get; set; }

    }
}
