using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using cloudImport.Services;
//using cloudImport.Common;
//using Newtonsoft.Json;
using Playnite.SDK;
using Playnite.SDK.Plugins;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Management;

namespace cloudImport
{
    public class cloudImport : GenericPlugin
    {
        string desc;
        string button;
        private static readonly ILogger logger = LogManager.GetLogger();
        //private readonly OfflineDownloader offlineDownloader;
        //private readonly RecursiveFileProcessor directorySearch;
        private cloudImportSettingsViewModel settings { get; set; }
        public string ico = char.ConvertFromUtf32(0xee51);
        public override Guid Id { get; } = Guid.Parse("01070387-0e2b-41e7-a634-8bfbc2793ea3");

        public cloudImport(IPlayniteAPI api) : base(api)
        {
            settings = new cloudImportSettingsViewModel(this, PlayniteApi);
            Properties = new GenericPluginProperties
            {
                HasSettings = true
            };
            //offlineDownloader = new OfflineDownloader();
            //directorySearch = new RecursiveFileProcessor();


        }

        
        public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
        {
            string desc = "Import Game/Rom(s) From The Cloud";
            string desc2 = "Remove Game From Local Storage";
            //string button = "Download";

            //TODO Move each action to separate methods?
            var gameMenuItems = new List<GameMenuItem>
            {

                new GameMenuItem
                {

                    Description = desc,
                    MenuSection = $"PlayX|",
                    Action = _ =>
                    {
                        if (Process.GetProcessesByName("rclone").Length > 0)
                        {
                            // Is running
                        
                            //code to run on press
                            string localDownloadPath = settings.Settings.LocalSavePath;
                            var game = PlayniteApi.MainView.SelectedGames.Last();
                            if (game.Platforms.Any(p => p.SpecificationId == "pc_windows"))
                            {

                                string result = processGame(game,localDownloadPath);
                                //string result = "test"
                                if (!String.IsNullOrEmpty(result) && result != "cancel")
                                {
                                    PlayniteApi.Dialogs.ShowMessage("Game imported! :D.");
                                    string driveTagPrefix = "[PPI]";
                                    string tagName = string.Empty;
                                                            
                                    FileInfo s = new FileInfo(game.InstallDirectory);
                                    //string sourceDrive = System.IO.Path.GetPathRoot(s.FullName).ToUpper();
                                    tagName = string.Format("{0} {1}", driveTagPrefix,@game.InstallDirectory);
                                    Tag driveTag = PlayniteApi.Database.Tags.Add(tagName);
                                    AddTag(game, driveTag);
                                    game.InstallDirectory = result;
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(result) && result == "cancel")
                                    {
                                        PlayniteApi.Dialogs.ShowMessage("Cancelled.");
                                        return;
                                    }
                                    PlayniteApi.Dialogs.ShowMessage("Import failed, can't find the game on the cloud.");
                                    return;
                                }
                                
                                //offlineDownloader = new OfflineDownloader(game);

                                //offlineDownloader.downloadFromCloud(game);
                            }

                            else
                            {
                                //return;
                                //check if game is rom
                                //PlayniteApi.Dialogs.ShowMessage(game.Platforms.FirstOrDefault().SpecificationId.ToString());//sony_playstation3, nintendo_wiiu
                                //return;
                                if(game.Roms.Any())
                                {
                                    if (game.Platforms.Any(p => p.SpecificationId == "sony_playstation3"))
                                    {

                                        string result = processGame(game,localDownloadPath);
                                        //string result = "test"
                                        if (!String.IsNullOrEmpty(result) && result != "cancel")
                                        {
                                            PlayniteApi.Dialogs.ShowMessage("Game imported! :D.");
                                            string driveTagPrefix = "[PPI]";
                                            string tagName = string.Empty;

                                            FileInfo s = new FileInfo(game.InstallDirectory);
                                            //string sourceDrive = System.IO.Path.GetPathRoot(s.FullName).ToUpper();
                                            tagName = string.Format("{0} {1}", driveTagPrefix,@game.InstallDirectory);
                                            Tag driveTag = PlayniteApi.Database.Tags.Add(tagName);
                                            AddTag(game, driveTag);
                                            game.InstallDirectory = result;
                                        }
                                        else
                                        {
                                            if (!String.IsNullOrEmpty(result) && result == "cancel")
                                            {
                                                PlayniteApi.Dialogs.ShowMessage("Cancelled.");
                                                return;
                                            }
                                            PlayniteApi.Dialogs.ShowMessage("Import failed, can't find the game on the cloud.");
                                            return;
                                        }
                                
                                        //offlineDownloader = new OfflineDownloader(game);

                                        //offlineDownloader.downloadFromCloud(game);
                                    }

                                    if (game.Platforms.Any(p => p.SpecificationId == "nintendo_wiiu"))
                                    {

                                        string result = processGame(game,localDownloadPath);
                                        //string result = "test"
                                        if (!String.IsNullOrEmpty(result) && result != "cancel")
                                        {
                                            PlayniteApi.Dialogs.ShowMessage("Game imported! :D.");
                                            string driveTagPrefix = "[PPI]";
                                            string tagName = string.Empty;

                                            FileInfo s = new FileInfo(game.InstallDirectory);
                                            //string sourceDrive = System.IO.Path.GetPathRoot(s.FullName).ToUpper();
                                            tagName = string.Format("{0} {1}", driveTagPrefix,@game.InstallDirectory);
                                            Tag driveTag = PlayniteApi.Database.Tags.Add(tagName);
                                            AddTag(game, driveTag);
                                            game.InstallDirectory = result;
                                            return;
                                        }
                                        else
                                        {
                                            if (!String.IsNullOrEmpty(result) && result == "cancel")
                                            {
                                                PlayniteApi.Dialogs.ShowMessage("Cancelled.");
                                                return;
                                            }
                                            PlayniteApi.Dialogs.ShowMessage("Import failed, can't find the game on the cloud.");
                                            return;
                                        }
                                
                                        //offlineDownloader = new OfflineDownloader(game);

                                        //offlineDownloader.downloadFromCloud(game);
                                    }
                                    var progressOptions = new GlobalProgressOptions("Starting Import!", true);
                                    progressOptions.IsIndeterminate = false;
                                    int countFiles = game.Roms.Count();

                                    bool tagged = false;
                                    string orgInstallDir = game.InstallDirectory;
                                    PlayniteApi.Dialogs.ActivateGlobalProgress((a) =>
                                    {
                                        a.ProgressMaxValue = countFiles;
                                        foreach(var rom in game.Roms)
                                        {
                                            a.Text ="Importing " + rom.Name;
                                            //PlayniteApi.Dialogs.ShowMessage(rom.Path);
                                            //var installDir = @game.InstallDirectory;
                                            string romPath = @rom.Path.Replace(@"{InstallDir}\",@orgInstallDir);

                                            var fullPath = Path.Combine(@game.InstallDirectory,romPath);
                                            //PlayniteApi.Dialogs.ShowMessage(fullPath);
                                            var rootPath = Path.GetPathRoot(game.InstallDirectory);
                                            var availableDrives = DriveInfo.GetDrives().Where(d => d.IsReady);
                                            if (!String.IsNullOrEmpty(game.InstallDirectory))
                                            {
                                                foreach (var drive in availableDrives)
                                                {
                                                    var pathWithoutDrive = fullPath.Substring(drive.Name.Length);
                                                    var newPath = Path.Combine(drive.Name, pathWithoutDrive);
                                                    //PlayniteApi.Dialogs.ShowMessage(newPath);
                                                    if (File.Exists(newPath))
                                                    {
                                                        //get roms platform
                                                        //PlayniteApi.Dialogs.ShowMessage("File Exists");
                                                        string pID = game.Platforms.FirstOrDefault().ToString();
                                                        //PlayniteApi.Dialogs.ShowMessage(pID);
                                                        //check if directory exists
                                                        string basePath = settings.Settings.LocalSavePath;
                                                        string platPath = Path.Combine(basePath, pID);
                                                        if (!Directory.Exists(platPath))
                                                        {
                                                            Directory.CreateDirectory(platPath);
                                                            string gameName = game.Name.Replace(":","");
                                                            string gameLocalDir = Path.Combine(platPath, gameName);
                                                            if (!Directory.Exists(gameLocalDir))
                                                            {
                                                                Directory.CreateDirectory(gameLocalDir);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string gameName = game.Name.Replace(":","");
                                                            string gameLocalDir = Path.Combine(platPath, gameName);
                                                            if (!Directory.Exists(gameLocalDir))
                                                            {
                                                                Directory.CreateDirectory(gameLocalDir);
                                                            }
                                                        }
                                                        string gameNameClean = game.Name.Replace(":","").Trim();
                                                        string destPath = Path.Combine(basePath,platPath,gameNameClean,@rom.Path.Replace(@"{InstallDir}\",""));
                                                        //PlayniteApi.Dialogs.ShowMessage(destPath);
                                                        if(!File.Exists(destPath))
                                                        {
                                                            //string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                                                            //PlayniteApi.Dialogs.ShowMessage(invalid);
                                                            //PlayniteApi.Dialogs.ShowMessage("Copy");
                                                            File.Copy(@newPath,@destPath);
                                                            a.CurrentProgressValue++;
                                                            //exception for ps1 games as they contain cue and bin files per rom
                                                            if (game.Platforms.FirstOrDefault().SpecificationId == "sony_playstation")
                                                            {
                                                                File.Copy(@newPath.Replace(".cue",".bin"),@destPath.Replace(".cue",".bin"));
                                                            }
                                                            if (tagged==false)
                                                            {
                                                                
                                                                string driveTagPrefix = "[PPI]";
                                                                string tagName = string.Empty;

                                                                FileInfo s = new FileInfo(game.InstallDirectory);
                                                                //string sourceDrive = System.IO.Path.GetPathRoot(s.FullName).ToUpper();
                                                                tagName = string.Format("{0} {1}", driveTagPrefix, orgInstallDir);
                                                                Tag driveTag = PlayniteApi.Database.Tags.Add(tagName);
                                                                AddTag(game, driveTag);
                                                                tagged = true;
                                                                if (orgInstallDir != Path.Combine(basePath,platPath,gameNameClean))
                                                                {
                                                                    game.InstallDirectory = Path.Combine(basePath,platPath,gameNameClean);

                                                                }
                                                                //Tag tag =  String.Format("[PPI] {0}",game.InstallDirectory);
                                                                //game.Tags.Add(tag);
                                                            }




                                                        }
                                                        else
                                                        {
                                                            PlayniteApi.Dialogs.ShowErrorMessage("Game is already in the local folder","PlayX");
                                                        }
                                                    

                                                        //existringPath = newPath;

                                                        //return true;
                                                    }
                                                    //if (predicate(newPath))
                                                    //{
                                                    //    existringPath = newPath;

                                                    //return existringPath;
                                                    //}
                                                    //return false;
                                                }
                                            }
                                        }
                                    }, progressOptions);
                                    PlayniteApi.Dialogs.ShowMessage("Roms imported! :D.");
                                    return;
        }
                            }
                        }


                        else
                        {
                            PlayniteApi.Dialogs.ShowErrorMessage("Rclone is not running!","PlayX");
                        }
                    }

                },
                new GameMenuItem
                {

                    Description = desc2,
                    MenuSection = $"PlayX|",
                    Action = _ =>
                    {
                        bool shouldProcess = false;
                        var game = PlayniteApi.MainView.SelectedGames.Last();
                        foreach(Tag tag in game.Tags)
                        {
                            if(tag.Name.Contains("[PPI]"))
                            {
                                shouldProcess = true;
                            }
                        }
                        if(shouldProcess==true)
                        {
                            if (Process.GetProcessesByName("rclone").Length > 0)
                            {
                                // Is running
                        
                                //code to run on press
                            
                                if (game.Platforms.Any(p => p.SpecificationId == "sony_playstation"))
                                {
                                    //var localDownloadPath = settings.Settings.LocalSavePath;
                                    //string result = offlineDownloader.processGame(game,localDownloadPath);

                                    string prefix = "[PPI]";
                                    foreach(Tag tag in game.Tags)
                                    {

                                        if(tag.Name.Contains(prefix))
                                        {
                                            string curInstall = game.InstallDirectory;
                                            string rootInstall = curInstall;

                                            string orgInstall = tag.Name.Substring(prefix.Length).Trim();
                                            //PlayniteApi.Dialogs.ShowMessage(orgInstall);
                                            game.InstallDirectory = orgInstall;
                                            //localPath = Path.Combine(curInstall)
                                            if(Directory.Exists(rootInstall))
                                            {
                                                //PlayniteApi.Dialogs.ShowMessage("delete");
                                                //PlayniteApi.Dialogs.ShowMessage(rootInstall);
                                                Directory.Delete(rootInstall,true);
                                            }
                                            PlayniteApi.Dialogs.ShowMessage("Removed from local storage!");
                                            RemoveTag(game,tag);
                                            return;
                                        }
                                    }

                                }
                                if (game.Platforms.Any(p => p.SpecificationId == "sony_playstation3"))
                                {
                                    //var localDownloadPath = settings.Settings.LocalSavePath;
                                    //string result = offlineDownloader.processGame(game,localDownloadPath);

                                    string prefix = "[PPI]";
                                    foreach(Tag tag in game.Tags)
                                    {

                                        if(tag.Name.Contains(prefix))
                                        {
                                            string curInstall = game.InstallDirectory;
                                            string rootInstall = Path.GetFullPath(Path.Combine(curInstall, @"..\..\"));

                                            string orgInstall = tag.Name.Substring(prefix.Length).Trim();
                                            //PlayniteApi.Dialogs.ShowMessage(orgInstall);
                                            game.InstallDirectory = orgInstall;
                                            //localPath = Path.Combine(curInstall)
                                            if(Directory.Exists(rootInstall))
                                            {
                                                //PlayniteApi.Dialogs.ShowMessage("delete");
                                                //PlayniteApi.Dialogs.ShowMessage(rootInstall);
                                                Directory.Delete(rootInstall,true);
                                            }
                                            PlayniteApi.Dialogs.ShowMessage("Removed from local storage!");
                                            RemoveTag(game,tag);
                                            return;
                                        }
                                    }

                                }
                                if (game.Platforms.Any(p => p.SpecificationId == "nintendo_wiiu"))
                                {
                                    //var localDownloadPath = settings.Settings.LocalSavePath;
                                    //string result = offlineDownloader.processGame(game,localDownloadPath);

                                    string prefix = "[PPI]";
                                    foreach(Tag tag in game.Tags)
                                    {

                                        if(tag.Name.Contains(prefix))
                                        {
                                            string curInstall = game.InstallDirectory;
                                            string rootInstall = Path.GetFullPath(Path.Combine(curInstall, @"..\"));
                                            string orgInstall = tag.Name.Substring(prefix.Length).Trim();
                                            //PlayniteApi.Dialogs.ShowMessage(orgInstall);
                                            game.InstallDirectory = orgInstall;
                                            //localPath = Path.Combine(curInstall)
                                            if(Directory.Exists(rootInstall))
                                            {
                                                //PlayniteApi.Dialogs.ShowMessage("delete");
                                                Directory.Delete(rootInstall,true);
                                            }
                                            PlayniteApi.Dialogs.ShowMessage("Removed from local storage!");
                                            RemoveTag(game,tag);
                                            return;
                                        }
                                    }

                                }
                                if (game.Platforms.Any(p => p.SpecificationId == "pc_windows"))
                                {
                                    //var localDownloadPath = settings.Settings.LocalSavePath;
                                    //string result = offlineDownloader.processGame(game,localDownloadPath);

                                    string prefix = "[PPI]";
                                    foreach(Tag tag in game.Tags)
                                    {

                                        if(tag.Name.Contains(prefix))
                                        {
                                            string curInstall = game.InstallDirectory;
                                            string orgInstall = tag.Name.Substring(prefix.Length).Trim();
                                            //PlayniteApi.Dialogs.ShowMessage(orgInstall);
                                            game.InstallDirectory = orgInstall;
                                            //localPath = Path.Combine(curInstall)
                                            if(Directory.Exists(curInstall))
                                            {
                                                //PlayniteApi.Dialogs.ShowMessage("delete");
                                                Directory.Delete(curInstall,true);
                                            }
                                            PlayniteApi.Dialogs.ShowMessage("Removed from local storage!");
                                            RemoveTag(game,tag);
                                            return;
                                        }
                                    }
                                    return;
                                }
                                else
                                {
                           
                                    //game is rom?
                                    //check tags
                                    string prefix = "[PPI]";
                                    foreach(Tag tag in game.Tags)
                                    {

                                        if(tag.Name.Contains(prefix))
                                        {
                                            string curInstall = game.InstallDirectory;
                                            string orgInstall = tag.Name.Substring(prefix.Length).Trim();
                                            //PlayniteApi.Dialogs.ShowMessage(orgInstall);
                                            game.InstallDirectory = orgInstall;
                                            //localPath = Path.Combine(curInstall)
                                            IDictionary<string, int> pathPartsScore = new Dictionary<string, int>();
                                            IDictionary<string, int> pathPartsScoreClone = new Dictionary<string, int>();
                                            var pInstallDir = curInstall.ToString();
                                            foreach (string InstallListPart in pInstallDir.Split(Path.DirectorySeparatorChar))
                                            {
                                                pathPartsScore.Add(InstallListPart, 0);
                                                pathPartsScoreClone.Add(InstallListPart, 0);
                                            }
                                            foreach(string gameNamePart in game.Name.Trim().Replace(":","").Split(null))
                                            {
                                                foreach(string key in pathPartsScore.Keys)
                                                {
                                                    foreach (string gameNamePartWord in gameNamePart.Split(null))
                                                    {
                                                        if (key.Contains(gameNamePartWord))
                                                            {
                                                            //int val = pathPartsScore[key];
                                                            pathPartsScoreClone[key] = pathPartsScoreClone[key] + 1;
                                                        }
                                                    }
                                                }
                                            }
                                            var max = pathPartsScoreClone.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                                            var pathWithoutDriveArray = curInstall.Split(Path.DirectorySeparatorChar).ToList();
                                            int count = pathWithoutDriveArray.IndexOf(max);
                                            pathWithoutDriveArray.RemoveRange(count + 1, pathWithoutDriveArray.Count() - count - 1);
                                            curInstall = String.Join(Path.DirectorySeparatorChar.ToString(), pathWithoutDriveArray);
                                            //PlayniteApi.Dialogs.ShowMessage(curInstall);
                                            if(Directory.Exists(curInstall))
                                            {
                                                //PlayniteApi.Dialogs.ShowMessage("delete");
                                                Directory.Delete(curInstall,true);
                                            }
                                            PlayniteApi.Dialogs.ShowMessage("Removed from local storage!");
                                            RemoveTag(game,tag);
                                            return;
                                        }
                                    }
                                }
                                return;
                            }
                            else
                            {
                                PlayniteApi.Dialogs.ShowErrorMessage("Rclone is not running!","PlayX");
                            }
                        }
                    }
                 }
            };
            return gameMenuItems;
        }
        /*
        public bool getRcloneMount(Game game, string platform, out string existringPath)
        {
            existringPath = null;
            var rootPath = Path.GetPathRoot(game.InstallDirectory);
            var availableDrives = DriveInfo.GetDrives().Where(d => d.IsReady);
            if (!String.IsNullOrEmpty(game.InstallDirectory))
            {
                foreach (var drive in availableDrives)
                {
                    string pathWithoutDrive;
                    if (game.Platforms.FirstOrDefault().SpecificationId != "pc_windows")
                    { pathWithoutDrive = game.InstallDirectory.Substring(drive.Name.Length); }
                    else
                    {
                        IDictionary<string, int> pathPartsScore = new Dictionary<string, int>();
                        var pInstallDir = game.InstallDirectory.ToString();
                        foreach (string InstallListPart in pInstallDir.Split(Path.DirectorySeparatorChar))
                        {
                            gameNamePart
                        }
                    }
        */
        public bool getRcloneMount(Game game, string platform, out string existringPath)
        {
            existringPath = null;
            var rootPath = Path.GetPathRoot(game.InstallDirectory);
            var availableDrives = DriveInfo.GetDrives().Where(d => d.IsReady);
            if (!String.IsNullOrEmpty(game.InstallDirectory))
            {
                foreach (var drive in availableDrives)
                {
                    string pathWithoutDrive;
                    
                    
                    if (game.Platforms.FirstOrDefault().SpecificationId == "pc_windows")
                    {
                        IDictionary<string, int> pathPartsScore = new Dictionary<string, int>();
                        IDictionary<string, int> pathPartsScoreClone = new Dictionary<string, int>();
                        var pInstallDir = game.InstallDirectory.ToString();
                        foreach (string InstallListPart in pInstallDir.Split(Path.DirectorySeparatorChar))
                        {
                            pathPartsScore.Add(InstallListPart, 0);
                            pathPartsScoreClone.Add(InstallListPart, 0);
                        }
                        foreach(string gameNamePart in game.Name.Trim().Replace(":","").Split(null))
                        {
                            foreach(string key in pathPartsScore.Keys)
                            {
                                foreach (string gameNamePartWord in gameNamePart.Split(null))
                                {
                                    if (key.Contains(gameNamePartWord))
                                        {
                                        //int val = pathPartsScore[key];
                                        pathPartsScoreClone[key] = pathPartsScoreClone[key] + 1; 
                                    }
                                }
                            }
                        }
                        var max = pathPartsScoreClone.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                        /*//show values and keys in dict 
                        foreach (KeyValuePair<string, int> kvp in pathPartsScoreClone)
                        {
                            PlayniteApi.Dialogs.ShowMessage(kvp.ToString());
                        }
                        
                        break;
                        */
                        var pathWithoutDriveArray = game.InstallDirectory.Substring(drive.Name.Length).Split(Path.DirectorySeparatorChar).ToList();
                        int count = pathWithoutDriveArray.IndexOf(max);
                        pathWithoutDriveArray.RemoveRange(count + 1, pathWithoutDriveArray.Count() - count - 1);
                        pathWithoutDrive = String.Join(Path.DirectorySeparatorChar.ToString(), pathWithoutDriveArray);
                    }
                    else
                    { 
                    pathWithoutDrive = game.InstallDirectory.Substring(drive.Name.Length);
                    }
                    var newPath = Path.Combine(drive.Name, pathWithoutDrive);
                    //PlayniteApi.Dialogs.ShowMessage(newPath);
                    //break;
                    if (Directory.Exists(newPath))
                    {
                        existringPath = newPath;

                        return true;
                    }
                    //if (predicate(newPath))
                    //{
                    //    existringPath = newPath;

                    //return existringPath;
                    //}
                    //return false;
                }
            }

            return false;
        }
        public string processGame(Game game, string destinationPath)
        {

            string rcGamePath;


            string platform = game.Platforms.FirstOrDefault().SpecificationId.ToString();
            bool foundPath = getRcloneMount(game, platform, out rcGamePath);
            //playniteApi.Dialogs.ShowMessage(test);
            if (foundPath)
            {
                string platDir = Path.Combine(destinationPath, game.Platforms.FirstOrDefault().Name.Trim());
                string destDir = Path.Combine(destinationPath, game.Platforms.FirstOrDefault().Name.Trim(), game.Name.Replace(":", "").Trim()+"\\");
                if (!Directory.Exists(platDir))
                {
                    Directory.CreateDirectory(platDir);
                }

                if (platform == "pc_windows")
                {

                    string sourceDir = rcGamePath;

                    bool CompletionStatus = Copy(sourceDir, destDir);
                    //PlayniteApi.Dialogs.ShowMessage(CompletionStatus.ToString());
                    if (CompletionStatus == true)
                    {
                        string gameEXEpath = game.GameActions.FirstOrDefault().Path;

                        string modifiedDestDir = Path.GetDirectoryName(Directory.GetFiles(destDir,gameEXEpath, SearchOption.AllDirectories)[0]);
                        //PlayniteApi.Dialogs.ShowMessage(modifiedDestDir);
                        return modifiedDestDir;
                    }
                    else
                    {
                        //PlayniteApi.Dialogs.ShowMessage("Cancelled");
                        rcGamePath = "cancel";
                        return rcGamePath;
                    }
                }
                if (platform == "sony_playstation3")
                {
                    string sourceDir = Path.GetFullPath(Path.Combine(rcGamePath, @"..\..\"));
                    bool CompletionStatus = Copy(sourceDir, destDir);
                    //PlayniteApi.Dialogs.ShowMessage(CompletionStatus.ToString());
                    if (CompletionStatus == true)
                    {
                        string modifiedDest = Path.Combine(destDir, "PS3_GAME", "USRDIR");
                        return modifiedDest;
                    }
                    else
                    {
                        //PlayniteApi.Dialogs.ShowMessage("Cancelled");
                        rcGamePath = "cancel";
                        return rcGamePath;
                    }
                    
                }
                
                if (platform == "nintendo_wiiu")
                {
                    string sourceDir = Path.GetFullPath(Path.Combine(rcGamePath, @"..\"));
                    bool CompletionStatus = Copy(sourceDir, destDir);
                    //PlayniteApi.Dialogs.ShowMessage(CompletionStatus.ToString());
                    if (CompletionStatus == true)
                    {
                        string modifiedDest = Path.Combine(destDir, "PS3_GAME", "USRDIR");
                        return modifiedDest;
                    }
                    else
                    {
                        //PlayniteApi.Dialogs.ShowMessage("Cancelled");
                        rcGamePath = "cancel";
                        return rcGamePath;
                    }
                }
                else
                {
                    rcGamePath = null;
                    return rcGamePath;
                }
            }
            else
            {
                rcGamePath = null;
                return rcGamePath;
            }
        }
        public bool Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);
            //PlayniteApi.Dialogs.ShowMessage(sourceDirectory);
            //PlayniteApi.Dialogs.ShowMessage(targetDirectory);
            bool cStatus = false;
            cStatus = CopyFilesRecursively(sourceDirectory, targetDirectory);//CopyAll(diSource, diTarget,true);
            if (cStatus==true)
            {
                return true;
            }
            else
            {
                Directory.Delete(targetDirectory, true);
                return false;
            }
        }
        public bool CopyFilesRecursively(string sourcePath, string targetPath)
        {
            bool cStatus = true;
            var progressOptions = new GlobalProgressOptions("Starting Import!", true);
            progressOptions.IsIndeterminate = false;
            int countFiles = System.IO.Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories).Count();
            int dirs = System.IO.Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories).Count();
            //Now Create all of the directories
            PlayniteApi.Dialogs.ActivateGlobalProgress((a) =>
            {
                a.ProgressMaxValue = countFiles + dirs;
                if(Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories).Count() > 0)
                { 
                    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                    {
                        if (a.CancelToken.IsCancellationRequested)
                        {
                            //stat = false;
                            //CopyAll(diSourceSubDir, nextTargetSubDir, stat);
                            cStatus = false;
                            break;
                        }
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                        a.Text = "Creating Directory: " + dirPath; 
                        a.CurrentProgressValue++;
                    }
                }
                else
                {
                    Directory.CreateDirectory(sourcePath.Replace(sourcePath, targetPath));
                    a.Text = "Creating Directory: " + sourcePath;
                    a.CurrentProgressValue++;
                }
                if (cStatus == true)
                { 
                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                    {
                        if (a.CancelToken.IsCancellationRequested)
                        {
                            //stat = false;
                            //CopyAll(diSourceSubDir, nextTargetSubDir, stat);
                            cStatus = false;
                            break;
                        }
                        a.Text = "Importing File: " + newPath;
                        File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                        
                        a.CurrentProgressValue++;
                    }
                }
            }, progressOptions);
            if (cStatus == false)
            {
                return false;
            }
            else
            {
                return true;
            }
                

        }
        public bool CopyAll(DirectoryInfo source, DirectoryInfo target,bool stat)
        {
            Directory.CreateDirectory(target.FullName);
            var progressOptions = new GlobalProgressOptions("Grabbing Game!", true);
            progressOptions.IsIndeterminate = false;
            //bool stat = true;
            while (stat == true)
            {
                PlayniteApi.Dialogs.ActivateGlobalProgress((a) =>
                {
                    //var games = args.Games.Distinct();
                    //PlayniteApi.Dialogs.ShowMessage(source.FullName);
                    int countFiles = System.IO.Directory.GetDirectories(source.FullName, "*", SearchOption.AllDirectories).Count();
                    int dirs = System.IO.Directory.GetDirectories(source.FullName, "*", SearchOption.AllDirectories).Count();
                    a.ProgressMaxValue = countFiles + dirs;
                    foreach (FileInfo fi in source.GetFiles())
                    {
                        if (a.CancelToken.IsCancellationRequested)
                        {
                            stat = false;
                            //CopyAll(diSourceSubDir, nextTargetSubDir,stat);
                            return;
                        }
                        //a.(string.Format(@"Copying {0}\{1}", target.FullName, fi.Name));
                        Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                        fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                        a.CurrentProgressValue++;
                    }
                    foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                    {
                        DirectoryInfo nextTargetSubDir =
                            target.CreateSubdirectory(diSourceSubDir.Name);
                        if (a.CancelToken.IsCancellationRequested)
                        {
                            //stat = false;
                            CopyAll(diSourceSubDir, nextTargetSubDir, stat);
                            break;
                        }
                        
                        CopyAll(diSourceSubDir, nextTargetSubDir, stat);
                        a.CurrentProgressValue++;
                    }
                }, progressOptions);
            }
            //playniteApi.Dialogs.ShowMessage(ResourceProvider.GetString("LOCExtra_Metadata_Loader_DialogMessageDone"), "Extra Metadata Loader");
            // Copy each file into the new directory.


            // Copy each subdirectory using recursion.
            if(stat==true)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool AddTag(Game game, Tag tag)
        {
            if (game.TagIds == null)
            {
                game.TagIds = new List<Guid> { tag.Id };
                PlayniteApi.Database.Games.Update(game);
                bool tagAdded = true;
                return tagAdded;
            }
            else if (game.TagIds.Contains(tag.Id) == false)
            {
                game.TagIds.AddMissing(tag.Id);
                PlayniteApi.Database.Games.Update(game);
                bool tagAdded = true;
                return tagAdded;
            }
            else
            {
                bool tagAdded = false;
                return tagAdded;
            }
        }
        public bool RemoveTag(Game game, Tag tag, bool updateGame = true)
        {
            var tagRemoved = false;
            if (game.TagIds == null)
            {
                return false;
            }
            else if (game.TagIds.Contains(tag.Id))
            {
                game.TagIds.Remove(tag.Id);
                tagRemoved = true;
            }
            if (tagRemoved && updateGame)
            {
                PlayniteApi.Database.Games.Update(game);
            }
            return tagRemoved;
        }

        public override void OnGameInstalled(OnGameInstalledEventArgs args)
        {
            // Add code to be executed when game is finished installing.
        }

        public override void OnGameStarted(OnGameStartedEventArgs args)
        {
            // Add code to be executed when game is started running.
        }

        public override void OnGameStarting(OnGameStartingEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameStopped(OnGameStoppedEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameUninstalled(OnGameUninstalledEventArgs args)
        {
            // Add code to be executed when game is uninstalled.
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            // Add code to be executed when Playnite is initialized.
            if (Process.GetProcessesByName("rclone").Length <= 0)
            {
                //rclone isn't active
                //start rclone?
                //check if settings are set for plugin
                var rcPath = settings.Settings.RclonePath;
                //Check if rclone path is set
                if (String.IsNullOrEmpty(rcPath))
                {
                    //try to look in playnite directory
                    //string[] appPath = {PlayniteApi.Paths.ApplicationPath};
                    string rcloneEXE = "rclone.exe";

                    //string rcloneScanResult = null;
                    //Console.WriteLine("SCANNING");
                    string[] filePaths = Directory.GetFiles(PlayniteApi.Paths.ApplicationPath, "*.exe", SearchOption.AllDirectories);
                    //Console.WriteLine(filePaths);
                    foreach (var item in filePaths)
                    {
                        if (item.Contains(rcloneEXE))
                        {
                            settings.Settings.RclonePath = item;
                        }

                    }

                    //ask user if he wants to search directory
                    var updatedPath = settings.Settings.RclonePath;
                    if (String.IsNullOrEmpty(updatedPath))
                    {
                        PlayniteApi.Dialogs.ShowErrorMessage("Please review the Rclone settings", "Rclone Exe Not Set");
                    }
                }
                //same for conf file
                var rcConfPath = settings.Settings.RcloneConfPath;
                if (String.IsNullOrEmpty(rcConfPath))
                {
                    //try to look in playnite directory
                    //string[] appPath = {PlayniteApi.Paths.ApplicationPath};
                    string rcloneCONF = "rclone.conf";
                    //string rcloneScanResult = null;
                    //Console.WriteLine("SCANNING");
                    string[] filePaths = Directory.GetFiles(PlayniteApi.Paths.ApplicationPath, "*.conf", SearchOption.AllDirectories);
                    //Console.WriteLine(filePaths);
                    foreach (var item in filePaths)
                    {
                        if (item.Contains(rcloneCONF))
                        {
                            settings.Settings.RcloneConfPath = item;
                        }
                    }
                    var updatedPath = settings.Settings.RcloneConfPath;
                    if (String.IsNullOrEmpty(updatedPath))
                    {
                        PlayniteApi.Dialogs.ShowErrorMessage("Please review the Rclone settings", "Rclone Config Not Set");
                    }
                }

                var localDownloadPath = settings.Settings.LocalSavePath;
                if (String.IsNullOrEmpty(localDownloadPath))
                {
                    //try to look in playnite directory
                    //string[] appPath = {PlayniteApi.Paths.ApplicationPath};
                    //string rcloneCONF = "rclone.conf";
                    //string rcloneScanResult = null;
                    //Console.WriteLine("SCANNING");
                    //string[] filePaths = Directory.GetDirectories(PlayniteApi.Paths.ApplicationPath, "*.conf", SearchOption.AllDirectories);
                    //Console.WriteLine(filePaths);
                    //var appName = "PlayPass";
                    //PlayniteApi.Dialogs.ShowMessage(PlayniteApi.Paths.ExtensionsDataPath);
                    string[] paths = { PlayniteApi.Paths.ApplicationPath, "PlayX" };
                    string expectedPath = Path.Combine(paths);
                    if (!Directory.Exists(expectedPath))
                    {
                        Directory.CreateDirectory(expectedPath);
                        settings.Settings.LocalSavePath = expectedPath;
                    }
                    else
                    {
                        settings.Settings.LocalSavePath = expectedPath;
                    }
                    //var updatedPath = settings.Settings.RcloneConfPath;
                }
                var mountStartup = settings.Settings.MountOnStart;
                bool RisRunning = Process.GetProcessesByName("rclone.exe").Any();
                string rclonePath = settings.Settings.RclonePath;
                string remotePath = settings.Settings.RemotePath;
                string mountPath = settings.Settings.MountPoint;
                string confPass = settings.Settings.CryptPass.Decrypt();
                //PlayniteApi.Dialogs.ShowMessage(confPass);
                string extraArgs = settings.Settings.ExtraRcloneCmds;
                if (RisRunning == false)
                {
                    if (mountStartup == true)
                    {
                        if (File.Exists(rcPath))
                        {
                            if (!String.IsNullOrEmpty(remotePath))
                            {

                                //try to mount
                                //PlayniteApi.Dialogs.ShowMessage("Mounting!");
                                //H:\Playnite\gameStation\mount\rclone.exe mount GameStation: B: --rc--rc - addr localhost: 0--rc - user = p1KaLmIg2k--rc - pass = B7fZp5Tm1qbi6dss92C31a--vfs - cache - mode = full


                                string strCmdPass = String.Format("set RCLONE_CONFIG_PASS={0}", confPass);
                                //PlayniteApi.Dialogs.ShowMessage(rclonePath);
                                //PlayniteApi.Dialogs.ShowMessage(remotePath);
                                //PlayniteApi.Dialogs.ShowMessage(mountPath);
                                //PlayniteApi.Dialogs.ShowMessage(extraArgs);
                                string strCmdMount = String.Format(@"{0} mount {1} {2} {3}", @rclonePath, @remotePath, @mountPath, @extraArgs);
                                //PlayniteApi.Dialogs.ShowMessage(strCmdMount);
                                //strCmdText = "/C rclone";
                                var availableDrives = DriveInfo.GetDrives().Where(d => d.IsReady);
                                Process cmd = new Process();
                                cmd.StartInfo.FileName = "cmd.exe";
                                cmd.StartInfo.RedirectStandardInput = true;
                                cmd.StartInfo.RedirectStandardOutput = true;
                                cmd.StartInfo.CreateNoWindow = true;
                                cmd.StartInfo.UseShellExecute = false;
                                cmd.Start();
                                //cmd.StandardOutput.;
                                cmd.StandardInput.WriteLine(strCmdPass);
                                
                                cmd.StandardInput.WriteLine(strCmdMount);

                                System.Threading.Thread.Sleep(1000);
                                //await Task.Delay(2 * 1000);

                                var availableDrivesWithRclone = DriveInfo.GetDrives().Where(d => d.IsReady);
                                if (availableDrives.Count() == availableDrivesWithRclone.Count())
                                {
                                    PlayniteApi.Dialogs.ShowErrorMessage("Failed to mount rclone, check your config.","PlayX");
                                }

                                //cmd.StandardInput.Close();
                                //md.WaitForExit();
                                //System.Diagnostics.Process.Start("CMD.exe", strCmdPass);
                                //System.Diagnostics.Process.Start("CMD.exe", strCmdMount);

                            }
                            else
                            {
                                PlayniteApi.Dialogs.ShowErrorMessage("Remote name and path is not set, please check your config", "PlayX");
                            }
                        }
                        else
                        {
                            PlayniteApi.Dialogs.ShowErrorMessage("Can't find rclone.exe, please check your config", "PlayX");
                        }
                    }
                }
            }
        }

        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            //process.Kill();
            foreach (var node in Process.GetProcessesByName("rclone"))
            {
                node.Kill();
            }
        }

        public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
        {
            // Add code to be executed when library is updated.
            
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new cloudImportSettingsView();
        }
        /*
        public override IEnumerable<TopPanelItem> GetTopPanelItems()
            {
            //System.Threading.Thread.Sleep(5000);
            //await Task.Delay(5000);
            if (Process.GetProcessesByName("rclone").Length <= 0)
                {
                    ico = char.ConvertFromUtf32(0xea6e);
                
                }
                else
                {
                    ico = char.ConvertFromUtf32(0xee51);
                }
                return new List<TopPanelItem>()
                {
                    new TopPanelItem
                    {
                        Icon = new TextBlock
                        {
                            Text = ico,
                            FontSize = 22,
                            FontFamily = ResourceProvider.GetResource("FontIcoFont") as FontFamily
                        },
                        Title = "PlayPass Status",
                        Activated = () =>
                        {
                            //GameInstaller();
                        }
                    }
                };
        }*/
    }
}