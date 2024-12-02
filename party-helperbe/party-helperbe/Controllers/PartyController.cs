using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using party_helperbe.Common.RequestModels;
using party_helperbe.DataAccess.Models;
using System.Net.WebSockets;

namespace party_helperbe.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        private readonly PgSQLDbContext _appData;

        public PartyController(PgSQLDbContext appData)
        {
            _appData = appData;
        }



        [HttpPost("create")]
        [Authorize(Roles ="Admin,User")]

        public async Task<IActionResult> CreateParty([FromBody]PartyRequest partyRequest)
        {

            if(string.IsNullOrEmpty(partyRequest.partyName)||partyRequest.partyDate is null)
            {
                return BadRequest(new { error = "Empty request" });
            }

            bool found=await _appData.Partys.AnyAsync(p=>p.partyDate==partyRequest.partyDate&&p.partyName==partyRequest.partyName);
            if (found)
            {
                return BadRequest(new { error = "Party already exist" });
            }

            var claimId = this.User.Claims.FirstOrDefault(x => x.Type == "memberId");

            int creatorId = int.Parse(claimId.Value);

            var party = new Party
            {
                creatorId = creatorId,
                partyDate = partyRequest.partyDate,
                partyName = partyRequest.partyName,

            };

            await _appData.Partys.AddAsync(party);
            await _appData.SaveChangesAsync();

            return Ok(party);
        }

        [HttpPost("join/{partyId}")]
        [Authorize(Roles="User")]

        public async Task<IActionResult> JoinParty(int partyId)
        {
            var claimId = this.User.Claims.FirstOrDefault(x => x.Type == "memberId");

            int memberId = int.Parse(claimId.Value);

            var memberName = await _appData.Members.Where(m => m.memberId == memberId).Select(m => m.userName).FirstOrDefaultAsync();

            var participant = new Participant
            { 
               partyId= partyId,
               memberId=memberId,
               participantName=memberName,
            };

            await _appData.Participants.AddAsync(participant);
            await _appData.SaveChangesAsync();

            return Ok(participant);

        }


    }
}
