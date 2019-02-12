using System;
using System.Collections.Generic;
using System.Text;

namespace PlaylistCreator.Tool
{
    internal class MainAppSettings
    {
        public string BaseFolder { get; set; }
        public string SearchFolder { get; set; }
        public List<FolderSettings> Folders { get; set; } = new List<FolderSettings>();
    }
}
