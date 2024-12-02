namespace party_helperbe.DataAccess.Models
{
    public class Party
    {
        public int partyId { get; set; }

        public string? partyName { get; set; }

        public DateTime? partyDate { get; set; }

        public int creatorId {  get; set; }
    }
}
