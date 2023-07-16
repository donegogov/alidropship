using Iop.Api.Util;
using System;
using System.Collections.Generic;

namespace Iop.Api
{
    /// <summary>
    /// International Open Platform client implemetation.
    /// </summary>
    public class IopClient : IIopClient
    {
        internal string serverUrl;
        internal string appKey;
        internal string appSecret;
        internal string signMethod = Constants.SIGN_METHOD_SHA256;
        internal string sdkVersion = "iop-sdk-net-20220713";
        internal string logLevel = Constants.LOG_LEVEL_ERROR;

        internal DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        internal WebUtils webUtils;
        internal IIopLogger topLogger;
        internal bool disableTrace = false; // disable log api error
        internal IDictionary<string, string> customrParameters; // set client custom params

        #region IopClient Constructors

        public IopClient(string serverUrl, string appKey, string appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.serverUrl = serverUrl;
            this.webUtils = new WebUtils();
            //this.topLogger = Iop.Api.IopLogger.Instance;
        }

        #endregion

        public void SetTimeout(int timeout)
        {
            this.webUtils.Timeout = timeout;
        }

        public void SetSignMethod(string signMethod)
        {
            if (signMethod.Equals(Constants.SIGN_METHOD_HMAC) || signMethod.Equals(Constants.SIGN_METHOD_SHA256))
            {
                this.signMethod = signMethod;
            }
        }

        public void SetReadWriteTimeout(int readWriteTimeout)
        {
            this.webUtils.ReadWriteTimeout = readWriteTimeout;
        }

        public void SetDisableTrace(bool disableTrace)
        {
            this.disableTrace = disableTrace;
        }

        public void SetIgnoreSSLCheck(bool ignore)
        {
            this.webUtils.IgnoreSSLCheck = ignore;
        }

        /// <summary>
        /// disable local proxy
        /// </summary>
        public void SetDisableWebProxy(bool disable)
        {
            this.webUtils.DisableWebProxy = disable;
        }

        public void SetMaxConnectionLimit(int limit)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = limit;
        }

        public void SetCustomParameters(IDictionary<string, string> customParameters)
        {
            this.customrParameters = customParameters;
        }

        #region IIopClient Members

        public virtual IopResponse Execute(IopRequest request, GopProtocolEnum protocol)
        {
            return DoExecute(request, null, DateTime.UtcNow, protocol);
        }

        public virtual IopResponse Execute(IopRequest request, string accessToken, GopProtocolEnum protocol)
        {
            return DoExecute(request, accessToken, DateTime.UtcNow, protocol);
        }

        public virtual IopResponse Execute(IopRequest request, string accessToken, DateTime timestamp, GopProtocolEnum protocol)
        {
            return DoExecute(request, accessToken, timestamp, protocol);
        }

        #endregion

        private IopResponse DoExecute(IopRequest request, string accessToken, DateTime timestamp, GopProtocolEnum protocol)
        {

            long start = DateTime.Now.Ticks;

            // add common params
            IopDictionary sysParams = new IopDictionary(request.GetParameters());

            sysParams.Add(Constants.APP_KEY, appKey);
            sysParams.Add(Constants.TIMESTAMP, GetTimestamp(timestamp));
            sysParams.Add(Constants.ACCESS_TOKEN, accessToken);
            sysParams.Add(Constants.PARTNER_ID, sdkVersion);
            sysParams.AddAll(this.customrParameters);
            sysParams.Add(Constants.SIGN_METHOD, this.signMethod);
            sysParams.Add(Constants.METHOD, request.GetApiName());
            sysParams.Add(Constants.FORMAT, request.GetFormat());
            sysParams.Add(Constants.SIMPLIFY, request.GetSimplify());
            if (IsDebugEnabled())
            {
                sysParams.Add(Constants.DEBUG, true);
            }

            string realServerUrl = null;
            if (protocol.Equals(GopProtocolEnum.GOP))
            {
                sysParams.Add(Constants.SIGN, IopUtils.SignRequest(request.GetApiName(), sysParams, appSecret, this.signMethod));
                realServerUrl = GetServerUrl(this.serverUrl, request.GetApiName(), accessToken, false);
            }
            if (protocol.Equals(GopProtocolEnum.TOP))
            {
                sysParams.Add(Constants.SIGN, IopUtils.SignRequest("", sysParams, appSecret, this.signMethod));
                realServerUrl = this.serverUrl + "/sync";
            }
            if (protocol.Equals(GopProtocolEnum.RESTFUL))
            {
                sysParams.Add(Constants.SIGN, IopUtils.SignRequest(request.GetApiName(), sysParams, appSecret, this.signMethod));
                realServerUrl = GetServerUrl(this.serverUrl, request.GetApiName(), accessToken, true);

            }
            string reqUrl = WebUtils.BuildRequestUrl(realServerUrl, sysParams);

            try
            {
                string body = null;
                if (request.GetFileParameters() != null) // if file params is set
                {
                    body = webUtils.DoPost(realServerUrl, sysParams, request.GetFileParameters(), request.GetHeaderParameters());
                }
                else
                {
                    if (request.GetHttpMethod().Equals(Constants.METHOD_POST))
                    {
                        body = webUtils.DoPost(realServerUrl, sysParams, request.GetHeaderParameters());
                    }
                    if (request.GetHttpMethod().Equals(Constants.METHOD_GET))
                    {
                        body = webUtils.DoGet(realServerUrl, sysParams, request.GetHeaderParameters());
                    }
                    if (request.GetHttpMethod().Equals(Constants.METHOD_DELETE))
                    {
                        body = webUtils.DoDel(realServerUrl, sysParams);
                    }
                    if (request.GetHttpMethod().Equals(Constants.METHOD_PUT))
                    {
                        body = webUtils.DoPut(realServerUrl, sysParams, request.GetHeaderParameters());
                    }
                }

                IopResponse response = ParseResponse(body);

                // log error response
                if (response.IsError())
                {
                    TimeSpan latency = new TimeSpan(DateTime.Now.Ticks - start);
                    //LogApiError(appKey, sdkVersion, request.GetApiName(), serverUrl, sysParams, latency.TotalMilliseconds, response.Body);
                }
                else
                {
                    if (IsDebugEnabled() || IsInfoEnabled())
                    {
                        TimeSpan latency = new TimeSpan(DateTime.Now.Ticks - start);
                        //LogApiError(appKey, sdkVersion, request.GetApiName(), serverUrl, sysParams, latency.TotalMilliseconds, response.Body);
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                TimeSpan latency = new TimeSpan(DateTime.Now.Ticks - start);
                //LogApiError(appKey, sdkVersion, request.GetApiName(), serverUrl, sysParams, latency.TotalMilliseconds, e.GetType() + ": " + e.Message);
                throw e;
            }
        }

        private IopResponse ParseResponse(string jsonRsp)
        {
            IDictionary<string, object> root = FastJSON.JSON.Parse(jsonRsp) as IDictionary<string, object>;
            IopResponse iopRsp = new IopResponse();
            if (root.ContainsKey(Constants.RSP_ERR_RSP)) {
                object tmp = root[Constants.RSP_ERR_RSP];
                root = tmp as IDictionary<string, object>;
            }
            iopRsp.Type = GetStringValue(root, Constants.RSP_TYPE);
            iopRsp.Code = GetStringValue(root, Constants.RSP_CODE);
            iopRsp.Message = GetStringValue(root, Constants.RSP_MSG);
            iopRsp.RequestId = GetStringValue(root, Constants.RSP_REQUEST_ID);
            iopRsp.Body = jsonRsp;
            return iopRsp;
        }

        private String GetStringValue(IDictionary<string, object> raw, String key)
        {
            if (raw.ContainsKey(key))
            {
                object value = raw[key];
                if (value != null)
                {
                    return (String)value;
                }
            }
            return null;
        }

        private long GetTimestamp(DateTime dateTime)
        {
            return (dateTime.Ticks - dt1970.Ticks) / 10000;
        }

        private bool IsDebugEnabled()
        {
            return logLevel == Constants.LOG_LEVEL_DEBUG;
        }

        private bool IsInfoEnabled()
        {
            return logLevel == Constants.LOG_LEVEL_INFO;
        }

        private bool IsErrorEnabled()
        {
            return logLevel == Constants.LOG_LEVEL_ERROR;
        }

        public void SetLogLevel(String logLevel)
        {
            this.logLevel = logLevel;
        }

        
        internal virtual string GetServerUrl(string serverUrl, string apiName, string session, bool newVersion)
        {
            if (apiName == null || apiName.Length == 0)
            {
                return serverUrl;
            }
            bool hasPrepend = serverUrl.EndsWith("/");
            if (hasPrepend)
            {
                serverUrl = serverUrl.Substring(0, serverUrl.Length - 1);
            }
            serverUrl += "/rest";
            if (newVersion)
            {
                serverUrl += "/2.0";
            }

            return serverUrl + apiName;
            
        }

        internal void LogApiError(string appKey, String sdkVersion, string apiName, string url, Dictionary<string, string> parameters, double latency, string errorMessage)
        {
            if (!disableTrace)
            {
                this.topLogger.TraceApiError(appKey, sdkVersion, apiName, url, parameters, latency, errorMessage);
            }
        }
    }
}
