using System.ComponentModel.DataAnnotations;

namespace party_helperbe.DataAccess.Models
{
    public class Member
    {
        public int memberId {  get; set; }

        [EmailAddress]
        public string? emailAddress { get; set;}

        public string? userName { get; set; }    

        public string? Password { get; set; }

    }
}
