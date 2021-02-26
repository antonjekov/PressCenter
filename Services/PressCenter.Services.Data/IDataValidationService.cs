using PressCenter.Data.Models;

namespace PressCenter.Services.Data
{
    public interface IDataValidationService
    {
        bool ValidateNews(INews news);
        bool ValidateTopNews(ITopNews news);
    }
}