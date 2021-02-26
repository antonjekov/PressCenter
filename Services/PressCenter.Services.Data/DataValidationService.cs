namespace PressCenter.Services.Data
{
    using System;

    using PressCenter.Data.Models;

    public class DataValidationService : IDataValidationService
    {
        public bool ValidateNews(INews news)
        {
            if (string.IsNullOrWhiteSpace(news.Title) ||
                 string.IsNullOrWhiteSpace(news.Content) ||
                 string.IsNullOrWhiteSpace(news.ImageUrl) ||
                 string.IsNullOrWhiteSpace(news.RemoteId) ||
                 string.IsNullOrWhiteSpace(news.OriginalUrl) ||
                 news.OriginalUrl == null)
            {
                return false;
            }

            if (!Uri.TryCreate(news.OriginalUrl, UriKind.Absolute, out Uri uriResult) &&
                  uriResult.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            if (!Uri.TryCreate(news.ImageUrl, UriKind.RelativeOrAbsolute, out _))
            {
                return false;
            }

            return true;
        }

        public bool ValidateTopNews(ITopNews news)
        {
            if (string.IsNullOrWhiteSpace(news.Title) ||
                 string.IsNullOrWhiteSpace(news.ImageUrl) ||
                 string.IsNullOrWhiteSpace(news.RemoteId) ||
                 string.IsNullOrWhiteSpace(news.OriginalUrl) ||
                 news.ImageUrl == null ||
                 news.OriginalUrl == null)
            {
                return false;
            }

            if (!Uri.TryCreate(news.OriginalUrl, UriKind.Absolute, out Uri uriResult) &&
                  uriResult.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            if (news.ImageUrl != string.Empty &&
                !Uri.TryCreate(news.ImageUrl, UriKind.Absolute, out Uri uriImageResult) &&
                uriImageResult.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            return true;
        }
    }
}
