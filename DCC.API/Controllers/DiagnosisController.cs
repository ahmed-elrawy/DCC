using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DCC.API.Data;
using DCC.API.Dtos;
using DCC.API.Model;
using DCC.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DCC.API.Controllers
{
    // 
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        #region privateMembers
        public DataContext _context;

        private readonly IDccRepository _repo;

        private readonly IMapper _mapper;

        public IDiagnosisRepository _diag;
        #endregion

        #region ctor
        public DiagnosisController(IDccRepository repo, DataContext context, IMapper mapper, IDiagnosisRepository diag)
        {
            _diag = diag;
            _mapper = mapper;
            _repo = repo;

        }
        #endregion

        #region getBodyAreas
         
        [HttpGet("getBodyAreas", Name = "GetAllBodyAreas")]
        public async Task<IActionResult> GetAllBodyAreas()
        {
            var allBody = await _diag.GetBodyAreas();
            return Ok(allBody);
        }
        #endregion

        #region  creatBodyArea
         
        [HttpPost("creatBodyArea", Name = "CreateBodyArea")]
        public async Task<IActionResult> CreateBodyArea(BodyAreaForCreationDto BodyArea)
        {

            if (BodyArea != null)
            {
                var BodyMapped = _mapper.Map<BodyAreas>(BodyArea);
                _repo.Add(BodyMapped);
                await _repo.SaveAll();
                return Ok();
            }
            return BadRequest("Cheak Your Object");
        }
        #endregion

        #region creatDrug
         
        [HttpPost("CreateDrug", Name = "CreateDrug")]
        public async Task<IActionResult> CreateDrug(DrugForCreationDto drug)
        {
            if (drug != null)
            {
                var drugMapped = _mapper.Map<Drug>(drug);
                _repo.Add(drugMapped);
                await _repo.SaveAll();
                return Ok();
            }
            return BadRequest("Cheak Your Object");

        }
        #endregion

        #region creatBulletin
         
        [HttpPost("creatBulletin", Name = "CreatBulletin")]
        public async Task<IActionResult> CreatBulletin(TreatmentBulletinForCreationDto Bulletin)
        {
            if (Bulletin != null)
            {
                try
                {
                    var BulletinMapped = _mapper.Map<TreatmentBulletin>(Bulletin);
                    _repo.Add(BulletinMapped);
                    await _repo.SaveAll();
                }
                catch (System.Exception ex)
                {

                    throw;
                }
                return Ok();
            }
            return BadRequest("Cheak Your Object");

        }
        #endregion

        #region GetAllDrugs
         
        [HttpGet("GetAllDrugs", Name = "GetAllDrugs")]
        public async Task<IActionResult> GetAllDrugs()
        {
            var AllDrugs = await _diag.GetAllDrugs();

            //  var AllDrugsMapped = _mapper.Map<DrugForReturnDto>(AllDrugs);
            List<DrugForReturnDto> AllDrugsMapped = new List<DrugForReturnDto>();
            foreach (var item in AllDrugs)
            {
                var allMapped = new DrugForReturnDto();
                allMapped.DrugName = item.DrugName;
                allMapped.DrugId = item.DrugId;
                allMapped.TreatmentBulletin = _mapper.Map<TreatmentBulletinForCreationDto>(item.TreatmentBulletin);
                // allMapped.TreatmentBulletin.Composition = item.TreatmentBulletin.Composition;
                // allMapped.TreatmentBulletin.Dosing = item.TreatmentBulletin.Dosing;
                // allMapped.TreatmentBulletin.Indications = item.TreatmentBulletin.Indications;
                // allMapped.TreatmentBulletin.SideEffects = item.TreatmentBulletin.SideEffects;
                AllDrugsMapped.Add(allMapped);
            }
            return Ok(AllDrugsMapped);
            //  return Ok(AllDrugsMapped);
        }
        #endregion

        #region createSypmtom
         
        [HttpPost("CreateSypmtom", Name = "CreateSypmtom")]
        public async Task<IActionResult> CreateSypmtom(SymptomForCreationDto drug)
        {
            if (drug != null)
            {
                var symptomMapped = _mapper.Map<Symptom>(drug);
                _repo.Add(symptomMapped);
                await _repo.SaveAll();
                return Ok();
            }
            return BadRequest("Cheak Your Object");

        }

        #endregion

        #region getAllBulletin
         
        [HttpGet("getAllBulletin", Name = "GetAllBulletin")]
        public async Task<IActionResult> GetAllBulletin()
        {
            var allBulletin = await _diag.GetAllBulletin();
            //  var allBulletinMapped = _mapper.Map<TreatmentBulletinForReturnDto>(allBulletin);

            List<TreatmentBulletinForReturnDto> allBulletinMapped = new List<TreatmentBulletinForReturnDto>();
            foreach (var item in allBulletin)
            {
                var allMapped = new TreatmentBulletinForReturnDto();
                var drugname = await _diag.GetDrugById(item.DrugId);
                allMapped.TreatmentBulletinId = item.TreatmentBulletinId;
                allMapped.Composition = item.Composition;
                allMapped.Indications = item.Indications;
                allMapped.SideEffects = item.SideEffects;
                allMapped.Dosing = item.Dosing;

                allMapped.DrugName = drugname.DrugName;
                allBulletinMapped.Add(allMapped);
            }
            return Ok(allBulletinMapped);
        }
        #endregion

        #region  CreateRequest 
         
        [HttpPost("CreateRequest", Name = "CreateRequest")]
        public async Task<IActionResult> CreateRequest(RequestForCreationDto request)
        {
            if (request != null)
            {
                Request reuest = new Request();

                reuest.DrugId = request.DrugId;
                reuest.RequestId = request.RequestId;
                reuest.BodyAreasId = request.BodyAreasId;
                reuest.SymptomId = request.SymptomId;
                reuest.UserId = request.UserId;
                reuest.TimeCreated = DateTime.Now;
                // var History = new History
                // {
                //     RequestId = request.RequestId,
                //     UserId = request.UserId,


                // };
                // _repo.Add(History);
                // await _repo.SaveAll();

                // int lastProductId = History.HistoryId;
                // reuest.HistoryId = lastProductId;

                // var requestMapped = _mapper.Map<Request>(request);
                _repo.Add(reuest);
                await _repo.SaveAll();
                return Ok();
            }

            return BadRequest("Cheak Your Object");

        }


        #endregion

        #region  GetUserHistory 
         
        [HttpGet("GetUserHistory", Name = "GetUserHistory")]
        public async Task<IActionResult> GetUserHistory(string userId)
        {
            if (userId != null)
            {
                var history = await _diag.GetUserHistoryByUserId(userId);
                List<UserHistoryDto> AllHistoryMapped = new List<UserHistoryDto>();

                foreach (var item in history)
                {
                    var historyMapped = new UserHistoryDto();
                    historyMapped.DrugName = item.Drug.DrugName;
                    historyMapped.SymptomName = item.Symptom.SymptomName;
                    historyMapped.TimeCreated = item.TimeCreated;
                    historyMapped.BodyAreasName = item.BodyAreas.NameArea;
                    historyMapped.RequestId = item.RequestId;

                    AllHistoryMapped.Add(historyMapped);
                }
                return Ok(AllHistoryMapped);
            }

            return BadRequest("Cheak Your Object");

        }


        #endregion

        #region GetSympotByBodyAreasId
         
        [HttpGet("GetSympotByBodyAreasId", Name = "GetSympotByBodyAreasId")]
        public async Task<IActionResult> GetSympotByBodyAreasId(int BodyId)
        {
            if (BodyId > 0)
            {
                var allSympot = await _diag.GetSympotByBodyAreasId(BodyId);
                //  var allBulletinMapped = _mapper.Map<TreatmentBulletinForReturnDto>(allBulletin);

                List<SymptomForReturnDto> allSymptoms = new List<SymptomForReturnDto>();
                foreach (var item in allSympot)
                {
                    var allMapped = new SymptomForReturnDto();
                    allMapped.SymptomName = item.SymptomName;
                    allMapped.SymptomId = item.SymptomId;
                    allSymptoms.Add(allMapped);
                }
                return Ok(allSymptoms);
            }
            return BadRequest("Pleas Enter Valid Id");

        }
        #endregion

        #region GetDrugBySyptomId
         
        [HttpGet("GetDrugBySyptomId", Name = "GetDrugBySyptomId")]

        public async Task<IActionResult> GetDrugBySyptomId(int symptomId)
        {
            if (symptomId > 0)
            {

                var AllDrug = await _diag.GetDrugBySyptomId(symptomId);
                //  var allBulletinMapped = _mapper.Map<TreatmentBulletinForReturnDto>(allBulletin);

                List<DrugForReturnDto> AllDrugs = new List<DrugForReturnDto>();
                foreach (var item in AllDrug)
                {
                    var allMapped = new DrugForReturnDto();
                    allMapped.DrugName = item.Drug.DrugName;
                    allMapped.DrugId = item.DrugId;
                    allMapped.TreatmentBulletin = _mapper.Map<TreatmentBulletinForCreationDto>(item.Drug.TreatmentBulletin);
                    AllDrugs.Add(allMapped);
                }
                return Ok(AllDrugs);
            }
            return BadRequest("Pleas Enter Valid Id");

        }
        #endregion

        #region GetDrugById
         
        [HttpGet("GetDrugById", Name = "GetDrugById")]
        public async Task<IActionResult> GetDrugById(int drugId)
        {
            if (drugId > 0)
            {
                var allDrugs = await _diag.GetDrugById(drugId);

                var allMapped = new DrugForReturnDto();
                allMapped.DrugName = allDrugs.DrugName;
                allMapped.DrugId = allDrugs.DrugId;
                allMapped.TreatmentBulletin = _mapper.Map<TreatmentBulletinForCreationDto>(allDrugs.TreatmentBulletin);


                return Ok(allMapped);
            }
            return BadRequest("Pleas Enter Valid Id");

        }
        #endregion

        #region GetSpec
        [AllowAnonymous]
        [HttpGet("getspecil")]
        public async Task<IEnumerable<string>> GetSpec()
        {
            return await _repo.GetSpec();
        }
        #endregion
    }

}
