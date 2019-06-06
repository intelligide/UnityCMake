using UnityEditor;

namespace cmake
{
    [InitializeOnLoad]
    public class EditorStartup {
        static EditorStartup()
        {
            if(EditorPrefs.GetBool(GlobalSettings.Keys.buildOnEditorLoad, false))
            {
                CMakeUtility.Configure();
                CMakeUtility.Build();
            }
        }
    }

}
