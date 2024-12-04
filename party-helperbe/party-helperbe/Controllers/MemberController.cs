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


    }
}
