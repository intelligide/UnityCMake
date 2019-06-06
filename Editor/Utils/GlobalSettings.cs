using UnityEditor;
using UnityEngine;

namespace cmake
{
    internal static class GlobalSettings
    {
        internal static class Keys
        {
            internal const string executablePath = "cmake.ExecutablePath";
            internal const string buildOnEditorLoad = "cmake.buildOnEditorLoad";
            internal const string buildOnScriptReload = "cmake.buildOnScriptReload";
            internal const string buildBeforePlayerCompilation = "cmake.buildBeforePlayerCompilation";   
        }

        static bool m_loaded = false;

        static string m_executablePath = 
        #if UNITY_EDITOR_WIN
        "cmake.exe";
        #else
        "cmake";
        #endif
        internal static string ExecutablePath
        {
            get { return m_executablePath; }
            set { TrySave(ref m_executablePath, value, Keys.executablePath); }
        }

        static bool m_buildOnEditorLoad = false;
        internal static bool BuildOnEditorLoad
        {
            get { return m_buildOnEditorLoad; }
            set { TrySave(ref m_buildOnEditorLoad, value, Keys.buildOnScriptReload); }
        }

        static bool m_buildOnScriptReload = false;
        internal static bool BuildOnScriptReload
        {
            get { return m_buildOnScriptReload; }
            set { TrySave(ref m_buildOnScriptReload, value, Keys.buildOnScriptReload); }
        }

        static bool m_buildBeforePlayerCompilation = true;
        internal static bool BuildBeforePlayerCompilation
        {
            get { return m_buildBeforePlayerCompilation; }
            set { TrySave(ref m_buildBeforePlayerCompilation, value, Keys.buildBeforePlayerCompilation); }
        }

        static GlobalSettings()
        {
            Load();
        }

        #if UNITY_2018_3_OR_NEWER
        [SettingsProvider]
        static SettingsProvider PreferenceGUI()
        {
            return new SettingsProvider("Preferences/CMake", SettingsScope.User)
            {
                guiHandler = searchContext => OpenGUI()
            };
        }
        #else
        [PreferenceItem("CMake")]
        static void PreferenceGUI()
        {
            OpenGUI();
        }
        #endif

        static void OpenGUI()
        {
            if (!m_loaded)
                Load();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            ExecutablePath = EditorGUILayout.TextField("CMake Executable", ExecutablePath);
            GUILayout.Button("Browse", GUILayout.Width(80), GUILayout.Height(14));
            GUILayout.Button("Download", GUILayout.Width(80), GUILayout.Height(14));

            EditorGUILayout.EndHorizontal();

            BuildOnEditorLoad = EditorGUILayout.Toggle("Build On Editor Load", BuildOnEditorLoad);
            BuildOnScriptReload = EditorGUILayout.Toggle("Build On Script Reload", BuildOnScriptReload);
            BuildBeforePlayerCompilation = EditorGUILayout.Toggle("Build On Script Reload", BuildBeforePlayerCompilation);
        }

        static void Load()
        {
            m_executablePath = EditorPrefs.GetString(Keys.executablePath, m_executablePath);
            m_buildOnEditorLoad = EditorPrefs.GetBool(Keys.buildOnEditorLoad, m_buildOnEditorLoad);
            m_buildOnScriptReload = EditorPrefs.GetBool(Keys.buildOnScriptReload, m_buildOnScriptReload);
            m_buildBeforePlayerCompilation = EditorPrefs.GetBool(Keys.buildBeforePlayerCompilation, m_buildBeforePlayerCompilation);
            m_loaded = true;
        }

        static void TrySave<T>(ref T field, T newValue, string key)
        {
            if (field.Equals(newValue))
                return;

            if (typeof(T) == typeof(float))
                EditorPrefs.SetFloat(key, (float)(object)newValue);
            else if (typeof(T) == typeof(int))
                EditorPrefs.SetInt(key, (int)(object)newValue);
            else if (typeof(T) == typeof(bool))
                EditorPrefs.SetBool(key, (bool)(object)newValue);
            else if (typeof(T) == typeof(string))
                EditorPrefs.SetString(key, (string)(object)newValue);

            field = newValue;
        }
    }
}
