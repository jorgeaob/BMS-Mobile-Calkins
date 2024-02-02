using Plugin.Settings;
using Plugin.Settings.Abstractions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ConexionModel
    {
        public static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        #region Settings Constants
        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;
        #endregion

        public static string GeneralSettings
        {
            get { return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(SettingsKey, value); }
        }

        public string _cadena;
        public static string cadena
        {
            get => AppSettings.GetValueOrDefault(nameof(cadena), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(cadena), value);
        }

        public string cadenaConexion
        {
            get { return _cadena; }
            set { _cadena = value; }
        }
    }
}
