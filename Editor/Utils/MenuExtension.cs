using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Diagnostics;
using System.IO;

namespace cmake
{
    internal static class MenuExtension
    {
        [MenuItem("Tools/CMake/Configure")]
        public static void Configure()
        {
            CMakeUtility.Configure();
        }

        [MenuItem("Tools/CMake/Build")]
        public static void Build()
        {
            CMakeUtility.Build();
        }

        [MenuItem("Tools/CMake/Install")]
        public static void Install()
        {
            CMakeUtility.Install();
        }

        [MenuItem("Tools/CMake/Clean (Test)")]
        public static void Clean()
        {
            CMakeUtility.Clean();
        }

        [MenuItem("Tools/CMake/Full Clean (Test)")]
        public static void FullClean()
        {
            CMakeUtility.FullClean();
        }
    }
}
