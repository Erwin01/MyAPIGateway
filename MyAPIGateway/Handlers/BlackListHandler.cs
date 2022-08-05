using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyAPIGateway.Handlers
{
    public class BlackListHandler : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken) 
        {

            var myHeader = requestMessage.Headers.FirstOrDefault(c => c.Key == "MyHeader");

            if (myHeader.Value != null && myHeader.Value.Any())
            {
                return await base.SendAsync(requestMessage, cancellationToken);
            }


            var response = new HttpResponseMessage(HttpStatusCode.BadGateway);
            response.ReasonPhrase = "Your header is not valid";

            return await Task.FromResult<HttpResponseMessage>(response);
        }
    }
}
