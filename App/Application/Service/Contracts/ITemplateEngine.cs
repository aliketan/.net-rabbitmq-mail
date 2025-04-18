namespace App.Application.Service.Contracts
{
    public interface ITemplateEngine
    {
        string Parse(string source, object model);
        Task<string> ParseAsync(string source, object model);
    }
}
