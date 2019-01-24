using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.Win32;

namespace OpenApiConnectedService.Package.ViewModels
{
    public class SettingsWizardPage : ConnectedServiceWizardPage
    {
        private const string NswagStudioIsNotInstalled = "NSwagStudio is not installed";

        private readonly ConnectedServiceProviderContext _context;

        private readonly IDictionary<string, object> _metadata;

        private readonly string[] _propertyNames = {
            nameof(NswagStudioStatus)
        };

        public string NswagStudioStatus
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public SettingsWizardPage(ConnectedServiceProviderContext context)
        {
            _metadata = new Dictionary<string, object>
            {
                {nameof(NswagStudioStatus), null}
            };

            _context = context;

            // set up the configuration dialog
            Title = Constants.ExtensionName;
            Description = "Check NSwagStudio status";
            Legend = "Settings";
            RefreshNswagStudioStatus();

            View = new Views.SettingsWizardPageView
            {
                DataContext = this
            };
        }

        public void RefreshNswagStudioStatus()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\NSwagFile\shell\open\command"))
            {
                if (key != null)
                {
                    var openCommand = key.GetValue(null).ToString();
                    var commandParts = openCommand.Split(new[] {'"'}, StringSplitOptions.RemoveEmptyEntries);
                    if (commandParts.Length == 2)
                    {
                        var exeFullPath = commandParts.First();
                        NswagStudioStatus = exeFullPath;
                        return;
                    }
                }
            }

            NswagStudioStatus = NswagStudioIsNotInstalled;
        }

        public override async Task OnPageEnteringAsync(WizardEnteringArgs args)
        {
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