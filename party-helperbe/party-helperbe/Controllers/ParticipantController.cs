using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using party_helperbe.Common.Models;
using party_helperbe.DataAccess.Models;
using System.IO;

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

        public async Task<IActionResult> GetParticipants(int partyId, int page = 1)
        {

            List<Participant> participants = await _appData.Participants.Where(p => p.partyId == partyId).ToListAsync();

            var totalParticipants = participants.Count();

            var totalPages = (int)Math.Ceiling((decimal)totalParticipants / 5);

            var particiapntsOnPage = participants.Skip((page - 1) * 5).Take(5).Select(p => new ParticiapntInfo
            {
                participantId = p.participantId,
                participantName = p.participantName
            }).ToList();

            return Ok(new { Page = page, TotalPages = totalPages, Particiapnts = particiapntsOnPage });

        }

        [HttpDelete("participant/remove/{participantId}")]
        [Authorize(Roles = "Admin,User")]

        public async Task<IActionResult> Remove(int participantId)
        {

            var particiapnt= await _appData.Participants.FirstOrDefaultAsync(p=> p.participantId == participantId);
           

            if (particiapnt == null)
            {
                return BadRequest(new { error = "Participant does not exist" });
            }

            var party = await _appData.Partys.FirstOrDefaultAsync(p => p.partyId == particiapnt.partyId);
            var claimId = this.User.Claims.FirstOrDefault(x => x.Type == "memberId");
            int memberId = int.Parse(claimId.Value);

            if (memberId != party.creatorId && memberId != particiapnt.memberId)
            {
                return Unauthorized(new { error = "You do not have permision to remove" });
            }

            _appData.Participants.Remove(particiapnt);
             await _appData.SaveChangesAsync();

            return Ok("The member was removed");

           
        }

        
    }
}
