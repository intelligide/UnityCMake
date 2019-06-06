using System;
using System.IO;
using Ionic.Zip;

namespace cmake
{
    internal static class CMakeDownloader
    {
        internal const string DefaultVersion = "3.14.5";

        #if UNITY_EDITOR_WIN
        internal const string Url = "https://github.com/Kitware/CMake/releases/download/v{0}/cmake-{0}-win64-x64.zip";
        #elif UNITY_EDITOR_OSX
        internal const string Url = "https://github.com/Kitware/CMake/releases/download/v{0}/cmake-{0}-Darwin-x86_64.tar.gz";
        #elif UNITY_EDITOR_LINUX
        internal const string Url = "https://github.com/Kitware/CMake/releases/download/v{0}/cmake-{0}-Linux-x86_64.tar.gz";
        #endif

        public static string Download(string version = null)
        {
            if(version == null)
            {
                version = DefaultVersion;
            }

            string fullUrl = String.Format(Url, version);

            // Unzip(zipPath, extractPath);

            return "";
        }

        public static void Unzip(string zipFilePath, string location)
        {
            Directory.CreateDirectory(location);
            
            using(ZipFile zip = ZipFile.Read(zipFilePath)) 
            {
                zip.ExtractAll(location, ExtractExistingFileAction.OverwriteSilently);
            }

        }
    }
}
