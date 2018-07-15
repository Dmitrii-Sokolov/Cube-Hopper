using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Screenshoter))]
[CanEditMultipleObjects]
public class ScreenshoterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Screenshoter myScript = (Screenshoter)target;
        if (GUILayout.Button("Take a camera capture set"))
        {
            myScript.TakeCameraPicture();
        }
        if (GUILayout.Button("Take a game screenshot"))
        {
            myScript.TakeGamePicture();
        }
    }
}
