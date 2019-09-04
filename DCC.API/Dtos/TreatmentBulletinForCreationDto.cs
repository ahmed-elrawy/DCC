namespace DCC.API.Dtos
{
    public class TreatmentBulletinForCreationDto
    {
        public int TreatmentBulletinId { get; set; }
        public int DrugId { get; set; }
        public string Composition { get; set; }
        public string Indications { get; set; }
        public string Dosing { get; set; }
        public string SideEffects { get; set; }

    }
}