using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace PlaylistCreator.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");

            var config = configurationBuilder.Build();

            var settings = new MainAppSettings();
            config.Bind(settings);


            if (Directory.Exists(settings.SearchFolder))
            {
                SearchRootFolder(settings);
            }
            else
            {
                Console.WriteLine("Error: Failed to find search folder");
            }

            Console.WriteLine();
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }

        private static void SearchRootFolder(MainAppSettings settings)
        {
            var subDirs = Directory.GetDirectories(settings.SearchFolder);

            foreach(var subDir in subDirs)
            {
                SearchFolder(settings, subDir);
            }
        }

        private static void SearchFolder(MainAppSettings settings, string subDir)
        {
            var subDirName = Path.GetFileName(subDir);
            var playlist = new Playlist();
            var settingsFolder = settings.Folders.FirstOrDefault(f => f.FolderName == subDirName);

            if (settingsFolder == null)
                return;

            var playlistName = settingsFolder.PlaylistName ?? $"{subDirName}";
            var emulatorLocation = String.IsNullOrEmpty(settingsFolder.EmulatorLocation) ? "DETECT" : settingsFolder.EmulatorLocation;
            var emulatorName = String.IsNullOrEmpty(settingsFolder.EmulatorName) ? "DETECT" : settingsFolder.EmulatorName;

            var files = Directory.GetFiles(subDir);
            foreach(var file in files)
            {
                var fileName = Path.GetFileName(file);
                var romName = fileName;
                var extension = Path.GetExtension(file);
                if (extension.Equals(".lpl") || extension.Equals(".bin"))
                    continue;

                if (extension.Equals(".zip"))
                {
                    using (ZipArchive archive = ZipFile.OpenRead(file))
                    {
                        var zipFile = archive.Entries.FirstOrDefault();
                        fileName = $"{fileName}#{zipFile.Name}";
                    }
                }

                var item = new PlaylistItem()
                {
                    ROMLocation = $"{settings.BaseFolder}/{subDirName}/{fileName}",
                    ROMName = Path.GetFileNameWithoutExtension(romName).Replace("_", " "),
                    EmulatorLocation = emulatorLocation,
                    EmulatorName = emulatorName,
                    PlaylistName = $"{playlistName}.lpl"
                };

                if (!String.IsNullOrEmpty(settingsFolder.EmulatorName))
                {
                    item.EmulatorName = settingsFolder.EmulatorName;
                }

                playlist.Items.Add(item);
            }

            playlist.CreatePlayList(subDir);
        }
    }
}
