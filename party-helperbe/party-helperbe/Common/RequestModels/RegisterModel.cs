using System.ComponentModel.DataAnnotations;

namespace party_helperbe.Common.RequestModels
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string? emailAddress { get; set; }

        [Required]
        public string? userName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
