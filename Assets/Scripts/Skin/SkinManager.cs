using System;
using System.IO;
using DefaultNamespace.Configs.Skin;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Skin
{
    public class SkinManager : MonoBehaviour
    {
        #region Singleton

        private static SkinManager _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public static SkinManager Instance => _instance;

        #endregion

        [SerializeField]
        private SerializedDictionary<ClothesColorType, Color> colorMap =
            new SerializedDictionary<ClothesColorType, Color>();

        [Space] 
        [SerializeField] 
        private SerializedDictionary<ClothesType, Material> materialClothesMap =
            new SerializedDictionary<ClothesType, Material>();

        public void SetColorPlayer()
        {
            foreach (var item in GameManager.Instance.GameData.userData.clothesColorMap)
            {
                materialClothesMap[item.Key].color = colorMap[item.Value];
            }
        }

        [Button]
        public void UpdateColorClothes(ClothesType clothesType, ClothesColorType colorType)
        {
            GameManager.Instance.GameData.userData.clothesColorMap[clothesType] = colorType;

            materialClothesMap[clothesType].color = colorMap[colorType];
        }

        [Button]
        public void ChangeColorPhoto(ClothesType clothesType, ClothesColorType colorType)
        {
            materialClothesMap[clothesType].color = colorMap[colorType];
        }


/*
#if UNITY_EDITOR 
        [Header("Photo")]
        [SerializeField] private Camera currentCamera;
        [SerializeField] private Vector2Int imageResolution;
        [SerializeField, FolderPath] private string savePath;
        [Button]
        public void SPTakePhoto(ClothesType type)
        {
            foreach (var color in colorMap)
            {
                string name = type.ToString() + " " + color.Key.ToString();

                ChangeColorPhoto(type, color.Key);

                SaveToPng(name);
            }
        }
        
        void SaveToPng(string saveName)
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
#endif
*/
        

        private void Awake()
        {
            InitSingleton();
        }
    }
}

[Serializable]
public class ClothesData
{
    public ClothesType clothesType;
    public ClothesColorType clothesColorType;
}


public enum ClothesType
{
    Boot,
    Gloves,
    Glass1,
    Glass2,
    Glass3,
    Robe1,
    Robe2,
    Robe3,
}


public enum ClothesColorType
{
    Black,
    Red,
    Green,
    Blue,
    Orange,
    Violet,
    Aqua,
    Gray,
    Magenta,
    Purple,
    Yellow,
}