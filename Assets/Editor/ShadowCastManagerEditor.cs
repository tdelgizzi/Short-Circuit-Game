using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShadowCasterManager))]
public class ShadowCastManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShadowCasterManager manager = (ShadowCasterManager)target;

        if (GUILayout.Button("Generate ShadowCast objects"))
        {
            manager.CreateShadowCasters();
        }
    }
}
