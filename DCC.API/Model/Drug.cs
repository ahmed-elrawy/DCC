using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCC.API.Model
{
    public class Drug
    {

        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int TreatmentBulletinId { get; set; }
        public TreatmentBulletin TreatmentBulletin { get; set; }
        public int DrugTypeId { get; set; }
        public DrugType DrugType { get; set; }
        public ICollection<DrugSymptom> DrugSymptom { get; set; }
    }
}
