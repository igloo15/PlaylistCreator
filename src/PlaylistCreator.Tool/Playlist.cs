using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlaylistCreator.Tool
{
    internal class Playlist
    {
        public List<PlaylistItem> Items { get; set; } = new List<PlaylistItem>();

        public void CreatePlayList(string folder)
        {
            string playlistName = "";
            StringBuilder sb = new StringBuilder();

            foreach(var item in Items)
            {
                sb
                    .AppendLine(item.ROMLocation)
                    .AppendLine(item.ROMName)
                    .AppendLine(item.EmulatorLocation)
                    .AppendLine(item.EmulatorName)
                    .AppendLine(item.CRC)
                    .AppendLine(item.PlaylistName);

                playlistName = item.PlaylistName;
            }

            File.WriteAllText(Path.Combine(folder, playlistName), sb.ToString());
        }
    }

    internal class PlaylistItem
    {
        public string ROMLocation { get; set; }
        public string ROMName { get; set; }
        public string EmulatorLocation { get; set; }
        public string EmulatorName { get; set; } = "DETECT";
        public string CRC { get; set; } = "DETECT";
        public string PlaylistName { get; set; }
    }

}
