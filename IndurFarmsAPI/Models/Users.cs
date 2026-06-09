namespace IndurFarmsAPI.Models
{
    public class Users
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int Role {  get; set; }
        public required string Phone {  get; set; }
        public required string PasswordHash { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly UpdatedDate { get; set; }
        public Cart Cart { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Orders> Orders { get; set; } = new List<Orders>(); 
    }
}
