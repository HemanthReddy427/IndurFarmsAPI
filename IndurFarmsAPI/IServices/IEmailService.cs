namespace IndurFarmsAPI.IServices
{
    public interface IEmailService
    {
        Task SendRegistrationEmail(string email);
    }
}
