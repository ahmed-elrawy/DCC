using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCC.API.Helper;
using DCC.API.Model;
using DCC.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace DCC.API.Data
{
    public class DiagnosisRepository : IDiagnosisRepository
    {
        #region  private Members
        public DataContext _context;
        private readonly IDccRepository _repo;
        #endregion

        #region  ctor
        public DiagnosisRepository(DataContext context, IDccRepository repo)
        {
            _context = context;
            _repo = repo;


        }
        #endregion

        #region  MethodImplemntions
        public async Task<IEnumerable<BodyAreas>> GetBodyAreas()
        {
            var bodyArea = await _context.BodyAreas.ToListAsync();
            return bodyArea;
        }

        public async Task<Drug> GetDrug(int id)
        {
            var drug = await _context.Drugs.Include(s => s.TreatmentBulletin)
            .Include(s => s.DrugSymptom.Where(bs => bs.DrugId == id).ToArray())
            .FirstOrDefaultAsync();

            return drug;
        }

        public async Task<IEnumerable<DrugSymptom>> GetDrugBySyptomId(int syptomId)
        {
            var drugSyptom = await _context.DrugSymptom.Where(d => d.SymptomId == syptomId)
            .Include(s => s.Drug).ThenInclude(t => t.TreatmentBulletin)
            .ToListAsync();
            return drugSyptom;
        }




        public async Task<IEnumerable<Drug>> GetAllDrugs()
        {
            var drugs = await _context.Drugs.Include(d => d.TreatmentBulletin).ToListAsync();
            return drugs;
        }

        public async Task<IEnumerable<Symptom>> GetSympotByBodyAreasId(int BodyId)
        {
            var sympot = await _context.Symptoms.Where(b => b.BodyAreasId == BodyId).ToListAsync();
            return sympot;
        }



        public Task<IEnumerable<Symptom>> GetSyptomByDrugId(int drugId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<TreatmentBulletin>> GetAllBulletin()
        {
            var AllBulletin = await _context.TreatmentBulletins.ToListAsync();
            return AllBulletin;
        }

        public async Task<Drug> GetDrugById(int drugId)
        {
            var drug = await _context.Drugs.Where(d => d.DrugId == drugId).Include(t => t.TreatmentBulletin).FirstOrDefaultAsync();
            return drug;
        }

        public async Task<IEnumerable<Request>> GetUserHistoryByUserId(string userid)
        {
            var historey = await _context.Requests.Where(u => u.UserId == userid)
            .Include(c => c.Drug)
            .ThenInclude(t => t.TreatmentBulletin).Include(r => r.Symptom).Include(b => b.BodyAreas).ToListAsync();
            return historey;
        }
        #endregion
    }
}