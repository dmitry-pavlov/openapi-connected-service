using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using OpenApiConnectedService.Package.ViewModels;

namespace OpenApiConnectedService.Package.Views
{
    /// <summary>
    /// Interaction logic for SettingsWizardPageView.xaml
    /// </summary>
    public partial class SettingsWizardPageView : UserControl
    {
        public SettingsWizardPageView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            var page = DataContext as SettingsWizardPage;
            page?.RefreshNswagStudioStatus();
        }
    }
}
