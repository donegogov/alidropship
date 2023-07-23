using Iop.Api;

namespace AliexpressOpenPlatformAPI.Services
{
    public interface IDropShippingApiService
    {
        IopResponse ApiGetFeedName(string accessToken);
        IopResponse ApiGetCategory(string accessToken);

        IopResponse ApiGetAliexpressProducts(string accessToken, string country, string targetCurrency, string targetLanguage, string pageSize, string sort, string pageNumber, string categoryId, string feedName);
    }
}
