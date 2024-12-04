using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using party_helperbe.Common.Models;
using party_helperbe.DataAccess.Models;

namespace party_helperbe.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly PgSQLDbContext _appData;

        public MemberController(PgSQLDbContext appData)
        {
            _appData = appData;
        }

        [HttpGet("member/show/{page}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetMemebers(int page = 1)
        {

            List<Member> memebers= await _appData.Members.ToListAsync();

            var totalMembers = memebers.Count();

            var totalPages = (int)Math.Ceiling((decimal)totalMembers / 5);

            var membersOnPage = memebers.Skip((page - 1) * 5).Take(5).Select(m => new MemberInfo
            {
                memberId = m.memberId,
                emailAddress = m.emailAddress,
                memberName = m.userName
            }).ToList();

            return Ok(new {Page  = page, TotalPages=totalPages, Members =  membersOnPage});
        }

        [HttpDelete("member/delete/{memberId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteMember(int memberId)
        {
            var member = await _appData.Members.FirstOrDefaultAsync(m=> m.memberId == memberId);
            var partys = await _appData.Partys.Where(p => p.creatorId == memberId).ToListAsync();
            var partyIds = partys.Select(p => p.partyId).ToList();

            var participants = await _appData.Participants
                .Where(p => partyIds.Contains(p.partyId.GetValueOrDefault()))
                .ToListAsync();

            _appData.Partys.RemoveRange(partys);
            _appData.Participants.RemoveRange(participants);
            _appData.Members.Remove(member);
            _appData.SaveChangesAsync();

            return Ok("Member was deleted");
        }


    }
}
