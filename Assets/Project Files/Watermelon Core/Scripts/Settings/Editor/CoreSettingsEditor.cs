using UnityEditor;

namespace Watermelon
{
    [CustomEditor(typeof(CoreSettings))]
    public class CoreSettingsEditor : Editor
    {
        private CoreSettings coreSettings;

        private void OnEnable()
        {
            coreSettings = (CoreSettings)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                if (coreSettings != null)
                {
                    CoreEditor.ApplySettings(coreSettings);
                }
            }
        }

        [MenuItem("Window/Watermelon Core/Core Settings", priority = 900)]
        private static void SelectSettings()
        {
            CoreSettings coreSettings = EditorUtils.GetAsset<CoreSettings>();
            if(coreSettings != null)
            {
                Selection.activeObject = coreSettings;

                EditorGUIUtility.PingObject(coreSettings);
            }
        }
    }
}