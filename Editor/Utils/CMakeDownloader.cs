using System;
using System.IO;
using System.Linq;
using SharpCompress.Readers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

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
            UnityWebRequest www = UnityWebRequest.Get(fullUrl);

            UnityWebRequestAsyncOperation asyncOp = www.SendWebRequest();
            do
            {
                EditorUtility.DisplayProgressBar("CMake", "Downloading CMake...", asyncOp.progress);
            } 
            while (!asyncOp.isDone);

            if(www.isNetworkError || www.isHttpError) {
                Debug.LogError(www.error);
                EditorUtility.ClearProgressBar();
                return null;
            }
            
            string destdir = Path.Combine(EditorApplication.applicationContentsPath, "Tools/CMake");
            if (!Directory.Exists(destdir))
            {
                Directory.CreateDirectory(destdir);
            }

            using (Stream stream = new MemoryStream(www.downloadHandler.data))
            using (var reader = ReaderFactory.Open(stream))
            {
                while (reader.MoveToNextEntry())
                {
                    EditorUtility.DisplayProgressBar("CMake", "Uncompressing CMake...", asyncOp.progress);
                    if (!reader.Entry.IsDirectory)
                    {
                        using (var entryStream = reader.OpenEntryStream())
                        {                       
                            string file = Path.GetFileName(reader.Entry.Key);
                            string folder = Path.GetDirectoryName(reader.Entry.Key);
                            
                            {
                                var fragments = folder.Split(new char[]{ Path.DirectorySeparatorChar });
                                if(fragments.Length > 1)
                                {
                                    folder = String.Join(Path.DirectorySeparatorChar.ToString(), fragments.Skip(1).Take(fragments.Length - 1).ToArray());
                                }
                                else
                                {
                                    folder = "";
                                }
                            }
                            
                            string filedestdir = Path.Combine(destdir, folder);
                            if (!Directory.Exists(filedestdir))
                            {
                                Directory.CreateDirectory(filedestdir);
                            }
                            string destinationFileName = Path.Combine(filedestdir, file);

                            using (FileStream fs = File.OpenWrite(destinationFileName))
                            {
                                entryStream.CopyTo(fs);
                            }
                        }
                    }
                }
            }

            EditorUtility.ClearProgressBar();

            #if UNITY_EDITOR_WIN
            return Path.Combine(destdir, "bin/cmake.exe");
            #else
            return Path.Combine(destdir, "bin/cmake");
            #endif
        }
    }
}
