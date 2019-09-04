namespace DCC.API.Model
{
    public class DrugSymptom
    {
        public int DrugId { get; set; }
        public int SymptomId { get; set; }
        public Drug Drug { get; set; }
        public Symptom Symptom { get; set; }
        
    }
}