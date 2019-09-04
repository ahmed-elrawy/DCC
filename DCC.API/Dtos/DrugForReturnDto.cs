namespace DCC.API.Dtos
{
    public class DrugForReturnDto
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public TreatmentBulletinForCreationDto TreatmentBulletin { get; set; }

    }
}