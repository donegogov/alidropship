using Iop.Api;

namespace AliexpressOpenPlatformAPI.Services
{
    public class DropShippingApiService  : IDropShippingApiService
    {
        public IopResponse ApiGetFeedName(string accessToken)
        {
            IIopClient client = new IopClient(AliExpressDefaults.AliApiURL, AliExpressDefaults.AppKey, AliExpressDefaults.AppSecret);
            IopRequest request = new IopRequest();
            request.SetApiName("aliexpress.ds.feedname.get");
            //request.AddApiParameter("app_signature", "your_signature");
            IopResponse response = client.Execute(request, accessToken, GopProtocolEnum.GOP);
            return response;
        }
    }
}
