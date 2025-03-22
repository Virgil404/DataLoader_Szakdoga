namespace DataloaderApi.Extension.Services.Interface
{
    public interface IEmaliSenderService
    {
        public Task SendEmail(List<string> email, string subject, string message);
    }
}
