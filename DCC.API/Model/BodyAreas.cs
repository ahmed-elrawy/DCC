using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCC.API.Model
{
    public class BodyAreas
    {
        [Key]
        public int BodyAreasId { get; set; }
        public string NameArea { get; set; }
        public ICollection<Symptom> Symptoms { get; set; }


    }
}
