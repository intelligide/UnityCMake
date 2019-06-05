using UnityEditor;
using UnityEngine;

namespace cmake
{
    internal static class GlobalSettings
    {
        static class Keys
        {
            internal const string executablePath = "cmake.ExecutablePath";
        }

        static bool m_loaded = false;

        static string m_executablePath = "cmake";
        internal static string ExecutablePath
        {
            get { return m_executablePath; }
            set { TrySave(ref m_executablePath, value, Keys.executablePath); }
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
        }

        static void Load()
        {
            m_executablePath = EditorPrefs.GetString(Keys.executablePath, "cmake.exe");
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
