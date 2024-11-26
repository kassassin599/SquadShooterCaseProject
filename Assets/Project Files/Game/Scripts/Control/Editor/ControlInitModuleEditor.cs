using UnityEditor;

namespace Watermelon
{
    [CustomEditor(typeof(ControlInitModule))]
    public class ControlInitModuleEditor : InitModuleEditor
    {
        public override void OnCreated()
        {
            GamepadData gamepadData = EditorUtils.GetAsset<GamepadData>();
            if(gamepadData != null )
            {
                serializedObject.Update();
                serializedObject.FindProperty("gamepadData").objectReferenceValue = gamepadData;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
