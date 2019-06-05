using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Diagnostics;
using System.IO;

namespace cmake
{
    internal class BuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            string projectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string sourcePath = projectPath;
            string buildPath = Path.Combine(projectPath, "Library/cmake-build");

            var cmakeTool = new CMakeTool(GlobalSettings.ExecutablePath);
            var cp = cmakeTool.Configure(sourceFolder: projectPath, buildFolder: buildPath);

            EditorUtility.DisplayProgressBar("CMake", "Configuring CMake", 1);

            while(!cp.HasExited)
            {
                string s = cp.StandardOutput.ReadToEnd();
                if(s.Length > 0)
                {
                    UnityEngine.Debug.Log(s);
                }
            }

            if(cp.ExitCode != 0) {
                throw new BuildFailedException(cp.StandardError.ReadToEnd());
            }

            EditorUtility.DisplayProgressBar("CMake", "Building with CMake", 1);
            cmakeTool.Build();


            EditorUtility.DisplayProgressBar("CMake", "Refresh Asset Database", 1);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
