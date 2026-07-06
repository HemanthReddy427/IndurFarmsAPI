namespace IndurFarmsAPI.DTO_s
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
