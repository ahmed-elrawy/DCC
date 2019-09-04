namespace DCC.API.Dtos
{
    public class TreatmentBulletinForReturnDto
    {


       public int TreatmentBulletinId { get; set; }
        public string DrugName { get; set; }
        public string Composition { get; set; }
        public string Indications { get; set; }
        public string Dosing { get; set; }
        public string SideEffects { get; set; }

    }
}