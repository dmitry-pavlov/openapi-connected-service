using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ConnectedServices;

namespace OpenApiConnectedService.Package
{
    [ConnectedServiceHandlerExport("Contoso.SampleService", AppliesTo = "CSharp+Web")]
    internal class Handler : ConnectedServiceHandler
    {
        public override Task<AddServiceInstanceResult> AddServiceInstanceAsync(ConnectedServiceHandlerContext context,
            CancellationToken ct)
        {
            AddServiceInstanceResult result = new AddServiceInstanceResult(
                "Sample",
                new Uri("https://github.com/Microsoft/ConnectedServicesSdkSamples"));
            return Task.FromResult(result);
        }
    }
}