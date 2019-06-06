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
            if(EditorPrefs.GetBool(GlobalSettings.Keys.buildBeforePlayerCompilation, true))
            {
                CMakeUtility.Configure();
                CMakeUtility.Build();
                CMakeUtility.Install();
            }
        }
    }
}
