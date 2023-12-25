using System;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CameraManager
{
    public class CameraController : MonoBehaviour
    {
        #region Singleton

        private static CameraController _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        public static CameraController Instance => _instance;
        #endregion

        [SerializeField] private GameObject freeLookCamera;
        
        
        [Title("Fall Camera")]
        [SerializeField] private CinemachineVirtualCamera fallCamera;
        [SerializeField] private Transform fallCameraFollow;
        
        [Title("Victory Camera")]
        [SerializeField] private CinemachineVirtualCamera victoryCamera;
        
        [Title("MainScreen Camera")]
        [SerializeField] private CinemachineVirtualCamera mainScreenCamera;

        [Header("Shop Camera")] 
        [SerializeField] private CinemachineVirtualCamera shopCamera;
        
        [Header("Clothes Camera")]
        [SerializeField] private CinemachineVirtualCamera clothesCamera;

        [Button]
        public void EnableFallCamera()
        {
            fallCamera.enabled = true;
            fallCamera.Follow = null;
        }

        [Button]
        public void DisableFallCamera()
        {
            fallCamera.Follow = fallCameraFollow;
            fallCamera.enabled = false;
        }

        public void EnableVictoryCamera()
        {
            victoryCamera.enabled = true;
        }

        public void DisableVictoryCamera()
        {
            victoryCamera.enabled = false;
        }

        public void SetStatusCameraMain(bool enable)
        {
            mainScreenCamera.enabled = enable;
        }

        public void SetStatusCameraShop(bool enable)
        {
            shopCamera.enabled = enable;
        }

        public void SetStatusCameraClothes(bool enable)
        {
            clothesCamera.enabled = enable;
        }
    }
}