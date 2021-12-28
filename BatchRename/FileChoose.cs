using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class FileChoose : INotifyPropertyChanged
    {
        public string Filename { get; set; }
        public string PreviewFile { get; set; }
        public string Newfilename { get; set; }
        public string Path { get; set; }
        public string Error { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FileChoose Clone()
        {
            return new FileChoose()
            {
                Filename = this.Filename,
                PreviewFile = this.PreviewFile,
                Newfilename = this.Newfilename,
                Path = this.Path,
                Error = this.Error
            };
        }

    }
}
