using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Horus.Services.Api
{
    public class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _accessTokenProvider;

        public AuthenticatedHttpClientHandler(IAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _accessTokenProvider.ObserveToken.Take(1).ToTask(cancellationToken);

            if (token == null)
            {
                throw new Exception("Unauthorized, token is null");
            }
            
            request.Headers.Add("Authorization", token);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}