using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.ConnectedServices;

namespace OpenApiConnectedService.Package
{
    public class Instance : ConnectedServiceInstance
    {
        public Instance()
        {
            InstanceId = Constants.ExtensionCategory;
            Name = Constants.ExtensionName;
        }

        public string ServiceUri
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        private void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) return;

            if (!Metadata.ContainsKey(propertyName))
            {
                Metadata.Add(propertyName, value);
                OnPropertyChanged(propertyName);
            }
            else
            {
                var currentValue = (T) Metadata[propertyName];
                if (!EqualityComparer<T>.Default.Equals(currentValue, value))
                {
                    Metadata[propertyName] = value;
                    OnPropertyChanged(propertyName);
                }
            }
        }

        private T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null && Metadata.ContainsKey(propertyName))
            {
                var currentValue = (T) Metadata[propertyName];
                return currentValue;
            }
            return default(T);
        }
    }
}