namespace party_helperbe.DataAccess.Models
{
    public class Participant
    {
        public int participantId { get; set; } 

        public string? participantName { get; set; }

        public int? partyId {  get; set; }

        public int? memberId { get; set; }
    }
}
