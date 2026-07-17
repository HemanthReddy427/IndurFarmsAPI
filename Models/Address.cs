namespace IndurFarmsAPI.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BasicAddress { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Users User { get; set; }
    }
}
