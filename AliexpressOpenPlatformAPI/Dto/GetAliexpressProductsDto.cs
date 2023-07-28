using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressOpenPlatformAPI.Dto
{
    public class GetAliexpressProductsDto
    {
        public string AccessToken { get; set; }
        public string StoreUrl { get; set; }
        public string Country { get; set; }
        public string TargetCurrency { get; set; }
        public string TargetLanguage { get; set; }
        public string PageSize { get; set; }
        public string Sort { get; set; }
        public string PageNumber { get; set; }
        public string CategoryId { get; set; }
        public string FeedName { get; set; }
    }
}
