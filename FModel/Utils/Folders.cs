﻿using FModel.Logger;
using FModel.Windows.CustomNotifier;
using System;
using System.Diagnostics;
using System.IO;

namespace FModel.Utils
{
    static class Folders
    {
        public static void LoadFolders()
        {
            FixNoOutputPath();
            CreateDefaultSubFolders();
        }

        public static void SetGameName(string pakPath)
        {
            int index = pakPath.LastIndexOf("\\Content\\Paks");
            if (index > -1)
            {
                string p = pakPath.Substring(0, index);
                string game = p.Substring(p.LastIndexOf("\\") + 1);
                Globals.Game = game switch
                {
                    "FortniteGame" => EGame.Fortnite,
                    "ShooterGame" => EGame.Valorant,
                    _ => EGame.Unknown,
                };
            }
        }

        public static string GetGameName() => GetGameName(Globals.Game);
        public static string GetGameName(EGame game) =>
            game switch
            {
                EGame.Unknown => string.Empty,
                EGame.Fortnite => "FortniteGame",
                EGame.Valorant => "ShooterGame",
                _ => string.Empty,
            };

    /// <summary>
    /// if OutputPath is empty the .Exe directory will be OutputPath
    /// </summary>
    private static void FixNoOutputPath()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.OutputPath))
            {
                string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Output";
                Properties.Settings.Default.OutputPath = path;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Delete outdated files/folders and create new folders
        /// </summary>
        private static void CreateDefaultSubFolders()
        {
            //3.0-
            if (File.Exists(Properties.Settings.Default.OutputPath + "\\AESManager.xml")) { File.Delete(Properties.Settings.Default.OutputPath + "\\AESManager.xml"); }
            if (File.Exists(Properties.Settings.Default.OutputPath + "\\FAESManager.xml")) { File.Delete(Properties.Settings.Default.OutputPath + "\\FAESManager.xml"); }
            if (Directory.Exists(Properties.Settings.Default.OutputPath + "\\Backup\\")) { Directory.Delete(Properties.Settings.Default.OutputPath + "\\Backup\\", true); }
            if (Directory.Exists(Properties.Settings.Default.OutputPath + "\\Extracted\\")) { Directory.Delete(Properties.Settings.Default.OutputPath + "\\Extracted\\", true); }
            if (Directory.Exists(Properties.Settings.Default.OutputPath + "\\Saved_JSON\\")) { Directory.Delete(Properties.Settings.Default.OutputPath + "\\Saved_JSON\\", true); }

            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FModel\\"); // settings
            Directory.CreateDirectory(Properties.Settings.Default.OutputPath + "\\Backups\\");
            Directory.CreateDirectory(Properties.Settings.Default.OutputPath + "\\Exports\\");
            Directory.CreateDirectory(Properties.Settings.Default.OutputPath + "\\Fonts\\");
            Directory.CreateDirectory(Properties.Settings.Default.OutputPath + "\\Icons\\");
            Directory.CreateDirectory(Properties.Settings.Default.OutputPath + "\\JSONs\\");
            Directory.CreateDirectory(Properties.Settings.Default.OutputPath + "\\Sounds\\");
            Directory.CreateDirectory(Properties.Settings.Default.OutputPath + "\\Logs\\");
        }

        public static void CheckWatermarks()
        {
            bool bSave = false;
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.IconWatermarkPath) &&
                !File.Exists(Properties.Settings.Default.IconWatermarkPath))
            {
                Properties.Settings.Default.IconWatermarkPath = string.Empty;
                Properties.Settings.Default.UseIconWatermark = false;
                bSave = true;

                Globals.gNotifier.ShowCustomMessage(Properties.Resources.IconWatermark, Properties.Resources.IconWatermarkNotFound, "/FModel;component/Resources/file-image.ico");
                DebugHelper.WriteLine("{0} {1} {2}", "[FModel]", "[Watermarks]", "IconWatermarkPath not found, option disabled");
            }

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.ChallengeBannerPath) &&
                !File.Exists(Properties.Settings.Default.ChallengeBannerPath))
            {
                Properties.Settings.Default.ChallengeBannerPath = string.Empty;
                Properties.Settings.Default.UseChallengeBanner = false;
                bSave = true;

                Globals.gNotifier.ShowCustomMessage(Properties.Resources.ChallengesCustomBanner, Properties.Resources.CustomBannerNotFound, "/FModel;component/Resources/file-image.ico");
                DebugHelper.WriteLine("{0} {1} {2}", "[FModel]", "[Watermarks]", "ChallengeBannerPath not found, option disabled");
            }

            if (bSave)
                Properties.Settings.Default.Save();
        }
    }
}
