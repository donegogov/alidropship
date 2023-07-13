using System;
using System.Collections.Generic;
using Iop.Api.Util;

namespace Iop.Api
{
    /// <summary>
    /// International Open Platform basic request.
    /// </summary>
    public class IopRequest
    {
        /// <summary>
        /// API name
        /// </summary>
        private string apiName;

        /// <summary>
        /// API parameters
        /// </summary>
        private IopDictionary apiParams;

        /// <summary>
        /// File parameters
        /// </summary>
        private IDictionary<string, FileItem> fileParams;
        /// <summary>
        /// HTTP header parameters
        /// </summary>
        private IopDictionary headerParams;

        /// <summary>
        /// HTTP method: GET or POST, default is POST
        /// </summary>
        private string httpMethod = Constants.METHOD_POST;

        private string simplify = "false";//兼容TOP协议

        private string format = "json";//兼容TOP协议

        public IopRequest()
        {

        }

        public IopRequest(string apiName)
        {
            this.apiName = apiName;
        }

        public void AddApiParameter(string key, string value)
        {
            if (this.apiParams == null)
            {
                this.apiParams = new IopDictionary();
            }
            this.apiParams.Add(key, value);
        }

        public void AddFileParameter(string key, FileItem file)
        {
            if (this.fileParams == null)
            {
                this.fileParams = new Dictionary<string, FileItem>();
            }
            this.fileParams.Add(key, file);
        }

        public void AddHeaderParameter(string key, string value)
        {
            if (this.headerParams == null)
            {
                this.headerParams = new IopDictionary();
            }
            this.headerParams.Add(key, value);
        }

        public string GetApiName()
        {
            return this.apiName;
        }

        public void SetApiName(string apiName)
        {
            this.apiName = apiName;
        }

        public string GetHttpMethod()
        {
            return this.httpMethod;
        }

        public void SetHttpMethod(string httpMethod)
        {
            this.httpMethod = httpMethod;
        }

        public IDictionary<string, string> GetParameters()
        {
            if (this.apiParams == null)
            {
                this.apiParams = new IopDictionary();
            }
            return this.apiParams;
        }

        public IDictionary<string, FileItem> GetFileParameters()
        {
            return this.fileParams;
        }

        public IDictionary<string, string> GetHeaderParameters()
        {
            return this.headerParams;
        }

        public void SetSimplify(string simplify)
        {
            this.simplify = simplify;
        }

        public string GetSimplify()
        {
            return this.simplify;
        }

        public void SetFormat(string format)
        {
            this.format = format;
        }

        public string GetFormat()
        {
            return this.format;
        }

    }
}
