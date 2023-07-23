using Iop.Api;

namespace AliexpressOpenPlatformAPI.Services
{
    public class DropShippingApiService  : IDropShippingApiService
    {
        public IopResponse ApiGetCategory(string accessToken)
        {
            IIopClient client = new IopClient(AliExpressDefaults.AliApiURL, AliExpressDefaults.AppKey, AliExpressDefaults.AppSecret);
            IopRequest request = new IopRequest();
            request.SetApiName("aliexpress.ds.category.get");
            //request.AddApiParameter("app_signature", "your_signature");
            IopResponse response = client.Execute(request, GopProtocolEnum.GOP);
            return response;
        }

        public IopResponse ApiGetFeedName(string accessToken)
        {
            IIopClient client = new IopClient(AliExpressDefaults.AliApiURL, AliExpressDefaults.AppKey, AliExpressDefaults.AppSecret);
            IopRequest request = new IopRequest();
            request.SetApiName("aliexpress.ds.feedname.get");
            //request.AddApiParameter("app_signature", "your_signature");
            IopResponse response = client.Execute(request, GopProtocolEnum.GOP);
            return response;
        }
        public IopResponse ApiGetAliexpressProducts(string accessToken,
            string country,
            string targetCurrency,
            string targetLanguage,
            string pageSize,
            string sort,
            string pageNumber,
            string categoryId,
            string feedName)
        {
            IIopClient client = new IopClient(AliExpressDefaults.AliApiURL, AliExpressDefaults.AppKey, AliExpressDefaults.AppSecret);
            IopRequest request = new IopRequest();
            request.SetApiName("aliexpress.ds.recommend.feed.get");
            request.AddApiParameter("country", country);
            request.AddApiParameter("target_currency", targetCurrency);
            request.AddApiParameter("target_language", targetLanguage);
            request.AddApiParameter("page_size", pageSize);
            request.AddApiParameter("sort", sort);
            request.AddApiParameter("page_no", pageNumber);
            request.AddApiParameter("category_id", categoryId);
            request.AddApiParameter("feed_name", feedName);
            IopResponse response = client.Execute(request, GopProtocolEnum.GOP);
            
            return response;
        }
    }
}
