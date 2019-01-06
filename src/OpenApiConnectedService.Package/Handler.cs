using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ConnectedServices;

namespace OpenApiConnectedService.Package
{
    [ConnectedServiceHandlerExport(Constants.ProviderId, AppliesTo = "CSharp+Web")]
    internal class Handler : ConnectedServiceHandler
    {
        public override async Task<AddServiceInstanceResult> AddServiceInstanceAsync(ConnectedServiceHandlerContext context,
            CancellationToken ct)
        {
            await GenerateAsync(context);

            var folderName = context.ServiceInstance.Name;
            var gettingStartedUrl = new Uri(Constants.Website);
            var result = new AddServiceInstanceResult(folderName, gettingStartedUrl);

            return result;
        }

        private async Task GenerateAsync(ConnectedServiceHandlerContext context)
        {
            var instance = (Instance) context.ServiceInstance;

            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, $"Generating {instance.Name} from {instance.ServiceUri}");
        }
    }
}