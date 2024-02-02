using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace BMSMobile.Services
{
    [AddINotifyPropertyChangedInterface]
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string urlServidor
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(urlServidor), string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(urlServidor), value);
            }
        }

        public static void ClearAllData()
        {
            AppSettings.Clear();
        }
    }
}
