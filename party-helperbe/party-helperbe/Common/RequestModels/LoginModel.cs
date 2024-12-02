using System.ComponentModel.DataAnnotations;

namespace party_helperbe.Common.RequestModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress] 
        public string? emailAdress { get; set; }

        [Required]
        public string Password {  get; set; }
    }
}
