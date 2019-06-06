using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace cmake
{
    public static class CMakeUtility
    {
        private static string ProjectPath
        {
            get { return Path.GetFullPath(Path.Combine(Application.dataPath, "..")); }
        }

        public static string SourcePath
        {
            get { return ProjectPath; }
        }

        public static string BuildPath
        {
            get { return Path.Combine(ProjectPath, "Library/cmake-build"); }
        }

        private static Dictionary<string, string> m_unityDefs;
        private static Dictionary<string, string> UnityDefs()
        {
            if(m_unityDefs == null)
            {
                m_unityDefs = new Dictionary<string, string>();
                m_unityDefs.Add("UNITY_VERSION_STRING", Application.unityVersion);

                string[] v = Application.unityVersion.Split(new char[]{ '.' });

                m_unityDefs.Add("UNITY_VERSION_MAJOR", v[0]);
                m_unityDefs.Add("UNITY_VERSION_MINOR", v[1]);
                m_unityDefs.Add("UNITY_VERSION_PATCH", v[2]);

                m_unityDefs.Add("UNITY_PLUGIN_API_INCLUDE_DIRS", Path.Combine(EditorApplication.applicationContentsPath, "PluginAPI"));
            }

            return m_unityDefs;
        }

        public static void Configure()
        {
            var cmakeTool = new CMakeTool(GlobalSettings.ExecutablePath);
            var cp = cmakeTool.Configure(sourceFolder: ProjectPath, buildFolder: BuildPath, defs: UnityDefs());

            EditorUtility.DisplayProgressBar("CMake", "CMake: Configuring ", 1);

            try
            {
                if(cp.Start())
                {
                    while(!cp.HasExited)
                    {
                        string s = cp.StandardOutput.ReadToEnd();
                        if(s.Length > 0)
                        {
                            UnityEngine.Debug.Log(s);
                        }
                    }
                } else {
                    throw new BuildFailedException("Cannot start CMake");
                }
            } 
            catch(System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                throw new BuildFailedException(e);
            }

            EditorUtility.ClearProgressBar();


            if(cp.ExitCode != 0) {
                throw new BuildFailedException(cp.StandardError.ReadToEnd());
            }
        }

        public static void Build()
        {
            string projectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string sourcePath = projectPath;
            string buildPath = Path.Combine(projectPath, "Library/cmake-build");

            var cmakeTool = new CMakeTool(GlobalSettings.ExecutablePath);
            var cp = cmakeTool.Build(buildFolder: buildPath);

            EditorUtility.DisplayProgressBar("CMake", "CMake: Building", 1);

            try
            {
                if(cp.Start())
                {
                    while(!cp.HasExited)
                    {
                        string s = cp.StandardOutput.ReadToEnd();
                        if(s.Length > 0)
                        {
                            UnityEngine.Debug.Log(s);
                        }
                    }
                } else {
                    throw new BuildFailedException("Cannot start CMake");
                }
            } 
            catch(System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                throw new BuildFailedException(e);
            }

            EditorUtility.ClearProgressBar();

            if(cp.ExitCode != 0) {
                throw new BuildFailedException(cp.StandardError.ReadToEnd());
            }
        }

        public static void Install()
        {
            var cmakeTool = new CMakeTool(GlobalSettings.ExecutablePath);
            var cp = cmakeTool.Install(buildFolder: BuildPath);

            EditorUtility.DisplayProgressBar("CMake", "CMake: Installing", 1);

            try
            {
                if(cp.Start())
                {
                    while(!cp.HasExited)
                    {
                        string s = cp.StandardOutput.ReadToEnd();
                        if(s.Length > 0)
                        {
                            UnityEngine.Debug.Log(s);
                        }
                    }
                } else {
                    throw new BuildFailedException("Cannot start CMake");
                }
            } 
            catch(System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                throw new BuildFailedException(e);
            }

            EditorUtility.ClearProgressBar();

            if(cp.ExitCode != 0) {
                throw new BuildFailedException(cp.StandardError.ReadToEnd());
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void Clean()
        {
            // TODO
        }

        public static void FullClean()
        {
            Clean();
            //TODO
        }
    }
}
