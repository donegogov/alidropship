using Iop.Api;

namespace AliexpressOpenPlatformAPI.Services
{
    public interface IDropShippingApiService
    {
        IopResponse ApiGetFeedName(string accessToken);
        IopResponse ApiGetCategory(string accessToken);
    }
}
