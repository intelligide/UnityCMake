using UnityEditor;

namespace cmake
{
    public static class EditorExtension
    {
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            if(EditorPrefs.GetBool(GlobalSettings.Keys.buildOnScriptReload, false))
            {
                CMakeUtility.Configure();
                CMakeUtility.Build();
            }
        }
    }
}
