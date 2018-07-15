using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Screenshoter : MonoBehaviour
{
    [Serializable]
    public class ResolutionSettings
    {
        public string Name;
        public Vector2Int Resolution;
        public float NormalScale = 100f;
        public int[] Scales;
    }

    [SerializeField]
    private ResolutionSettings[] Resolutions;

    private Camera TargetCamera = null;

    private void Awake()
    {
        TargetCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            TakeGamePicture();
    }

    public void TakeCameraPicture()
    {
        foreach (var item in Resolutions)
            foreach (var scale in item.Scales)
                CameraSave(item, scale);
    }

    private void CameraSave(ResolutionSettings res, int scale)
    {
        var height = Mathf.CeilToInt(res.Resolution.y * scale / res.NormalScale);
        var width = Mathf.CeilToInt(res.Resolution.x * scale / res.NormalScale);

        var path = string.Format("{0}/{1}.png",
            Application.persistentDataPath,
            string.IsNullOrEmpty(res.Name) ? $"Capture_{height}x{width}" : $"{res.Name}{scale}");

        RenderTexture renderTexture = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32);
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        TargetCamera.targetTexture = renderTexture;
        TargetCamera.Render();

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        TargetCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);

        byte[] bytes = texture2D.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log($"Screenshot from camer succesfully saved at {path}");
    }

    public void TakeGamePicture(string Name = "Screenshot_")
    {
        var path = $"{Application.persistentDataPath}//{Name}{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_")}{Screen.width}x{Screen.height}.png";
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log($"Screenshot succesfully saved at {path}");
    }
}
