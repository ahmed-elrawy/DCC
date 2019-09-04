using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCC.API.Model;

namespace DCC.API.Repository
{
    public interface IDiagnosisRepository
    {
         Task<Drug> GetDrug(int id );
        Task<IEnumerable<Drug>> GetAllDrugs();
 
        Task<IEnumerable<BodyAreas>> GetBodyAreas();
        Task<IEnumerable<Symptom>> GetSympotByBodyAreasId(int id);
        Task<IEnumerable<DrugSymptom>> GetDrugBySyptomId(int syptomId);
        Task<IEnumerable<Symptom>> GetSyptomByDrugId(int drugId);
        Task<IEnumerable<TreatmentBulletin>> GetAllBulletin();

        Task<Drug> GetDrugById(int drugId);

        Task<IEnumerable<Request>> GetUserHistoryByUserId(string userid); 
        



    }
}