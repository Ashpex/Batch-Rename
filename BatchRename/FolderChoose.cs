using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class FolderChoose : INotifyPropertyChanged
    {
        public string Foldername { get; set; }
        public string PreviewFolder { get; set; }
        public string Newfolder { get; set; }
        public string Path { get; set; }
        public string Error { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FolderChoose Clone()
        {
            return new FolderChoose()
            {
                Foldername = this.Foldername,
                PreviewFolder = this.PreviewFolder,
                Newfolder = this.Newfolder,
                Path = this.Path,
                Error = this.Error
            };
        }
    }
}
