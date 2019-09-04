using System.ComponentModel.DataAnnotations;

namespace DCC.API.Model
{
    public class TreatmentBulletin
    {
        [Key]
        public int TreatmentBulletinId { get; set; }
        public string Composition { get; set; }
        public string Indications { get; set; }
        public string Dosing { get; set; }
        public string SideEffects { get; set; }

        public int DrugId { get; set; }
        public Drug Drug { get; set; }


    }
}
