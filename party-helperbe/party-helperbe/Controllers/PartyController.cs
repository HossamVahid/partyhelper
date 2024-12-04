using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using party_helperbe.Common.Models;
using party_helperbe.Common.RequestModels;
using party_helperbe.DataAccess.Models;
using System.Net.WebSockets;
using System.Security.Claims;

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



        [HttpPost("party/create")]
        [Authorize(Roles ="User")]

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

        [HttpPost("party/join/{partyId}")]
        [Authorize(Roles="User")]

        public async Task<IActionResult> JoinParty(int partyId)
        {
            var claimId = this.User.Claims.FirstOrDefault(x => x.Type == "memberId");

            int memberId = int.Parse(claimId.Value);

            var memberName = await _appData.Members.Where(m => m.memberId == memberId).Select(m => m.userName).FirstOrDefaultAsync();

            bool found = await _appData.Participants.AnyAsync(m=> m.memberId == memberId && m.partyId == partyId);
            if(found)
            {
                return BadRequest(new { error = "Member is in this party" });
            }

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

        [HttpGet("party/show/{page}")]


        public async Task<IActionResult> ShowPartys(int page = 1)
        {
            List<Party> partys = await _appData.Partys.ToListAsync();

            var totalPartys = partys.Count();

            var totalPages = (int)Math.Ceiling((decimal)totalPartys / 5);

            var partysOnPage = partys.Skip((page - 1) * 5).Take(5).Select(p => new PartyInfo
            {
                partyId = p.partyId,
                partyName = p.partyName
            }).ToList();

            return Ok(new {Page=page, TotalPages=totalPages,Partys=partysOnPage});
        }

        [HttpDelete("party/delete/{partyId}")]
        [Authorize(Roles = "Admin,User")]

        public async Task<IActionResult> DeleteParty(int partyId)
        {
            var party= await _appData.Partys.FirstOrDefaultAsync(p=>p.partyId==partyId);
            if (party==null)
            {
                return BadRequest(new { error = "Party does not exist" });
            }

            var claimId = this.User.Claims.FirstOrDefault(x => x.Type == "memberId");
            int memberId = int.Parse(claimId.Value);

            if(memberId != party.creatorId) 
            {
                return Unauthorized(new { error = "You dont have acces to delete this party" });
            }

            var participants= await _appData.Participants.Where(p=>p.partyId == partyId).ToListAsync();

            _appData.Participants.RemoveRange(participants);
             _appData.Partys.Remove(party);

            await _appData.SaveChangesAsync();

            return Ok("Party was succesfuly deleted");

        }


    }
}
