using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyAPIGateway.Handlers
{
    public class RemoveEncodingDeletingHandler : DelegatingHandler
    {
        /// <summary>
        /// Remove accept Encoding from Header
        /// Add a handler before launching the request and remove the Encoding, the same in the Postman
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> api/userPosts </returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.AcceptEncoding.Clear();
            return await base.SendAsync(request, cancellationToken: CancellationToken.None);
        }
    }
}
