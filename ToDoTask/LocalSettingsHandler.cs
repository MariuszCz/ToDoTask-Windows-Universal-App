using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ToDoTask
{
    class LocalSettingsHandler
    {
            
        private ApplicationDataContainer localSettings;
        public LocalSettingsHandler()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }
        public void saveToLoadSettings(String key, String value)
        {
            localSettings.Values[key] = value;
        }

        public String getFromLoadSettings(String key)
        {
            Object value = localSettings.Values[key];

            if (value == null)
            {
                return null;
            }
            else
            {
                return value.ToString();
            }
        }

    }
}

