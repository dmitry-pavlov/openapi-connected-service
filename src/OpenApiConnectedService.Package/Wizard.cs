using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ConnectedServices;
using OpenApiConnectedService.Package.ViewModels;

namespace OpenApiConnectedService.Package
{
    internal class Wizard : ConnectedServiceWizard
    {
        internal ConnectedServiceProviderContext Context { get; }

        public Wizard(ConnectedServiceProviderContext context)
        {
            Context = context;

            Pages.Add(new ServiceEndpointWizardPage(Context));

            foreach (var page in Pages)
            {
                page.PropertyChanged += OnPagePropertyChanged;
            }
        }

        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsFinishEnabled = Pages.All(page => !page.HasErrors);
            IsNextEnabled = !Pages.First(page => page.IsSelected).HasErrors && Pages.Count > 1;
        }

        public override Task<ConnectedServiceInstance> GetFinishedServiceInstanceAsync()
        {
            var instance = new Instance();

            var serviceEndpointWizardPage = Pages.OfType<ServiceEndpointWizardPage>().Single();
            instance.Name = serviceEndpointWizardPage.ServiceName;
            instance.ServiceUri = serviceEndpointWizardPage.ServiceUri;

            return Task.FromResult<ConnectedServiceInstance>(instance);
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var page in Pages)
            {
                page.PropertyChanged -= OnPagePropertyChanged;
            }
            base.Dispose(disposing);
        }
    }
}