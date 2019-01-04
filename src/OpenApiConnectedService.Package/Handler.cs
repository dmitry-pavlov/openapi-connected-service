using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ConnectedServices;

namespace OpenApiConnectedService.Package
{
    [ConnectedServiceHandlerExport(Constants.ProviderId, AppliesTo = "CSharp+Web")]
    internal class Handler : ConnectedServiceHandler
    {
        public override Task<AddServiceInstanceResult> AddServiceInstanceAsync(ConnectedServiceHandlerContext context,
            CancellationToken ct)
        {
            var serviceInstance = context.ServiceInstance;

            var result = new AddServiceInstanceResult(
                serviceInstance.Name,
                new Uri(Constants.Website));

            return Task.FromResult(result);
        }
    }
}