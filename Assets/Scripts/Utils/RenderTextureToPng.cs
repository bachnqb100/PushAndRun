
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.Skin;
using Sirenix.OdinInspector;
using UnityEngine;

public class RenderTextureToPng : MonoBehaviour
{
    [SerializeField] private Camera currentCamera;
    [SerializeField] private Vector2Int imageResolution;
    [SerializeField] private string saveName;
    [SerializeField, FolderPath] private string savePath;
    
    [Header("All Objects")]
    [SerializeField] private List<GameObject> allObjects;


    [Button]
    void TakePhotoChangeText(SkinManager skinManager)
    {
        
    }

    [Button]
    void SaveAllToPng()
    {
        foreach (var e in allObjects)
        {
            e.SetActive(false);
        }
        
        foreach (var obj in allObjects)
        {
            foreach (var e in allObjects)
            {
                e.SetActive(false);
            }

            obj.SetActive(true);
            SaveToPng(obj.name);
            obj.SetActive(false);
        }
    }
    
    
    [Button]
    void SaveToPng()
    {
        RenderTexture renderTexture = new RenderTexture(imageResolution.x, imageResolution.y, 24 ,RenderTextureFormat.ARGB32);
        currentCamera.targetTexture = renderTexture;
        currentCamera.Render();
        currentCamera.targetTexture = null;
        Texture2D texture = ToTexture2D(renderTexture);
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        if(!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }
        File.WriteAllBytes(savePath + "/" + saveName + ".png", bytes);
    }
    
    public void SaveToPng(Camera currentCamera, Vector2Int imageResolution, string name, string savePath)
    {
        RenderTexture renderTexture = new RenderTexture(imageResolution.x, imageResolution.y, 24 ,RenderTextureFormat.ARGB32);
        currentCamera.targetTexture = renderTexture;
        currentCamera.Render();
        currentCamera.targetTexture = null;
        Texture2D texture = ToTexture2D(renderTexture);
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        if(!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }
        File.WriteAllBytes(savePath + "/" + name + ".png", bytes);
    }
    
    public void SaveToPng(string name)
    {
        RenderTexture renderTexture = new RenderTexture(imageResolution.x, imageResolution.y, 24 ,RenderTextureFormat.ARGB32);
        currentCamera.targetTexture = renderTexture;
        currentCamera.Render();
        currentCamera.targetTexture = null;
        Texture2D texture = ToTexture2D(renderTexture);
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        if(!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }
        File.WriteAllBytes(savePath + "/" + name + ".png", bytes);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.ARGB32, false);
        var oldRT = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        
        tex.Apply();

        RenderTexture.active = oldRT;
        return tex;
    }
}