using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSize
{
    class SizeInfo
    {
        public string _path {get; set;}
        public long size {get; set;}
        public SizeInfo(string path, long size)
        {
            this._path = path;
            this.size = size;
        }

    }
}
