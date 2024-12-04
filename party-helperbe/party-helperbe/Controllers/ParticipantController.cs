using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using party_helperbe.Common.Models;
using party_helperbe.DataAccess.Models;

namespace party_helperbe.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {

        private readonly PgSQLDbContext _appData;

        public ParticipantController(PgSQLDbContext appData)
        {
            _appData = appData;
        }

        
        
        [HttpGet("participant/show/{partyId}/{page}")]
        
        public async Task<IActionResult> GetParticipants(int partyId,int page = 1)
        {
           
            List<Participant> participants = await _appData.Participants.Where(p=>p.partyId == partyId).ToListAsync();

            var totalParticipants= participants.Count();

            var totalPages = (int)Math.Ceiling((decimal)totalParticipants / 5);

            var particiapntsOnPage=participants.Skip((page-1)*5).Take(5).Select(p=> new ParticiapntInfo
            {
                participantId = p.participantId,
                participantName = p.participantName
            }).ToList();

            return Ok(new { Page = page, TotalPages = totalPages, Particiapnts = particiapntsOnPage });

        }


    }
}
