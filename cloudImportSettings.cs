using Playnite.SDK;
using Playnite.SDK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Playnite.SDK;
using Playnite.SDK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using cloudImport.Services;
namespace cloudImport
{
    public class cloudImportSettings : ObservableObject
    {
        //private string option1 = string.Empty;

        //private bool optionThatWontBeSaved = false;

        //public string Option1 { get => option1; set => SetValue(ref option1, value); }

        // Playnite serializes settings object to a JSON object and saves it as text file.
        // If you want to exclude some property from being saved then use `JsonDontSerialize` ignore attribute.
        //string a = "test".Crypt();
        [DontSerialize]
        private bool mountOnStart { get; set; } = true;
        public bool MountOnStart
        {
            get => mountOnStart;
            set
            {
                mountOnStart = value;
                OnPropertyChanged();
            }
        }
        [DontSerialize]
        private string remotePath = string.Empty;
        public string RemotePath
        {
            get => remotePath;
            set
            {
                remotePath = value;
                OnPropertyChanged();
            }
        }
        [DontSerialize]
        private string mountPoint = "*";
        public string MountPoint
        {
            get => mountPoint;
            set
            {
                mountPoint = value;
                OnPropertyChanged();
            }
        }
        [DontSerialize]
        private string extraRcloneCmds = "--vfs-cache-mode=full --file-perms 0777 --dir-perms 0777";
        public string ExtraRcloneCmds
        {
            get => extraRcloneCmds;
            set
            {
                extraRcloneCmds = value;
                OnPropertyChanged();
            }
        }
        [DontSerialize]
        
        private string cryptPass = string.Empty;
        public string CryptPass
        {
            get => cryptPass;
            set
            {
                if(!value.IsBase64())
                {
                    cryptPass = value.Crypt();
                }
                else
                {
                    cryptPass = value;
                }
                OnPropertyChanged();
            }
        }

        [DontSerialize]
        private string rclonePath = string.Empty;
        public string RclonePath
        {
            get => rclonePath;
            set
            {
                rclonePath = value;
                OnPropertyChanged();
            }
        }
        [DontSerialize]
        private string rcloneConfPath = string.Empty;
        public string RcloneConfPath
        {
            get => rcloneConfPath;
            set
            {
                rcloneConfPath = value;
                OnPropertyChanged();
            }
        }
        [DontSerialize]
        private string localSavePath = string.Empty;
        public string LocalSavePath
        {
            get => localSavePath;
            set
            {
                localSavePath = value;
                OnPropertyChanged();
            }
        }
    }

    public class cloudImportSettingsViewModel : ObservableObject, ISettings
    {
        private readonly cloudImport plugin;
        private readonly IPlayniteAPI playniteApi;
        private cloudImportSettings editingClone { get; set; }

        private cloudImportSettings settings;
        public cloudImportSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged();
            }
        }
        public cloudImportSettingsViewModel(cloudImport plugin, IPlayniteAPI playniteApi)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;
            this.playniteApi = playniteApi;
            // Load saved settings.
            var savedSettings = plugin.LoadPluginSettings<cloudImportSettings>();

            // LoadPluginSettings returns null if not saved data is available.
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new cloudImportSettings();
            }
        }
        public RelayCommand<object> BrowseSelectRcloneCommand
        {
            get => new RelayCommand<object>((a) =>
            {
                var filePath = playniteApi.Dialogs.SelectFile("rclone|rclone.exe");
                if (!String.IsNullOrEmpty(filePath))
                {
                    settings.RclonePath = filePath;
                }
            });
        }

        public RelayCommand<object> BrowseSelectRcloneConfCommand
        {
            get => new RelayCommand<object>((a) =>
            {
                var filePath = playniteApi.Dialogs.SelectFile("rclone|rclone.conf");
                if (!String.IsNullOrEmpty(filePath))
                {
                    settings.RcloneConfPath = filePath;
                }
            });
        }
        public RelayCommand<object> LocalSaveCommand
        {
            get => new RelayCommand<object>((a) =>
            {
                var filePath = playniteApi.Dialogs.SelectFolder();
                if (!String.IsNullOrEmpty(filePath))
                {
                    settings.LocalSavePath = filePath;
                }
            });
        }
        public cloudImportSettingsViewModel(cloudImport plugin)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;

            // Load saved settings.
            var savedSettings = plugin.LoadPluginSettings<cloudImportSettings>();

            // LoadPluginSettings returns null if not saved data is available.
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new cloudImportSettings();
            }
        }

        public void BeginEdit()
        {
            // Code executed when settings view is opened and user starts editing values.
            editingClone = Serialization.GetClone(Settings);
            editingClone.CryptPass = Settings.CryptPass.Decrypt();
        }

        public void CancelEdit()
        {
            // Code executed when user decides to cancel any changes made since BeginEdit was called.
            // This method should revert any changes made to Option1 and Option2.
            Settings = editingClone;
        }

        public void EndEdit()
        {
            // Code executed when user decides to confirm changes made since BeginEdit was called.
            // This method should save settings made to Option1 and Option2.
            plugin.SavePluginSettings(Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            // Code execute when user decides to confirm changes made since BeginEdit was called.
            // Executed before EndEdit is called and EndEdit is not called if false is returned.
            // List of errors is presented to user if verification fails.
            errors = new List<string>();
            return true;
        }
    }
}