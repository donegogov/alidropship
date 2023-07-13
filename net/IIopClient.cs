using System;

namespace Iop.Api
{
    /// <summary>
    /// International Open Platform API Client Interface.
    /// </summary>
    public interface IIopClient
    {
        /// <summary>
        /// Execute api request without access token.
        /// </summary>
        /// <param name="request">common api requst</param>
        /// <returns>common response</returns>
        IopResponse Execute(IopRequest request, GopProtocolEnum protocol);

        /// <summary>
        /// Execute api request with access token.
        /// </summary>
        /// <param name="request">common api requst</param>
        /// <param name="session">user access token</param>
        /// <returns>common respons</returns>
        IopResponse Execute(IopRequest request, string accessToken, GopProtocolEnum protocol);

        /// <summary>
        /// execute api with accessToken and timestamp
        /// </summary>
        /// <param name="request">common api requst</param>
        /// <param name="session">user access token</param>
        /// <param name="timestamp">request timestamp</param>
        /// <returns>common respons</returns>
        IopResponse Execute(IopRequest request, string accessToken, DateTime timestamp, GopProtocolEnum protocol);
    }
}
