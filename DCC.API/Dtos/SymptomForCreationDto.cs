using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCC.API.Dtos
{
    public class SymptomForCreationDto
    {
        [Key]
        public int SymptomId { get; set; }
        public string SymptomName { get; set; }
        public int BodyAreasId { get; set; }
     }
}