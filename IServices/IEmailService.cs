namespace IndurFarmsAPI.IServices
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string name);
    }
}
