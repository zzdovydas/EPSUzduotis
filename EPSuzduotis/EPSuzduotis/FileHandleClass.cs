using System;
using System.Collections.Generic;
using System.Text;

namespace EPSuzduotis
{
    class FileHandleClass
    {

        private string path;

        public FileHandleClass(string _path)
        {
            path = _path;
        }

        public bool FileExists()
        {
            bool exists = false;

            if (System.IO.File.Exists(path))
                exists = true;

            return exists;
        }

        public byte[] GetFileBytes()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return bytes;
        }


    }
}
