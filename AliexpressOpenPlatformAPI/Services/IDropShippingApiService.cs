using Iop.Api;

namespace AliexpressOpenPlatformAPI.Services
{
    public interface IDropShippingApiService
    {
        IopResponse GetFeedName(string accessToken);
    }
}
