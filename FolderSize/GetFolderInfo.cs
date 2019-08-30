using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;

namespace FolderSize
{
    class GetFolderInfo
    {
        static StringCollection log = new StringCollection();
        string[] drives = System.Environment.GetLogicalDrives();
        public void RecurciveFileSearch(List<SizeInfo> sizeList, string aPath)
        {
            if (sizeList == null) return;
            //foreach (string dr in drives)
            //{
                //System.IO.DriveInfo di = new System.IO.DriveInfo(dr);

                // Here we skip the drive if it is not ready to be read. This
                // is not necessarily the appropriate action in all scenarios.
                //if (!di.IsReady)
                //{
                    //Console.WriteLine("The drive {0} could not be read", di.Name);
                    //continue;
                //}
               // System.IO.DirectoryInfo rootDir = di.RootDirectory;
                //WalkDirectoryTree(rootDir, sizeList);
                WalkDirectoryTree(new DirectoryInfo(aPath), sizeList);
            //}

        }

        static void WalkDirectoryTree(DirectoryInfo root, List<SizeInfo> sizeList)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                long Size = 0;
                foreach (System.IO.FileInfo fi in files)
                {
                    Size += fi.Length;
                }

                sizeList.Add(new SizeInfo(root.FullName, Size));
                subDirs = root.GetDirectories();
                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    try
                    {
                        FileSystemInfo[] sysInfo = dirInfo.GetFileSystemInfos();
                        WalkDirectoryTree(dirInfo, sizeList);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        log.Add(e.Message);
                    }
                    catch (Exception ex)
                    {
                        log.Add(ex.Message);
                    }
                }

            }

        }

    }

}

