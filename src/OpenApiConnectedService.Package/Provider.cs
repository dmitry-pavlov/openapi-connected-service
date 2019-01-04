using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ConnectedServices;

namespace OpenApiConnectedService.Package
{
    [ConnectedServiceProviderExport("Contoso.SampleService")]
    internal class Provider : ConnectedServiceProvider
    {
        public Provider()
        {
            this.Category = "Sample";
            this.Name = "Sample Connected Service";
            this.Description = "A sample Connected Services";
            this.Icon = null;
            this.CreatedBy = "Contoso";
            this.Version = new Version(1, 0, 0);
            this.MoreInfoUri = new Uri("https://aka.ms/ConnectedServicesSDK");
        }

        public override Task<ConnectedServiceConfigurator> CreateConfiguratorAsync(ConnectedServiceProviderContext context)
        {
            ConnectedServiceConfigurator configurator = new ViewModels.SinglePageViewModel();
            return Task.FromResult(configurator);
        }
    }
}
