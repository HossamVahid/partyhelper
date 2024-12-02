using System.ComponentModel.DataAnnotations;

namespace party_helperbe.Common.RequestModels
{
    public class PartyRequest
    {
        [Required]
        public string? partyName { get; set; }

        [Required]
        public DateTime? partyDate { get; set; }

    }
}
