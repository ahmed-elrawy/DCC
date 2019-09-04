using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCC.API.Model
{
    public class Symptom
  {
        [Key]
        public int SymptomId { get; set; }
        public string SymptomName { get; set; }
        public int BodyAreasId { get; set; }
        public ICollection<DrugSymptom> DrugSymptom { get; set; }

        public BodyAreas BodyAreas { get; set; }
    }
}
