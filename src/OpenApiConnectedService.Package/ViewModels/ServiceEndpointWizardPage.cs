using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ConnectedServices;
using OpenApiConnectedService.Package.Properties;

namespace OpenApiConnectedService.Package.ViewModels
{
    public class ServiceEndpointWizardPage : ConnectedServiceWizardPage
    {
        private readonly ConnectedServiceProviderContext _context;

        private readonly IDictionary<string, object> _metadata;

        private readonly string[] _propertyNames = {
            nameof(ServiceName),
            nameof(ServiceUri)
        };

        public string ServiceUri
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public string ServiceName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public ServiceEndpointWizardPage(ConnectedServiceProviderContext context)
        {
            _metadata = new Dictionary<string, object>
            {
                {nameof(ServiceName), null},
                {nameof(ServiceUri), null}
            };

            _context = context;

            // set up the configuration dialog
            Title = Constants.ExtensionName;
            Description = "Specify the service to add";
            Legend = "Service Endpoint";
            ServiceName = Settings.Default.ServiceName;
            ServiceUri = Settings.Default.ServiceUri;

            View = new Views.ServiceEndpointWizardPageView
            {
                DataContext = this
            };
        }

        public override async Task OnPageEnteringAsync(WizardEnteringArgs args)
        {
           // Wizard.IsNextEnabled = !string.IsNullOrEmpty(ServiceUri);
           // Wizard.IsFinishEnabled = !string.IsNullOrEmpty(ServiceUri);
            await _context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, "On Page Entering");
            await base.OnPageEnteringAsync(args);
        }

        public override async Task<PageNavigationResult> OnPageLeavingAsync(WizardLeavingArgs args)
        {
            await _context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, "On Page Leaving");


            return await base.OnPageLeavingAsync(args);
        }

        protected override void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (_propertyNames.Contains(propertyName))
            {
                HasErrors = !IsPageConfigured();
            }

            base.OnPropertyChanged(propertyName);
        }

        private bool IsPageConfigured()
        {
            if (string.IsNullOrEmpty(ServiceName)) return false;
            if (!Uri.TryCreate(ServiceUri, UriKind.Absolute, out Uri uri)) return false;

            try
            {
                CodeGenerator.ValidateIdentifiers(new CodeNamespace(ServiceName));
            }
            catch(ArgumentException)
            {
                return false;
            }

            return true;
        }

        private void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) return;

            if (!_metadata.ContainsKey(propertyName))
            {
                _metadata.Add(propertyName, value);
                OnPropertyChanged(propertyName);
            }
            else
            {
                var currentValue = (T) _metadata[propertyName];
                if (!EqualityComparer<T>.Default.Equals(currentValue, value))
                {
                    _metadata[propertyName] = value;
                    OnPropertyChanged(propertyName);
                }
            }
        }

        private T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null && _metadata.ContainsKey(propertyName))
            {
                var currentValue = (T) _metadata[propertyName];
                return currentValue;
            }
            return default(T);
        }
    }
}